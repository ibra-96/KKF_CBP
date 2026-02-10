namespace AlfaPeople.KingKhalidFoundation.Data.Model
{
    using System;
    using System.ComponentModel;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class IncubationWorkshop
    {
        public IncubationWorkshop()
        {
            IncubationWorkshopAttachments = new HashSet<IncubationWorkshopAttachment>();
            WorkshopProjectProposals = new HashSet<WorkshopProjectProposal>();
            WorkshopPrivateInvitations = new HashSet<WorkshopPrivateInvitation>();
            IncubationWorkshopRatings = new HashSet<IncubationWorkshopRating>();
            IncubationWorkShopFollowUpEvaluations = new HashSet<IncubationWorkShopFollowUpEvaluation>();
            FollowUpProjectPlans = new HashSet<FollowUpProjectPlan>();
            //3-2-2026
            ProjectImpactEvaluations = new HashSet<ProjectImpactEvaluation>();


        }

        public Guid IncubationWorkshopID { get; set; }
        //3-2-2026
        public virtual ICollection<ProjectImpactEvaluation> ProjectImpactEvaluations { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Column(TypeName = "date")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime StartDate { get; set; }

        [Column(TypeName = "date")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime EndDate { get; set; }

        [Column(TypeName = "date")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime LastTimeToApply { get; set; }

        [Required]
        public string TrainingHeadquarters { get; set; }

        [Required]
        [DefaultValue(true)]
        public bool ISPublic { get; set; }

        public Guid ConsultantID { get; set; }

        public Guid IncubationWorkshopModelID { get; set; }

        public Guid IncubationtWorkshopStatusID { get; set; }

        public Guid RegionID { get; set; }

        public Guid GovernorateID { get; set; }

        [Display(Name = "Neighborhood")]
        public Guid? CityID { get; set; }

        public Guid FundingSourceID { get; set; }

        public virtual FundingSource FundingSource { get; set; }

        public virtual Region Region { get; set; }

        public virtual City City { get; set; }

        public virtual Governorate Governorate { get; set; }

        public virtual Consultant Consultant { get; set; }

        public virtual IncubationtWorkshopStatu IncubationtWorkshopStatu { get; set; }

        public virtual IncubationWorkshopModel IncubationWorkshopModel { get; set; }

        public virtual ICollection<IncubationWorkshopAttachment> IncubationWorkshopAttachments { get; set; }

        public virtual ICollection<IncubationWorkshopRating> IncubationWorkshopRatings { get; set; }

        public virtual ICollection<WorkshopProjectProposal> WorkshopProjectProposals { get; set; }

        public virtual ICollection<WorkshopPrivateInvitation> WorkshopPrivateInvitations { get; set; }

        public virtual ICollection<IncubationWorkShopFollowUpEvaluation> IncubationWorkShopFollowUpEvaluations { get; set; }

        public virtual ICollection<FollowUpProjectPlan> FollowUpProjectPlans { get; set; }
        //shadia
        public virtual ICollection<IncubationWorkshopBLTransactions> IncubationWorkshopBLTransactions { get; set; } // الحقول الديناميكية
             //2-25-2025                                                                                                       //
        [Required]
        [DefaultValue(false)]
        public bool IsDeleted { get; set; } = false;

        public DateTime? DeletedDate { get; set; }
        public Guid? DeletedBy { get; set; }
        //end


    }
}