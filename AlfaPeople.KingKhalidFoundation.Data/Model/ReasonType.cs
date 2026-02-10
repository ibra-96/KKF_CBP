using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AlfaPeople.KingKhalidFoundation.Data.Model
{
    public class ReasonType
    {
        public ReasonType()
        {
            CorporateApplicationStatus = new HashSet<CorporateApplicationStatu>();
        }

        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<CorporateApplicationStatu> CorporateApplicationStatus { get; set; }
    }
}