using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlfaPeople.KingKhalidFoundation.Data.Model
{
    [Table("FundingSource")]
    public partial class FundingSource
    {
        public FundingSource()
        {
            IncubationWorkshops = new HashSet<IncubationWorkshop>();
            IncubationAdvertisings = new HashSet<IncubationAdvertising>();
        }

        public Guid FundingSourceID { get; set; }

        [Required]
        [StringLength(10)]
        public string Nickname { get; set; }

        [Required]
        [StringLength(50)]
        public string FundingSourceNameAR { get; set; }

        [Required]
        [StringLength(50)]
        public string FundingSourceNameEN { get; set; }

        public string GrantLogoPic { get; set; }

        public string GrantHeaderPic { get; set; }

        public string GrantBackgroundPic { get; set; }

        public string RegistrationBackgroundPic { get; set; }

        [Required]
        [DefaultValue(true)]
        public bool IsActive { get; set; }

        [Required]
        [DefaultValue(false)]
        public bool UseCustomThemes { get; set; }

        [Required]
        [DefaultValue(false)]
        public bool HideKKFLogo { get; set; }

        public virtual ICollection<IncubationAdvertising> IncubationAdvertisings { get; set; }

        public virtual ICollection<IncubationWorkshop> IncubationWorkshops { get; set; }
    }
}