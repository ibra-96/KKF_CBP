using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AlfaPeople.KingKhaledFoundation.Admin.Web.Models
{
    public class CanceledIncubationInvitationsReportVM
    {
        public string IncubationType { get; set; } // نوع الاحتضان (جزئي أو كامل)
        public string AdvertisingName { get; set; } // اسم الإعلان
        public string Email { get; set; } // البريد الإلكتروني للمدعو
        public string OrganizationName { get; set; } // اسم المنظمة
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime InvitationDate { get; set; } // تاريخ إرسال الدعوة
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? CancellationDate { get; set; } // تاريخ إلغاء الدعوة

    }

}