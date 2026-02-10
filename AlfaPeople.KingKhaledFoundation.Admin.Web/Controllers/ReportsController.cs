using System;
using System.IO;
using System.Linq;
using System.Data;
using System.Web.Mvc;
using ClosedXML.Excel;
using AlphaPeople.Core;
using System.Data.Entity;
using System.Configuration;
using AlphaPeople.Repository;
using System.Collections.Generic;
using AlfaPeople.KingKhalidFoundation.Data;
using AlfaPeople.KingKhalidFoundation.Data.Model;
using AlfaPeople.KingKhaledFoundation.Admin.Web.Models;
using AlfaPeople.KingKhaledFoundation.Admin.Web.Helper;

namespace AlfaPeople.KingKhaledFoundation.Admin.Web.Controllers
{
    [Authorize(Roles = "Admin, CB Manager, CB Supervisor, CB Analyst")]
    public class ReportsController : BaseController
    {
        #region MemmbersAndFields
        private readonly IRepository repository;
        private readonly CommonHelper helper;
        private readonly string Logpath = ConfigurationManager.AppSettings["Log"].ToString();
        #endregion

        #region Controllers
        public ReportsController()
        {
            repository = new Repository(new KingkhaledFoundationDB());
            helper = new CommonHelper();
        }
        #endregion

        #region Actions

        public ActionResult ChartArrayBasic()
        {
            return View();
        }

        #region Capacity Building Reports

        #region CorporationReport
        [HttpGet]
        public ActionResult Corporations()
        {
            ViewBag.Corporations = new SelectList(repository.GetAll<CorporateApplicationForm>().Where(c=>c.IsDraft!=true).ToList(), "CorporateApplicationFormID", "Name");
            ViewBag.CorporationsCategoryID = new SelectList(repository.GetAll<CorporationsCategory>().ToList(), "CorporationsCategoryID", CultureHelper.CurrentCulture != 3 ? "CorporationsCategoryNameEN" : "CorporationsCategoryNameAR");
            ViewBag.GovernorateID = new SelectList(repository.GetQuery<Governorate>(f => f.IsActive == true), "GovernorateID", CultureHelper.CurrentCulture != 3 ? "GovernorateEN" : "GovernorateAR");
            ViewBag.CorporateFieldOfWorkName = new SelectList(repository.GetQuery<CorporateFieldOfWork>(f => f.IsActive == true), "CorporateFieldOfWorkID", CultureHelper.CurrentCulture != 3 ? "CorporateFieldOfWorkNameEN" : "CorporateFieldOfWorkNameAR");
            CorporationsResult Model = new CorporationsResult();
            return View(Model);
        }

