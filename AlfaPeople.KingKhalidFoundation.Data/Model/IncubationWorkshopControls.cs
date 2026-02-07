namespace AlfaPeople.KingKhalidFoundation.Data.Model
{
    using System;
    using System.ComponentModel;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class IncubationWorkshopControls
    {
        public IncubationWorkshopControls()
        {
            this.IncubationWorkshopControlsTypes = new HashSet<IncubationWorkshopControlsType>();
        }

        [Key]
        public Guid ControlsID { get; set; }

        [Required]
        [StringLength(50)]
        public string ControlsName { get; set; }

        public virtual ICollection<IncubationWorkshopControlsType> IncubationWorkshopControlsTypes { get; set; }
    }
}