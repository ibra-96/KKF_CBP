using System;
using System.Linq;
using System.Web.Mvc;
using AlphaPeople.Core;
using System.Configuration;
using AlphaPeople.Repository;
using AlfaPeople.KingKhalidFoundation.Data;
using AlfaPeople.KingKhalidFoundation.Data.Model;
using AlfaPeople.KingKhaledFoundation.Admin.Web.Helper;
using AlfaPeople.KingKhaledFoundation.Admin.Web.Models;

using ClosedXML.Excel;
using System.IO;

namespace AlfaPeople.KingKhaledFoundation.Admin.Web.Controllers
{
    [Authorize(Roles = "Admin, CB Manager, CB Supervisor, CB Analyst")]
    public class IncubationBaselineRequestController : BaseController
    {
        private readonly IRepository repository;
        private readonly CommonHelper helper;
        private readonly string Logpath = ConfigurationManager.AppSettings["Log"].ToString();

        public IncubationBaselineRequestController()
        {
            helper = new CommonHelper();
            repository = new Repository(new KingkhaledFoundationDB());
        }
        public ActionResult BaselineReport()
        {
            ViewBag.lang = CultureHelper.CurrentCulture;
            var result = repository.GetQuery<IncubationBaseline>(f => f.IncubationBaselineStatus.NameEN != "Draft").ToList();
            return View(result);
        }

        public ActionResult Index()
        {
            ViewBag.lang = CultureHelper.CurrentCulture;
            var result = repository.GetQuery<IncubationBaseline>(f => f.IncubationBaselineStatus.NameEN != "Draft").ToList();
            return View(result);
        }
        //7-4-2025


