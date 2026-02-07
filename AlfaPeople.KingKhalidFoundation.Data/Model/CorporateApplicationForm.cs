namespace AlfaPeople.KingKhalidFoundation.Data.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("CorporateApplicationForm")]
    public partial class CorporateApplicationForm
    {
        public CorporateApplicationForm()
        {
            CorporateApplicationFormAttachments = new HashSet<CorporateApplicationFormAttachment>();
        }
        //26-2-2025
        // إضافة حالة المسودة
        [Required]
        public bool IsDraft { get; set; } = false; // القيمة الافتراضية تعني أن النموذج مسودة
        public Guid CorporateApplicationFormID { get; set; }

        //[Required]
        [StringLength(150)]
        [Display(Name = "CorporationName", ResourceType = typeof(Resources.CorporationRegistered))]
        public string Name { get; set; }

        [Column(TypeName = "date")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        [Display(Name = "FoundedYear", ResourceType = typeof(Resources.CorporationRegistered))]
        public DateTime? FoundedYear { get; set; }

        //[Required]
        [StringLength(20)]
        [Display(Name = "CorporationRegitrationNumber", ResourceType = typeof(Resources.CorporationRegistered))]
        public string RegistrationNumber { get; set; }

        [Display(Name = "TaxNumber", ResourceType = typeof(Resources.CorporationRegistered))]
        public string TaxNumber { get; set; }

        //[Required]
        [Display(Name = "ABriefHistoryOftheCorporationAssociation", ResourceType = typeof(Resources.CorporationRegistered))]
        public string History { get; set; }

        //[Required]
        [Display(Name = "Vision", ResourceType = typeof(Resources.CorporationRegistered))]
        public string Vision { get; set; }

        //[Required]
        [Display(Name = "Mission", ResourceType = typeof(Resources.CorporationRegistered))]
        public string Mission { get; set; }

        //[Required]
        [Display(Name = "Objectives", ResourceType = typeof(Resources.CorporationRegistered))]
        public string Objectives { get; set; }

        //[Required]
        [Display(Name = "Address", ResourceType = typeof(Resources.CorporationProfile))]
        public string Address { get; set; }

        //[Required]
        [StringLength(50)]
        [Display(Name = "PostalCode", ResourceType = typeof(Resources.CorporationProfile))]
        public string PostalCode { get; set; }

        [StringLength(50)]
        [Display(Name = "POBox", ResourceType = typeof(Resources.CorporationProfile))]
        public string POBox { get; set; }

        [StringLength(50)]
        [Display(Name = "Extension", ResourceType = typeof(Resources.CorporationProfile))]
        public string Extension { get; set; }

        //[Required]
        [StringLength(50)]
        [Display(Name = "TelephoneNumber", ResourceType = typeof(Resources.CorporationProfile))]
        public string TelephoneNumber { get; set; }

        [StringLength(50)]
        [Display(Name = "FaxNumber", ResourceType = typeof(Resources.CorporationProfile))]
        public string FaxNumber { get; set; }

        //[Required]
        [StringLength(50)]
        [Display(Name = "OficialEmail", ResourceType = typeof(Resources.CorporationProfile))]
        public string OfficialEmail { get; set; }

        //[Required]
        [EmailAddress]
        [StringLength(50)]
        [Display(Name = "AdministratorEmail", ResourceType = typeof(Resources.CorporationProfile))]
        public string AdministratorEmail { get; set; }

        [StringLength(50)]
        [Display(Name = "Website", ResourceType = typeof(Resources.CorporationProfile))]
        public string Website { get; set; }

        [StringLength(50)]
        [Display(Name = "TwitterAccount", ResourceType = typeof(Resources.CorporationProfile))]
        public string TwitterAccount { get; set; }

        [StringLength(50)]
        [Display(Name = "YouTubeAccount", ResourceType = typeof(Resources.CorporationProfile))]
        public string YouTubeAccount { get; set; }

        [StringLength(50)]
        [Display(Name = "SnapchatAccount", ResourceType = typeof(Resources.CorporationProfile))]
        public string SnapchatAccount { get; set; }

        [StringLength(50)]
        [Display(Name = "InstagramAccount", ResourceType = typeof(Resources.CorporationProfile))]
        public string InstagramAccount { get; set; }

        //[Required]
        [StringLength(50)]
        [Display(Name = "NameOfAdministratorCorporation", ResourceType = typeof(Resources.CorporationRegistered))]
        public string CorporateAdministratorName { get; set; }

        //[Required]
        [StringLength(50)]
        [Display(Name = "JobTitle", ResourceType = typeof(Resources.CorporationProfile))]
        public string CorporateAdministratorJobTitle { get; set; }

        //[Required]
        [StringLength(50, MinimumLength = 10)]
        [Range(100000000, 1000000000000, ErrorMessageResourceName = "CorpAdminMobileNumMsg", ErrorMessageResourceType = typeof(Resources.CorporationProfile))]
        [Display(Name = "Mobile", ResourceType = typeof(Resources.CorporationProfile))]
        public string CorporateAdministratorMobileNumber { get; set; }

        //[Required]
        [StringLength(50)]
        [Display(Name = "TelephoneNumber", ResourceType = typeof(Resources.CorporationProfile))]
        public string CorporateAdministratorTelephoneNumber { get; set; }

        //[Required]
        [StringLength(50)]
        [Display(Name = "Extension", ResourceType = typeof(Resources.CorporationProfile))]
        public string CorporateAdministratorExtension { get; set; }

        //[Required]
        [Display(Name = "Sector", ResourceType = typeof(Resources.CorporationRegistered))]
        public Guid? CorporationsCategoryID { get; set; }

        [Display(Name = "FieldOfWork", ResourceType = typeof(Resources.CorporationRegistered))]
        public Guid? CorporateFieldOfWorkID { get; set; }

        [Display(Name = "NonProfitSectorType", ResourceType = typeof(Resources.CorporationRegistered))]
        public Guid? ClassificationSectorID { get; set; }

        //[Required]
        [Display(Name = "AuthorizationAuthority", ResourceType = typeof(Resources.CorporationRegistered))]
        public Guid? AuthorizationAuthorityID { get; set; }

        //[Required]
        [Display(Name = "WhyWouldYouLikeToJoinUs", ResourceType = typeof(Resources.CorporationProfile))]
        public Guid ProgramID { get; set; }

        //[Required]
      
        [Display(Name = "Region", ResourceType = typeof(Resources.CorporationProfile))]
        public Guid? RegionID { get; set; }

        //[Required]
        [Display(Name = "Governorate", ResourceType = typeof(Resources.CorporationProfile))]
        public Guid? GovernorateID { get; set; }

        [Display(Name = "Neighborhoods", ResourceType = typeof(Resources.CorporationProfile))]
        public Guid? CityID { get; set; }

        public Guid FrontendUserID { get; set; }

        [Column(TypeName = "date")]
        [Display(Name = "DateElection", ResourceType = typeof(Resources.CorporationRegistered))]
        public DateTime? DateElection { get; set; }

        [Column(TypeName = "image")]
        public byte[] Picture { get; set; }

        [Display(Name = "GenderTypeOnTheOrganization", ResourceType = typeof(Resources.CorporationRegistered))]
        public CorporateGenderType? corporateGenderType { get; set; }

        public virtual AuthorizationAuthority AuthorizationAuthority { get; set; }

        public virtual City City { get; set; }

        public virtual ClassificationSector ClassificationSector { get; set; }

        public virtual CorporationsCategory CorporationsCategory { get; set; }

        public virtual CorporateFieldOfWork CorporateFieldOfWork { get; set; }

        public virtual FrontendUser FrontendUser { get; set; }

        public virtual Governorate Governorate { get; set; }

        public virtual Program Program { get; set; }

        public virtual Region Region { get; set; }

        public virtual CorporateApplicationStatu CorporateApplicationStatu { get; set; }

        public virtual ICollection<CorporateApplicationFormAttachment> CorporateApplicationFormAttachments { get; set; }
    }
}