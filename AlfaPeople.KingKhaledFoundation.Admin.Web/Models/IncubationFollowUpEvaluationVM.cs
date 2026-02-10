using System;
using System.Web;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AlfaPeople.KingKhalidFoundation.Data.Model;

namespace AlfaPeople.KingKhaledFoundation.Admin.Web.Models
{
    public class IncubationFollowUpEvaluationVM
    {
        [Display(Name = "Browse File")]
        public HttpPostedFileBase[] files { get; set; }

        public IncubationFollowUpEvaluation IFEVM { get; set; }

        public List<IncubationFollowUpEvaluationLines> LstIFELVM { get; set; }
    }
}