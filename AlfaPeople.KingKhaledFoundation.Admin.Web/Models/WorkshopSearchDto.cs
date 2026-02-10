using System;
using AlfaPeople.KingKhalidFoundation.Data.Model;

namespace AlfaPeople.KingKhaledFoundation.Admin.Web.Models
{
    public class WorkshopSearchDto
    {
        public DateTime? DateFrom { get; set; }

        public DateTime? DateTo { get; set; }

        public Guid? IncubationWorkshopModelId { get; set; }

        public GenderPerson? Gender { get; set; }

        public Guid? GovernateId { get; set; }

        public Guid? ConsultantId { get; set; }

        public Guid? IncubationWorkshopId { get; set; }

        public Guid? CorporateId { get; set; }

        public ProjectSuccessRate? ProjectSuccessRate { get; set; }
    }
}