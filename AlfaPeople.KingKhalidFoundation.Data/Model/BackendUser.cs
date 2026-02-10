namespace AlfaPeople.KingKhalidFoundation.Data.Model
{
    using System;
    using System.ComponentModel;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class BackendUser
    {
        public BackendUser()
        {
            CorporateApplicationStatus = new HashSet<CorporateApplicationStatu>();
        }

        public Guid BackendUserID { get; set; }

        [Required]
        [StringLength(256)]
        public string UserName { get; set; }

        [ForeignKey("AspNetUser")]
        public string FK_AspUser { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [ForeignKey("BackendUserPositions")]
        public Guid BackEndPositionId { get; set; }

        [DefaultValue(true)]
        public bool IsActive { get; set; }

        public DateTime CreateDate { get; set; }

        public virtual AspNetUser AspNetUser { get; set; }

        public virtual ICollection<CorporateApplicationStatu> CorporateApplicationStatus { get; set; }

        public virtual BackendUserPosition BackendUserPositions { get; set; }

    }
}