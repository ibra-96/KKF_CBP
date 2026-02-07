namespace AlfaPeople.KingKhalidFoundation.Data.Model
{
    using System;
    using System.ComponentModel;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class FrontendUser
    {
        public FrontendUser()
        {
            IndividualApplicationForms = new HashSet<IndividualApplicationForm>();

            FrontendUsers1 = new HashSet<FrontendUser>();
            FrontendGroups = new HashSet<FrontendGroup>();
            CorporateApplicationForms = new HashSet<CorporateApplicationForm>();
            WorkshopProjectProposals = new HashSet<WorkshopProjectProposal>();
            Incubations = new HashSet<Incubation>();
            IncubationProjectProposals = new HashSet<IncubationProjectProposal>();
            IncubationFollowUpEvaluations = new HashSet<IncubationFollowUpEvaluation>();
            IncubationWorkShopFollowUpEvaluations = new HashSet<IncubationWorkShopFollowUpEvaluation>();
            IncubationWorkshopRatings = new HashSet<IncubationWorkshopRating>();
            WorkshopPrivateInvitations = new HashSet<WorkshopPrivateInvitation>();
            FollowUpProjectPlanRequests = new HashSet<FollowUpProjectPlanRequest>();
            IncubationBaselines = new HashSet<IncubationBaseline>();
            IncubationWSBLTransactionsValue = new HashSet<IncubationWorkshopBLTransactionsValue>();
        }

        [Key]
        public Guid UserID { get; set; }

        public Guid? ParentID { get; set; }

        public DateTime CreateDate { get; set; }

        public bool LockoutEnabled { get; set; }

        public DateTime? LockoutEndDateUtc { get; set; }

        public bool IsApproved { get; set; }

        public string Password { get; set; }

        [DefaultValue(true)]
        public bool IsActive { get; set; }

        [Required]
        [StringLength(128)]
        public string FK_AspUser { get; set; }

        public virtual AspNetUser AspNetUser { get; set; }

        public virtual ICollection<CorporateApplicationForm> CorporateApplicationForms { get; set; }

        public virtual ICollection<IncubationProjectProposal> IncubationProjectProposals { get; set; }

        public virtual ICollection<WorkshopProjectProposal> WorkshopProjectProposals { get; set; }

        public virtual ICollection<IndividualApplicationForm> IndividualApplicationForms { get; set; }

        public virtual ICollection<FrontendUser> FrontendUsers1 { get; set; }

        public virtual ICollection<Incubation> Incubations { get; set; }

        public virtual FrontendUser FrontendUser1 { get; set; }

        public virtual ICollection<FrontendGroup> FrontendGroups { get; set; }

        public virtual ICollection<IncubationFollowUpEvaluation> IncubationFollowUpEvaluations { get; set; }

        public virtual ICollection<IncubationWorkShopFollowUpEvaluation> IncubationWorkShopFollowUpEvaluations { get; set; }

        public virtual ICollection<IncubationWorkshopRating> IncubationWorkshopRatings { get; set; }

        public virtual ICollection<WorkshopPrivateInvitation> WorkshopPrivateInvitations { get; set; }

        public virtual ICollection<FollowUpProjectPlanRequest> FollowUpProjectPlanRequests { get; set; }
        public virtual ICollection<IncubationBaseline> IncubationBaselines { get; set; }
        public virtual ICollection<IncubationWorkshopBLTransactionsValue> IncubationWSBLTransactionsValue { get; set; }

    }
}