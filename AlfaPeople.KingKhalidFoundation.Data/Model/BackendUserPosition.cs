namespace AlfaPeople.KingKhalidFoundation.Data.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class BackendUserPosition
    {
        public BackendUserPosition()
        {
            BackendUsers = new HashSet<BackendUser>();
        }

        public Guid BackendUserPositionID { get; set; }

        [Required]
        [StringLength(50)]
        public string NameAR { get; set; }

        [Required]
        public bool IsActive { get; set; }
        [Required]
        [StringLength(50)]
        public string NameEN { get; set; }

        public virtual ICollection<BackendUser> BackendUsers { get; set; }
    }
}