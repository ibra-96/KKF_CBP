using System;
using System.Web;
using AlfaPeople.KingKhalidFoundation.Data.Model;
using System.ComponentModel.DataAnnotations;

namespace AlfaPeople.KingKhaledFoundation.Admin.Web.Models
{
    public class IncubationWorkshopVM
    {
        public IncubationWorkshop IncubationWorkshop { get; set; }

        [Display(Name = "Browse File")]
        public HttpPostedFileBase[] files { get; set; }

        public string LstOfEmails { get; set; }

        public bool SendInvitation { get; set; }
    }
}