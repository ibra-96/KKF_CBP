using System;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;

using AlphaPeople.Repository;
using AlfaPeople.KingKhalidFoundation.Data;
using AlfaPeople.KingKhalidFoundation.Data.Model;
using AlfaPeople.KingKhaledFoundation.Admin.Web.Models;

namespace AlfaPeople.KingKhaledFoundation.Admin.Web.Controllers
{
    [Authorize(Roles = "Admin, CB Manager, CB Supervisor, CB Analyst")]
    public class IncubationWorkshopPostImpactTransactionsController : BaseController
    {
        private readonly IRepository repository;

      
        private const string POST_IMPACT_TYPE_NAME = "IncubationWorkshopPostImpactTransactions";

        public IncubationWorkshopPostImpactTransactionsController()
        {
            repository = new Repository(new KingkhaledFoundationDB());
        }

      
        [HttpGet]
        public ActionResult ManagePostImpact(Guid? workshopID)
        {
            PrepareViewBags();

            // قائمة ورش العمل
            var workshops = repository.GetQuery<IncubationWorkshop>()
                .Where(w => !w.IsDeleted)
                .Select(w => new SelectListItem
                {
                    Value = w.IncubationWorkshopID.ToString(),
                    Text = w.Name
                }).ToList();

            // جلب/إنشاء نوع النموذج (Post Impact)
            var transType = repository.FindOne<IncubationWorkshopBLTransactionsType>(t => t.TransTypeName == POST_IMPACT_TYPE_NAME);

            if (transType == null)
            {
                transType = new IncubationWorkshopBLTransactionsType()
                {
                    TransTypeID = Guid.NewGuid(),
                    TransTypeName = POST_IMPACT_TYPE_NAME,
                    IsMasterData = false
                };
                repository.Add(transType);
                repository.UnitOfWork.SaveChanges();
            }

            var model = new IWSBLTransactionsTypeVM
            {
                TransTypeID = transType.TransTypeID,
                TransTypeName = transType.TransTypeName,
                IsMasterData = transType.IsMasterData.ToString(),

                Workshops = workshops,
                WorkshopID = workshopID,

                LstTransaction = new List<IWSBLTransactionsVM>()
            };

            // جلب الأسئلة الخاصة بهذه الورشة + هذا النوع
            if (workshopID.HasValue)
            {
                var transactions = repository.GetQuery<IncubationWorkshopBLTransactions>()
                    .Where(t => t.IncubationWorkshopID == workshopID.Value && t.TransTypeID == transType.TransTypeID)
                    .OrderBy(t => t.RankNumber)
                    .Include(t => t.Options)
                    .ToList();

                foreach (var item in transactions)
                {
                    model.LstTransaction.Add(new IWSBLTransactionsVM
                    {
                        TransID = item.TransID.ToString(),
                        TransTypeID = item.TransTypeID.ToString(),
                        FieldNameEn = item.FieldNameEn,
                        FieldNameAr = item.FieldNameAr,
                        RankNumber = item.RankNumber.ToString(),
                        IsRequired = item.IsRequired,
                        ViewList_Display = item.ViewList_Display,
                        WorkshopID = workshopID,

                        ControlTypeID = item.ControlTypeID.ToString(),

                        Options = (item.Options != null)
                            ? item.Options.Select(o => new IncubationWorkshopControlOptionVM
                            {
                                OptionID = o.OptionID,
                                OptionNameEn = o.OptionNameEn,
                                OptionNameAr = o.OptionNameAr,
                                TransID = o.TransID
                            }).ToList()
                            : new List<IncubationWorkshopControlOptionVM>()
                    });
                }
            }

            // AJAX: يرجع القائمة كـ Partial
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_LstTransactionList", model.LstTransaction);
            }

           
          
