using System;
using System.Web.Mvc;
using System.Collections.Generic;

namespace AlfaPeople.KingKhaledFoundation.Admin.Web.Models
{
    public class CorporationsResult
    {
        public CorporationSearchDto SearchDto { get; set; }

        public List<CorporationDto> Corporations { get; set; } = new List<CorporationDto>();

        public string FileGuid { get; set; }

        public FileContentResult FileContentResult { get; set; }
    }

    public class CorporationDto
    {
        public string ProjectName { get; set; }

        public string Corporation { get; set; }

        public int Age { get; set; }

        public string Governate { get; set; }

        public string CorporationCategory { get; set; }

        public string Gender { get; set; }

        public int CorporationVolume { get; set; }

        public string FieldOfWork { get; set; }
    }
}