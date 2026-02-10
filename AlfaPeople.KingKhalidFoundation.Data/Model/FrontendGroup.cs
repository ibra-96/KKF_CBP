namespace AlfaPeople.KingKhalidFoundation.Data.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class FrontendGroup
    {
        public FrontendGroup()
        {
            FrontendRoles = new HashSet<FrontendRole>();
            FrontendUsers = new HashSet<FrontendUser>();
        }

        [Key]
        public Guid GroupID { get; set; }

        [Required]
        [StringLength(50)]
        public string NameAR { get; set; }

        [Required]
        [StringLength(50)]
        public string NameEN { get; set; }

        public virtual ICollection<FrontendRole> FrontendRoles { get; set; }

        public virtual ICollection<FrontendUser> FrontendUsers { get; set; }
    }
}