            return View("ManagePostImpact", model);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManagePostImpact(IWSBLTransactionsTypeVM model)
        {
            PrepareViewBags();

          
            model.Workshops = repository.GetQuery<IncubationWorkshop>()
                .Where(w => !w.IsDeleted)
                .Select(w => new SelectListItem
                {
                    Value = w.IncubationWorkshopID.ToString(),
                    Text = w.Name
                }).ToList();

            try
            {
                if (!model.WorkshopID.HasValue)
                {
                    ViewBag.err = "الرجاء اختيار ورشة عمل.";
                    return View("ManagePostImpact", model);
                }

                if (model.LstTransaction == null || !model.LstTransaction.Any())
                {
                    ViewBag.err = "يجب إضافة حقل واحد على الأقل.";
                    return View("ManagePostImpact", model);
                }

                var transType = repository.GetQuery<IncubationWorkshopBLTransactionsType>()
                    .Include(t => t.IncubationWorkshopBLTrans)
                    .FirstOrDefault(t => t.TransTypeID == model.TransTypeID);

                if (transType == null)
                {
                    ViewBag.err = "تعذر العثور على نوع النموذج.";
                    return View("ManagePostImpact", model);
                }

                transType.TransTypeName = model.TransTypeName;
                transType.IsMasterData = bool.Parse(model.IsMasterData);

                foreach (var item in model.LstTransaction)
                {
                    // جديد
                    if (string.IsNullOrWhiteSpace(item.TransID))
                    {
                        var newTrans = CreateNewTransaction(item, model.WorkshopID, model.TransTypeID);
                        transType.IncubationWorkshopBLTrans.Add(newTrans);
                    }
                    else
                    {
                        var existingTrans = transType.IncubationWorkshopBLTrans
                            .FirstOrDefault(t => t.TransID.ToString() == item.TransID);

                        if (existingTrans != null)
                        {
                            UpdateExistingTransaction(existingTrans, item, model.WorkshopID, model.TransTypeID);
                        }
                    }
                }

                repository.UnitOfWork.SaveChanges();
                TempData["SuccessMessage"] = "تم حفظ نموذج قياس الأثر البعدي بنجاح";
                return RedirectToAction("ManagePostImpact", new { workshopID = model.WorkshopID });
            }
            catch (Exception ex)
            {
                ViewBag.err = $"حدث خطأ أثناء الحفظ: {ex.Message}";
                return View("ManagePostImpact", model);
            }
        }

   
        [HttpGet]
        public ActionResult AppendLst(bool? IsNew, Guid workshopID, Guid transTypeID)
        {
            ViewBag.IsNew = IsNew ?? false;
            PrepareViewBags();

            var controls = repository.GetAll<IncubationWorkshopControls>().ToList();
            var defaultControl = controls.FirstOrDefault(c => c.ControlsName == "TextBox");

            var vm = new IWSBLTransactionsVM()
            {
                TransID = string.Empty,
                WorkshopID = workshopID,
                TransTypeID = transTypeID.ToString(),
                FieldNameEn = string.Empty,
                FieldNameAr = string.Empty,
                RankNumber = "1",
                ControlID = defaultControl?.ControlsID.ToString() ?? Guid.Empty.ToString(),
                ControlTypeID = defaultControl?.IncubationWorkshopControlsTypes.FirstOrDefault()?.ControlTypeID.ToString() ?? Guid.Empty.ToString(),
                Options = new List<IncubationWorkshopControlOptionVM>()
            };

          
            ViewBag.LstLookupTransFields = new SelectList(
                repository.GetQuery<IncubationWorkshopBLTransactions>(t => t.IncubationWorkshopID == workshopID && t.TransTypeID == transTypeID),
                "TransID", "FieldNameEn"
            );

            return PartialView("_LstTransaction", vm);
        }

   
        [HttpGet]
        public ActionResult GetControlTypes(Guid Control)
        {
            if (Control == Guid.Empty)
                return Json(new { success = false, message = "المعرف غير صالح" }, JsonRequestBehavior.AllowGet);

            var controlTypes = repository.GetQuery<IncubationWorkshopControlsType>()
                .Where(t => t.ControlsID == Control)
                .Select(n => new
                {
                    Value = n.ControlTypeID.ToString(),
                    Text = n.ControlTypeName
                }).ToList();

            return Json(new { success = true, data = controlTypes }, JsonRequestBehavior.AllowGet);
        }

     
        private void PrepareViewBags()
        {
            ViewBag.Title = "نموذج قياس الأثر البعدي";

            ViewBag.LstRequired = new SelectList(new List<SelectListItem>
            {
                new SelectListItem { Value = "true", Text = "Yes" },
                new SelectListItem { Value = "false", Text = "No" }
            }, "Value", "Text", "true");

            var controls = repository.GetAll<IncubationWorkshopControls>().ToList();
            var defaultControl = controls.FirstOrDefault(c => c.ControlsName == "TextBox");

            ViewBag.LstControls = new SelectList(controls, "ControlsID", "ControlsName", defaultControl?.ControlsID);
            ViewBag.LstControlTypes = new SelectList(repository.GetQuery<IncubationWorkshopControlsType>(), "ControlTypeID", "ControlTypeName");

            ViewBag.LstLookupTransTypes = new SelectList(repository.GetAll<IncubationWorkshopBLTransactionsType>(), "TransTypeID", "TransTypeName");
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
                IsRequired = item.IsRequired,
                RankNumber = int.Parse(item.RankNumber),
                IncubationWorkshopID = workshopID ?? Guid.Empty,
                Options = new List<IncubationWorkshopControlOptions>()
            };

         
            bool isOptionControl =
                item.ControlTypeID == "e1de6bd1-6ffa-41a1-b276-d9f3a47f0c3a" ||
                item.ControlTypeID == "76da194e-c139-4db6-865b-f1ea0d546a91";

