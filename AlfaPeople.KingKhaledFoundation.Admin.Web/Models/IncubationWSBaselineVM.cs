using System;
using AlfaPeople.KingKhalidFoundation.Data.Model;

namespace AlfaPeople.KingKhaledFoundation.Admin.Web.Models
{
    public class IncubationWSBaselineVM
    {

        public string Feadback { get; set; }
        public Guid FrontendUserID { get; set; }
        public DateTime SubmissionDate { get; set; }
        public Guid IncubationWSBLTransValStatusID { get; set; }
        public IncubationWorkshopBLTransactionsType IncubationWorkshopBLTransactionsType { get; set; }
        public Guid IncubationWorkshopID { get; set; }   // إضافة الحقل هنا
        public string WorkshopName { get; set; } // إضافة اسم الورشة
    }
}