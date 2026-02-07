using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using AlphaPeople.Core;
using System.Transactions;
using System.Configuration;
using AlphaPeople.Repository;
using AlfaPeople.KingKhalidFoundation.Data;
using AlfaPeople.KingKhalidFoundation.Data.Model;
using AlfaPeople.KingKhaledFoundation.Admin.Web.Models;

namespace AlfaPeople.KingKhaledFoundation.Admin.Web.Controllers
{
    [Authorize(Roles = "Admin, CB Manager, CB Supervisor, CB Analyst")]
    public class FollowUpProjectPlanController : BaseController
    {
        #region Members&Fields
        private readonly IRepository repository;
        private readonly CommonHelper helper;
        private readonly string Logpath = ConfigurationManager.AppSettings["Log"].ToString();
        private readonly DateTime dateNow = DateTime.Now.Date;
        #endregion

        #region Ctor
        public FollowUpProjectPlanController()
        {
            repository = new Repository(new KingkhaledFoundationDB());
            helper = new CommonHelper();
        }
        #endregion

        #region Actions
        // GET: FollowUpProjectPlan
        [HttpGet]
        public ActionResult Index()
        {
            //2-25-2025
            var FollowUpProjectPlanList = repository.GetQuery<FollowUpProjectPlan>().Where(t=>!t.IncubationWorkshop.IsDeleted)
                .ToList();
            return View(FollowUpProjectPlanList);
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.WorkShop = new SelectList(repository.GetQuery<IncubationWorkshop>(r => r.EndDate < DateTime.Now &&!r.IsDeleted&& r.IncubationtWorkshopStatu.NameEN == "Active"), "IncubationWorkshopID", "Name");
            return View();
        }

