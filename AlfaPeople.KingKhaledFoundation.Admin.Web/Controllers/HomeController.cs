using System;
using System.Linq;
using System.Web.Mvc;
using AlphaPeople.Core;
using System.Configuration;
using AlphaPeople.Repository;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using AlfaPeople.KingKhalidFoundation.Data;
using AlfaPeople.KingKhalidFoundation.Data.Model;
using AlfaPeople.KingKhaledFoundation.Admin.Web.Helper;
using AlfaPeople.KingKhaledFoundation.Admin.Web.Models;

namespace AlfaPeople.KingKhaledFoundation.Admin.Web.Controllers
{
    [Authorize(Roles = "Admin, CB Manager, CB Supervisor, CB Analyst")]
    public class HomeController : BaseController
    {
        private readonly IRepository repository;
        private readonly CommonHelper helper;
        private readonly DateTime dateNow = DateTime.Now.Date;
        private readonly string Logpath = ConfigurationManager.AppSettings["Log"].ToString();

        public HomeController()
        {
            helper = new CommonHelper();
            repository = new Repository(new KingkhaledFoundationDB());
        }

        public ActionResult dashboard()
        {
          
            string ProgramType = "";
            if (User.IsInRole("CB Manager") || User.IsInRole("CB Supervisor") || User.IsInRole("CB Analyst"))
            {
                ProgramType = "Capacity Building";
            }

            ViewBag.lang = CultureHelper.CurrentCulture;
            dshboardVM _dshboardVM = new dshboardVM();

            _dshboardVM.lstCorporationRequestsVM = new List<Models.CorporationRequestsVM>();
            var LstFrontendUserNotApproved = (User.IsInRole("Admin") && string.IsNullOrWhiteSpace(ProgramType)) ?
                repository.Find<FrontendUser>(f => f.AspNetUser.AspNetRoles.Any(g => g.Name == "Corporation") && f.CorporateApplicationForms.Any(d => d.CorporateApplicationStatu.ApplicantStatu.ApplicantStatusName == "Pending")).ToList() :
                repository.Find<FrontendUser>(f => f.AspNetUser.AspNetRoles.Any(g => g.Name == "Corporation") && f.CorporateApplicationForms.Any(d => (d.Program.ProgramName == ProgramType) && d.CorporateApplicationStatu.ApplicantStatu.ApplicantStatusName == "Pending")).ToList();

            foreach (var FrontendUserNotApproved in LstFrontendUserNotApproved.OrderByDescending(o => o.CreateDate).Take(8))
            {
                CorporationRequestsVM _CorporationRequestsVM = new CorporationRequestsVM();
                var res = FrontendUserNotApproved.CorporateApplicationForms.Select(f => new { History = f.History, Name = f.Name, Pic = f.Picture }).FirstOrDefault();
                _CorporationRequestsVM.FronUserID = FrontendUserNotApproved.UserID;
                _CorporationRequestsVM.History = res.History;
                _CorporationRequestsVM.Pic = res.Pic;
                _CorporationRequestsVM.Name = res.Name;
                _dshboardVM.lstCorporationRequestsVM.Add(_CorporationRequestsVM);
            }
            return View(_dshboardVM);
        }
        [HttpGet]
        public JsonResult GetCorporationUserData(Guid UserId)
        {
            var _CorporateApplicationForm = repository.Get<CorporateApplicationForm>(f => f.FrontendUserID == UserId)
                .Select(f => new
                {
                    Picture = f.Picture != null ? string.Format("data:image/gif;base64,{0}", Convert.ToBase64String(f.Picture)) : null,
                    Email = f.FrontendUser?.AspNetUser?.Email ?? "",
                    f.Name,
                    f.Mission,
                    f.Address,
                    AuthorizationAuthorityName = f.AuthorizationAuthority != null
                        ? (CultureHelper.CurrentCulture == 3 ? f.AuthorizationAuthority.AuthorizationAuthorityNameAR : f.AuthorizationAuthority.AuthorizationAuthorityNameEN)
                        : "N/A",
                    CityName = f.CityID.HasValue ? (CultureHelper.CurrentCulture == 3 ? f.City?.CityNameAR : f.City?.CityNameEN) ?? "N/A" : "N/A",
                    ClassificationSectorName = f.ClassificationSector != null
                        ? (CultureHelper.CurrentCulture == 3 ? f.ClassificationSector.ClassificationSectorNameAR : f.ClassificationSector.ClassificationSectorNameEN)
                        : "N/A",
                    CorporateFieldOfWorkName = f.CorporateFieldOfWork != null
                        ? (CultureHelper.CurrentCulture == 3 ? f.CorporateFieldOfWork.CorporateFieldOfWorkNameAR : f.CorporateFieldOfWork.CorporateFieldOfWorkNameEN)
                        : "N/A",
                    CorporateGenderType = f.corporateGenderType.HasValue
                        ? EnumHelper<CorporateGenderType>.GetDisplayValue(f.corporateGenderType.Value)
                        : "N/A",
                    f.CorporateAdministratorExtension,
                    f.CorporateAdministratorJobTitle,
                    f.CorporateAdministratorMobileNumber,
                    f.CorporateAdministratorName,
                    f.CorporateAdministratorTelephoneNumber,
                    CorporationsCategoryName = f.CorporationsCategory != null
                        ? (CultureHelper.CurrentCulture == 3 ? f.CorporationsCategory.CorporationsCategoryNameAR : f.CorporationsCategory.CorporationsCategoryNameEN)
                        : "N/A",
                    AdminEmail = f.AdministratorEmail ?? "",
                    UserName = f.FrontendUser?.AspNetUser?.UserName ?? "",
                    DateElection = f.DateElection.HasValue ? f.DateElection.Value.ToShortDateString() : "N/A",
                    f.Extension,
                    f.FaxNumber,
                    FoundedYear = f.FoundedYear.HasValue ? f.FoundedYear.Value.ToShortDateString() : "N/A",
                    Governorate = f.Governorate != null
                        ? (CultureHelper.CurrentCulture == 3 ? f.Governorate.GovernorateAR : f.Governorate.GovernorateEN)
                        : "N/A",
                    f.History,
                    f.InstagramAccount,
                    f.Objectives,
                    f.OfficialEmail,
                    f.POBox,
                    f.PostalCode,
                    ProgramName = f.Program != null
                        ? (CultureHelper.CurrentCulture == 3 ? f.Program.ProgramNameAR : f.Program.ProgramName)
                        : "N/A",
                    RegionName = f.Region != null
                        ? (CultureHelper.CurrentCulture == 3 ? f.Region.RegionNameAR : f.Region.RegionNameEN)
                        : "N/A",
                    f.RegistrationNumber,
                    f.SnapchatAccount,
                    f.TaxNumber,
                    f.TelephoneNumber,
                    TwitterAccount = f.TwitterAccount ?? "",
                    f.Vision,
                    f.Website,
                    f.YouTubeAccount,
                    lstFile = f.CorporateApplicationFormAttachments
                        .Where(y => y.CorporateApplicationFormID == f.CorporateApplicationFormID)
                        .Select(y => new
                        {
                            id = y.AttachmentID,
                            name = y.Name ?? "Unnamed",
                            y.Type,
                            y.Size
                        }).ToList()
                }).ToList();

            var jsonResult = Json(_CorporateApplicationForm, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        //[HttpGet]
        //public JsonResult GetCorporationUserData(Guid UserId)
        //{
        //    var _CorporateApplicationForm = repository.Get<CorporateApplicationForm>(f => f.FrontendUserID == UserId).Select(f => new
        //    {
        //        Picture = string.Format("data:image/gif;base64,{0}",
        //    Convert.ToBase64String(f.Picture)),
        //        f.FrontendUser.AspNetUser.Email,
        //        f.Name,
        //        f.Mission,
        //        f.Address,
        //        AuthorizationAuthorityName = CultureHelper.CurrentCulture == 3 ? f.AuthorizationAuthority.AuthorizationAuthorityNameAR : f.AuthorizationAuthority.AuthorizationAuthorityNameEN,
        //        CityName = CultureHelper.CurrentCulture == 3 ? f.CityID.HasValue ? f.City.CityNameAR : "" : f.CityID.HasValue ? f.City.CityNameEN : "",
        //        ClassificationSectorName = (f.ClassificationSector != null) ? CultureHelper.CurrentCulture == 3 ? f.ClassificationSector.ClassificationSectorNameAR : f.ClassificationSector.ClassificationSectorNameEN : "",
        //        CorporateFieldOfWorkName = (f.CorporateFieldOfWork != null) ? CultureHelper.CurrentCulture == 3 ? f.CorporateFieldOfWork.CorporateFieldOfWorkNameAR : f.CorporateFieldOfWork.CorporateFieldOfWorkNameEN : "",
        //        CorporateGenderType = f.corporateGenderType.HasValue ? EnumHelper<CorporateGenderType>.GetDisplayValue(f.corporateGenderType.Value) : "",
        //        /*f.Code,*/
        //        f.CorporateAdministratorExtension,
        //        f.CorporateAdministratorJobTitle,
        //        f.CorporateAdministratorMobileNumber,
        //        f.CorporateAdministratorName,
        //        f.CorporateAdministratorTelephoneNumber,
        //        CorporationsCategoryName = CultureHelper.CurrentCulture == 3 ? f.CorporationsCategory.CorporationsCategoryNameAR : f.CorporationsCategory.CorporationsCategoryNameEN,
        //        AdminEmail = f.AdministratorEmail,
        //        UserName = f.FrontendUser.AspNetUser.UserName,
        //        DateElection = f.DateElection.HasValue ? f.DateElection.Value.ToShortDateString() : "",
        //        f.Extension,
        //        f.FaxNumber,
        //        FoundedYear = f.FoundedYear.Value.ToShortDateString(),
        //        Governorate = CultureHelper.CurrentCulture == 3 ? f.Governorate.GovernorateAR : f.Governorate.GovernorateEN,
        //        f.History,
        //        f.InstagramAccount,
        //        f.Objectives,
        //        f.OfficialEmail,
        //        f.POBox,
        //        f.PostalCode,
        //        ProgramName = CultureHelper.CurrentCulture == 3 ? f.Program.ProgramNameAR : f.Program.ProgramName,
        //        RegionName = CultureHelper.CurrentCulture == 3 ? f.Region.RegionNameAR : f.Region.RegionNameEN,
        //        f.RegistrationNumber,
        //        f.SnapchatAccount,
        //        f.TaxNumber,
        //        f.TelephoneNumber,
        //        TwitterAccount = f.TwitterAccount,
        //        f.Vision,
        //        f.Website,
        //        f.YouTubeAccount,
        //        lstFile = f.CorporateApplicationFormAttachments.Where(y => y.CorporateApplicationFormID == f.CorporateApplicationFormID).Select(y => new
        //        {
        //            id = y.AttachmentID,
        //            name = y.Name,
        //            y.Type,
        //            y.Size
        //        })
        //    }).ToList();
        //    var jsonResult = Json(_CorporateApplicationForm, JsonRequestBehavior.AllowGet);
        //    jsonResult.MaxJsonLength = 1073741824;
        //    return jsonResult;
        //}


        // تحسين الكود الخلفي مع الترحيل والتحميل المؤجل
        //    [HttpGet]
        //    public JsonResult GetCorporationUserDataForDashboardAndRegistration(Guid UserId)
        //    {
        //        var _CorporateApplicationForm = repository.Get<CorporateApplicationForm>(f => f.FrontendUserID == UserId)
        //.Select(f => new
        //{


        //Picture = string.Format("data:image/gif;base64,{0}",
        //        Convert.ToBase64String(f.Picture)),
        //            f.FrontendUser.AspNetUser.Email,
        //            f.Name,
        //            f.Mission,
        //            f.Address,
        //            AuthorizationAuthorityName = CultureHelper.CurrentCulture == 3 ? f.AuthorizationAuthority.AuthorizationAuthorityNameAR : f.AuthorizationAuthority.AuthorizationAuthorityNameEN,
        //            CityName = CultureHelper.CurrentCulture == 3 ? f.CityID.HasValue ? f.City.CityNameAR : "" : f.CityID.HasValue ? f.City.CityNameEN : "",
        //            ClassificationSectorName = (f.ClassificationSector != null) ? CultureHelper.CurrentCulture == 3 ? f.ClassificationSector.ClassificationSectorNameAR : f.ClassificationSector.ClassificationSectorNameEN : "",
        //            CorporateFieldOfWorkName = (f.CorporateFieldOfWork != null) ? CultureHelper.CurrentCulture == 3 ? f.CorporateFieldOfWork.CorporateFieldOfWorkNameAR : f.CorporateFieldOfWork.CorporateFieldOfWorkNameEN : "",
        //            CorporateGenderType = f.corporateGenderType.HasValue ? EnumHelper<CorporateGenderType>.GetDisplayValue(f.corporateGenderType.Value) : "",
        //            /*f.Code,*/
        //            f.CorporateAdministratorExtension,
        //            f.CorporateAdministratorJobTitle,
        //            f.CorporateAdministratorMobileNumber,
        //            f.CorporateAdministratorName,
        //            f.CorporateAdministratorTelephoneNumber,
        //            CorporationsCategoryName = CultureHelper.CurrentCulture == 3 ? f.CorporationsCategory.CorporationsCategoryNameAR : f.CorporationsCategory.CorporationsCategoryNameEN,
        //            AdminEmail = f.AdministratorEmail,
        //            UserName = f.FrontendUser.AspNetUser.UserName,
        //            DateElection = f.DateElection.HasValue ? f.DateElection.Value.ToShortDateString() : "",
        //            f.Extension,
        //            f.FaxNumber,
        //            FoundedYear = f.FoundedYear.Value.ToShortDateString(),
        //            Governorate = CultureHelper.CurrentCulture == 3 ? f.Governorate.GovernorateAR : f.Governorate.GovernorateEN,
        //            f.History,
        //            f.InstagramAccount,
        //            f.Objectives,
        //            f.OfficialEmail,
        //            f.POBox,
        //            f.PostalCode,
        //            ProgramName = CultureHelper.CurrentCulture == 3 ? f.Program.ProgramNameAR : f.Program.ProgramName,
        //            RegionName = CultureHelper.CurrentCulture == 3 ? f.Region.RegionNameAR : f.Region.RegionNameEN,
        //            f.RegistrationNumber,
        //            f.SnapchatAccount,
        //            f.TaxNumber,
        //            f.TelephoneNumber,
        //            TwitterAccount = f.TwitterAccount,
        //            f.Vision,
        //            f.Website,
        //            f.YouTubeAccount,
        //}).FirstOrDefault();


        //        return new JsonResult
        //        {
        //            Data = _CorporateApplicationForm,
        //            JsonRequestBehavior = JsonRequestBehavior.AllowGet,
        //            MaxJsonLength = int.MaxValue
        //        };
        //    }

        [HttpGet]
        public JsonResult GetCorporationUserDataForDashboardAndRegistration(Guid UserId)
        {
            var _CorporateApplicationForm = repository.Get<CorporateApplicationForm>(f => f.FrontendUserID == UserId)
                .Select(f => new
                {
                    Picture = f.Picture != null ? $"data:image/gif;base64,{Convert.ToBase64String(f.Picture)}" : null,
                    Email = f.FrontendUser?.AspNetUser?.Email ?? "N/A",
                    Name = f.Name ?? "N/A",
                    Mission = f.Mission ?? "N/A",
                    Address = f.Address ?? "N/A",
                    AuthorizationAuthorityName = f.AuthorizationAuthority != null
                        ? (CultureHelper.CurrentCulture == 3 ? f.AuthorizationAuthority.AuthorizationAuthorityNameAR : f.AuthorizationAuthority.AuthorizationAuthorityNameEN)
                        : "N/A",
                    CityName = f.CityID.HasValue
                        ? (CultureHelper.CurrentCulture == 3 ? f.City?.CityNameAR : f.City?.CityNameEN) ?? "N/A"
                        : "N/A",
                    ClassificationSectorName = f.ClassificationSector != null
                        ? (CultureHelper.CurrentCulture == 3 ? f.ClassificationSector.ClassificationSectorNameAR : f.ClassificationSector.ClassificationSectorNameEN)
                        : "N/A",
                    CorporateFieldOfWorkName = f.CorporateFieldOfWork != null
                        ? (CultureHelper.CurrentCulture == 3 ? f.CorporateFieldOfWork.CorporateFieldOfWorkNameAR : f.CorporateFieldOfWork.CorporateFieldOfWorkNameEN)
                        : "N/A",
                    CorporateGenderType = f.corporateGenderType.HasValue
                        ? EnumHelper<CorporateGenderType>.GetDisplayValue(f.corporateGenderType.Value)
                        : "N/A",
                    CorporateAdministratorExtension = f.CorporateAdministratorExtension ?? "N/A",
                    CorporateAdministratorJobTitle = f.CorporateAdministratorJobTitle ?? "N/A",
                    CorporateAdministratorMobileNumber = f.CorporateAdministratorMobileNumber ?? "N/A",
                    CorporateAdministratorName = f.CorporateAdministratorName ?? "N/A",
                    CorporateAdministratorTelephoneNumber = f.CorporateAdministratorTelephoneNumber ?? "N/A",
                    CorporationsCategoryName = f.CorporationsCategory != null
                        ? (CultureHelper.CurrentCulture == 3 ? f.CorporationsCategory.CorporationsCategoryNameAR : f.CorporationsCategory.CorporationsCategoryNameEN)
                        : "N/A",
                    AdminEmail = f.AdministratorEmail ?? "N/A",
                    UserName = f.FrontendUser?.AspNetUser?.UserName ?? "N/A",
                    DateElection = f.DateElection.HasValue ? f.DateElection.Value.ToShortDateString() : "N/A",
                    Extension = f.Extension ?? "N/A",
                    FaxNumber = f.FaxNumber ?? "N/A",
                    FoundedYear = f.FoundedYear.HasValue ? f.FoundedYear.Value.ToShortDateString() : "N/A",
                    Governorate = f.Governorate != null
                        ? (CultureHelper.CurrentCulture == 3 ? f.Governorate.GovernorateAR : f.Governorate.GovernorateEN)
                        : "N/A",
                    History = f.History ?? "N/A",
                    InstagramAccount = f.InstagramAccount ?? "N/A",
                    Objectives = f.Objectives ?? "N/A",
                    OfficialEmail = f.OfficialEmail ?? "N/A",
                    POBox = f.POBox ?? "N/A",
                    PostalCode = f.PostalCode ?? "N/A",
                    ProgramName = f.Program != null
                        ? (CultureHelper.CurrentCulture == 3 ? f.Program.ProgramNameAR : f.Program.ProgramName)
                        : "N/A",
                    RegionName = f.Region != null
                        ? (CultureHelper.CurrentCulture == 3 ? f.Region.RegionNameAR : f.Region.RegionNameEN)
                        : "N/A",
                    RegistrationNumber = f.RegistrationNumber ?? "N/A",
                    SnapchatAccount = f.SnapchatAccount ?? "N/A",
                    TaxNumber = f.TaxNumber ?? "N/A",
                    TelephoneNumber = f.TelephoneNumber ?? "N/A",
                    TwitterAccount = f.TwitterAccount ?? "N/A",
                    Vision = f.Vision ?? "N/A",
                    Website = f.Website ?? "N/A",
                    YouTubeAccount = f.YouTubeAccount ?? "N/A",
                })
                .FirstOrDefault();

            return new JsonResult
            {
                Data = _CorporateApplicationForm,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = int.MaxValue
            };
        }


        [HttpGet]
        public JsonResult GetCorporationFiles(Guid UserId)
        {
            // استرجاع جميع الملفات دون تقسيم
            var files = repository.Get<CorporateApplicationForm>(f => f.FrontendUserID == UserId)
                .SelectMany(f => f.CorporateApplicationFormAttachments.Select(y => new
                {
                    y.AttachmentID,
                    y.Name,
                    y.Type,
                    y.Size
                }))
                .OrderBy(f => f.AttachmentID)
                .ToList();

            return new JsonResult
            {
                Data = files,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = int.MaxValue // تخصيص الحد الأقصى
            };
        }



        [HttpGet]
        public JsonResult GetIndUserData(Guid UserId)
        {
            var _IndApplicationForm = repository.Get<IndividualApplicationForm>(f => f.FrontendUserID == UserId).Select(f => new { ProgramName = f.Program.ProgramName, Name = f.Name, birthdate = f.BirthDate.ToShortDateString(), Gender = f.Gender ? "Female" : "Male", IdentityNumber = f.IdentityNumber, WorkStartDate = f.WorkStartDate.ToShortDateString(), NationalityAR = f.CountriesAndNationality.NationalityAR, CompanyName = f.CompanyName, Position = f.Position, f.PositionDetails, f.Region.RegionNameEN, f.Governorate.GovernorateEN, f.City.CityNameEN, f.Address, f.PostalCode, f.POBox, f.MobileNumber, f.TelephoneNumber, f.Extension, f.FrontendUser.AspNetUser.UserName, f.FrontendUser.AspNetUser.Email }).ToList();
            return Json(_IndApplicationForm, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ConfirmCorUser(Guid FronUserID)
        {
            try
            {
                var _FronUser = repository.GetByKey<FrontendUser>(FronUserID);
                _FronUser.IsApproved = true;
                repository.Update(_FronUser);
                var CorUserId = repository.FindOne<CorporateApplicationForm>(f => f.FrontendUserID == FronUserID).CorporateApplicationFormID;

                var _CorStatus = repository.FindOne<CorporateApplicationStatu>(f => f.CorporateApplicationFormID == CorUserId);

                _CorStatus.ApplicantStatusID = repository.FindOne<ApplicantStatu>(f => f.ApplicantStatusName == "Accepted").ApplicantStatusID;
                var userId = User.Identity.GetUserId();
                Guid BackEndId = Guid.Parse(userId);
                _CorStatus.Fk_BackEndMakeAction = BackEndId;
                _CorStatus.DateTimeMakeAction = DateTime.Now;
                repository.Update(_CorStatus);
                repository.UnitOfWork.SaveChanges();
                string Message = CultureHelper.CurrentCulture == 3 ? "تمت العملية بنجاح" : "Operation Accomplished Successfully";
                var Program = repository.FindOne<CorporateApplicationForm>(f => f.FrontendUserID == FronUserID).Program.ProgramNameAR;
                string siAdditionalLine = Program == "الاستثمار الاجتماعي" ?
                    $"تهديكم مؤسسة الملك خالد أطيب التحيات، ونحيطكم علماً بأنه تم قبول تسجيلكم في برنامج { Program }، يرجى الدخول على المنصة الإلكترونية و مراجعة لوحة التحكم للتقديم على المنحة و تعبئة نموذج مقترح المشروع. <br/>"
                    : $"تهديكم مؤسسة الملك خالد أطيب التحيات، ونحيطكم علمًا بأنه تم قبول تسجيلكم مبدئيًا في برنامج { Program }.<br/>";


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
                return Json(Message, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult ConfirmIndUser(Guid FronUserID)
        {
            try
            {
                var _FronUser = repository.GetByKey<FrontendUser>(FronUserID);
                _FronUser.IsApproved = true;
                repository.Update<FrontendUser>(_FronUser);
                var IndUserId = repository.FindOne<IndividualApplicationForm>(f => f.FrontendUserID == FronUserID).IndividualApplicationFormID;

                var _IndStatus = repository.FindOne<IndividualApplicantStatu>(f => f.IndividualApplicationFormID == IndUserId);

                _IndStatus.ApplicantStatusID = repository.FindOne<ApplicantStatu>(f => f.ApplicantStatusName == "Accepted").ApplicantStatusID;

                repository.Update<IndividualApplicantStatu>(_IndStatus);
                repository.UnitOfWork.SaveChanges();
                string Message = "User Accepted Sucssessfully";
                return Json(Message, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult RejectCorUser(Guid FronUserID, string Feadback, string ReasonType)
        {
            try
            {
                var YourRadioButton = Request.Form["ReasonTypeName"];

                var _FronUser = repository.GetByKey<FrontendUser>(FronUserID);
                _FronUser.IsApproved = false;
                repository.Update<FrontendUser>(_FronUser);
                var CorUserId = repository.FindOne<CorporateApplicationForm>(f => f.FrontendUserID == FronUserID).CorporateApplicationFormID;

                var _CorStatus = repository.FindOne<CorporateApplicationStatu>(f => f.CorporateApplicationFormID == CorUserId);

                _CorStatus.ApplicantStatusID = repository.FindOne<ApplicantStatu>(f => f.ApplicantStatusName == "Rejected").ApplicantStatusID;
                _CorStatus.FeadBack = Feadback;
                _CorStatus.ResonTypeID = repository.GetQuery<ReasonType>(f => f.Name == ReasonType).FirstOrDefault().Id;
                var userId = User.Identity.GetUserId();
                Guid BackEndId = Guid.Parse(userId);
                _CorStatus.Fk_BackEndMakeAction = BackEndId;
                _CorStatus.DateTimeMakeAction = DateTime.Now;
                repository.Update<CorporateApplicationStatu>(_CorStatus);
                string Message = CultureHelper.CurrentCulture == 3 ? "تمت العملية بنجاح" : "Operation Accomplished Successfully";
                var Program = repository.FindOne<CorporateApplicationForm>(f => f.FrontendUserID == FronUserID).Program.ProgramNameAR;
                MailHelper mailHelper = new MailHelper();

                mailHelper.ToEmail = _FronUser.AspNetUser.Email;
                if (ReasonType == "Missing Data")
                {
                    mailHelper.Subject = $"استكمال متطلبات التسجيل في برنامج {Program}";
                    mailHelper.IsHtml = true;
                    mailHelper.Body = "المكرمين/ المنظمات غير الربحية                         الموقرين<br/>"
                        + "السلام عليكم ورحمة الله وبركاته، <br/>"
                        + "تهديكم مؤسسة الملك خالد أطيب التحيات، <br/> ونحيطكم علماً بأن نموذج التسجيل الخاص بكم تنقصه البيانات التالية:  <br/>"
                        + Feadback + "<br/>"
                        + "مع تمنياتنا لكم بالتوفيق،<br/>"
                        + $" برنامج  {Program}. <br/>";
                }
                else
                {
                    var frontend = repository.GetByKey<FrontendUser>(FronUserID);
                    frontend.IsApproved = false;
                    repository.Update<FrontendUser>(frontend);
                    mailHelper.Subject = "تنويه";
                    mailHelper.IsHtml = true;
                    mailHelper.Body = "المكرمين/ المنظمات غير الربحية                         الموقرين<br/>"
                      + "السلام عليكم ورحمة الله وبركاته، <br/>"
                      + "تهديكم مؤسسة الملك خالد أطيب التحيات، <br/> ويؤسفنا إبلاغكم بأنه تم رفض طلب تسجيلكم وذلك للأسباب التالية:  <br/>"
                      + Feadback + "<br/>"
                      + "مع تمنياتنا لكم بالتوفيق،<br/>"
                      + $" برنامج {Program}. <br/>";
                }

                mailHelper.Send("");
                repository.UnitOfWork.SaveChanges();

                return Json(Message, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult RejectIndUser(Guid FronUserID, string Feadback)
        {
            try
            {
                var _FronUser = repository.GetByKey<FrontendUser>(FronUserID);
                _FronUser.IsApproved = false;
                repository.Update<FrontendUser>(_FronUser);
                var IndUserId = repository.FindOne<IndividualApplicationForm>(f => f.FrontendUserID == FronUserID).IndividualApplicationFormID;

                var _IndStatus = repository.FindOne<IndividualApplicantStatu>(f => f.IndividualApplicationFormID == IndUserId);

                _IndStatus.ApplicantStatusID = repository.FindOne<ApplicantStatu>(f => f.ApplicantStatusName == "Rejected").ApplicantStatusID;
                _IndStatus.FeadBack = Feadback;
                repository.Update<IndividualApplicantStatu>(_IndStatus);
                repository.UnitOfWork.SaveChanges();
                string Message = "User Rejected Sucssessfully";
                return Json(Message, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        public FileResult Download(Guid id)
        {
            var attachmet = repository.GetByKey<CorporateApplicationFormAttachment>(id);
            byte[] fileBytes = System.IO.File.ReadAllBytes(attachmet.URL);
            string fileName = attachmet.Name;
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
        //27-3-2025
        // مثال: عرض ملف إقرار الالتزام (Preview وليس تحميل)
        // Admin - IncubationWorkshopsController
    
        [HttpGet]
        public ActionResult PreviewCommitmentFile(Guid id)
        {
            var attachment = repository.GetByKey<IncubationWorkshopAttachment>(id);
            if (attachment == null || !System.IO.File.Exists(attachment.URL))
                return HttpNotFound("الملف غير موجود");

            byte[] fileBytes = System.IO.File.ReadAllBytes(attachment.URL);
            string contentType = GetContentType(attachment.URL); // كما في دالتك الحالية

            return File(fileBytes, contentType);
        }

        //public ActionResult PreviewCommitmentFile(Guid id)
        //{
        //    var attachment = repository.GetByKey<IncubationWorkshopAttachment>(id);
        //    if (attachment == null)
        //        return HttpNotFound("الملف غير موجود");

        //    string filePath = Server.MapPath(attachment.URL); // هنا المسار النسبي صالح لأنك داخل مشروع Admin

        //    if (!System.IO.File.Exists(filePath))
        //        return HttpNotFound("الملف غير موجود على السيرفر");

        //    byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
        //    string contentType = GetContentType(filePath);

        //    return File(fileBytes, contentType);
        //}

        //1-6-2025
        [HttpGet]
        public ActionResult PreviewFile(Guid id)
        {
            // استرجاع المرفق من قاعدة البيانات باستخدام المعرف
            var attachment = repository.GetByKey<CorporateApplicationFormAttachment>(id);

            if (attachment == null || !System.IO.File.Exists(attachment.URL))
            {
                return HttpNotFound("The requested file does not exist.");
            }

            // قراءة بيانات الملف
            byte[] fileBytes = System.IO.File.ReadAllBytes(attachment.URL);

            // تحديد نوع المحتوى (Content-Type) بناءً على امتداد الملف
            string contentType = GetContentType(attachment.URL);

            // إعادة الملف مع نوع المحتوى المناسب
            return File(fileBytes, contentType);
        }
        //1-6-2025
        private string GetContentType(string filePath)
        {
            // الحصول على الامتداد من مسار الملف
            var extension = System.IO.Path.GetExtension(filePath).ToLowerInvariant();

            switch (extension)
            {
                case ".pdf":
                    return "application/pdf";
                case ".jpg":
                case ".jpeg":
                    return "image/jpeg";
                case ".png":
                    return "image/png";
                case ".gif":
                    return "image/gif";
                case ".txt":
                    return "text/plain";
                case ".doc":
                    return "application/msword";
                case ".docx":
                    return "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                case ".xls":
                    return "application/vnd.ms-excel";
                case ".xlsx":
                    return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                case ".ppt":
                    return "application/vnd.ms-powerpoint";
                case ".pptx":
                    return "application/vnd.openxmlformats-officedocument.presentationml.presentation";
                
                default:
                    return "application/octet-stream"; // نوع افتراضي للملفات غير المعروفة
            }
        }



    }
}