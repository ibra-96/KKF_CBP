using System;
using System.Web.Mvc;
using System.Collections.Generic;

namespace AlfaPeople.KingKhaledFoundation.Admin.Web.Models
{
    public class CorporationContactResultDto
    {
        public CorporationContactSearchDto SearchDto { get; set; }

        public List<CorporationContactDto> CorporationContacts { get; set; } = new List<CorporationContactDto>();

        public string FileGuid { get; set; }

        public FileResult FileResult { get; set; }
    }

    public class CorporationContactDto
    {
        public string Corporation { get; set; }

        public string ProjectManagerName { get; set; }

        public string ProjectManagerEmail { get; set; }

        public string ProjectManagerMobile { get; set; }

        public string CorporationAddress { get; set; }

        public string CorporationManager { get; set; }

        public string CorporationManagerEmail { get; set; }

        public string CorporationManagerMobile { get; set; }
    }
}