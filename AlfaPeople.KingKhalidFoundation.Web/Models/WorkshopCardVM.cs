using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AlfaPeople.KingKhalidFoundation.Web.Models
{
    public class WorkshopCardVM
    {
        public Guid WorkshopId { get; set; }
        public string Title { get; set; }

        public bool CanSeePostImpact { get; set; }
        public bool PostImpactCompleted { get; set; }
        public string PostImpactStatusLabel { get; set; } // Open / Closed / Completed / NotAvailable
    }

}