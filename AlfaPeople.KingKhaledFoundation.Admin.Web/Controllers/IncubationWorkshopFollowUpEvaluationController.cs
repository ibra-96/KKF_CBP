using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using AlphaPeople.Core;
using System.Data.Entity;
using System.Transactions;
using System.Configuration;
using AlphaPeople.Repository;
using System.Collections.Generic;
using AlfaPeople.KingKhalidFoundation.Data;
using AlfaPeople.KingKhalidFoundation.Data.Model;
using AlfaPeople.KingKhaledFoundation.Admin.Web.Models;

namespace AlfaPeople.KingKhaledFoundation.Admin.Web.Controllers
{
    [Authorize(Roles = "Admin, CB Manager, CB Supervisor, CB Analyst")]
    public class IncubationWorkshopFollowUpEvaluationController : BaseController
    {
        private readonly IRepository repository;
        private readonly CommonHelper helper;
        private readonly DateTime dateNow = DateTime.Now.Date;
        private readonly string Logpath = ConfigurationManager.AppSettings["Log"].ToString();

        public IncubationWorkshopFollowUpEvaluationController()
        {
            repository = new Repository(new KingkhaledFoundationDB());
            helper = new CommonHelper();
        }

        // GET: IncubationFollowUpEvaluation
        public ActionResult Index(string Type)
        {
            var IncubFollowEval = repository.GetQuery<IncubationWorkShopFollowUpEvaluation>().Where(t=>!t.IncubationWorkshop.IsDeleted).ToList();
            return View(IncubFollowEval);
        }

        [HttpGet]
        public ActionResult AppendLst()
        {
            var EvaluationLines = new IncubationWorkShopFollowUpEvaluationLines();
            ViewBag.adminUserID = new SelectList(repository.GetQuery<BackendUser>(f => f.AspNetUser.AspNetRoles.Any(r => r.Name == "CB Manager" || r.Name == "CB Supervisor" || r.Name == "CB Analyst")).ToList(), "BackendUserID", "UserName");
            return PartialView("_LstIncubationWorkshopFollowUpEvaluationLines", EvaluationLines);
        }

        // GET: IncubationFollowUpEvaluation/Create
        public ActionResult Create(Guid? id, string Type = "Incubation")
        {
            ViewBag.CorporationUserID = new SelectList(repository.GetQuery<FrontendUser>(f => f.IsActive == true).ToList(), "UserID", "AspNetUser.UserName");
            ViewBag.IncubationID = new SelectList(repository.GetQuery<Incubation>().ToList(), "IncubationID", "Name");
            ViewBag.adminUserID = new SelectList(repository.GetQuery<BackendUser>(f => f.AspNetUser.AspNetRoles.Any(r => r.Name == "CB Manager" || r.Name == "CB Supervisor" || r.Name == "CB Analyst")).ToList(), "BackendUserID", "UserName");
            ViewBag.workShop = new SelectList(repository.GetQuery<IncubationWorkshop>().Where(t => !t.IsDeleted).ToList(), "IncubationWorkshopID", "Name");
            IncubationWorkshopFollowUpEvaluationVM IFEVM = new IncubationWorkshopFollowUpEvaluationVM()
            {
                IFEVM = new IncubationWorkShopFollowUpEvaluation(),
                LstIFELVM = new List<IncubationWorkShopFollowUpEvaluationLines>()
            };
            return View(IFEVM);
        }

