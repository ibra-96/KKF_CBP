using System;
using System.Web.Mvc;
using System.Collections.Generic;

namespace AlfaPeople.KingKhaledFoundation.Admin.Web.Models
{
    public class WorkshopResultDto
    {
        public WorkshopSearchDto SearchDto { get; set; }

        public string FileGuid { get; set; }

        public FileContentResult FileContentResult { get; set; }

        public List<WorkShopDto> Results { get; set; } = new List<WorkShopDto>();

        public int GrantsCount { get; set; }
    }

    public class WorkShopDto
    {
        public string WorkshopName { get; set; }

        public string WorkshopModelName { get; set; }

        public string CorporationName { get; set; }

        public string Attender { get; set; }

        public string GovernorateName { get; set; }

        public int? CountOfAdventages { get; set; }

        public int? CountOfConfirmed { get; set; }

        public int? CountOfPartcipation { get; set; }

        public string Position { get; set; }

        public string Qualification { get; set; }

        public string ProjectSuccessRate { get; set; }

        public string Gender { get; set; }

        public string ConsultantName { get; set; }
    }
}