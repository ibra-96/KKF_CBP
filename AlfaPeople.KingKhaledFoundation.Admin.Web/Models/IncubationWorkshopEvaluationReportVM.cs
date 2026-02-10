using System;

namespace AlfaPeople.KingKhaledFoundation.Admin.Web.Models
{
    public class IncubationWorkshopEvaluationReportVM
    {
        public TrainingWorkshopVM HowDoYouEvaluateTheWorkshop { get; set; }
        public AchievingGoalVM WorkshopAchieveObjectives { get; set; }
        public MeetTheWorkRequirementVM IsWorkshopUseful { get; set; }
        public TrainingWorkshopVM TrainingMaterial { get; set; }
        public TrainingWorkshopVM HowDoYouEvaluateParticpation { get; set; }
        public TrainerRatingVM TheAbilityToDeliverTheInformation { get; set; }
        public TrainerRatingVM BodyLanguage { get; set; }
        public TrainerRatingVM ClarityOfVoiceAndTone { get; set; }
        public TrainerRatingVM MasteryOfTrainingMaterial { get; set; }
        public TrainerRatingVM AbilityToManageDiscussionAndHandleQuestions { get; set; }
        public TrainerRatingVM LinkingTheTrainingMaterialToReality { get; set; }
        public TrainerRatingVM AbilityToAchieveAGoal { get; set; }
        public WorkshopClassVM WorkshopClass { get; set; }
        public WorkshopClassVM Hosting { get; set; }
    }

    public class TrainingWorkshopVM
    {
        public int VeryGood { get; set; }
        public int Average { get; set; }
        public int Bad { get; set; }
    }

    public class TrainerRatingVM
    {
        public int Excellent { get; set; }
        public int VGood { get; set; }
        public int Good { get; set; }
        public int OK { get; set; }
        public int Bad { get; set; }
    }

    public class AchievingGoalVM
    {
        public int GoalsAchievedCompletely { get; set; }
        public int TheGoalsWerePartiallyAchieved { get; set; }
        public int DidNotAchieveGoals { get; set; }
    }

    public class MeetTheWorkRequirementVM
    {
        public int Useful { get; set; }
        public int SomewhatUseful { get; set; }
        public int Unuseful { get; set; }
    }

    public class WorkshopClassVM
    {
        public int Fit { get; set; }
        public int FairlyAppropriate { get; set; }
        public int Unfit { get; set; }
    }
}