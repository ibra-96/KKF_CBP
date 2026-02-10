namespace AlfaPeople.KingKhalidFoundation.Data.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class IncubationtWorkshopStatu
    {
        public IncubationtWorkshopStatu()
        {
            IncubationWorkshops = new HashSet<IncubationWorkshop>();
        }

        [Key]
        public Guid IncubationtWorkshopStatusID { get; set; }

        [Required]
        [StringLength(50)]
        public string NameAR { get; set; }

        [Required]
        [StringLength(50)]
        public string NameEN { get; set; }

        public virtual ICollection<IncubationWorkshop> IncubationWorkshops { get; set; }
    }
}