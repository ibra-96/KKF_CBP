namespace AlfaPeople.KingKhalidFoundation.Data.Model
{
    using System;
    using System.ComponentModel;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class IncubationWorkshopModel
    {
        public IncubationWorkshopModel()
        {
            IncubationWorkshops = new HashSet<IncubationWorkshop>();
        }

        [Key]
        public Guid IncubationWorkshopModeID { get; set; }

        [Required]
        [StringLength(50)]
        public string NameAR { get; set; }

        [Required]
        [StringLength(50)]
        public string NameEN { get; set; }

        [DefaultValue(true)]
        public bool IsActive { get; set; }

        [ForeignKey("AspNetUser")]
        public string FK_AspUserCreateModel { get; set; }

        public virtual AspNetUser AspNetUser { get; set; }

        public virtual ICollection<IncubationWorkshop> IncubationWorkshops { get; set; }
    }
}