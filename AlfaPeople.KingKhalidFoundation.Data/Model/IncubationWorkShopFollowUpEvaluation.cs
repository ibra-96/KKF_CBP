using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AlfaPeople.KingKhalidFoundation.Data.Model
{
    public partial class IncubationWorkShopFollowUpEvaluation
    {
        public IncubationWorkShopFollowUpEvaluation()
        {
            IncubationWorkShopFollowUpEvaluationLines = new HashSet<IncubationWorkShopFollowUpEvaluationLines>();
            IncubationWorkShopFollowUpEvaluationAttchments = new HashSet<IncubationWorkShopFollowUpEvaluationAttchment>();
        }

        [Key]
        public Guid IncubationWorkShopFollowUpEvaluationID { get; set; }

        [Required]
        public Guid FrontendUserID { get; set; }

        [Required]
        public Guid IncubationWorkshopID { get; set; }

        public string Impact { get; set; }

        public string SuccessStories { get; set; }

        public ProjectSuccessRate ProjectSuccessRate { get; set; }

        public string Reasone { get; set; }

        public virtual FrontendUser FrontendUser { get; set; }

        public virtual IncubationWorkshop IncubationWorkshop { get; set; }

        public virtual ICollection<IncubationWorkShopFollowUpEvaluationLines> IncubationWorkShopFollowUpEvaluationLines { get; set; }

        public virtual ICollection<IncubationWorkShopFollowUpEvaluationAttchment> IncubationWorkShopFollowUpEvaluationAttchments { get; set; }

    }
}