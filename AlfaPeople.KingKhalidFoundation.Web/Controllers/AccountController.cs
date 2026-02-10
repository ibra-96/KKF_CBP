using System;
using System.IO;
using System.Web;
using System.Linq;
using System.Web.Mvc;
using AlphaPeople.Core;
using System.Transactions;
using System.Configuration;
using System.Threading.Tasks;
using AlphaPeople.Repository;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using Microsoft.AspNet.Identity.Owin;
using AlfaPeople.KingKhalidFoundation.Data;
using AlfaPeople.KingKhalidFoundation.Web.Helper;
using AlfaPeople.KingKhalidFoundation.Web.Models;
using AlfaPeople.KingKhalidFoundation.Data.Model;

namespace AlfaPeople.KingKhalidFoundation.Web.Controllers
{
    [Authorize]
    public class AccountController : BaseController
    {
        private readonly CommonHelper helper;
        private readonly IRepository repository;
        private ApplicationUserManager _userManager;
        private ApplicationSignInManager _signInManager;
        private readonly string Logpath = ConfigurationManager.AppSettings["Log"].ToString();

        public AccountController()
        {
            repository = new Repository(new KingkhaledFoundationDB());
            helper = new CommonHelper();

        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string partner, string returnUrl)
        {
            // منع التخزين المؤقت
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
            Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            Response.Cache.AppendCacheExtension("must-revalidate, proxy-revalidate");

            ViewBag.ReturnUrl = returnUrl;

            if (!string.IsNullOrWhiteSpace(partner))
            {
                var fundingSource = repository.Get<FundingSource>(f => f.Nickname == partner && (f.RegistrationBackgroundPic != null && f.RegistrationBackgroundPic != "") && f.UseCustomThemes);
                if (fundingSource != null && fundingSource.Count() > 0)
                    ViewBag.BG = fundingSource.FirstOrDefault().RegistrationBackgroundPic;
                ViewBag.HideKKFLogo = fundingSource.FirstOrDefault().HideKKFLogo.ToString();
            }
            return View(new LoginViewModel());
        }

        //
        // POST: /Account/Login
        //[HttpPost]
        //[AllowAnonymous]
        ////[ValidateInput(false)]
        //[ValidateAntiForgeryToken]
        //public ActionResult Login(LoginViewModel model, string partner, string returnUrl)
        //{
        //    if (!string.IsNullOrWhiteSpace(partner))
        //    {
        //        var fundingSource = repository.Get<FundingSource>(f => f.Nickname == partner && (f.RegistrationBackgroundPic != null && f.RegistrationBackgroundPic != "") && f.UseCustomThemes);
        //        if (fundingSource != null && fundingSource.Count() > 0)
        //            ViewBag.BG = fundingSource.FirstOrDefault().RegistrationBackgroundPic;
        //        ViewBag.HideKKFLogo = fundingSource.FirstOrDefault().HideKKFLogo.ToString();
        //    }

        //    if (!ModelState.IsValid)
        //    {
        //        return View(model);
        //    }

        //    // This doesn't count login failures towards account lockout
        //    // To enable password failures to trigger account lockout, change to shouldLockout: true
        //    var result = SignInManager.PasswordSignIn(model.Username, model.Password, model.RememberMe, shouldLockout: false);
        //    switch (result)
        //    {
        //        case SignInStatus.Success:
        //            //  var userID = User.Identity.GetUserId();
        //            var userLogin = UserManager.FindByName(model.Username);
        //            if (repository.FindOne<AspNetUser>(f => f.Id == userLogin.Id).AspNetRoles.Select(f => f.Name).FirstOrDefault() == "Corporation")
        //            {
        //                var FronEndUserData = repository.FindOne<FrontendUser>(f => f.FK_AspUser == userLogin.Id);

        //                var id = FronEndUserData.CorporateApplicationForms.Select(f => f.CorporateApplicationFormID).First();
        //                var _corAppStateus = repository.FindOne<CorporateApplicationStatu>(f => f.CorporateApplicationFormID == id);
        //                if (FronEndUserData.IsActive == true)
        //                {

        //                    if (_corAppStateus.ApplicantStatu.ApplicantStatusName == "Accepted")
        //                    {
        //                        return RedirectToAction("dashboard", "home");

        //                    }
        //                    else if (_corAppStateus.ApplicantStatu.ApplicantStatusName == "Pending")
        //                    {
        //                        return RedirectToAction("CorporationProfile", "home", new { Msg = App_GlobalResources.General.MsgApply });// "Your request has been received, Please wait while the request is reviewed and answered." });
        //                    }
        //                    else if (_corAppStateus.ApplicantStatu.ApplicantStatusName == "Rejected")
        //                    {
        //                        string ReasonName = repository.GetByKey<ReasonType>(_corAppStateus.ResonTypeID).Name;

        //                        if (ReasonName == "Missing Data")
        //                        {
        //                            return RedirectToAction("UpdateCorporationProfile", "Account", new { Msg = _corAppStateus.FeadBack, id = userLogin.Id });

        //                        }
        //                        ModelState.AddModelError("", App_GlobalResources.General.LoginMsg1);

        //                        return View(model);

        //                    }
        //                    else
        //                    {
        //                        ModelState.AddModelError("", App_GlobalResources.General.LoginMsg1);
        //                        return View(model);

        //                    }
        //                }
        //                else
        //                {
        //                    ModelState.AddModelError("", App_GlobalResources.General.LoginMsg2);
        //                    return View(model);

        //                }
        //            }
        //            else if (repository.FindOne<AspNetUser>(f => f.Id == userLogin.Id).AspNetRoles.Select(f => f.Name).FirstOrDefault() == "IndIvidual")
        //            {
        //                var FronEndUserData = repository.FindOne<FrontendUser>(f => f.FK_AspUser == userLogin.Id);

        //                var id = FronEndUserData.IndividualApplicationForms.Select(f => f.IndividualApplicationFormID).First();
        //                var _indAppStateus = repository.FindOne<IndividualApplicantStatu>(f => f.IndividualApplicationFormID == id);
        //                if (FronEndUserData.IsActive == true)
        //                {
        //                    if (_indAppStateus.ApplicantStatu.ApplicantStatusName == "Accepted")
        //                    {
        //                        return RedirectToAction("dashboard", "home");

        //                    }
        //                    else if (_indAppStateus.ApplicantStatu.ApplicantStatusName == "Pending")
        //                    {
        //                        return RedirectToAction("IndIvidualProfile", "home");

        //                    }
        //                    else
        //                    {
        //                        ModelState.AddModelError("", App_GlobalResources.General.LoginMsg1);
        //                        return View(model);

        //                    }
        //                }
        //                else
        //                {
        //                    ModelState.AddModelError("", App_GlobalResources.General.LoginMsg2);
        //                    return View(model);