        [HttpPost]
        public ActionResult Corporations(CorporationsResult model)
        {
            ViewBag.Corporations = new SelectList(repository.GetAll<CorporateApplicationForm>().Where(c=>c.IsDraft!=true).ToList(), "CorporateApplicationFormID", "Name");
            ViewBag.CorporationsCategoryID = new SelectList(repository.GetAll<CorporationsCategory>().ToList(), "CorporationsCategoryID", CultureHelper.CurrentCulture != 3 ? "CorporationsCategoryNameEN" : "CorporationsCategoryNameAR");
            ViewBag.GovernorateID = new SelectList(repository.GetQuery<Governorate>(f => f.IsActive == true), "GovernorateID", CultureHelper.CurrentCulture != 3 ? "GovernorateEN" : "GovernorateAR");
            ViewBag.CorporateFieldOfWorkName = new SelectList(repository.GetQuery<CorporateFieldOfWork>(f => f.IsActive == true), "CorporateFieldOfWorkID", CultureHelper.CurrentCulture != 3 ? "CorporateFieldOfWorkNameEN" : "CorporateFieldOfWorkNameAR");
            List<CorporateApplicationForm> Corporates = repository.GetAll<CorporateApplicationForm>().Where(c => c.IsDraft != true).ToList();
            if ((model.SearchDto.DateFrom.HasValue && model.SearchDto.DateTo.HasValue) || model.SearchDto.CorporationsCategoryID.HasValue ||
                model.SearchDto.GenderType.HasValue || model.SearchDto.Government.HasValue || model.SearchDto.CorporationName.HasValue || model.SearchDto.CorporateFieldOfWorkID.HasValue)
            {
                if (model.SearchDto.DateFrom.HasValue && model.SearchDto.DateTo.HasValue)
                {
                    Corporates = Corporates.Where(c => c.CorporateApplicationStatu.DateTimeMakeAction >= model.SearchDto.DateFrom.Value && c.CorporateApplicationStatu.DateTimeMakeAction <= model.SearchDto.DateTo.Value).ToList();
                }

                if (model.SearchDto.CorporationsCategoryID.HasValue)
                {
                    Corporates = Corporates.Where(c => c.CorporationsCategoryID == model.SearchDto.CorporationsCategoryID.Value).ToList();
                }

                if (model.SearchDto.CorporateFieldOfWorkID.HasValue)
                {
                    Corporates = Corporates.Where(c => c.CorporateFieldOfWorkID == model.SearchDto.CorporateFieldOfWorkID.Value).ToList();
                }

                if (model.SearchDto.GenderType.HasValue)
                {
                    Corporates = Corporates.Where(c => c.corporateGenderType == model.SearchDto.GenderType.Value).ToList();
                }

                if (model.SearchDto.Government.HasValue)
                {
                    Corporates = Corporates.Where(c => c.GovernorateID == model.SearchDto.Government.Value).ToList();
                }

                if (model.SearchDto.CorporationName.HasValue)
                {
                    Corporates = Corporates.Where(c => c.CorporateApplicationFormID == model.SearchDto.CorporationName.Value).ToList();
                }
            }
            if (Corporates.Count > 0)
            {
                var WorkshopProjectProposalsCorporates = Corporates.Where(e => e.FrontendUser.WorkshopProjectProposals.Where(c => c.WorkshopProjectProposalStatu.NameEN == "Accepted").Count() > 0);
                foreach (var IncubationCorporate in Corporates)
                {
                    foreach (var Incubation in IncubationCorporate.FrontendUser.IncubationProjectProposals.Where(i => i.IncubationProjectProposalStatu.NameEN == "Accepted"))
                    {
                        model.Corporations.Add(new CorporationDto()
                        {
                            Corporation = IncubationCorporate.Name,
                            CorporationCategory = CultureHelper.CurrentCulture != 3 ? IncubationCorporate.CorporationsCategory.CorporationsCategoryNameEN : IncubationCorporate.CorporationsCategory.CorporationsCategoryNameAR,
                            Governate = CultureHelper.CurrentCulture != 3 ? IncubationCorporate.Governorate.GovernorateEN : IncubationCorporate.Governorate.GovernorateAR,
                            ProjectName = CultureHelper.CurrentCulture != 3 ? $" {Incubation.IncubationAdvertising.IncubationType.NameEN } - {Incubation.IncubationAdvertising.Name}" : $"{Incubation.IncubationAdvertising.IncubationType.NameAR } - {Incubation.IncubationAdvertising.Name}",
                            CorporationVolume = IncubationCorporate.FrontendUser.IncubationBaselines.Sum(r => r.FullTimeStaff.Value + r.PartTimeStaff.Value + r.VolunteerStaff.Value),
                            Age =(IncubationCorporate?.FoundedYear.HasValue == true) ? DateTime.Now.Year - IncubationCorporate.FoundedYear.Value.Year : 0,

                            FieldOfWork = CultureHelper.CurrentCulture != 3 ? IncubationCorporate.CorporateFieldOfWork.CorporateFieldOfWorkNameEN : IncubationCorporate.CorporateFieldOfWork.CorporateFieldOfWorkNameAR,
                            Gender = GetGenderName(IncubationCorporate.corporateGenderType)
                        });
                    }
                }

                foreach (var WorkshopProjectProposalsCorporate in WorkshopProjectProposalsCorporates)
                {
                    foreach (var WorkshopProjectProposal in WorkshopProjectProposalsCorporate.FrontendUser.WorkshopProjectProposals.Where(c => c.WorkshopProjectProposalStatu.NameEN == "Accepted").ToList())
                    {
                        model.Corporations.Add(new CorporationDto()
                        {
                            Corporation = WorkshopProjectProposalsCorporate.Name,
                            CorporationCategory = CultureHelper.CurrentCulture != 3 ? WorkshopProjectProposalsCorporate.CorporationsCategory.CorporationsCategoryNameEN : WorkshopProjectProposalsCorporate.CorporationsCategory.CorporationsCategoryNameAR,
                            Governate = CultureHelper.CurrentCulture != 3 ? WorkshopProjectProposalsCorporate.Governorate.GovernorateEN : WorkshopProjectProposalsCorporate.Governorate.GovernorateAR,
                            ProjectName = CultureHelper.CurrentCulture != 3 ? $"WorkShop - {WorkshopProjectProposal.IncubationWorkshop.Name}" : $"ورشه عمل - {WorkshopProjectProposal.IncubationWorkshop.Name}",
                            CorporationVolume = WorkshopProjectProposalsCorporate.FrontendUser.IncubationBaselines.Sum(r => r.BoardDirectorsCount ?? 0 + r.DepartmentFollowUpEvaluationEmpCount ?? 0 + r.FRDevSpecialistsCount ?? 0 + r.specializedPandPDepartmentEmpCount ?? 0 + r.DepartmentHREmpCount ?? 0),
                            Age = (WorkshopProjectProposalsCorporate?.FoundedYear.HasValue==true)? DateTime.Now.Year - WorkshopProjectProposalsCorporate.FoundedYear.Value.Year:0,
                            FieldOfWork = CultureHelper.CurrentCulture != 3 ? WorkshopProjectProposalsCorporate.CorporateFieldOfWork.CorporateFieldOfWorkNameEN : WorkshopProjectProposalsCorporate.CorporateFieldOfWork.CorporateFieldOfWorkNameAR,
                            Gender = GetGenderName(WorkshopProjectProposalsCorporate.corporateGenderType)
                        });
                    }
                }
            }
            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[7] { new DataColumn(App_GlobalResources.CorporationsReport.CorprationName),
                                            new DataColumn(App_GlobalResources.CorporationsReport.Category),
                                            new DataColumn(App_GlobalResources.General.Governorate),
                                            new DataColumn(App_GlobalResources.CorporationsReport.Project),
                                            new DataColumn(App_GlobalResources.CorporationsReport.CorporationSize),
                                            new DataColumn(App_GlobalResources.CorporationsReport.Age),
                                            new DataColumn(App_GlobalResources.CorporationsReport.FieldOfWork)
                                            });
            foreach (var Corporation in model.Corporations)
            {
                dt.Rows.Add(Corporation.Corporation, Corporation.CorporationCategory, Corporation.Governate, Corporation.ProjectName, Corporation.CorporationVolume, Corporation.Age, Corporation.FieldOfWork);
            }
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    FileContentResult fileContent = new FileContentResult(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                    fileContent.FileDownloadName = $"{Guid.NewGuid().ToString()}.xlsx";
                    model.FileContentResult = fileContent;
                    model.FileGuid = $"{Guid.NewGuid().ToString()}";
                    TempData[model.FileGuid] = fileContent;
                }
            }
            return View("CorporatesResult", model);
        }

        public ActionResult CorporatesResult(CorporationsResult Model)
        {
            ViewBag.Corporations = new SelectList(repository.GetAll<CorporateApplicationForm>().Where(c=>c.IsDraft!=true).ToList(), "CorporateApplicationFormID", "Name");
            ViewBag.CorporationsCategoryID = new SelectList(repository.GetAll<CorporationsCategory>().ToList(), "CorporationsCategoryID", CultureHelper.CurrentCulture != 3 ? "CorporationsCategoryNameEN" : "CorporationsCategoryNameAR");
            ViewBag.GovernorateID = new SelectList(repository.GetQuery<Governorate>(f => f.IsActive == true), "GovernorateID", CultureHelper.CurrentCulture != 3 ? "GovernorateEN" : "GovernorateAR");
            ViewBag.CorporateFieldOfWorkName = new SelectList(repository.GetQuery<CorporateFieldOfWork>(f => f.IsActive == true), "CorporateFieldOfWorkID", CultureHelper.CurrentCulture != 3 ? "CorporateFieldOfWorkNameEN" : "CorporateFieldOfWorkNameAR");
            return View(Model);
        }
        #endregion

        #region IncubationReports
        [HttpGet]
        public ActionResult Incubation()
        {
            ViewBag.Corporations = new SelectList(repository.GetAll<CorporateApplicationForm>().Where(c=>c.IsDraft!=true).ToList(), "CorporateApplicationFormID", "Name");
            ViewBag.CorporationsCategoryID = new SelectList(repository.GetAll<CorporationsCategory>().ToList(), "CorporationsCategoryID", CultureHelper.CurrentCulture != 3 ? "CorporationsCategoryNameEN" : "CorporationsCategoryNameAR");
            ViewBag.GovernorateID = new SelectList(repository.GetQuery<Governorate>(f => f.IsActive == true), "GovernorateID", CultureHelper.CurrentCulture != 3 ? "GovernorateEN" : "GovernorateAR");
            ViewBag.CorporateFieldOfWorkName = new SelectList(repository.GetQuery<CorporateFieldOfWork>(f => f.IsActive == true), "CorporateFieldOfWorkID", CultureHelper.CurrentCulture != 3 ? "CorporateFieldOfWorkNameEN" : "CorporateFieldOfWorkNameAR");
            ViewBag.Consultants = new SelectList(repository.GetQuery<Consultant>(f => f.IsActive == true), "ConsultantID", "Name");
            ViewBag.FundingSources = new SelectList(repository.GetQuery<FundingSource>(f => f.IsActive == true), "FundingSourceID", CultureHelper.CurrentCulture != 3 ? "FundingSourceNameEN" : "FundingSourceNameAR");
            IncubationResultDto incubationResultDto = new IncubationResultDto();
            return View(incubationResultDto);
        }

        [HttpPost]
        public ActionResult Incubation(IncubationResultDto model)
        {
            ViewBag.Corporations = new SelectList(repository.GetAll<CorporateApplicationForm>().Where(c=>c.IsDraft!=true).ToList(), "CorporateApplicationFormID", "Name");
            ViewBag.CorporationsCategoryID = new SelectList(repository.GetAll<CorporationsCategory>().ToList(), "CorporationsCategoryID", CultureHelper.CurrentCulture != 3 ? "CorporationsCategoryNameEN" : "CorporationsCategoryNameAR");
            ViewBag.GovernorateID = new SelectList(repository.GetQuery<Governorate>(f => f.IsActive == true), "GovernorateID", CultureHelper.CurrentCulture != 3 ? "GovernorateEN" : "GovernorateAR");
            ViewBag.CorporateFieldOfWorkName = new SelectList(repository.GetQuery<CorporateFieldOfWork>(f => f.IsActive == true), "CorporateFieldOfWorkID", CultureHelper.CurrentCulture != 3 ? "CorporateFieldOfWorkNameEN" : "CorporateFieldOfWorkNameAR");
            ViewBag.Consultants = new SelectList(repository.GetQuery<Consultant>(f => f.IsActive == true), "ConsultantID", "Name");
            ViewBag.FundingSources = new SelectList(repository.GetQuery<FundingSource>(f => f.IsActive == true), "FundingSourceID", CultureHelper.CurrentCulture != 3 ? "FundingSourceNameEN" : "FundingSourceNameAR");
            var incubations = repository.Get<Incubation>(i => i.IncubationType.NameEN == "Incubation");
            IncubationResultDto incubationResultDto = new IncubationResultDto();
            if ((model.SearchDto.DateFrom.HasValue && model.SearchDto.DateTo.HasValue) || model.SearchDto.CorporationsCategoryID.HasValue || model.SearchDto.Government.HasValue || model.SearchDto.CorporationName.HasValue
                            || model.SearchDto.CorporateFieldOfWorkID.HasValue)
            {
                if (model.SearchDto.DateFrom.HasValue)
                {
                    incubations = incubations.Where(c => c.StartDate >= model.SearchDto.DateFrom.Value).ToList();
                }
                if (model.SearchDto.DateTo.HasValue)
                {
                    incubations = incubations.Where(c => c.EndDate <= model.SearchDto.DateTo.Value).ToList();
                }
                if (model.SearchDto.CorporationsCategoryID.HasValue)
                {
                    incubations = incubations.Where(c => c.IncubationProjectProposal.FrontendUser.CorporateApplicationForms.Any(i => i.CorporationsCategoryID == model.SearchDto.CorporationsCategoryID.Value)).ToList();
                }
                if (model.SearchDto.CorporateFieldOfWorkID.HasValue)
                {
                    incubations = incubations
                        .Where(c => c.IncubationProjectProposal.FrontendUser.CorporateApplicationForms
                        .Any(i => i.CorporateFieldOfWorkID == model.SearchDto.CorporateFieldOfWorkID.Value)).ToList();
                }
                if (model.SearchDto.Government.HasValue)
                {
                    incubations = incubations.Where(c => c.IncubationProjectProposal.FrontendUser.CorporateApplicationForms
                                           .Any(i => i.GovernorateID == model.SearchDto.Government.Value)).ToList();
                }
                if (model.SearchDto.CorporationName.HasValue)
                {
                    incubations = incubations.Where(c => c.IncubationProjectProposal.FrontendUser.CorporateApplicationForms
                                           .Any(i => i.CorporateApplicationFormID == model.SearchDto.CorporationName.Value)).ToList();
                }
            }
            foreach (var incubation in incubations)
            {
                var corperation = incubation.IncubationProjectProposal.FrontendUser.CorporateApplicationForms.FirstOrDefault(c => c.FrontendUserID == incubation.IncubationProjectProposal.FrontendUserID);
                incubationResultDto.Incubations.Add(new IncubationDto()
                {
                    Consultant = incubation.Consultant.Name,
                    Corporation = incubation.IncubationProjectProposal.FrontendUser.CorporateApplicationForms.FirstOrDefault(c => c.FrontendUserID == incubation.IncubationProjectProposal.FrontendUserID).Name,
                    FoundedYear = corperation.FoundedYear.HasValue ? corperation.FoundedYear.Value.Year : 0,//26-2-2025

                    //FoundedYear = corperation != null ? corperation.FoundedYear.Year : 0,
                    Age = (corperation?.FoundedYear.HasValue == true)
                            ? DateTime.Now.Year - corperation.FoundedYear.Value.Year
                            : 0,

                    //Age = corperation != null ? DateTime.Now.Year - corperation.FoundedYear.Year : 0,
                    CorporationCategory = CultureHelper.CurrentCulture != 3 ?
                    corperation.CorporationsCategory.CorporationsCategoryNameEN : corperation.CorporationsCategory.CorporationsCategoryNameAR,
                    FieldOfWork = CultureHelper.CurrentCulture != 3 ?
                    corperation.CorporateFieldOfWork.CorporateFieldOfWorkNameEN : corperation.CorporateFieldOfWork.CorporateFieldOfWorkNameAR,
                    FundingSource = CultureHelper.CurrentCulture != 3 ? incubation.IncubationAdvertising.FundingSource.FundingSourceNameEN : incubation.IncubationAdvertising.FundingSource.FundingSourceNameAR,
                    Gender = GetGenderName(corperation.corporateGenderType),
                    Governate = CultureHelper.CurrentCulture != 3 ? corperation.Governorate.GovernorateEN : corperation.Governorate.GovernorateAR,
                    ModelName = CultureHelper.CurrentCulture != 3 ? incubation.IncubationModel.NameEN : incubation.IncubationModel.NameAR
                });
            }

            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[9] {
                        new DataColumn($"{App_GlobalResources.AccelerationReport.CorperationName}"),
                        new DataColumn($"{App_GlobalResources.AccelerationReport.FoundedYear}"),
                        new DataColumn($"{App_GlobalResources.AccelerationReport.Age}"),
                        new DataColumn($"{App_GlobalResources.AccelerationReport.Governate}"),
                        new DataColumn($"{App_GlobalResources.AccelerationReport.FundingSource}"),
                        new DataColumn($"{App_GlobalResources.AccelerationReport.Consultant}"),
                        new DataColumn($"{App_GlobalResources.AccelerationReport.CorporationCategory}"),
                        new DataColumn($"{App_GlobalResources.AccelerationReport.Gender}"),
                        new DataColumn($"{App_GlobalResources.AccelerationReport.FieldOfWork}")
                    });

            foreach (var Incubation in incubationResultDto.Incubations)
            {
                dt.Rows.Add(Incubation.Corporation, Incubation.FoundedYear, Incubation.Age, Incubation.Governate, Incubation.FundingSource, Incubation.Consultant, Incubation.CorporationCategory, Incubation.Gender, Incubation.FieldOfWork);
            }
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    FileContentResult fileContent = new FileContentResult(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                    fileContent.FileDownloadName = $"{Guid.NewGuid().ToString()}.xlsx";
                    incubationResultDto.FileResult = fileContent;
                    incubationResultDto.FileGuid = $"{Guid.NewGuid().ToString()}";
                    TempData[incubationResultDto.FileGuid] = fileContent;
                }
            }
            return View("IncubationResult", incubationResultDto);
        }

        public ActionResult IncubationResult(IncubationResultDto Model)
        {
            ViewBag.Corporations = new SelectList(repository.GetAll<CorporateApplicationForm>().Where(c=>c.IsDraft!=true).ToList(), "CorporateApplicationFormID", "Name");
            ViewBag.CorporationsCategoryID = new SelectList(repository.GetAll<CorporationsCategory>().ToList(), "CorporationsCategoryID", CultureHelper.CurrentCulture != 3 ? "CorporationsCategoryNameEN" : "CorporationsCategoryNameAR");
            ViewBag.GovernorateID = new SelectList(repository.GetQuery<Governorate>(f => f.IsActive == true), "GovernorateID", CultureHelper.CurrentCulture != 3 ? "GovernorateEN" : "GovernorateAR");
            ViewBag.CorporateFieldOfWorkName = new SelectList(repository.GetQuery<CorporateFieldOfWork>(f => f.IsActive == true), "CorporateFieldOfWorkID", CultureHelper.CurrentCulture != 3 ? "CorporateFieldOfWorkNameEN" : "CorporateFieldOfWorkNameAR");
            ViewBag.Consultants = new SelectList(repository.GetQuery<Consultant>(f => f.IsActive == true), "ConsultantID", "Name");
            ViewBag.FundingSources = new SelectList(repository.GetQuery<FundingSource>(f => f.IsActive == true), "FundingSourceID", CultureHelper.CurrentCulture != 3 ? "FundingSourceNameEN" : "FundingSourceNameAR");
            return View(Model);
        }
        #endregion

        #region AccelerationReport
        [HttpGet]
        public ActionResult Acceleration()
        {
            ViewBag.Corporations = new SelectList(repository.GetAll<CorporateApplicationForm>().Where(c=>c.IsDraft!=true).ToList(), "CorporateApplicationFormID", "Name");
            ViewBag.CorporationsCategoryID = new SelectList(repository.GetAll<CorporationsCategory>().ToList(), "CorporationsCategoryID", CultureHelper.CurrentCulture != 3 ? "CorporationsCategoryNameEN" : "CorporationsCategoryNameAR");
            ViewBag.GovernorateID = new SelectList(repository.GetQuery<Governorate>(f => f.IsActive == true), "GovernorateID", CultureHelper.CurrentCulture != 3 ? "GovernorateEN" : "GovernorateAR");
            ViewBag.CorporateFieldOfWorkName = new SelectList(repository.GetQuery<CorporateFieldOfWork>(f => f.IsActive == true), "CorporateFieldOfWorkID", CultureHelper.CurrentCulture != 3 ? "CorporateFieldOfWorkNameEN" : "CorporateFieldOfWorkNameAR");
            ViewBag.Consultants = new SelectList(repository.GetQuery<Consultant>(f => f.IsActive == true), "ConsultantID", "Name");
            ViewBag.FundingSources = new SelectList(repository.GetQuery<FundingSource>(f => f.IsActive == true), "FundingSourceID", CultureHelper.CurrentCulture != 3 ? "FundingSourceNameEN" : "FundingSourceNameAR");
            IncubationResultDto incubationResultDto = new IncubationResultDto();
            return View(incubationResultDto);
        }

        [HttpPost]
        public ActionResult Acceleration(IncubationResultDto model)
        {
            ViewBag.Corporations = new SelectList(repository.GetAll<CorporateApplicationForm>().Where(c=>c.IsDraft!=true).ToList(), "CorporateApplicationFormID", "Name");
            ViewBag.CorporationsCategoryID = new SelectList(repository.GetAll<CorporationsCategory>().ToList(), "CorporationsCategoryID", CultureHelper.CurrentCulture != 3 ? "CorporationsCategoryNameEN" : "CorporationsCategoryNameAR");
            ViewBag.GovernorateID = new SelectList(repository.GetQuery<Governorate>(f => f.IsActive == true), "GovernorateID", CultureHelper.CurrentCulture != 3 ? "GovernorateEN" : "GovernorateAR");
            ViewBag.CorporateFieldOfWorkName = new SelectList(repository.GetQuery<CorporateFieldOfWork>(f => f.IsActive == true), "CorporateFieldOfWorkID", CultureHelper.CurrentCulture != 3 ? "CorporateFieldOfWorkNameEN" : "CorporateFieldOfWorkNameAR");
            ViewBag.Consultants = new SelectList(repository.GetQuery<Consultant>(f => f.IsActive == true), "ConsultantID", "Name");
            ViewBag.FundingSources = new SelectList(repository.GetQuery<FundingSource>(f => f.IsActive == true), "FundingSourceID", CultureHelper.CurrentCulture != 3 ? "FundingSourceNameEN" : "FundingSourceNameAR");
            var incubations = repository.Get<Incubation>(i => i.IncubationType.NameEN == "Acceleration");
            IncubationResultDto incubationResultDto = new IncubationResultDto();
            if ((model.SearchDto.DateFrom.HasValue && model.SearchDto.DateTo.HasValue) || model.SearchDto.CorporationsCategoryID.HasValue ||
                model.SearchDto.Government.HasValue || model.SearchDto.CorporationName.HasValue || model.SearchDto.CorporateFieldOfWorkID.HasValue)
            {
                if (model.SearchDto.DateFrom.HasValue)
                {
                    incubations = incubations.Where(c => c.StartDate >= model.SearchDto.DateFrom.Value).ToList();
                }
                if (model.SearchDto.DateTo.HasValue)
                {
                    incubations = incubations.Where(c => c.EndDate <= model.SearchDto.DateTo.Value).ToList();
                }
                if (model.SearchDto.CorporationsCategoryID.HasValue)
                {
                    incubations = incubations.Where(c => c.IncubationProjectProposal.FrontendUser.CorporateApplicationForms.Any(i => i.CorporationsCategoryID == model.SearchDto.CorporationsCategoryID.Value)).ToList();
                }
                if (model.SearchDto.CorporateFieldOfWorkID.HasValue)
                {
                    incubations = incubations
                        .Where(c => c.IncubationProjectProposal.FrontendUser.CorporateApplicationForms
                        .Any(i => i.CorporateFieldOfWorkID == model.SearchDto.CorporateFieldOfWorkID.Value)).ToList();
                }
                if (model.SearchDto.Government.HasValue)
                {
                    incubations = incubations.Where(c => c.IncubationProjectProposal.FrontendUser.CorporateApplicationForms
                                           .Any(i => i.GovernorateID == model.SearchDto.Government.Value)).ToList();
                }
                if (model.SearchDto.CorporationName.HasValue)
                {
                    incubations = incubations.Where(c => c.IncubationProjectProposal.FrontendUser.CorporateApplicationForms
                                           .Any(i => i.CorporateApplicationFormID == model.SearchDto.CorporationName.Value)).ToList();
                }
            }
            foreach (var incubation in incubations)
            {
                var corperation = incubation.IncubationProjectProposal.FrontendUser.CorporateApplicationForms.FirstOrDefault(c => c.FrontendUserID == incubation.IncubationProjectProposal.FrontendUserID);
                incubationResultDto.Incubations.Add(new IncubationDto()
                {
                    Consultant = incubation.Consultant.Name,
                    Corporation = incubation.IncubationProjectProposal.FrontendUser.CorporateApplicationForms.FirstOrDefault(c => c.FrontendUserID == incubation.IncubationProjectProposal.FrontendUserID).Name,
                    CorporationCategory = CultureHelper.CurrentCulture != 3 ?
                    corperation.CorporationsCategory.CorporationsCategoryNameEN : corperation.CorporationsCategory.CorporationsCategoryNameAR,
                    FieldOfWork = CultureHelper.CurrentCulture != 3 ?
                    corperation.CorporateFieldOfWork.CorporateFieldOfWorkNameEN : corperation.CorporateFieldOfWork.CorporateFieldOfWorkNameAR,
                    FundingSource = CultureHelper.CurrentCulture != 3 ? incubation.IncubationAdvertising.FundingSource.FundingSourceNameEN : incubation.IncubationAdvertising.FundingSource.FundingSourceNameAR,
                    Gender = GetGenderName(corperation.corporateGenderType),
                    Governate = CultureHelper.CurrentCulture != 3 ? corperation.Governorate.GovernorateEN : corperation.Governorate.GovernorateAR,
                    ModelName = CultureHelper.CurrentCulture != 3 ? incubation.IncubationModel.NameEN : incubation.IncubationModel.NameAR
                });
            }
            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[7] {
                        new DataColumn($"{App_GlobalResources.AccelerationReport.ModelName}"),
                        new DataColumn($"{App_GlobalResources.AccelerationReport.CorperationName}"),
                        new DataColumn($"{App_GlobalResources.AccelerationReport.CorporationCategory}"),
                        new DataColumn($"{App_GlobalResources.AccelerationReport.Governate}"),
                        new DataColumn($"{App_GlobalResources.AccelerationReport.FundingSource}"),
                        new DataColumn($"{App_GlobalResources.AccelerationReport.Gender}"),
                        new DataColumn($"{App_GlobalResources.AccelerationReport.FieldOfWork}")
                    });

            foreach (var Incubation in incubationResultDto.Incubations)
            {
                dt.Rows.Add(Incubation.ModelName, Incubation.Corporation, Incubation.CorporationCategory, Incubation.Governate, Incubation.FundingSource, Incubation.Gender, Incubation.FieldOfWork);
            }
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    FileContentResult fileContent = new FileContentResult(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                    fileContent.FileDownloadName = $"{Guid.NewGuid().ToString()}.xlsx";
                    incubationResultDto.FileResult = fileContent;
                    incubationResultDto.FileGuid = $"{Guid.NewGuid().ToString()}";
                    TempData[incubationResultDto.FileGuid] = fileContent;
                }
            }
            return View("AccelerationResult", incubationResultDto);
        }

        public ActionResult AccelerationResult(IncubationResultDto Model)
        {
            ViewBag.Corporations = new SelectList(repository.GetAll<CorporateApplicationForm>().Where(c=>c.IsDraft!=true).ToList(), "CorporateApplicationFormID", "Name");
            ViewBag.CorporationsCategoryID = new SelectList(repository.GetAll<CorporationsCategory>().ToList(), "CorporationsCategoryID", CultureHelper.CurrentCulture != 3 ? "CorporationsCategoryNameEN" : "CorporationsCategoryNameAR");
            ViewBag.GovernorateID = new SelectList(repository.GetQuery<Governorate>(f => f.IsActive == true), "GovernorateID", CultureHelper.CurrentCulture != 3 ? "GovernorateEN" : "GovernorateAR");
            ViewBag.CorporateFieldOfWorkName = new SelectList(repository.GetQuery<CorporateFieldOfWork>(f => f.IsActive == true), "CorporateFieldOfWorkID", CultureHelper.CurrentCulture != 3 ? "CorporateFieldOfWorkNameEN" : "CorporateFieldOfWorkNameAR");
            ViewBag.Consultants = new SelectList(repository.GetQuery<Consultant>(f => f.IsActive == true), "ConsultantID", "Name");
            ViewBag.FundingSources = new SelectList(repository.GetQuery<FundingSource>(f => f.IsActive == true), "FundingSourceID", CultureHelper.CurrentCulture != 3 ? "FundingSourceNameEN" : "FundingSourceNameAR");
            return View(Model);
        }
        #endregion

        #region WorkshopReport
        [HttpGet]
        public ActionResult WorkShops()
        {
            ViewBag.IncubationWorkShopModels = new SelectList(repository.GetAll<IncubationWorkshopModel>(), "IncubationWorkshopModeID", CultureHelper.CurrentCulture != 3 ? "NameEN" : "NameAR");
            ViewBag.Governates = new SelectList(repository.GetAll<Governorate>(), "GovernorateID", CultureHelper.CurrentCulture != 3 ? "GovernorateAR" : "GovernorateEN");
            ViewBag.IncubationWorkShops = new SelectList(repository.GetAll<IncubationWorkshop>().Where(w => !w.IsDeleted), "IncubationWorkshopID", "Name");
            ViewBag.Consultants = new SelectList(repository.GetAll<Consultant>(), "ConsultantID", "Name");
            ViewBag.Corporations = new SelectList(repository.GetAll<CorporateApplicationForm>(), "CorporateApplicationFormID", "Name");
            WorkshopResultDto model = new WorkshopResultDto();
            return View(model);
        }

        [HttpPost]
        public ActionResult WorkShops(WorkshopResultDto model)
        {
            ViewBag.IncubationWorkShopModels = new SelectList(repository.GetAll<IncubationWorkshopModel>(), "IncubationWorkshopModeID", CultureHelper.CurrentCulture != 3 ? "NameEN" : "NameAR");
            ViewBag.Governates = new SelectList(repository.GetAll<Governorate>(), "GovernorateID", CultureHelper.CurrentCulture != 3 ? "GovernorateAR" : "GovernorateEN");
            ViewBag.IncubationWorkShops = new SelectList(repository.GetAll<IncubationWorkshop>().Where(w => !w.IsDeleted), "IncubationWorkshopID", "Name");
            ViewBag.Consultants = new SelectList(repository.GetAll<Consultant>(), "ConsultantID", "Name");
            ViewBag.Corporations = new SelectList(repository.GetAll<CorporateApplicationForm>(), "CorporateApplicationFormID", "Name");
            WorkshopResultDto WorkshopResultDto = new WorkshopResultDto();
            var IncubationWorkshops = repository.GetQuery<IncubationWorkshop>(p => p.WorkshopProjectProposals.Where(w => w.WorkshopProjectProposalStatu.NameEN == "Accepted").Count() > 0).ToList();

            if ((model.SearchDto.DateFrom.HasValue && model.SearchDto.DateTo.HasValue) || model.SearchDto.CorporateId.HasValue || model.SearchDto.IncubationWorkshopModelId.HasValue || model.SearchDto.IncubationWorkshopId.HasValue || model.SearchDto.ProjectSuccessRate.HasValue || model.SearchDto.ConsultantId.HasValue || model.SearchDto.Gender.HasValue || model.SearchDto.GovernateId.HasValue)
            {
                if (model.SearchDto.DateFrom.HasValue)
                {
                    IncubationWorkshops = IncubationWorkshops.Where(c => c.StartDate >= model.SearchDto.DateFrom.Value && c.EndDate <= model.SearchDto.DateTo.Value).ToList();
                }
                if (model.SearchDto.CorporateId.HasValue)
                {
                    var Corporation = repository.GetByKey<CorporateApplicationForm>(model.SearchDto.CorporateId.Value);
                    IncubationWorkshops = IncubationWorkshops.Where(i => i.WorkshopProjectProposals.Where(l => l.FrontendUserID == Corporation.FrontendUserID).Count() > 0).ToList();
                }
                if (model.SearchDto.IncubationWorkshopModelId.HasValue)
                {
                    IncubationWorkshops = IncubationWorkshops.Where(g => g.IncubationWorkshopModel.IncubationWorkshopModeID == model.SearchDto.IncubationWorkshopModelId.Value).ToList();
                }
                if (model.SearchDto.IncubationWorkshopId.HasValue)
                {
                    IncubationWorkshops = IncubationWorkshops.Where(g => g.IncubationWorkshopID == model.SearchDto.IncubationWorkshopId.Value).ToList();
                }

                if (model.SearchDto.GovernateId.HasValue)
                {
                    IncubationWorkshops = IncubationWorkshops.Where(g => g.GovernorateID == model.SearchDto.GovernateId.Value).ToList();
                }

                if (model.SearchDto.ConsultantId.HasValue)
                {
                    IncubationWorkshops = IncubationWorkshops.Where(g => g.ConsultantID == model.SearchDto.ConsultantId.Value).ToList();
                }
            }


            foreach (var IncubationWorkshop in IncubationWorkshops)
            {
                foreach (var IncubationWorkshopProjectProposal in IncubationWorkshop.WorkshopProjectProposals.Where(i => i.WorkshopProjectProposalStatu.NameEN == "Accepted" && i.WorkshopPP_InvitationStatus == WorkshopPPInvitationStatus.attend))
                {
                    var corp = repository.Get<CorporateApplicationForm>(c => c.FrontendUserID == IncubationWorkshopProjectProposal.FrontendUserID).FirstOrDefault();
                    foreach (var Employee in IncubationWorkshopProjectProposal.EmployeesAttendIncubationWorkShops.Where(i => i.WorkshopProjectProposal.IncubationWorkshop.WorkshopPrivateInvitations.Any(p => p.FrontendUserID == corp.FrontendUserID && p.InvitationStatus == InvitationStatus.attend)))
                    {
                        WorkshopResultDto.Results.Add(new WorkShopDto()
                        {
                            Attender = Employee.Name,
                            CorporationName = corp.Name,
                            ConsultantName = IncubationWorkshop.Consultant.Name,
                            CountOfAdventages = IncubationWorkshop.WorkshopPrivateInvitations.Count,
                            CountOfConfirmed = IncubationWorkshop.WorkshopProjectProposals.Count(c => c.WorkshopProjectProposalStatu.NameEN == "Accepted" && c.WorkshopPP_InvitationStatus == WorkshopPPInvitationStatus.attend),
                            GovernorateName = CultureHelper.CurrentCulture != 3 ? IncubationWorkshop.Governorate.GovernorateEN : IncubationWorkshop.Governorate.GovernorateAR,
                            Position = Employee.Position,
                            CountOfPartcipation = IncubationWorkshop.WorkshopPrivateInvitations.Where(c => c.InvitationStatus == InvitationStatus.attend).Count(),//WorkshopPrivateInvitation.InvitationStatus == Attend
                            Qualification = Employee.EducationalQualificationAndSpecialization,
                            WorkshopName = IncubationWorkshop.Name,
                            WorkshopModelName = CultureHelper.CurrentCulture != 3 ? IncubationWorkshop.IncubationWorkshopModel.NameEN : IncubationWorkshop.IncubationWorkshopModel.NameAR,
                            Gender = GetGenderPersonName(Employee.Gender)
                        });
                    }
                }
            }

            if (WorkshopResultDto.Results?.Count > 0)
            {
                DataTable dt = new DataTable("Grid");
                dt.Columns.AddRange(new DataColumn[12] {
                        new DataColumn(App_GlobalResources.WorkshopReport.Workshop),
                        new DataColumn(App_GlobalResources.WorkshopReport.WorkshopModel),
                        new DataColumn(App_GlobalResources.WorkshopReport.Corporate),
                        new DataColumn(App_GlobalResources.WorkshopReport.Attender),
                        new DataColumn(App_GlobalResources.WorkshopReport.Governorate),
                        new DataColumn(App_GlobalResources.WorkshopReport.TargetCount),
                        new DataColumn(App_GlobalResources.WorkshopReport.CountOfConfirmations),
                        new DataColumn(App_GlobalResources.WorkshopReport.CountOfPartcipation),
                        new DataColumn(App_GlobalResources.WorkshopReport.Position),
                        new DataColumn(App_GlobalResources.WorkshopReport.Gender),
                        new DataColumn(App_GlobalResources.WorkshopReport.EducationalQualificationAndSpecialization),
                        new DataColumn(App_GlobalResources.WorkshopReport.Consultant)
                    });

                foreach (var Result in WorkshopResultDto.Results)
                {
                    dt.Rows.Add(Result.WorkshopName, Result.WorkshopModelName, Result.CorporationName, Result.Attender, Result.GovernorateName, Result.CountOfAdventages, Result.CountOfConfirmed, Result.CountOfPartcipation, Result.Position ,Result.Gender,Result.Qualification, Result.ConsultantName);
                }
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dt);

                    using (MemoryStream stream = new MemoryStream())
                    {
                        wb.SaveAs(stream);
                        FileContentResult fileContent = new FileContentResult(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                        fileContent.FileDownloadName = $"{Guid.NewGuid().ToString()}.xlsx";
                        WorkshopResultDto.FileContentResult = fileContent;
                        WorkshopResultDto.FileGuid = $"{Guid.NewGuid().ToString()}";
                        TempData[WorkshopResultDto.FileGuid] = fileContent;
                    }
                }
            }
            return View("WorkShopsResult", WorkshopResultDto);
        }

        public ActionResult WorkShopsResult(WorkshopResultDto model)
        {
            ViewBag.IncubationWorkShopModels = new SelectList(repository.GetAll<IncubationWorkshopModel>(), "IncubationWorkshopModeID", CultureHelper.CurrentCulture != 3 ? "NameEN" : "NameAR");
            ViewBag.Governates = new SelectList(repository.GetAll<Governorate>(), "GovernorateID", CultureHelper.CurrentCulture != 3 ? "GovernorateAR" : "GovernorateEN");
            ViewBag.IncubationWorkShops = new SelectList(repository.GetAll<IncubationWorkshop>().Where(w => !w.IsDeleted), "IncubationWorkshopID", "Name");
            ViewBag.Consultants = new SelectList(repository.GetAll<Consultant>(), "ConsultantID", "Name");
            ViewBag.Corporations = new SelectList(repository.GetAll<CorporateApplicationForm>(), "CorporateApplicationFormID", "Name");
            return View(model);
        }
        #endregion

        #endregion

        #region DownloadAction
        [HttpGet]
        public virtual ActionResult Download(string fileGuid)
        {
            if (TempData[fileGuid] != null)
            {
                FileContentResult data = TempData[fileGuid] as FileContentResult;
                return data;
            }
            else
            {
                return new EmptyResult();
            }
        }
        #endregion

        #endregion

        #region Helpers

        private string GetGenderName(CorporateGenderType? type)
        {
            if (CultureHelper.CurrentCulture == 3)
            {
                if (type == CorporateGenderType.Joint)
                {
                    return "رجال و نساء";
                }
                if (type == CorporateGenderType.men)
                {
                    return "رجال";
                }
                if (type == CorporateGenderType.women)
                {
                    return "نساء";
                }
            }
            else
            {
                if (type == CorporateGenderType.Joint)
                {
                    return "Men & Woman";
                }
                if (type == CorporateGenderType.men)
                {
                    return "Men";
                }
                if (type == CorporateGenderType.women)
                {
                    return "Woman";
                }
            }
            return "";
        }

        private string GetFieldOfEmploymentName(FieldOfEmployment? type)
        {
            if (CultureHelper.CurrentCulture == 3)
            {
                if (type == FieldOfEmployment.Administrative)
                {
                    return "المجال الإداري";
                }
                if (type == FieldOfEmployment.Literary)
                {
                    return "المجال الأدبي";
                }
                if (type == FieldOfEmployment.Professional)
                {
                    return "المجال المهني";
                }
            }
            else
            {
                if (type == FieldOfEmployment.Administrative)
                {
                    return "Administrative Field";
                }
                if (type == FieldOfEmployment.Literary)
                {
                    return "Literary Field";
                }
                if (type == FieldOfEmployment.Professional)
                {
                    return "Professional Field";
                }
            }
            return "";
        }

        private string GetCorporationPerformanceName(CorporationPerformance? type)
        {
            if (CultureHelper.CurrentCulture == 3)
            {
                if (type == CorporationPerformance.Excellent)
                    return "ممتاز";
                if (type == CorporationPerformance.Good)
                    return "جيد";
                if (type == CorporationPerformance.Bad)
                    return "سئ";
            }
            else
            {
                if (type == CorporationPerformance.Excellent)
                    return "Excellent";
                if (type == CorporationPerformance.Good)
                    return "Good";
                if (type == CorporationPerformance.Bad)
                    return "Bad";
            }
            return "";
        }

        public string GetEvaluationGrantsName(EvaluationGrants? type)
        {
            if (CultureHelper.CurrentCulture == 3)
            {
                if (type == EvaluationGrants.Successful)
                    return "ناجح";
                if (type == EvaluationGrants.DidNotExpected)
                    return "لم تحقق النتائج المتوقعة";
                if (type == EvaluationGrants.Average)
                    return "متوسط";
            }
            else
            {
                if (type == EvaluationGrants.Successful)
                    return "Successful";
                if (type == EvaluationGrants.DidNotExpected)
                    return "Didn't Achieve The Expected Results";
                if (type == EvaluationGrants.Average)
                    return "Average";
            }
            return "";
        }

        public string GetGenderPersonName(GenderPerson? type)
        {
            if (CultureHelper.CurrentCulture != 3)
            {
                if (type == GenderPerson.Male)
                    return "Male";
                if (type == GenderPerson.Female)
                    return "Female";
            }
            else
            {
                if (type == GenderPerson.Male)
                    return "ذكر";
                if (type == GenderPerson.Female)
                    return "أنثي";
            }
            return "";
        }

        #endregion
    }
}