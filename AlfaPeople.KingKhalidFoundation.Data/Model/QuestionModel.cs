using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlfaPeople.KingKhalidFoundation.Data.Model
{
    public class QuestionModel
    {
        // مفتاح أساسي
        public int Id { get; set; }

        // السؤال باللغة الإنجليزية
        [Required]
        [Display(Name = "السؤال باللغة الانجليزية")]
        public string EnglishQuestion { get; set; }

        // السؤال باللغة العربية
        [Required]
        [Display(Name = "السؤال باللغة العربية")]
        public string ArabicQuestion { get; set; }

        // نوع السؤال
        [Required]
        [Display(Name = "نوع السؤال")]
        public string QuestionType { get; set; }

        // نوع الإجابة
        [Required]
        [Display(Name = "نوع الإجابة")]
        public string AnswerType { get; set; }

        // مرئي (إذا كان السؤال مرئيًا في النموذج)
        [Display(Name = "مرئي")]
        public bool IsVisible { get; set; }

        // مطلوب (هل الإجابة على هذا السؤال إلزامية)
        [Display(Name = "مطلوب")]
        public bool IsRequired { get; set; }

        // الترتيب (ترتيب السؤال في النموذج)
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "الترتيب يجب أن يكون رقماً صحيحاً")]
        [Display(Name = "الترتيب")]
        public int Order { get; set; }
        // حقل لتخزين خيارات الـ Checkbox
        public virtual ICollection<CheckboxOption> CheckboxOptions { get; set; }
        // مفتاح أجنبي يشير إلى IncubationWorkshop
        [ForeignKey("IncubationWorkshop")]
        public Guid IncubationWorkshopId { get; set; }

        public virtual IncubationWorkshop IncubationWorkshop { get; set; }
    }
}