namespace AlfaPeople.KingKhalidFoundation.Data.Model
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class IncubationAdvertisingModel
    {
        public IncubationAdvertisingModel() { }

        [Key]
        public Guid ID { get; set; }

        public Guid IncubationModelID { get; set; }

        public Guid IncubationAdID { get; set; }

        public virtual IncubationModel IncubationModel { get; set; }

        public virtual IncubationAdvertising IncubationAdvertising { get; set; }
    }
}