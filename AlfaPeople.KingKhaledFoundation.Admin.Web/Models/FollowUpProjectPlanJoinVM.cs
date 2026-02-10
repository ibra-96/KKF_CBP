using System;
using System.Web;
using AlfaPeople.KingKhalidFoundation.Data.Model;

namespace AlfaPeople.KingKhaledFoundation.Admin.Web.Models
{
    public class FollowUpProjectPlanJoinVM
    {
        public HttpPostedFileBase[] files { get; set; }

        public FollowUpProjectPlan FollowUpProjectPlan { get; set; }

        public FollowUpProjectPlanRequest FollowUpProjectPlanRequest { get; set; }
    }
}