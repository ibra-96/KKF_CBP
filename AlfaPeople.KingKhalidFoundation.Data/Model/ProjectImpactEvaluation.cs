using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlfaPeople.KingKhalidFoundation.Data.Model
{
    public class ProjectImpactEvaluation
    {
        public ProjectImpactEvaluation()
        {
            ProjectImpactEvaluationRequests = new HashSet<ProjectImpactEvaluationRequest>();
        }

        [Key]
        public Guid ProjectImpactEvaluationId { get; set; }

        [Required]
        public Guid IncubationWorkshopID { get; set; }

        [Required]
        [Column(TypeName = "date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Deadline { get; set; }

        [Column(TypeName = "date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? ReminderDate { get; set; }

        public string Notes { get; set; }

       
        public bool IsFilled { get; set; }

        public virtual IncubationWorkshop IncubationWorkshop { get; set; }
        public virtual ICollection<ProjectImpactEvaluationRequest> ProjectImpactEvaluationRequests { get; set; }
    }
}
