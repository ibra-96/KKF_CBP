using System;
using System.IO;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using AlphaPeople.Core;
using System.Transactions;
using System.Configuration;
using AlphaPeople.Repository;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using AlfaPeople.KingKhalidFoundation.Data;
using AlfaPeople.KingKhalidFoundation.Data.Model;
using AlfaPeople.KingKhaledFoundation.Admin.Web.Helper;
using AlfaPeople.KingKhaledFoundation.Admin.Web.Models;

namespace AlfaPeople.KingKhaledFoundation.Admin.Web.Controllers
{
    [Authorize(Roles = "Admin, CB Manager, CB Supervisor, CB Analyst")]
    public class IncubationsController : BaseController
    {
        private readonly IRepository repository;
        private readonly CommonHelper helper;
        private readonly string Logpath = ConfigurationManager.AppSettings["Log"].ToString();

        public IncubationsController()
        {
            helper = new CommonHelper();
            repository = new Repository(new KingkhaledFoundationDB());
        }

        // GET: Incubations
        public ActionResult Index(string Type)
        {
            if (Type == "Incubation")
                ViewBag.Title = App_GlobalResources.General.IncubationProjects;
            else
                ViewBag.Title = App_GlobalResources.General.AccelerationProjects;
            ViewBag.Type = Type;
            var incubation = repository.GetQuery<Incubation>(f => f.IncubationType.NameEN == Type&&!f.IncubationAdvertising.IsDeleted).ToList();
            return View(incubation);
        }

        // GET: Incubations/Create
        public ActionResult Create(string Type)
        {
            if (Type == "Incubation")
                ViewBag.Title = App_GlobalResources.General.NewIncubationProject;
            else
                ViewBag.Title = App_GlobalResources.General.NewAccelerationProject;

            TempData["Type"] = Type;
            ViewBag.Type = Type;

            ViewBag.ConsultantID = new SelectList(repository.GetQuery<Consultant>(f => f.IsActive == true), "ConsultantID", "Name");

            if (CultureHelper.CurrentCulture == 3)
            {
                ViewBag.TypeOfIntervention = new SelectList(repository.GetQuery<TypeOfIntervention>(f => f.IsActive == true), "TypeOfInterventionID", "TypeOfInterventionNameAR");
            }
            else
            {
                ViewBag.TypeOfIntervention = new SelectList(repository.GetQuery<TypeOfIntervention>(f => f.IsActive == true), "TypeOfInterventionID", "TypeOfInterventionNameEN");
            }

            ViewBag.IncubationAdID = new SelectList(repository.GetQuery<IncubationAdvertising>(f => f.IncubationType.NameEN == Type && f.IsActive == true && f.Incubations.Count != f.IncubationAdvertisingModels.Count), "IncubationAdID", "Name");
            ViewBag.SpecialistChargeOfIncubation = new SelectList(repository.GetQuery<BackendUser>(f => f.AspNetUser.AspNetRoles.Any(r =>/* r.Name == "Admin"||*/ r.Name == "CB Manager" || r.Name == "CB Supervisor" || r.Name == "CB Analyst")).ToList(), "BackendUserID", "UserName");
            return View();
        }

