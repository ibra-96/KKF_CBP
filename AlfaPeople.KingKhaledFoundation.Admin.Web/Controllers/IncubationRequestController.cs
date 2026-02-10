using System;
using System.Linq;
using System.Web.Mvc;
using AlphaPeople.Core;
using System.Configuration;
using AlphaPeople.Repository;
using AlfaPeople.KingKhalidFoundation.Data;
using AlfaPeople.KingKhalidFoundation.Data.Model;
using AlfaPeople.KingKhaledFoundation.Admin.Web.Helper;
using AlfaPeople.KingKhaledFoundation.Admin.Web.Models;

namespace AlfaPeople.KingKhaledFoundation.Admin.Web.Controllers
{
    [Authorize(Roles = "Admin, CB Manager, CB Supervisor, CB Analyst")]
    public class IncubationRequestController : BaseController
    {
        private readonly IRepository repository;
        private readonly CommonHelper helper;
        private readonly string Logpath = ConfigurationManager.AppSettings["Log"].ToString();

        public IncubationRequestController()
        {
            helper = new CommonHelper();
            repository = new Repository(new KingkhaledFoundationDB());
        }

        // GET: IncubationRequest
        public ActionResult Index(string Type)
        {
            ViewBag.Type = Type;
            if (Type == "Incubation")
                ViewBag.Title = App_GlobalResources.General.IncubationRequests;
            else
                ViewBag.Title = App_GlobalResources.General.AccelerationRequests;

            ViewBag.lang = CultureHelper.CurrentCulture;
            var result = repository.GetQuery<IncubationProjectProposal>(f => f.IncubationAdvertising.IncubationType.NameEN == Type && f.IncubationAdvertising.IsActive == true).ToList();
            return View(result);
        }

        [HttpGet]
        public ActionResult IncubationProjectProposalRequest(Guid Id, string Type)
        {
            TempData["Type"] = Type;
            ViewBag.Type = Type;
            ViewBag.Title = App_GlobalResources.IncubationRequest.Baseline;
            var incubationPP = repository.GetByKey<IncubationProjectProposal>(Id);
            ViewBag.LstDrop = new SelectList(repository.GetQuery<IncubationBaselineStatus>(f => f.NameEN == "Update Baseline Application Form" || f.NameEN == "Rejected" || f.NameEN == "Accepted").ToList(), "IncubationBaselineStatusID", CultureHelper.CurrentCulture == 3 ? "NameAR" : "NameEN");
            if (incubationPP != null)
            {
                var incubationAd = repository.GetByKey<IncubationAdvertising>(incubationPP.IncubationAdID);
                var incubationBL = repository.GetQuery<IncubationBaseline>(b => b.FrontendUserID == incubationPP.FrontendUserID).FirstOrDefault();
                ViewBag.dateBoardApproval = incubationBL.DateBoardApproval?.ToString("yyyy-MM-dd");

                IncubationProjectProposalVM IPPVM = new IncubationProjectProposalVM()
                {
                    IncubationAd = incubationAd,
                    incubationPP = incubationPP,
                    IncubationBL = incubationBL
                };
                return View(IPPVM);
            }
            return View();
        }

