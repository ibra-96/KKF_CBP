using System;
using System.Linq;
using System.Web.Mvc;
using AlphaPeople.Core;
using System.Configuration;
using AlphaPeople.Repository;
using Microsoft.AspNet.Identity;
using AlfaPeople.KingKhalidFoundation.Data;
using AlfaPeople.KingKhalidFoundation.Data.Model;
using AlfaPeople.KingKhaledFoundation.Admin.Web.Helper;

namespace AlfaPeople.KingKhaledFoundation.Admin.Web.Controllers
{
    [Authorize(Roles = "Admin, CB Manager, CB Supervisor, CB Analyst")]
    public class CorporationRequestsController : BaseController
    {
        private KingkhaledFoundationDB db = new KingkhaledFoundationDB();
        private readonly IRepository repository;
        private readonly CommonHelper helper;
        private readonly string Logpath = ConfigurationManager.AppSettings["Log"].ToString();

        public CorporationRequestsController()
        {
            repository = new Repository(new KingkhaledFoundationDB());
            helper = new CommonHelper();
        }

        // GET: CorporationRequests
        public ActionResult All()
        {
            return View();
        }

        public ActionResult Index()
        {
            string ProgramType = "";
            if (User.IsInRole("CB Manager") || User.IsInRole("CB Supervisor") || User.IsInRole("CB Analyst"))
            {
                ProgramType = "Capacity Building";
            }

            var LstFrontendUserNotApproved = (User.IsInRole("Admin") && string.IsNullOrWhiteSpace(ProgramType)) ?
                repository.Find<CorporateApplicationForm>(f => f.CorporateApplicationStatu.ApplicantStatu.ApplicantStatusName == "Pending").ToList() :
                repository.Find<CorporateApplicationForm>(f => (f.Program.ProgramName == ProgramType) && f.CorporateApplicationStatu.ApplicantStatu.ApplicantStatusName == "Pending").ToList();
            return View(LstFrontendUserNotApproved);
        }

        // GET: CorporationRequests/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return View();
            }

            var _CorporateApplicationForm = repository.Get<CorporateApplicationForm>(f => f.FrontendUserID == id).ToList();

            if (_CorporateApplicationForm == null)
            {
                return HttpNotFound();
            }

