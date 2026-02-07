using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AlfaPeople.KingKhaledFoundation.Admin.Web.Models
{
    public class CanceledWorkshopInvitationsReportVM
    {
        public string WorkshopType { get; set; }
        public string WorkshopName { get; set; }
        public string Email { get; set; }
        public string OrganizationName { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime InvitationDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? CancellationDate { get; set; } // تاريخ إلغاء الدعوة
    }

}