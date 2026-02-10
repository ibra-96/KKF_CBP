using System;
using System.Configuration;
using System.Linq;
using System.Transactions;
using System.Web.Mvc;
using AlphaPeople.Core;
using AlphaPeople.Repository;
using AlfaPeople.KingKhalidFoundation.Data;
using AlfaPeople.KingKhalidFoundation.Data.Model;
using AlfaPeople.KingKhaledFoundation.Admin.Web.Models;
using System.Data.Entity;
using AlfaPeople.KingKhaledFoundation.Admin.Web.Helper;

namespace AlfaPeople.KingKhaledFoundation.Admin.Web.Controllers
{
    [Authorize(Roles = "Admin, CB Manager, CB Supervisor, CB Analyst")]
    public class ProjectImpactEvaluationController : BaseController
    {
        #region Members&Fields
        private readonly IRepository repository;
        private readonly CommonHelper helper;
        private readonly string Logpath = ConfigurationManager.AppSettings["Log"].ToString();
        private readonly DateTime dateNow = DateTime.Now.Date;
        private readonly string portalBaseUrl = ConfigurationManager.AppSettings["PortalBaseUrl"]?.ToString();
        private static readonly Guid BaselineTransTypeId = Guid.Parse("2655D2B1-90D2-4C17-B9E7-A0448CD8CB67");

        private static readonly Guid PostImpactTransTypeId = Guid.Parse("2655D2B1-90D2-4C17-B9E7-A0448CD84444");
        #endregion

        #region Ctor
        public ProjectImpactEvaluationController()
        {
            repository = new Repository(new KingkhaledFoundationDB());
            helper = new CommonHelper();
        }
        #endregion

        #region Actions