        [HttpPost]
        public ActionResult Create(FollowUpProjectPlanVM model)
        {
            if (ModelState.IsValid)
            {
                var Entity = new FollowUpProjectPlan()
                {
                    FollowUpProjectPlanId = Guid.NewGuid(),
                    Deadline = model.FollowUpProjectPlan.Deadline,
                    IncubationWorkshopID = model.FollowUpProjectPlan.IncubationWorkshopID,
                    Name = model.FollowUpProjectPlan.Name,
                    Notes = model.FollowUpProjectPlan.Notes
                };
                if (model.FrontEndUsers != null)
                {
                    foreach (var FrontEndUser in model.FrontEndUsers)
                    {
                        Entity.FollowUpProjectPlanRequests.Add(
                            new FollowUpProjectPlanRequest()
                            {
                                FollowUpProjectPlanRequestId = Guid.NewGuid(),
                                FrontendUserId = FrontEndUser,
                                FollowUpProjectPlanStatus = FollowUpProjectPlanStatus.Pending
                            });
                    }
                }
                using (TransactionScope scope = new TransactionScope())
                {
                    try
                    {
                        if (model.files != null)
                        {
                            string FolderName = User.Identity.Name;
                            string path = Server.MapPath("~/Uploads/" + FolderName + "/");
                            if (!Directory.Exists(path))
                            {
                                Directory.CreateDirectory(path);
                            }
                            for (int i = 0; i < model.files.Count(); i++)
                            {
                                if (model.files[i] != null)
                                {
                                    model.files[i].SaveAs(path + Path.GetFileName(model.files[i].FileName));
                                    Entity.FollowUpProjectPlanAttachments.Add(new FollowUpProjectPlanAttachment()
                                    {
                                        AttachmentID = Guid.NewGuid(),
                                        Name = Path.GetFileName(model.files[i].FileName),
                                        ScreenName = "FollowUp Project Plan",
                                        Size = model.files[i].ContentLength.ToString(),
                                        URL = path + Path.GetFileName(model.files[i].FileName),
                                        Type = model.files[i].ContentType
                                    });
                                }
                            }
                            ViewBag.Message = "File uploaded successfully.";
                        }
                        if (model.FrontEndUsers != null)
                        {
                            foreach (var FrontEndUser in model.FrontEndUsers)
                            {
                                var _User = repository.Get<FrontendUser>(f => f.UserID == FrontEndUser).FirstOrDefault();
                                if (_User != null)
                                {
                                    if (_User.AspNetUser.Email != null)
                                    {
                                        MailHelper mailHelper = new MailHelper();
                                        mailHelper.ToEmail = _User.AspNetUser.Email;
                                        mailHelper.Subject = "طلب إرفاق خطط العمل ";
                                        mailHelper.IsHtml = true;
                                        mailHelper.Body = "المكرمين/ المنظمات غير الربحية                         الموقرين<br/>"
                                                                + "السلام عليكم ورحمة الله وبركاته، <br/>"
                                                                + "تهديكم مؤسسة الملك خالد أطيب التحيات، <br/>"
                                                                + $"ونرجو منكم التكرم بزيارة البوابة الإلكترونية لإرفاق خطة العمل الخاصة بـورشة: {model.FollowUpProjectPlan?.IncubationWorkshop?.Name}. <br/>"
                                                                + "شاكرين ومقدرين اهتمامكم وتعاونكم، <br/>"
                                                                + " برنامج بناء القدرات. <br/>";
                                        var fundingSource = repository.GetByKey<IncubationWorkshop>(Entity.IncubationWorkshopID).FundingSource;
                                        if (fundingSource.UseCustomThemes)
                                            mailHelper.Send($"?partner={fundingSource.Nickname}");
                                        else
                                            mailHelper.Send("");
                                    }
                                }
                            }
                        }
                        repository.Add(Entity);
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
            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(Guid Id)
        {
            var entity = repository.GetByKey<FollowUpProjectPlan>(Id);
            ViewBag.Title = App_GlobalResources.General.FollowUpProjectPlan + " " + entity.Name + " " + App_GlobalResources.General.Update;
            ViewBag.WorkShop = new SelectList(repository.GetQuery<IncubationWorkshop>(r => r.EndDate < DateTime.Now && r.IncubationtWorkshopStatu.NameEN == "Active"), "IncubationWorkshopID", "Name");
            ViewBag.Deadline = entity.Deadline.ToString("yyyy-MM-dd");
            ViewBag.Corprates = new MultiSelectList(repository.GetQuery<FrontendUser>(r => r.IsActive == true && r.WorkshopProjectProposals.Any(p => p.IncubationWorkshopID == entity.IncubationWorkshopID && p.WorkshopProjectProposalStatu.NameEN == "Accepted") && r.WorkshopPrivateInvitations.Any(i => i.IncubationWorkshopID == entity.IncubationWorkshopID && i.InvitationStatus == InvitationStatus.attend)).ToList(), "UserID", "CorporateApplicationForms.FirstOrDefault().Name", entity.FollowUpProjectPlanRequests.Select(r => r.FrontendUserId).ToArray());
            return View(new FollowUpProjectPlanVM() { FollowUpProjectPlan = entity });
        }

        [HttpPost]
        public ActionResult Edit(FollowUpProjectPlanVM model)
        {
            if (ModelState.IsValid)
            {
                repository.Update(model.FollowUpProjectPlan);
                using (TransactionScope scope = new TransactionScope())
                {
                    try
                    {
                        if (model.files != null)
                        {
                            string FolderName = User.Identity.Name;
                            string path = Server.MapPath("~/Uploads/" + FolderName + "/");
                            if (!Directory.Exists(path))
                            {
                                Directory.CreateDirectory(path);
                            }
                            for (int i = 0; i < model.files.Count(); i++)
                            {
                                if (model.files[i] != null)
                                {
                                    model.files[i].SaveAs(path + Path.GetFileName(model.files[i].FileName));
                                    FollowUpProjectPlanAttachment _attch = new FollowUpProjectPlanAttachment()
                                    {
                                        AttachmentID = Guid.NewGuid(),
                                        FollowUpProjectPlanId = model.FollowUpProjectPlan.FollowUpProjectPlanId,
                                        Name = Path.GetFileName(model.files[i].FileName),
                                        ScreenName = "Edit FollowUp Project Plan",
                                        Size = model.files[i].ContentLength.ToString(),
                                        URL = path + Path.GetFileName(model.files[i].FileName),
                                        Type = model.files[i].ContentType
                                    };
                                    repository.Add(_attch);
                                }
                            }
                            ViewBag.Message = "File uploaded successfully.";
                        }
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
            return View(model);
        }

        [HttpGet]
        public ActionResult GetUsers(Guid Id)
        {
            var FrontEndUsersQuery = repository.GetQuery<FrontendUser>(r => r.IsActive == true && r.WorkshopProjectProposals.Any(p => p.IncubationWorkshopID == Id && p.WorkshopProjectProposalStatu.NameEN == "Accepted") && r.WorkshopPrivateInvitations.Any(i => i.IncubationWorkshopID == Id && i.InvitationStatus == InvitationStatus.attend)).ToList();
            return Json(new SelectList(FrontEndUsersQuery, "UserID", "AspNetUser.UserName"), JsonRequestBehavior.AllowGet); //CorporateApplicationForms.FirstOrDefault().Name
        }

        [HttpGet]
        public JsonResult DeleteAttachment(Guid Id)
        {
            try
            {
                var Attachment = repository.GetByKey<FollowUpProjectPlanAttachment>(Id);
                repository.Delete(Attachment);
                repository.UnitOfWork.SaveChanges();

                if (System.IO.File.Exists(Attachment.URL))
                {
                    System.IO.File.Delete(Attachment.URL);
                }
                return Json("Deleted Succsesfully", JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json("Deletion Failed", JsonRequestBehavior.AllowGet);
            }
        }

        public FileResult Download(Guid id)
        {
            var attachmet = repository.GetByKey<FollowUpProjectPlanAttachment>(id);
            byte[] fileBytes = System.IO.File.ReadAllBytes(attachmet.URL);
            string fileName = attachmet.Name;
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
        #endregion
    }
}