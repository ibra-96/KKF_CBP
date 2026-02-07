using System;
using System.Linq;
using System.Web.Mvc;
using AlphaPeople.Core;
using System.Data.Entity;
using System.Configuration;
using AlphaPeople.Repository;
using AlfaPeople.KingKhalidFoundation.Data;
using AlfaPeople.KingKhalidFoundation.Data.Model;
using AlfaPeople.KingKhaledFoundation.Admin.Web.Helper;

namespace AlfaPeople.KingKhaledFoundation.Admin.Web.Controllers
{
    [Authorize(Roles = "Admin, CB Manager, CB Supervisor, CB Analyst/*, SI Manager, SI Supervisor, SI Analyst*/")]
    public class GovernoratesController : BaseController
    {

        private readonly IRepository repository;
        private readonly CommonHelper helper;
        private readonly string Logpath = ConfigurationManager.AppSettings["Log"].ToString();


        public GovernoratesController()
        {
            helper = new CommonHelper();
            repository = new Repository(new KingkhaledFoundationDB());
        }
        public ActionResult All()
        {
            return View();
        }

        // GET: Governorates
        public ActionResult Index()
        {
            ViewBag.lang = CultureHelper.CurrentCulture;

            var governorates = repository.GetQuery<Governorate>().Include(g => g.Region);
            return View(governorates.ToList());
        }


        // GET: Governorates/Create
        public ActionResult Create()
        {
            if (CultureHelper.CurrentCulture == 3)

                ViewBag.RegionID = new SelectList(repository.GetAll<Region>().ToList(), "RegionID", "RegionNameAR");
            else
                ViewBag.RegionID = new SelectList(repository.GetAll<Region>().ToList(), "RegionID", "RegionNameEN");

            return View();
        }

        // POST: Governorates/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "GovernorateID,IsActive,RegionID,GovernorateAR,GovernorateEN")] Governorate governorate)
        {
            if (ModelState.IsValid)
            {
                governorate.GovernorateID = Guid.NewGuid();
                repository.Add<Governorate>(governorate);
                repository.UnitOfWork.SaveChanges();
                return Json(new { success = true, html = GlobalClass.RenderRazorViewToString(this, "Index", repository.GetAll<Governorate>().ToList()), message = "Created Successfully", style = "custome" }, JsonRequestBehavior.AllowGet);
            }

            if (CultureHelper.CurrentCulture == 3)

                ViewBag.RegionID = new SelectList(repository.GetAll<Region>().ToList(), "RegionID", "RegionNameAR");
            else
                ViewBag.RegionID = new SelectList(repository.GetAll<Region>().ToList(), "RegionID", "RegionNameEN");
            return Json(new { success = false, html = GlobalClass.RenderRazorViewToString(this, "Index", repository.GetQuery<Governorate>().Include(g => g.Region)), message = "Error occurred", style = "custome2" }, JsonRequestBehavior.AllowGet);
        }

        // GET: Governorates/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (CultureHelper.CurrentCulture == 3)

                ViewBag.RegionID = new SelectList(repository.GetAll<Region>().ToList(), "RegionID", "RegionNameAR");
            else
                ViewBag.RegionID = new SelectList(repository.GetAll<Region>().ToList(), "RegionID", "RegionNameEN");

            if (id == null)
            {
                return View();
            }
            Governorate governorate = repository.GetByKey<Governorate>(id);
            if (governorate == null)
            {
                return HttpNotFound();
            }
            return View(governorate);
        }

        // POST: Governorates/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "GovernorateID,IsActive,RegionID,GovernorateAR,GovernorateEN")] Governorate governorate)
        {
            if (ModelState.IsValid)
            {
                repository.Update(governorate);
                repository.UnitOfWork.SaveChanges();
                return Json(new { success = true, html = GlobalClass.RenderRazorViewToString(this, "Index", repository.GetAll<Governorate>().ToList()), message = "Updated Successfully", style = "custome" }, JsonRequestBehavior.AllowGet);
            }

            if (CultureHelper.CurrentCulture == 3)

                ViewBag.RegionID = new SelectList(repository.GetAll<Region>().ToList(), "RegionID", "RegionNameAR");
            else
                ViewBag.RegionID = new SelectList(repository.GetAll<Region>().ToList(), "RegionID", "RegionNameEN");

            return Json(new { success = false, html = GlobalClass.RenderRazorViewToString(this, "Index", repository.GetQuery<Governorate>().Include(g => g.Region)), message = "Error occurred", style = "custome2" }, JsonRequestBehavior.AllowGet);
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
