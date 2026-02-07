using AlfaPeople.KingKhalidFoundation.Data.Model;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlfaPeople.KingKhaledFoundation.Admin.Web.Models
{
    public class IncubationWorkshopControlOptionVM
    {
        public Guid OptionID { get; set; } // معرف الخيار

        [Required(ErrorMessage = "اسم الخيار بالإنجليزية مطلوب.")]
        [StringLength(256, ErrorMessage = "اسم الخيار بالإنجليزية يجب أن لا يتجاوز 256 حرفًا.")]
        public string OptionNameEn { get; set; } // اسم الخيار بالإنجليزية

        [Required(ErrorMessage = "اسم الخيار بالعربية مطلوب.")]
        [StringLength(256, ErrorMessage = "اسم الخيار بالعربية يجب أن لا يتجاوز 256 حرفًا.")]
        public string OptionNameAr { get; set; } // اسم الخيار بالعربية

        // تحديد معرف الحقل الديناميكي إذا كان مطلوبًا للعرض في الواجهة
        [ForeignKey("IncubationWorkshopBLTransactions")]
        public Guid TransID { get; set; } 

        // إذا كنت تحتاج لعرض اسم الحقل الديناميكي المرتبط بهذا الخيار
        //public string TransactionName { get; set; }

        // إذا كنت تحتاج للتحقق من حالة الخيار (مثلاً تم اختياره أو لا)
        //public bool IsSelected { get; set; }
    }
}
