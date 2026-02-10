using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AlfaPeople.KingKhalidFoundation.Data.Model;
namespace AlfaPeople.KingKhaledFoundation.Admin.Web.Models
{
    public class IncubationWSPostImpactReadOnlyVM
    {
        public Guid EvaluationId { get; set; }
        public Guid FrontendUserID { get; set; }
        public Guid IncubationWorkshopID { get; set; }
        public string WorkshopName { get; set; }
        public string CorporateName { get; set; }

        // من جدول ProjectImpactEvaluationRequest
        public DateTime? FilledOn { get; set; }

        // آخر تاريخ حفظ داخل جدول القيم IncubationWorkshopBLTransactionsValue
        public DateTime? LastSubmissionDate { get; set; }

        public IncubationWorkshopBLTransactionsType IncubationWorkshopBLTransactionsType { get; set; }
        public List<IncubationWorkshopBLTransactionsValue> Values { get; set; } = new List<IncubationWorkshopBLTransactionsValue>();
    }
}