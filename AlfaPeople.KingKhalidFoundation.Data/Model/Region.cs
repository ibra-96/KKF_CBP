namespace AlfaPeople.KingKhalidFoundation.Data.Model
{
    using System;
    using System.ComponentModel;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Region
    {
        public Region()
        {
            Governorates = new HashSet<Governorate>();
            IncubationWorkshops = new HashSet<IncubationWorkshop>();
            CorporateApplicationForms = new HashSet<CorporateApplicationForm>();
            IndividualApplicationForms = new HashSet<IndividualApplicationForm>();
        }

        public Guid RegionID { get; set; }

        [Required]
        [StringLength(100)]
        public string RegionNameAR { get; set; }

        [Required]
        [StringLength(100)]
        public string RegionNameEN { get; set; }

        [DefaultValue(true)]
        public bool IsActive { get; set; }

        public virtual ICollection<CorporateApplicationForm> CorporateApplicationForms { get; set; }

        public virtual ICollection<Governorate> Governorates { get; set; }

        public virtual ICollection<IncubationWorkshop> IncubationWorkshops { get; set; }

        public virtual ICollection<IndividualApplicationForm> IndividualApplicationForms { get; set; }
    }
}