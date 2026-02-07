using System;
using System.IO;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using AlphaPeople.Core;
using System.Configuration;
using AlphaPeople.Repository;
using System.Collections.Generic;
using AlfaPeople.KingKhalidFoundation.Data;
using AlfaPeople.KingKhalidFoundation.Data.Model;
using AlfaPeople.KingKhaledFoundation.Admin.Web.Models;
using System.Data.Entity;
using ClosedXML.Excel;
using AlfaPeople.KingKhaledFoundation.Admin.Web.Helper;

namespace AlfaPeople.KingKhaledFoundation.Admin.Web.Controllers
{
    [Authorize(Roles = "Admin, CB Manager, CB Supervisor, CB Analyst")]
    public class IncubationWorkshopBLTransactionsController : BaseController
    {
        private readonly IRepository repository;
        private readonly CommonHelper helper;
        private readonly string Logpath = ConfigurationManager.AppSettings["Log"].ToString();

        public IncubationWorkshopBLTransactionsController()
        {
            repository = new Repository(new KingkhaledFoundationDB());
            helper = new CommonHelper();
        }


        //23-3-2025
        [HttpGet]
        public ActionResult WorkshopDynamicReport()
        {
            ViewBag.Workshops = repository.GetQuery<IncubationWorkshop>()
                .Where(w => !w.IsDeleted)
                .Select(w => new SelectListItem
                {
                    Value = w.IncubationWorkshopID.ToString(),
                    Text = w.Name
                }).ToList();

            return View();
        }
        [HttpPost]
        public ActionResult WorkshopDynamicReport(Guid workshopId, string exportExcel)
        {
            bool isArabic = CultureHelper.CurrentCulture == 3;
            ViewBag.IsPost = true;
            ViewBag.SelectedWorkshopId = workshopId;
            // جلب ورش العمل للقائمة المنسدلة
            ViewBag.Workshops = repository.GetQuery<IncubationWorkshop>()
                .Where(w => !w.IsDeleted)
                .Select(w => new SelectListItem
                {
                    Value = w.IncubationWorkshopID.ToString(),
                    Text = w.Name
                }).ToList();

            // جلب بيانات الورشة المختارة
            var workshop = repository.GetQuery<IncubationWorkshop>(w => w.IncubationWorkshopID == workshopId)
                .Include(w => w.Consultant)
                .Include(w => w.City)
                .Include(w => w.Region)
                .Include(w => w.Governorate)
                .Include(w => w.FundingSource)
                .Include(w => w.IncubationWorkshopModel)
                .FirstOrDefault();

            var transactions = repository.GetQuery<IncubationWorkshopBLTransactions>(t => t.IncubationWorkshopID == workshopId)
                .Include(t => t.Options)
                .ToList();

            var transactionIds = transactions.Select(t => t.TransID).ToList();

            var values = repository.GetQuery<IncubationWorkshopBLTransactionsValue>(v => transactionIds.Contains(v.TransID))
                .Include(v => v.IncubationWorkshopBLTransValStatus)
                .Include(v => v.IncubationWSBLTransValueAttachment)
                .Include(v => v.OptionValues)
           
                .ToList();

            var frontendUserIds = values.Select(v => v.FrontendUserID).Distinct().ToList();

            var frontendUsers = repository.GetQuery<FrontendUser>(u => frontendUserIds.Contains(u.UserID))
                .Include(u => u.CorporateApplicationForms)
                .ToList();

            var model = new List<WorkshopSurveyReportVM>();

            foreach (var frontendUser in frontendUsers)
            {
                var corp = frontendUser.CorporateApplicationForms.FirstOrDefault();
                var relatedAnswers = values.Where(v => v.FrontendUserID == frontendUser.UserID).ToList();

                var dynamicAnswers = new Dictionary<string, string>();
                var allOptions = repository.GetAll<IncubationWorkshopControlOptions>().ToList();
                foreach (var trans in transactions)
                {
                   

                    var answerSet = relatedAnswers.Where(v => v.TransID == trans.TransID).ToList();
                    if (answerSet.Any())
                    {
                        if (trans.Options.Any())
                        {
                           
                            var selected = answerSet
                              .SelectMany(a => a.OptionValues)
                              .Select(o =>
                              {
                                  var option = allOptions.FirstOrDefault(opt => opt.OptionID == o.OptionID);
                                  return isArabic ? option?.OptionNameAr : option?.OptionNameEn;
                              })
                              .Where(val => !string.IsNullOrEmpty(val))
                              .ToList();
                            //var selected = answerSet
                            //   .SelectMany(a => a.OptionValues)
                            //   .Select(o => 
                            //   allOptions.FirstOrDefault(opt => opt.OptionID == o.OptionID)?.OptionNameAr)
                            //   .Where(val => !string.IsNullOrEmpty(val))
                            //   .ToList();
                            dynamicAnswers[isArabic ? trans.FieldNameAr : trans.FieldNameEn] = string.Join(", ", selected);
                        }
                        else if (answerSet.First().IncubationWSBLTransValueAttachment?.Count > 0)
                        {
                            dynamicAnswers[isArabic ? trans.FieldNameAr : trans.FieldNameEn]= string.Join(", ", answerSet.First().IncubationWSBLTransValueAttachment.Select(f => f.Name));
                        }
                        else
                        {
                            dynamicAnswers[isArabic ? trans.FieldNameAr : trans.FieldNameEn] = answerSet.First().Value;
                        }
                    }
                }

                model.Add(new WorkshopSurveyReportVM
                {
                    CorporationName = corp?.Name,
                    ProjectName = workshop?.Name,
                    ProjectDescription = workshop?.Description,
                    StartDate = workshop?.StartDate.ToString("yyyy-MM-dd"),
                    EndDate = workshop?.EndDate.ToString("yyyy-MM-dd"),
                    ConsultantName = workshop?.Consultant?.Name,
                    Region = workshop?.Region?.RegionNameAR,
                    Governorate = workshop?.Governorate?.GovernorateAR,
                    City = workshop?.City?.CityNameAR,
                    FundingSource = workshop?.FundingSource?.FundingSourceNameAR,
                    WorkshopModel = workshop?.IncubationWorkshopModel?.NameAR,
                    FormStatus = relatedAnswers.FirstOrDefault()?.IncubationWorkshopBLTransValStatus?.NameAR,
                    FormSubmittedDate = relatedAnswers.Min(v => v.SubmissionDate)?.ToString("yyyy-MM-dd"),
                   
                    Feadback = relatedAnswers.FirstOrDefault()?.Feadback,
                    DynamicAnswers = dynamicAnswers
                });
            }

            TempData["WorkshopReportData"] = model;
            TempData["WorkshopName"] = workshop?.Name;

            //if (exportExcel == "yes")
            //    return RedirectToAction("ExportWorkshopSurveyToExcel");

            return View("WorkshopDynamicReport", model);
        }

