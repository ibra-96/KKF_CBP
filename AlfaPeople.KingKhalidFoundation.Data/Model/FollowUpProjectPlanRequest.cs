using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AlfaPeople.KingKhalidFoundation.Data.Model
{
    public class FollowUpProjectPlanRequest
    {
        public FollowUpProjectPlanRequest()
        {
            FollowUpProjectPlanRequestAttachments = new HashSet<FollowUpProjectPlanRequestAttachment>();
        }

        [Key]
        public Guid FollowUpProjectPlanRequestId { get; set; }

        [Required]
        public Guid FollowUpProjectPlanId { get; set; }

        [Required]
        public Guid FrontendUserId { get; set; }

        [Required]
        public FollowUpProjectPlanStatus FollowUpProjectPlanStatus { get; set; }

        public string Notes { get; set; }

        public string feedBack { get; set; }

        public virtual FrontendUser FrontendUser { get; set; }

        public virtual FollowUpProjectPlan FollowUpProjectPlan { get; set; }

        public virtual ICollection<FollowUpProjectPlanRequestAttachment> FollowUpProjectPlanRequestAttachments { get; set; }
    }
}