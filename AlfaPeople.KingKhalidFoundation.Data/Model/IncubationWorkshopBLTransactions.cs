namespace AlfaPeople.KingKhalidFoundation.Data.Model
{
    using System;
    using System.ComponentModel;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
   
    public partial class IncubationWorkshopBLTransactions
    {
        public IncubationWorkshopBLTransactions()
        {
            this.IncubationWSBLTransactionsValue = new HashSet<IncubationWorkshopBLTransactionsValue>();
            //shadia
           
            this.Options = new HashSet<IncubationWorkshopControlOptions>(); // الخيارات المرتبطة بالحقل
        }

        [Key]
        public Guid TransID { get; set; }

        [Required]
        [StringLength(256)]
        public string FieldNameEn { get; set; }

        [Required]
        [StringLength(256)]
        public string FieldNameAr { get; set; }

        [Required]
        [DefaultValue(true)]
        public string IsRequired { get; set; }

        [Required]
        [DefaultValue(true)]
        public string ViewList_Display { get; set; }

        [Required]
        public int RankNumber { get; set; }

        public virtual IncubationWorkshopControlsType IncubationWSControlsType { get; set; }

        public Guid TransTypeID { get; set; }
        public virtual IncubationWorkshopBLTransactionsType IncubationWorkshopBLTransType { get; set; }
        public virtual ICollection<IncubationWorkshopBLTransactionsValue> IncubationWSBLTransactionsValue { get; set; }
        //shadia
        [Required]
        [ForeignKey("IncubationWorkshop")]
        public Guid IncubationWorkshopID { get; set; } // ربط الحقول بورشة العمل
        public virtual IncubationWorkshop IncubationWorkshop { get; set; } // العلاقة مع ورشة العمل

        public Guid ControlTypeID { get; set; }
        public virtual IncubationWorkshopControls IncubationWorkshopControls { get; set; }


        // العلاقة مع الخيارات
        public virtual ICollection<IncubationWorkshopControlOptions> Options { get; set; }

    
    }
}