namespace AlfaPeople.KingKhalidFoundation.Data.Model
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("TrainingCourses")]
    public partial class TrainingCours
    {
        [Key]
        public Guid TrainingCoursesID { get; set; }

        public Guid IndividualApplicationFormID { get; set; }

        [Required]
        [StringLength(100)]
        public string Qualification { get; set; }

        [Required]
        [StringLength(100)]
        public string Specialization { get; set; }

        [Column(TypeName = "date")]
        public DateTime Date { get; set; }

        [Required]
        [StringLength(150)]
        public string IssuedFrom { get; set; }

        public virtual IndividualApplicationForm IndividualApplicationForm { get; set; }
    }
}