        [HttpPost]
        public ActionResult Create(IncubationWorkshopFollowUpEvaluationVM model)
        {
            ViewBag.CorporationUserID = new SelectList(repository.GetQuery<FrontendUser>(f => f.IsActive == true).ToList(), "UserID", "AspNetUser.UserName");
            ViewBag.adminUserID = new SelectList(repository.GetQuery<BackendUser>(f => f.AspNetUser.AspNetRoles.Any(r => r.Name == "CB Manager" || r.Name == "CB Supervisor" || r.Name == "CB Analyst")).ToList(), "BackendUserID", "UserName");
            ViewBag.workShop = new SelectList(repository.GetQuery<IncubationWorkshop>().Where(t => !t.IsDeleted).ToList(), "IncubationWorkshopID", "Name");

            var entity = new IncubationWorkShopFollowUpEvaluation()
            {
                IncubationWorkShopFollowUpEvaluationID = Guid.NewGuid(),
                FrontendUserID = model.IFEVM.FrontendUserID,
                Impact = model.IFEVM.Impact,
                Reasone = model.IFEVM.Reasone,
                ProjectSuccessRate = model.IFEVM.ProjectSuccessRate,
                SuccessStories = model.IFEVM.SuccessStories,
                IncubationWorkshopID = model.IFEVM.IncubationWorkshopID
            };
            if (model.LstIFELVM != null)
            {
                foreach (var incubationLine in model.LstIFELVM)
                {
                    entity.IncubationWorkShopFollowUpEvaluationLines.Add(new IncubationWorkShopFollowUpEvaluationLines()
                    {
                        Id = Guid.NewGuid(),
                        BackendUserID = incubationLine.BackendUserID,
                        Date = incubationLine.Date,
                        FollowUpMethod = incubationLine.FollowUpMethod,
                        NextStep = incubationLine.NextStep,
                        Notes = incubationLine.Notes,
                        PhaseOutput = incubationLine.PhaseOutput
                    });
                }
            }

            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    repository.Add(entity);
                    if (model.files != null)
                    {
                        string FolderName = User.Identity.Name;
                        string path = Server.MapPath("~/Uploads/" + FolderName + "/");
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        for (int i = 0; i < model.files.Count(); i++)
                        {
                            if (model.files[i] != null)
                            {
                                model.files[i].SaveAs(path + Path.GetFileName(model.files[i].FileName));
                                IncubationWorkShopFollowUpEvaluationAttchment _Attach = new IncubationWorkShopFollowUpEvaluationAttchment();
                                _Attach.AttachmentID = Guid.NewGuid();
                                _Attach.IncubationWorkShopFollowUpEvaluationID = entity.IncubationWorkShopFollowUpEvaluationID;
                                _Attach.Name = Path.GetFileName(model.files[i].FileName);
                                _Attach.ScreenName = "Incubation Workshop FollowUp Evaluation";
                                _Attach.Size = model.files[i].ContentLength.ToString();
                                _Attach.URL = path + Path.GetFileName(model.files[i].FileName);
                                _Attach.Type = model.files[i].ContentType;
                                repository.Add(_Attach);
                            }
                        }
                        ViewBag.Message = "File uploaded successfully.";
                    }
                    repository.UnitOfWork.SaveChanges();
                    scope.Complete();
                }
                catch (Exception)
                {
                    scope.Dispose();
                    return View(model);
                }
            }

