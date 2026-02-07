namespace AlfaPeople.KingKhalidFoundation.Data.Model
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class IndividualApplicantStatu
    {
        [Key]
        public Guid IndividualApplicationFormID { get; set; }

        public Guid ApplicantStatusID { get; set; }

        public string FeadBack { get; set; }

        public virtual ApplicantStatu ApplicantStatu { get; set; }

        public virtual IndividualApplicationForm IndividualApplicationForm { get; set; }
    }
}