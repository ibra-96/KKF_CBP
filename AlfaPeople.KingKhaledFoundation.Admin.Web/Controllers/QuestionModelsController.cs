using System;
using System.Linq;
using System.Web.Mvc;
using AlphaPeople.Core;
using System.Configuration;
using AlphaPeople.Repository;
using AlfaPeople.KingKhalidFoundation.Data;
using AlfaPeople.KingKhalidFoundation.Data.Model;
using AlfaPeople.KingKhaledFoundation.Admin.Web.Helper;
using System.Collections.Generic;

namespace AlfaPeople.KingKhaledFoundation.Admin.Web.Controllers
{
    [Authorize(Roles = "Admin, CB Manager, CB Supervisor, CB Analyst")]
    public class QuestionModelsController : BaseController
    {
        private readonly IRepository repository;
        private readonly CommonHelper helper;
        private readonly string Logpath = ConfigurationManager.AppSettings["Log"].ToString();

        public QuestionModelsController()
        {
            helper = new CommonHelper();
            repository = new Repository(new KingkhaledFoundationDB());
        }

        // عرض جميع الأسئلة
        public ActionResult Index()
        {
            ViewBag.lang = CultureHelper.CurrentCulture;
            var questions = repository.GetAll<QuestionModel>().ToList();
            return View(questions);
        }

