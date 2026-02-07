using System;
using System.Web;
using System.ComponentModel.DataAnnotations;
using AlfaPeople.KingKhalidFoundation.Data.Model;

namespace AlfaPeople.KingKhalidFoundation.Web.Models
{
    public class UpdateCorporationProfileVM
    {
        public CorporateApplicationForm _CorporateApplicationForm { get; set; }

        [Display(Name = "Browse File")]
        public HttpPostedFileBase[] files { get; set; }

        [Display(Name = "Browse File")]
        public HttpPostedFileBase file { get; set; }
    }
}