        // GET: ProjectImpactEvaluation
        [HttpGet]
        public ActionResult Index()
        {
            //var list = repository.GetQuery<ProjectImpactEvaluation>()
            //                     .Where(t => !t.IncubationWorkshop.IsDeleted)
            //                     .ToList();

            //return View(list);
            var list = repository.GetQuery<ProjectImpactEvaluation>()
                   .Where(t => !t.IncubationWorkshop.IsDeleted)
                   .Include("ProjectImpactEvaluationRequests.FrontendUser.AspNetUser")
                   .Include("ProjectImpactEvaluationRequests.FrontendUser.CorporateApplicationForms")
                   .ToList();

            return View(list);
        }
        [HttpGet]
        public ActionResult ViewSubmittedForm(Guid evaluationId, Guid frontendUserId)
        {
            var eval = repository.GetByKey<ProjectImpactEvaluation>(evaluationId);
            if (eval == null)
            {
                TempData["PopupError"] = "لا يوجد تقييم أثر بهذا الرقم.";
                return RedirectToAction("Index");
            }

            var workshopId = eval.IncubationWorkshopID;

          
            var transType = repository.GetQuery<IncubationWorkshopBLTransactionsType>()
                .Include("IncubationWorkshopBLTrans.Options")
                .Include("IncubationWorkshopBLTrans.IncubationWSControlsType.IncubationWorkshopControl")
                .Include("IncubationWorkshopBLTrans.IncubationWSBLTransactionsValue.OptionValues")
                .Include("IncubationWorkshopBLTrans.IncubationWSBLTransactionsValue.IncubationWSBLTransValueAttachment")
                .FirstOrDefault(t =>
                    t.TransTypeID == PostImpactTransTypeId &&
                    t.IncubationWorkshopBLTrans.Any(trans => trans.IncubationWorkshopID == workshopId));

            if (transType == null)
            {
                TempData["PopupError"] = "لا يوجد نموذج Post Impact لهذه الورشة.";
                return RedirectToAction("Details", new { id = evaluationId });
            }

            // اجلب قيم الجمعية فقط
            var values = repository.GetQuery<IncubationWorkshopBLTransactionsValue>()
                .Include("OptionValues")
                .Include("IncubationWSBLTransValueAttachment")
                .Where(v =>
                    v.FrontendUserID == frontendUserId &&
                    v.IncubationWorkshopBLTrans.IncubationWorkshopID == workshopId &&
                    v.IncubationWorkshopBLTrans.TransTypeID == PostImpactTransTypeId)
                .ToList();
            if (!values.Any())
            {
                TempData["PopupError"] = "لا توجد بيانات مُعبّأة لهذه الجمعية.";
                return RedirectToAction("Details", new { id = evaluationId });
            }
            ViewBag.Values = values;
            ViewBag.WorkshopId = workshopId;
            ViewBag.FrontendUserId = frontendUserId;

            return View(transType); // View read-only
        }
        [HttpGet]
        public ActionResult PostImpactDetails(Guid evaluationId, Guid frontendUserId)
        {
            ViewBag.lang = CultureHelper.CurrentCulture;

            // 1) جلب الطلب لمعرفة الورشة واسم الجمعية وتاريخ التعبئة FilledOn
            var req = repository.GetQuery<ProjectImpactEvaluationRequest>()
                .Include("ProjectImpactEvaluation.IncubationWorkshop")
                .Include("FrontendUser.AspNetUser")
                .Include("FrontendUser.CorporateApplicationForms")
                .FirstOrDefault(r => r.ProjectImpactEvaluationId == evaluationId
                                  && r.FrontendUserId == frontendUserId);

            if (req == null)
            {
                TempData["PopupError"] = "لا يوجد طلب (Request) لهذه الجمعية ضمن هذا التقييم.";
                return RedirectToAction("Details", new { id = evaluationId });
               
            }

            var workshopId = req.ProjectImpactEvaluation.IncubationWorkshopID;
            var workshopName = req.ProjectImpactEvaluation?.IncubationWorkshop?.Name;

            var corpName =
                (req.FrontendUser?.CorporateApplicationForms != null && req.FrontendUser.CorporateApplicationForms.Any())
                    ? req.FrontendUser.CorporateApplicationForms.FirstOrDefault().Name
                    : req.FrontendUser?.AspNetUser?.UserName;

            // 2) جلب تعريف نموذج Post Impact + الأسئلة + الخيارات + نوع الكنترول
            //var transType = repository.GetQuery<IncubationWorkshopBLTransactionsType>()
            //    .Include(t => t.IncubationWorkshopBLTrans.Select(x => x.Options))
            //    .Include(t => t.IncubationWorkshopBLTrans.Select(x => x.IncubationWSControlsType.IncubationWorkshopControl))
            //    .FirstOrDefault(t =>
            //        t.TransTypeID == PostImpactTransTypeId &&
            //        t.IncubationWorkshopBLTrans.Any(tr => tr.IncubationWorkshopID == workshopId));
            // جلب TransType PostImpact
            var transType = repository.GetQuery<IncubationWorkshopBLTransactionsType>()
                .Include(t => t.IncubationWorkshopBLTrans.Select(trans => trans.Options))
                .Include(t => t.IncubationWorkshopBLTrans.Select(trans => trans.IncubationWSBLTransactionsValue))
                .Include(t => t.IncubationWorkshopBLTrans.Select(trans => trans.IncubationWSBLTransactionsValue.Select(val => val.IncubationWSBLTransValueAttachment)))
                .Include(t => t.IncubationWorkshopBLTrans.Select(trans => trans.IncubationWSControlsType))
                .FirstOrDefault(t =>
                    t.TransTypeID == PostImpactTransTypeId
                    && t.IncubationWorkshopBLTrans.Any(trans => trans.IncubationWorkshopID == workshopId));

            if (transType == null)
            {
                TempData["PopupError"] = "لا يوجد نموذج Post Impact لهذه الورشة.";
                return RedirectToAction("Details", new { id = evaluationId });
            }
            // 3) جلب قيم الجمعية فقط + OptionValues + Attachments
            var values = repository.GetQuery<IncubationWorkshopBLTransactionsValue>(v =>
                    v.FrontendUserID == frontendUserId &&
                    v.IncubationWorkshopBLTrans.IncubationWorkshopID == workshopId &&
                    v.IncubationWorkshopBLTrans.TransTypeID == PostImpactTransTypeId &&
                    v.IncubationWorkshopBLTrans.ViewList_Display == "true")
                .Include(v => v.IncubationWorkshopBLTrans)
                .Include(v => v.IncubationWorkshopBLTrans.Options)
                .Include(v => v.OptionValues)
                .Include(v => v.IncubationWSBLTransValueAttachment)
                .ToList();

            if (!values.Any())
            {
                TempData["PopupError"] = "لا توجد إجابات/قيم مسجلة لهذه الجمعية في نموذج قياس الأثر.";
                return RedirectToAction("Details", new { id = evaluationId });
            }

            // 4) ربط القيم داخل كل سؤال (نفس فكرة نموذجك القديم)
            foreach (var tr in transType.IncubationWorkshopBLTrans)
            {
                tr.IncubationWSBLTransactionsValue = values.Where(v => v.TransID == tr.TransID).ToList();
            }

            var vm = new IncubationWSPostImpactReadOnlyVM
            {
                EvaluationId = evaluationId,
                FrontendUserID = frontendUserId,
                IncubationWorkshopID = workshopId,
                WorkshopName = workshopName,
                CorporateName = corpName,
                FilledOn = req.FilledOn,
                LastSubmissionDate = values.Max(x => (DateTime?)x.SubmissionDate),
                IncubationWorkshopBLTransactionsType = transType,
                Values = values
            };

            return View(vm); // Views/ProjectImpactEvaluation/PostImpactDetails.cshtml
        }
        [HttpGet]
        public ActionResult Details(Guid id)
        {
            var eval = repository.GetQuery<ProjectImpactEvaluation>()
                .Include("IncubationWorkshop")
                .Include("ProjectImpactEvaluationRequests.FrontendUser.AspNetUser")
                .Include("ProjectImpactEvaluationRequests.FrontendUser.CorporateApplicationForms")
                .FirstOrDefault(x => x.ProjectImpactEvaluationId == id);

            if (eval == null) return HttpNotFound();

            return View(eval);
        }
      //  [HttpGet]
      //  public ActionResult Details1(Guid FUserID, Guid StatusID, DateTime SubmissionDate, Guid IncubationWorkshopID, string WorkshopName)
      //  {
      //      ViewBag.lang = CultureHelper.CurrentCulture;
      //      ViewBag.workshopID = IncubationWorkshopID;
      //      ViewBag.workshopName = WorkshopName;
      //      ViewBag.frontEnd = FUserID;
      //      // جلب حالة الطلب
      //      var IncILStatus = repository.GetByKey<IncubationWorkshopBLTransValStatus>(StatusID);

