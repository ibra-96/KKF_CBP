namespace AlfaPeople.KingKhalidFoundation.Data.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class IncubationType
    {
        public IncubationType()
        {
            Incubations = new HashSet<Incubation>();
            IncubationModels = new HashSet<IncubationModel>();
            IncubationFollowUpEvaluations = new HashSet<IncubationFollowUpEvaluation>();
        }

        public Guid IncubationTypeID { get; set; }

        [Required]
        [StringLength(50)]
        public string NameAR { get; set; }

        [Required]
        [StringLength(50)]
        public string NameEN { get; set; }

        public virtual ICollection<Incubation> Incubations { get; set; }

        public virtual ICollection<IncubationModel> IncubationModels { get; set; }

        public virtual ICollection<IncubationFollowUpEvaluation> IncubationFollowUpEvaluations { get; set; }
    }
}