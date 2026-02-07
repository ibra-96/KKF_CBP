namespace AlfaPeople.KingKhalidFoundation.Data.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class IncubationProjectProposal
    {
        public IncubationProjectProposal()
        {
            Incubations = new HashSet<Incubation>();
            IncubationProjectProposalAttachements = new HashSet<IncubationProjectProposalAttachement>();
        }

        public Guid IncubationProjectProposalID { get; set; }

        [Required]
        public Guid? IncubationProjectProposalStatusID { get; set; }

        [Required]
        public Guid? IncubationAdID { get; set; }

        [Required]
        public Guid FrontendUserID { get; set; }

        [Column(TypeName = "date")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime submissionDate { get; set; }

        public string Feadback { get; set; }

        public virtual FrontendUser FrontendUser { get; set; }

        public virtual ICollection<Incubation> Incubations { get; set; }

        public virtual IncubationAdvertising IncubationAdvertising { get; set; }

        public virtual IncubationProjectProposalStatu IncubationProjectProposalStatu { get; set; }


        public virtual ICollection<IncubationProjectProposalAttachement> IncubationProjectProposalAttachements { get; set; }
    }
}