        // POST: Incubations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IncuationsVM model)
        {
            string Type = TempData["Type"].ToString();
            TempData.Keep();
            ViewBag.Type = Type;
            if (Type == "Incubation")
                ViewBag.Title = App_GlobalResources.General.NewIncubationProject;
            else
                ViewBag.Title = App_GlobalResources.General.NewAccelerationProject;

            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();
                try
                {
                    model.incubation.IncubationID = Guid.NewGuid();
                    model.incubation.IncubationTypeID = repository.GetQuery<IncubationType>(f => f.NameEN == Type).FirstOrDefault().IncubationTypeID;
                    model.incubation.IncubationStatusID = repository.GetQuery<IncubationStatus>(f => f.NameEN == "Active").FirstOrDefault().IncubationStatusID;
                    using (TransactionScope scope = new TransactionScope())
                    {
                        try
                        {
                            repository.Add(model.incubation);
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
                                        IncubationAttchment _Attach = new IncubationAttchment();
                                        _Attach.AttachmentID = Guid.NewGuid();
                                        _Attach.IncubationID = model.incubation.IncubationID;
                                        _Attach.Name = Path.GetFileName(model.files[i].FileName);
                                        _Attach.ScreenName = "Incubations Projects";
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
                            ViewBag.ConsultantID = new SelectList(repository.GetQuery<Consultant>(f => f.IsActive == true), "ConsultantID", "Name");
                            if (CultureHelper.CurrentCulture == 3)
                            {
                                ViewBag.TypeOfIntervention = new SelectList(repository.GetQuery<TypeOfIntervention>(f => f.IsActive == true), "TypeOfInterventionID", "TypeOfInterventionNameAR");
                            }
                            else
                            {
                                ViewBag.TypeOfIntervention = new SelectList(repository.GetQuery<TypeOfIntervention>(f => f.IsActive == true), "TypeOfInterventionID", "TypeOfInterventionNameEN");
                            }

                            ViewBag.IncubationAdID = new SelectList(repository.GetQuery<IncubationAdvertising>(f => f.IncubationType.NameEN == Type && f.IsActive == true && f.Incubations.Count != f.IncubationAdvertisingModels.Count), "IncubationAdID", "Name");
                            ViewBag.SpecialistChargeOfIncubation = new SelectList(repository.GetQuery<BackendUser>(f => f.AspNetUser.AspNetRoles.Any(r =>/* r.Name == "Admin"||*/ r.Name == "CB Manager" || r.Name == "CB Supervisor" || r.Name == "CB Analyst")).ToList(), "BackendUserID", "UserName");
                            return View(model);
                        }
                    }
                }
                catch (Exception)
                {
                    ViewBag.ConsultantID = new SelectList(repository.GetQuery<Consultant>(f => f.IsActive == true), "ConsultantID", "Name");
                    ViewBag.IncubationAdID = new SelectList(repository.GetQuery<IncubationAdvertising>(f => f.IncubationType.NameEN == Type && f.IsActive == true && f.Incubations.Count != f.IncubationAdvertisingModels.Count), "IncubationAdID", "Name");
                    if (CultureHelper.CurrentCulture == 3)
                    {
                        ViewBag.TypeOfIntervention = new SelectList(repository.GetQuery<TypeOfIntervention>(f => f.IsActive == true), "TypeOfInterventionID", "TypeOfInterventionNameAR");
                    }
                    else
                    {
                        ViewBag.TypeOfIntervention = new SelectList(repository.GetQuery<TypeOfIntervention>(f => f.IsActive == true), "TypeOfInterventionID", "TypeOfInterventionNameEN");
                    }
                    ViewBag.SpecialistChargeOfIncubation = new SelectList(repository.GetQuery<BackendUser>(f => f.AspNetUser.AspNetRoles.Any(r =>/* r.Name == "Admin"||*/ r.Name == "CB Manager" || r.Name == "CB Supervisor" || r.Name == "CB Analyst")).ToList(), "BackendUserID", "UserName");
                    return View(model);
                }
            }
            return RedirectToAction("Index", new { Type = Type });
        }

