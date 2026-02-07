namespace AlfaPeople.KingKhalidFoundation.Data.Model
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class ConsultantAttachment
    {
        [Key]
        public Guid AttachmentID { get; set; }

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

        public Guid ConsultantID { get; set; }

        public virtual Consultant Consultant { get; set; }
    }
}
