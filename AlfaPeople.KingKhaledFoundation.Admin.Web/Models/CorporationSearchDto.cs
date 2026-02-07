using System;
using AlfaPeople.KingKhalidFoundation.Data.Model;

namespace AlfaPeople.KingKhaledFoundation.Admin.Web.Models
{
    public class CorporationSearchDto
    {
        public string ProjectName { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public Guid? Government { get; set; }
        public int? CorporationAge { get; set; }
        public CorporateGenderType? GenderType { get; set; }
        public Guid? CorporationName { get; set; }
        public Guid? CorporateFieldOfWorkID { get; set; }
        public Guid? CorporationsCategoryID { get; set; }
    }
}