      //      // جلب القيم مع تضمين الخيارات والبيانات المرتبطة
      //      var IncIL = repository.GetQuery<IncubationWorkshopBLTransactionsValue>(v =>
      //      v.IncubationWorkshopBLTrans.ViewList_Display == "true" &&
      //    v.FrontendUserID == FUserID &&
      //    v.IncubationWorkshopBLTransValStatusID == StatusID &&
      //     DbFunctions.TruncateTime(v.SubmissionDate) == SubmissionDate.Date &&
      //          v.IncubationWorkshopBLTrans.IncubationWorkshopID == IncubationWorkshopID)
      //.Include(v => v.IncubationWorkshopBLTrans)
      //.Include(v => v.IncubationWorkshopBLTrans.Options)
      //.Include(v => v.OptionValues)
      //.Include(v => v.IncubationWorkshopBLTrans.IncubationWorkshopBLTransType)
      //.ToList();


      //      // التحقق من وجود القيم
      //      if (!IncIL.Any())
      //          return RedirectToAction("dashboard", new { Msg = "No records found." });

      //      // إعداد القائمة المنسدلة حسب اللغة والحالة
      //      if (IncILStatus.NameEN == "Pending" || IncILStatus.NameEN == "Baseline Application Form Updated")
      //      {
      //          ViewBag.LstDrop = new SelectList(
      //              repository.GetQuery<IncubationWorkshopBLTransValStatus>()
      //                        .Where(f => f.NameEN == "Update Baseline Application Form" || f.NameEN == "Rejected" || f.NameEN == "Accepted")
      //                        .ToList(),
      //              "IncubationWorkshopBLTransValStatusID",
      //              CultureHelper.CurrentCulture == 3 ? "NameAR" : "NameEN"
      //          );

