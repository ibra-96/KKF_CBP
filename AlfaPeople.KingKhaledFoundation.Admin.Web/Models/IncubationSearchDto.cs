using System;

namespace AlfaPeople.KingKhaledFoundation.Admin.Web.Models
{
    public class IncubationSearchDto
    {
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public Guid? Government { get; set; }
        public Guid? FundingSource { get; set; }
        public Guid? Cunsaltant { get; set; }
        public Guid? CorporateFieldOfWorkID { get; set; }
        public Guid? CorporationsCategoryID { get; set; }
        public Guid? CorporationName { get; set; }
    }
}