        //                }
        //            }
        //            else
        //            {
        //                ModelState.AddModelError("", App_GlobalResources.General.LoginMsg3);
        //                return View(model);
        //            }
        //        case SignInStatus.LockedOut:
        //            return View("Lockout");
        //        case SignInStatus.RequiresVerification:
        //            return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
        //        case SignInStatus.Failure:
        //        default:
        //            ModelState.AddModelError("", App_GlobalResources.General.LoginMsg3);
        //            return View(model);
        //    }
        //}
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string partner, string returnUrl)
        {
           // AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
         
            if (!string.IsNullOrWhiteSpace(partner))
            {
                var fundingSource = repository.Get<FundingSource>(f =>
                    f.Nickname == partner &&
                    !string.IsNullOrEmpty(f.RegistrationBackgroundPic) &&
                    f.UseCustomThemes
                );

                if (fundingSource != null && fundingSource.Any())
                {
                    ViewBag.BG = fundingSource.First().RegistrationBackgroundPic;
                    ViewBag.HideKKFLogo = fundingSource.First().HideKKFLogo.ToString();
                }
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (Request.IsAuthenticated)
            {
                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                Session.Clear();
                Session.Abandon();
            }
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
            Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            Response.Cache.AppendCacheExtension("must-revalidate, proxy-revalidate");


            var result = SignInManager.PasswordSignIn(model.Username, model.Password, model.RememberMe, shouldLockout: false);

            switch (result)
            {
                case SignInStatus.Success:
                    var userLogin = UserManager.FindByName(model.Username);

                    var userRole = repository
                        .FindOne<AspNetUser>(f => f.Id == userLogin.Id)
                        .AspNetRoles
                        .Select(f => f.Name)
                        .FirstOrDefault();

                    if (userRole == "Corporation")
                    {
                        var frontendUser = repository.FindOne<FrontendUser>(f => f.FK_AspUser == userLogin.Id);
                        if (frontendUser == null)
                        {
                            ModelState.AddModelError("", App_GlobalResources.General.LoginMsg1);
                            return View(model);
                        }

                        var corporateApplication = frontendUser.CorporateApplicationForms.FirstOrDefault();
                        if (corporateApplication == null || corporateApplication.IsDraft)
                        {
                            TempData["ErrorMessage"] = null;
                            TempData["SuccessMessage"] = null;

                            return RedirectToAction("CorporationRegistrationForm", "Account", new { id = userLogin.Id });
                        }

                        if (frontendUser.IsActive)
                        {
                            var applicationStatus = repository.FindOne<CorporateApplicationStatu>(
                                f => f.CorporateApplicationFormID == corporateApplication.CorporateApplicationFormID
                            );

                            if (applicationStatus == null)
                            {
                                return RedirectToAction("CorporationRegistrationForm", "Account", new { id = userLogin.Id });
                            }

                            if (applicationStatus.ApplicantStatu.ApplicantStatusName == "Accepted")
                            {
                                return RedirectToAction("Dashboard", "Home");
                            }
                            else if (applicationStatus.ApplicantStatu.ApplicantStatusName == "Pending")
                            {
                                return RedirectToAction("CorporationProfile", "Home", new { Msg = App_GlobalResources.General.MsgApply });
                            }
                            else if (applicationStatus.ApplicantStatu.ApplicantStatusName == "Rejected")
                            {
                                var reasonName = repository.GetByKey<ReasonType>(applicationStatus.ResonTypeID)?.Name;

                                if (reasonName == "Missing Data")
                                {
                                    return RedirectToAction("UpdateCorporationProfile", "Account", new { Msg = applicationStatus.FeadBack, id = userLogin.Id });
                                }

                                ModelState.AddModelError("", App_GlobalResources.General.LoginMsg1);
                                return View(model);
                            }
                            else
                            {
                                ModelState.AddModelError("", App_GlobalResources.General.LoginMsg1);
                                return View(model);
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("", App_GlobalResources.General.LoginMsg2);
                            return View(model);
                        }
                    }
                    else if (userRole == "IndIvidual")
                    {
                        var frontendUser = repository.FindOne<FrontendUser>(f => f.FK_AspUser == userLogin.Id);
                        if (frontendUser == null)
                        {
                            ModelState.AddModelError("", App_GlobalResources.General.LoginMsg1);
                            return View(model);
                        }

                        var individualApplication = frontendUser.IndividualApplicationForms.FirstOrDefault();
                        if (individualApplication == null || individualApplication.IsDraft)
                        {
                            return RedirectToAction("IndividualRegistrationForm", "Account", new { id = userLogin.Id });
                        }

                        var applicationStatus = repository.FindOne<IndividualApplicantStatu>(
                            f => f.IndividualApplicationFormID == individualApplication.IndividualApplicationFormID
                        );

                        if (frontendUser.IsActive)
                        {
                            if (applicationStatus.ApplicantStatu.ApplicantStatusName == "Accepted")
                            {
                                return RedirectToAction("Dashboard", "Home");
                            }
                            else if (applicationStatus.ApplicantStatu.ApplicantStatusName == "Pending")
                            {
                                return RedirectToAction("IndIvidualProfile", "Home");
                            }
                            else
                            {
                                ModelState.AddModelError("", App_GlobalResources.General.LoginMsg1);
                                return View(model);
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("", App_GlobalResources.General.LoginMsg2);
                            return View(model);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", App_GlobalResources.General.LoginMsg3);
                        return View(model);
                    }

                case SignInStatus.LockedOut:
                    return View("Lockout");

                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });

                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", App_GlobalResources.General.LoginMsg3);
                    return View(model);
            }
        }
        [HandleError]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult NotFound()
        {
            return View();
        }

        public FileResult Download(Guid id)
        {
            var attachmet = repository.GetByKey<CorporateApplicationFormAttachment>(id);
            byte[] fileBytes = System.IO.File.ReadAllBytes(attachmet.URL);
            string fileName = attachmet.Name;
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        [HttpGet]
        public JsonResult DeleteAttachment(Guid Id)
        {
            try
            {
                var Attachment = repository.GetByKey<CorporateApplicationFormAttachment>(Id);
                repository.Delete<CorporateApplicationFormAttachment>(Attachment);
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

        public ActionResult UpdateCorporationProfile(string Msg, Guid id)
        {
            ViewBag.Msg = Msg;
            var result = repository.GetQuery<CorporateApplicationForm>(f => f.FrontendUserID == id).FirstOrDefault();

            ViewBag.CorporationsCategoryID = new SelectList(repository.GetQuery<CorporationsCategory>(f => f.IsActive == true), "CorporationsCategoryID", CultureHelper.CurrentCulture != 3 ? "CorporationsCategoryNameEN" : "CorporationsCategoryNameAR", result.CorporationsCategoryID);
            ViewBag.CorporateFieldOfWorkID = new SelectList(repository.GetQuery<CorporateFieldOfWork>(f => f.IsActive == true), "CorporateFieldOfWorkID", CultureHelper.CurrentCulture != 3 ? "CorporateFieldOfWorkNameEN" : "CorporateFieldOfWorkNameAR", result.CorporateFieldOfWorkID);
            ViewBag.AuthorizationAuthorityID = new SelectList(repository.Get<AuthorizationAuthority>(f => f.IsActive == true), "AuthorizationAuthorityID", CultureHelper.CurrentCulture != 3 ? "AuthorizationAuthorityNameEN" : "AuthorizationAuthorityNameAR", result.AuthorizationAuthorityID);
            ViewBag.RegionID = new SelectList(repository.Get<Region>(f => f.IsActive == true), "RegionID", CultureHelper.CurrentCulture != 3 ? "RegionNameEN" : "RegionNameAR", result.RegionID);
            ViewBag.CityID = new SelectList(repository.GetQuery<City>(f => f.IsActive == true), "CityID", CultureHelper.CurrentCulture != 3 ? "CityNameEN" : "CityNameAR", result.CityID);
            ViewBag.GovernorateID = new SelectList(repository.GetQuery<Governorate>(f => f.IsActive == true), "GovernorateID", CultureHelper.CurrentCulture != 3 ? "GovernorateEN" : "GovernorateAR", result.GovernorateID);
            ViewBag.ClassificationSectorID = new SelectList(repository.Get<ClassificationSector>(f => f.IsActive == true), "ClassificationSectorID", CultureHelper.CurrentCulture != 3 ? "ClassificationSectorNameEN" : "ClassificationSectorNameAR", result.ClassificationSectorID);
            ViewBag.FoundedYear = result.FoundedYear.HasValue
    ? result.FoundedYear.Value.ToString("yyyy-MM-dd")
    : "";

            ViewBag.DateElection = result.DateElection != null ? result.DateElection.Value.ToString("yyyy-MM-dd") : DateTime.Now.ToString("yyyy-MM-dd");
            UpdateCorporationProfileVM res = new Models.UpdateCorporationProfileVM();
            res._CorporateApplicationForm = result;
            return View(res);
        }

        [HttpPost]
        public JsonResult CheckUserExistence(string UserName, string Email, string CurrentUserId)
        {
            bool userNameExists = repository.Count<AspNetUser>(
                f => f.UserName == UserName && f.Id != CurrentUserId) > 0;

            bool emailExists = repository.Count<AspNetUser>(
                f => f.Email == Email && f.Id != CurrentUserId) > 0;

            string message = "";

            if (userNameExists && emailExists)
            {
                message = "اسم المستخدم والبريد الإلكتروني موجودان بالفعل.";
            }
            else if (userNameExists)
            {
                message = "اسم المستخدم موجود بالفعل.";
            }
            else if (emailExists)
            {
                message = "البريد الإلكتروني موجود بالفعل.";
            }

            return Json(new
            {
                Success = !(userNameExists || emailExists),
                Message = message,
                UserNameExists = userNameExists,
                EmailExists = emailExists
            });
        }


        [HttpPost]
        [Authorize]
        public ActionResult UpdateCorporationProfile(UpdateCorporationProfileVM Model)
        {
            if (ModelState.IsValid)
            {
                // تحديث المستخدم
                var user = repository.GetByKey<AspNetUser>(Model._CorporateApplicationForm.FrontendUser.FK_AspUser);
                if (user != null)
                {
                    user.UserName = Model._CorporateApplicationForm.FrontendUser.AspNetUser.UserName;
                    user.Email = Model._CorporateApplicationForm.FrontendUser.AspNetUser.Email;
                    repository.Update<AspNetUser>(user);
                    repository.UnitOfWork.SaveChanges();
                }

                    byte[] array;
                using (MemoryStream ms = new MemoryStream())
                {
                    if (Model.file != null)
                    {
                        Model.file.InputStream.CopyTo(ms);
                        array = ms.GetBuffer();
                        Model._CorporateApplicationForm.Picture = array;
                    }
                }

                Model._CorporateApplicationForm.ProgramID = repository.FindOne<Program>(p => p.ProgramName == "Capacity Building").ProgramID;
                repository.Update<CorporateApplicationForm>(Model._CorporateApplicationForm);
                
                var statCor = repository.GetQuery<CorporateApplicationStatu>(d => d.CorporateApplicationFormID == Model._CorporateApplicationForm.CorporateApplicationFormID).FirstOrDefault();
                statCor.ResonTypeID = null;
                statCor.ApplicantStatusID = repository.GetQuery<ApplicantStatu>(f => f.ApplicantStatusName == "Pending").FirstOrDefault().ApplicantStatusID;
                repository.Update<CorporateApplicationStatu>(statCor);
                if (Model.files != null)
                {
                    string FolderName = Model._CorporateApplicationForm.FrontendUser.AspNetUser.UserName;
                    string path = Server.MapPath("~/Uploads/" + FolderName + "/");

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    for (int i = 0; i < Model.files.Count(); i++)
                    {
                        if (Model.files[i] != null)
                        {
                            if (Model.files[i].ContentType == "application/pdf" || Model.files[i].ContentType == "image/jpeg" || Model.files[i].ContentType == "image/png")
                            {
                                Model.files[i].SaveAs(path + Path.GetFileName(Model.files[i].FileName));
                                CorporateApplicationFormAttachment _Attach = new CorporateApplicationFormAttachment();
                                _Attach.AttachmentID = Guid.NewGuid();
                                _Attach.CorporateApplicationFormID = Model._CorporateApplicationForm.CorporateApplicationFormID;
                                _Attach.Name = Path.GetFileName(Model.files[i].FileName);
                                _Attach.ScreenName = "Attachment Corporation Registration Projects";
                                _Attach.Size = Model.files[i].ContentLength.ToString();
                                _Attach.URL = path + Path.GetFileName(Model.files[i].FileName);
                                _Attach.Type = Model.files[i].ContentType;
                                repository.Add(_Attach);
                            }
                            else
                            {
                                throw new HttpException(500, "Uploaded files must be image or pdf.");
                            }
                        }

                        ViewBag.Message = "File uploaded successfully.";
                    }

                    repository.UnitOfWork.SaveChanges();

                    var Program = repository.GetByKey<Program>(Model._CorporateApplicationForm.ProgramID);
                    MailHelper mailHelper = new MailHelper();

                    mailHelper.ToEmail = repository.GetByKey<AspNetUser>(User.Identity.GetUserId())?.Email;
                    mailHelper.Subject = "إحاطة";
                    mailHelper.IsHtml = true;
                    mailHelper.Body = "المكرمين/ المنظمات غير الربحية                         الموقرين<br/>"
                        + "السلام عليكم ورحمة الله وبركاته، <br/>"
                        + $"نتقدم لكم بالشكر على تعبئة نموذج التسجيل في برنامج {Program.ProgramNameAR}،<br/> ونحيطكم علماً بأنه تم استلام طلبكم، وسيتم إفادتكم في حال قبول تسجيلكم. <br/>"
                        + "مع التحية،<br/>"
                        + $" برنامج {Program.ProgramNameAR}. <br/>";
                    mailHelper.Send("");
                    return RedirectToAction("CorporationProfile", "home", new { Msg = App_GlobalResources.General.MsgApply });
                }
            }
            ViewBag.CorporationsCategoryID = new SelectList(repository.GetQuery<CorporationsCategory>(f => f.IsActive == true), "CorporationsCategoryID", CultureHelper.CurrentCulture != 3 ? "CorporationsCategoryNameEN" : "CorporationsCategoryNameAR", Model._CorporateApplicationForm.CorporationsCategoryID);
            ViewBag.CorporateFieldOfWorkID = new SelectList(repository.GetQuery<CorporateFieldOfWork>(f => f.IsActive == true), "CorporateFieldOfWorkID", CultureHelper.CurrentCulture != 3 ? "CorporateFieldOfWorkNameEN" : "CorporateFieldOfWorkNameAR", Model._CorporateApplicationForm.CorporateFieldOfWorkID);
            ViewBag.AuthorizationAuthorityID = new SelectList(repository.Get<AuthorizationAuthority>(f => f.IsActive == true), "AuthorizationAuthorityID", CultureHelper.CurrentCulture != 3 ? "AuthorizationAuthorityNameEN" : "AuthorizationAuthorityNameAR", Model._CorporateApplicationForm.AuthorizationAuthorityID);
            ViewBag.RegionID = new SelectList(repository.Get<Region>(f => f.IsActive == true), "RegionID", CultureHelper.CurrentCulture != 3 ? "RegionNameEN" : "RegionNameAR", Model._CorporateApplicationForm.RegionID);
            ViewBag.CityID = new SelectList(repository.GetQuery<City>(f => f.IsActive == true), "CityID", CultureHelper.CurrentCulture != 3 ? "CityNameEN" : "CityNameAR", Model._CorporateApplicationForm.CityID);
            ViewBag.GovernorateID = new SelectList(repository.GetQuery<Governorate>(f => f.IsActive == true), "GovernorateID", CultureHelper.CurrentCulture != 3 ? "GovernorateEN" : "GovernorateAR", Model._CorporateApplicationForm.GovernorateID);
            ViewBag.ClassificationSectorID = new SelectList(repository.Get<ClassificationSector>(f => f.IsActive == true), "ClassificationSectorID", CultureHelper.CurrentCulture != 3 ? "ClassificationSectorNameEN" : "ClassificationSectorNameAR", Model._CorporateApplicationForm.ClassificationSectorID);

            UpdateCorporationProfileVM res = new Models.UpdateCorporationProfileVM();
            res._CorporateApplicationForm = new CorporateApplicationForm();
            res._CorporateApplicationForm = repository.GetByKey<CorporateApplicationForm>(Model._CorporateApplicationForm.CorporateApplicationFormID);
            return View(res);
        }

     
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register(string partner)
        {



            if (Request.IsAuthenticated)
            {
                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                Session.Clear();
                Session.Abandon();
            
            }

            // منع الكاش
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));

            if (!string.IsNullOrWhiteSpace(partner))
            {
                var fundingSource = repository.Get<FundingSource>(f => f.Nickname == partner && (f.RegistrationBackgroundPic != null && f.RegistrationBackgroundPic != "") && f.UseCustomThemes);
                if (fundingSource != null && fundingSource.Count() > 0)
                    ViewBag.BG = fundingSource.FirstOrDefault().RegistrationBackgroundPic;
                ViewBag.HideKKFLogo = fundingSource.FirstOrDefault().HideKKFLogo.ToString();
            }
            return View(new RegisterVM() { _RegisterViewModel = new RegisterViewModel() });
        }

        //
        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterVM Model)
        {
             
            Model._RegisterViewModel.UserRoles = repository.Get<AspNetRole>(a => a.Name == "Corporation").FirstOrDefault().Id;
            if (ModelState.IsValid)
            {
                if (repository.Count<AspNetUser>(f => f.UserName == Model.Username) > 0)
                {
                    AddError("Username  Exists");
                    ViewBag.Name = new SelectList(repository.GetAll<AspNetRole>(), "Id", "Name");
                    return View();
                }
                if (repository.Count<AspNetUser>(f => f.Email == Model._RegisterViewModel.Email) > 0)
                {
                    AddError("Email  Exists");
                    ViewBag.Name = new SelectList(repository.GetAll<AspNetRole>(), "Id", "Name");
                    return View();
                }

                TempData["RegisterVM"] = Model;
                if (repository.GetByKey<AspNetRole>(Model._RegisterViewModel.UserRoles).Name == "Corporation")
                {
                    TempData["ErrorMessage"] = null;
                    TempData["SuccessMessage"] = null;
                    TempData["ActiveTab"] = 1;
                    return RedirectToAction("CorporationRegistrationForm", "Account");
                }
                else
                {
                    return RedirectToAction("IndIvidualRegistrationForm", "Account");
                }
            }

            ViewBag.Name = new SelectList(repository.GetAll<AspNetRole>(), "Id", "Name");
            return View();

        }

      


        //
        // GET: /Account/CorporationRegistrationForm
        [AllowAnonymous]
        public ActionResult CorporationRegistrationForm(Guid? id)
        {
           // AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            //TempData["ErrorMessage"] = null;
            //TempData["SuccessMessage"] = null;
            RegisterVM _RegisterVM;
            var userId = User.Identity.GetUserId();

            if (TempData["RegisterVM"] != null)
            {
                _RegisterVM = (RegisterVM)TempData["RegisterVM"];
                TempData.Keep("RegisterVM");
            }
            else
            {
                var user = repository.GetQuery<AspNetUser>().FirstOrDefault(u => u.Id == userId);
                if (user != null)
                {
                    _RegisterVM = new RegisterVM
                    {
                        Username = user.UserName,
                        _RegisterViewModel = new RegisterViewModel
                        {
                            Email = user.Email
                        }
                    };
                }
                else
                {
                    return RedirectToAction("register", "Account"); // إعادة التوجيه للتسجيل إذا لم يكن هناك بيانات

                }
            }
            //        var existingDraft = id.HasValue
            //? repository.GetQuery<CorporateApplicationForm>().FirstOrDefault(f => f.CorporateApplicationFormID == id.Value)
            //: repository.GetQuery<CorporateApplicationForm>().FirstOrDefault(f => f.FrontendUserID.ToString() == userId && f.IsDraft);

            var existingDraft = repository.GetQuery<CorporateApplicationForm>()
                .FirstOrDefault(f => f.FrontendUserID.ToString() == userId && f.IsDraft);

            List<CorporateApplicationFormAttachment> attachments = new List<CorporateApplicationFormAttachment>();
            if (existingDraft != null)
            {
                attachments = repository.GetQuery<CorporateApplicationFormAttachment>()
                    .Where(a => a.CorporateApplicationFormID == existingDraft.CorporateApplicationFormID)
                    .ToList();
            }
            CorporationRegistrationFormVM _CorporationRegistrationFormVM = new CorporationRegistrationFormVM
            {
                _RegisterVM = _RegisterVM,
                _CorporateApplicationForm = existingDraft ?? new CorporateApplicationForm(),
                Attachments = attachments

            };
            // جلب القيم المخزنة مسبقًا
            _CorporationRegistrationFormVM._CorporateApplicationForm.RegionID = existingDraft?.RegionID;
            _CorporationRegistrationFormVM._CorporateApplicationForm.GovernorateID = existingDraft?.GovernorateID;
            _CorporationRegistrationFormVM._CorporateApplicationForm.CityID = existingDraft?.CityID;
            if (existingDraft?.Picture != null)
            {
                string base64Image = Convert.ToBase64String(existingDraft.Picture);
                string imageSrc = $"data:image/png;base64,{base64Image}";
                ViewBag.ProfilePicture = imageSrc;
            }
          
            var categories = repository.GetQuery<CorporationsCategory>(f => f.IsActive == true)
    .Select(c => new SelectListItem
    {
        Value = c.CorporationsCategoryID.ToString(), // `Guid` يجب تحويله إلى `string`
        Text = CultureHelper.CurrentCulture != 3 ? c.CorporationsCategoryNameEN : c.CorporationsCategoryNameAR
    }).ToList();

            // تحديد العنصر المحدد مسبقًا فقط إذا كانت هناك مسودة
            var selectedCategory = existingDraft?.CorporationsCategoryID.ToString();

            ViewBag.CorporationsCategoryID = new SelectList(categories, "Value", "Text", selectedCategory);

            ViewBag.CorporateFieldOfWorkID = new SelectList(repository.GetQuery<CorporateFieldOfWork>(f => f.IsActive == true),
                "CorporateFieldOfWorkID",
                CultureHelper.CurrentCulture != 3 ? "CorporateFieldOfWorkNameEN" : "CorporateFieldOfWorkNameAR",
                existingDraft?.CorporateFieldOfWorkID);

            ViewBag.AuthorizationAuthorityID = new SelectList(repository.Get<AuthorizationAuthority>(f => f.IsActive == true),
                "AuthorizationAuthorityID",
                CultureHelper.CurrentCulture != 3 ? "AuthorizationAuthorityNameEN" : "AuthorizationAuthorityNameAR",
                existingDraft?.AuthorizationAuthorityID);
           
            

                // تحميل المناطق مع تعيين المنطقة المخزنة مسبقًا
                ViewBag.RegionID = new SelectList(
                    repository.GetQuery<Region>(r => r.IsActive),
                    "RegionID",
                    CultureHelper.CurrentCulture != 3 ? "RegionNameEN" : "RegionNameAR",
                    existingDraft?.RegionID
                );

            // تحميل المحافظات بناءً على المنطقة المخزنة
            //var governoratesQuery = repository.GetQuery<Governorate>().Where(f => f.IsActive);
            //if (existingDraft?.RegionID != null)
            //{
            //    governoratesQuery = governoratesQuery.Where(f => f.RegionID == existingDraft.RegionID);
            //}
            //var governorates = governoratesQuery.ToList();

            //ViewBag.GovernorateID = new SelectList(
            //        governorates,
            //        "GovernorateID",
            //        CultureHelper.CurrentCulture != 3 ? "GovernorateEN" : "GovernorateAR",
            //        existingDraft?.GovernorateID
            //    );
            var governoratesQuery = repository.GetQuery<Governorate>().Where(f => f.IsActive);
            if (existingDraft?.RegionID != null)
            {
                governoratesQuery = governoratesQuery.Where(f => f.RegionID == existingDraft.RegionID);
            }
            var governorates = governoratesQuery.ToList();

            ViewBag.GovernorateID = new SelectList(
                governorates,
                "GovernorateID",
                CultureHelper.CurrentCulture != 3 ? "GovernorateEN" : "GovernorateAR",
                existingDraft?.GovernorateID ?? Guid.Empty //  تأكد من أن القيمة الافتراضية ليست `null`
            );

            // تحميل المدن بناءً على المحافظة المخزنة
            var citiesQuery = repository.GetQuery<City>().Where(c => c.IsActive);
            if (existingDraft?.GovernorateID != null)
            {
                citiesQuery = citiesQuery.Where(c => c.GovernorateID == existingDraft.GovernorateID);
            }
            var cities = citiesQuery.ToList();

            ViewBag.CityID = new SelectList(
                    cities,
                    "CityID",
                    CultureHelper.CurrentCulture != 3 ? "CityNameEN" : "CityNameAR",
                    existingDraft?.CityID
                );

            //ViewBag.RegionID = new SelectList(repository.Get<Region>(f => f.IsActive == true),
            //    "RegionID",
            //    CultureHelper.CurrentCulture != 3 ? "RegionNameEN" : "RegionNameAR",
            //    existingDraft?.RegionID);



            //// تحميل المحافظات بناءً على المنطقة المحفوظة في قاعدة البيانات
            //if (existingDraft?.RegionID != null)
            //{
            //    var governorates = repository.GetQuery<Governorate>(f => f.IsActive == true && f.RegionID == existingDraft.RegionID).ToList();
            //    ViewBag.GovernorateID = new SelectList(
            //        governorates,
            //        "GovernorateID",
            //        CultureHelper.CurrentCulture != 3 ? "GovernorateEN" : "GovernorateAR",
            //        existingDraft?.GovernorateID // تحديد المحافظة المحفوظة تلقائيًا
            //    );

            //    // **ضمان تحديد المحافظة المخزنة** عند عدم العثور عليها
            //    if (!governorates.Any(g => g.GovernorateID == existingDraft?.GovernorateID))
            //    {
            //        existingDraft.GovernorateID = governorates.FirstOrDefault()?.GovernorateID;
            //    }
            //}

            //// تحميل المدن بناءً على المحافظة المحفوظة في قاعدة البيانات
            //if (existingDraft?.GovernorateID != null)
            //{
            //    var cities = repository.GetQuery<City>(f => f.IsActive == true && f.GovernorateID == existingDraft.GovernorateID).ToList();
            //    ViewBag.CityID = new SelectList(
            //        cities,
            //        "CityID",
            //        CultureHelper.CurrentCulture != 3 ? "CityNameEN" : "CityNameAR",
            //        existingDraft?.CityID // تحديد المدينة المحفوظة تلقائيًا
            //    );

            //    // **ضمان تحديد المدينة المخزنة** عند عدم العثور عليها
            //    if (!cities.Any(c => c.CityID == existingDraft?.CityID))
            //    {
            //        existingDraft.CityID = cities.FirstOrDefault()?.CityID;
            //    }
            //}

            //  معالجة القيم الفارغة بطريقة آمنة عند عدم وجود `existingDraft`
            //ViewBag.FoundedYear = existingDraft?.FoundedYear?.ToString("yyyy-MM-dd") ?? "";
            if (existingDraft?.DateElection != null)
            {
                ViewBag.DateElection = existingDraft.DateElection.HasValue ? existingDraft.DateElection.Value.ToString("yyyy-MM-dd") : "";
            }
            if (existingDraft?.FoundedYear != null)
            {
                ViewBag.FoundedYear = existingDraft.FoundedYear.HasValue ? existingDraft.FoundedYear.Value.ToString("yyyy-MM-dd") : "";
                // تحقق مما إذا كان التاريخ متاحًا أم لا}
            }


                //ViewBag.DateElection = existingDraft?.DateElection?.ToString("yyyy-MM-dd") ?? "";

                var corporationsCategoryID = existingDraft != null ? existingDraft.CorporationsCategoryID : Guid.Empty;

          
                        var classificationSectors = repository.GetQuery<ClassificationSector>()
                .Where(s => s.IsActive)
                .Select(s => new SelectListItem
                {
                    Value = s.ClassificationSectorID.ToString(),
                    Text = CultureHelper.CurrentCulture != 3 ? s.ClassificationSectorNameEN : s.ClassificationSectorNameAR
                }).ToList();
            // تحديد العنصر المحفوظ مسبقًا
            var selectedSector = existingDraft?.ClassificationSectorID.ToString();

            ViewBag.ClassificationSectorID = new SelectList(classificationSectors, "Value", "Text", selectedSector);

            // تحميل نوع المنظمة إذا كان مخزن مسبقًا
            if (existingDraft?.corporateGenderType != null)
            {
                ViewBag.CorporateGenderType = new SelectList(
                    Enum.GetValues(typeof(CorporateGenderType)).Cast<CorporateGenderType>().Select(e => new SelectListItem
                    {
                        Value = ((int)e).ToString(),
                        Text = e.ToString()
                    }),
                    "Value",
                    "Text",
                    ((int)existingDraft.corporateGenderType).ToString()
                );
            }
            else
            {
                ViewBag.CorporateGenderType = new SelectList(
                    Enum.GetValues(typeof(CorporateGenderType)).Cast<CorporateGenderType>().Select(e => new SelectListItem
                    {
                        Value = ((int)e).ToString(),
                        Text = e.ToString()
                    }),
                    "Value",
                    "Text"
                );
            }
           
            return View(_CorporationRegistrationFormVM);
        }



        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult CorporationRegistrationForm(CorporationRegistrationFormVM Model, string actionType)
        {

            //AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            var email = Model._RegisterVM._RegisterViewModel.Email.Trim().ToLower();
            var existingUser = UserManager.FindByEmail(email);

            
            bool issubmitForm = (actionType == "submitForm");
           
            if (string.IsNullOrEmpty(actionType))
            {
                LoadDropDownListCorporationRegistrationForm();
                TempData["ErrorMessage"] = "لم يتم تحديد نوع الإجراء (حفظ كمسودة أو إرسال الطلب).";
                return RedirectToAction("CorporationRegistrationForm");
            }
        
            bool isDraft = (actionType == "saveAsDraft");


            //using (TransactionScope scope = new TransactionScope())
            //{
            using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromMinutes(2) }))
            {
                bool isNewUser = false;
                string userId = null;
                ApplicationUser user = null;
                try
                {
                   

                    // التحقق مما إذا كان المستخدم مسجل دخول أو البحث عنه
                    if (User.Identity.IsAuthenticated)
                    {
                        userId = User.Identity.GetUserId();
                        user = UserManager.FindByEmail(Model._RegisterVM._RegisterViewModel.Email);
                    }
                    else
                    {
                        user = UserManager.FindByEmail(Model._RegisterVM._RegisterViewModel.Email);
                        if (user != null)
                        {
                            userId = user.Id;
                        }
                    }

                    // إنشاء مستخدم جديد إذا لم يكن موجودًا

                    if (user == null)
                    {
                        if (string.IsNullOrEmpty(Model._RegisterVM._RegisterViewModel.Password))
                        {
                           // throw new ArgumentException("كلمة المرور مطلوبة عند إنشاء الحساب لأول مرة.");
                        }

                        user = new ApplicationUser
                        {
                            UserName = Model._RegisterVM.Username,
                            Email = Model._RegisterVM._RegisterViewModel.Email
                        };
                       

                        var result = UserManager.Create(user, Model._RegisterVM._RegisterViewModel.Password);
                        if (!result.Succeeded)
                        {
                            LoadDropDownListCorporationRegistrationForm();
                            AddErrors(result);
                            return View(Model);
                        }

                        userId = user.Id;
                        isNewUser = true;

                        var roleName = repository.GetByKey<AspNetRole>(Model._RegisterVM._RegisterViewModel.UserRoles)?.Name;
                        if (!string.IsNullOrEmpty(roleName))
                        {
                            SignInManager.UserManager.AddToRole(user.Id, roleName);
                        }
                    }


                    // البحث عن `frontendUser`
                    var AspNetUser = repository.GetQuery<AspNetUser>().FirstOrDefault(f => f.UserName == user.UserName);
                    var frontendUser = repository.GetQuery<FrontendUser>().FirstOrDefault(f => f.FK_AspUser == user.Id);

                    if (frontendUser == null)
                    {


                        frontendUser = new FrontendUser
                        {
                            UserID = Guid.Parse(userId),
                            CreateDate = DateTime.Now,
                            FK_AspUser = user.Id,
                            IsActive = true,
                            IsApproved = false,
                            Password = Model._RegisterVM._RegisterViewModel.Password
                        };
                        repository.Add(frontendUser);
                        repository.UnitOfWork.SaveChanges();
                    }

                    // البحث عن مسودة قديمة
                    var existingDraft = repository.GetQuery<CorporateApplicationForm>()
                        .FirstOrDefault(f => f.FrontendUserID.ToString() == userId && f.IsDraft);

                    // البحث عن البرنامج
                    var program = repository.FindOne<Program>(p => p.ProgramName == "Capacity Building");
                    if (program == null)
                    {
                        LoadDropDownListCorporationRegistrationForm();
                        TempData["ErrorMessage"] = "لم يتم العثور على البرنامج المحدد.";
                        return View(Model);
                    }

                    // تحديد إذا كنا نقوم بتحديث مسودة أو إنشاء طلب جديد
                    bool isNewApplication = existingDraft == null;
                    CorporateApplicationForm applicationForm = existingDraft ?? new CorporateApplicationForm
                    {
                        CorporateApplicationFormID = isNewApplication ? Guid.NewGuid() : existingDraft.CorporateApplicationFormID,
                        FrontendUserID = frontendUser.UserID,
                        ProgramID = program.ProgramID
                    };

                    // تحويل المسودة إلى طلب نهائي إذا كان المستخدم طلب ذلك
                    if (!isDraft && existingDraft != null)
                    {
                        applicationForm.IsDraft = false;
                    }
                    else
                    {
                        applicationForm.IsDraft = isDraft;
                    }

                    // تحديث بيانات النموذج
                    //  تحديث بيانات `applicationForm`
                    //                        // تحديث كافة البيانات، مع السماح بأن تكون الحقول فارغة عند الحفظ كمسودة
                    applicationForm.Name = string.IsNullOrEmpty(Model._CorporateApplicationForm.Name) ? null : Model._CorporateApplicationForm.Name;
                    applicationForm.FoundedYear = Model._CorporateApplicationForm.FoundedYear.HasValue && Model._CorporateApplicationForm.FoundedYear.Value != DateTime.MinValue
                ? Model._CorporateApplicationForm.FoundedYear.Value
                : (DateTime?)null;

                    //applicationForm.FoundedYear = Model._CorporateApplicationForm.FoundedYear.Value == DateTime.MinValue ? (DateTime?)null : Model._CorporateApplicationForm.FoundedYear.Value;
                    applicationForm.RegistrationNumber = string.IsNullOrEmpty(Model._CorporateApplicationForm.RegistrationNumber) ? null : Model._CorporateApplicationForm.RegistrationNumber;
                    applicationForm.TaxNumber = string.IsNullOrEmpty(Model._CorporateApplicationForm.TaxNumber) ? null : Model._CorporateApplicationForm.TaxNumber;
                    applicationForm.History = string.IsNullOrEmpty(Model._CorporateApplicationForm.History) ? null : Model._CorporateApplicationForm.History;
                    applicationForm.Vision = string.IsNullOrEmpty(Model._CorporateApplicationForm.Vision) ? null : Model._CorporateApplicationForm.Vision;
                    applicationForm.Mission = string.IsNullOrEmpty(Model._CorporateApplicationForm.Mission) ? null : Model._CorporateApplicationForm.Mission;
                    applicationForm.Objectives = string.IsNullOrEmpty(Model._CorporateApplicationForm.Objectives) ? null : Model._CorporateApplicationForm.Objectives;
                    applicationForm.Address = string.IsNullOrEmpty(Model._CorporateApplicationForm.Address) ? null : Model._CorporateApplicationForm.Address;
                    applicationForm.PostalCode = string.IsNullOrEmpty(Model._CorporateApplicationForm.PostalCode) ? null : Model._CorporateApplicationForm.PostalCode;
                    applicationForm.POBox = string.IsNullOrEmpty(Model._CorporateApplicationForm.POBox) ? null : Model._CorporateApplicationForm.POBox;
                    applicationForm.Extension = string.IsNullOrEmpty(Model._CorporateApplicationForm.Extension) ? null : Model._CorporateApplicationForm.Extension;
                    applicationForm.TelephoneNumber = string.IsNullOrEmpty(Model._CorporateApplicationForm.TelephoneNumber) ? null : Model._CorporateApplicationForm.TelephoneNumber;
                    applicationForm.FaxNumber = string.IsNullOrEmpty(Model._CorporateApplicationForm.FaxNumber) ? null : Model._CorporateApplicationForm.FaxNumber;
                    applicationForm.OfficialEmail = string.IsNullOrEmpty(Model._CorporateApplicationForm.OfficialEmail) ? null : Model._CorporateApplicationForm.OfficialEmail;
                    applicationForm.AdministratorEmail = string.IsNullOrEmpty(Model._CorporateApplicationForm.AdministratorEmail) ? null : Model._CorporateApplicationForm.AdministratorEmail;
                    applicationForm.Website = string.IsNullOrEmpty(Model._CorporateApplicationForm.Website) ? null : Model._CorporateApplicationForm.Website;
                    applicationForm.TwitterAccount = string.IsNullOrEmpty(Model._CorporateApplicationForm.TwitterAccount) ? null : Model._CorporateApplicationForm.TwitterAccount;
                    applicationForm.YouTubeAccount = string.IsNullOrEmpty(Model._CorporateApplicationForm.YouTubeAccount) ? null : Model._CorporateApplicationForm.YouTubeAccount;
                    applicationForm.SnapchatAccount = string.IsNullOrEmpty(Model._CorporateApplicationForm.SnapchatAccount) ? null : Model._CorporateApplicationForm.SnapchatAccount;
                    applicationForm.InstagramAccount = string.IsNullOrEmpty(Model._CorporateApplicationForm.InstagramAccount) ? null : Model._CorporateApplicationForm.InstagramAccount;
                    applicationForm.CorporateAdministratorName = string.IsNullOrEmpty(Model._CorporateApplicationForm.CorporateAdministratorName) ? null : Model._CorporateApplicationForm.CorporateAdministratorName;
                    applicationForm.CorporateAdministratorJobTitle = string.IsNullOrEmpty(Model._CorporateApplicationForm.CorporateAdministratorJobTitle) ? null : Model._CorporateApplicationForm.CorporateAdministratorJobTitle;
                    applicationForm.CorporateAdministratorMobileNumber = string.IsNullOrEmpty(Model._CorporateApplicationForm.CorporateAdministratorMobileNumber) ? null : Model._CorporateApplicationForm.CorporateAdministratorMobileNumber;
                    applicationForm.CorporateAdministratorTelephoneNumber = string.IsNullOrEmpty(Model._CorporateApplicationForm.CorporateAdministratorTelephoneNumber) ? null : Model._CorporateApplicationForm.CorporateAdministratorTelephoneNumber;
                    applicationForm.CorporateAdministratorExtension = string.IsNullOrEmpty(Model._CorporateApplicationForm.CorporateAdministratorExtension) ? null : Model._CorporateApplicationForm.CorporateAdministratorExtension;

                    // 9️ القيم الافتراضية للمفاتيح الأجنبية عند الحفظ كمسودة
                    applicationForm.CorporationsCategoryID = Model._CorporateApplicationForm.CorporationsCategoryID == Guid.Empty ? (Guid?)null : Model._CorporateApplicationForm.CorporationsCategoryID;
                    applicationForm.CorporateFieldOfWorkID = Model._CorporateApplicationForm.CorporateFieldOfWorkID == Guid.Empty ? (Guid?)null : Model._CorporateApplicationForm.CorporateFieldOfWorkID;
                    applicationForm.ClassificationSectorID = Model._CorporateApplicationForm.ClassificationSectorID == Guid.Empty ? (Guid?)null : Model._CorporateApplicationForm.ClassificationSectorID;
                    applicationForm.AuthorizationAuthorityID = Model._CorporateApplicationForm.AuthorizationAuthorityID == Guid.Empty ? (Guid?)null : Model._CorporateApplicationForm.AuthorizationAuthorityID;

                    applicationForm.RegionID = isDraft && Model._CorporateApplicationForm.RegionID == Guid.Empty ?
                     (Guid?)null : Model._CorporateApplicationForm.RegionID;

                    applicationForm.GovernorateID = isDraft && Model._CorporateApplicationForm.GovernorateID == Guid.Empty ?
                        (Guid?)null : Model._CorporateApplicationForm.GovernorateID;

                    applicationForm.CityID = isDraft && Model._CorporateApplicationForm.CityID == Guid.Empty ?
                        (Guid?)null : Model._CorporateApplicationForm.CityID;
                    // ** إضافة القيم المفقودة**
                    applicationForm.DateElection = Model._CorporateApplicationForm?.DateElection.HasValue == true && Model._CorporateApplicationForm.DateElection.Value != DateTime.MinValue
        ? Model._CorporateApplicationForm.DateElection.Value
        : (DateTime?)null;


                    applicationForm.corporateGenderType = Model._CorporateApplicationForm?.corporateGenderType.HasValue == true
                        ? Model._CorporateApplicationForm.corporateGenderType
                        : (CorporateGenderType?)null;
                    //  حفظ صورة المؤسسة
                    if (Model.file != null && Model.file.ContentLength > 0)
                    {
                        if (Model.file.ContentType == "image/jpeg" || Model.file.ContentType == "image/png" || Model.file.ContentType == "image/gif")
                         {
                            using (MemoryStream ms = new MemoryStream())
                            {
                                Model.file.InputStream.CopyTo(ms);
                                applicationForm.Picture = ms.ToArray();
                            }
                          }
                       
                    }
                    if (!isDraft && applicationForm.Picture == null)
                    {
                        LoadDropDownListCorporationRegistrationForm();
                        TempData["ErrorMessage"] = "يرجى رفع شعار المؤسسة قبل إرسال الطلب النهائي.";
                        return RedirectToAction("CorporationRegistrationForm", "Account", new { id = user.Id });
                    }
                   

                    if (isNewApplication)
                        {
                            repository.Add(applicationForm);
                        }
                        else
                        {
                            repository.Update(applicationForm);
                        }

                        repository.UnitOfWork.SaveChanges();

                        // إضافة حالة الطلب عند الإرسال النهائي فقط
                        if (!isDraft)
                        {
                            var _CorStatus = new CorporateApplicationStatu
                            {
                                CorporateApplicationFormID = applicationForm.CorporateApplicationFormID,
                                ApplicantStatusID = repository.FindOne<ApplicantStatu>(f => f.ApplicantStatusName == "Pending").ApplicantStatusID,
                                DateTimeMakeAction = DateTime.Now
                            };
                            repository.Add(_CorStatus);
                        repository.UnitOfWork.SaveChanges();
                    }
       
                            if (Model.files != null && Model.files.Count() > 0)
                            {
                                if (Model.files[0] != null)
                                {
                                    string FolderName = Model._RegisterVM.Username;
                                    string path = Server.MapPath("~/Uploads/" + FolderName + "/");

                                    if (!Directory.Exists(path))
                                    {
                                        Directory.CreateDirectory(path);
                                    }

                                    for (int i = 0; i < Model.files.Count(); i++)
                                    {

                                string extension = Path.GetExtension(Model.files[i].FileName).ToLower();

                                if (extension != ".jpg" && extension != ".jpeg" && extension != ".png" && extension != ".gif" && extension != ".pdf")
                                {
                                    //continue;
                                    //throw new HttpException(500, "Uploaded files must be image or pdf.");
                                    LoadDropDownListCorporationRegistrationForm();
                                    TempData["ErrorMessage"] = "Uploaded files must be image or pdf.";
                                    ViewBag.Message = "Uploaded files must be image or pdf.";
                                    continue;
                                    //  return RedirectToAction("CorporationRegistrationForm", "Account", new { id = user.Id });
                                }

                                if (Model.files[i].ContentType == "application/pdf" || Model.files[i].ContentType == "image/jpeg" || Model.files[i].ContentType == "image/png")
                                        {
                                            Model.files[i].SaveAs(path + Path.GetFileName(Model.files[i].FileName));
                                            CorporateApplicationFormAttachment _Attach = new CorporateApplicationFormAttachment();
                                            _Attach.AttachmentID = Guid.NewGuid();
                                            _Attach.CorporateApplicationFormID = applicationForm.CorporateApplicationFormID;
                                            _Attach.Name = Path.GetFileName(Model.files[i].FileName);
                                            _Attach.ScreenName = "Attachment Corporation Registration Projects";
                                            _Attach.Size = Model.files[i].ContentLength.ToString();
                                            _Attach.URL = path + Path.GetFileName(Model.files[i].FileName);
                                            _Attach.Type = Model.files[i].ContentType;
                                            repository.Add(_Attach);
                                        }
                                ViewBag.Message = "File uploaded successfully.";
                                //else
                                //{
                                //    throw new HttpException(500, "Uploaded files must be image or pdf.");
                                //}
                            }
                                    repository.UnitOfWork.SaveChanges();
                                   
                                }
                        
                    }


                  

                            //repository.UnitOfWork.SaveChanges();
                        SignInManager.SignIn(user, isPersistent: false, rememberBrowser: false);
                        //  إرسال بريد التأكيد عند الإرسال النهائي
                        if (!isDraft)
                        {

                            MailHelper mailHelper = new MailHelper
                            {
                                ToEmail = Model._RegisterVM._RegisterViewModel.Email,
                                Subject = "إحاطة",
                                IsHtml = true,
                                Body = $@"
                                            <p>المكرمين/ المنظمات غير الربحية</p>
                                            <p>السلام عليكم ورحمة الله وبركاته،</p>
                                            <p>نتقدم لكم بالشكر على تعبئة نموذج التسجيل في برنامج {program.ProgramNameAR}.</p>
                                            <p>نحيطكم علماً بأنه تم استلام طلبكم، وسيتم إفادتكم في حال قبول تسجيلكم.</p>
                                            <p>مع التحية،</p>
                                            <p>برنامج {program.ProgramNameAR}.</p>"
                            };
                            mailHelper.Send("");
                        var corpName = applicationForm?.Name ?? "غير محدد";
                        var programNameAr = program?.ProgramNameAR ?? program?.ProgramName ?? "البرنامج";
                        var now = DateTime.Now;

                        MailHelper mailHelperToCBP = new MailHelper
                        {
                            ToEmail = "cbp@kkf.org.sa",
                            Subject = $"تسجيل جمعية جديدة - {corpName}",
                            IsHtml = true,
                            Body = $@"
                                <p>السادة فريق برنامج بناء القدرات المحترمين،</p>
                                <p>السلام عليكم ورحمة الله وبركاته،</p>

                                <p>نود إحاطتكم بأنه تم تسجيل جمعية جديدة على المنصة.</p>

                                <ul>
                                    <li><strong>اسم الجمعية:</strong> {corpName}</li>
                                    <li><strong>البرنامج:</strong> {programNameAr}</li>
                                    <li><strong>تاريخ ووقت التسجيل:</strong> {now:yyyy-MM-dd HH:mm}</li>
                                </ul>

                                <p>مع التحية،</p>"
                       
                        };

                    
                        mailHelperToCBP.Send("");
                    }



                        repository.UnitOfWork.SaveChanges();
                        scope.Complete();

                        TempData["SuccessMessage"] = isDraft ? "تم حفظ النموذج كمسودة بنجاح." : "تم إرسال الطلب بنجاح.";
                      
                        if (isDraft)
                        {
                            //var userLogin = UserManager.FindByName(model.Username);
                            return RedirectToAction("CorporationRegistrationForm", "Account", new { id = user.Id });
                           
                        }
                        else
                        {
                            // إذا تم الإرسال النهائي، انتقل إلى ملف المؤسسة
                            return RedirectToAction("CorporationProfile", "Home");
                        }
                    }
                    catch (Exception ex)
                    {

                    try
                    {
                        if (isNewUser && user != null)
                        {
                            // حذف جميع البيانات المرتبطة بالمستخدم في FrontendUser
                            var frontendUser = repository.GetQuery<FrontendUser>().FirstOrDefault(f => f.FK_AspUser == user.Id);
                            if (frontendUser != null)
                            {
                                repository.Delete(frontendUser);
                                repository.UnitOfWork.SaveChanges();
                            }

                            // حذف جميع السجلات المرتبطة بـ CorporateApplicationForm
                            var applications = repository.GetQuery<CorporateApplicationForm>().Where(f => f.FrontendUserID == frontendUser.UserID).ToList();
                            foreach (var app in applications)
                            {
                                repository.Delete(app);
                            }
                            repository.UnitOfWork.SaveChanges();

                            // حذف جميع السجلات المرتبطة بـ CorporateApplicationStatus
                            var appStatuses = repository.GetQuery<CorporateApplicationStatu>().Where(s => applications.Any(a => a.CorporateApplicationFormID == s.CorporateApplicationFormID)).ToList();
                            foreach (var status in appStatuses)
                            {
                                repository.Delete(status);
                            }
                            repository.UnitOfWork.SaveChanges();

                            // حذف جميع الأدوار (Roles)
                            var userRoles = UserManager.GetRoles(user.Id);
                            foreach (var role in userRoles)
                            {
                                UserManager.RemoveFromRole(user.Id, role);
                            }

                            // حذف جميع بيانات تسجيل الدخول المرتبطة
                            var logins = user.Logins.ToList();
                            foreach (var login in logins)
                            {
                                UserManager.RemoveLogin(user.Id, new UserLoginInfo(login.LoginProvider, login.ProviderKey));
                            }

                            // حذف جميع الـ Claims
                            var claims = user.Claims.ToList();
                            foreach (var claim in claims)
                            {
                                UserManager.RemoveClaim(user.Id, new System.Security.Claims.Claim(claim.ClaimType, claim.ClaimValue));
                            }

                            // حذف المستخدم
                            var result = UserManager.Delete(user);
                            if (!result.Succeeded)
                            {
                                throw new Exception($"فشل في حذف المستخدم: {string.Join(", ", result.Errors)}");
                            }
                        }

                        scope.Complete();
                    }
                    catch (Exception e)
                    {
                        scope.Dispose();
                        throw new Exception("خطأ أثناء محاولة حذف المستخدم بعد فشل حفظ النموذج: " + e.Message);
                    }

                    scope.Dispose();
                    LoadDropDownListCorporationRegistrationForm();
                    AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                       TempData["ErrorMessage"] = "حدث خطأ أثناء معالجة الطلب: ";
                    ////+ ex.Message
                    return View(Model);
                }
                }
   

        }



     


        ////
        ////// POST: /Account/CorporationRegistrationForm
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public ActionResult CorporationRegistrationForm(CorporationRegistrationFormVM Model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        if (Model.file != null && (Model.file.ContentLength > 0))
        //        {
        //            if (Model.file.ContentType == "image/jpeg" || Model.file.ContentType == "image/png" || Model.file.ContentType == "image/gif")
        //            {

        //                byte[] array;
        //                using (MemoryStream ms = new MemoryStream())
        //                {
        //                    Model.file.InputStream.CopyTo(ms);
        //                    array = ms.GetBuffer();
        //                }

        //                using (TransactionScope scope = new TransactionScope())
        //                {
        //                    var user = new ApplicationUser { UserName = Model._RegisterVM.Username, Email = Model._RegisterVM._RegisterViewModel.Email };
        //                    try
        //                    {
        //                        var result = UserManager.Create(user, Model._RegisterVM._RegisterViewModel.Password);
        //                        if (result.Succeeded)
        //                        {

        //                            Model._CorporateApplicationForm.ProgramID = repository.FindOne<Program>(p => p.ProgramName == "Capacity Building").ProgramID;
        //                            FrontendUser _FrontendUser = new FrontendUser();
        //                            _FrontendUser.UserID = Guid.Parse(user.Id);
        //                            _FrontendUser.CreateDate = DateTime.Now;
        //                            _FrontendUser.FK_AspUser = user.Id;
        //                            _FrontendUser.IsActive = true;
        //                            _FrontendUser.IsApproved = false;
        //                            _FrontendUser.Password = Model._RegisterVM._RegisterViewModel.Password;
        //                            Model._CorporateApplicationForm.CorporateApplicationFormID = Guid.NewGuid();
        //                            Model._CorporateApplicationForm.Picture = array;
        //                            Model._CorporateApplicationForm.FrontendUserID = _FrontendUser.UserID;
        //                            repository.Add(_FrontendUser);
        //                            repository.Add(Model._CorporateApplicationForm);

        //                            var _CorStatus = new CorporateApplicationStatu();
        //                            _CorStatus.CorporateApplicationFormID = Model._CorporateApplicationForm.CorporateApplicationFormID;
        //                            _CorStatus.ApplicantStatusID = repository.FindOne<ApplicantStatu>(f => f.ApplicantStatusName == "Pending").ApplicantStatusID;
        //                            _CorStatus.DateTimeMakeAction = DateTime.Now;
        //                            repository.Add(_CorStatus);

        //                            SignInManager.UserManager.AddToRole(user.Id, repository.GetByKey<AspNetRole>(Model._RegisterVM._RegisterViewModel.UserRoles).Name);


        //                            if (Model.files != null && Model.files.Count() > 0)
        //                            {
        //                                if (Model.files[0] != null)
        //                                {
        //                                    string FolderName = Model._RegisterVM.Username;
        //                                    string path = Server.MapPath("~/Uploads/" + FolderName + "/");

        //                                    if (!Directory.Exists(path))
        //                                    {
        //                                        Directory.CreateDirectory(path);
        //                                    }

        //                                    for (int i = 0; i < Model.files.Count(); i++)
        //                                    {
        //                                        if (Model.files[i].ContentType == "application/pdf" || Model.files[i].ContentType == "image/jpeg" || Model.files[i].ContentType == "image/png")
        //                                        {
        //                                            Model.files[i].SaveAs(path + Path.GetFileName(Model.files[i].FileName));
        //                                            CorporateApplicationFormAttachment _Attach = new CorporateApplicationFormAttachment();
        //                                            _Attach.AttachmentID = Guid.NewGuid();
        //                                            _Attach.CorporateApplicationFormID = Model._CorporateApplicationForm.CorporateApplicationFormID;
        //                                            _Attach.Name = Path.GetFileName(Model.files[i].FileName);
        //                                            _Attach.ScreenName = "Attachment Corporation Registration Projects";
        //                                            _Attach.Size = Model.files[i].ContentLength.ToString();
        //                                            _Attach.URL = path + Path.GetFileName(Model.files[i].FileName);
        //                                            _Attach.Type = Model.files[i].ContentType;
        //                                            repository.Add(_Attach);
        //                                        }
        //                                        else
        //                                        {
        //                                            throw new HttpException(500, "Uploaded files must be image or pdf.");
        //                                        }
        //                                    }
        //                                    ViewBag.Message = "File uploaded successfully.";
        //                                }
        //                                else
        //                                {
        //                                    AddErrors(result);
        //                                    SignInManager.UserManager.Delete(user);
        //                                    LoadDropDownListCorporationRegistrationForm();
        //                                    scope.Dispose();
        //                                    return View(Model);
        //                                }
        //                            }
        //                            else
        //                            {
        //                                AddErrors(result);
        //                                SignInManager.UserManager.Delete(user);
        //                                LoadDropDownListCorporationRegistrationForm();
        //                                scope.Dispose();
        //                                return View(Model);
        //                            }


        //                            repository.UnitOfWork.SaveChanges();
        //                            SignInManager.SignIn(user, isPersistent: false, rememberBrowser: false);
        //                            var Program = repository.GetByKey<Program>(Model._CorporateApplicationForm.ProgramID);
        //                            scope.Complete();
        //                            MailHelper mailHelper = new MailHelper();
        //                            mailHelper.ToEmail = Model._RegisterVM._RegisterViewModel.Email;
        //                            mailHelper.Subject = "إحاطة";
        //                            mailHelper.IsHtml = true;
        //                            mailHelper.Body = "المكرمين/ المنظمات غير الربحية                         الموقرين<br/>"
        //                                + "السلام عليكم ورحمة الله وبركاته، <br/>"
        //                                + $"نتقدم لكم بالشكر على تعبئة نموذج التسجيل في برنامج {Program.ProgramNameAR}،<br/> ونحيطكم علماً بأنه تم استلام طلبكم، وسيتم إفادتكم في حال قبول تسجيلكم. <br/>"
        //                                + "مع التحية،<br/>"
        //                                + $" برنامج {Program.ProgramNameAR}. <br/>";
        //                            mailHelper.Send("");
        //                            return RedirectToAction("CorporationProfile", "home", new { Msg = App_GlobalResources.General.MsgApply });
        //                        }
        //                        else
        //                        {
        //                            AddErrors(result);
        //                            SignInManager.UserManager.Delete(user);
        //                            LoadDropDownListCorporationRegistrationForm();
        //                            scope.Dispose();
        //                            return View(Model);
        //                        }
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        SignInManager.UserManager.Delete(user);
        //                        string[] lines = { "Message : " + ex.Message, "InnerException : " + ((ex.InnerException == null) ? "" : ex.InnerException.ToString()), "Date : " + DateTime.Now.ToString(), "________________________________________________________/t /n" };
        //                        helper.LogException(lines, Logpath);
        //                        AddError(ex.Message.ToString());
        //                        LoadDropDownListCorporationRegistrationForm();
        //                        scope.Dispose();
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                AddError("File Not Image");
        //            }
        //        }
        //        AddError("Please Upload Image ");

        //        LoadDropDownListCorporationRegistrationForm();
        //        return View(Model);
        //    }
        //    else
        //    {
        //        LoadDropDownListCorporationRegistrationForm();
        //        return View(Model);
        //    }
        //}

        public void LoadDropDownListCorporationRegistrationForm()
        {
            var userId = User.Identity.GetUserId();
            var existingDraft = repository.GetQuery<CorporateApplicationForm>()
                .FirstOrDefault(f => f.FrontendUserID.ToString() == userId && f.IsDraft);
            ////  تحميل الصورة الشخصية (إذا كانت موجودة في المسودة)
            if (existingDraft?.Picture != null)
            {
                string base64Image = Convert.ToBase64String(existingDraft.Picture);
                string imageSrc = $"data:image/png;base64,{base64Image}";
                ViewBag.ProfilePicture = imageSrc;
            }
            //تحميل القوائم المنسدلة
            ViewBag.CorporationsCategoryID = new SelectList(
                repository.GetQuery<CorporationsCategory>(f => f.IsActive == true),
                "CorporationsCategoryID",
                CultureHelper.CurrentCulture != 3 ? "CorporationsCategoryNameEN" : "CorporationsCategoryNameAR"
            );
            //        var categories = repository.GetQuery<CorporationsCategory>(f => f.IsActive == true)
            //.Select(c => new SelectListItem
            //{
            //    Value = c.CorporationsCategoryID.ToString(), // `Guid` يجب تحويله إلى `string`
            //    Text = CultureHelper.CurrentCulture != 3 ? c.CorporationsCategoryNameEN : c.CorporationsCategoryNameAR
            //}).ToList();

            // تحديد العنصر المحدد مسبقًا فقط إذا كانت هناك مسودة
            //var selectedCategory = existingDraft?.CorporationsCategoryID.ToString();

            //ViewBag.CorporationsCategoryID = new SelectList(categories, "Value", "Text", selectedCategory);
            if (existingDraft?.DateElection != null)
            {
                ViewBag.DateElection = existingDraft.DateElection.HasValue ? existingDraft.DateElection.Value.ToString("yyyy-MM-dd") : "";
            }
            if (existingDraft?.FoundedYear != null)
            {
                ViewBag.FoundedYear = existingDraft.FoundedYear.HasValue ? existingDraft.FoundedYear.Value.ToString("yyyy-MM-dd") : "";
                // تحقق مما إذا كان التاريخ متاحًا أم لا}
            }

            ViewBag.CorporateFieldOfWorkID = new SelectList(
                repository.GetQuery<CorporateFieldOfWork>(f => f.IsActive == true),
                "CorporateFieldOfWorkID",
                CultureHelper.CurrentCulture != 3 ? "CorporateFieldOfWorkNameEN" : "CorporateFieldOfWorkNameAR"
            );

            ViewBag.AuthorizationAuthorityID = new SelectList(
                repository.Get<AuthorizationAuthority>(f => f.IsActive == true),
                "AuthorizationAuthorityID",
                CultureHelper.CurrentCulture != 3 ? "AuthorizationAuthorityNameEN" : "AuthorizationAuthorityNameAR"
            );

            ViewBag.RegionID = new SelectList(repository.Get<Region>(f => f.IsActive == true),
              "RegionID",
              CultureHelper.CurrentCulture != 3 ? "RegionNameEN" : "RegionNameAR",
              existingDraft?.RegionID);



            // تحميل المحافظات بناءً على المنطقة المحفوظة في قاعدة البيانات
            if (existingDraft?.RegionID != null)
            {
                var governorates = repository.GetQuery<Governorate>(f => f.IsActive == true && f.RegionID == existingDraft.RegionID).ToList();
                ViewBag.GovernorateID = new SelectList(
                    governorates,
                    "GovernorateID",
                    CultureHelper.CurrentCulture != 3 ? "GovernorateEN" : "GovernorateAR",
                    existingDraft?.GovernorateID // تحديد المحافظة المحفوظة تلقائيًا
                );

                // **ضمان تحديد المحافظة المخزنة** عند عدم العثور عليها
                if (!governorates.Any(g => g.GovernorateID == existingDraft?.GovernorateID))
                {
                    existingDraft.GovernorateID = governorates.FirstOrDefault()?.GovernorateID;
                }
            }

            // تحميل المدن بناءً على المحافظة المحفوظة في قاعدة البيانات
            if (existingDraft?.GovernorateID != null)
            {
                var cities = repository.GetQuery<City>(f => f.IsActive == true && f.GovernorateID == existingDraft.GovernorateID).ToList();
                ViewBag.CityID = new SelectList(
                    cities,
                    "CityID",
                    CultureHelper.CurrentCulture != 3 ? "CityNameEN" : "CityNameAR",
                    existingDraft?.CityID // تحديد المدينة المحفوظة تلقائيًا
                );

                // **ضمان تحديد المدينة المخزنة** عند عدم العثور عليها
                if (!cities.Any(c => c.CityID == existingDraft?.CityID))
                {
                    existingDraft.CityID = cities.FirstOrDefault()?.CityID;
                }
            }

            //  تحميل قائمة تصنيف القطاع
            //ViewBag.ClassificationSectorID = new SelectList(
            //    repository.GetQuery<ClassificationSector>(f => f.IsActive == true),
            //    "ClassificationSectorID",
            //    CultureHelper.CurrentCulture != 3 ? "ClassificationSectorNameEN" : "ClassificationSectorNameAR",
            //    existingDraft?.ClassificationSectorID
            //);
            var classificationSectors = repository.GetQuery<ClassificationSector>()
    .Where(s => s.IsActive)
    .Select(s => new SelectListItem
    {
        Value = s.ClassificationSectorID.ToString(),
        Text = CultureHelper.CurrentCulture != 3 ? s.ClassificationSectorNameEN : s.ClassificationSectorNameAR
    }).ToList();

            ViewBag.ClassificationSectorID = new SelectList(classificationSectors, "Value", "Text", existingDraft?.ClassificationSectorID);

            // تحميل نوع المنظمة إذا كان مخزن مسبقًا
            if (existingDraft?.corporateGenderType != null)
            {
                ViewBag.CorporateGenderType = new SelectList(
                    Enum.GetValues(typeof(CorporateGenderType)).Cast<CorporateGenderType>().Select(e => new SelectListItem
                    {
                        Value = ((int)e).ToString(),
                        Text = e.ToString()
                    }),
                    "Value",
                    "Text",
                    ((int)existingDraft.corporateGenderType).ToString()
                );
            }
            else
            {
                ViewBag.CorporateGenderType = new SelectList(
                    Enum.GetValues(typeof(CorporateGenderType)).Cast<CorporateGenderType>().Select(e => new SelectListItem
                    {
                        Value = ((int)e).ToString(),
                        Text = e.ToString()
                    }),
                    "Value",
                    "Text"
                );
            }
        }

        //
        // GET: /Account/IndIvidualRegistrationForm
        [AllowAnonymous]
        public ActionResult IndIvidualRegistrationForm()
        {
            if (TempData["RegisterVM"] != null)
            {
                RegisterVM _RegisterVM = (RegisterVM)TempData["RegisterVM"];
                TempData.Keep("RegisterVM");
                IndIvidualRegistrationFormVM _IndIvidualRegistrationFormVM = new IndIvidualRegistrationFormVM();
                _IndIvidualRegistrationFormVM._RegisterVM = _RegisterVM;
                LoadDropDownListIndIvidualRegistrationForm();
                return View(_IndIvidualRegistrationFormVM);
            }
            else
            {
                return RedirectToAction("register", "Account");
            }
        }

        //
        // POST: /Account/IndIvidualRegistrationForm
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult IndIvidualRegistrationForm(IndIvidualRegistrationFormVM Model, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file != null && (file.ContentLength > 0))
                {
                    if (file.ContentType == "image/jpeg" || file.ContentType == "image/png" || file.ContentType == "image/gif")
                    {
                        byte[] array;
                        using (MemoryStream ms = new MemoryStream())
                        {
                            file.InputStream.CopyTo(ms);
                            array = ms.GetBuffer();
                        }

                        using (TransactionScope scope = new TransactionScope())
                        {
                            var user = new ApplicationUser { UserName = Model._RegisterVM.Username, Email = Model._RegisterVM._RegisterViewModel.Email };
                            try
                            {
                                var result = UserManager.Create(user, Model._RegisterVM._RegisterViewModel.Password);
                                if (result.Succeeded)
                                {

                                    FrontendUser _FrontendUser = new FrontendUser();
                                    _FrontendUser.UserID = Guid.Parse(user.Id);
                                    _FrontendUser.CreateDate = DateTime.Now;
                                    _FrontendUser.FK_AspUser = user.Id;
                                    _FrontendUser.IsActive = true;
                                    _FrontendUser.IsApproved = false;
                                    _FrontendUser.Password = Model._RegisterVM._RegisterViewModel.Password;
                                    Model._IndividualApplicationForm.IndividualApplicationFormID = Guid.NewGuid();
                                    Model._IndividualApplicationForm.Picture = array;
                                    Model._IndividualApplicationForm.FrontendUserID = _FrontendUser.UserID;
                                    repository.Add<FrontendUser>(_FrontendUser);
                                    repository.Add<IndividualApplicationForm>(Model._IndividualApplicationForm);

                                    IndividualApplicantStatu _IndStatus = new IndividualApplicantStatu();
                                    _IndStatus.IndividualApplicationFormID = Model._IndividualApplicationForm.IndividualApplicationFormID;
                                    _IndStatus.ApplicantStatusID = repository.FindOne<ApplicantStatu>(f => f.ApplicantStatusName == "Pending").ApplicantStatusID;
                                    repository.Add<IndividualApplicantStatu>(_IndStatus);

                                    SignInManager.UserManager.AddToRole(user.Id, repository.GetByKey<AspNetRole>(Model._RegisterVM._RegisterViewModel.UserRoles).Name);

                                    repository.UnitOfWork.SaveChanges();
                                    SignInManager.SignIn(user, isPersistent: false, rememberBrowser: false);
                                    scope.Complete();
                                    TempData["IndividualRegistrationFormVM"] = Model;
                                    return RedirectToAction("IndividualProfile", "Home");
                                }
                                else
                                {
                                    AddErrors(result);
                                    scope.Dispose();
                                    LoadDropDownListIndIvidualRegistrationForm();

                                    return View();
                                }
                            }
                            catch (Exception ex)
                            {
                                SignInManager.UserManager.Delete(user);
                                string[] lines = { "Message : " + ex.Message, "InnerException : " + ((ex.InnerException == null) ? "" : ex.InnerException.ToString()), "Date : " + DateTime.Now.ToString(), "________________________________________________________/t /n" };
                                helper.LogException(lines, Logpath);
                                AddError(ex.Message.ToString());
                                LoadDropDownListIndIvidualRegistrationForm();
                                scope.Dispose();
                            }
                        }
                    }
                    else
                    {
                        AddError("File Not Image");
                    }
                }
                AddError("Please Upload Image ");

                LoadDropDownListIndIvidualRegistrationForm();
                return View();
            }
            else
            {
                LoadDropDownListIndIvidualRegistrationForm();
                return View();
            }
        }

        public void LoadDropDownListIndIvidualRegistrationForm()
        {
            ViewBag.Gender = new SelectList(new List<object> { new { key = false, value = "Male" }, new { key = true, value = "Female" } }, "key", "value");
            ViewBag.Nationality = new SelectList(repository.GetAll<CountriesAndNationality>(), "CountriesAndNationalitiesID", "NationalityEN");
            ViewBag.RegionID = new SelectList(repository.Get<Region>(f => f.IsActive == true), "RegionID", "RegionNameEN");
        }

        [HttpGet]
        [AllowAnonymous]
        public JsonResult GetSectors(Guid category)
        {
            if (!string.IsNullOrWhiteSpace(category.ToString()) && category.ToString().Length != 0)
            {

                IEnumerable<SelectListItem> Sectors = repository.Get<ClassificationSector>(f => f.CorporationsCategoryID == category && f.IsActive == true)
                     .Select(n =>
                      new SelectListItem
                      {
                          Value = n.ClassificationSectorID.ToString(),
                          Text = CultureHelper.CurrentCulture != 3 ? n.ClassificationSectorNameEN : n.ClassificationSectorNameAR
                      }).ToList();

                return Json(new SelectList(Sectors, "Value", "Text"), JsonRequestBehavior.AllowGet);
            }
            return null;
        }

        [HttpGet]
        [AllowAnonymous]
        public JsonResult GetGovernorates(Guid Region)
        {
            if (!string.IsNullOrWhiteSpace(Region.ToString()) && Region.ToString().Length != 0)
            {

                IEnumerable<SelectListItem> Sectors = repository.Get<Governorate>(f => f.RegionID == Region && f.IsActive == true)
                     .Select(n =>
                      new SelectListItem
                      {
                          Value = n.GovernorateID.ToString(),
                          Text = CultureHelper.CurrentCulture != 3 ? n.GovernorateEN : n.GovernorateAR
                      }).ToList();

                return Json(new SelectList(Sectors, "Value", "Text"), JsonRequestBehavior.AllowGet);
            }
            return null;
        }

        [HttpGet]
        [AllowAnonymous]
        public JsonResult GetCities(Guid Governorate)
        {
            if (!string.IsNullOrWhiteSpace(Governorate.ToString()) && Governorate.ToString().Length != 0)
            {
                IEnumerable<SelectListItem> Sectors = repository.Get<City>(f => f.GovernorateID == Governorate && f.IsActive == true)
                     .Select(n =>
                      new SelectListItem
                      {
                          Value = n.CityID.ToString(),
                          Text = CultureHelper.CurrentCulture != 3 ? n.CityNameEN : n.CityNameAR
                      }).ToList();

                return Json(new SelectList(Sectors, "Value", "Text"), JsonRequestBehavior.AllowGet);
            }
            return null;
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ForgotPasswordStepOne(string partner)
        {
            if (!string.IsNullOrWhiteSpace(partner))
            {
                var fundingSource = repository.Get<FundingSource>(f => f.Nickname == partner && (f.RegistrationBackgroundPic != null && f.RegistrationBackgroundPic != "") && f.UseCustomThemes);
                if (fundingSource != null && fundingSource.Count() > 0)
                    ViewBag.BG = fundingSource.FirstOrDefault().RegistrationBackgroundPic;
                ViewBag.HideKKFLogo = fundingSource.FirstOrDefault().HideKKFLogo.ToString();
            }

            if (TempData.ContainsKey("FP_Msg"))
                ViewBag.Msg = TempData["FP_Msg"].ToString();
            TempData.Peek("FP_Msg");
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPasswordStepOne(string partner, ForgotPasswordViewModel model)
        {
            try
            {
                ViewBag.Msg = "";
                if (ModelState.IsValid)
                {
                    if (!string.IsNullOrWhiteSpace(model.Email))
                    {
                        var CorporateProfile = repository.GetQuery<CorporateApplicationForm>(f => f.FrontendUser.AspNetUser.Email == model.Email).FirstOrDefault();
                        if (CorporateProfile != null)
                        {
                            TempData["CAF_ID"] = CorporateProfile.CorporateApplicationFormID;
                            if (string.IsNullOrWhiteSpace(partner))
                                return RedirectToAction("ForgotPasswordStepTwo");
                            return RedirectToAction("ForgotPasswordStepTwo", new { partner = partner });
                        }
                    }
                }
                ViewBag.Msg = App_GlobalResources.General.EmailNotExist;
                return View(model);
            }
            catch (Exception)
            {
                ViewBag.Msg = App_GlobalResources.General.ErorrMsg;
                return View(model);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ForgotPasswordStepTwo(string partner)
        {
            if (!string.IsNullOrWhiteSpace(partner))
            {
                var fundingSource = repository.Get<FundingSource>(f => f.Nickname == partner && (f.RegistrationBackgroundPic != null && f.RegistrationBackgroundPic != "") && f.UseCustomThemes);
                if (fundingSource != null && fundingSource.Count() > 0)
                    ViewBag.BG = fundingSource.FirstOrDefault().RegistrationBackgroundPic;
                ViewBag.HideKKFLogo = fundingSource.FirstOrDefault().HideKKFLogo.ToString();
            }

            try
            {
                Guid Id = Guid.Empty;
                if (TempData.ContainsKey("CAF_ID"))
                    Id = Guid.Parse(TempData["CAF_ID"].ToString());
                TempData.Keep();

                ViewBag.Msg = "";
                if (Id != Guid.Empty && Id != null)
                {
                    var CorporateProfile = repository.GetByKey<CorporateApplicationForm>(Id);
                    if (CorporateProfile != null)
                        return View(new CustomForgotPasswordVM() { CorpName = CorporateProfile.Name, imgSrc = string.Format("data:image/gif;base64,{0}", Convert.ToBase64String(CorporateProfile.Picture)) });
                }
                TempData["FP_Msg"] = App_GlobalResources.General.EmailNotExist;
                return RedirectToAction("ForgotPasswordStepOne");
            }
            catch (Exception)
            {
                TempData["FP_Msg"] = App_GlobalResources.General.ErorrMsg;
                return RedirectToAction("ForgotPasswordStepOne");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPasswordStepTwo(string partner, CustomForgotPasswordVM model)
        {
            try
            {
                Guid Id = Guid.Empty;
                if (TempData.ContainsKey("CAF_ID"))
                    Id = Guid.Parse(TempData["CAF_ID"].ToString());

                ViewBag.Msg = "";
                if (ModelState.IsValid)
                {
                    if (Id != Guid.Empty && !string.IsNullOrWhiteSpace(model.CorpName) && !string.IsNullOrWhiteSpace(model.CorpRegNum))
                    {
                        var CorporateProfile = repository.GetQuery<CorporateApplicationForm>(c => c.CorporateApplicationFormID == Id && c.Name == model.CorpName && c.RegistrationNumber == model.CorpRegNum).FirstOrDefault();
                        if (CorporateProfile != null)
                        {
                            var user = UserManager.FindByName(CorporateProfile.FrontendUser.AspNetUser.UserName);
                            if (user == null)
                            {
                                TempData.Keep("CAF_ID");
                                ViewBag.Msg = App_GlobalResources.General.ErorrMsg;
                                return View(model);
                            }
                            string code = UserManager.GeneratePasswordResetToken(user.Id);
                            var link = Url.Action("ResetPassword", "Account", new { UserId = user.Id, Code = code }, protocol: Request.Url.Scheme);

                            MailHelper mailHelper = new MailHelper()
                            {
                                ToEmail = user.Email,
                                Subject = "إفادة",
                                //Body = "السادة الكرام، <br/> السلام عليكم ورحمة الله وبركاته، <br/> تهديكم مؤسسة الملك خالد أطيب التحيات، <br />"
                                //     + "تم إرسال هذا البريد الإلكتروني لإعادة تعيين كلمة المرور الخاصة بكم، <br/> إذا لم تقم بذلك، يرجى تجاهل هذا البريد الإلكتروني والاتصال بنا. <br/> "
                                //     + $"لإعادة تعيين كلمة المرور الخاصة بكم يرجي النقر  <a style='font-size:16px; font-weight: bold; text-decoration:none; color:#C0a979;' target='_blank' href='{link}'>هنا!</a>"
                                //     + $"<br/><br/><br/>"
                                //     + "مع أطيب التحيات، <br/> مؤسسة الملك خالد.",
                              

                                Body = "السلام عليكم ورحمة الله وبركاته، <br/>"
                                + "تهديكم مؤسسة الملك خالد أطيب التحيات، <br/>"
                                + "تم استلام طلبكم لإعادة تعيين كلمة المرور الخاصة بكم، <br/>"
                                + $"لإعادة تعيين كلمة المرور الخاصة بكم يرجى النقر <a style='font-size:16px; font-weight: bold; text-decoration:none; color:#C0a979;' target='_blank' href='{link}'>هنا!</a> <br/>"
                                + "إذا لم تطلب إعادة تعيين كلمة المرور، يرجى تجاهل هذا البريد الإلكتروني والاتصال بنا. <br/>"
                                + "مع أطيب التحيات، <br/>"
                                + "مؤسسة الملك خالد. <br/>",

                                IsHtml = true
                            };

                            mailHelper.Send("");
                            TempData.Peek("CAF_ID");
                            TempData["ProsIsDone"] = true;
                            if (string.IsNullOrWhiteSpace(partner))
                                return RedirectToAction("ForgotPasswordStepThree");
                            return RedirectToAction("ForgotPasswordStepThree", new { partner = partner });
                        }
                    }
                }
                TempData.Keep("CAF_ID");
                ViewBag.Msg = App_GlobalResources.General.DoesntMatch;
                return View(model);
            }
            //catch (Exception ex)
            catch (Exception)
            {
                TempData.Keep("CAF_ID");
                ViewBag.Msg = App_GlobalResources.General.ErorrMsg;
                return View(model);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ForgotPasswordStepThree(string partner)
        {
            if (!string.IsNullOrWhiteSpace(partner))
            {
                var fundingSource = repository.Get<FundingSource>(f => f.Nickname == partner && (f.RegistrationBackgroundPic != null && f.RegistrationBackgroundPic != "") && f.UseCustomThemes);
                if (fundingSource != null && fundingSource.Count() > 0)
                    ViewBag.BG = fundingSource.FirstOrDefault().RegistrationBackgroundPic;
                ViewBag.HideKKFLogo = fundingSource.FirstOrDefault().HideKKFLogo.ToString();
            }

            try
            {
                if (TempData.ContainsKey("ProsIsDone"))
                {
                    if (bool.Parse(TempData.ContainsKey("ProsIsDone").ToString()))
                    {
                        TempData.Clear();
                        return View();
                    }
                }
                TempData["FP_Msg"] = App_GlobalResources.General.ErorrMsg;
                return RedirectToAction("ForgotPasswordStepOne");
            }
            catch (Exception)
            {
                TempData["FP_Msg"] = App_GlobalResources.General.ErorrMsg;
                return RedirectToAction("ForgotPasswordStepOne");
            }
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                // return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string UserId, string Code)
        {
            return Code == null || UserId == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = UserManager.FindById(model.UserId);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                AddError("Invalid Token");
                return View();
            }
            var result = UserManager.ResetPassword(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                var fUser = repository.GetByKey<FrontendUser>(Guid.Parse(user.Id));
                fUser.Password = model.Password;
                repository.Update(fUser);
                repository.UnitOfWork.SaveChanges();

                return RedirectToAction("LogIn", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("register");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("Errors", error);
            }
        }
        private void AddError(string error)
        {

            ModelState.AddModelError("Errors", error);

        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}