using System;
using System.ComponentModel.DataAnnotations;

namespace AlfaPeople.KingKhalidFoundation.Data.Model
{
    public enum CorporateGenderType
    {
        [Display(Name = "Men", ResourceType = typeof(Resources.Genders))]
        men,

        [Display(Name = "Woman", ResourceType = typeof(Resources.Genders))]
        women,

        [Display(Name = "Both", ResourceType = typeof(Resources.Genders))]
        Joint
    }

    public enum EvaluationGrants
    {
        [Display(Name = "Successful", ResourceType = typeof(Resources.EvaluationGrants))]
        Successful,
        [Display(Name = "Average", ResourceType = typeof(Resources.EvaluationGrants))]
        Average,
        [Display(Name = "DidNotExpected", ResourceType = typeof(Resources.EvaluationGrants))]
        DidNotExpected
    }

    public enum CorporationPerformance
    {
        [Display(Name = "Excellent", ResourceType = typeof(Resources.CorporationPerformance))]
        Excellent,
        [Display(Name = "Good", ResourceType = typeof(Resources.CorporationPerformance))]
        Good,
        [Display(Name = "Bad", ResourceType = typeof(Resources.CorporationPerformance))]
        Bad
    }

    public enum FieldOfEmployment
    {
        [Display(Name = "Administrative", ResourceType = typeof(Resources.FieldOfEmployment))]
        Administrative,
        [Display(Name = "Professional", ResourceType = typeof(Resources.FieldOfEmployment))]
        Professional,
        [Display(Name = "Literary", ResourceType = typeof(Resources.FieldOfEmployment))]
        Literary
    }

    public enum DonorTypes
    {

        [Display(Name = "DonorInstitutions", ResourceType = typeof(Resources.DonorTypes))]
        DonorInstitutions,
        [Display(Name = "GovernmentAgencies", ResourceType = typeof(Resources.DonorTypes))]
        GovernmentAgencies,
        [Display(Name = "PrivateSector", ResourceType = typeof(Resources.DonorTypes))]
        PrivateSector,
        [Display(Name = "Individuals", ResourceType = typeof(Resources.DonorTypes))]
        Individuals,
        [Display(Name = "Investments", ResourceType = typeof(Resources.DonorTypes))]
        Investments,
        [Display(Name = "Alms", ResourceType = typeof(Resources.DonorTypes))]
        Alms
    }

    public enum ProjectSuccessRate
    {
        [Display(Name = "HighSuccessRate", ResourceType = typeof(Resources.ProjectSuccessRate))]
        HighSuccessRate,
        [Display(Name = "MoreThanAverageSuccessRate", ResourceType = typeof(Resources.ProjectSuccessRate))]
        MoreThanAverageSuccessRate,
        [Display(Name = "AverageSuccessRate", ResourceType = typeof(Resources.ProjectSuccessRate))]
        AverageSuccessRate,
        [Display(Name = "LowSuccessRate", ResourceType = typeof(Resources.ProjectSuccessRate))]
        LowSuccessRate,
        [Display(Name = "Fail", ResourceType = typeof(Resources.ProjectSuccessRate))]
        Fail,
        [Display(Name = "undefined", ResourceType = typeof(Resources.ProjectSuccessRate))]
        Undefined
    }

    public enum FollowUpMethod
    {
        [Display(Name = "PhoneCall", ResourceType = typeof(Resources.FollowUpMethod))]
        PhoneCall,
        [Display(Name = "FieldVisit", ResourceType = typeof(Resources.FollowUpMethod))]
        FieldVisit,
        [Display(Name = "MeetingAtTheFoundation", ResourceType = typeof(Resources.FollowUpMethod))]
        MeetingAtTheFoundation,
        [Display(Name = "EmailQuestionnairelink", ResourceType = typeof(Resources.FollowUpMethod))]
        EmailQuestionnairelink
    }

    public enum TrainingWorkshop
    {
        [Display(Name = "VeryGood", ResourceType = typeof(Resources.TrainingWorkshop))]
        VeryGood,
        [Display(Name = "Average", ResourceType = typeof(Resources.TrainingWorkshop))]
        Average,
        [Display(Name = "Bad", ResourceType = typeof(Resources.TrainingWorkshop))]
        Bad
    }

    public enum AchievingGoal
    {
        [Display(Name = "TheGoalsWerePartiallyAchieved", ResourceType = typeof(Resources.AchievingGoal))]
        TheGoalsWerePartiallyAchieved,
        [Display(Name = "GoalsAchievedCompletely", ResourceType = typeof(Resources.AchievingGoal))]
        GoalsAchievedCompletely,
        [Display(Name = "DidNotAchieveGoals", ResourceType = typeof(Resources.AchievingGoal))]
        DidNotAchieveGoals,
    }

