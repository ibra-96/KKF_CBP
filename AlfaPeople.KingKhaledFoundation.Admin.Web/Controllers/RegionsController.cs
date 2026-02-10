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
    public class RegionsController : BaseController
    {
        private readonly IRepository repository;
        private readonly CommonHelper helper;
        private readonly string Logpath = ConfigurationManager.AppSettings["Log"].ToString();

        public RegionsController()
        {
            helper = new CommonHelper();
            repository = new Repository(new KingkhaledFoundationDB());
        }

        public ActionResult All()
        {
            return View();
        }

        // GET: Regions
        public ActionResult Index()
        {
            ViewBag.lang = CultureHelper.CurrentCulture;
            return View(repository.GetAll<Region>().ToList());
        }

        // GET: Regions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Regions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RegionID,RegionNameAR,RegionNameEN,IsActive")] Region region)
        {
            if (ModelState.IsValid)
            {
                region.RegionID = Guid.NewGuid();
                repository.Add<Region>(region);
                repository.UnitOfWork.SaveChanges();
                return Json(new { success = true, html = GlobalClass.RenderRazorViewToString(this, "Index", repository.GetAll<Region>().ToList()), message = "Created Successfully", style = "custome" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = true, html = GlobalClass.RenderRazorViewToString(this, "Index", repository.GetAll<Region>().ToList()), message = "Error occurred", style = "custome2" }, JsonRequestBehavior.AllowGet);
        }

        // GET: Regions/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return View();
            }
            Region region = repository.GetByKey<Region>(id);
            if (region == null)
            {
                return HttpNotFound();
            }
            return View(region);
        }

        // POST: Regions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RegionID,RegionNameAR,RegionNameEN,IsActive")] Region region)
        {
            if (ModelState.IsValid)
            {
                repository.Update<Region>(region);
                repository.UnitOfWork.SaveChanges();
                return Json(new { success = true, html = GlobalClass.RenderRazorViewToString(this, "Index", repository.GetAll<Region>().ToList()), message = "Updated Successfully", style = "custome" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = true, html = GlobalClass.RenderRazorViewToString(this, "Index", repository.GetAll<Region>().ToList()), message = "Error occurred", style = "custome2" }, JsonRequestBehavior.AllowGet);
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
