using System;
using System.Web;
using AlfaPeople.KingKhalidFoundation.Data.Model;
using System.ComponentModel.DataAnnotations;

namespace AlfaPeople.KingKhalidFoundation.Web.Models
{
    public class FollowUpProjectPlanJoinVM
    {
        [Display(Name = "Browse File")]

        public HttpPostedFileBase[] files { get; set; }

        public FollowUpProjectPlan FollowUpProjectPlan { get; set; }

        public FollowUpProjectPlanRequest FollowUpProjectPlanRequest { get; set; }
    }
}