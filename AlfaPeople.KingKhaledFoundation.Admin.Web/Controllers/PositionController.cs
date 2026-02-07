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
    [Authorize(Roles = "Admin, CB Manager")]
    public class PositionController : BaseController
    {
        private readonly IRepository repository;
        private readonly CommonHelper helper;
        private readonly string Logpath = ConfigurationManager.AppSettings["Log"].ToString();

        public PositionController()
        {
            helper = new CommonHelper();
            repository = new Repository(new KingkhaledFoundationDB());
        }

        public ActionResult All()
        {
            return View();
        }

        // GET: Position
        public ActionResult Index()
        {
            ViewBag.lang = CultureHelper.CurrentCulture;
            return View(repository.GetAll<BackendUserPosition>().ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BackendUserPosition position)
        {
            if (ModelState.IsValid)
            {
                position.BackendUserPositionID = Guid.NewGuid();
                repository.Add(position);
                repository.UnitOfWork.SaveChanges();
                return Json(new { success = true, html = GlobalClass.RenderRazorViewToString(this, "Index", repository.GetAll<BackendUserPosition>().ToList()), message = "Created Successfully", style = "custome" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = true, html = GlobalClass.RenderRazorViewToString(this, "Index", repository.GetAll<BackendUserPosition>().ToList()), message = "Error occurred", style = "custome2" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return View();
            }
            BackendUserPosition position = repository.GetByKey<BackendUserPosition>(id);
            if (position == null)
            {
                return HttpNotFound();
            }
            return View(position);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BackendUserPosition position)
        {
            if (ModelState.IsValid)
            {
                repository.Update(position);
                repository.UnitOfWork.SaveChanges();
                return Json(new { success = true, html = GlobalClass.RenderRazorViewToString(this, "Index", repository.GetAll<BackendUserPosition>().ToList()), message = "Updated Successfully", style = "custome" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = true, html = GlobalClass.RenderRazorViewToString(this, "Index", repository.GetAll<BackendUserPosition>().ToList()), message = "Error occurred", style = "custome2" }, JsonRequestBehavior.AllowGet);
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