            return RedirectToAction("index", new { Type = "Incubation" });
        }

        [HttpGet]
        public ActionResult Edit(Guid IncubationWorkshopFollowUpEvaluationID, string Type)
        {
            var Entity = repository.GetQuery<IncubationWorkShopFollowUpEvaluation>(i => i.IncubationWorkShopFollowUpEvaluationID == IncubationWorkshopFollowUpEvaluationID).Include(c => c.IncubationWorkShopFollowUpEvaluationLines).SingleOrDefault();
            IncubationWorkshopFollowUpEvaluationVM model = new IncubationWorkshopFollowUpEvaluationVM();

            ViewBag.CorporationUserID = new SelectList(repository.GetQuery<FrontendUser>(f => f.IsActive == true).ToList(), "UserID", "AspNetUser.UserName");  /*CorporateApplicationForms.FirstOrDefault().Name*/
            ViewBag.adminUserID = new SelectList(repository.GetQuery<BackendUser>(f => f.AspNetUser.AspNetRoles.Any(r =>/* r.Name == "Admin"||*/ r.Name == "CB Manager" || r.Name == "CB Supervisor" || r.Name == "CB Analyst")).ToList(), "BackendUserID", "UserName");
            ViewBag.workShop = new SelectList(repository.GetQuery<IncubationWorkshop>().Where(t=>!t.IsDeleted).ToList(), "IncubationWorkshopID", "Name");

            model.IFEVM = Entity;
            model.LstIFELVM = Entity.IncubationWorkShopFollowUpEvaluationLines.ToList();

            return View(model);
        }


        [HttpPost]
        public ActionResult Edit(IncubationWorkshopFollowUpEvaluationVM Model)
        {
            var Entity = repository.GetQuery<IncubationWorkShopFollowUpEvaluation>(i => i.IncubationWorkShopFollowUpEvaluationID == Model.IFEVM.IncubationWorkShopFollowUpEvaluationID).Include(c => c.IncubationWorkShopFollowUpEvaluationLines).SingleOrDefault();

            var liensCollections = Entity.IncubationWorkShopFollowUpEvaluationLines.ToList();

            if (Entity.IncubationWorkShopFollowUpEvaluationLines != null)
            {
                foreach (var IncubationFollowUpEvaluationLine in liensCollections)
                    repository.Delete(IncubationFollowUpEvaluationLine);
            }

            repository.UnitOfWork.SaveChanges();

            Entity.Impact = Model.IFEVM.Impact;
            Entity.Reasone = Model.IFEVM.Reasone;
            Entity.SuccessStories = Model.IFEVM.SuccessStories;
            Entity.ProjectSuccessRate = Model.IFEVM.ProjectSuccessRate;
            Entity.IncubationWorkshopID = Model.IFEVM.IncubationWorkshopID;

            repository.Update(Entity);
            repository.UnitOfWork.SaveChanges();

            if (Model.LstIFELVM != null)
            {
                foreach (var line in Model.LstIFELVM)
                {
                    repository.Add(new IncubationWorkShopFollowUpEvaluationLines()
                    {
                        Id = Guid.NewGuid(),
                        BackendUserID = line.BackendUserID,
                        Date = line.Date,
                        FollowUpMethod = line.FollowUpMethod,
                        IncubationWorkShopFollowUpEvaluationID = Entity.IncubationWorkShopFollowUpEvaluationID,
                        NextStep = line.NextStep,
                        Notes = line.Notes,
                        PhaseOutput = line.PhaseOutput
                    });
                }
            }

            if (Model.files != null)
            {
                string FolderName = User.Identity.Name;
                string path = Server.MapPath("~/Uploads/" + FolderName + "/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                for (int i = 0; i < Model.files.Count(); i++)
                {
                    if (Model.files[i] != null)
                    {
                        Model.files[i].SaveAs(path + Path.GetFileName(Model.files[i].FileName));
                        IncubationWorkShopFollowUpEvaluationAttchment _Attach = new IncubationWorkShopFollowUpEvaluationAttchment();
                        _Attach.AttachmentID = Guid.NewGuid();
                        _Attach.IncubationWorkShopFollowUpEvaluationID = Entity.IncubationWorkShopFollowUpEvaluationID;
                        _Attach.Name = Path.GetFileName(Model.files[i].FileName);
                        _Attach.ScreenName = "Incubation Workshop FollowUp Evaluation";
                        _Attach.Size = Model.files[i].ContentLength.ToString();
                        _Attach.URL = path + Path.GetFileName(Model.files[i].FileName);
                        _Attach.Type = Model.files[i].ContentType;
                        repository.Add(_Attach);
                    }
                }
                ViewBag.Message = "File uploaded successfully.";
            }

            repository.UnitOfWork.SaveChanges();
            return RedirectToAction("index");
        }

        public FileResult Download(Guid id)
        {
            var attachmet = repository.GetByKey<IncubationWorkShopFollowUpEvaluationAttchment>(id);
            byte[] fileBytes = System.IO.File.ReadAllBytes(attachmet.URL);
            string fileName = attachmet.Name;
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        [HttpGet]
        public JsonResult DeleteAttachment(Guid Id)
        {
            try
            {
                var Attachment = repository.GetByKey<IncubationWorkShopFollowUpEvaluationAttchment>(Id);
                repository.Delete(Attachment);
                repository.UnitOfWork.SaveChanges();

                if (System.IO.File.Exists(Attachment.URL))
                {
                    System.IO.File.Delete(Attachment.URL);
                }
                return Json("Deleted Succsesfully", JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json("Deletion Failed", JsonRequestBehavior.AllowGet);
            }
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