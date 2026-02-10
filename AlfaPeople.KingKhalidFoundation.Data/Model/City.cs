namespace AlfaPeople.KingKhalidFoundation.Data.Model
{
    using System;
    using System.ComponentModel;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class City
    {
        public City()
        {
            IncubationWorkshops = new HashSet<IncubationWorkshop>();
            CorporateApplicationForms = new HashSet<CorporateApplicationForm>();
            IndividualApplicationForms = new HashSet<IndividualApplicationForm>();
        }

        public Guid CityID { get; set; }

        public Guid GovernorateID { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "NeighborhoodsAR")]

        public string CityNameAR { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "NeighborhoodsEN")]
        public string CityNameEN { get; set; }

        public virtual Governorate Governorate { get; set; }

        [DefaultValue(true)]
        public bool IsActive { get; set; }

        public virtual ICollection<CorporateApplicationForm> CorporateApplicationForms { get; set; }
        public virtual ICollection<IncubationWorkshop> IncubationWorkshops { get; set; }

        public virtual ICollection<IndividualApplicationForm> IndividualApplicationForms { get; set; }
    }
}