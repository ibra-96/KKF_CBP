namespace AlfaPeople.KingKhalidFoundation.Data.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class BackendGroup
    {
        public BackendGroup()
        {
            BackendRoles = new HashSet<BackendRole>();
        }

        [Key]
        public Guid GroupID { get; set; }

        [Required]
        [StringLength(50)]
        public string NameAR { get; set; }

        [Required]
        [StringLength(50)]
        public string NameEN { get; set; }

        public virtual ICollection<BackendRole> BackendRoles { get; set; }
    }
}