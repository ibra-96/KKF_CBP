using System;
using System.Web;
using System.Linq;
using System.Web.Mvc;
using AlphaPeople.Core;
using System.Configuration;
using AlphaPeople.Repository;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using AlfaPeople.KingKhalidFoundation.Data;
using AlfaPeople.KingKhalidFoundation.Data.Model;
using AlfaPeople.KingKhalidFoundation.Web.Global;
using AlfaPeople.KingKhalidFoundation.Web.Models;

namespace AlfaPeople.KingKhalidFoundation.Web.Controllers
{
    [Authorize(Roles = "Corporation")]
    public class EmployeeController : BaseController
    {
        private readonly CommonHelper helper;
        private readonly IRepository repository;
        private ApplicationUserManager _userManager;
        private ApplicationSignInManager _signInManager;
        private readonly string Logpath = ConfigurationManager.AppSettings["Log"].ToString();

        public EmployeeController()
        {
            repository = new Repository(new KingkhaledFoundationDB());
            helper = new CommonHelper();
        }

        public EmployeeController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }


        public ActionResult All()
        {
            var UserId = User.Identity.GetUserId();
            var fronendId = Guid.Parse(UserId);
            if (repository.GetQuery<FrontendUser>(f => f.FK_AspUser == UserId && f.CorporateApplicationForms.Any(g => g.CorporateApplicationStatu.ApplicantStatu.ApplicantStatusName == "Accepted")).ToList().Count != 0)
            {
                var img = Convert.ToBase64String(repository.GetQuery<CorporateApplicationForm>(f => f.FrontendUserID == fronendId).FirstOrDefault().Picture);
                ViewBag.imgSrc = String.Format("data:image/gif;base64,{0}", img);
                TempData["ParentFrontId"] = UserId;
                return View();
            }

            return RedirectToAction("CorporationProfile", "Home", new { Msg = App_GlobalResources.General.MsgApply });
        }


        // GET: Employee
        public ActionResult Index()
        {
            Guid FrontId = Guid.Parse(TempData["ParentFrontId"].ToString());
            TempData.Peek("ParentFrontId");
            return View(repository.GetQuery<FrontendUser>(f => f.ParentID == FrontId).ToList());
        }

        [HttpGet]
        public ActionResult Create()
        {
            TempData.Keep("ParentFrontId");
            return View();
        }

        [HttpPost]
        public ActionResult Create(RegisterEmployee Model)
        {
            Guid frontId = Guid.Parse(TempData["ParentFrontId"].ToString());
            TempData.Keep("ParentFrontId");

            if (ModelState.IsValid)
            {
                if (repository.Count<AspNetUser>(f => f.UserName == Model.Email) > 0)
                {
                    AddError("Username  Exists");
                    return Json(new { success = false, html = GlobalClass.RenderRazorViewToString(this, "Index", repository.GetQuery<FrontendUser>(f => f.ParentID == frontId).ToList()), message = "Username  Exists", style = "custome2" }, JsonRequestBehavior.AllowGet);
                }
                if (repository.Count<AspNetUser>(f => f.Email == Model.Email) > 0)
                {
                    AddError("Email  Exists");
                    return Json(new { success = false, html = GlobalClass.RenderRazorViewToString(this, "Index", repository.GetQuery<FrontendUser>(f => f.ParentID == frontId).ToList()), message = "Email  Exists", style = "custome2" }, JsonRequestBehavior.AllowGet);
                }

                var user = new ApplicationUser { UserName = Model.Username, Email = Model.Email };

                try
                {
                    var result = UserManager.Create(user, Model.Password);

                    if (result.Succeeded)
                    {
                        FrontendUser _FrontendUser = new FrontendUser();
                        _FrontendUser.UserID = Guid.NewGuid();
                        _FrontendUser.CreateDate = DateTime.Now;
                        _FrontendUser.FK_AspUser = user.Id;
                        _FrontendUser.AspNetUser = repository.GetByKey<AspNetUser>(user.Id);
                        _FrontendUser.IsActive = Model.IsActive;
                        _FrontendUser.IsApproved = true;
                        _FrontendUser.Password = Model.Password;
                        _FrontendUser.ParentID = frontId;
                        repository.Add<FrontendUser>(_FrontendUser);
                        SignInManager.UserManager.AddToRole(user.Id, "Corporation IndIvidual");
                        repository.UnitOfWork.SaveChanges();

                        return Json(new { success = true, html = GlobalClass.RenderRazorViewToString(this, "Index", repository.GetQuery<FrontendUser>(f => f.ParentID == frontId).ToList()), message = "Created Successfully", style = "custome" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        AddErrors(result);
                        return Json(new { success = false, html = GlobalClass.RenderRazorViewToString(this, "Index", repository.GetQuery<FrontendUser>(f => f.ParentID == frontId).ToList()), message = "Error Occurred", style = "custome2" }, JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception ex)
                {
                    SignInManager.UserManager.Delete(user);
                    string[] lines = { "Message : " + ex.Message, "InnerException : " + ((ex.InnerException == null) ? "" : ex.InnerException.ToString()), "Date : " + DateTime.Now.ToString(), "________________________________________________________/t /n" };
                    helper.LogException(lines, Logpath);
                    AddError(ex.Message.ToString());
                    return Json(new { success = false, html = GlobalClass.RenderRazorViewToString(this, "Index", repository.GetQuery<FrontendUser>(f => f.ParentID == frontId).ToList()), message = "Error" }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { success = false, html = GlobalClass.RenderRazorViewToString(this, "Index", repository.GetQuery<FrontendUser>(f => f.ParentID == frontId).ToList()), message = "Error occurred", style = "custome2" }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult Edit(Guid? id)
        {
            TempData.Keep("ParentFrontId");

            if (id == null)
                return View();

            FrontendUser FrontEnduser = repository.GetByKey<FrontendUser>(id);

            if (FrontEnduser == null)
                return View();

            return View(FrontEnduser);
        }

        [HttpPost]
        public ActionResult Edit(FrontendUser Model)
        {
            TempData.Keep("ParentFrontId");
            Guid frontId = Guid.Parse(TempData["ParentFrontId"].ToString());
            TempData.Keep("ParentFrontId");
            if (ModelState.IsValid)
            {
                AspNetUser _user = new AspNetUser();
                var resFront = repository.GetByKey<FrontendUser>(Model.UserID);
                resFront.Password = Model.Password;
                resFront.IsActive = Model.IsActive;

                _user = Model.AspNetUser;
                PasswordHasher hashpass = new PasswordHasher();
                _user.PasswordHash = hashpass.HashPassword(Model.Password);
                _user.UserName = Model.AspNetUser.UserName;
                repository.Update(_user);
                repository.Update(resFront);
                repository.UnitOfWork.SaveChanges();
                return Json(new { success = true, html = GlobalClass.RenderRazorViewToString(this, "Index", repository.GetQuery<FrontendUser>(f => f.ParentID == frontId).ToList()), message = "Updated Successfully", style = "custome" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false, html = GlobalClass.RenderRazorViewToString(this, "Index", repository.GetQuery<FrontendUser>(f => f.ParentID == frontId).ToList()), message = "Error occurred", style = "custome2" }, JsonRequestBehavior.AllowGet);
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

        private void AddError(string error)
        {

            ModelState.AddModelError("Errors", error);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("Errors", error);
            }
        }
    }
}