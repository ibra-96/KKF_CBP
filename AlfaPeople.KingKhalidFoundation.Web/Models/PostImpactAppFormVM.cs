using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AlfaPeople.KingKhalidFoundation.Web.Models
{
    public class PostImpactAppFormVM
    {
        public Guid WorkshopId { get; set; }
        public Guid RequestId { get; set; }   

        public DateTime Deadline { get; set; }

        // الأسئلة
        public List<PostImpactQuestionVM> Questions { get; set; } = new List<PostImpactQuestionVM>();

        // الإجابات (مفتاح السؤال -> قيمة)
        public Dictionary<Guid, string> Answers { get; set; } = new Dictionary<Guid, string>();

        // لو في أسئلة ملفات
        public Dictionary<Guid, HttpPostedFileBase> FileAnswers { get; set; } = new Dictionary<Guid, HttpPostedFileBase>();

        public DateTime? LastSubmitDate { get; set; }
    }

    public class PostImpactQuestionVM
    {
        public Guid QuestionId { get; set; }
        public string TitleAr { get; set; }
        public string TitleEn { get; set; }

        public string InputType { get; set; } // Text, Number, Radio, Checkbox, File...
        public bool IsRequired { get; set; }

        public List<PostImpactOptionVM> Options { get; set; } = new List<PostImpactOptionVM>();
    }

    public class PostImpactOptionVM
    {
        public Guid OptionId { get; set; }
        public string TextAr { get; set; }
        public string TextEn { get; set; }
    }

}