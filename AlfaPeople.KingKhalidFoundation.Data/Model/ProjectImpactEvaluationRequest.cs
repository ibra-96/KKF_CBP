using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlfaPeople.KingKhalidFoundation.Data.Model
{

    public enum ProjectImpactEvaluationStatus
    {
        Pending = 0,
        Filled = 1
    }

    public class ProjectImpactEvaluationRequest
    {
        [Key]
        public Guid ProjectImpactEvaluationRequestId { get; set; }

        [Required]
        public Guid ProjectImpactEvaluationId { get; set; }

        [Required]
        public Guid FrontendUserId { get; set; }

        public ProjectImpactEvaluationStatus Status { get; set; } = ProjectImpactEvaluationStatus.Pending;

        public DateTime? ReminderSentOn { get; set; }
        public DateTime? FilledOn { get; set; }

        public virtual ProjectImpactEvaluation ProjectImpactEvaluation { get; set; }
        public virtual FrontendUser FrontendUser { get; set; }
    }
}
