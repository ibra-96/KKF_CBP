using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlfaPeople.KingKhalidFoundation.Data.Model
{
    public class WorkshopPrivateInvitation
    {
        [Key]
        public Guid WorkshopPrivateInvitationId { get; set; }

        [Required]
        public string Email { get; set; }

        public InvitationStatus InvitationStatus { get; set; }

        public Guid? FrontendUserID { get; set; }

        public virtual FrontendUser FrontendUser { get; set; }

        public Guid IncubationWorkshopID { get; set; }

        public virtual IncubationWorkshop IncubationWorkshop { get; set; }
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