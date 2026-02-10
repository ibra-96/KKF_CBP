namespace AlfaPeople.KingKhalidFoundation.Data.Model
{
    using System;
    using System.ComponentModel;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ClassificationSector")]
    public partial class ClassificationSector
    {
        public ClassificationSector()
        {
            CorporateApplicationForms = new HashSet<CorporateApplicationForm>();
        }

        public Guid ClassificationSectorID { get; set; }

        public Guid CorporationsCategoryID { get; set; }

        [Required]
        [StringLength(50)]
        public string ClassificationSectorNameAR { get; set; }

        [Required]
        [StringLength(50)]
        public string ClassificationSectorNameEN { get; set; }

        [DefaultValue(true)]
        public bool IsActive { get; set; }

        public virtual CorporationsCategory CorporationsCategory { get; set; }

        public virtual ICollection<CorporateApplicationForm> CorporateApplicationForms { get; set; }
    }
}