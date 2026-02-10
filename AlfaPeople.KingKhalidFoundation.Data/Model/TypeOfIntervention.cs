using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlfaPeople.KingKhalidFoundation.Data.Model
{
    [Table("TypeOfIntervention")]
    public partial class TypeOfIntervention
    {
        public TypeOfIntervention()
        {
            Incubations = new HashSet<Incubation>();
        }

        public Guid TypeOfInterventionID { get; set; }

        [Required]
        [StringLength(50)]
        public string TypeOfInterventionNameAR { get; set; }

        [Required]
        [StringLength(50)]
        public string TypeOfInterventionNameEN { get; set; }

        [Required]
        [DefaultValue(true)]
        public bool IsActive { get; set; }

        public virtual ICollection<Incubation> Incubations { get; set; }
    }
}