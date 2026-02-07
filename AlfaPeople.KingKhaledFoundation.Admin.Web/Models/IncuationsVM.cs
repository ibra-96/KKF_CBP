using System;
using System.Web;
using System.Web.Mvc;
using AlfaPeople.KingKhalidFoundation.Data.Model;

namespace AlfaPeople.KingKhaledFoundation.Admin.Web.Models
{
    public class IncuationsVM
    {
        public Incubation incubation { get; set; }

        public HttpPostedFileBase[] files { get; set; }
    }

    public class IncAdModVM
    {
        public SelectList IncubationPP { get; set; }

        public SelectList IncubationAdModel { get; set; }
    }
}