        // عرض صفحة إنشاء سؤال جديد
        public ActionResult Create()
        {
            // تحميل قائمة ورش العمل لاختيار الورشة المرتبطة بالسؤال
            ViewBag.IncubationWorkshopId = new SelectList(repository.GetAll<IncubationWorkshop>(), "IncubationWorkshopID", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(List<QuestionModel> questions, Guid IncubationWorkshopId)
        {
            if (!ModelState.IsValid)
            {
                // طباعة الأخطاء للتحقق من سبب فشل التحقق
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                foreach (var error in errors)
                {
                    Console.WriteLine(error);  // أو اعرضها في الـ View باستخدام ViewData
                }

                return Json(new { success = false, message = "حدث خطأ أثناء الحفظ", errors = errors }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                foreach (var question in questions)
                {
                    // تأكد من تعيين معرف الورشة للسؤال
                    question.IncubationWorkshopId = IncubationWorkshopId;

                    // إذا كان السؤال يحتوي على خيارات Checkbox
                    if (question.CheckboxOptions != null && question.CheckboxOptions.Any())
                    {
                        // التأكد من أن الخيارات غير فارغة أو تحتوي على قيم
                        foreach (var option in question.CheckboxOptions)
                        {
                            // يمكنك إضافة منطق إضافي هنا للتحقق من قيم الخيار قبل حفظها
                            if (string.IsNullOrEmpty(option.OptionText))
                            {
                                // إذا كان هناك خيار فارغ، يمكن أن تضيف رسالة تحذير أو تمنع الحفظ
                                ModelState.AddModelError("", "يجب إدخال جميع خيارات الـ Checkbox.");
                                return View();
                            }
                        }
                    }

                    // حفظ كل سؤال
                    repository.Add<QuestionModel>(question);

                    // إذا كان السؤال يحتوي على خيارات، احفظها أيضًا
                    if (question.CheckboxOptions != null && question.CheckboxOptions.Any())
                    {
                        foreach (var option in question.CheckboxOptions)
                        {
                            option.QuestionId = question.Id; // ربط الخيار بالسؤال
                            repository.Add<CheckboxOption>(option);
                        }
                    }
                }

                // حفظ التغييرات في قاعدة البيانات
                repository.UnitOfWork.SaveChanges();

                // إعادة التوجيه إلى صفحة Create بعد حفظ الأسئلة
                return RedirectToAction("Create");
            }

            // في حال حدوث خطأ في التحقق من صحة البيانات
            return Json(new { success = false, message = "حدث خطأ أثناء الحفظ" }, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(List<QuestionModel> questions, Guid IncubationWorkshopId)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        foreach (var question in questions)
        //        {
        //            // تأكد من تعيين معرف الورشة للسؤال
        //            question.IncubationWorkshopId = IncubationWorkshopId;

        //            // حفظ كل سؤال
        //            repository.Add<QuestionModel>(question);
        //        }

        //        repository.UnitOfWork.SaveChanges();
        //        return RedirectToAction("Create");
        //        //return Json(new { success = true, message = "تم إنشاء الأسئلة بنجاح" }, JsonRequestBehavior.AllowGet);
        //    }

        //    return Json(new { success = false, message = "حدث خطأ أثناء الحفظ" }, JsonRequestBehavior.AllowGet);
        //}


        // عرض صفحة تعديل سؤال
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var questionModel = repository.GetByKey<QuestionModel>(id);
            if (questionModel == null)
            {
                return HttpNotFound();
            }

            // تحميل خيارات الـ Checkbox عند تعديل السؤال
            ViewBag.CheckboxOptions = repository.GetAll<CheckboxOption>()
                                                .Where(co => co.QuestionId == questionModel.Id)
                                                .Select(co => co.OptionText)
                                                .ToList();

            ViewBag.IncubationWorkshopId = new SelectList(repository.GetAll<IncubationWorkshop>(), "IncubationWorkshopID", "Name", questionModel.IncubationWorkshopId);
            return View(questionModel);
        }

        // حفظ التعديلات على السؤال
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(QuestionModel questionModel, List<string> checkboxOptions)
        {
            if (ModelState.IsValid)
            {
                // التعامل مع نوع السؤال "Checkbox" وإضافة الخيارات
                if (questionModel.QuestionType == "Checkbox" && checkboxOptions != null)
                {
                    // حذف الخيارات القديمة من الجدول (إذا كانت موجودة)
                    var oldOptions = repository.GetAll<CheckboxOption>().Where(co => co.QuestionId == questionModel.Id).ToList();
                    foreach (var option in oldOptions)
                    {
                        repository.Delete<CheckboxOption>(option);
                    }

                    // إضافة الخيارات الجديدة
                    foreach (var option in checkboxOptions)
                    {
                        var checkboxOption = new CheckboxOption
                        {
                            QuestionId = questionModel.Id,
                            OptionText = option
                        };
                        repository.Add<CheckboxOption>(checkboxOption);
                    }
                }

                repository.Update<QuestionModel>(questionModel);
                repository.UnitOfWork.SaveChanges();

                return Json(new { success = true, html = GlobalClass.RenderRazorViewToString(this, "Index", repository.GetAll<QuestionModel>().ToList()), message = "تم تحديث السؤال بنجاح", style = "custome" }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { success = false, html = GlobalClass.RenderRazorViewToString(this, "Index", repository.GetAll<QuestionModel>().ToList()), message = "حدث خطأ أثناء التحديث", style = "custome2" }, JsonRequestBehavior.AllowGet);
        }

        // حذف السؤال
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var questionModel = repository.GetByKey<QuestionModel>(id);
            if (questionModel == null)
            {
                return HttpNotFound();
            }

            return View(questionModel);
        }

        // تأكيد الحذف
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var questionModel = repository.GetByKey<QuestionModel>(id);
            if (questionModel != null)
            {
                repository.Delete<QuestionModel>(questionModel);

                // حذف الخيارات المرتبطة بالسؤال
                var checkboxOptions = repository.GetAll<CheckboxOption>().Where(co => co.QuestionId == id).ToList();
                foreach (var option in checkboxOptions)
                {
                    repository.Delete<CheckboxOption>(option);
                }

                repository.UnitOfWork.SaveChanges();
                return Json(new { success = true, html = GlobalClass.RenderRazorViewToString(this, "Index", repository.GetAll<QuestionModel>().ToList()), message = "تم حذف السؤال بنجاح", style = "custome" }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { success = false, html = GlobalClass.RenderRazorViewToString(this, "Index", repository.GetAll<QuestionModel>().ToList()), message = "حدث خطأ أثناء الحذف", style = "custome2" }, JsonRequestBehavior.AllowGet);
        }
    }
}