using System;
using System.Web;
using System.ComponentModel.DataAnnotations;
using AlfaPeople.KingKhalidFoundation.Data.Model;
using System.Collections.Generic;

namespace AlfaPeople.KingKhalidFoundation.Web.Models
{
    public class CorporationRegistrationFormVM
    {
        public RegisterVM _RegisterVM { get; set; }

        public CorporateApplicationForm _CorporateApplicationForm { get; set; }

        [Required(ErrorMessageResourceName = "SelectFileMsg", ErrorMessageResourceType = typeof(App_GlobalResources.General))]
        [Display(Name = "BrowseFile", ResourceType = typeof(App_GlobalResources.General))]
        public HttpPostedFileBase[] files { get; set; }

        [Required(ErrorMessageResourceName = "SelectImageMsg", ErrorMessageResourceType = typeof(App_GlobalResources.General))]
        [Display(Name = "BrowseImage", ResourceType = typeof(App_GlobalResources.General))]
        public HttpPostedFileBase file { get; set; }
        //26-2-2025
        public bool IsDraft { get; set; } = true; // الحقل الافتراضي لتحديد ما إذا كان الحفظ كمسودة
        public int ActiveTab { get; set; } = 1; // التاب الأول كافتراضي

        //17-3-2025
        public List<CorporateApplicationFormAttachment> Attachments { get; set; }
    }
}