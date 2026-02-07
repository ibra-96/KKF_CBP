using System;

namespace AlfaPeople.KingKhaledFoundation.Admin.Web.Models
{
    public class CorporationContactSearchDto
    {
        public DateTime? DateFrom { get; set; }

        public DateTime? DateTo { get; set; }

        public Guid? GrantModelId { get; set; }

        public Guid? Government { get; set; }

        public Guid? Corperation { get; set; }
    }
}