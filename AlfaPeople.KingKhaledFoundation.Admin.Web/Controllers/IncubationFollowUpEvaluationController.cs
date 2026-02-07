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
    public class IncubationFollowUpEvaluationController : BaseController
    {
        private readonly IRepository repository;
        private readonly CommonHelper helper;
        private readonly DateTime dateNow = DateTime.Now.Date;
        private readonly string Logpath = ConfigurationManager.AppSettings["Log"].ToString();

        public IncubationFollowUpEvaluationController()
        {
            helper = new CommonHelper();
            repository = new Repository(new KingkhaledFoundationDB());
        }

        // GET: IncubationFollowUpEvaluation
        public ActionResult Index(string Type)
        {
            ViewBag.Type = Type;
            if (Type == "Incubation")

                ViewBag.Title = App_GlobalResources.General.Incubations + " - " + App_GlobalResources.General.FollowUpEvaluation;
            else
                ViewBag.Title = App_GlobalResources.General.Accelerations + " - " + App_GlobalResources.General.FollowUpEvaluation;

            var IncubFollowEval = repository.GetQuery<IncubationFollowUpEvaluation>().Where(i => i.IncubationType.NameEN == Type&&!i.Incubation.IncubationAdvertising.IsDeleted).ToList();

            return View(IncubFollowEval);
        }

        [HttpGet]
        public ActionResult AppendLst()
        {
            var EvaluationLines = new IncubationFollowUpEvaluationLines();
            var users = repository.GetQuery<BackendUser>(f => f.IsActive == true).Include(u => u.AspNetUser).Include(u => u.AspNetUser.AspNetRoles).Where(f => f.AspNetUser.AspNetRoles.Any(r => r.Name == "CB Manager" || r.Name == "CB Supervisor" || r.Name == "CB Analyst")).ToList();
            ViewBag.adminUserID = new SelectList(users, "BackendUserID", "UserName");
            return PartialView("_LstIncubationFollowUpEvaluationLines", EvaluationLines);
        }

        // GET: IncubationFollowUpEvaluation/Create
        public ActionResult Create(Guid? id, string Type)
        {
            TempData["Type"] = Type;
            ViewBag.Type = Type;
            ViewBag.Title = "Create" + Type + "FollowUp Evaluation";
            if (Type == "Incubation")

                ViewBag.Title = App_GlobalResources.General.Create + " " + App_GlobalResources.General.Incubations + " " + App_GlobalResources.General.FollowUpEvaluation;
            else
                ViewBag.Title = App_GlobalResources.General.Create + " " + App_GlobalResources.General.Accelerations + " " + App_GlobalResources.General.FollowUpEvaluation;

            var Program = repository.GetQuery<Program>(r => r.ProgramName == "Capacity Building").Select(r => r.ProgramID).FirstOrDefault();
            ViewBag.CorporationUserID = new SelectList(repository.GetQuery<FrontendUser>(f => f.IsActive == true && f.CorporateApplicationForms.Any(c => c.ProgramID == Program)).ToList(), "UserID", "AspNetUser.UserName");  /*CorporateApplicationForms.FirstOrDefault().Name*/

            ViewBag.IncubationID = new SelectList(repository.GetQuery<Incubation>().Where(t=>!t.IncubationAdvertising.IsDeleted).ToList(), "IncubationID", "Name");
            var users = repository.GetQuery<BackendUser>(f => f.IsActive == true).Include(u => u.AspNetUser).Include(u => u.AspNetUser.AspNetRoles).Where(f => f.AspNetUser.AspNetRoles.Any(r => r.Name == "CB Manager" || r.Name == "CB Supervisor" || r.Name == "CB Analyst")).ToList();
            ViewBag.adminUserID = new SelectList(users, "BackendUserID", "UserName");
            IncubationFollowUpEvaluationVM IFEVM = new IncubationFollowUpEvaluationVM()
            {
                IFEVM = new IncubationFollowUpEvaluation(),
                LstIFELVM = new List<IncubationFollowUpEvaluationLines>()
            };
            return View(IFEVM);
        }

        [HttpPost]
        public ActionResult Create(IncubationFollowUpEvaluationVM model)
        {
            string type = TempData["Type"].ToString();
            TempData.Keep();
            ViewBag.Type = type;
            var IncubationTypeID = type != null ? repository.GetQuery<IncubationType>().FirstOrDefault(t => t.NameEN == type).IncubationTypeID : repository.GetQuery<IncubationType>().FirstOrDefault(t => t.NameEN == "Incubation").IncubationTypeID;

            var Program = repository.GetQuery<Program>(r => r.ProgramName == "Capacity Building").Select(r => r.ProgramID).FirstOrDefault();
            ViewBag.CorporationUserID = new SelectList(repository.GetQuery<FrontendUser>(f => f.IsActive == true && f.CorporateApplicationForms.Any(c => c.ProgramID == Program)).ToList(), "UserID", "AspNetUser.UserName");  /*CorporateApplicationForms.FirstOrDefault().Name*/

            ViewBag.IncubationID = new SelectList(repository.GetQuery<Incubation>().Where(t => !t.IncubationAdvertising.IsDeleted).ToList(), "IncubationID", "Name");
            var users = repository.GetQuery<BackendUser>(f => f.IsActive == true).Include(u => u.AspNetUser).Include(u => u.AspNetUser.AspNetRoles).Where(f => f.AspNetUser.AspNetRoles.Any(r => r.Name == "CB Manager" || r.Name == "CB Supervisor" || r.Name == "CB Analyst")).ToList();
            ViewBag.adminUserID = new SelectList(users, "BackendUserID", "UserName");

            var entity = new IncubationFollowUpEvaluation()
            {
                IncubationFollowUpEvaluationID = Guid.NewGuid(),
                FrontendUserID = model.IFEVM.FrontendUserID,
                IncubationTypeID = IncubationTypeID,
                Impact = model.IFEVM.Impact,
                IncubationID = model.IFEVM.IncubationID,
                Reasone = model.IFEVM.Reasone,
                ProjectSuccessRate = model.IFEVM.ProjectSuccessRate,
                SuccessStories = model.IFEVM.SuccessStories,
            };

            if (model.LstIFELVM != null)
            {
                foreach (var incubationLine in model.LstIFELVM)
                {
                    entity.IncubationFollowUpEvaluationLines.Add(new IncubationFollowUpEvaluationLines()
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
                                IncubationFollowUpEvaluationAttchment _Attach = new IncubationFollowUpEvaluationAttchment();
                                _Attach.AttachmentID = Guid.NewGuid();
                                _Attach.IncubationFollowUpEvaluationID = entity.IncubationFollowUpEvaluationID;
                                _Attach.Name = Path.GetFileName(model.files[i].FileName);
                                _Attach.ScreenName = "Incubation FollowUp Evaluation";
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
        public ActionResult Edit(Guid IncubationFollowUpEvaluationID, string Type)
        {
            TempData["Type"] = Type;
            ViewBag.Type = Type;
            if (Type == "Incubation")
                ViewBag.Title = $"{App_GlobalResources.General.Update} {App_GlobalResources.General.Incubations}";
            else
                ViewBag.Title = $"{App_GlobalResources.General.Update} {App_GlobalResources.General.Incubations}";

            var Entity = repository.GetQuery<IncubationFollowUpEvaluation>(i => i.IncubationFollowUpEvaluationID == IncubationFollowUpEvaluationID).Include(c => c.IncubationFollowUpEvaluationLines).SingleOrDefault();
            IncubationFollowUpEvaluationVM model = new IncubationFollowUpEvaluationVM();

            ViewBag.CorporationUserID = new SelectList(repository.GetQuery<FrontendUser>(f => f.IsActive == true).ToList(), "UserID", "AspNetUser.UserName");  /*CorporateApplicationForms.FirstOrDefault().Name*/
            ViewBag.IncubationID = new SelectList(repository.GetQuery<Incubation>().Where(t => !t.IncubationAdvertising.IsDeleted).ToList(), "IncubationID", "Name");
            var users = repository.GetQuery<BackendUser>(f => f.IsActive == true).Include(u => u.AspNetUser).Include(u => u.AspNetUser.AspNetRoles).Where(f => f.AspNetUser.AspNetRoles.Any(r => r.Name == "CB Manager" || r.Name == "CB Supervisor" || r.Name == "CB Analyst")).ToList();
            ViewBag.adminUserID = new SelectList(users, "BackendUserID", "UserName");
            model.IFEVM = Entity;
            model.LstIFELVM = Entity.IncubationFollowUpEvaluationLines.ToList();

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(IncubationFollowUpEvaluationVM Model)
        {
            string type = TempData["Type"].ToString();
            TempData.Keep();
            ViewBag.Type = type;
            var Entity = repository.GetQuery<IncubationFollowUpEvaluation>(i => i.IncubationFollowUpEvaluationID == Model.IFEVM.IncubationFollowUpEvaluationID).Include(c => c.IncubationFollowUpEvaluationLines).SingleOrDefault();

            var liensCollections = Entity.IncubationFollowUpEvaluationLines.ToList();
            if (Entity.IncubationFollowUpEvaluationLines != null)
            {
                foreach (var IncubationFollowUpEvaluationLine in liensCollections)
                    repository.Delete(IncubationFollowUpEvaluationLine);
            }

            repository.UnitOfWork.SaveChanges();

            Entity.Impact = Model.IFEVM.Impact;
            Entity.IncubationID = Model.IFEVM.IncubationID;
            Entity.IncubationTypeID = Model.IFEVM.IncubationTypeID;
            Entity.Reasone = Model.IFEVM.Reasone;
            Entity.SuccessStories = Model.IFEVM.SuccessStories;
            Entity.ProjectSuccessRate = Model.IFEVM.ProjectSuccessRate;

            repository.Update(Entity);
            repository.UnitOfWork.SaveChanges();

            if (Model.LstIFELVM != null)
            {
                foreach (var line in Model.LstIFELVM)
                {
                    repository.Add(new IncubationFollowUpEvaluationLines()
                    {
                        Id = Guid.NewGuid(),
                        BackendUserID = line.BackendUserID,
                        Date = line.Date,
                        FollowUpMethod = line.FollowUpMethod,
                        IncubationFollowUpEvaluationID = Entity.IncubationFollowUpEvaluationID,
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
                        IncubationFollowUpEvaluationAttchment _Attach = new IncubationFollowUpEvaluationAttchment();
                        _Attach.AttachmentID = Guid.NewGuid();
                        _Attach.IncubationFollowUpEvaluationID = Entity.IncubationFollowUpEvaluationID;
                        _Attach.Name = Path.GetFileName(Model.files[i].FileName);
                        _Attach.ScreenName = "Incubation FollowUp Evaluation";
                        _Attach.Size = Model.files[i].ContentLength.ToString();
                        _Attach.URL = path + Path.GetFileName(Model.files[i].FileName);
                        _Attach.Type = Model.files[i].ContentType;
                        repository.Add(_Attach);
                    }
                }
                ViewBag.Message = "File uploaded successfully.";
            }

            repository.UnitOfWork.SaveChanges();
            if (type != "" || type != null)
                return RedirectToAction("index", new { Type = type });
            return RedirectToAction("index", new { Type = "Incubation" });
        }

        public FileResult Download(Guid id)
        {
            var attachmet = repository.GetByKey<IncubationFollowUpEvaluationAttchment>(id);
            byte[] fileBytes = System.IO.File.ReadAllBytes(attachmet.URL);
            string fileName = attachmet.Name;
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        [HttpGet]
        public JsonResult DeleteAttachment(Guid Id)
        {
            try
            {
                var Attachment = repository.GetByKey<IncubationFollowUpEvaluationAttchment>(Id);
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