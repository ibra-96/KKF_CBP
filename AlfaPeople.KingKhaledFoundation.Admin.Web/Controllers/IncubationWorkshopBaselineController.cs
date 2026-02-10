using System;
using System.Linq;
using System.Web.Mvc;
using AlphaPeople.Core;
using System.Configuration;
using AlphaPeople.Repository;
using Microsoft.Ajax.Utilities;
using AlfaPeople.KingKhalidFoundation.Data;
using AlfaPeople.KingKhalidFoundation.Data.Model;
using AlfaPeople.KingKhaledFoundation.Admin.Web.Helper;
using AlfaPeople.KingKhaledFoundation.Admin.Web.Models;
using System.Data.Entity;
using System.Collections.Generic;

namespace AlfaPeople.KingKhaledFoundation.Admin.Web.Controllers
{
    [Authorize(Roles = "Admin, CB Manager, CB Supervisor, CB Analyst")]
    public class IncubationWorkshopBaselineController : BaseController
    {
        private readonly IRepository repository;
        private readonly CommonHelper helper;
        private readonly string Logpath = ConfigurationManager.AppSettings["Log"].ToString();

        public IncubationWorkshopBaselineController()
        {
            helper = new CommonHelper();
            repository = new Repository(new KingkhaledFoundationDB());
        }
        public ActionResult Index()
        {
            ViewBag.lang = CultureHelper.CurrentCulture;

            var result = repository.GetQuery<IncubationWorkshopBLTransactionsValue>()
                .Where(v => v.IncubationWorkshopBLTransValStatus.NameEN != "Draft")
                .GroupBy(v => new { v.FrontendUserID, v.IncubationWorkshopBLTransValStatusID,v.IncubationWorkshopBLTrans.IncubationWorkshop.IncubationWorkshopID})
                .Select(g => g.FirstOrDefault())
                .ToList();

            if (!result.Any())
            {
                ViewBag.Message = "لا توجد بيانات حالياً.";
            }

            return View(result);
        }

        //  public ActionResult Index()
        //   {
        //      ViewBag.lang = CultureHelper.CurrentCulture;
        //      var result = repository.GetQuery<IncubationWorkshopBLTransactionsValue>()
        //.Where(v => v.IncubationWorkshopBLTransValStatus.NameEN != "Draft")
        // .DistinctBy(d => new { d.FrontendUserID,  d.SubmissionDate }).ToList();

        //      if (!result.Any())
        //      {
        //          ViewBag.Message = "لا توجد بيانات حالياً.";
        //      }

        //      return View(result);


        //  }
        [HttpGet]
        public ActionResult Details(Guid FUserID, Guid StatusID, DateTime SubmissionDate, Guid IncubationWorkshopID, string WorkshopName)
            {
            ViewBag.lang = CultureHelper.CurrentCulture;
            ViewBag.workshopID = IncubationWorkshopID;
            ViewBag.workshopName =WorkshopName;
            ViewBag.frontEnd = FUserID;
            // جلب حالة الطلب
            var IncILStatus = repository.GetByKey<IncubationWorkshopBLTransValStatus>(StatusID);

            // جلب القيم مع تضمين الخيارات والبيانات المرتبطة
            var IncIL = repository.GetQuery<IncubationWorkshopBLTransactionsValue>(v =>
            v.IncubationWorkshopBLTrans.ViewList_Display == "true" &&
          v.FrontendUserID == FUserID &&
          v.IncubationWorkshopBLTransValStatusID == StatusID &&
           DbFunctions.TruncateTime(v.SubmissionDate) == SubmissionDate.Date&&
                v.IncubationWorkshopBLTrans.IncubationWorkshopID == IncubationWorkshopID)
      .Include(v => v.IncubationWorkshopBLTrans)
      .Include(v => v.IncubationWorkshopBLTrans.Options)
      .Include(v => v.OptionValues)
      .Include(v => v.IncubationWorkshopBLTrans.IncubationWorkshopBLTransType)
      .ToList();
            

            // التحقق من وجود القيم
            if (!IncIL.Any())
                return RedirectToAction("dashboard", new { Msg = "No records found." });

            // إعداد القائمة المنسدلة حسب اللغة والحالة
            if (IncILStatus.NameEN == "Pending" || IncILStatus.NameEN == "Baseline Application Form Updated")
            {
                ViewBag.LstDrop = new SelectList(
                    repository.GetQuery<IncubationWorkshopBLTransValStatus>()
                              .Where(f => f.NameEN == "Update Baseline Application Form" || f.NameEN == "Rejected" || f.NameEN == "Accepted")
                              .ToList(),
                    "IncubationWorkshopBLTransValStatusID",
                    CultureHelper.CurrentCulture == 3 ? "NameAR" : "NameEN"
                );
                
            }
            else
            {
                ViewBag.LstDrop = new SelectList(
                    repository.GetQuery<IncubationWorkshopBLTransValStatus>()
                              .Where(f => f.NameEN == "Update Baseline Application Form")
                              .ToList(),
                    "IncubationWorkshopBLTransValStatusID",
                    CultureHelper.CurrentCulture == 3 ? "NameAR" : "NameEN"
                );
            }

            var model = new IncubationWSBaselineVM()
            {
                FrontendUserID = FUserID,
                SubmissionDate = SubmissionDate,
                IncubationWSBLTransValStatusID = StatusID,
                Feadback = IncIL.FirstOrDefault()?.Feadback,
                IncubationWorkshopID= IncubationWorkshopID,
                //WorkshopID = WorkshopID, // إضافة WorkshopID
                WorkshopName = WorkshopName, // إضافة WorkshopName
                IncubationWorkshopBLTransactionsType = IncIL.FirstOrDefault()?.IncubationWorkshopBLTrans?.IncubationWorkshopBLTransType
            };

            // تحميل الخيارات المحددة
            foreach (var trans in model.IncubationWorkshopBLTransactionsType?.IncubationWorkshopBLTrans ?? new List<IncubationWorkshopBLTransactions>())
            {
                trans.IncubationWSBLTransactionsValue = IncIL.Where(v => v.TransID == trans.TransID).ToList();
            }
           
            return View(model);
        }

