using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlfaPeople.KingKhalidFoundation.Data.Model
{
    public class IncubationWorkshopRating
    {
        [Key]
        public Guid IncubationWorkshopRatingId { get; set; }

        public Guid FrontendUserId { get; set; }

        public FrontendUser FrontendUser { get; set; }

        public Guid IncubationWorkshopID { get; set; }

        public IncubationWorkshop IncubationWorkshop { get; set; }

        [Column(TypeName = "date")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime EvaluationDate { get; set; }

        public TrainingWorkshop TrainingWorkshop { get; set; }

        public string ReasonForTrainingWorkshop { get; set; }

        public AchievingGoal AchievingGoal { get; set; }

        public string ReasonForAchievingGoals { get; set; }

        public MeetTheWorkRequirement MeetTheWorkRequirement { get; set; }

        public string ReasonForMeetTheWorkRequirement { get; set; }

        public TrainingMaterial TrainingMaterial { get; set; }

        public string ReasonForTrainingMaterial { get; set; }

        public PartcipationReaction PartcipationReaction { get; set; }

        public string ReasonForPartcipationReaction { get; set; }

        public string Weakness { get; set; }

        public string Power { get; set; }

        public WorkshopClass WorkshopClass { get; set; }

        public string ReasonForWorkshopClass { get; set; }

        public WorkshopClass Hosting { get; set; }

        public string ReasonForHosting { get; set; }

        public TrainerRating TheAbilityToDeliverInformation { get; set; }

        public TrainerRating BodyLanguage { get; set; }

        public TrainerRating ClarityOfVoiceAndTone { get; set; }

        public TrainerRating MasteryOfTrainingMaterial { get; set; }

        public TrainerRating AbilityToManageDiscussionAndHandleQuestions { get; set; }

        public TrainerRating LinkingTheTrainingMaterialToReality { get; set; }

        public TrainerRating AbilityToAchieveAGoal { get; set; }

        public string CommentsOnTrainer { get; set; }
    }
}