      //      }
      //      else
      //      {
      //          ViewBag.LstDrop = new SelectList(
      //              repository.GetQuery<IncubationWorkshopBLTransValStatus>()
      //                        .Where(f => f.NameEN == "Update Baseline Application Form")
      //                        .ToList(),
      //              "IncubationWorkshopBLTransValStatusID",
      //              CultureHelper.CurrentCulture == 3 ? "NameAR" : "NameEN"
      //          );
      //      }

      //      var model = new IncubationWSBaselineVM()
      //      {
      //          FrontendUserID = FUserID,
      //          SubmissionDate = SubmissionDate,
      //          IncubationWSBLTransValStatusID = StatusID,
      //          Feadback = IncIL.FirstOrDefault()?.Feadback,
      //          IncubationWorkshopID = IncubationWorkshopID,
      //          //WorkshopID = WorkshopID, // إضافة WorkshopID
      //          WorkshopName = WorkshopName, // إضافة WorkshopName
      //          IncubationWorkshopBLTransactionsType = IncIL.FirstOrDefault()?.IncubationWorkshopBLTrans?.IncubationWorkshopBLTransType
      //      };

      //      // تحميل الخيارات المحددة
      //      foreach (var trans in model.IncubationWorkshopBLTransactionsType?.IncubationWorkshopBLTrans ?? new List<IncubationWorkshopBLTransactions>())
      //      {
      //          trans.IncubationWSBLTransactionsValue = IncIL.Where(v => v.TransID == trans.TransID).ToList();
      //      }

      //      return View(model);
      //  }


        // GET: ProjectImpactEvaluation/Create
        [HttpGet]
        public ActionResult Create()
        {
            FillWorkshops();

            return View(new ProjectImpactEvaluationVM
            {
                ProjectImpactEvaluation = new ProjectImpactEvaluation
                {
                    Deadline = DateTime.Today,
                    ReminderDate = null,
                    Notes = null,
                    IsFilled = false
                }
            });
        }

