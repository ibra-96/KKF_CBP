namespace AlfaPeople.KingKhalidFoundation.Data.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class ApplicantStatu
    {
        public ApplicantStatu()
        {
            CorporateApplicationStatus = new HashSet<CorporateApplicationStatu>();
            IndividualApplicantStatus = new HashSet<IndividualApplicantStatu>();
        }

        [Key]
        public Guid ApplicantStatusID { get; set; }

        [Required]
        [StringLength(100)]
        public string ApplicantStatusName { get; set; }

        public virtual ICollection<CorporateApplicationStatu> CorporateApplicationStatus { get; set; }

        public virtual ICollection<IndividualApplicantStatu> IndividualApplicantStatus { get; set; }
    }
}