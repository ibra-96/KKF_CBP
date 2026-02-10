using System;
using System.Linq;
using System.Web.Mvc;
using AlphaPeople.Core;
using System.Configuration;
using AlphaPeople.Repository;
using AlfaPeople.KingKhalidFoundation.Data;
using AlfaPeople.KingKhalidFoundation.Data.Model;
using AlfaPeople.KingKhaledFoundation.Admin.Web.Helper;

namespace AlfaPeople.KingKhaledFoundation.Admin.Web.Controllers
{
    [Authorize(Roles = "Admin, CB Manager, CB Supervisor, CB Analyst")]
    public class WorkshopRequestController : BaseController
    {
        private readonly IRepository repository;
        private readonly CommonHelper helper;
        private readonly string Logpath = ConfigurationManager.AppSettings["Log"].ToString();

        public WorkshopRequestController()
        {
            helper = new CommonHelper();
            repository = new Repository(new KingkhaledFoundationDB());
        }

        // GET: Incubation Request
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.lang = CultureHelper.CurrentCulture;
            ViewBag.Title = App_GlobalResources.General.WorkshopRequests;
            //2-25-2025
            var result = repository.GetQuery<WorkshopProjectProposal>(f => f.IncubationWorkshop.IncubationtWorkshopStatu.NameEN == "Active"&&!f.IncubationWorkshop.IsDeleted).ToList();
            return View(result);
        }

        // GET: Incubation Workshop Requests
        [HttpGet]
        public ActionResult WorkshopRequests(Guid Id)
        {
            ViewBag.lang = CultureHelper.CurrentCulture;
            ViewBag.Title = App_GlobalResources.General.WorkshopRequests;
            var result = repository.GetByKey<WorkshopProjectProposal>(Id);
            if (CultureHelper.CurrentCulture == 3)
                ViewBag.LstDrop = new SelectList(repository.GetQuery<WorkshopProjectProposalStatu>(s => s.NameEN == "Accepted" || s.NameEN == "Rejected" || s.NameEN == "Update Project Proposal").ToList(), "WorkshopProjectProposalStatusID", "NameAR");
            else
                ViewBag.LstDrop = new SelectList(repository.GetQuery<WorkshopProjectProposalStatu>(s => s.NameEN == "Accepted" || s.NameEN == "Rejected" || s.NameEN == "Update Project Proposal").ToList(), "WorkshopProjectProposalStatusID", "NameEN");
            return View(result);
        }

