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
    public class AuthorizationAuthoritiesController : BaseController
    {

        private readonly IRepository repository;
        private readonly CommonHelper helper;
        private readonly string Logpath = ConfigurationManager.AppSettings["Log"].ToString();

        public AuthorizationAuthoritiesController()
        {
            helper = new CommonHelper();
            repository = new Repository(new KingkhaledFoundationDB());
        }

        public ActionResult All()
        {
            return View();
        }

        // GET: AuthorizationAuthorities
        public ActionResult Index()
        {
            ViewBag.lang = CultureHelper.CurrentCulture;
            return View(repository.GetAll<AuthorizationAuthority>().ToList());
        }

        // GET: AuthorizationAuthorities/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AuthorizationAuthorities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AuthorizationAuthorityID,AuthorizationAuthorityNameAR,IsActive,AuthorizationAuthorityNameEN")] AuthorizationAuthority authorizationAuthority)
        {
            if (ModelState.IsValid)
            {
                authorizationAuthority.AuthorizationAuthorityID = Guid.NewGuid();
                repository.Add<AuthorizationAuthority>(authorizationAuthority);
                repository.UnitOfWork.SaveChanges();
                return Json(new { success = true, html = GlobalClass.RenderRazorViewToString(this, "Index", repository.GetAll<AuthorizationAuthority>().ToList()), message = "Created Successfully", style = "custome" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false, html = GlobalClass.RenderRazorViewToString(this, "Index", repository.GetAll<AuthorizationAuthority>().ToList()), message = "Error occurred", style = "custome2" }, JsonRequestBehavior.AllowGet);
        }

        // GET: AuthorizationAuthorities/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return View();
            }
            AuthorizationAuthority authorizationAuthority = repository.GetByKey<AuthorizationAuthority>(id);
            if (authorizationAuthority == null)
            {
                return HttpNotFound();
            }
            return View(authorizationAuthority);
        }

        // POST: AuthorizationAuthorities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AuthorizationAuthorityID,AuthorizationAuthorityNameAR,IsActive,AuthorizationAuthorityNameEN")] AuthorizationAuthority authorizationAuthority)
        {
            if (ModelState.IsValid)
            {
                repository.Update<AuthorizationAuthority>(authorizationAuthority);
                repository.UnitOfWork.SaveChanges();
                return Json(new { success = true, html = GlobalClass.RenderRazorViewToString(this, "Index", repository.GetAll<AuthorizationAuthority>().ToList()), message = "Created Successfully", style = "custome" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false, html = GlobalClass.RenderRazorViewToString(this, "Index", repository.GetAll<AuthorizationAuthority>().ToList()), message = "Error occurred", style = "custome2" }, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                repository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
