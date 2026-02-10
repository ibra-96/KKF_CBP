using System;
using System.Web;
using System.ComponentModel.DataAnnotations;
using AlfaPeople.KingKhalidFoundation.Data.Model;

namespace AlfaPeople.KingKhaledFoundation.Admin.Web.Models
{
    public class IncubationAdvertisingVM
    {
        public IncubationAdvertising incubationAdvertising { get; set; }

        [Display(Name = "Browse File")]
        public HttpPostedFileBase[] files { get; set; }

        [Required(ErrorMessage = "Please select Models.")]
        public Guid[] IncubationAdvertisingModels { get; set; }

        public string LstOfEmails { get; set; }
    }
}