        // GET: Incuations/Edit/5
        public ActionResult Edit(Guid id, string Type)
        {
            TempData["Type"] = Type;
            ViewBag.Type = Type;

            if (id == null)
                return View();

            Incubation incubation = repository.GetByKey<Incubation>(id);

            if (incubation == null)
                return HttpNotFound();
            if (CultureHelper.CurrentCulture == 3)
            {
                ViewBag.IncubationModelID = new SelectList(repository.GetQuery<IncubationAdvertisingModel>(f => (f.IncubationAdID == incubation.IncubationAdID && !f.IncubationAdvertising.Incubations.Any(i => i.IncubationModelID == f.IncubationModelID)) || (f.IncubationAdID == incubation.IncubationAdID && f.IncubationModelID == incubation.IncubationModelID)).ToList(), "IncubationModel.IncubationModelID", "IncubationModel.NameAR", incubation.IncubationModelID);
                ViewBag.TypeOfIntervention = new SelectList(repository.GetQuery<TypeOfIntervention>(f => f.IsActive == true), "TypeOfInterventionID", "TypeOfInterventionNameAR", incubation.TypeOfInterventionID);
            }
            else
            {
                ViewBag.IncubationModelID = new SelectList(repository.GetQuery<IncubationAdvertisingModel>(f => (f.IncubationAdID == incubation.IncubationAdID && !f.IncubationAdvertising.Incubations.Any(i => i.IncubationModelID == f.IncubationModelID)) || (f.IncubationAdID == incubation.IncubationAdID && f.IncubationModelID == incubation.IncubationModelID)).ToList(), "IncubationModel.IncubationModelID", "IncubationModel.NameEN", incubation.IncubationModelID);
                ViewBag.TypeOfIntervention = new SelectList(repository.GetQuery<TypeOfIntervention>(f => f.IsActive == true), "TypeOfInterventionID", "TypeOfInterventionNameEN", incubation.TypeOfInterventionID);
            }

            ViewBag.Title = App_GlobalResources.General.IncubationProjects + " " + incubation.Name + " " + App_GlobalResources.General.Update;
            ViewBag.ConsultantID = new SelectList(repository.GetQuery<Consultant>(f => f.IsActive == true), "ConsultantID", "Name", incubation.ConsultantID);
            ViewBag.IncubationAdID = new SelectList(repository.GetQuery<IncubationAdvertising>(f => (f.IncubationType.NameEN == Type && f.IsActive == true && f.Incubations.Count != f.IncubationAdvertisingModels.Count) || (f.IncubationAdID == incubation.IncubationAdID)), "IncubationAdID", "Name", incubation.IncubationAdID);
            ViewBag.IncubationProjectProposalID = new SelectList(repository.GetQuery<IncubationProjectProposal>(f => (f.IncubationAdID == incubation.IncubationAdID && f.IncubationProjectProposalStatu.NameEN == "Accepted" && f.IncubationAdvertising.IncubationAdvertisingModels.Count != f.Incubations.Count) || (f.IncubationProjectProposalID == incubation.IncubationProjectProposalID)).ToList(), "IncubationProjectProposalID", "FrontendUser.AspNetUser.UserName", incubation.IncubationProjectProposalID); /*FrontendUser.CorporateApplicationForms.FirstOrDefault().Name*/
            ViewBag.SpecialistChargeOfIncubation = new SelectList(repository.GetQuery<BackendUser>(f => f.AspNetUser.AspNetRoles.Any(r =>/* r.Name == "Admin"||*/ r.Name == "CB Manager" || r.Name == "CB Supervisor" || r.Name == "CB Analyst")).ToList(), "BackendUserID", "UserName");
            ViewBag.startDate = incubation.StartDate.ToString("yyyy-MM-dd");
            ViewBag.endDate = incubation.EndDate.ToString("yyyy-MM-dd");

            IncuationsVM incuationsVM = new IncuationsVM();
            incuationsVM.incubation = incubation;
            return View(incuationsVM);
        }

        // POST: Grants/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(IncuationsVM model)
        {
            string Type = TempData["Type"].ToString();
            TempData.Keep();
            ViewBag.Type = Type;
            ViewBag.Title = App_GlobalResources.General.Update + " " + model.incubation.Name + " " + App_GlobalResources.General.Grant;

            if (ModelState.IsValid)
            {
                try
                {
                    repository.Update(model.incubation);

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
                                IncubationAttchment _Attach = new IncubationAttchment();
                                _Attach.AttachmentID = Guid.NewGuid();
                                _Attach.IncubationID = model.incubation.IncubationID;
                                _Attach.Name = Path.GetFileName(model.files[i].FileName);
                                _Attach.ScreenName = "Incubations Projects";
                                _Attach.Size = model.files[i].ContentLength.ToString();
                                _Attach.URL = path + Path.GetFileName(model.files[i].FileName);
                                _Attach.Type = model.files[i].ContentType;
                                repository.Add(_Attach);
                            }
                        }
                    }

