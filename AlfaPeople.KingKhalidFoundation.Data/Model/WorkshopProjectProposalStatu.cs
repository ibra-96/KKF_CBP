using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AlfaPeople.KingKhalidFoundation.Data.Model
{
    public class WorkshopProjectProposalStatu
    {
        public WorkshopProjectProposalStatu()
        {
            WorkshopProjectProposals = new HashSet<WorkshopProjectProposal>();
        }

        [Key]
        public Guid WorkshopProjectProposalStatusID { get; set; }

        [Required]
        [StringLength(50)]
        public string NameAR { get; set; }

        [Required]
        [StringLength(50)]
        public string NameEN { get; set; }

        public virtual ICollection<WorkshopProjectProposal> WorkshopProjectProposals { get; set; }
    }
}