        //[HttpGet]
        //public ActionResult Details(Guid FUserID, Guid StatusID, DateTime SubmissionDate)
        //{
        //    ViewBag.lang = CultureHelper.CurrentCulture;
        //    var IncILStatus = repository.GetByKey<IncubationWorkshopBLTransValStatus>(StatusID);
        //    var IncIL = repository.GetQuery<IncubationWorkshopBLTransactionsValue>(v => v.FrontendUserID == FUserID && v.IncubationWorkshopBLTransValStatusID == StatusID && v.SubmissionDate == SubmissionDate).ToList();

        //    if (CultureHelper.CurrentCulture == 3)
        //    {
        //        if (IncILStatus.NameEN == "Pending" || IncILStatus.NameEN == "Baseline Application Form Updated")
        //        {
        //            ViewBag.LstDrop = new SelectList(repository.GetQuery<IncubationWorkshopBLTransValStatus>(f => f.NameEN == "Update Baseline Application Form" || f.NameEN == "Rejected" || f.NameEN == "Accepted").ToList(), "IncubationWorkshopBLTransValStatusID", "NameAR");
        //        }
        //        else
        //        {
        //            ViewBag.LstDrop = new SelectList(repository.GetQuery<IncubationWorkshopBLTransValStatus>(f => f.NameEN == "Update Baseline Application Form").ToList(), "IncubationWorkshopBLTransValStatusID", "NameAR");
        //        }
        //    }
        //    else
        //    {
        //        if (IncILStatus.NameEN == "Pending" || IncILStatus.NameEN == "Baseline Application Form Updated")
        //        {
        //            ViewBag.LstDrop = new SelectList(repository.GetQuery<IncubationWorkshopBLTransValStatus>(f => f.NameEN == "Update Baseline Application Form" || f.NameEN == "Rejected" || f.NameEN == "Accepted").ToList(), "IncubationWorkshopBLTransValStatusID", "NameEN");
        //        }
        //        else
        //        {
        //            ViewBag.LstDrop = new SelectList(repository.GetQuery<IncubationWorkshopBLTransValStatus>(f => f.NameEN == "Update Baseline Application Form").ToList(), "IncubationWorkshopBLTransValStatusID", "NameEN");
        //        }
        //    }

        //    return View(new IncubationWSBaselineVM()
        //    {
        //        FrontendUserID = FUserID,
        //        SubmissionDate = SubmissionDate,
        //        IncubationWSBLTransValStatusID = StatusID,
        //        Feadback = IncIL.FirstOrDefault().Feadback,
        //        IncubationWorkshopBLTransactionsType = IncIL.FirstOrDefault().IncubationWorkshopBLTrans.IncubationWorkshopBLTransType
        //    });
        //}

