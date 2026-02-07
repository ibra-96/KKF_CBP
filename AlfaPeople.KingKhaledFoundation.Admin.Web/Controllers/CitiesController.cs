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
    [Authorize(Roles = "Admin, CB Manager, CB Supervisor, CB Analyst")]
    public class CitiesController : BaseController
    {

        private readonly IRepository repository;
        private readonly CommonHelper helper;
        private readonly string Logpath = ConfigurationManager.AppSettings["Log"].ToString();

        public CitiesController()
        {
            helper = new CommonHelper();
            repository = new Repository(new KingkhaledFoundationDB());
        }


        public ActionResult All()
        {
            return View();
        }

        // GET: Cities
        public ActionResult Index()
        {
            ViewBag.lang = CultureHelper.CurrentCulture;
            var cities = repository.GetQuery<City>().Include(c => c.Governorate).ToList();
            return View(cities.ToList());
        }

        // GET: Cities/Create
        public ActionResult Create()
        {
            if (CultureHelper.CurrentCulture == 3)
                ViewBag.GovernorateID = new SelectList(repository.GetAll<Governorate>().ToList(), "GovernorateID", "GovernorateAR");
            else
                ViewBag.GovernorateID = new SelectList(repository.GetAll<Governorate>().ToList(), "GovernorateID", "GovernorateEN");
            return View();
        }

        // POST: Cities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CityID,IsActive,GovernorateID,CityNameAR,CityNameEN")] City city)
        {
            if (ModelState.IsValid)
            {
                city.CityID = Guid.NewGuid();
                repository.Add<City>(city);
                repository.UnitOfWork.SaveChanges();
                return Json(new { success = true, html = GlobalClass.RenderRazorViewToString(this, "Index", repository.GetQuery<City>().Include(g => g.Governorate)), message = "Created Successfully", style = "custome" }, JsonRequestBehavior.AllowGet);
            }

            if (CultureHelper.CurrentCulture == 3)
                ViewBag.GovernorateID = new SelectList(repository.GetAll<Governorate>().ToList(), "GovernorateID", "GovernorateAR");
            else
                ViewBag.GovernorateID = new SelectList(repository.GetAll<Governorate>().ToList(), "GovernorateID", "GovernorateEN");
            return Json(new { success = false, html = GlobalClass.RenderRazorViewToString(this, "Index", repository.GetQuery<City>().Include(g => g.Governorate)), message = "Error occurred", style = "custome2" }, JsonRequestBehavior.AllowGet);
        }

        // GET: Cities/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (CultureHelper.CurrentCulture == 3)
                ViewBag.GovernorateID = new SelectList(repository.GetAll<Governorate>().ToList(), "GovernorateID", "GovernorateAR");
            else
                ViewBag.GovernorateID = new SelectList(repository.GetAll<Governorate>().ToList(), "GovernorateID", "GovernorateEN");

            if (id == null)
            {
                return View();
            }
            City city = repository.GetByKey<City>(id);
            if (city == null)
            {
                return HttpNotFound();
            }
            return View(city);
        }

        // POST: Cities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CityID,IsActive,GovernorateID,CityNameAR,CityNameEN")] City city)
        {
            if (ModelState.IsValid)
            {
                repository.Update(city);
                repository.UnitOfWork.SaveChanges();
                return Json(new { success = true, html = GlobalClass.RenderRazorViewToString(this, "Index", repository.GetQuery<City>().Include(g => g.Governorate)), message = "Updated Successfully", style = "custome" }, JsonRequestBehavior.AllowGet);

            }
            if (CultureHelper.CurrentCulture == 3)
                ViewBag.GovernorateID = new SelectList(repository.GetAll<Governorate>().ToList(), "GovernorateID", "GovernorateAR");
            else
                ViewBag.GovernorateID = new SelectList(repository.GetAll<Governorate>().ToList(), "GovernorateID", "GovernorateEN"); return Json(new { success = false, html = GlobalClass.RenderRazorViewToString(this, "Index", repository.GetQuery<City>().Include(g => g.Governorate)), message = "Error occurred", style = "custome2" }, JsonRequestBehavior.AllowGet);
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
