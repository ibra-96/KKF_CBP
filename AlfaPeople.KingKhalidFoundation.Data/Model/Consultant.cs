namespace AlfaPeople.KingKhalidFoundation.Data.Model
{
    using System;
    using System.ComponentModel;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Consultant
    {
        public Consultant()
        {
            ConsultantAttachments = new HashSet<ConsultantAttachment>();
            Incubations = new HashSet<Incubation>();
            IncubationWorkshops = new HashSet<IncubationWorkshop>();
        }

        public Guid ConsultantID { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(250)]
        public string Address { get; set; }

        [Required]
        [StringLength(50)]
        public string POBox { get; set; }

        [Required]
        [StringLength(50)]
        public string PostalCode { get; set; }

        [Required]
        [StringLength(50)]
        public string OfficialMail { get; set; }

        [Required]
        [StringLength(50)]
        public string MobileNumber { get; set; }

        [DefaultValue(true)]
        public bool IsActive { get; set; }

        [Required]
        [StringLength(50)]
        public string TelephoneNumber { get; set; }

        [Required]
        [StringLength(50)]
        public string Extension { get; set; }

        public virtual ICollection<ConsultantAttachment> ConsultantAttachments { get; set; }

        public virtual ICollection<Incubation> Incubations { get; set; }

        public virtual ICollection<IncubationWorkshop> IncubationWorkshops { get; set; }
    }
}