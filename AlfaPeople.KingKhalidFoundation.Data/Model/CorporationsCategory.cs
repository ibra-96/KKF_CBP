namespace AlfaPeople.KingKhalidFoundation.Data.Model
{
    using System;
    using System.ComponentModel;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("CorporationsCategory")]
    public partial class CorporationsCategory
    {
        public CorporationsCategory()
        {
            ClassificationSectors = new HashSet<ClassificationSector>();
            CorporateApplicationForms = new HashSet<CorporateApplicationForm>();
        }

        public Guid CorporationsCategoryID { get; set; }

        [Required]
        [StringLength(50)]
        public string CorporationsCategoryNameAR { get; set; }

        [Required]
        [StringLength(50)]
        public string CorporationsCategoryNameEN { get; set; }

        [Required]
        [DefaultValue(true)]
        public bool IsActive { get; set; }

        public virtual ICollection<ClassificationSector> ClassificationSectors { get; set; }

        public virtual ICollection<CorporateApplicationForm> CorporateApplicationForms { get; set; }
    }
}