            if (isOptionControl && item.Options != null && item.Options.Any())
            {
                foreach (var opt in item.Options)
                {
                    newTrans.Options.Add(new IncubationWorkshopControlOptions
                    {
                        OptionID = Guid.NewGuid(),
                        OptionNameEn = opt.OptionNameEn,
                        OptionNameAr = opt.OptionNameAr,
                        TransID = newTrans.TransID
                    });
                }
            }

            return newTrans;
        }

        private void UpdateExistingTransaction(IncubationWorkshopBLTransactions trans, IWSBLTransactionsVM item, Guid? workshopID, Guid transTypeID)
        {
            trans.FieldNameEn = item.FieldNameEn;
            trans.FieldNameAr = item.FieldNameAr;
            trans.IsRequired = item.IsRequired;
            trans.RankNumber = int.Parse(item.RankNumber);
            trans.ViewList_Display = item.ViewList_Display;
            trans.IncubationWorkshopID = workshopID ?? Guid.Empty;
            trans.TransTypeID = transTypeID;

            bool isOptionControl =
                trans.ControlTypeID.ToString() == "e1de6bd1-6ffa-41a1-b276-d9f3a47f0c3a" ||
                trans.ControlTypeID.ToString() == "76da194e-c139-4db6-865b-f1ea0d546a91";

            if (!isOptionControl) return;

            if (item.Options == null) item.Options = new List<IncubationWorkshopControlOptionVM>();

          
            foreach (var opt in item.Options)
            {
                var existing = trans.Options.FirstOrDefault(o =>
                    o.OptionNameEn == opt.OptionNameEn &&
                    o.OptionNameAr == opt.OptionNameAr);

                if (existing == null)
                {
                    trans.Options.Add(new IncubationWorkshopControlOptions
                    {
                        OptionID = Guid.NewGuid(),
                        OptionNameEn = opt.OptionNameEn,
                        OptionNameAr = opt.OptionNameAr,
                        TransID = trans.TransID
                    });
                }
                else
                {
                    existing.OptionNameEn = opt.OptionNameEn;
                    existing.OptionNameAr = opt.OptionNameAr;
                }
            }

  
            var toRemove = trans.Options
                .Where(o => !item.Options.Any(x => x.OptionNameEn == o.OptionNameEn && x.OptionNameAr == o.OptionNameAr))
                .ToList();

            foreach (var r in toRemove)
            {
                trans.Options.Remove(r);
                repository.Delete(r);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) repository.Dispose();
            base.Dispose(disposing);
        }
    }
}
