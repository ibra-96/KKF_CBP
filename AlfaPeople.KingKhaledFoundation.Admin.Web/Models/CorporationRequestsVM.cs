using System;

namespace AlfaPeople.KingKhaledFoundation.Admin.Web.Models
{
    public class CorporationRequestsVM
    {
        public byte[] Pic { get; set; }
        public string Name { get; set; }
        public string History { get; set; }
        public Guid FronUserID { get; set; }
    }
}