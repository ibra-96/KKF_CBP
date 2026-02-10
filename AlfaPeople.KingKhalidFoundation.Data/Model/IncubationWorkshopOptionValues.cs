using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlfaPeople.KingKhalidFoundation.Data.Model
{
    public class IncubationWorkshopOptionValues
    {
        [Key]
        public Guid ValueID { get; set; }  

        [Required]
        public Guid OptionID { get; set; }  // ربط القيمة بالخيار المحدد
        [ForeignKey(nameof(OptionID))]
        public virtual IncubationWorkshopControlOptions Option { get; set; }


        public Guid? TransID { get; set; }                                  // الربط مع TransactionsValue

        public Guid TransValueID { get; set; }
        [ForeignKey("TransValueID")]
        public virtual IncubationWorkshopBLTransactionsValue TransactionsValue { get; set; }
        [Required]
        public string Value { get; set; }  
    }
}
