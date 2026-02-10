namespace AlfaPeople.KingKhalidFoundation.Data.Model
{
    using System;
    using System.ComponentModel;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Program
    {
        public Program()
        {
            CorporateApplicationForms = new HashSet<CorporateApplicationForm>();
            IndividualApplicationForms = new HashSet<IndividualApplicationForm>();
        }

        public Guid ProgramID { get; set; }

        [Required]
        [StringLength(50)]
        public string ProgramName { get; set; }

        public string ProgramNameAR { get; set; }

        [DefaultValue(true)]
        public bool IsActive { get; set; }

        public virtual ICollection<CorporateApplicationForm> CorporateApplicationForms { get; set; }

        public virtual ICollection<IndividualApplicationForm> IndividualApplicationForms { get; set; }
    }
}