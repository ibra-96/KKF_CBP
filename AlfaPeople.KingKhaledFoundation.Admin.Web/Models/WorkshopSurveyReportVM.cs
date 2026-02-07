using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AlfaPeople.KingKhaledFoundation.Admin.Web.Models
{
    public class WorkshopSurveyReportVM
    {
        public string CorporationName { get; set; }
        public string ProjectName { get; set; }
        public string ProjectDescription { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string ConsultantName { get; set; }
        public string Region { get; set; }
        public string Governorate { get; set; }
        public string City { get; set; }
        public string FundingSource { get; set; }
        public string WorkshopModel { get; set; }
        public string FormStatus { get; set; }
        public string FormSubmittedDate { get; set; }
        //public string FormModifiedDate { get; set; }
        public string Feadback { get; set; }

        public Dictionary<string, string> DynamicAnswers { get; set; } = new Dictionary<string, string>();
    }


}