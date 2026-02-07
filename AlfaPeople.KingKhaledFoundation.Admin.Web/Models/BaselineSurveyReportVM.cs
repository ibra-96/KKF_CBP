using System;
using System.Collections.Generic;

namespace AlfaPeople.KingKhaledFoundation.Admin.Web.Models
{
    //7-4-2025
    public class BaselineSurveyReportVM
    {
        public Guid IncubationBaselineID { get; set; }
        public Guid IncubationBaselineStatusID { get; set; }
        public Guid FrontendUserID { get; set; }
        public string Region { get; set; }
        public string Governorate { get; set; }
        public string City { get; set; }

        public DateTime? SubmissionDate { get; set; }
        public string SubmissionDateText => SubmissionDate?.ToString("yyyy-MM-dd");

        public string Feadback { get; set; }

        public string CorporationName { get; set; }
        public string Email { get; set; }
        public string StatusName { get; set; }

        #region The Strategy
        public string ReasonEstablishingCorporate { get; set; }
        public string History { get; set; }
        public string Vision { get; set; }
        public string Mission { get; set; }
        public string Objectives { get; set; }
        public string HowPlanMade { get; set; }
        public string StrategicPlan { get; set; }
        public string OperatingPlan { get; set; }
        public string StrategicObjective { get; set; }
        #endregion

        #region Board of Directors
        public int? BoardDirectorsCount { get; set; }
        public DateTime? DateBoardApproval { get; set; }
        public string GovernanceGuide { get; set; }
        public bool InternalRegulation { get; set; }
        public int? BoardMeetingNumber { get; set; }
        public string BoardDirectorRoles { get; set; }
        #endregion

        #region Finance Resource
        public int? FRDevSpecialistsCount { get; set; }
        public int? CorporateRevenuesLastYear { get; set; }
        public int? SupportersCountLastYear { get; set; }
        public bool DonorInstitutions { get; set; }
        public bool GovernmentAgencies { get; set; }
        public bool PrivateSector { get; set; }
        public bool Individuals { get; set; }
        public bool Investments { get; set; }
        public bool Alms { get; set; }
        public string IndicateNumberAndSupportAmount { get; set; }
        public int? CurrentCorporateBudget { get; set; }
        public int? CharteredAccountantNotesCount { get; set; }
        public string CharteredAccountantNotes { get; set; }
        #endregion

        #region Programs & Projects
        public int? specializedPandPDepartmentEmpCount { get; set; }
        public bool PlanAccordingStrategicObjectives { get; set; }
        public string PlanImplementationActivitiesParticipationClarify { get; set; }
        public string IndicateDevelopmentProjectsWorkingOrganization { get; set; }
        public int? BudgetAllocatedProjects { get; set; }
        public int? BudgetAllocatedOperation { get; set; }
        #endregion

        #region Follow-up & Evaluation
        public int? DepartmentFollowUpEvaluationEmpCount { get; set; }
        public bool OrganizationFollowUpEvaluationPlan { get; set; }
        public bool OrganizationFollowUpEvaluationReports { get; set; }
        public string AttachFollowUpandEvaluationForms { get; set; }
        #endregion

        #region Corporate Communications
        public int? DepartmentCorporateCommunicationPublicRelationsEmpCount { get; set; }
        public bool CorporateCommunicationPlan { get; set; }
        public bool SeparateItemGeneralBudget { get; set; }
        public int? BudgetAllocatedCorporateCommunication { get; set; }
        #endregion

        #region Human Resources
        public int? DepartmentHREmpCount { get; set; }
        public int? FullTimeStaff { get; set; }
        public int? PartTimeStaff { get; set; }
        public int? VolunteerStaff { get; set; }
        public string AttractEmployees { get; set; }
        public string StaffPerformanceMonitored { get; set; }
        public string OrganizationalStructureUnderHRManagement { get; set; }
        public string TrainingNeedsCorporateStaffIdentified { get; set; }
        #endregion
    }
}
