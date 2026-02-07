namespace AlfaPeople.KingKhalidFoundation.Data.Model
{
    using System;
    using System.ComponentModel;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class IncubationWorkshopBLTransValueAttachment
    {
        [Key]
        public Guid TransAttachmentID { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(150)]
        public string ScreenName { get; set; }

        [Required]
        [StringLength(150)]
        public string Type { get; set; }

        [Required]
        [StringLength(50)]
        public string Size { get; set; }

        [Required]
        [StringLength(250)]
        public string URL { get; set; }

        public Guid TransValueID { get; set; }

        public virtual IncubationWorkshopBLTransactionsValue IncubationWSBLTransValue { get; set; }
    }
}