        [HttpPost]
        public ActionResult IncubationProjectProposalRequest(IncubationProjectProposalVM model, string feadback)
        {
            string type = TempData["Type"].ToString();
            TempData.Keep();
            ViewBag.Type = type;

            var _IncubationBL = repository.GetByKey<IncubationBaseline>(model.IncubationBL.IncubationBaselineID);
            _IncubationBL.IncubationBaselineStatusID = model.IncubationBL.IncubationBaselineStatusID;
            _IncubationBL.Feadback = feadback;
            var _IncubationBLStatus = repository.GetByKey<IncubationBaselineStatus>(model.IncubationBL.IncubationBaselineStatusID);
            var _IncubationProjectProposal = repository.GetByKey<IncubationProjectProposal>(model.incubationPP.IncubationProjectProposalID);
            _IncubationProjectProposal.Feadback = feadback;
            switch (_IncubationBLStatus.NameEN)
            {
                case "Update Baseline Application Form":
                    _IncubationProjectProposal.IncubationProjectProposalStatusID = repository.GetQuery<IncubationProjectProposalStatu>(s => s.NameEN == "Update Project Proposal").FirstOrDefault().IncubationProjectProposalStatusID;
                    break;
                case "Accepted":
                    _IncubationProjectProposal.IncubationProjectProposalStatusID = repository.GetQuery<IncubationProjectProposalStatu>(s => s.NameEN == "Accepted").FirstOrDefault().IncubationProjectProposalStatusID;
                    break;
                case "Rejected":
                    _IncubationProjectProposal.IncubationProjectProposalStatusID = repository.GetQuery<IncubationProjectProposalStatu>(s => s.NameEN == "Rejected").FirstOrDefault().IncubationProjectProposalStatusID;
                    break;
            }

            repository.Update(_IncubationBL);
            repository.Update(_IncubationProjectProposal);
            repository.UnitOfWork.SaveChanges();

            var FrontEndMail = _IncubationProjectProposal.FrontendUser.AspNetUser.Email;

            MailHelper mailHelper = new MailHelper();

            mailHelper.ToEmail = FrontEndMail;
            if (_IncubationProjectProposal.IncubationProjectProposalStatu.NameEN == "Update Project Proposal")
            {
                if (_IncubationProjectProposal.IncubationAdvertising.IncubationType.NameEN == "Incubation")
                {
                    mailHelper.Subject = "استكمال متطلبات الاحتضان الكامل";
                    mailHelper.Body = "المكرمين/ المنظمات غير الربحية                         الموقرين <br />"
                                        + "السلام عليكم ورحمة الله وبركاته، <br />"
                                        + "تهديكم مؤسسة الملك خالد أطيب التحيات، <br /> ونحيطكم علماً بأن طلب الالتحاق بمشروع الاحتضان الكامل الخاص بكم تنقصه البيانات التالية: <br />"
                                        + $" {feadback}  <br />"
                                        + $" نرجو منكم التكرم بإكمال البيانات على البوابة الإلكترونية وإعادة إرسال الطلب بحد أقصى : {model.IncubationAd.EndDate.Date }.  <br />"
                                        + "شاكرين ومقدرين اهتمامكم وتعاونكم، <br />"
                                        + " برنامج بناء القدرات.  <br />";
                }
                else
                {
                    mailHelper.Subject = "استكمال متطلبات الاحتضان الجزئي";
                    mailHelper.Body = "المكرمين/ المنظمات غير الربحية                         الموقرين <br />"
                                        + "السلام عليكم ورحمة الله وبركاته، <br />"
                                        + "تهديكم مؤسسة الملك خالد أطيب التحيات، <br /> ونحيطكم علماً بأن طلب الالتحاق بمشروع الاحتضان الجزئي الخاص بكم تنقصه البيانات التالية: <br />"
                                        + $" {feadback}  <br />"
                                        + $" نرجو منكم التكرم بإكمال البيانات على البوابة الإلكترونية وإعادة إرسال الطلب بحد أقصى : {model.IncubationAd.EndDate.Date }.  <br />"
                                        + "شاكرين ومقدرين اهتمامكم وتعاونكم، <br />"
                                        + " برنامج بناء القدرات.  <br />";
                }
            }
            if (_IncubationProjectProposal.IncubationProjectProposalStatu.NameEN == "Rejected")
            {
                if (_IncubationProjectProposal.IncubationAdvertising.IncubationType.NameEN == "Incubation")
                {
                    mailHelper.Subject = "تنويه";
                    mailHelper.Body = "المكرمين/ المنظمات غير الربحية                         الموقرين <br />"
                                        + "السلام عليكم ورحمة الله وبركاته، <br />"
                                        + "تهديكم مؤسسة الملك خالد أطيب التحيات،<br /> ويؤسفنا إبلاغكم بأنه تم رفض طلبكم للمشاركة في مشروع الاحتضان الكامل،<br /> وذلك للأسباب التالية :  <br />"
                                        + $" {feadback}  <br />"
                                        + $" نرجو منكم التكرم بإكمال البيانات على البوابة الإلكترونية وإعادة إرسال الطلب بحد أقصى : {model.IncubationAd.EndDate}.  <br />"
                                        + "شاكرين ومقدرين اهتمامكم وتعاونكم، <br />"
                                        + " برنامج بناء القدرات.  <br />";
                }
                else
                {
                    mailHelper.Subject = "تنويه";
                    mailHelper.Body = "المكرمين/ المنظمات غير الربحية                         الموقرين <br />"
                                        + "السلام عليكم ورحمة الله وبركاته، <br />"
                                        + "تهديكم مؤسسة الملك خالد أطيب التحيات،<br /> ويؤسفنا إبلاغكم بأنه تم رفض طلبكم للمشاركة في مشروع الاحتضان الجزئي،<br /> وذلك للأسباب التالية :  <br />"
                                        + $" {feadback}  <br />"
                                        + $" نرجو منكم التكرم بإكمال البيانات على البوابة الإلكترونية وإعادة إرسال الطلب بحد أقصى : {model.IncubationAd.EndDate}.  <br />"
                                        + "شاكرين ومقدرين اهتمامكم وتعاونكم، <br />"
                                        + " برنامج بناء القدرات.  <br />";
                }
            }
            if (_IncubationProjectProposal.IncubationProjectProposalStatu.NameEN == "Accepted")
            {
                if (_IncubationProjectProposal.IncubationAdvertising.IncubationType.NameEN == "Incubation")
                {
                    mailHelper.Subject = "تهنئة";
                    mailHelper.Body = "المكرمين/ المنظمات غير الربحية                         الموقرين <br />"
                                        + "السلام عليكم ورحمة الله وبركاته، <br />"
                                        + "تهديكم مؤسسة الملك خالد أطيب التحيات، <br /> ونحيطكم علماً بأنه تم قبول طلبكم للالتحاق في مشروع الاحتضان الكامل، <br /> وسيتم التواصل معكم لتحديد الخطوات القادمة. <br />"
                                        + "مع تمنياتنا لكم بالتوفيق، <br />"
                                        + " برنامج بناء القدرات.  <br />";
                }
                else
                {
                    mailHelper.Subject = "تهنئة";
                    mailHelper.Body = "المكرمين/ المنظمات غير الربحية                         الموقرين <br />"
                                        + "السلام عليكم ورحمة الله وبركاته، <br />"
                                        + "تهديكم مؤسسة الملك خالد أطيب التحيات، <br /> ونحيطكم علماً بأنه تم قبول طلبكم للالتحاق في مشروع الاحتضان الجزئي، <br /> وسيتم التواصل معكم لتحديد الخطوات القادمة. <br />"
                                        + "مع تمنياتنا لكم بالتوفيق، <br />"
                                        + " برنامج بناء القدرات.  <br />";
                }
            }

            var fundingSource = repository.GetByKey<FundingSource>(_IncubationProjectProposal.IncubationAdvertising.FundingSourceID);
            if (fundingSource.UseCustomThemes)
                mailHelper.Send($"?partner={fundingSource.Nickname}");
            else
                mailHelper.Send("");

            if (type == "" || type == null)
                return RedirectToAction("index", new { Type = "Incubation" });
            return RedirectToAction("index", new { Type = type });
        }

