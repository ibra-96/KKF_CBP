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
using AlfaPeople.KingKhaledFoundation.Admin.Web.Models;
using AlfaPeople.KingKhaledFoundation.Admin.Web.Helper;

namespace AlfaPeople.KingKhaledFoundation.Admin.Web.Controllers
{
    [Authorize(Roles = "Admin, CB Manager")]
    public class EmployeeController : BaseController
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private readonly IRepository repository;
        private readonly CommonHelper helper;
        private readonly string Logpath = ConfigurationManager.AppSettings["Log"].ToString();

        public EmployeeController()
        {
            helper = new CommonHelper();
            repository = new Repository(new KingkhaledFoundationDB());
        }

        public EmployeeController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ActionResult All()
        {
            return View();
        }

        public ActionResult Index()
        {
            var AllEmp = repository.GetAll<BackendUser>().ToList();
            return View(AllEmp);
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.Role = new SelectList(repository.Find<AspNetRole>(f => f.Name != "Corporation" && f.Name != "Corporation IndIvidual" && f.Name != "IndIvidual"), "Name", "Name");
            ViewBag.Position = new SelectList(repository.GetQuery<BackendUserPosition>(f => f.IsActive == true), "BackendUserPositionID", CultureHelper.CurrentCulture != 3 ? "NameEN" : "NameAR");
            return View();
        }

        [HttpPost]
        public ActionResult Create(EmployeeVM model)
        {
            string erorrMsg = "Error Occurred";
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = UserManager.Create(user, model._BackEndUser.Password);
                erorrMsg = result?.Errors?.Count() > 0 ? result.Errors.ToList()[0] : erorrMsg;
                if (result.Succeeded)
                {
                    SignInManager.UserManager.AddToRole(user.Id, model.RoleId);
                    BackendUser backend = new BackendUser();

                    backend.CreateDate = DateTime.Now;
                    backend.UserName = model._BackEndUser.UserName;
                    backend.Password = model._BackEndUser.Password;
                    backend.IsActive = model._BackEndUser.IsActive;
                    backend.FK_AspUser = user.Id;
                    backend.AspNetUser = repository.GetByKey<AspNetUser>(user.Id);

                    backend.BackEndPositionId = model.PositionId;
                    backend.BackendUserPositions = repository.GetByKey<BackendUserPosition>(model.PositionId);

                    backend.BackendUserID = Guid.Parse(user.Id);
                    repository.Add(backend);
                    repository.UnitOfWork.SaveChanges();
                    return Json(new { success = true, html = GlobalClass.RenderRazorViewToString(this, "Index", repository.GetAll<BackendUser>().ToList()), message = "Created Successfully", style = "custome" }, JsonRequestBehavior.AllowGet);
                }
            }
            ViewBag.Role = new SelectList(repository.Find<AspNetRole>(f => f.Name != "Corporation" && f.Name != "Corporation IndIvidual" && f.Name != "IndIvidual"), "Name", "Name");
            ViewBag.Position = new SelectList(repository.GetQuery<BackendUserPosition>(f => f.IsActive == true), "BackendUserPositionID", CultureHelper.CurrentCulture != 3 ? "NameEN" : "NameAR");
            return Json(new { success = false, html = GlobalClass.RenderRazorViewToString(this, "Index", repository.GetAll<BackendUser>().ToList()), message = erorrMsg, style = "custome2" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Edit(Guid? id)
        {
            ViewBag.Role = new SelectList(repository.Find<AspNetRole>(f => f.Name != "Corporation" && f.Name != "Corporation IndIvidual" && f.Name != "IndIvidual").ToList(), "Name", "Name");
            ViewBag.Position = new SelectList(repository.GetQuery<BackendUserPosition>(f => f.IsActive == true).ToList(), "BackendUserPositionID", CultureHelper.CurrentCulture != 3 ? "NameEN" : "NameAR");

            if (id == null)
            {
                return View();
            }
            BackendUser backEnduser = repository.GetByKey<BackendUser>(id);
            if (backEnduser == null)
            {
                return HttpNotFound();
            }
            EmployeeVM _EmpVM = new EmployeeVM();
            _EmpVM._BackEndUser = backEnduser;
            _EmpVM.RoleId = backEnduser.AspNetUser.AspNetRoles.First().Name;
            _EmpVM.PositionId = backEnduser.BackEndPositionId;

            return View(_EmpVM);
        }

        [HttpPost]
        public ActionResult Edit(EmployeeVM model)
        {
            if (ModelState.IsValid)
            {
                model._BackEndUser.BackEndPositionId = model.PositionId;
                repository.Update(model._BackEndUser);
                var _user = repository.GetByKey<AspNetUser>(model._BackEndUser.FK_AspUser);
                _user.Email = model._BackEndUser.AspNetUser.Email;
                SignInManager.UserManager.RemoveFromRole(_user.Id, _user.AspNetRoles.FirstOrDefault().Name);
                SignInManager.UserManager.AddToRole(_user.Id, model.RoleId);
                PasswordHasher hashpass = new PasswordHasher();
                _user.PasswordHash = hashpass.HashPassword(model._BackEndUser.Password);
                _user.UserName = model._BackEndUser.AspNetUser.UserName;
                repository.Update(_user);
                repository.UnitOfWork.SaveChanges();
                return Json(new { success = true, html = GlobalClass.RenderRazorViewToString(this, "Index", repository.GetAll<BackendUser>().ToList()), message = "Created Successfully", style = "custome" }, JsonRequestBehavior.AllowGet);
            }
            ViewBag.Role = new SelectList(repository.Find<AspNetRole>(f => f.Name != "Corporation" && f.Name != "Corporation IndIvidual" && f.Name != "IndIvidual"), "Name", "Name");
            ViewBag.Position = new SelectList(repository.GetQuery<BackendUserPosition>(f => f.IsActive == true), "BackendUserPositionID", CultureHelper.CurrentCulture != 3 ? "NameEN" : "NameAR");
            return Json(new { success = false, html = GlobalClass.RenderRazorViewToString(this, "Index", repository.GetAll<BackendUser>().ToList()), message = "Error Occurred", style = "custome2" }, JsonRequestBehavior.AllowGet);
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
    }
}