using System;
using System.Linq;
using System.Web.Mvc;
using AlphaPeople.Core;
using System.Configuration;
using AlphaPeople.Repository;
using Microsoft.AspNet.Identity;
using AlfaPeople.KingKhalidFoundation.Data;
using AlfaPeople.KingKhalidFoundation.Data.Model;

namespace AlfaPeople.KingKhaledFoundation.Admin.Web.Controllers
{
    [Authorize(Roles = "Admin,CB Manager,CB Supervisor,CB Analyst")]
    public class ConsultantsController : BaseController
    {

        private readonly IRepository repository;
        private readonly CommonHelper helper;
        private readonly string Logpath = ConfigurationManager.AppSettings["Log"].ToString();

        public ConsultantsController()
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

        // GET: Consultants
        public ActionResult Index()
        {
            return View(repository.GetAll<Consultant>().ToList());
        }

        // GET: Consultants/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return View();
            }
            Consultant consultant = repository.GetByKey<Consultant>(id);
            if (consultant == null)
            {
                return HttpNotFound();
            }
            return View(consultant);
        }

        // GET: Consultants/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Consultants/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ConsultantID,Name,Address,POBox,PostalCode,OfficialMail,MobileNumber,TelephoneNumber,Extension,IsActive")] Consultant consultant)
        {
            if (ModelState.IsValid)
            {
                consultant.ConsultantID = Guid.NewGuid();

                repository.Add<Consultant>(consultant);
                repository.UnitOfWork.SaveChanges();
                return Json(new { success = true, html = GlobalClass.RenderRazorViewToString(this, "Index", repository.GetAll<Consultant>().ToList()), message = "Created Successfully", style = "custome" }, JsonRequestBehavior.AllowGet);

            }
            return Json(new { success = false, html = GlobalClass.RenderRazorViewToString(this, "Index", repository.GetAll<Consultant>().ToList()), message = "Error occurred", style = "custome2" }, JsonRequestBehavior.AllowGet);
        }

        // GET: Consultants/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return View();
            }
            Consultant consultant = repository.GetByKey<Consultant>(id);
            if (consultant == null)
            {
                return HttpNotFound();
            }
            return View(consultant);
        }

        // POST: Consultants/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ConsultantID,Name,Address,IsActive,POBox,PostalCode,OfficialMail,MobileNumber,TelephoneNumber,Extension")] Consultant consultant)
        {
            if (ModelState.IsValid)
            {
                repository.Update<Consultant>(consultant);
                repository.UnitOfWork.SaveChanges();
                return Json(new { success = true, html = GlobalClass.RenderRazorViewToString(this, "Index", repository.GetAll<Consultant>().ToList()), message = "Updated Successfully", style = "custome" }, JsonRequestBehavior.AllowGet);

            }
            return Json(new { success = false, html = GlobalClass.RenderRazorViewToString(this, "Index", repository.GetAll<Consultant>().ToList()), message = "Error occurred", style = "custome2" }, JsonRequestBehavior.AllowGet);
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
