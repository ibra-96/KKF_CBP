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
    public class IncubationWorkshopModelsController : BaseController
    {
        private readonly IRepository repository;
        private readonly CommonHelper helper;
        private readonly string Logpath = ConfigurationManager.AppSettings["Log"].ToString();

        public IncubationWorkshopModelsController()
        {
            helper = new CommonHelper();
            repository = new Repository(new KingkhaledFoundationDB());
        }

        public ActionResult All()
        {
            var userId = User.Identity.GetUserId();
            ViewBag.UserId = userId;
            return View();
        }

        // GET: IncubationModels
        public ActionResult Index()
        {
            ViewBag.lang = CultureHelper.CurrentCulture;
            return View(repository.GetAll<IncubationWorkshopModel>().ToList());
        }

        // GET: IncubationWorkshopModels/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
                return View();

            var incubationWorkshopMode = repository.GetByKey<IncubationWorkshopModel>(id);

            if (incubationWorkshopMode == null)
                return View();

            return View(incubationWorkshopMode);
        }

        // GET: IncubationWorkshopModels/Create
        public ActionResult Create()
        {
            var userId = User.Identity.GetUserId();
            ViewBag.UserId = userId;
            return View();
        }

        // POST: IncubationWorkshopModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IncubationWorkshopModeID,FK_AspUserCreateModel,NameAR,IsActive,NameEN")] IncubationWorkshopModel incubationWorkshopMode)
        {
            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();
                incubationWorkshopMode.AspNetUser = new AspNetUser();
                incubationWorkshopMode.IncubationWorkshopModeID = Guid.NewGuid();
                incubationWorkshopMode.AspNetUser = repository.GetByKey<AspNetUser>(userId);
                incubationWorkshopMode.FK_AspUserCreateModel = userId;
                repository.Add(incubationWorkshopMode);
                repository.UnitOfWork.SaveChanges();
                return Json(new { success = true, html = GlobalClass.RenderRazorViewToString(this, "Index", repository.GetAll<IncubationWorkshopModel>().ToList()), message = "Submitted Successfully" }, JsonRequestBehavior.AllowGet);
            }
            return View(incubationWorkshopMode);
        }

        // GET: IncubationWorkshopModels/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
                return View();

            var incubationWorkshopMode = repository.GetByKey<IncubationWorkshopModel>(id);

            if (incubationWorkshopMode == null)
                return View();

            return View(incubationWorkshopMode);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IncubationWorkshopModeID,IsActive,FK_AspUserCreateModel,NameAR,NameEN")] IncubationWorkshopModel incubationWorkshopMode)
        {
            if (ModelState.IsValid)
            {
                repository.Update<IncubationWorkshopModel>(incubationWorkshopMode);
                repository.UnitOfWork.SaveChanges();
                return Json(new { success = true, html = GlobalClass.RenderRazorViewToString(this, "Index", repository.GetAll<IncubationWorkshopModel>().ToList()), message = "Submitted Successfully" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = true, html = GlobalClass.RenderRazorViewToString(this, "Index", repository.GetAll<IncubationWorkshopModel>().ToList()), message = "Submitted Successfully" }, JsonRequestBehavior.AllowGet);
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
