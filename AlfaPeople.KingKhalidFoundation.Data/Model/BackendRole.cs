namespace AlfaPeople.KingKhalidFoundation.Data.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class BackendRole
    {
        public BackendRole()
        {
            BackendGroups = new HashSet<BackendGroup>();
        }

        [Key]
        public Guid RoleID { get; set; }

        [Required]
        [StringLength(50)]
        public string NameAR { get; set; }

        [Required]
        [StringLength(50)]
        public string NameEN { get; set; }

        public bool IsCreate { get; set; }

        public bool IsRead { get; set; }

        public bool IsUpdate { get; set; }

        public bool IsDelete { get; set; }

        public virtual ICollection<BackendGroup> BackendGroups { get; set; }
    }
}