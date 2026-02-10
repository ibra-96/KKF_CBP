using System;
using System.Web;
using AlfaPeople.KingKhalidFoundation.Data.Model;

namespace AlfaPeople.KingKhalidFoundation.Web.Models
{
    public class IncubationBaselineVM
    {
        public bool DraftFlage { get; set; }

        public HttpPostedFileBase StrategicPlan { get; set; }

        public HttpPostedFileBase OperatingPlan { get; set; }

        public HttpPostedFileBase GovernanceGuide { get; set; }

        public HttpPostedFileBase CharteredAccountantNotes { get; set; }

        public HttpPostedFileBase AttachFollowUpandEvaluationForms { get; set; }

        public IncubationBaseline IncubationBL { get; set; }
    }
}