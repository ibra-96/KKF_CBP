namespace AlfaPeople.KingKhalidFoundation.Data.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class CountriesAndNationality
    {
        public CountriesAndNationality()
        {
            IndividualApplicationForms = new HashSet<IndividualApplicationForm>();
        }

        [Key]
        public Guid CountriesAndNationalitiesID { get; set; }

        [Required]
        [StringLength(2)]
        public string Abbreviation { get; set; }

        [Required]
        [StringLength(50)]
        public string NameEN { get; set; }

        [Required]
        [StringLength(50)]
        public string NameAR { get; set; }

        [Required]
        [StringLength(50)]
        public string NationalityEN { get; set; }

        [Required]
        [StringLength(50)]
        public string NationalityAR { get; set; }

        public virtual ICollection<IndividualApplicationForm> IndividualApplicationForms { get; set; }
    }
}