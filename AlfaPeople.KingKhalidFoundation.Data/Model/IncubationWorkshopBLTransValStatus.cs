namespace AlfaPeople.KingKhalidFoundation.Data.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class IncubationWorkshopBLTransValStatus
    {
        public IncubationWorkshopBLTransValStatus()
        {
            this.IncubationWSBLTransactionsValue = new HashSet<IncubationWorkshopBLTransactionsValue>();
        }

        [Key]
        public Guid IncubationWorkshopBLTransValStatusID { get; set; }

        [Required]
        [StringLength(50)]
        public string NameAR { get; set; }

        [Required]
        [StringLength(50)]
        public string NameEN { get; set; }

        public virtual ICollection<IncubationWorkshopBLTransactionsValue> IncubationWSBLTransactionsValue { get; set; }
    }
}