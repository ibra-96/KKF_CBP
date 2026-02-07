using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlfaPeople.KingKhalidFoundation.Data.Model
{
    [Table("CorporateFieldOfWork")]
    public partial class CorporateFieldOfWork
    {
        public CorporateFieldOfWork()
        {
            CorporateApplicationForms = new HashSet<CorporateApplicationForm>();
        }

        public Guid CorporateFieldOfWorkID { get; set; }

        [Required]
        [StringLength(50)]
        public string CorporateFieldOfWorkNameAR { get; set; }

        [Required]
        [StringLength(50)]
        public string CorporateFieldOfWorkNameEN { get; set; }

        [Required]
        [DefaultValue(true)]
        public bool IsActive { get; set; }

        public virtual ICollection<CorporateApplicationForm> CorporateApplicationForms { get; set; }
    }
}