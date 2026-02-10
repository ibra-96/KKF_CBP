namespace AlfaPeople.KingKhalidFoundation.Data.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class IncubationBaselineStatus
    {
        public IncubationBaselineStatus()
        {
            IncubationBaselines = new HashSet<IncubationBaseline>();
        }

        [Key]
        public Guid IncubationBaselineStatusID { get; set; }

        [Required]
        [StringLength(50)]
        public string NameAR { get; set; }

        [Required]
        [StringLength(50)]
        public string NameEN { get; set; }

        public virtual ICollection<IncubationBaseline> IncubationBaselines { get; set; }
    }
}