using System;
using System.Web;
using System.ComponentModel.DataAnnotations;
using AlfaPeople.KingKhalidFoundation.Data.Model;

namespace AlfaPeople.KingKhaledFoundation.Admin.Web.Models
{
    public class FundingSourceVM
    {
        public FundingSource FundingSource { get; set; }

        [Display(Name = "Browse File")]
        public HttpPostedFileBase LogoPic { get; set; }

        [Display(Name = "Browse File")]
        public HttpPostedFileBase HeaderPic { get; set; }

        [Display(Name = "Browse File")]
        public HttpPostedFileBase GBackgroundPic { get; set; }

        [Display(Name = "Browse File")]
        public HttpPostedFileBase RBackgroundPic { get; set; }
    }
}