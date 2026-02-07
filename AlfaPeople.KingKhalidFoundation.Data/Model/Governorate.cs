namespace AlfaPeople.KingKhalidFoundation.Data.Model
{
    using System;
    using System.ComponentModel;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Governorate
    {
        public Governorate()
        {
            Cities = new HashSet<City>();
            IncubationWorkshops = new HashSet<IncubationWorkshop>();
            CorporateApplicationForms = new HashSet<CorporateApplicationForm>();
            IndividualApplicationForms = new HashSet<IndividualApplicationForm>();
        }

        public Guid GovernorateID { get; set; }

        public Guid RegionID { get; set; }

        [Required]
        [StringLength(100)]
        public string GovernorateAR { get; set; }

        [Required]
        [StringLength(100)]
        public string GovernorateEN { get; set; }

        [DefaultValue(true)]
        public bool IsActive { get; set; }

        public virtual ICollection<City> Cities { get; set; }

        public virtual ICollection<CorporateApplicationForm> CorporateApplicationForms { get; set; }

        public virtual Region Region { get; set; }

        public virtual ICollection<IncubationWorkshop> IncubationWorkshops { get; set; }

        public virtual ICollection<IndividualApplicationForm> IndividualApplicationForms { get; set; }
    }
}