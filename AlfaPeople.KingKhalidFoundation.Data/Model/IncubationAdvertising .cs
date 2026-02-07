namespace AlfaPeople.KingKhalidFoundation.Data.Model
{
    using System;
    using System.ComponentModel;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class IncubationAdvertising
    {
        public IncubationAdvertising()
        {
            Incubations = new HashSet<Incubation>();
            IncubationAdvertisingAttachments = new HashSet<IncubationAdvertisingAttachment>();
            IncubationAdvertisingModels = new HashSet<IncubationAdvertisingModel>();
            IncubationPrivateInvitations = new HashSet<IncubationPrivateInvitation>();
            IncubationProjectProposals = new HashSet<IncubationProjectProposal>();
        }

        [Key]
        public Guid IncubationAdID { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Column(TypeName = "date")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime StartDate { get; set; }

        [Column(TypeName = "date")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime EndDate { get; set; }

        [Required]
        public string AdvertisingDetails { get; set; }

        [Required]
        public string EntryRequirements { get; set; }

        public Guid IncubationTypeID { get; set; }

        [Required]
        [DefaultValue(true)]
        public bool IsActive { get; set; }

        [Required]
        [DefaultValue(true)]
        public bool ISPublic { get; set; }


        // إضافة خاصية الحذف 2-25-2025

        [Required]
        [DefaultValue(false)]
        public bool IsDeleted { get; set; } = false;

        public DateTime? DeletedDate { get; set; }
        public Guid? DeletedBy { get; set; }
        //end
        public Guid FundingSourceID { get; set; }

        public virtual FundingSource FundingSource { get; set; }

        public virtual IncubationType IncubationType { get; set; }

        public virtual ICollection<Incubation> Incubations { get; set; }

        public virtual ICollection<IncubationAdvertisingModel> IncubationAdvertisingModels { get; set; }

        public virtual ICollection<IncubationAdvertisingAttachment> IncubationAdvertisingAttachments { get; set; }

        public virtual ICollection<IncubationPrivateInvitation> IncubationPrivateInvitations { get; set; }

        public virtual ICollection<IncubationProjectProposal> IncubationProjectProposals { get; set; }
      

    }
}