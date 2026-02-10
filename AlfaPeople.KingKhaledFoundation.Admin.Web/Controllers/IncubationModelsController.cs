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
    [Authorize(Roles = "Admin,CB Manager,CB Supervisor,CB Analyst")]
    public class IncubationModelsController : BaseController
    {
        private readonly IRepository repository;
        private readonly CommonHelper helper;
        private readonly string Logpath = ConfigurationManager.AppSettings["Log"].ToString();

        public IncubationModelsController()
        {
            helper = new CommonHelper();
            repository = new Repository(new KingkhaledFoundationDB());
        }

        public ActionResult All(string Type)
        {
            if (Type == "Incubation")
                ViewBag.Title = App_GlobalResources.General.IncubationModel;
            else
                ViewBag.Title = App_GlobalResources.General.AccelerationModel;

            var userId = User.Identity.GetUserId();
            ViewBag.UserId = userId;
            TempData["Type"] = Type;
            return View();
        }

        // GET: IncubationModels
        public ActionResult Index()
        {
            ViewBag.lang = CultureHelper.CurrentCulture;
            string Type = (string)TempData["Type"];
            TempData.Peek("Type");
            return View(repository.GetQuery<IncubationModel>(f => f.IncubationType.NameEN == Type).ToList());
        }

        // GET: IncubationModels/Create
        public ActionResult Create()
        {
            TempData.Keep("Type");
            var userId = User.Identity.GetUserId();
            ViewBag.UserId = userId;
            return View();
        }

        // POST: IncubationModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IncubationModelID,NameAR,IsActive,FK_AspUserCreateModel,NameEN")] IncubationModel incubationModel)
        {
            string Type = (string)TempData["Type"];
            TempData.Keep("Type");
            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();
                incubationModel.AspNetUser = new AspNetUser();
                incubationModel.IncubationTypeID = repository.FindOne<IncubationType>(f => f.NameEN == Type).IncubationTypeID;
                incubationModel.IncubationModelID = Guid.NewGuid();
                incubationModel.AspNetUser = repository.GetByKey<AspNetUser>(userId);
                repository.Add<IncubationModel>(incubationModel);
                repository.UnitOfWork.SaveChanges();
                return Json(new { success = true, html = GlobalClass.RenderRazorViewToString(this, "Index", repository.GetQuery<IncubationModel>(f => f.IncubationType.NameEN == Type).ToList()), message = "Created Successfully", style = "custome" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false, html = GlobalClass.RenderRazorViewToString(this, "Index", repository.GetQuery<IncubationModel>(f => f.IncubationType.NameEN == Type).ToList()), message = "Error occurred", style = "custome2" }, JsonRequestBehavior.AllowGet);
        }

        // GET: IncubationModels/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return View();
            }
            TempData.Keep("Type");
            var incubationModel = repository.GetByKey<IncubationModel>(id);
            if (incubationModel == null)
            {
                return HttpNotFound();
            }
            return View(incubationModel);
        }

        // POST: IncubationModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IncubationModelID,NameAR,IsActive,FK_AspUserCreateModel,NameEN")] IncubationModel incubationModel)
        {
            string Type = (string)TempData["Type"];
            TempData.Keep("Type");

            if (ModelState.IsValid)
            {
                incubationModel.IncubationTypeID = repository.FindOne<IncubationType>(f => f.NameEN == Type).IncubationTypeID;
                repository.Update<IncubationModel>(incubationModel);
                repository.UnitOfWork.SaveChanges();
                return Json(new { success = true, html = GlobalClass.RenderRazorViewToString(this, "Index", repository.GetQuery<IncubationModel>(f => f.IncubationType.NameEN == Type).ToList()), message = "Updated Successfully", style = "custome" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false, html = GlobalClass.RenderRazorViewToString(this, "Index", repository.GetQuery<IncubationModel>(f => f.IncubationType.NameEN == Type).ToList()), message = "Error occurred", style = "custome2" }, JsonRequestBehavior.AllowGet);
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