        // POST: ProjectImpactEvaluation/Create  (يرسل "بدء تعبئة" للجمعيات المختارة)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProjectImpactEvaluationVM model)
        {
            FillWorkshops();

            if (!ModelState.IsValid)
                return View(model);

            var entity = new ProjectImpactEvaluation
            {
                ProjectImpactEvaluationId = Guid.NewGuid(),
                IncubationWorkshopID = model.ProjectImpactEvaluation.IncubationWorkshopID,
                Deadline = model.ProjectImpactEvaluation.Deadline,
                ReminderDate = model.ProjectImpactEvaluation.ReminderDate,
                Notes = model.ProjectImpactEvaluation.Notes,
                IsFilled = false
            };
            // بعد إنشاء entity وقبل using(TransactionScope)
            if (model.FrontEndUsers != null && model.FrontEndUsers.Any())
            {
                var workshopId = entity.IncubationWorkshopID;

                // الجمعيات التي لديها طلب قياس أثر مسبقاً لنفس الورشة (بأي ProjectImpactEvaluation سابق)
                var duplicatedUserIds = repository.GetQuery<ProjectImpactEvaluationRequest>()
                    .Where(r =>
                        model.FrontEndUsers.Contains(r.FrontendUserId) &&
                        r.ProjectImpactEvaluation.IncubationWorkshopID == workshopId)
                    .Select(r => r.FrontendUserId)
                    .Distinct()
                    .ToList();

                if (duplicatedUserIds.Any())
                {
                   
                    var duplicatedNames = repository.GetQuery<FrontendUser>()
                        .Where(u => duplicatedUserIds.Contains(u.UserID))
                        .Select(u =>
                            (u.CorporateApplicationForms.Any()
                                ? u.CorporateApplicationForms.FirstOrDefault().Name
                                : u.AspNetUser.UserName))
                        .ToList();

                    ModelState.AddModelError("", "لا يمكن إضافة قياس أثر لنفس الجمعية في نفس الورشة. الجمعيات المكررة: "
                        + string.Join(" , ", duplicatedNames));

                    FillWorkshops();
                    return View(model);
                }
            }

            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    
                    if (model.FrontEndUsers != null)
                    {
                        foreach (var frontEndUserId in model.FrontEndUsers)
                        {
                            entity.ProjectImpactEvaluationRequests.Add(new ProjectImpactEvaluationRequest
                            {
                                ProjectImpactEvaluationRequestId = Guid.NewGuid(),
                                ProjectImpactEvaluationId = entity.ProjectImpactEvaluationId,
                                FrontendUserId = frontEndUserId,
                                Status = ProjectImpactEvaluationStatus.Pending
                            });

                            SendStartEmail(entity.IncubationWorkshopID, frontEndUserId, entity);
                        }
                    }

                    repository.Add(entity);
                    repository.UnitOfWork.SaveChanges();

                    scope.Complete();
                }
                catch (Exception)
                {
                    scope.Dispose();
                    return View(model);
                }
            }

