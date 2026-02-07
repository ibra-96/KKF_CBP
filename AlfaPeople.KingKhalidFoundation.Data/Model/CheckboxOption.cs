using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlfaPeople.KingKhalidFoundation.Data.Model
{
    public class CheckboxOption
    {

        public int Id { get; set; }  // مفتاح أساسي
        public int QuestionId { get; set; }  // مفتاح أجنبي يرتبط بـ QuestionModel
        public string OptionText { get; set; }  // النص الفعلي للخيار

        public virtual QuestionModel Question { get; set; }  // علاقة مع QuestionModel
    }
}