        [HttpGet]
        public JsonResult GetCorporationUserData(Guid UserId)
        {
            var _CorporateApplicationForm = repository.Get<CorporateApplicationForm>(f => f.FrontendUserID == UserId).Select(f => new { Picture = String.Format("data:image/gif;base64,{0}", Convert.ToBase64String(f.Picture)), f.FrontendUser.AspNetUser.Email, f.Name, f.Mission, f.Address, f.AuthorizationAuthority.AuthorizationAuthorityNameEN, CityNameEN = f.CityID.HasValue ? f.City.CityNameEN : "", f.ClassificationSector.ClassificationSectorNameEN, /*f.Code,*/ f.CorporateAdministratorExtension, f.CorporateAdministratorJobTitle, f.CorporateAdministratorMobileNumber, f.CorporateAdministratorName, f.CorporateAdministratorTelephoneNumber, f.CorporationsCategory.CorporationsCategoryNameEN, DateElection = f.DateElection.HasValue ? f.DateElection.Value.ToShortDateString() : "", f.Extension, f.FaxNumber, FoundedYear = f.FoundedYear, f.Governorate.GovernorateEN, f.History, f.InstagramAccount, f.Objectives, f.OfficialEmail, f.POBox, f.PostalCode, f.Program.ProgramName, f.Region.RegionNameEN, f.RegistrationNumber, f.SnapchatAccount, f.TaxNumber, f.TelephoneNumber, f.TwitterAccount, f.Vision, f.Website, f.YouTubeAccount }).ToList();
            return Json(_CorporateApplicationForm, JsonRequestBehavior.AllowGet);
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

        public FileResult DownloadIncubationAdvertisingAttachments(Guid id)
        {
            var attachmet = repository.GetByKey<IncubationAdvertisingAttachment>(id);
            byte[] fileBytes = System.IO.File.ReadAllBytes(attachmet.URL);
            string fileName = attachmet.Name;
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

    }
}