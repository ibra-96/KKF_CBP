using System;
using System.ComponentModel.DataAnnotations;

namespace AlfaPeople.KingKhalidFoundation.Data.Model
{
    public class IncubationWorkShopFollowUpEvaluationLines
    {
        [Key]
        public Guid Id { get; set; }

        public Guid IncubationWorkShopFollowUpEvaluationID { get; set; }

        public string Date { get; set; }

        [Required]
        public Guid BackendUserID { get; set; }

        public string PhaseOutput { get; set; }

        public string Notes { get; set; }

        public string NextStep { get; set; }

        public FollowUpMethod FollowUpMethod { get; set; }

        public virtual BackendUser Incubation { get; set; }

        public virtual IncubationWorkShopFollowUpEvaluation IncubationWorkShopFollowUpEvaluation { get; set; }

    }
}