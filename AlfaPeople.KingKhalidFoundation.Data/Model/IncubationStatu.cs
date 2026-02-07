namespace AlfaPeople.KingKhalidFoundation.Data.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class IncubationStatus
    {
        public IncubationStatus()
        {
            Incubations = new HashSet<Incubation>();
        }

        [Key]
        public Guid IncubationStatusID { get; set; }

        [Required]
        [StringLength(50)]
        public string NameAR { get; set; }

        [Required]
        [StringLength(50)]
        public string NameEN { get; set; }

        public virtual ICollection<Incubation> Incubations { get; set; }
    }
}