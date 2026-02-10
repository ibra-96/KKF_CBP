using System;
using System.Web.Mvc;
using System.Collections.Generic;

namespace AlfaPeople.KingKhaledFoundation.Admin.Web.Models
{
    public class IncubationResultDto
    {
        public IncubationSearchDto SearchDto { get; set; }

        public List<IncubationDto> Incubations { get; set; } = new List<IncubationDto>();

        public string FileGuid { get; set; }

        public FileResult FileResult { get; set; }
    }

    public class IncubationDto
    {
        public string ModelName { get; set; }

        public string Corporation { get; set; }

        public string Governate { get; set; }

        public string FundingSource { get; set; }

        public string Consultant { get; set; }

        public string CorporationCategory { get; set; }

        public string Gender { get; set; }

        public string FieldOfWork { get; set; }

        public int Age { get; set; }

        public int FoundedYear { get; set; }
    }
}