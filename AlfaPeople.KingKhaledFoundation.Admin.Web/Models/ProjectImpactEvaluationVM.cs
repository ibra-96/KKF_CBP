using System;
using AlfaPeople.KingKhalidFoundation.Data.Model;

namespace AlfaPeople.KingKhaledFoundation.Admin.Web.Models
{
    public class ProjectImpactEvaluationVM
    {
   
        public ProjectImpactEvaluation ProjectImpactEvaluation { get; set; }

        public Guid[] FrontEndUsers { get; set; }
    }
}
