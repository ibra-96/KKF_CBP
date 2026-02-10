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
    public class CorporationsCategoryController : BaseController
    {

        private readonly IRepository repository;
        private readonly CommonHelper helper;
        private readonly string Logpath = ConfigurationManager.AppSettings["Log"].ToString();

        public CorporationsCategoryController()
        {
            helper = new CommonHelper();
            repository = new Repository(new KingkhaledFoundationDB());
        }

        public ActionResult All()
        {
            return View();
        }

        public ActionResult Index()
        {
            ViewBag.lang = CultureHelper.CurrentCulture;
            return View(repository.GetAll<CorporationsCategory>().ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CorporationsCategory category)
        {
            if (ModelState.IsValid)
            {
                category.CorporationsCategoryID = Guid.NewGuid();
                repository.Add(category);
                repository.UnitOfWork.SaveChanges();
                return Json(new { success = true, html = GlobalClass.RenderRazorViewToString(this, "Index", repository.GetAll<CorporationsCategory>().ToList()), message = "Created Successfully", style = "custome" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false, html = GlobalClass.RenderRazorViewToString(this, "Index", repository.GetAll<CorporationsCategory>().ToList()), message = "Error occurred", style = "custome2" }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return View();
            }
            CorporationsCategory cat = repository.GetByKey<CorporationsCategory>(id);
            if (cat == null)
            {
                return HttpNotFound();
            }
            return View(cat);
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CorporationsCategory cat)
        {
            if (ModelState.IsValid)
            {
                repository.Update<CorporationsCategory>(cat);
                repository.UnitOfWork.SaveChanges();
                return Json(new { success = true, html = GlobalClass.RenderRazorViewToString(this, "Index", repository.GetAll<CorporationsCategory>().ToList()), message = "Updated Successfully", style = "custome" }, JsonRequestBehavior.AllowGet);

            }
            return Json(new { success = false, html = GlobalClass.RenderRazorViewToString(this, "Index", repository.GetAll<CorporationsCategory>().ToList()), message = "Error occurred", style = "custome2" }, JsonRequestBehavior.AllowGet);
        }
    }
}