        [HttpPost]
        public ActionResult WorkshopRequests(Guid WorkshopProjectProposalID, Guid ActionID, string feadback)
        {
            try
            {
                if (WorkshopProjectProposalID != null && ActionID != null && (feadback != null || !string.IsNullOrWhiteSpace(feadback)))
                {
                    var entity = repository.GetByKey<WorkshopProjectProposal>(WorkshopProjectProposalID);
                    entity.WorkshopProjectProposalStatusID = ActionID;
                    entity.Feedback = feadback;
                    repository.Update(entity);
                    repository.UnitOfWork.SaveChanges();

                    var UserMail = entity.FrontendUser?.AspNetUser?.Email;
                    if (UserMail != null)
                    {
                        MailHelper mailHelper = new MailHelper();
                        mailHelper.ToEmail = UserMail;
                        mailHelper.IsHtml = true;

                        if (entity.WorkshopProjectProposalStatu.NameEN == "Update Project Proposal")
                        {
                            mailHelper.Subject = "استكمال متطلبات حضور ورشة عمل";
                            mailHelper.Body = "المكرمين/ المنظمات غير الربحية                         الموقرين<br/>"
                                            + "السلام عليكم ورحمة الله وبركاته،"
                                            + "تهديكم مؤسسة الملك خالد أطيب التحيات، <br/>"
                                            + $"ونحيطكم علماً بأن طلب التسجيل بورشة عمل {entity.IncubationWorkshop.Name} الخاص بكم تنقصه البيانات التالية: <br/>"
                                            + $"{feadback} <br/>"
                                            //+ "- ولتأكيد حضور المرشحين، نود منكم التكرم باستكمال ما يلي: <br/>"
                                            //+ "1- إرفاق صور من تذاكر السفر. <br/>"
                                            //+ "2- تحديد رغبتكم بالإقامة في الفندق من عدمها. <br/>"
                                            //+ "3- تحديد رغبتكم بتوفير مواصلات من الفندق للمؤسسة، ومن المؤسسة للفندق. <br/>"
                                            //+ $"نرجو إكمال البيانات على البوابة الإلكترونية وإعادة إرسال الطلب بحد أقصى: {entity.IncubationWorkshop.LastTimeToApply.Date /*.ToString("dd/MM/yyyy")*/ }. <br/>"
                                            //+ "كما نرجو إحضار فواتير الوقود للقادمين بالسيارة من خارج الرياض. <br/>"
                                            + "شاكرين ومقدرين اهتمامكم وتعاونكم، <br/>"
                                            + " برنامج بناء القدرات. <br/>";
                        }
                        if (entity.WorkshopProjectProposalStatu.NameEN == "Rejected")
                        {
                            mailHelper.Subject = "تنويه";
                            mailHelper.Body =
                              "السادة المنظمات غير الربحية الموقرين،<br/><br/>" +
                              "السلام عليكم ورحمة الله وبركاته،،<br/><br/>" +
                              $"تهديكم مؤسسة الملك خالد أطيب التحيات، ونشكركم على تعبئة استمارة التسجيل من قبلكم لحضور مشروع <b>{entity.IncubationWorkshop.Name}</b>،<br/>" +
                              $"ونعتذر لكم للأسباب التالية:<br/><br/>" +
                              $"{feadback}<br/><br/>" +
                              "ونتطلع لانضمامكم في المشاريع القادمة.<br/><br/>" +
                              "وتفضلوا بقبول فائق التحية والتقدير،،<br/>" +
                              "فريق برنامج بناء القدرات<br/>" +
                              "مؤسسة الملك خالد.";
                            //mailHelper.Body = "المكرمين/ المنظمات غير الربحية                         الموقرين<br/>"
                            //                + "السلام عليكم ورحمة الله وبركاته، <br/>"
                            //                + "تهديكم مؤسسة الملك خالد أطيب التحيات، <br/>"
                            //                + $"ويؤسفنا إبلاغكم بأنه تم رفض طلبكم للالتحاق بورشة عمل {entity.IncubationWorkshop.Name}. <br/>"
                            //                + "وذلك للأسباب التالية: <br/>"
                            //                + $" {feadback} <br/>"
                            //                + "شاكرين ومقدرين اهتمامكم وتفهمكم، <br/>"
                            //                + "برنامج بناء القدرات. <br/>";
                        }
                        if (entity.WorkshopProjectProposalStatu.NameEN == "Accepted")
                        {
                            mailHelper.Subject = "تهنئة ";
                           
                                mailHelper.Body =
                                    "السادة المنظمات غير الربحية الموقرين،<br/><br/>" +
                                    "السلام عليكم ورحمة الله وبركاته،،<br/><br/>" +
                                    $"تهديكم مؤسسة الملك خالد أطيب التحيات، ونشكركم على تعبئة استمارة التسجيل من قبلكم لحضور مشروع <b>{entity.IncubationWorkshop.Name}</b>،<br/>" +
                                    "ونفيدكم بأنه تم قبولكم لحضور المشروع، وسيتم التواصل معكم لاستكمال الإجراءات وإرفاق المتطلبات التالية:<br/><br/>" +
                                    "1. تذكرة السفر (طيارة، قطار) للمرشح موضح فيها الاسم وتاريخ الوصول والمغادرة والقيمة المالية.<br/>" +
                                    "2. تحديد رغبتكم بالإقامة بالفندق من عدمها، علماً أن الغرفة مخصصة لمرشح واحد لتوفير الخصوصية.<br/>" +
                                    "3. تحديد رغبتكم بتوفير مواصلات من الفندق إلى المؤسسة والعكس، وذلك لحضور ورشة العمل.<br/>" +
                                    "4. نرجو إحضار فواتير الوقود للقادمين عبر السيارة من خارج الرياض.<br/><br/>" +
                                    "وتفضلوا بقبول فائق التحية والتقدير،،<br/>" +
                                    "فريق برنامج بناء القدرات<br/>" +
                                    "مؤسسة الملك خالد.";
                          

                            //mailHelper.Body = "المكرمين/ المنظمات غير الربحية                         الموقرين<br/>"
                            //                + "السلام عليكم ورحمة الله وبركاته، <br/>"
                            //                + "تهديكم مؤسسة الملك خالد أطيب التحيات، <br/>"
                            //                + $"ونحيطكم علماً بأنه تم قبول طلبكم لحضور ورشة عمل {entity.IncubationWorkshop.Name}. <br/>"
                            //                + "ونتطلع للقائكم في الموعد المشار إليه بالبوابة الإلكترونية. <br/>"
                            //                + "مع تمنياتنا لكم بالتوفيق، <br/>"
                            //                + "برنامج بناء القدرات. <br/>";
                        }

                        var fundingSource = repository.GetByKey<FundingSource>(entity.IncubationWorkshop.FundingSourceID);
                        if (fundingSource.UseCustomThemes)
                            mailHelper.Send($"?partner={fundingSource.Nickname}");
                        else
                            mailHelper.Send("");
                    }
                    return RedirectToAction("Index");
                }

                var result = repository.GetByKey<WorkshopProjectProposal>(WorkshopProjectProposalID);
                ViewBag.LstDrop = new SelectList(repository.GetQuery<WorkshopProjectProposalStatu>(s => s.NameEN == "Accepted" || s.NameEN == "Rejected" || s.NameEN == "Update Project Proposal").ToList(), "WorkshopProjectProposalStatusID", CultureHelper.CurrentCulture != 3 ? "NameEN" : "NameAR");
                return View(result);
            }
            catch (Exception)
            {
                var result = repository.GetByKey<WorkshopProjectProposal>(WorkshopProjectProposalID);
                ViewBag.Title = App_GlobalResources.General.WorkshopRequests;
                if (CultureHelper.CurrentCulture == 3)
                    ViewBag.LstDrop = new SelectList(repository.GetQuery<WorkshopProjectProposalStatu>(s => s.NameEN == "Accepted" || s.NameEN == "Rejected" || s.NameEN == "Update Project Proposal").ToList(), "WorkshopProjectProposalStatusID", "NameAR");
                else
                    ViewBag.LstDrop = new SelectList(repository.GetQuery<WorkshopProjectProposalStatu>(s => s.NameEN == "Accepted" || s.NameEN == "Rejected" || s.NameEN == "Update Project Proposal").ToList(), "WorkshopProjectProposalStatusID", "NameEN");
                return View(result);
            }
        }

        public FileResult Download(Guid id)
        {
            var attachmet = repository.GetByKey<IncubationWorkshopAttachment>(id);
            byte[] fileBytes = System.IO.File.ReadAllBytes(attachmet.URL);
            string fileName = attachmet.Name;
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        public FileResult DownloadWSRequest(Guid id)
        {
            var attachmet = repository.GetByKey<WorkshopProjectProposalAttachment>(id);
            byte[] fileBytes = System.IO.File.ReadAllBytes(attachmet.URL);
            string fileName = attachmet.Name;
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        [HttpGet]
        public JsonResult DeleteAttachment(Guid Id)
        {
            try
            {
                var Attachment = repository.GetByKey<WorkshopProjectProposalAttachment>(Id);
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
    }
}