            return RedirectToAction("Index");
        }

        // GET: ProjectImpactEvaluationGetUsers/Edit/{id}

        [HttpGet]
        public ActionResult Edit(Guid id)
        {
            var eval = repository.GetByKey<ProjectImpactEvaluation>(id);
            if (eval == null) return HttpNotFound();

            FillWorkshops();

            // اختيار الجمعيات المرتبطة بالورشة 
            ViewBag.Corprates = new MultiSelectList(
                repository.GetQuery<FrontendUser>(r =>
                    r.IsActive == true
                    && r.WorkshopProjectProposals.Any(p => p.IncubationWorkshopID == eval.IncubationWorkshopID && p.WorkshopProjectProposalStatu.NameEN == "Accepted")
                    && r.WorkshopPrivateInvitations.Any(i => i.IncubationWorkshopID == eval.IncubationWorkshopID && i.InvitationStatus == InvitationStatus.attend)
                ).ToList(),
                "UserID",
                "AspNetUser.UserName"
            );

            return View(new ProjectImpactEvaluationVM { ProjectImpactEvaluation = eval });
        }

        // POST: ProjectImpactEvaluation/Edit
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProjectImpactEvaluationVM model)
        {
            FillWorkshops();

            if (!ModelState.IsValid)
                return View(model);

            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    repository.Update(model.ProjectImpactEvaluation);
                    repository.UnitOfWork.SaveChanges();
                    scope.Complete();
                }
                catch
                {
                    scope.Dispose();
                    return View(model);
                }
            }

            return RedirectToAction("Index");
        }

        // 
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MarkFilled(Guid evaluationId, Guid frontendUserId)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    var eval = repository.GetByKey<ProjectImpactEvaluation>(evaluationId);
                    if (eval == null) return HttpNotFound();

                    var req = repository.Get<ProjectImpactEvaluationRequest>(r =>
                                r.ProjectImpactEvaluationId == evaluationId &&
                                r.FrontendUserId == frontendUserId)
                            .FirstOrDefault();

                    if (req == null)
                        return new HttpStatusCodeResult(400, "Request not found for this corporate.");

                    // لو هي بالفعل Filled لا نكرر الإرسال
                    if (req.Status != ProjectImpactEvaluationStatus.Filled)
                    {
                        req.Status = ProjectImpactEvaluationStatus.Filled;
                        req.FilledOn = DateTime.Now;
                        repository.Update(req);

                        // تحديث IsFilled العام إذا كل الجمعيات صارت Filled
                        eval.IsFilled = eval.ProjectImpactEvaluationRequests.All(x => x.Status == ProjectImpactEvaluationStatus.Filled);
                        repository.Update(eval);

                        repository.UnitOfWork.SaveChanges();

                    
                        SendCompletionEmail(eval.IncubationWorkshopID, frontendUserId, eval);
                    }

                    scope.Complete();
                }
                catch
                {
                    scope.Dispose();
                    return RedirectToAction("Edit", new { id = evaluationId });
                }
            }

            return RedirectToAction("Edit", new { id = evaluationId });
        }

        // GET: ProjectImpactEvaluation/GetUsers?Id=...
        //  [HttpGet]
        //public ActionResult GetUsers(Guid Id)
        //{
        //    var frontEndUsersQuery = repository.GetQuery<FrontendUser>(r =>
        //            r.IsActive == true
        //            && r.WorkshopProjectProposals.Any(p => p.IncubationWorkshopID == Id && p.WorkshopProjectProposalStatu.NameEN == "Accepted")
        //            && r.WorkshopPrivateInvitations.Any(i => i.IncubationWorkshopID == Id && i.InvitationStatus == InvitationStatus.attend))
        //        .ToList();

        //    return Json(new SelectList(frontEndUsersQuery, "UserID", "AspNetUser.UserName"),
        //                JsonRequestBehavior.AllowGet);
        //}
        [HttpGet]
        public ActionResult GetUsers(Guid Id)
        {
            // 1) الجمعيات التي لديها قياس أثر مسبقاً لنفس الورشة
            var alreadyHasImpact = repository.GetQuery<ProjectImpactEvaluationRequest>()
                .Where(r => r.ProjectImpactEvaluation.IncubationWorkshopID == Id)
                .Select(r => r.FrontendUserId)
                .Distinct()
                .ToList();

            // 2) جلب الجمعيات المؤهلة + استبعاد من لديهم قياس أثر مسبقاً
            var list = repository.GetQuery<FrontendUser>(r =>
                    r.IsActive == true
                    && r.WorkshopProjectProposals.Any(p =>
                        p.IncubationWorkshopID == Id &&
                        p.WorkshopProjectProposalStatu.NameEN == "Accepted")
                    && r.WorkshopPrivateInvitations.Any(i =>
                        i.IncubationWorkshopID == Id &&
                        i.InvitationStatus == InvitationStatus.attend)
                    && !alreadyHasImpact.Contains(r.UserID) //  منع ظهور المكررين
                )
                .ToList();

            // 3) عرض اسم الجمعية بدل اليوزر نيم
            var result = list.Select(u => new
            {
                Value = u.UserID,
                Text = (u.CorporateApplicationForms != null && u.CorporateApplicationForms.Any())
                    ? (u.CorporateApplicationForms.FirstOrDefault().Name + " - (" + u.AspNetUser.UserName + ")")
                    : u.AspNetUser.UserName
            }).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //[HttpGet]
        //public ActionResult GetUsers(Guid Id)
        //{
        //    var list = repository.GetQuery<FrontendUser>(r =>
        //            r.IsActive == true
        //            && r.WorkshopProjectProposals.Any(p => p.IncubationWorkshopID == Id && p.WorkshopProjectProposalStatu.NameEN == "Accepted")
        //            && r.WorkshopPrivateInvitations.Any(i => i.IncubationWorkshopID == Id && i.InvitationStatus == InvitationStatus.attend))
        //        .ToList();

        //    // عرض اسم الجمعية بدل اليوزر نيم
        //    var result = list.Select(u => new
        //    {
        //        Value = u.UserID,
        //        //    Text = (u.CorporateApplicationForms != null && u.CorporateApplicationForms.Any())
        //        //        ? u.CorporateApplicationForms.FirstOrDefault().Name
        //        //        : u.AspNetUser.UserName
        //        //}).ToList();
        //        Text = (u.CorporateApplicationForms != null && u.CorporateApplicationForms.Any())
        //            ? (u.CorporateApplicationForms.FirstOrDefault().Name + " - (" + u.AspNetUser.UserName + ")")
        //            : u.AspNetUser.UserName
        //           }).ToList();

        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}

        // GET: ProjectImpactEvaluation/SendReminders
        // تشغّلينها يوميًا عبر Scheduler/Hangfire
        [HttpGet]
        public ActionResult SendReminders()
        {
            var today = DateTime.Today;

            var dueEvals = repository.GetQuery<ProjectImpactEvaluation>(e =>
                    e.ReminderDate.HasValue
                    && e.ReminderDate.Value <= today
                    && !e.IncubationWorkshop.IsDeleted)
                .ToList();

            int sentCount = 0;

            foreach (var eval in dueEvals)
            {
                var pendingRequests = eval.ProjectImpactEvaluationRequests
                    .Where(r => r.Status == ProjectImpactEvaluationStatus.Pending && r.ReminderSentOn == null)
                    .ToList();

                if (!pendingRequests.Any())
                    continue;

                foreach (var req in pendingRequests)
                {
                    if (SendReminderEmail(eval.IncubationWorkshopID, req.FrontendUserId, eval))
                    {
                        req.ReminderSentOn = DateTime.Now;
                        repository.Update(req);
                        sentCount++;
                    }
                }

                repository.UnitOfWork.SaveChanges();
            }

            return Content($"Reminders sent: {sentCount}");
        }

        #endregion

        #region Helpers

        private void FillWorkshops()
        {
            ViewBag.WorkShop = new SelectList(
                repository.GetQuery<IncubationWorkshop>(r =>
                    r.EndDate < DateTime.Now && !r.IsDeleted && r.IncubationtWorkshopStatu.NameEN == "Active"),
                "IncubationWorkshopID",
                "Name"
            );
        }

        private string BuildFormLink(Guid evaluationId)
        {
           
            var baseUrl = (portalBaseUrl ?? "").TrimEnd('/');
            return $"{baseUrl}/ImpactEvaluation/Fill?evaluationId={evaluationId}";
        }

        private void SendStartEmail(Guid workshopId, Guid frontEndUserId, ProjectImpactEvaluation eval)
        {
            var user = repository.Get<FrontendUser>(f => f.UserID == frontEndUserId).FirstOrDefault();
            if (user?.AspNetUser?.Email == null) return;

            var workshop = repository.GetByKey<IncubationWorkshop>(workshopId);
            var fundingSource = workshop?.FundingSource;

            var dueDateStr = eval.Deadline.ToString("yyyy/MM/dd");
            var link = BuildFormLink(eval.ProjectImpactEvaluationId);

            var mailHelper = new MailHelper
            {
                ToEmail = user.AspNetUser.Email,
                Subject = "بدء تعبئة قياس الأثر",
                IsHtml = true,
                Body =
                    "السادة المنظمات غير الربحية الموقرين<br/>" +
                    "السلام عليكم ورحمة الله وبركاته،،<br/>" +
                    "تهديكم مؤسسة الملك خالد أطيب التحيات، ونشكركم على إتمام المشروع ..... " +
                    "ونحيطكم علماً ببدء فترة قياس الأثر وعليه نأمل منكم الدخول هنــا " +
                    $"<a href='{link}'>وتعبئة الاستمارة</a> " +
                    "وفق الوضع الحالي لكم لقياس مدى تحقق الإستفادة من المشروع .....<br/>" +
                    $"على أن لا يتجاوز تاريخ التعبئة للاستمارة {dueDateStr}<br/><br/>" +
                    "وتفضلوا بقبول فائق التحية والتقدير،،<br/>" +
                    "فريق برنامج بناء القدرات<br/>" +
                    "مؤسسة الملك خالد"
            };

            if (fundingSource != null && fundingSource.UseCustomThemes)
                mailHelper.Send($"?partner={fundingSource.Nickname}");
            else
                mailHelper.Send("");
        }

        private bool SendReminderEmail(Guid workshopId, Guid frontEndUserId, ProjectImpactEvaluation eval)
        {
            var user = repository.Get<FrontendUser>(f => f.UserID == frontEndUserId).FirstOrDefault();
            if (user?.AspNetUser?.Email == null) return false;

            var workshop = repository.GetByKey<IncubationWorkshop>(workshopId);
            var fundingSource = workshop?.FundingSource;

            var dueDateStr = eval.Deadline.ToString("yyyy/MM/dd");
            var link = BuildFormLink(eval.ProjectImpactEvaluationId);

            var mailHelper = new MailHelper
            {
                ToEmail = user.AspNetUser.Email,
                Subject = "تذكير بتعبئة قياس الأثر",
                IsHtml = true,
                Body =
                    "السادة المنظمات غير الربحية الموقرين<br/>" +
                    "السلام عليكم ورحمة الله وبركاته،،<br/>" +
                    "تهديكم مؤسسة الملك خالد أطيب التحيات، ونذكركم نأمل بالدخول هنــا " +
                    $"<a href='{link}'>وتعبئة الاستمارة</a> " +
                    "وفق الوضع الحالي لكم لقياس مدى تحقق الإستفادة من المشروع .....<br/>" +
                    $"على أن لا يتجاوز تاريخ التعبئة للاستمارة {dueDateStr}<br/><br/>" +
                    "وتفضلوا بقبول فائق التحية والتقدير،،<br/>" +
                    "فريق برنامج بناء القدرات<br/>" +
                    "مؤسسة الملك خالد"
            };

            if (fundingSource != null && fundingSource.UseCustomThemes)
                mailHelper.Send($"?partner={fundingSource.Nickname}");
            else
                mailHelper.Send("");

            return true;
        }

        private void SendCompletionEmail(Guid workshopId, Guid frontEndUserId, ProjectImpactEvaluation eval)
        {
            var user = repository.Get<FrontendUser>(f => f.UserID == frontEndUserId).FirstOrDefault();
            if (user?.AspNetUser?.Email == null) return;

            var workshop = repository.GetByKey<IncubationWorkshop>(workshopId);
            var fundingSource = workshop?.FundingSource;

            var mailHelper = new MailHelper
            {
                ToEmail = user.AspNetUser.Email,
                Subject = "شكرًا لإتمام تعبئة قياس الأثر",
                IsHtml = true,
                Body =
                    "السادة المنظمات غير الربحية الموقرين<br/>" +
                    "السلام عليكم ورحمة الله وبركاته،،<br/>" +
                    "تهديكم مؤسسة الملك خالد أطيب التحيات، ونشكركم على إتمام استمارة قياس الأثر لمشروع ..... " +
                    "متمنين لكم التميز والنجاح.<br/><br/>" +
                    "وتفضلوا بقبول فائق التحية والتقدير،،<br/>" +
                    "فريق برنامج بناء القدرات<br/>" +
                    "مؤسسة الملك خالد"
            };

            if (fundingSource != null && fundingSource.UseCustomThemes)
                mailHelper.Send($"?partner={fundingSource.Nickname}");
            else
                mailHelper.Send("");
        }

        #endregion
    }
}
