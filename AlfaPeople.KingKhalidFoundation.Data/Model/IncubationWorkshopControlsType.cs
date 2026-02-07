namespace AlfaPeople.KingKhalidFoundation.Data.Model
{
    using System;
    using System.ComponentModel;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class IncubationWorkshopControlsType
    {
        public IncubationWorkshopControlsType()
        {
            this.IncubationWorkshopBLTrans = new HashSet<IncubationWorkshopBLTransactions>();
        }

        [Key]
        public Guid ControlTypeID { get; set; }

        [Required]
        [StringLength(50)]
        public string ControlTypeName { get; set; }

        public Guid ControlsID { get; set; }
        public virtual IncubationWorkshopControls IncubationWorkshopControl { get; set; }
        public virtual ICollection<IncubationWorkshopBLTransactions> IncubationWorkshopBLTrans { get; set; }
    }
}