                    repository.UnitOfWork.SaveChanges();
                    return RedirectToAction("Index", new { Type = Type });
                }
                catch (Exception)
                {
                    ViewBag.ConsultantID = new SelectList(repository.GetQuery<Consultant>(f => f.IsActive == true), "ConsultantID", "Name");

                    if (CultureHelper.CurrentCulture == 3)
                    {
                        ViewBag.IncubationModelID = new SelectList(repository.GetQuery<IncubationAdvertisingModel>(f => (f.IncubationAdID == model.incubation.IncubationAdID && !f.IncubationAdvertising.Incubations.Any(i => i.IncubationModelID == f.IncubationModelID)) || (f.IncubationAdID == model.incubation.IncubationAdID && f.IncubationModelID == model.incubation.IncubationModelID)).ToList(), "IncubationModel.IncubationModelID", "IncubationModel.NameAR", model.incubation.IncubationModelID);
                        ViewBag.TypeOfIntervention = new SelectList(repository.GetQuery<TypeOfIntervention>(f => f.IsActive == true), "TypeOfInterventionID", "TypeOfInterventionNameAR");
                    }
                    else
                    {
                        ViewBag.IncubationModelID = new SelectList(repository.GetQuery<IncubationAdvertisingModel>(f => (f.IncubationAdID == model.incubation.IncubationAdID && !f.IncubationAdvertising.Incubations.Any(i => i.IncubationModelID == f.IncubationModelID)) || (f.IncubationAdID == model.incubation.IncubationAdID && f.IncubationModelID == model.incubation.IncubationModelID)).ToList(), "IncubationModel.IncubationModelID", "IncubationModel.NameEN", model.incubation.IncubationModelID);
                        ViewBag.TypeOfIntervention = new SelectList(repository.GetQuery<TypeOfIntervention>(f => f.IsActive == true), "TypeOfInterventionID", "TypeOfInterventionNameEN");
                    }

                    ViewBag.IncubationAdID = new SelectList(repository.GetQuery<IncubationAdvertising>(f => (f.IncubationType.NameEN == Type && f.IsActive == true && f.Incubations.Count != f.IncubationAdvertisingModels.Count) || (f.IncubationAdID == model.incubation.IncubationAdID)), "IncubationAdID", "Name", model.incubation.IncubationAdID);
                    ViewBag.IncubationProjectProposalID = new SelectList(repository.GetQuery<IncubationProjectProposal>(f => (f.IncubationAdID == model.incubation.IncubationAdID && f.IncubationProjectProposalStatu.NameEN == "Accepted" && f.IncubationAdvertising.IncubationAdvertisingModels.Count != f.Incubations.Count) || (f.IncubationProjectProposalID == model.incubation.IncubationProjectProposalID)).ToList(), "IncubationProjectProposalID", "FrontendUser.AspNetUser.UserName", model.incubation.IncubationProjectProposalID); /*FrontendUser.CorporateApplicationForms.FirstOrDefault().Name*/
                    ViewBag.SpecialistChargeOfIncubation = new SelectList(repository.GetQuery<BackendUser>(f => f.AspNetUser.AspNetRoles.Any(r =>/* r.Name == "Admin"||*/ r.Name == "CB Manager" || r.Name == "CB Supervisor" || r.Name == "CB Analyst")).ToList(), "BackendUserID", "UserName");
                    ViewBag.startDate = model.incubation.StartDate.ToString("yyyy-MM-dd");
                    ViewBag.endDate = model.incubation.EndDate.ToString("yyyy-MM-dd");
                    return View(model);
                }
            }

            ViewBag.ConsultantID = new SelectList(repository.GetQuery<Consultant>(f => f.IsActive == true), "ConsultantID", "Name");
            if (CultureHelper.CurrentCulture == 3)
            {
                ViewBag.IncubationModelID = new SelectList(repository.GetQuery<IncubationAdvertisingModel>(f => (f.IncubationAdID == model.incubation.IncubationAdID && !f.IncubationAdvertising.Incubations.Any(i => i.IncubationModelID == f.IncubationModelID)) || (f.IncubationAdID == model.incubation.IncubationAdID && f.IncubationModelID == model.incubation.IncubationModelID)).ToList(), "IncubationModel.IncubationModelID", "IncubationModel.NameAR", model.incubation.IncubationModelID);
                ViewBag.TypeOfIntervention = new SelectList(repository.GetQuery<TypeOfIntervention>(f => f.IsActive == true), "TypeOfInterventionID", "TypeOfInterventionNameAR");
            }
            else
            {
                ViewBag.IncubationModelID = new SelectList(repository.GetQuery<IncubationAdvertisingModel>(f => (f.IncubationAdID == model.incubation.IncubationAdID && !f.IncubationAdvertising.Incubations.Any(i => i.IncubationModelID == f.IncubationModelID)) || (f.IncubationAdID == model.incubation.IncubationAdID && f.IncubationModelID == model.incubation.IncubationModelID)).ToList(), "IncubationModel.IncubationModelID", "IncubationModel.NameEN", model.incubation.IncubationModelID);
                ViewBag.TypeOfIntervention = new SelectList(repository.GetQuery<TypeOfIntervention>(f => f.IsActive == true), "TypeOfInterventionID", "TypeOfInterventionNameEN");
            }

            ViewBag.IncubationAdID = new SelectList(repository.GetQuery<IncubationAdvertising>(f => (f.IncubationType.NameEN == Type && f.IsActive == true && f.Incubations.Count != f.IncubationAdvertisingModels.Count) || (f.IncubationAdID == model.incubation.IncubationAdID)), "IncubationAdID", "Name", model.incubation.IncubationAdID);
            ViewBag.IncubationProjectProposalID = new SelectList(repository.GetQuery<IncubationProjectProposal>(f => (f.IncubationAdID == model.incubation.IncubationAdID && f.IncubationProjectProposalStatu.NameEN == "Accepted" && f.IncubationAdvertising.IncubationAdvertisingModels.Count != f.Incubations.Count) || (f.IncubationProjectProposalID == model.incubation.IncubationProjectProposalID)).ToList(), "IncubationProjectProposalID", "FrontendUser.AspNetUser.UserName", model.incubation.IncubationProjectProposalID); /*FrontendUser.CorporateApplicationForms.FirstOrDefault().Name*/
            ViewBag.SpecialistChargeOfIncubation = new SelectList(repository.GetQuery<BackendUser>(f => f.AspNetUser.AspNetRoles.Any(r =>/* r.Name == "Admin"||*/ r.Name == "CB Manager" || r.Name == "CB Supervisor" || r.Name == "CB Analyst")).ToList(), "BackendUserID", "UserName");
            ViewBag.startDate = model.incubation.StartDate.ToString("yyyy-MM-dd");
            ViewBag.endDate = model.incubation.EndDate.ToString("yyyy-MM-dd");
            return View(model);
        }

        [HttpGet]
        public JsonResult getIncubationAdvertisingAcceptedCorp(Guid incubationAdId)
        {
            if (!string.IsNullOrWhiteSpace(incubationAdId.ToString()) && incubationAdId.ToString().Length != 0)
            {
                IEnumerable<SelectListItem> IncubationPP = repository.Get<IncubationProjectProposal>(f => f.IncubationAdID == incubationAdId && f.IncubationProjectProposalStatu.NameEN == "Accepted" && f.IncubationAdvertising.IncubationAdvertisingModels.Count != f.Incubations.Count)
                     .Select(n =>
                      new SelectListItem
                      {
                          Value = n.IncubationProjectProposalID.ToString(),
                          Text = n.FrontendUser.CorporateApplicationForms.FirstOrDefault().Name
                      }).ToList();

                return Json(new IncAdModVM { IncubationPP = new SelectList(IncubationPP, "Value", "Text") }, JsonRequestBehavior.AllowGet);
            }
            return null;
        }

        [HttpGet]
        public JsonResult getIncubationAdvertisingModel(Guid incubationAdId, Guid incubationPPId)
        {
            if (!string.IsNullOrWhiteSpace(incubationAdId.ToString()) && incubationAdId.ToString().Length != 0)
            {
                IEnumerable<SelectListItem> IncubationAdModel = repository.Get<IncubationAdvertisingModel>(f => f.IncubationAdID == incubationAdId && !f.IncubationAdvertising.Incubations.Any(i => i.IncubationModelID == f.IncubationModelID && i.IncubationProjectProposalID == incubationPPId))
                 .Select(n =>
                  new SelectListItem
                  {
                      Value = n.IncubationModel.IncubationModelID.ToString(),
                      Text = CultureHelper.CurrentCulture != 3 ? n.IncubationModel.NameEN : n.IncubationModel.NameAR
                  }).ToList();
                return Json(new IncAdModVM { IncubationAdModel = new SelectList(IncubationAdModel, "Value", "Text") }, JsonRequestBehavior.AllowGet);
            }
            return null;
        }

        public FileResult Download(Guid id)
        {
            var attachmet = repository.GetByKey<IncubationAttchment>(id);
            byte[] fileBytes = System.IO.File.ReadAllBytes(attachmet.URL);
            string fileName = attachmet.Name;
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        [HttpGet]
        public JsonResult DeleteAttachment(Guid Id)
        {
            try
            {
                var Attachment = repository.GetByKey<IncubationAttchment>(Id);
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

        public ActionResult IncubationRequests()
        {
            return View(repository.GetQuery<Incubation>(f => f.IncubationStatus.NameEN == "Pending" && f.IncubationType.NameEN == "Incubation" && !f.IncubationAdvertising.IsDeleted).ToList());
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