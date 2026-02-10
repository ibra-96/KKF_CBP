using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlfaPeople.KingKhalidFoundation.Data.Model
{
    public class FollowUpProjectPlan
    {
        public FollowUpProjectPlan()
        {
            FollowUpProjectPlanAttachments = new HashSet<FollowUpProjectPlanAttachment>();
            FollowUpProjectPlanRequests = new HashSet<FollowUpProjectPlanRequest>();
        }

        [Key]
        public Guid FollowUpProjectPlanId { get; set; }

        [Required]
        public Guid IncubationWorkshopID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [Column(TypeName = "date")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime Deadline { get; set; }

        public string Notes { get; set; }

        public virtual IncubationWorkshop IncubationWorkshop { get; set; }

        public virtual ICollection<FollowUpProjectPlanAttachment> FollowUpProjectPlanAttachments { get; set; }

        public virtual ICollection<FollowUpProjectPlanRequest> FollowUpProjectPlanRequests { get; set; }

    }
}