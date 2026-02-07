using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AlfaPeople.KingKhaledFoundation.Admin.Web.Models
{
    public class IncubationWorkshopQuestionVM
    {
        public Guid QuestionID { get; set; }
        public Guid IncubationWorkshopId { get; set; }
        public string QuestionText { get; set; }
        public string QuestionType { get; set; } // مثل (نص، اختيار متعدد، إلخ)
    }
}