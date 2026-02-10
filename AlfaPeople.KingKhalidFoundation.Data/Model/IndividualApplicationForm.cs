namespace AlfaPeople.KingKhalidFoundation.Data.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("IndividualApplicationForm")]
    public partial class IndividualApplicationForm
    {
        public IndividualApplicationForm()
        {
            EducationalQualifications = new HashSet<EducationalQualification>();
            IndividualApplicationFormAttachments = new HashSet<IndividualApplicationFormAttachment>();
            TrainingCourses = new HashSet<TrainingCours>();
        }
        //26-2-2025
        // إضافة حالة المسودة
        [Required]
        public bool IsDraft { get; set; } = true; // القيمة الافتراضية تعني أن النموذج مسودة
        public Guid IndividualApplicationFormID { get; set; }

        [Required]
        [StringLength(150)]
        public string Name { get; set; }

        [Required]
        [Column(TypeName = "date")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime BirthDate { get; set; }

        [Required]
        public bool Gender { get; set; }

        [Required]
        [StringLength(50)]
        public string IdentityNumber { get; set; }

        [StringLength(50)]
        public string CompanyName { get; set; }

        [Required]
        [StringLength(50)]
        public string MobileNumber { get; set; }

        [Required]
        [StringLength(50)]
        public string TelephoneNumber { get; set; }

        [Required]
        [StringLength(50)]
        public string Extension { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        [StringLength(50)]
        public string PostalCode { get; set; }

        [Required]
        [StringLength(50)]
        public string POBox { get; set; }

        [Column(TypeName = "date")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime WorkStartDate { get; set; }

        [StringLength(50)]
        public string Position { get; set; }

        public string PositionDetails { get; set; }

        [Required]
        public Guid Nationality { get; set; }

        [Required]
        public Guid RegionID { get; set; }

        [Required]
        public Guid GovernorateID { get; set; }

        [Display(Name = "Neighborhood")]
        public Guid? CityID { get; set; }

        [Required]
        public Guid ProgramID { get; set; }

        public Guid FrontendUserID { get; set; }

        [Column(TypeName = "image")]
        public byte[] Picture { get; set; }

        public virtual City City { get; set; }

        public virtual CountriesAndNationality CountriesAndNationality { get; set; }

        public virtual ICollection<EducationalQualification> EducationalQualifications { get; set; }

        public virtual FrontendUser FrontendUser { get; set; }

        public virtual Governorate Governorate { get; set; }

        public virtual IndividualApplicantStatu IndividualApplicantStatu { get; set; }

        public virtual Program Program { get; set; }

        public virtual Region Region { get; set; }

        public virtual ICollection<IndividualApplicationFormAttachment> IndividualApplicationFormAttachments { get; set; }

        public virtual ICollection<TrainingCours> TrainingCourses { get; set; }
    }
}