namespace AlfaPeople.KingKhalidFoundation.Data.Model
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class EducationalQualification
    {
        [Key]
        public Guid EducationalQualificationsID { get; set; }

        public Guid IndividualApplicationFormID { get; set; }

        [Required]
        [StringLength(100)]
        public string TrainingName { get; set; }

        [Required]
        [StringLength(100)]
        public string Organisers { get; set; }

        [Required]
        [StringLength(100)]
        public string Representation { get; set; }

        [Column(TypeName = "date")]
        public DateTime RepresentationDate { get; set; }

        public virtual IndividualApplicationForm IndividualApplicationForm { get; set; }
    }
}