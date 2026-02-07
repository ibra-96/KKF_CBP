namespace AlfaPeople.KingKhalidFoundation.Data.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class IncubationProjectProposalStatu
    {
        public IncubationProjectProposalStatu()
        {
            IncubationProjectProposals = new HashSet<IncubationProjectProposal>();
        }

        [Key]
        public Guid IncubationProjectProposalStatusID { get; set; }

        [Required]
        [StringLength(50)]
        public string NameAR { get; set; }

        [Required]
        [StringLength(50)]
        public string NameEN { get; set; }

        public virtual ICollection<IncubationProjectProposal> IncubationProjectProposals { get; set; }
    }
}