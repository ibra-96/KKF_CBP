namespace AlfaPeople.KingKhalidFoundation.Data.Model
{
    using System;
    using System.ComponentModel;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class IncubationWorkshopBLTransactionsValue
    {
        public IncubationWorkshopBLTransactionsValue()
        {
            this.IncubationWSBLTransValueAttachment = new HashSet<IncubationWorkshopBLTransValueAttachment>();
            this.OptionValues = new HashSet<IncubationWorkshopOptionValues>();
        }

        [Key]
        public Guid TransValueID { get; set; }

        [Required]
        public string Value { get; set; } 

        [Required]
        public Guid FrontendUserID { get; set; }
        public virtual FrontendUser FrontendUser { get; set; }

        [Required]
        public Guid IncubationWorkshopBLTransValStatusID { get; set; }
        public virtual IncubationWorkshopBLTransValStatus IncubationWorkshopBLTransValStatus { get; set; }

        public Guid TransID { get; set; }
        public virtual IncubationWorkshopBLTransactions IncubationWorkshopBLTrans { get; set; }

        [Column(TypeName = "date")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime? SubmissionDate { get; set; }

        public string Feadback { get; set; }

        public virtual ICollection<IncubationWorkshopBLTransValueAttachment> IncubationWSBLTransValueAttachment { get; set; }
        public virtual ICollection<IncubationWorkshopOptionValues> OptionValues { get; set; }
    }
}
