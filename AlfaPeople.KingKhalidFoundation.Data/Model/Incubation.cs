namespace AlfaPeople.KingKhalidFoundation.Data.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class Incubation
    {
        public Incubation()
        {
            IncubationAttchments = new HashSet<IncubationAttchment>();
            IncubationFollowUpEvaluations = new HashSet<IncubationFollowUpEvaluation>();
        }

        public Guid IncubationID { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public int ProjectDurationInDays { get; set; }

        [Column(TypeName = "date")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime StartDate { get; set; }

        [Column(TypeName = "date")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime EndDate { get; set; }

        [Required]
        public string ProjectDetails { get; set; }

        [Required]
        public string Targeted { get; set; }

        [Required]
        public string Notes { get; set; }

        public Guid IncubationModelID { get; set; }

        public Guid IncubationTypeID { get; set; }

        public Guid IncubationStatusID { get; set; }

        public Guid IncubationAdID { get; set; }

        public Guid ConsultantID { get; set; }

        public Guid TypeOfInterventionID { get; set; }

        public virtual TypeOfIntervention TypeOfIntervention { get; set; }

        [Required]
        [ForeignKey("BackendUser")]
        public Guid SpecialistChargeOfIncubation { get; set; }

        public virtual BackendUser BackendUser { get; set; }

        [Required]
        [ForeignKey("IncubationProjectProposal")]
        public Guid IncubationProjectProposalID { get; set; }

        public virtual Consultant Consultant { get; set; }

        public virtual IncubationType IncubationType { get; set; }

        public virtual IncubationModel IncubationModel { get; set; }

        public virtual IncubationStatus IncubationStatus { get; set; }

        public virtual IncubationAdvertising IncubationAdvertising { get; set; }

        public virtual IncubationProjectProposal IncubationProjectProposal { get; set; }

        public virtual ICollection<IncubationAttchment> IncubationAttchments { get; set; }

        public virtual ICollection<IncubationFollowUpEvaluation> IncubationFollowUpEvaluations { get; set; }
    }
}