using System;
using System.Web;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AlfaPeople.KingKhalidFoundation.Data.Model;

namespace AlfaPeople.KingKhaledFoundation.Admin.Web.Models
{
    public class IncubationWorkshopFollowUpEvaluationVM
    {
        [Display(Name = "Browse File")]
        public HttpPostedFileBase[] files { get; set; }

        public IncubationWorkShopFollowUpEvaluation IFEVM { get; set; }

        public List<IncubationWorkShopFollowUpEvaluationLines> LstIFELVM { get; set; }
    }
}