        public ActionResult ExportBaselineSurveyToExcel()
        {
            var lang = CultureHelper.CurrentCulture;
            bool isArabic = lang == 3;

            var data = repository.GetQuery<IncubationBaseline>(x => x.IncubationBaselineStatus.NameEN != "Draft")
                .ToList()
                .Select(x => new
                {
                    Organization = x.FrontendUser?.CorporateApplicationForms.FirstOrDefault()?.Name,
                    Region = isArabic ? x.FrontendUser?.CorporateApplicationForms.FirstOrDefault()?.Region?.RegionNameAR : x.FrontendUser?.CorporateApplicationForms.FirstOrDefault()?.Region?.RegionNameEN,
                    Governorate = isArabic ? x.FrontendUser?.CorporateApplicationForms.FirstOrDefault()?.Governorate?.GovernorateAR : x.FrontendUser?.CorporateApplicationForms.FirstOrDefault()?.Governorate?.GovernorateEN,
                    City = isArabic ? x.FrontendUser?.CorporateApplicationForms.FirstOrDefault()?.City?.CityNameAR : x.FrontendUser?.CorporateApplicationForms.FirstOrDefault()?.City?.CityNameEN,
                    Status = isArabic ? x.IncubationBaselineStatus?.NameAR : x.IncubationBaselineStatus?.NameEN,
                    SubmissionDate = x.submissionDate?.ToString("yyyy-MM-dd"),
                    Feedback = x.Feadback,

                    ReasonEstablishing = x.ReasonEstablishingCorporate,
                    History = x.History,
                    Vision = x.Vision,
                    Mission = x.Mission,
                    Objectives = x.Objectives,
                    HowPlanMade = x.HowPlanMade,
                    StrategicPlan = x.StrategicPlan,
                    OperatingPlan = x.OperatingPlan,
                    StrategicObjective = x.StrategicObjective,

                    BoardCount = x.BoardDirectorsCount,
                    BoardApprovalDate = x.DateBoardApproval?.ToString("yyyy-MM-dd"),
                    GovernanceGuide = x.GovernanceGuide,
                    InternalRegulation = x.InternalRegulation ? (isArabic ? "نعم" : "Yes") : (isArabic ? "لا" : "No"),
                    BoardMeetings = x.BoardMeetingNumber,
                    BoardRoles = x.BoardDirectorRoles,

                    FRDept = x.FRDevSpecialistsCount.HasValue && x.FRDevSpecialistsCount > 0 ? (isArabic ? "نعم" : "Yes") : (isArabic ? "لا" : "No"),
                    FRDeptCount = x.FRDevSpecialistsCount,
                    Revenues = x.CorporateRevenuesLastYear,
                    SupportersCount = x.SupportersCountLastYear,
                    DonorSources = string.Join("، ",
                        new[] {
                    x.DonorInstitutions ? (isArabic ? "مؤسسات مانحة" : "Donor Institutions") : null,
                    x.GovernmentAgencies ? (isArabic ? "جهات حكومية" : "Government Agencies") : null,
                    x.PrivateSector ? (isArabic ? "القطاع الخاص" : "Private Sector") : null,
                    x.Individuals ? (isArabic ? "أفراد" : "Individuals") : null,
                    x.Investments ? (isArabic ? "استثمارات" : "Investments") : null,
                    x.Alms ? (isArabic ? "زكاة" : "Alms") : null
                        }.Where(v => v != null)),
                    DonorDetails = x.IndicateNumberAndSupportAmount,
                    CurrentBudget = x.CurrentCorporateBudget,
                    AccountantNotesCount = x.CharteredAccountantNotesCount,

                    PnPDept = x.specializedPandPDepartmentEmpCount.HasValue && x.specializedPandPDepartmentEmpCount > 0 ? (isArabic ? "نعم" : "Yes") : (isArabic ? "لا" : "No"),
                    PnPDeptCount = x.specializedPandPDepartmentEmpCount,
                    PlansBasedOnStrategy = x.PlanAccordingStrategicObjectives ? (isArabic ? "نعم" : "Yes") : (isArabic ? "لا" : "No"),
                    ImplementationDetails = x.PlanImplementationActivitiesParticipationClarify,
                    DevelopmentProjects = x.IndicateDevelopmentProjectsWorkingOrganization,
                    ProjectsBudget = x.BudgetAllocatedProjects,
                    OperationalBudget = x.BudgetAllocatedOperation,

                    EvalDept = x.DepartmentFollowUpEvaluationEmpCount.HasValue && x.DepartmentFollowUpEvaluationEmpCount > 0 ? (isArabic ? "نعم" : "Yes") : (isArabic ? "لا" : "No"),
                    EvalDeptCount = x.DepartmentFollowUpEvaluationEmpCount,
                    EvalPlan = x.OrganizationFollowUpEvaluationPlan ? (isArabic ? "نعم" : "Yes") : (isArabic ? "لا" : "No"),
                    EvalReports = x.OrganizationFollowUpEvaluationReports ? (isArabic ? "نعم" : "Yes") : (isArabic ? "لا" : "No"),
                    EvalForms = x.AttachFollowUpandEvaluationForms,

                    CommDept = x.DepartmentCorporateCommunicationPublicRelationsEmpCount.HasValue && x.DepartmentCorporateCommunicationPublicRelationsEmpCount > 0 ? (isArabic ? "نعم" : "Yes") : (isArabic ? "لا" : "No"),
                    CommDeptCount = x.DepartmentCorporateCommunicationPublicRelationsEmpCount,
                    CommPlan = x.CorporateCommunicationPlan ? (isArabic ? "نعم" : "Yes") : (isArabic ? "لا" : "No"),
                    SeparateBudget = x.SeparateItemGeneralBudget ? (isArabic ? "نعم" : "Yes") : (isArabic ? "لا" : "No"),
                    CommBudget = x.BudgetAllocatedCorporateCommunication,

                    HRDept = x.DepartmentHREmpCount.HasValue && x.DepartmentHREmpCount > 0 ? (isArabic ? "نعم" : "Yes") : (isArabic ? "لا" : "No"),
                    HRDeptCount = x.DepartmentHREmpCount,
                    FullTime = x.FullTimeStaff,
                    PartTime = x.PartTimeStaff,
                    Volunteers = x.VolunteerStaff,
                    Attraction = x.AttractEmployees,
                    PerformanceMonitoring = x.StaffPerformanceMonitored,
                    OrgStructure = x.OrganizationalStructureUnderHRManagement,
                    TrainingNeeds = x.TrainingNeedsCorporateStaffIdentified
                }).ToList();

            using (var workbook = new ClosedXML.Excel.XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Baseline Survey Report");
                worksheet.Cell(1, 1).InsertTable(data);

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Seek(0, SeekOrigin.Begin);
                    var fileName = $"BaselineSurveyReport_{DateTime.Now:yyyyMMdd_HHmm}.xlsx";
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }


 public ActionResult BaselineSurveyReport()
    {
    ViewBag.lang = CultureHelper.CurrentCulture;

    var result = repository.GetQuery<IncubationBaseline>(x => x.IncubationBaselineStatus.NameEN != "Draft")
        .Select(x => new BaselineSurveyReportVM
        {
            CorporationName = x.FrontendUser.CorporateApplicationForms.FirstOrDefault().Name,
            Region = CultureHelper.CurrentCulture == 3 ? x.FrontendUser.CorporateApplicationForms.FirstOrDefault().Region.RegionNameAR: x.FrontendUser.CorporateApplicationForms.FirstOrDefault().Region.RegionNameEN,
            Governorate = CultureHelper.CurrentCulture == 3 ? x.FrontendUser.CorporateApplicationForms.FirstOrDefault().Governorate.GovernorateAR: x.FrontendUser.CorporateApplicationForms.FirstOrDefault().Governorate.GovernorateEN,
            City = CultureHelper.CurrentCulture == 3 ? x.FrontendUser.CorporateApplicationForms.FirstOrDefault().City.CityNameAR: x.FrontendUser.CorporateApplicationForms.FirstOrDefault().City.CityNameEN,


            ReasonEstablishingCorporate = x.ReasonEstablishingCorporate,
            History = x.History,
            Vision = x.Vision,
            Mission = x.Mission,
            Objectives = x.Objectives,
            HowPlanMade = x.HowPlanMade,
            StrategicPlan = x.StrategicPlan,
            OperatingPlan = x.OperatingPlan,
            StrategicObjective = x.StrategicObjective,

            BoardDirectorsCount = x.BoardDirectorsCount,
            DateBoardApproval = x.DateBoardApproval,
            GovernanceGuide = x.GovernanceGuide,
            InternalRegulation = x.InternalRegulation,
            BoardMeetingNumber = x.BoardMeetingNumber,
            BoardDirectorRoles = x.BoardDirectorRoles,

            FRDevSpecialistsCount = x.FRDevSpecialistsCount,
            CorporateRevenuesLastYear = x.CorporateRevenuesLastYear,
            SupportersCountLastYear = x.SupportersCountLastYear,
            DonorInstitutions = x.DonorInstitutions,
            GovernmentAgencies = x.GovernmentAgencies,
            PrivateSector = x.PrivateSector,
            Individuals = x.Individuals,
            Investments = x.Investments,
            Alms = x.Alms,
            IndicateNumberAndSupportAmount = x.IndicateNumberAndSupportAmount,
            CurrentCorporateBudget = x.CurrentCorporateBudget,
            CharteredAccountantNotesCount = x.CharteredAccountantNotesCount,
            CharteredAccountantNotes = x.CharteredAccountantNotes,

            specializedPandPDepartmentEmpCount = x.specializedPandPDepartmentEmpCount,
            PlanAccordingStrategicObjectives = x.PlanAccordingStrategicObjectives,
            PlanImplementationActivitiesParticipationClarify = x.PlanImplementationActivitiesParticipationClarify,
            IndicateDevelopmentProjectsWorkingOrganization = x.IndicateDevelopmentProjectsWorkingOrganization,
            BudgetAllocatedProjects = x.BudgetAllocatedProjects,
            BudgetAllocatedOperation = x.BudgetAllocatedOperation,

            DepartmentFollowUpEvaluationEmpCount = x.DepartmentFollowUpEvaluationEmpCount,
            OrganizationFollowUpEvaluationPlan = x.OrganizationFollowUpEvaluationPlan,
            OrganizationFollowUpEvaluationReports = x.OrganizationFollowUpEvaluationReports,
            AttachFollowUpandEvaluationForms = x.AttachFollowUpandEvaluationForms,

            DepartmentCorporateCommunicationPublicRelationsEmpCount = x.DepartmentCorporateCommunicationPublicRelationsEmpCount,
            CorporateCommunicationPlan = x.CorporateCommunicationPlan,
            SeparateItemGeneralBudget = x.SeparateItemGeneralBudget,
            BudgetAllocatedCorporateCommunication = x.BudgetAllocatedCorporateCommunication,

            DepartmentHREmpCount = x.DepartmentHREmpCount,
            FullTimeStaff = x.FullTimeStaff,
            PartTimeStaff = x.PartTimeStaff,
            VolunteerStaff = x.VolunteerStaff,
            AttractEmployees = x.AttractEmployees,
            StaffPerformanceMonitored = x.StaffPerformanceMonitored,
            OrganizationalStructureUnderHRManagement = x.OrganizationalStructureUnderHRManagement,
            TrainingNeedsCorporateStaffIdentified = x.TrainingNeedsCorporateStaffIdentified,

            Feadback = x.Feadback,
            SubmissionDate = x.submissionDate,
            StatusName = CultureHelper.CurrentCulture == 3 ? x.IncubationBaselineStatus.NameAR : x.IncubationBaselineStatus.NameEN
        })
        .ToList();

    return View(result);
}

        public ActionResult Details(Guid Id)
        {
            var IncIL = repository.GetByKey<IncubationBaseline>(Id);
            if (IncIL != null)
            {
                ViewBag.dateBoardApproval = IncIL.DateBoardApproval?.ToString("yyyy-MM-dd");
                if (CultureHelper.CurrentCulture == 3)
                {
                    if (IncIL.IncubationBaselineStatus.NameEN == "Pending" || IncIL.IncubationBaselineStatus.NameEN == "Baseline Application Form Updated")
                    {
                        ViewBag.LstDrop = new SelectList(repository.GetQuery<IncubationBaselineStatus>(f => f.NameEN == "Update Baseline Application Form" || f.NameEN == "Rejected" || f.NameEN == "Accepted").ToList(), "IncubationBaselineStatusID", "NameAR");
                    }
                    else
                    {
                        ViewBag.LstDrop = new SelectList(repository.GetQuery<IncubationBaselineStatus>(f => f.NameEN == "Update Baseline Application Form").ToList(), "IncubationBaselineStatusID", "NameAR");
                    }
                }
                else
                {
                    if (IncIL.IncubationBaselineStatus.NameEN == "Pending" || IncIL.IncubationBaselineStatus.NameEN == "Baseline Application Form Updated")
                    {
                        ViewBag.LstDrop = new SelectList(repository.GetQuery<IncubationBaselineStatus>(f => f.NameEN == "Update Baseline Application Form" || f.NameEN == "Rejected" || f.NameEN == "Accepted").ToList(), "IncubationBaselineStatusID", "NameEN");
                    }
                    else
                    {
                        ViewBag.LstDrop = new SelectList(repository.GetQuery<IncubationBaselineStatus>(f => f.NameEN == "Update Baseline Application Form").ToList(), "IncubationBaselineStatusID", "NameEN");
                    }
                }
                return View(IncIL);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Details(Guid IncubationBaselineID, Guid IncubationBaselineStatusID, string feadback)
        {
            var _IncubationBaseline = repository.GetByKey<IncubationBaseline>(IncubationBaselineID);
            _IncubationBaseline.IncubationBaselineStatusID = IncubationBaselineStatusID;
            _IncubationBaseline.Feadback = feadback;
            repository.Update(_IncubationBaseline);
            repository.UnitOfWork.SaveChanges();

            var FrontEndMail = _IncubationBaseline.FrontendUser.AspNetUser.Email;
            var IncubationBaselineStatus = repository.GetByKey<IncubationBaselineStatus>(IncubationBaselineStatusID);

            MailHelper mailHelper = new MailHelper();
            mailHelper.ToEmail = FrontEndMail;
            if (IncubationBaselineStatus.NameEN == "Update Baseline Application Form")
            {
                mailHelper.Subject = "استكمال متطلبات نموذج أستبيان الوضع الحالي للمنظمة";
                mailHelper.Body = "المكرمين/ المنظمات غير الربحية                         الموقرين <br />"
                                    + "السلام عليكم ورحمة الله وبركاته، <br />"
                                    + "تهديكم مؤسسة الملك خالد أطيب التحيات، <br /> ونحيطكم علماً بأن طلب أستبيان الوضع الحالي للمنظمة الخاص بكم تنقصه البيانات التالية: <br />"
                                    + $" {feadback}  <br />"
                                    + $" نرجو منكم التكرم بإكمال البيانات على البوابة الإلكترونية وإعادة إرسال الطلب قبل موعد انتهاء ورشة العمل المراد الاشتراك بها.  <br />"
                                    + "شاكرين ومقدرين اهتمامكم وتعاونكم، <br />"
                                    + " برنامج بناء القدرات.  <br />";
            }
            if (IncubationBaselineStatus.NameEN == "Rejected")
            {
                mailHelper.Subject = "تنويه";
                mailHelper.Body = "المكرمين/ المنظمات غير الربحية                         الموقرين <br />"
                                    + "السلام عليكم ورحمة الله وبركاته، <br />"
                                    + "تهديكم مؤسسة الملك خالد أطيب التحيات،<br /> ويؤسفنا إبلاغكم بأنه تم رفض طلب أستبيان الوضع الحالي الخاص بمنظمتكم،<br /> وذلك للأسباب التالية :  <br />"
                                    + $" {feadback}  <br />"
                                    + "شاكرين ومقدرين اهتمامكم وتعاونكم، <br />"
                                    + " برنامج بناء القدرات.  <br />";
            }
            if (IncubationBaselineStatus.NameEN == "Accepted")
            {
                mailHelper.Subject = "تهنئة";
                mailHelper.Body = "المكرمين/ المنظمات غير الربحية                         الموقرين <br />"
                                    + "السلام عليكم ورحمة الله وبركاته، <br />"
                                    + "تهديكم مؤسسة الملك خالد أطيب التحيات، <br /> ونحيطكم علماً بأنه تم قبول طلب أستبيان الوضع الحالي الخاص بمنظمتكم للالتحاق، <br /> الرجاء الذهاب الي البوابه الإلكترونية لتقديم بيانات المشاركين في ورشة العمل المراد الاشتراك بها. <br />"
                                    + "مع تمنياتنا لكم بالتوفيق، <br />"
                                    + " برنامج بناء القدرات.  <br />";
            }
            mailHelper.Send("");

            return RedirectToAction("Index");
        }

        public ActionResult CorporateProfile(Guid Id)
        {
            ViewBag.lang = CultureHelper.CurrentCulture;
            return View(repository.FindOne<CorporateApplicationForm>(f => f.FrontendUserID == Id && f.Program.ProgramName == "Capacity Building"));
        }

        public ActionResult Download(string URL)
        {
            try
            {
                byte[] fileBytes = System.IO.File.ReadAllBytes(URL);
                string fileName = URL.Substring(URL.LastIndexOf('\\') + 1);
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }
            catch (Exception)
            {
                return Content("Something Went Wrong");
            }
        }
    }
}