using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AlfaPeople.KingKhalidFoundation.Web.Models
{
  
        public class PostImpactRequestVM
        {
            public Guid WorkshopId { get; set; }
            public string WorkshopName { get; set; }

            //public Guid EvaluationId { get; set; }  // ProjectImpactEvaluationId 
            public Guid RequestId { get; set; }     // ProjectImpactEvaluationRequestId (طلب الأدمن)

            public DateTime Deadline { get; set; }      // آخر موعد للتعبئة
            public DateTime? ReminderDate { get; set; } // تاريخ التذكير

            public bool IsSubmitted { get; set; }
            public DateTime? SubmittedDate { get; set; }
        }
    }


