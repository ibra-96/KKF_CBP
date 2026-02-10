using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlfaPeople.KingKhalidFoundation.Data.Model
{
   public class IncubationWorkshopControlOptions
    {
        [Key]
        public Guid OptionID { get; set; } // معرف الخيار

        [Required]
     
        public string OptionNameEn { get; set; } // اسم الخيار بالإنجليزية

        [Required]
    
        public string OptionNameAr { get; set; } // اسم الخيار بالعربية
       
        //[Required]
        //public Guid ControlTypeID { get; set; } // ربط الخيار بنوع التحكم
        //public virtual IncubationWorkshopControlsType IncubationWorkshopControlsType { get; set; } // العلاقة مع نوع التحكم
        [Required]
        public Guid TransID { get; set; } // ربط الخيار بالحقل الديناميكي
        public virtual IncubationWorkshopBLTransactions IncubationWorkshopBLTrans { get; set; } // العلاقة مع الحقل الديناميكي
        public virtual ICollection<IncubationWorkshopOptionValues> OptionValues { get; set; }

    }
}