            return Json(new { success = true, html = GlobalClass.RenderRazorViewToString(this, "Index", _CorporateApplicationForm), message = "Submitted Successfully" }, JsonRequestBehavior.AllowGet);
        }

        public FileResult Download(Guid id)
        {
            var attachmet = repository.GetByKey<CorporateApplicationFormAttachment>(id);
            byte[] fileBytes = System.IO.File.ReadAllBytes(attachmet.URL);
            string fileName = attachmet.Name;
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        public FileResult DownloadCorFile(Guid id)
        {
            var attachmet = repository.GetByKey<CorporateApplicationFormAttachment>(id);
            byte[] fileBytes = System.IO.File.ReadAllBytes(attachmet.URL);
            string fileName = attachmet.Name;
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        public ActionResult CorporateIndex()
        {
            ViewBag.lang = CultureHelper.CurrentCulture;

            string ProgramType = "";
            if (User.IsInRole("CB Manager") || User.IsInRole("CB Supervisor") || User.IsInRole("CB Analyst"))
            {
                ProgramType = "Capacity Building";
            }
            var LstFrontendUserNotApproved = (User.IsInRole("Admin") && string.IsNullOrWhiteSpace(ProgramType)) ?
                repository.Find<CorporateApplicationForm>(f => f.CorporateApplicationStatu.ApplicantStatu.ApplicantStatusName == "Pending").ToList() :
                repository.Find<CorporateApplicationForm>(f => (f.Program.ProgramName == ProgramType) && f.CorporateApplicationStatu.ApplicantStatu.ApplicantStatusName == "Pending").ToList();
            return View(LstFrontendUserNotApproved.OrderByDescending(o => o.FrontendUser.CreateDate));
        }

        public JsonResult RejectCorperationAction(Guid UserID, string ActionForCorpertion, string ReasonType = null, string Feadback = "عدم استكمال البيانات")
        {
            try
            {
                if (ActionForCorpertion == "Accepted")
                    AcceptUser(UserID);
                else
                    RejectUser(UserID, ReasonType, Feadback);

                return Json("Action Done", JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        #region Helper
        private void RejectUser(Guid UserID, string ReasonType, string Feadback)
        {
            var _FronUser = repository.GetByKey<FrontendUser>(UserID);
            _FronUser.IsApproved = false;
            repository.Update<FrontendUser>(_FronUser);
            var CorUserId = repository.FindOne<CorporateApplicationForm>(f => f.FrontendUserID == UserID).CorporateApplicationFormID;

            var _CorStatus = repository.FindOne<CorporateApplicationStatu>(f => f.CorporateApplicationFormID == CorUserId);

            _CorStatus.ApplicantStatusID = repository.FindOne<ApplicantStatu>(f => f.ApplicantStatusName == "Rejected").ApplicantStatusID;
            _CorStatus.FeadBack = Feadback;
            var userId = User.Identity.GetUserId();
            Guid BackEndId = Guid.Parse(userId);
            _CorStatus.Fk_BackEndMakeAction = BackEndId;
            _CorStatus.DateTimeMakeAction = DateTime.Now;
            repository.Update<CorporateApplicationStatu>(_CorStatus);
            MailHelper mailHelper = new MailHelper();
            var Program = repository.FindOne<CorporateApplicationForm>(f => f.FrontendUserID == UserID).Program.ProgramNameAR;
            mailHelper.ToEmail = _FronUser.AspNetUser.Email;
            if (ReasonType == "Bad Request")
            {
                var frontend = repository.GetByKey<FrontendUser>(UserID);
                frontend.IsApproved = false;
                repository.Update<FrontendUser>(frontend);
                mailHelper.Subject = "تنويه";
                mailHelper.IsHtml = true;
                mailHelper.Body = "المكرمين/ المنظمات غير الربحية                         الموقرين<br/>"
                    + "السلام عليكم ورحمة الله وبركاته،، <br/>"
                    + "تهديكم مؤسسة الملك خالد أطيب التحيات، ويؤسفنا إبلاغكم بأنه تم رفض طلب تسجيلكم وذلك للأسباب التالية:  .  <br/>"
                    + Feadback + "<br/>"
                    + "شاكرين ومقدرين اهتمامكم وتفهمكم،<br/> "
                    + "مع تمنياتنا لكم بالتوفيق،<br/>"
                    + $" برنامج {Program}<br/>";
            }
            else
            {
                mailHelper.Subject = $"استكمال متطلبات التسجيل في برنامج {Program}";
                mailHelper.IsHtml = true;
                mailHelper.Body = "المكرمين/ المنظمات غير الربحية                         الموقرين<br/>"
                    + "السلام عليكم ورحمة الله وبركاته،، <br/>"
                    + "تهديكم مؤسسة الملك خالد أطيب التحيات، ونحيطكم علماً بأن نموذج التسجيل الخاص بكم تنقصه البيانات التالية:  .  <br/>"
                    + Feadback + "<br/>"
                    + "نرجو إكمال البيانات عبر البوابة الإلكترونية. <br/>"
                    + "مع تمنياتنا لكم بالتوفيق،<br/>"
                    + "شاكرين ومقدرين اهتمامكم وتعاونكم، <br/>"
                    + $" برنامج {Program} <br/>";
            }
            mailHelper.Send("");
            repository.UnitOfWork.SaveChanges();

        }

        private void AcceptUser(Guid FronUserID)
        {
            var _FronUser = repository.GetByKey<FrontendUser>(FronUserID);
            _FronUser.IsApproved = true;
            repository.Update<FrontendUser>(_FronUser);
            var CorUserId = repository.FindOne<CorporateApplicationForm>(f => f.FrontendUserID == FronUserID).CorporateApplicationFormID;

            var _CorStatus = repository.FindOne<CorporateApplicationStatu>(f => f.CorporateApplicationFormID == CorUserId);

            _CorStatus.ApplicantStatusID = repository.FindOne<ApplicantStatu>(f => f.ApplicantStatusName == "Accepted").ApplicantStatusID;
            var userId = User.Identity.GetUserId();
            Guid BackEndId = Guid.Parse(userId);
            _CorStatus.Fk_BackEndMakeAction = BackEndId;
            _CorStatus.DateTimeMakeAction = DateTime.Now;
            repository.Update(_CorStatus);
            repository.UnitOfWork.SaveChanges();
            var Program = repository.FindOne<CorporateApplicationForm>(f => f.FrontendUserID == FronUserID).Program.ProgramNameAR;
            string siAdditionalLine = Program == "الاستثمار الاجتماعي" ?
                    $"تهديكم مؤسسة الملك خالد أطيب التحيات، ونحيطكم علماً بأنه تم قبول تسجيلكم في برنامج { Program }، يرجى الدخول على المنصة الإلكترونية و مراجعة لوحة التحكم للتقديم على المنحة و تعبئة نموذج مقترح المشروع. <br/>"
                    : $"تهديكم مؤسسة الملك خالد أطيب التحيات، ونحيطكم علماً بأنه تم قبول تسجيلكم في برنامج { Program }.<br/>";

            MailHelper mailHelper = new MailHelper();
            mailHelper.ToEmail = _FronUser.AspNetUser.Email;
            mailHelper.Subject = $"تأكيد القبول في برنامج {Program }";
            mailHelper.IsHtml = true;
            mailHelper.Body = "المكرمين/ المنظمات غير الربحية                         الموقرين<br/>"
                      + "السلام عليكم ورحمة الله وبركاته، <br/>"
                      + siAdditionalLine
                      + "مع تمنياتنا لكم بالتوفيق،<br/>"
                      + $" برنامج  {Program }. <br/>";
            mailHelper.Send("");
        }
        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