        [HttpPost]
        public ActionResult Details(Guid FrontendUserID, Guid IncubationWSBLTransValStatusID, string feadback, Guid IncubationWorkshopID, string WorkshopName)
        {
            var _IncubationBaseline = repository.GetAll<IncubationWorkshopBLTransactionsValue>().Where(v => v.FrontendUserID == FrontendUserID&&v.IncubationWorkshopBLTrans.IncubationWorkshopID== IncubationWorkshopID).ToList();
            _IncubationBaseline.ForEach(v => { v.IncubationWorkshopBLTransValStatusID = IncubationWSBLTransValStatusID; v.Feadback = feadback; });
            repository.UnitOfWork.SaveChanges();

            //foreach (var item in _IncubationBaseline)
            //{
            //    item.Feadback = feadback;
            //    item.IncubationWorkshopBLTransValStatusID = IncubationWSBLTransValStatusID;
            //    repository.Update(item);
            //}

            //var FrontEndMail = _IncubationBaseline.FirstOrDefault().FrontendUser.AspNetUser.Email;
            //var IncubationWorkshopBLTransValStatus = repository.GetByKey<IncubationWorkshopBLTransValStatus>(IncubationWSBLTransValStatusID);

            //MailHelper mailHelper = new MailHelper();
            //mailHelper.ToEmail = FrontEndMail;
            //if (IncubationWorkshopBLTransValStatus.NameEN == "Update Baseline Application Form")
            //{
            //    mailHelper.Subject = "استكمال متطلبات نموذج أستبيان الوضع الحالي للمنظمة";
            //    mailHelper.Body = "المكرمين/ المنظمات غير الربحية                         الموقرين <br />"
            //                        + "السلام عليكم ورحمة الله وبركاته، <br />"
            //                        + "تهديكم مؤسسة الملك خالد أطيب التحيات، <br /> ونحيطكم علماً بأن طلب أستبيان الوضع الحالي للمنظمة الخاص بكم تنقصه البيانات التالية: <br />"
            //                        + $" {feadback}  <br />"
            //                        + $" نرجو منكم التكرم بإكمال البيانات على البوابة الإلكترونية وإعادة إرسال الطلب قبل موعد انتهاء ورشة العمل المراد الاشتراك بها.  <br />"
            //                        + "شاكرين ومقدرين اهتمامكم وتعاونكم، <br />"
            //                        + " برنامج بناء القدرات.  <br />";
            //}
            //if (IncubationWorkshopBLTransValStatus.NameEN == "Rejected")
            //{
            //    mailHelper.Subject = "تنويه";
            //    mailHelper.Body = "المكرمين/ المنظمات غير الربحية                         الموقرين <br />"
            //                        + "السلام عليكم ورحمة الله وبركاته، <br />"
            //                        + "تهديكم مؤسسة الملك خالد أطيب التحيات،<br /> ويؤسفنا إبلاغكم بأنه تم رفض طلب أستبيان الوضع الحالي الخاص بمنظمتكم،<br /> وذلك للأسباب التالية :  <br />"
            //                        + $" {feadback}  <br />"
            //                        + "شاكرين ومقدرين اهتمامكم وتعاونكم، <br />"
            //                        + " برنامج بناء القدرات.  <br />";
            //}
            //if (IncubationWorkshopBLTransValStatus.NameEN == "Accepted")
            //{
            //    mailHelper.Subject = "تهنئة";
            //    mailHelper.Body = "المكرمين/ المنظمات غير الربحية                         الموقرين <br />"
            //                        + "السلام عليكم ورحمة الله وبركاته، <br />"
            //                        + "تهديكم مؤسسة الملك خالد أطيب التحيات، <br /> ونحيطكم علماً بأنه تم قبول طلب أستبيان الوضع الحالي الخاص بمنظمتكم للالتحاق، <br /> الرجاء الذهاب الي البوابه الإلكترونية لتقديم بيانات المشاركين في ورشة العمل المراد الاشتراك بها. <br />"
            //                        + "مع تمنياتنا لكم بالتوفيق، <br />"
            //                        + " برنامج بناء القدرات.  <br />";
            //}
            //mailHelper.Send("");
            var FrontEndMail = _IncubationBaseline.FirstOrDefault()?.FrontendUser.AspNetUser.Email;
            var IncubationWorkshopBLTransValStatus = repository.GetByKey<IncubationWorkshopBLTransValStatus>(IncubationWSBLTransValStatusID);

            MailHelper mailHelper = new MailHelper();
            mailHelper.ToEmail = FrontEndMail;

            switch (IncubationWorkshopBLTransValStatus.NameEN)
            {
                case "Update Baseline Application Form":
                    mailHelper.Subject = $"طلب تعديل بيانات الاستمارة - {WorkshopName}";
                    //mailHelper.Body = "السادة المنظمات غير الربحية الموقرين،<br />" +
                    //                  "السلام عليكم ورحمة الله وبركاته،<br />" +
                    //                  $"تهديكم مؤسسة الملك خالد أطيب التحيات، ونشكركم على تعبئة استمارة تسجيل المرشح من قبلكم لحضور مشروع&nbsp;<b>{WorkshopName}</b>.<br />" +
                    //                  "نأمل منكم تحديث نموذج الترشيح من خلال الرابط التالي:<br />" +
                    //                  "<a href='https://cbp.kkf.org.sa/' target='_blank'>اضغط هنا لتحديث النموذج</a><br /><br />" +
                    //                  "وتفضلوا بقبول فائق التحية والتقدير،<br />" +
                    //                  "فريق برنامج بناء القدرات<br />" +
                    //                  "مؤسسة الملك خالد.";
                    
                    mailHelper.Body =
                        "السادة المنظمات غير الربحية الموقرين،<br/><br/>" +
                        "السلام عليكم ورحمة الله وبركاته،،<br/><br/>" +
                        $"تهديكم مؤسسة الملك خالد أطيب التحيات، ونشكركم على تعبئة استمارة التسجيل من قبلكم لحضور مشروع <b>{WorkshopName}</b>، ونأمل منكم التحديث من خلال الرابط التالي:<br/>" +
                        "<a href='https://cbp.kkf.org.sa/' target='_blank'>اضغط هنا لتحديث النموذج</a><br/><br/>" +
                        "وتفضلوا بقبول فائق التحية والتقدير،،<br/>" +
                        "فريق برنامج بناء القدرات<br/>" +
                        "مؤسسة الملك خالد";

                    break;

                case "Accepted":
                    mailHelper.Subject = $"قبول المنظمة للمشروع - {WorkshopName}";
                    mailHelper.Body = "السادة المنظمات غير الربحية الموقرين،<br />" +
                                      "السلام عليكم ورحمة الله وبركاته،<br />" +
                                      $"تهديكم مؤسسة الملك خالد أطيب التحيات، ونشكركم على تعبئة استمارة تسجيل المرشح من قبلكم لحضور مشروع&nbsp;<b>{WorkshopName}</b>.<br />" +
                                      "نفيدكم بأنه تم قبول المرشح لحضور المشروع، وسيتم التواصل معكم لاستكمال الإجراءات.<br /><br />" +
                                      "وتفضلوا بقبول فائق التحية والتقدير،<br />" +
                                      "فريق برنامج بناء القدرات<br />" +
                                      "مؤسسة الملك خالد.";
                    break;

                case "Rejected":
                    mailHelper.Subject = $"رفض المنظمة للانضمام للمشروع - {WorkshopName}";
                    mailHelper.Body = "السادة المنظمات غير الربحية الموقرين،<br />" +
                                      "السلام عليكم ورحمة الله وبركاته،<br />" +
                                      $"تهديكم مؤسسة الملك خالد أطيب التحيات، ونشكركم على تعبئة استمارة تسجيل المرشح من قبلكم لحضور مشروع&nbsp;<b>{WorkshopName}</b>.<br />" +
                                      "نعتذر عن قبول المرشح للحضور، ونتطلع لانضمامكم في المشاريع القادمة.<br /><br />" +
                                      "وتفضلوا بقبول فائق التحية والتقدير،<br />" +
                                      "فريق برنامج بناء القدرات<br />" +
                                      "مؤسسة الملك خالد.";
                    break;

                default:
                    mailHelper.Subject = $"إشعار هام من مؤسسة الملك خالد - {WorkshopName}";
                    mailHelper.Body = "السادة المنظمات غير الربحية الموقرين،<br />" +
                                      "السلام عليكم ورحمة الله وبركاته،<br />" +
                                      "يرجى متابعة حالة طلبكم على البوابة الإلكترونية الخاصة بمؤسسة الملك خالد.<br /><br />" +
                                      "وتفضلوا بقبول فائق التحية والتقدير،<br />" +
                                      "فريق برنامج بناء القدرات<br />" +
                                      "مؤسسة الملك خالد.";
                    break;
            }

            mailHelper.Send("");

            return RedirectToAction("Index");
        }

        public ActionResult CorporateProfile(Guid Id)
        {
            ViewBag.lang = CultureHelper.CurrentCulture;
            return View(repository.FindOne<CorporateApplicationForm>(f => f.FrontendUserID == Id && f.Program.ProgramName == "Capacity Building"));
        }

        public ActionResult Download(string URL)
        {
            try
            {
                byte[] fileBytes = System.IO.File.ReadAllBytes(URL);
                string fileName = URL.Substring(URL.LastIndexOf('\\') + 1);
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }
            catch (Exception)
            {
                return Content("Something Went Wrong");
            }
        }
    }
}