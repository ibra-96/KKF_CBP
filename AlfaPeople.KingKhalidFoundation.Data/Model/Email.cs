namespace AlfaPeople.KingKhalidFoundation.Data.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Email")]
    public partial class Email
    {
        public Email()
        {
            EmailAttachments = new HashSet<EmailAttachment>();
        }

        public Guid EmailID { get; set; }

        [Required]
        [StringLength(50)]
        public string Subject { get; set; }

        [Required]
        public string From { get; set; }

        [Required]
        public string To { get; set; }

        [Required]
        public string CC { get; set; }

        [Required]
        public string BC { get; set; }

        public string Content { get; set; }

        public virtual ICollection<EmailAttachment> EmailAttachments { get; set; }
    }
}