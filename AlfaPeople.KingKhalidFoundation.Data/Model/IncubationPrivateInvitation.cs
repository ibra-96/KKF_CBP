using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlfaPeople.KingKhalidFoundation.Data.Model
{
    public class IncubationPrivateInvitation
    {
        [Key]
        public Guid ID { get; set; }

        [Required]
        public string Email { get; set; }

        public Guid IncubationAdID { get; set; }

        public virtual IncubationAdvertising IncubationAdvertising { get; set; }
        //20-2-2025
        //  إضافة علاقة مع `FrontendUser` لاسترجاع اسم الجمعية
        public Guid? FrontendUserID { get; set; }  // ID المستخدم المرتبط
        [ForeignKey("FrontendUserID")]
        public virtual FrontendUser FrontendUser { get; set; }
        // غضافة الحالة للدعوة 
        public InvitationStatus Status { get; set; }
        //24-2-2025
        [Column(TypeName = "datetime")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // 4-3-2025 جديد: إضافة تاريخ التحديث (الإلغاء)
        [Column(TypeName = "datetime")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime? UpdatedDate { get; set; } = DateTime.Now;
    }
}
