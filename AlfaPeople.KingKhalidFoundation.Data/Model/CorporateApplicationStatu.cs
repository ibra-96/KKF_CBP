namespace AlfaPeople.KingKhalidFoundation.Data.Model
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class CorporateApplicationStatu
    {
        [Key]
        public Guid CorporateApplicationFormID { get; set; }

        public Guid ApplicantStatusID { get; set; }

        [ForeignKey("ReasonType")]

        public Guid? ResonTypeID { get; set; }

        public string FeadBack { get; set; }

        [ForeignKey("BackEndUser")]

        public Guid? Fk_BackEndMakeAction { get; set; }

        public virtual BackendUser BackEndUser { get; set; }

        public DateTime? DateTimeMakeAction { get; set; }
        public virtual ApplicantStatu ApplicantStatu { get; set; }

        public virtual ReasonType ReasonType { get; set; }

        public virtual CorporateApplicationForm CorporateApplicationForm { get; set; }
    }
}