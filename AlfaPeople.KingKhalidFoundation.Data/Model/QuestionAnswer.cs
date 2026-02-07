using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlfaPeople.KingKhalidFoundation.Data.Model
{
    public class QuestionAnswer
    {
        public Guid QuestionAnswerID { get; set; }

        // رابط للمشارك
        public Guid FrontendUserId { get; set; }
        public virtual FrontendUser FrontendUser { get; set; }

        // رابط للورشة
        public Guid IncubationWorkshopID { get; set; }
        public virtual IncubationWorkshop IncubationWorkshop { get; set; }

        // رابط للسؤال
        public int QuestionId { get; set; }
        public virtual QuestionModel Question { get; set; }

        // الإجابة
        public string Answer { get; set; }

        // إذا كانت الإجابة مطلوبة أم لا
        public bool IsRequired { get; set; }

        // الترتيب
        public int Order { get; set; }
    }
}