        [HttpGet]
        public ActionResult ExportWorkshopSurveyToExcel(Guid workshopId)
        {
            return ExportWorkshopSurvey(workshopId); // أو استدعاء الدالة التي تولد ملف Excel
        }
        [HttpGet]
        public ActionResult ExportWorkshopSurvey(Guid workshopId)
        {
            var isArabic = CultureHelper.CurrentCulture == 3;
            var workshop = repository.GetQuery<IncubationWorkshop>(w => w.IncubationWorkshopID == workshopId)
                .Include(w => w.Consultant)
                .Include(w => w.City)
                .Include(w => w.Region)
                .Include(w => w.Governorate)
                .Include(w => w.FundingSource)
                .Include(w => w.IncubationWorkshopModel)
                .FirstOrDefault();

            var transactions = repository.GetQuery<IncubationWorkshopBLTransactions>(t => t.IncubationWorkshopID == workshopId)
                .Include(t => t.Options)
                .ToList();

            var transactionIds = transactions.Select(t => t.TransID).ToList();

            var values = repository.GetQuery<IncubationWorkshopBLTransactionsValue>(v => transactionIds.Contains(v.TransID))
                .Include(v => v.OptionValues.Select(o => o.Option)) // لجلب اسم الخيارات
                .Include(v => v.IncubationWorkshopBLTransValStatus)
                .Include(v => v.IncubationWSBLTransValueAttachment)
                .ToList();

            var frontendUserIds = values.Select(v => v.FrontendUserID).Distinct().ToList();

            var frontendUsers = repository.GetQuery<FrontendUser>(u => frontendUserIds.Contains(u.UserID))
                .Include(u => u.CorporateApplicationForms)
                .ToList();

            var dt = new DataTable("ورشة العمل");

            // أعمدة ثابتة
            dt.Columns.Add("اسم الجمعية");
            dt.Columns.Add("اسم المشروع");
            dt.Columns.Add("تاريخ البداية");
            dt.Columns.Add("تاريخ النهاية");
            dt.Columns.Add("الاستشاري");
            dt.Columns.Add("المنطقة");
            dt.Columns.Add("المحافظة");
            dt.Columns.Add("الحي");
            dt.Columns.Add("مصدر التمويل");
            dt.Columns.Add("نموذج الورشة");
            dt.Columns.Add("حالة النموذج");
            dt.Columns.Add("تاريخ التقديم");
            dt.Columns.Add("ملاحظات النموذج");

            // أعمدة الأسئلة الديناميكية
            foreach (var trans in transactions)
                dt.Columns.Add(trans.FieldNameAr);

            foreach (var user in frontendUsers)
            {
                var corp = user.CorporateApplicationForms.FirstOrDefault();
                var relatedAnswers = values.Where(v => v.FrontendUserID == user.UserID).ToList();
                if (!relatedAnswers.Any())
                    continue;

                var row = dt.NewRow();
                row["اسم الجمعية"] = corp?.Name;
                row["اسم المشروع"] = workshop?.Name;
                row["تاريخ البداية"] = workshop?.StartDate.ToString("yyyy-MM-dd");
                row["تاريخ النهاية"] = workshop?.EndDate.ToString("yyyy-MM-dd");
                row["الاستشاري"] = workshop?.Consultant?.Name;
                row["المنطقة"] = isArabic ? workshop?.Region?.RegionNameAR : workshop?.Region?.RegionNameEN;
                //row["المنطقة"] = workshop?.Region?.RegionNameAR;
                row["المحافظة"] = isArabic ? workshop?.Governorate?.GovernorateAR : workshop?.Governorate?.GovernorateEN;
                row["الحي"] = isArabic ? workshop?.City?.CityNameAR : workshop?.City?.CityNameEN;
                row["مصدر التمويل"] = isArabic ? workshop?.FundingSource?.FundingSourceNameAR : workshop?.FundingSource?.FundingSourceNameEN;
                row["حالة النموذج"] = isArabic ? relatedAnswers.FirstOrDefault()?.IncubationWorkshopBLTransValStatus?.NameAR : relatedAnswers.FirstOrDefault()?.IncubationWorkshopBLTransValStatus?.NameEN;

                row["نموذج الورشة"] = isArabic ? workshop?.IncubationWorkshopModel?.NameAR : workshop?.IncubationWorkshopModel?.NameEN;
                row["حالة النموذج"] = isArabic ? relatedAnswers.FirstOrDefault()?.IncubationWorkshopBLTransValStatus?.NameAR: relatedAnswers.FirstOrDefault()?.IncubationWorkshopBLTransValStatus?.NameEN;
                row["تاريخ التقديم"] = relatedAnswers.Min(v => v.SubmissionDate)?.ToString("yyyy-MM-dd");
                row["ملاحظات النموذج"] = relatedAnswers.FirstOrDefault()?.Feadback;

                foreach (var trans in transactions)
                {
                    var answerSet = relatedAnswers.Where(v => v.TransID == trans.TransID).ToList();
                    if (answerSet.Any())
                    {
                        if (trans.Options.Any())
                        {
                            // خيارات متعددة – عرض الأسماء وليس القيمة (true)
                            //var selected = answerSet
                            //    .SelectMany(a => a.OptionValues)
                            //    .Select(o => o.Option?.OptionNameAr)
                            //    .ToList();
                            var selected = answerSet
                                .SelectMany(a => a.OptionValues)
                                .Select(o => isArabic ? o.Option?.OptionNameAr : o.Option?.OptionNameEn)
                                .Where(v => !string.IsNullOrEmpty(v))
                                .ToList();
                            row[isArabic ? trans.FieldNameAr:trans.FieldNameEn] = string.Join(", ", selected);
                        }
                        else if (answerSet.First().IncubationWSBLTransValueAttachment?.Any() == true)
                        {
                            // ملفات
                            row[isArabic ? trans.FieldNameAr:trans.FieldNameEn] = string.Join(", ", answerSet.First().IncubationWSBLTransValueAttachment.Select(f => f.Name));
                        }
                        else
                        {
                            // نصوص
                            row[isArabic ? trans.FieldNameAr : trans.FieldNameEn] = answerSet.First().Value;
                        }
                    }
                }

                dt.Rows.Add(row);
            }

            // التصدير إلى Excel
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    string safeWorkshopName = string.Join("_", workshop?.Name.Split(Path.GetInvalidFileNameChars()) ?? new string[] { "Workshop" });
                    string fileName = $"{safeWorkshopName}_Report_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }


        //[HttpPost]
        //public ActionResult ExportWorkshopSurveyToExcel(Guid workshopId)
        //{
        //    ViewBag.SelectedWorkshopId = workshopId;
        //    var data = TempData["WorkshopReportData"] as List<WorkshopSurveyReportVM>;
        //    var workshopName = TempData["WorkshopName"]?.ToString() ?? "Workshop";

        //    if (data == null || !data.Any())
        //        return Content("لا توجد بيانات للتصدير");

        //    DataTable dt = new DataTable();
        //    dt.Columns.Add("اسم الجمعية");
        //    dt.Columns.Add("اسم المشروع");
        //    dt.Columns.Add("وصف المشروع");
        //    dt.Columns.Add("تاريخ البداية");
        //    dt.Columns.Add("تاريخ النهاية");
        //    dt.Columns.Add("الاستشاري");
        //    dt.Columns.Add("المنطقة");
        //    dt.Columns.Add("المحافظة");
        //    dt.Columns.Add("الحي");
        //    dt.Columns.Add("مصدر التمويل");
        //    dt.Columns.Add("نموذج الورشة");
        //    dt.Columns.Add("حالة النموذج");
        //    dt.Columns.Add("تاريخ التقديم");


        //    var dynamicHeaders = data.First().DynamicAnswers.Keys.ToList();
        //    foreach (var header in dynamicHeaders)
        //        dt.Columns.Add(header);

        //    foreach (var item in data)
        //    {
        //        var row = dt.NewRow();
        //        row["اسم الجمعية"] = item.CorporationName;
        //        row["اسم المشروع"] = item.ProjectName;
        //        row["وصف المشروع"] = item.ProjectDescription;
        //        row["تاريخ البداية"] = item.StartDate;
        //        row["تاريخ النهاية"] = item.EndDate;
        //        row["الاستشاري"] = item.ConsultantName;
        //        row["المنطقة"] = item.Region;
        //        row["المحافظة"] = item.Governorate;
        //        row["الحي"] = item.City;
        //        row["مصدر التمويل"] = item.FundingSource;
        //        row["نموذج الورشة"] = item.WorkshopModel;
        //        row["حالة النموذج"] = item.FormStatus;
        //        row["تاريخ التقديم"] = item.FormSubmittedDate;


        //        foreach (var header in dynamicHeaders)
        //            row[header] = item.DynamicAnswers.ContainsKey(header) ? item.DynamicAnswers[header] : "";

        //        dt.Rows.Add(row);
        //    }

        //    using (XLWorkbook wb = new XLWorkbook())
        //    {
        //        wb.Worksheets.Add(dt);
        //        using (MemoryStream stream = new MemoryStream())
        //        {
        //            wb.SaveAs(stream);
        //            string safeWorkshopName = string.Join("_", workshopName.Split(Path.GetInvalidFileNameChars()));
        //            string fileName = $"{safeWorkshopName}_Workshop_Report_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
        //            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        //        }
        //    }
        //}







        [HttpGet]
        public ActionResult Manage(Guid? workshopID)
        {
            // إعداد القوائم المنسدلة
            //ViewBag.LstControls = new SelectList(repository.GetQuery<IncubationWorkshopControls>(), "ControlsID", "ControlsName");
            //ViewBag.LstControlTypes = new SelectList(repository.GetQuery<IncubationWorkshopControlsType>(), "ControlTypeID", "ControlTypeName");
            //ViewBag.LstLookupTransTypes = new SelectList(repository.GetQuery<IncubationWorkshopBLTransactionsType>(), "TransTypeID", "TransTypeName");
            ViewBag.LstLookupTransFields = workshopID.HasValue
                ? new SelectList(repository.GetQuery<IncubationWorkshopBLTransactions>().Where(t => t.IncubationWorkshopID == workshopID.Value), "TransID", "FieldName")
                : new SelectList(new List<SelectListItem>());
            //        ViewBag.LstRequired = new SelectList(new List<SelectListItem>
            //{
            //    new SelectListItem { Value = "true", Text = "Yes" },
            //    new SelectListItem { Value = "false", Text = "No" }
            //}, "Value", "Text");
            PrepareViewBags();
            // إعداد قائمة ورش العمل
            var workshops = repository.GetQuery<IncubationWorkshop>()
                 .Where(w => !w.IsDeleted )
                .Select(w => new SelectListItem
                {
                    Value = w.IncubationWorkshopID.ToString(),
                    Text = w.Name
                }).ToList();
            IncubationWorkshopBLTransactionsType transType = repository.FindOne<IncubationWorkshopBLTransactionsType>(t => t.TransTypeName == "IncubationWorkshopBaselineTransactions");

            // إنشاء النموذج
            if (transType == null)
            {
                transType = new IncubationWorkshopBLTransactionsType()
                {
                    TransTypeID = Guid.NewGuid(),
                    TransTypeName = "IncubationWorkshopBaselineTransactions",
                    IsMasterData = false
                };
                repository.Add(transType);
                repository.UnitOfWork.SaveChanges();

                var tableDesign = new       
                {
                    TransTypeID = transType.TransTypeID,
                    TransTypeName = transType.TransTypeName,
                    IsMasterData = transType.IsMasterData.ToString(),
                    LstTransaction = new List<IWSBLTransactionsVM>(),
                    Workshops = workshops,
                    WorkshopID = workshopID,
                };

                return View(tableDesign);
            }
            else
            {
                var tableDesign = new IWSBLTransactionsTypeVM
                {
                    TransTypeID = transType.TransTypeID,
                    TransTypeName = transType.TransTypeName,
                    IsMasterData = transType.IsMasterData.ToString(),
                    Workshops = workshops,
                    WorkshopID = workshopID,
                    LstTransaction = new List<IWSBLTransactionsVM>()
                };

                // جلب الحقول المرتبطة بورشة العمل
                if (workshopID.HasValue)
                {
                    var transactions = repository.GetQuery<IncubationWorkshopBLTransactions>()
                        .Where(t => t.IncubationWorkshopID == workshopID.Value)
                        .OrderBy(t => t.RankNumber).Include(t => t.Options)
                        .ToList();

                    foreach (var item in transactions)
                    {
                        var transactionVM = new IWSBLTransactionsVM
                        {
                            TransID = item.TransID.ToString(),
                            TransTypeID = item.TransTypeID.ToString(),
                            ControlID = item.IncubationWSControlsType?.ControlsID.ToString(),
                            ControlTypeID = item.ControlTypeID != null ? item.ControlTypeID.ToString() : "", 

                            //ControlTypeID = item.IncubationWSControlsType?.ControlTypeID.ToString(),
                            IsRequired = item.IsRequired,
                            FieldNameEn = item.FieldNameEn,
                            FieldNameAr = item.FieldNameAr,
                            RankNumber = item.RankNumber.ToString(),
                            ViewList_Display = item.ViewList_Display,
                            WorkshopID = workshopID,
                            Options = (item.ControlTypeID != null &&
                           (item.ControlTypeID.ToString() == "e1de6bd1-6ffa-41a1-b276-d9f3a47f0c3a" ||
                            item.ControlTypeID.ToString() == "76da194e-c139-4db6-865b-f1ea0d546a91"))
                    ? repository.GetQuery<IncubationWorkshopControlOptions>()
                        .Where(o => o.TransID == item.TransID)
                        .Select(o => new IncubationWorkshopControlOptionVM
                        {
                            OptionID = o.OptionID,
                            OptionNameEn = o.OptionNameEn,
                            OptionNameAr = o.OptionNameAr,
                            TransID = o.TransID,
                        }).ToList()
                    : new List<IncubationWorkshopControlOptionVM>()


                        //Options = item.ControlTypeID.ToString() == "e1de6bd1-6ffa-41a1-b276-d9f3a47f0c3a"
                        //    ? repository.GetQuery<IncubationWorkshopControlOptions>()
                        //        .Where(o => o.TransID == item.TransID)
                        //        .Select(o => new IncubationWorkshopControlOptionVM
                        //        {
                        //            OptionID = o.OptionID,
                        //            OptionNameEn = o.OptionNameEn,
                        //            OptionNameAr = o.OptionNameAr,
                        //            TransID = o.TransID,        
                        //        }).ToList()
                        //    : new List<IncubationWorkshopControlOptionVM>()
                    };

                        tableDesign.LstTransaction.Add(transactionVM);
                    }
                }

                // إذا كان الطلب AJAX، إعادة العناصر كـ PartialView
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return PartialView("_LstTransactionList", tableDesign.LstTransaction);
                }

                // إعادة الصفحة الكاملة إذا لم يكن الطلب AJAX
                return View(tableDesign);
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage(IWSBLTransactionsTypeVM model)
        {
            var workshops = repository.GetQuery<IncubationWorkshop>()
                 .Where(w => !w.IsDeleted)
                     .Select(w => new SelectListItem
                     {
                         Value = w.IncubationWorkshopID.ToString(),
                         Text = w.Name
                     }).ToList();
            // إعداد القوائم المنسدلة
            model.Workshops = workshops;
            model.WorkshopID = model.WorkshopID;
            try
            {
               
                PrepareViewBags();

                // التحقق من صحة النموذج
                if (model.LstTransaction == null || !model.LstTransaction.Any())
                {
                    ViewBag.err = "يجب إضافة حقل واحد على الأقل.";
                    return View(model);
                }

                var transType = repository.GetQuery<IncubationWorkshopBLTransactionsType>()
    .Include(t => t.IncubationWorkshopBLTrans)
    .FirstOrDefault(t => t.TransTypeID == model.TransTypeID);

                if (transType == null)
                {
                    ViewBag.err = "تعذر العثور على نوع الحقل.";
                    return View(model);
                }

                transType.TransTypeName = model.TransTypeName;
                transType.IsMasterData = bool.Parse(model.IsMasterData);
         
                foreach (var item in model.LstTransaction)
                {
                    if (string.IsNullOrWhiteSpace(item.TransID) )
                    {
                        if (item.LookupTransTypeID == null && item.LookupTransID == null)
                        {
                            // إضافة معاملة جديدة
                            var newTrans = CreateNewTransaction(item, model.WorkshopID, model.TransTypeID);
                            transType.IncubationWorkshopBLTrans.Add(newTrans);
                        }
                  
                    }
                    else
                    {
                        if (item.LookupTransTypeID == null && item.LookupTransID == null)
                        {
                            // تحديث معاملة موجودة&& t.TransTypeID == Guid.Parse(item.TransTypeID)
                            var existingTrans = transType.IncubationWorkshopBLTrans.FirstOrDefault(
                            t => t.TransID.ToString() == item.TransID && t.TransTypeID == Guid.Parse(item.TransTypeID));

                            if (existingTrans != null)
                            {
                                UpdateExistingTransaction(existingTrans, item, model.WorkshopID, model.TransTypeID);
                            }
                           
                           
                        }
                    }
                }

                repository.UnitOfWork.SaveChanges();
                TempData["SuccessMessage"] = "تم إضافة النموذج للمشروع بنجاح";
                return RedirectToAction("Manage");
            }
            catch (Exception ex)
            {
                
                model.WorkshopID = model.WorkshopID;
                ViewBag.err = $"حدث خطأ أثناء الحفظ: {ex.Message}";
                PrepareViewBags();
                return View(model);
            }
        }
        private void PrepareViewBags()
        {

            
           
            ViewBag.Title = App_GlobalResources.General.BaselineForm;
        
            ViewBag.LstRequired = new SelectList(new List<SelectListItem>
    {
        new SelectListItem { Value = "true", Text = "Yes" },
        new SelectListItem { Value = "false", Text = "No" }
    }, "Value", "Text", "true");

            ViewBag.LstControls = new SelectList(repository.GetQuery<IncubationWorkshopControls>(), "ControlsID", "ControlsName");
            //ViewBag.LstControlTypes = new SelectList(repository.GetQuery<IncubationWorkshopControlsType>(), "ControlTypeID", "ControlTypeName");
            ViewBag.LstLookupTransTypes = new SelectList(repository.GetQuery<IncubationWorkshopBLTransactionsType>(), "TransTypeID", "TransTypeName");
            //ViewBag.LstLookupTransFields = new SelectList(repository.GetQuery<IncubationWorkshopBLTransactions>(t => t.WorkshopID == workshopID), "TransID", "FieldName");
            var controls = repository.GetAll<IncubationWorkshopControls>().ToList();

            // تعيين القيمة الافتراضية TextBox
            var defaultControl = controls.FirstOrDefault(c => c.ControlsName == "TextBox");
          
            ViewBag.LstControls = new SelectList(
                controls,
                "ControlsID",
                "ControlsName",
                defaultControl?.ControlsID);

            //ViewBag.LstControlTypes = new SelectList(
            //    defaultControl?.IncubationWorkshopControlsTypes ?? new List<IncubationWorkshopControlsType>(),
            //    "ControlTypeID", "ControlTypeName");
            // تحميل أنواع الإدخال بناءً على `ControlID`
            ViewBag.LstControlTypes = new SelectList(repository.GetQuery<IncubationWorkshopControlsType>(), "ControlTypeID", "ControlTypeName");

            //if (defaultControl != null)
            //{
            //    ViewBag.LstControlTypes = new SelectList(
            //        repository.GetQuery<IncubationWorkshopControlsType>()
            //            .Where(ct => ct.ControlsID == defaultControl.ControlsID),
            //        "ControlTypeID", "ControlTypeName");
            //}
            //else
            //{
            //    ViewBag.LstControlTypes = new SelectList(new List<IncubationWorkshopControlsType>(), "ControlTypeID", "ControlTypeName");
            //}

            ViewBag.LstLookupTransTypes = new SelectList(
                repository.GetAll<IncubationWorkshopBLTransactionsType>(),
                "TransTypeID", "TransTypeName");

            //ViewBag.LstLookupTransFields = new SelectList(
            //    repository.GetQuery<IncubationWorkshopBLTransactions>(t => t.WorkshopID == workshopID),
            //    "TransID", "FieldName");

        }
        private IncubationWorkshopBLTransactions CreateNewTransaction(IWSBLTransactionsVM item, Guid? workshopID, Guid transTypeID)
        {
            var newTrans = new IncubationWorkshopBLTransactions
            {
                TransID = Guid.NewGuid(),
                TransTypeID = transTypeID,
                ControlTypeID = Guid.Parse(item.ControlTypeID),
                FieldNameEn = item.FieldNameEn,
                FieldNameAr = item.FieldNameAr,
                ViewList_Display = item.ViewList_Display,
                IsRequired = item.IsRequired.ToString()  ,               
                RankNumber = int.Parse(item.RankNumber),
                IncubationWorkshopID = workshopID ?? Guid.Empty,
                Options = new List<IncubationWorkshopControlOptions>()
            };

            // إضافة الخيارات إذا كان نوع التحكم هو Checkbox
            if (item.ControlTypeID == "e1de6bd1-6ffa-41a1-b276-d9f3a47f0c3a" || item.ControlTypeID == "76da194e-c139-4db6-865b-f1ea0d546a91" && item.Options != null && item.Options.Any())
            {
                foreach (var option in item.Options)
                {
                    Console.WriteLine($"الخيار: {option.OptionNameEn} - {option.OptionNameAr}");
                    newTrans.Options.Add(new IncubationWorkshopControlOptions
                    {
                        OptionID = Guid.NewGuid(),
                        OptionNameEn = option.OptionNameEn,
                        OptionNameAr = option.OptionNameAr,
                        TransID = newTrans.TransID
                    });
                }
            }

            return newTrans;
        }


 private void UpdateExistingTransaction(
    IncubationWorkshopBLTransactions trans,
    IWSBLTransactionsVM item,
    Guid? workshopID,
    Guid transTypeID)
        {
            trans.FieldNameEn = item.FieldNameEn;
            trans.FieldNameAr = item.FieldNameAr;
            trans.IsRequired = item.IsRequired;
            trans.RankNumber = int.Parse(item.RankNumber);
            trans.ViewList_Display = item.ViewList_Display;
            trans.IncubationWorkshopID = workshopID ?? Guid.Empty;
            trans.TransTypeID = transTypeID;
            // trans.ControlTypeID = Guid.Parse(item.ControlTypeID); // لم يتم تفعيله، حسب الحاجة

            // التحقق مما إذا كان نوع التحكم هو Checkbox
            if (trans.ControlTypeID.ToString() == "e1de6bd1-6ffa-41a1-b276-d9f3a47f0c3a" || trans.ControlTypeID.ToString() == "76da194e-c139-4db6-865b-f1ea0d546a91")
            {
                if (item.Options != null && item.Options.Any())
                {
                    foreach (var option in item.Options)
                    {
                        // البحث عن الخيار الموجود بنفس الاسم
                        var existingOption = trans.Options.FirstOrDefault(o =>
                            o.OptionNameEn == option.OptionNameEn &&
                            o.OptionNameAr == option.OptionNameAr);

                        if (existingOption != null)
                        {
                            // تحديث الخيار الموجود
                            existingOption.OptionNameEn = option.OptionNameEn;
                            existingOption.OptionNameAr = option.OptionNameAr;
                        }
                        else
                        {
                            // إضافة خيار جديد إذا لم يكن موجودًا
                            trans.Options.Add(new IncubationWorkshopControlOptions
                            {
                                OptionID = Guid.NewGuid(),
                                OptionNameEn = option.OptionNameEn,
                                OptionNameAr = option.OptionNameAr,
                                TransID = trans.TransID
                            });
                        }
                    }

                    // إزالة الخيارات التي لم تعد موجودة في المدخلات
                    var optionsToRemove = trans.Options
                        .Where(o => !item.Options.Any(opt =>
                            opt.OptionNameEn == o.OptionNameEn && opt.OptionNameAr == o.OptionNameAr))
                        .ToList();

                    foreach (var option in optionsToRemove)
                    {
                        trans.Options.Remove(option); // إزالة من الكائن
                        repository.Delete(option); // حذف من قاعدة البيانات
                    }
                }
            }
        }

        //  private void UpdateExistingTransaction(
        //IncubationWorkshopBLTransactions trans,
        //IWSBLTransactionsVM item,
        //Guid? workshopID,
        //Guid transTypeID)
        //  {
        //      trans.FieldNameEn = item.FieldNameEn;
        //      trans.FieldNameAr = item.FieldNameAr;
        //      trans.IsRequired = item.IsRequired;
        //      trans.RankNumber = int.Parse(item.RankNumber);
        //      trans.ViewList_Display = item.ViewList_Display;
        //      trans.IncubationWorkshopID = workshopID ?? Guid.Empty;
        //      trans.TransTypeID = transTypeID;
        //      //trans.ControlTypeID = Guid.Parse(item.ControlTypeID);

        //      // تحديث الخيارات إذا كان نوع التحكم هو Checkbox
        //      if (trans.ControlTypeID.ToString() == "e1de6bd1-6ffa-41a1-b276-d9f3a47f0c3a")
        //      {
        //          // حذف الخيارات القديمة
        //          var existingOptions = trans.Options.ToList();
        //          foreach (var option in existingOptions)
        //          {
        //              repository.Delete(option);  // حذف من قاعدة البيانات
        //          }

        //          // إعادة إضافة الخيارات الجديدة
        //          if (item.Options != null && item.Options.Any())
        //          {
        //              foreach (var option in item.Options)
        //              {
        //                  trans.Options.Add(new IncubationWorkshopControlOptions
        //                  {
        //                      OptionID = Guid.NewGuid(),
        //                      OptionNameEn = option.OptionNameEn,
        //                      OptionNameAr = option.OptionNameAr,
        //                      TransID = trans.TransID
        //                  });
        //              }
        //          }
        //      }
        //  }



        [HttpGet]
        public ActionResult AppendLst(bool? IsNew, Guid workshopID)
        {
            ViewBag.IsNew = IsNew ?? false;

            // جلب عناصر التحكم
            var controls = repository.GetAll<IncubationWorkshopControls>().ToList();
            if (!controls.Any())
            {
                ModelState.AddModelError("", "لا يوجد عناصر مرتبطة ");
                return PartialView("_LstTransaction", new IWSBLTransactionsVM());
            }

            // تعيين القيمة الافتراضية TextBox
            var defaultControl = controls.FirstOrDefault(c => c.ControlsName == "TextBox");

            var LstTransaction = new IWSBLTransactionsVM()
            {
                TransID = string.Empty,
                WorkshopID = workshopID,
                TransTypeID = "2655d2b1-90d2-4c17-b9e7-a0448cd8cb67",
                FieldNameEn = string.Empty,
                FieldNameAr = string.Empty,
                RankNumber = string.Empty,
                ControlID = defaultControl?.ControlsID.ToString() ?? Guid.Empty.ToString(),
                ControlTypeID = defaultControl?.IncubationWorkshopControlsTypes.FirstOrDefault()?.ControlTypeID.ToString() ?? Guid.Empty.ToString(),
                Options = new List<IncubationWorkshopControlOptionVM>()
            };


            ViewBag.LstRequired = new SelectList(new List<SelectListItem>
    {
        new SelectListItem { Value = "true", Text = "Yes" },
        new SelectListItem { Value = "false", Text = "No" }
    }, "Value", "Text", "true");

            ViewBag.LstControls = new SelectList(
                controls,
                "ControlsID",
                "ControlsName",
                defaultControl?.ControlsID);

            ViewBag.LstControlTypes = new SelectList(
                defaultControl?.IncubationWorkshopControlsTypes ?? new List<IncubationWorkshopControlsType>(),
                "ControlTypeID", "ControlTypeName");

            ViewBag.LstLookupTransTypes = new SelectList(
                repository.GetAll<IncubationWorkshopBLTransactionsType>(),
                "TransTypeID", "TransTypeName");

            ViewBag.LstLookupTransFields = new SelectList(
                repository.GetQuery<IncubationWorkshopBLTransactions>(t => t.IncubationWorkshopID == workshopID),
                "TransID", "FieldName");

            return PartialView("_LstTransaction", LstTransaction);
        }

        //[HttpGet]
        //public ActionResult GetControlTypes(Guid Control)
        //{
        //    if (Control != Guid.Empty)
        //    {
        //        var controlTypes = repository.GetQuery<IncubationWorkshopControlsType>()
        //            .Where(t => t.ControlsID == Control)
        //            .Select(n => new
        //            {
        //                Value = n.ControlTypeID.ToString(),
        //                Text = n.ControlTypeName
        //            }).ToList();

        //        return Json(controlTypes, JsonRequestBehavior.AllowGet);
        //    }
        //    return Json(new List<SelectListItem>(), JsonRequestBehavior.AllowGet);
        //}

        [HttpGet]
        public ActionResult GetControlTypes(Guid Control)
        {
            try
            {
                if (Control == Guid.Empty)
                {
                    return Json(new { success = false, message = "المعرف المرسل غير صالح." }, JsonRequestBehavior.AllowGet);
                }

                // جلب أنواع التحكم المرتبطة
                var controlTypes = repository.GetQuery<IncubationWorkshopControlsType>()
                    .Where(t => t.ControlsID == Control)
                    .Select(n => new
                    {
                        Value = n.ControlTypeID.ToString(),
                        Text = n.ControlTypeName
                    }).ToList();

                if (controlTypes.Count == 0)
                {
                    return Json(new { success = false, message = "لم يتم العثور على أنواع مرتبطة بهذا التحكم." }, JsonRequestBehavior.AllowGet);
                }

                // إرجاع الأنواع بنجاح
                return Json(new { success = true, data = controlTypes }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // التعامل مع الأخطاء غير المتوقعة
                return Json(new { success = false, message = $"حدث خطأ: {ex.Message}" }, JsonRequestBehavior.AllowGet);
            }
        }




        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                repository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}