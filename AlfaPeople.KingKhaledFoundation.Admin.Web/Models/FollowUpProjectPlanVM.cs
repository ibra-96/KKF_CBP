using System;
using System.Web;
using System.ComponentModel.DataAnnotations;
using AlfaPeople.KingKhalidFoundation.Data.Model;

namespace AlfaPeople.KingKhaledFoundation.Admin.Web.Models
{
    public class FollowUpProjectPlanVM
    {
        public FollowUpProjectPlan FollowUpProjectPlan { get; set; }

        public Guid[] FrontEndUsers { get; set; }

        [Display(Name = "Browse File")]
        public HttpPostedFileBase[] files { get; set; }
    }
}