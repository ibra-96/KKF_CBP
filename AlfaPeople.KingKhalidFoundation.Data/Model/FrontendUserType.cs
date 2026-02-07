namespace AlfaPeople.KingKhalidFoundation.Data.Model
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class FrontendUserType
    {
        [Key]
        public Guid UserTypeID { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }
    }
}