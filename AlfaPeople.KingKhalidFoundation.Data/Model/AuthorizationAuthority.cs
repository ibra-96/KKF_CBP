namespace AlfaPeople.KingKhalidFoundation.Data.Model
{
    using System;
    using System.ComponentModel;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("AuthorizationAuthority")]
    public partial class AuthorizationAuthority
    {
        public AuthorizationAuthority()
        {
            CorporateApplicationForms = new HashSet<CorporateApplicationForm>();
        }

        public Guid AuthorizationAuthorityID { get; set; }

        [Required]
        [StringLength(50)]
        public string AuthorizationAuthorityNameAR { get; set; }

        [Required]
        [StringLength(50)]
        public string AuthorizationAuthorityNameEN { get; set; }

        [DefaultValue(true)]
        public bool IsActive { get; set; }

        public virtual ICollection<CorporateApplicationForm> CorporateApplicationForms { get; set; }
    }
}