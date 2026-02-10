namespace AlfaPeople.KingKhalidFoundation.Data.Model
{
    using System;
    using System.ComponentModel;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class IncubationModel
    {
        public IncubationModel()
        {
            Incubations = new HashSet<Incubation>();
            IncubationAdvertisingModels = new HashSet<IncubationAdvertisingModel>();
        }

        public Guid IncubationModelID { get; set; }

        public Guid IncubationTypeID { get; set; }

        [Required]
        [StringLength(100)]
        public string NameAR { get; set; }

        [Required]
        [StringLength(100)]
        public string NameEN { get; set; }

        [DefaultValue(true)]
        public bool IsActive { get; set; }

        [StringLength(128)]
        public string FK_AspUserCreateModel { get; set; }

        public virtual AspNetUser AspNetUser { get; set; }

        public virtual IncubationType IncubationType { get; set; }

        public virtual ICollection<Incubation> Incubations { get; set; }

        public virtual ICollection<IncubationAdvertisingModel> IncubationAdvertisingModels { get; set; }
    }
}