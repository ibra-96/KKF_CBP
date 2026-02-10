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
    public class ClassificationSectorsController : BaseController
    {
        private readonly IRepository repository;
        private readonly CommonHelper helper;
        private readonly string Logpath = ConfigurationManager.AppSettings["Log"].ToString();

        public ClassificationSectorsController()
        {
            helper = new CommonHelper();
            repository = new Repository(new KingkhaledFoundationDB());
        }

        public ActionResult All()
        {
            return View();
        }

        // GET: ClassificationSectors
        public ActionResult Index()
        {
            ViewBag.lang = CultureHelper.CurrentCulture;
            var classificationSectors = repository.GetQuery<ClassificationSector>().Include(c => c.CorporationsCategory);
            return View(classificationSectors.ToList());
        }

        // GET: ClassificationSectors/Create
        public ActionResult Create()
        {
            ViewBag.CorporationsCategoryID = new SelectList(repository.GetAll<CorporationsCategory>().ToList(), "CorporationsCategoryID", CultureHelper.CurrentCulture != 3 ? "CorporationsCategoryNameEN" : "CorporationsCategoryNameAR");
            return View();
        }

        // POST: ClassificationSectors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ClassificationSectorID,CorporationsCategoryID,ClassificationSectorNameAR,IsActive,ClassificationSectorNameEN")] ClassificationSector classificationSector)
        {
            if (ModelState.IsValid)
            {
                classificationSector.ClassificationSectorID = Guid.NewGuid();
                repository.Add(classificationSector);
                repository.UnitOfWork.SaveChanges();
                return Json(new { success = true, html = GlobalClass.RenderRazorViewToString(this, "Index", repository.GetQuery<ClassificationSector>().Include(f => f.CorporationsCategory).ToList()), message = "Created Successfully", style = "custome" }, JsonRequestBehavior.AllowGet);
            }
            ViewBag.CorporationsCategoryID = new SelectList(repository.GetAll<CorporationsCategory>().ToList(), "CorporationsCategoryID", CultureHelper.CurrentCulture != 3 ? "CorporationsCategoryNameEN" : "CorporationsCategoryNameAR");
            return Json(new { success = false, html = GlobalClass.RenderRazorViewToString(this, "Index", repository.GetQuery<ClassificationSector>().Include(f => f.CorporationsCategory).ToList()), message = "Error Occurred", style = "custome2" }, JsonRequestBehavior.AllowGet);
        }

        // GET: ClassificationSectors/Edit/5
        public ActionResult Edit(Guid? id)
        {
            ViewBag.CorporationsCategoryID = new SelectList(repository.GetAll<CorporationsCategory>().ToList(), "CorporationsCategoryID", CultureHelper.CurrentCulture != 3 ? "CorporationsCategoryNameEN" : "CorporationsCategoryNameAR");
            if (id == null)
            {
                return View();
            }
            ClassificationSector classificationSector = repository.GetByKey<ClassificationSector>(id);
            if (classificationSector == null)
            {
                return HttpNotFound();
            }
            return View(classificationSector);
        }

        // POST: ClassificationSectors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ClassificationSectorID,CorporationsCategoryID,ClassificationSectorNameAR,IsActive,ClassificationSectorNameEN")] ClassificationSector classificationSector)
        {
            if (ModelState.IsValid)
            {
                repository.Update(classificationSector);
                repository.UnitOfWork.SaveChanges();
                return Json(new { success = true, html = GlobalClass.RenderRazorViewToString(this, "Index", repository.GetQuery<ClassificationSector>().Include(f => f.CorporationsCategory).ToList()), message = "Updated Successfully", style = "custome" }, JsonRequestBehavior.AllowGet);
            }
            ViewBag.CorporationsCategoryID = new SelectList(repository.GetAll<CorporationsCategory>().ToList(), "CorporationsCategoryID", CultureHelper.CurrentCulture != 3 ? "CorporationsCategoryNameEN" : "CorporationsCategoryNameAR");
            return Json(new { success = false, html = GlobalClass.RenderRazorViewToString(this, "Index", repository.GetQuery<ClassificationSector>().Include(f => f.CorporationsCategory).ToList()), message = "Error Occurred", style = "custome2" }, JsonRequestBehavior.AllowGet);
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
