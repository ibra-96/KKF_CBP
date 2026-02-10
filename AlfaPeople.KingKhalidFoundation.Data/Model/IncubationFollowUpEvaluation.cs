using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AlfaPeople.KingKhalidFoundation.Data.Model
{
    public partial class IncubationFollowUpEvaluation
    {
        public IncubationFollowUpEvaluation()
        {
            IncubationFollowUpEvaluationLines = new HashSet<IncubationFollowUpEvaluationLines>();
            IncubationFollowUpEvaluationAttchments = new HashSet<IncubationFollowUpEvaluationAttchment>();
        }

        [Key]
        public Guid IncubationFollowUpEvaluationID { get; set; }

        [Required]
        public Guid FrontendUserID { get; set; }

        [Required]
        public Guid IncubationID { get; set; }

        public Guid IncubationTypeID { get; set; }

        public string Impact { get; set; }

        public string SuccessStories { get; set; }

        public ProjectSuccessRate ProjectSuccessRate { get; set; }

        public string Reasone { get; set; }

        public virtual IncubationType IncubationType { get; set; }

        public virtual FrontendUser FrontendUser { get; set; }

        public virtual Incubation Incubation { get; set; }

        public virtual ICollection<IncubationFollowUpEvaluationLines> IncubationFollowUpEvaluationLines { get; set; }

        public virtual ICollection<IncubationFollowUpEvaluationAttchment> IncubationFollowUpEvaluationAttchments { get; set; }
    }
}