    public enum MeetTheWorkRequirement
    {
        [Display(Name = "Useful", ResourceType = typeof(Resources.MeetTheWorkRequirement))]
        Useful,
        [Display(Name = "SomewhatUseful", ResourceType = typeof(Resources.MeetTheWorkRequirement))]
        SomewhatUseful,
        [Display(Name = "Unuseful", ResourceType = typeof(Resources.MeetTheWorkRequirement))]
        UnUseful
    }

    public enum TrainingMaterial
    {
        [Display(Name = "VeryGood", ResourceType = typeof(Resources.TrainingWorkshop))]
        VeryGood,
        [Display(Name = "Average", ResourceType = typeof(Resources.TrainingWorkshop))]
        Average,
        [Display(Name = "Bad", ResourceType = typeof(Resources.TrainingWorkshop))]
        Bad
    }

    public enum PartcipationReaction
    {
        [Display(Name = "VeryGood", ResourceType = typeof(Resources.TrainingWorkshop))]
        VeryGood,
        [Display(Name = "Average", ResourceType = typeof(Resources.TrainingWorkshop))]
        Average,
        [Display(Name = "Bad", ResourceType = typeof(Resources.TrainingWorkshop))]
        Bad
    }

    public enum WorkshopClass
    {
        [Display(Name = "Fit", ResourceType = typeof(Resources.WorkshopClass))]
        Fit,
        [Display(Name = "FairlyAppropriate", ResourceType = typeof(Resources.WorkshopClass))]
        FairlyAppropriate,
        [Display(Name = "Unfit", ResourceType = typeof(Resources.WorkshopClass))]
        Unfit
    }

    public enum Hosting
    {
        [Display(Name = "Good", ResourceType = typeof(Resources.TrainerRating))]
        Good,
        [Display(Name = "Bad", ResourceType = typeof(Resources.TrainerRating))]
        Bad
    }

    public enum TrainerRating
    {
        [Display(Name = "Bad", ResourceType = typeof(Resources.TrainerRating))]
        Bad = 1,
        [Display(Name = "OK", ResourceType = typeof(Resources.TrainerRating))]
        OK = 2,
        [Display(Name = "Good", ResourceType = typeof(Resources.TrainerRating))]
        Good = 3,
        [Display(Name = "VGood", ResourceType = typeof(Resources.TrainerRating))]
        VGood = 4,
        [Display(Name = "Excellent", ResourceType = typeof(Resources.TrainerRating))]
        Excellent = 5
    }

    public enum InvitationStatus
    {
        [Display(Name = "attend", ResourceType = typeof(Resources.InvitationStatus))]
        attend,
        [Display(Name = "absent", ResourceType = typeof(Resources.InvitationStatus))]
        absent,
        [Display(Name = "pending", ResourceType = typeof(Resources.InvitationStatus))]
        pending,
        [Display(Name = "cancel", ResourceType = typeof(Resources.InvitationStatus))]
        cancel,
        //2-25-2025
        [Display(Name = "form_filled", ResourceType = typeof(Resources.InvitationStatus))]
        form_filled,
    }

    public enum WorkshopPPInvitationStatus
    {
        [Display(Name = "attend", ResourceType = typeof(Resources.WorkshopPPInvitationStatus))]
        attend,
        [Display(Name = "absent", ResourceType = typeof(Resources.WorkshopPPInvitationStatus))]
        absent,
    }

    public enum FollowUpProjectPlanStatus
    {
        [Display(Name = "Accept", ResourceType = typeof(Resources.FollowUpProjectPlanStatus))]
        Accept,
        [Display(Name = "Reject", ResourceType = typeof(Resources.FollowUpProjectPlanStatus))]
        Reject,
        [Display(Name = "Pending", ResourceType = typeof(Resources.FollowUpProjectPlanStatus))]
        Pending,
        [Display(Name = "UpdateProjectPlan", ResourceType = typeof(Resources.FollowUpProjectPlanStatus))]
        UpdateProjectPlan,
        [Display(Name = "ProjectPlanUpdated", ResourceType = typeof(Resources.FollowUpProjectPlanStatus))]
        ProjectPlanUpdated
    }

    public enum GenderPerson
    {
        [Display(Name = "Male", ResourceType = typeof(Resources.GenderPerson))]
        Male,
        [Display(Name = "Female", ResourceType = typeof(Resources.GenderPerson))]
        Female
    }
}