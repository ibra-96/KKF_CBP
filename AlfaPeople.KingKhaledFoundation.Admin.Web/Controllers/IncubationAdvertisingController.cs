using System;
using System.IO;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using AlphaPeople.Core;
using System.Transactions;
using System.Configuration;
using AlphaPeople.Repository;
using Microsoft.AspNet.Identity;
using AlfaPeople.KingKhalidFoundation.Data;
using AlfaPeople.KingKhalidFoundation.Data.Model;
using AlfaPeople.KingKhaledFoundation.Admin.Web.Models;
using AlfaPeople.KingKhaledFoundation.Admin.Web.Helper;
using System.Collections.Generic;
using System.Data.Entity;

namespace AlfaPeople.KingKhaledFoundation.Admin.Web.Controllers
{
    [Authorize(Roles = "Admin, CB Manager, CB Supervisor, CB Analyst")]
    public class IncubationAdvertisingController : BaseController
    {
        private readonly IRepository repository;
        private readonly CommonHelper helper;
        private readonly string Logpath = ConfigurationManager.AppSettings["Log"].ToString();

        public IncubationAdvertisingController()
        {
            helper = new CommonHelper();
            repository = new Repository(new KingkhaledFoundationDB());
        }

        // GET: Grants
        public ActionResult Index(string Type)
        {
            if (Type == "Incubation")
                ViewBag.Title = App_GlobalResources.General.Incubations + " " + App_GlobalResources.General.Advertising;
            else
                ViewBag.Title = App_GlobalResources.General.Accelerations + " " + App_GlobalResources.General.Advertising;

            ViewBag.Type = Type;
            // تم إضافة شرط (IsActive == true) لاستبعاد الإعلانات المحذوفة من الظهور
            //2-25-2025
            // استبعاد الإعلانات المحذوفة من الواجهة
            var adIncubation = repository.GetQuery<IncubationAdvertising>(
                f => f.IncubationType.NameEN == Type && f.IsActive && !f.IsDeleted
            ).ToList();

            return View(adIncubation);
        }
        //2-25-2025
        //25-2-2025
        [HttpPost]
        public JsonResult DeleteAdvertising(Guid id)
        {
            try
            {
                var ad = repository.GetByKey<IncubationAdvertising>(id);
                if (ad == null)
                    return Json(new { success = false, message = "الإعلان غير موجود." });

                if (ad.IsDeleted)
                    return Json(new { success = false, message = "الإعلان محذوف مسبقاً." });

                ad.IsDeleted = true;
                ad.DeletedDate = DateTime.Now;
                ad.DeletedBy = new Guid(User.Identity.GetUserId());// وضع علامة الحذف
                ad.IsActive = false;            // جعل الإعلان غير نشط
                repository.Update(ad);
                repository.UnitOfWork.SaveChanges();

                return Json(new { success = true, message = "تم حذف الإعلان بنجاح." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "حدث خطأ: " + ex.Message });
            }
        }

        // GET: Grants/Create
        public ActionResult Create(string Type)
        {
            //6-2-2025
            // استعلام للحصول على قائمة مجالات العمل
            if (CultureHelper.CurrentCulture == 3) // اللغة العربية
            {
                ViewBag.FieldsOfWork = repository.GetQuery<CorporateFieldOfWork>(f => f.IsActive == true)
                                                  .Select(f => new { f.CorporateFieldOfWorkID, f.CorporateFieldOfWorkNameAR });
            }
            else // اللغة الإنجليزية
            {
                ViewBag.FieldsOfWork = repository.GetQuery<CorporateFieldOfWork>(f => f.IsActive == true)
                                                  .Select(f => new { f.CorporateFieldOfWorkID, f.CorporateFieldOfWorkNameEN });
            }
            if (CultureHelper.CurrentCulture == 3) // اللغة العربية
            {
                ViewBag.Regions = repository.GetQuery<Region>(r => r.IsActive == true)
                                            .Select(r => new { r.RegionID, r.RegionNameAR });
            }
            else // اللغة الإنجليزية
            {
                ViewBag.Regions = repository.GetQuery<Region>(r => r.IsActive == true)
                                            .Select(r => new { r.RegionID, r.RegionNameEN });
            }
            //

            if (string.IsNullOrWhiteSpace(Type))
                return RedirectToAction("Index", new { Type = Type });

            if (Type == "Incubation")
                ViewBag.Title = App_GlobalResources.General.Advertising + " " + App_GlobalResources.General.Incubations + " " + App_GlobalResources.General.New;
            else if (Type == "Acceleration")
                ViewBag.Title = App_GlobalResources.General.Advertising + " " + App_GlobalResources.General.Accelerations + " " + App_GlobalResources.General.New;
            else
                return RedirectToAction("Index", new { Type = Type });

            TempData["Type"] = Type;
            ViewBag.Type = Type;
            if (CultureHelper.CurrentCulture == 3)
            {
                ViewBag.IncubationModels = new MultiSelectList(repository.GetQuery<IncubationModel>(f => f.IsActive == true && f.IncubationType.NameEN == Type).ToList(), "IncubationModelID", "NameAR");
                ViewBag.FundingSource = new SelectList(repository.GetQuery<FundingSource>(f => f.IsActive == true), "FundingSourceID", "FundingSourceNameAR");
            }
            else
            {
                ViewBag.IncubationModels = new MultiSelectList(repository.GetQuery<IncubationModel>(f => f.IsActive == true && f.IncubationType.NameEN == Type), "IncubationModelID", "NameEN");
                ViewBag.FundingSource = new SelectList(repository.GetQuery<FundingSource>(f => f.IsActive == true), "FundingSourceID", "FundingSourceNameEN");
            }
            return View();
        }

        // POST: Grants/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string Type, IncubationAdvertisingVM model, List<Guid> SelectedFieldsOfWork, List<Guid> SelectedRegions)
        {
            if (string.IsNullOrWhiteSpace(Type))
                return RedirectToAction("Index", new { Type = Type });

            if (Type == "Incubation")
                ViewBag.Title = App_GlobalResources.General.New + " " + App_GlobalResources.General.Incubations + " " + App_GlobalResources.General.Advertising;
            else if (Type == "Acceleration")
                ViewBag.Title = App_GlobalResources.General.New + " " + App_GlobalResources.General.Accelerations + " " + App_GlobalResources.General.Advertising;
            else
                return RedirectToAction("Index", new { Type = Type });

            TempData.Keep();
            ViewBag.Type = Type;
            if (CultureHelper.CurrentCulture == 3) // اللغة العربية
            {
                ViewBag.FieldsOfWork = repository.GetQuery<CorporateFieldOfWork>(f => f.IsActive == true)
                                                  .Select(f => new { f.CorporateFieldOfWorkID, f.CorporateFieldOfWorkNameAR });
            }
            else // اللغة الإنجليزية
            {
                ViewBag.FieldsOfWork = repository.GetQuery<CorporateFieldOfWork>(f => f.IsActive == true)
                                                  .Select(f => new { f.CorporateFieldOfWorkID, f.CorporateFieldOfWorkNameEN });
            }
            if (CultureHelper.CurrentCulture == 3) // اللغة العربية
            {
                ViewBag.Regions = repository.GetQuery<Region>(r => r.IsActive == true)
                                            .Select(r => new { r.RegionID, r.RegionNameAR });
            }
            else // اللغة الإنجليزية
            {
                ViewBag.Regions = repository.GetQuery<Region>(r => r.IsActive == true)
                                            .Select(r => new { r.RegionID, r.RegionNameEN });
            }
            if (ModelState.IsValid)
            {

                if (model.incubationAdvertising.ISPublic != true)
                {
                    if (model.LstOfEmails != null)
                    {
                        string[] Emails = model.LstOfEmails.Trim().Split(',');
                        for (int i = 0; i < Emails.Count(); i++)
                        {
                            try
                            {
                                if (Emails[i].Trim() != "")
                                {
                                    var addr = new System.Net.Mail.MailAddress(Emails[i].Trim());
                                }
                            }
                            catch
                            {
                                if (CultureHelper.CurrentCulture == 3)
                                {
                                    ViewBag.FundingSource = new SelectList(repository.GetQuery<FundingSource>(f => f.IsActive == true), "FundingSourceID", "FundingSourceNameAR");
                                    ViewBag.IncubationModels = new MultiSelectList(repository.GetQuery<IncubationModel>(f => f.IsActive == true && f.IncubationType.NameEN == Type), "IncubationModelID", "NameAR");
                                }
                                else
                                {
                                    ViewBag.FundingSource = new SelectList(repository.GetQuery<FundingSource>(f => f.IsActive == true), "FundingSourceID", "FundingSourceNameEN");
                                    ViewBag.IncubationModels = new MultiSelectList(repository.GetQuery<IncubationModel>(f => f.IsActive == true && f.IncubationType.NameEN == Type), "IncubationModelID", "NameEN");
                                }
                                ModelState.AddModelError("invalidEmails", "Invalid Email");
                                return View(model);
                            }
                        }
                    }
                }
                var userId = User.Identity.GetUserId();
                try
                {
                    //6-2-2025
                    var workshopID = Guid.NewGuid();
                    model.incubationAdvertising.IncubationAdID = Guid.NewGuid();
                    model.incubationAdvertising.IncubationTypeID = repository.FindOne<IncubationType>(f => f.NameEN == Type).IncubationTypeID;
                    model.incubationAdvertising.IsActive = true;
                    //2-25-2025
                    model.incubationAdvertising.IsDeleted = false; // تهيئة خاصية الحذف على أنها غير محذوفة بشكل افتراضي

                    if (model.incubationAdvertising.ISPublic != true)
                    {


                        if ((SelectedRegions != null && SelectedRegions.Count > 0) || (SelectedFieldsOfWork != null && SelectedFieldsOfWork.Count > 0))
                        {
                            var InvitList = repository.GetQuery<FrontendUser>(a => a.IsActive &&
                                a.CorporateApplicationForms.FirstOrDefault().Program.ProgramName == "Capacity Building" &&
                                a.CorporateApplicationForms.FirstOrDefault().CorporateApplicationStatu.ApplicantStatu.ApplicantStatusName == "Accepted");

                            // إذا تم اختيار مناطق فقط
                            if (SelectedRegions != null && SelectedRegions.Count > 0 && (SelectedFieldsOfWork == null || SelectedFieldsOfWork.Count == 0))
                            {
                                InvitList = InvitList.Where(a => a.CorporateApplicationForms.Any(f => SelectedRegions.Contains((Guid)f.RegionID)));
                            }
                            // إذا تم اختيار مجالات عمل فقط
                            else if (SelectedFieldsOfWork != null && SelectedFieldsOfWork.Count > 0 && (SelectedRegions == null || SelectedRegions.Count == 0))
                            {
                                InvitList = InvitList.Where(a => a.CorporateApplicationForms.Any(f => SelectedFieldsOfWork.Contains((Guid)f.CorporateFieldOfWorkID)));
                            }
                            // إذا تم اختيار كلاهما معًا
                            else if (SelectedRegions != null && SelectedRegions.Count > 0 && SelectedFieldsOfWork != null && SelectedFieldsOfWork.Count > 0)
                            {
                                InvitList = InvitList.Where(a => a.CorporateApplicationForms.Any(f => SelectedRegions.Contains((Guid)f.RegionID)) &&
                                                                 a.CorporateApplicationForms.Any(f => SelectedFieldsOfWork.Contains((Guid)f.CorporateFieldOfWorkID)));
                            }

                            var CapacityBuldingUser = InvitList.Select(f => f.AspNetUser).ToList();

                            for (int i = 0; i < CapacityBuldingUser.Count(); i++)
                            {
                                //20-2-2025
                                var email = CapacityBuldingUser[i].Email.Trim();
                                // البحث عن المستخدم في `FrontendUser`
                                var frontendUser = repository.GetQuery<FrontendUser>()
                                                            .FirstOrDefault(fu => fu.AspNetUser.Email == email);

                                IncubationPrivateInvitation _Private = new IncubationPrivateInvitation();
                                _Private.ID = Guid.NewGuid();
                                _Private.Email = CapacityBuldingUser[i].Email.Trim();
                               
                                _Private.IncubationAdID = model.incubationAdvertising.IncubationAdID;

                                //20-2-2025
                                _Private.Status = InvitationStatus.pending;//  تعيين الحالة "pending"
                                _Private.FrontendUserID = frontendUser?.UserID; //  تعيين `FrontendUserID` إذا كان المستخدم موجودًا
                                                                                //24-2-2025
                                _Private.CreatedDate = DateTime.Now;// تعيين التاريخ والوقت الحالي
                                repository.Add(_Private);

                                MailHelper mailHelper = new MailHelper();
                                mailHelper.ToEmail = CapacityBuldingUser[i].Email.Trim();
                                mailHelper.Subject = Type == "Incubation" ? "بدء التقديم على مشروع الاحتضان الكامل" : "بدء التقديم على مشروع الاحتضان الجزئي";
                                mailHelper.IsHtml = true;
                                if (Type == "Incubation")
                                {
                                    mailHelper.Body = "المكرمين/ المنظمات غير الربحية                         الموقرين<br/>"
                                       + "السلام عليكم ورحمة الله وبركاته، <br/>"
                                       + "تهديكم مؤسسة الملك خالد أطيب التحيات، <br/> ونحيطكم علماً بأنه تم فتح باب التقديم على مشروع الاحتضان الكامل ضمن مشاريع برنامج بناء القدرات.<br/> نأمل منكم التكرم بزيارة البوابة الإلكترونية وتعبئة نموذج الالتحاق بالمشروع في موعد أقصاه   :  "
                                       + model.incubationAdvertising.EndDate.ToString("dd/MM/yyyy") + "<br/>"
                                       + "مع تمنياتنا لكم بالتوفيق،<br/>"
                                       + " برنامج بناء القدرات. <br/>";
                                }
                                else if (Type == "Acceleration")
                                {
                                    mailHelper.Body = "المكرمين/ المنظمات غير الربحية                         الموقرين<br/>"
                                     + "السلام عليكم ورحمة الله وبركاته، <br/>"
                                     + "تهديكم مؤسسة الملك خالد أطيب التحيات،<br/> ونحيطكم علماً بأنه تم فتح باب التقديم على مشروع الاحتضان الجزئي ضمن مشاريع برنامج بناء القدرات.<br/> نأمل منكم التكرم بزيارة البوابة الإلكترونية وتعبئة نموذج الالتحاق بالمشروع في موعد أقصاه   :  "
                                     + model.incubationAdvertising.EndDate.ToString("dd/MM/yyyy") + "<br/>"
                                     + "مع تمنياتنا لكم بالتوفيق،<br/>"
                                     + " برنامج بناء القدرات. <br/>";
                                }
                                var fundingSource = repository.GetByKey<FundingSource>(model.incubationAdvertising.FundingSourceID);
                                if (fundingSource.UseCustomThemes)
                                {
                                    mailHelper.Send($"?partner={fundingSource.Nickname}");
                                }
                                else
                                {

                                    mailHelper.Send("");
                                }
                            }
                        }




                        if (model.LstOfEmails != null)
                        {
                            string[] Emails = model.LstOfEmails.Split(',');
                            for (int i = 0; i < Emails.Count(); i++)
                            { //20-2-2025
                                var email = Emails[i].Trim();
                                // البحث عن المستخدم في `FrontendUser`
                                var frontendUser = repository.GetQuery<FrontendUser>()
                                                            .FirstOrDefault(fu => fu.AspNetUser.Email == email);
                                if (Emails[i].Trim() != "")
                                {
                                    IncubationPrivateInvitation _Private = new IncubationPrivateInvitation();
                                    _Private.ID = Guid.NewGuid();
                                    _Private.Email = Emails[i].Trim();
                                    _Private.IncubationAdID = model.incubationAdvertising.IncubationAdID;
                                    //20-2-2025
                                    _Private.Status = InvitationStatus.pending;//  تعيين الحالة "pending"
                                    _Private.FrontendUserID = frontendUser?.UserID; //  تعيين `FrontendUserID` إذا كان المستخدم موجودًا
                                    _Private.CreatedDate = DateTime.Now;                                         //
                                    repository.Add(_Private);

                                    string ff = Emails[i].Trim();
                                    MailHelper mailHelper = new MailHelper();
                                    mailHelper.ToEmail = Emails[i].Trim();
                                    mailHelper.Subject = Type == "Incubation" ? "بدء التقديم على مشروع الاحتضان الكامل" : "بدء التقديم على مشروع الاحتضان الجزئي";
                                    mailHelper.IsHtml = true;
                                    if (Type == "Incubation")
                                    {
                                        mailHelper.Body = "المكرمين/ المنظمات غير الربحية                         الموقرين<br/>"
                                           + "السلام عليكم ورحمة الله وبركاته، <br/>"
                                           + "تهديكم مؤسسة الملك خالد أطيب التحيات، <br/> ونحيطكم علماً بأنه تم فتح باب التقديم على مشروع الاحتضان الكامل ضمن مشاريع برنامج بناء القدرات.<br/> نأمل منكم التكرم بزيارة البوابة الإلكترونية وتعبئة نموذج الالتحاق بالمشروع في موعد أقصاه   :  "
                                           + model.incubationAdvertising.EndDate.ToString("dd/MM/yyyy") + "<br/>"
                                           + "مع تمنياتنا لكم بالتوفيق،<br/>"
                                           + " برنامج بناء القدرات. <br/>";
                                    }
                                    else if (Type == "Acceleration")
                                    {
                                        mailHelper.Body = "المكرمين/ المنظمات غير الربحية                         الموقرين<br/>"
                                         + "السلام عليكم ورحمة الله وبركاته، <br/>"
                                         + "تهديكم مؤسسة الملك خالد أطيب التحيات،<br/> ونحيطكم علماً بأنه تم فتح باب التقديم على مشروع الاحتضان الجزئي ضمن مشاريع برنامج بناء القدرات.<br/> نأمل منكم التكرم بزيارة البوابة الإلكترونية وتعبئة نموذج الالتحاق بالمشروع في موعد أقصاه   :  "
                                         + model.incubationAdvertising.EndDate.ToString("dd/MM/yyyy") + "<br/>"
                                         + "مع تمنياتنا لكم بالتوفيق،<br/>"
                                         + " برنامج بناء القدرات. <br/>";
                                    }
                                    var fundingSource = repository.GetByKey<FundingSource>(model.incubationAdvertising.FundingSourceID);
                                    if (fundingSource.UseCustomThemes)
                                    {
                                        mailHelper.Send($"?partner={fundingSource.Nickname}");
                                    }
                                    else
                                    {

                                        mailHelper.Send("");
                                    }
                                }
                            }
                        }

                    }
                    else
                    {
                        var CapacityBuldingUsers = repository.Get<CorporateApplicationForm>(c => c.Program.ProgramName == "Capacity Building" && c.CorporateApplicationStatu.ApplicantStatu.ApplicantStatusName == "Accepted");
                        foreach (var CapacityBuldingUser in CapacityBuldingUsers)
                        {
                            MailHelper mailHelper = new MailHelper();
                            mailHelper.ToEmail = CapacityBuldingUser.FrontendUser.AspNetUser.Email;
                            mailHelper.Subject = Type == "Incubation" ? "بدء التقديم على مشروع الاحتضان الكامل" : "بدء التقديم على مشروع الاحتضان الجزئي";
                            mailHelper.IsHtml = true;
                            if (Type == "Incubation")
                            {
                                mailHelper.Body = "المكرمين/ المنظمات غير الربحية                         الموقرين<br/>"
                                   + "السلام عليكم ورحمة الله وبركاته، <br/>"
                                   + "تهديكم مؤسسة الملك خالد أطيب التحيات، <br/> ونحيطكم علماً بأنه تم فتح باب التقديم على مشروع الاحتضان الكامل ضمن مشاريع برنامج بناء القدرات.<br/> نأمل منكم التكرم بزيارة البوابة الإلكترونية وتعبئة نموذج الالتحاق بالمشروع في موعد أقصاه   :  "
                                   + model.incubationAdvertising.EndDate.ToString("dd/MM/yyyy") + "<br/>"
                                   + "مع تمنياتنا لكم بالتوفيق،<br/>"
                                   + " برنامج بناء القدرات. <br/>";
                            }
                            else if (Type == "Acceleration")
                            {
                                mailHelper.Body = "المكرمين/ المنظمات غير الربحية                         الموقرين<br/>"
                                 + "السلام عليكم ورحمة الله وبركاته، <br/>"
                                 + "تهديكم مؤسسة الملك خالد أطيب التحيات،<br/> ونحيطكم علماً بأنه تم فتح باب التقديم على مشروع الاحتضان الجزئي ضمن مشاريع برنامج بناء القدرات.<br/> نأمل منكم التكرم بزيارة البوابة الإلكترونية وتعبئة نموذج الالتحاق بالمشروع في موعد أقصاه   :  "
                                 + model.incubationAdvertising.EndDate.ToString("dd/MM/yyyy") + "<br/>"
                                 + "مع تمنياتنا لكم بالتوفيق،<br/>"
                                 + " برنامج بناء القدرات. <br/>";
                            }
                            var fundingSource = repository.GetByKey<FundingSource>(model.incubationAdvertising.FundingSourceID);
                            if (fundingSource.UseCustomThemes)
                            {
                                mailHelper.Send($"?partner={fundingSource.Nickname}");
                            }
                            else
                            {
                                mailHelper.Send("");
                            }
                        }
                    }

                    using (TransactionScope scope = new TransactionScope())
                    {
                        try
                        {
                            repository.Add(model.incubationAdvertising);

                            if (model.IncubationAdvertisingModels != null)
                            {
                                for (int i = 0; i < model.IncubationAdvertisingModels.Count(); i++)
                                {
                                    if (model.IncubationAdvertisingModels[i] != null)
                                    {
                                        IncubationAdvertisingModel incubationAdModel = new IncubationAdvertisingModel();
                                        incubationAdModel.ID = Guid.NewGuid();
                                        incubationAdModel.IncubationAdID = model.incubationAdvertising.IncubationAdID;
                                        incubationAdModel.IncubationModelID = model.IncubationAdvertisingModels[i];
                                        repository.Add(incubationAdModel);
                                    }
                                }
                            }

                            if (model.files != null)
                            {

                                string FolderName = User.Identity.Name;
                                string path = Server.MapPath("~/Uploads/" + FolderName + "/");
                                if (!Directory.Exists(path))
                                {
                                    Directory.CreateDirectory(path);
                                }
                                for (int i = 0; i < model.files.Count(); i++)
                                {
                                    if (model.files[i] != null)
                                    {
                                        model.files[i].SaveAs(path + Path.GetFileName(model.files[i].FileName));
                                        IncubationAdvertisingAttachment _Attach = new IncubationAdvertisingAttachment();
                                        _Attach.AttachmentID = Guid.NewGuid();
                                        _Attach.IncubationAdID = model.incubationAdvertising.IncubationAdID;
                                        _Attach.Name = Path.GetFileName(model.files[i].FileName);
                                        _Attach.ScreenName = "Incubation Advertising";
                                        _Attach.Size = model.files[i].ContentLength.ToString();
                                        _Attach.URL = path + Path.GetFileName(model.files[i].FileName);
                                        _Attach.Type = model.files[i].ContentType;
                                        repository.Add(_Attach);
                                    }
                                }
                                ViewBag.Message = "File uploaded successfully.";
                            }

                            repository.UnitOfWork.SaveChanges();
                            scope.Complete();
                        }
                        catch (Exception)
                        {
                            scope.Dispose();
                            if (CultureHelper.CurrentCulture == 3)
                            {
                                ViewBag.FundingSource = new SelectList(repository.GetQuery<FundingSource>(f => f.IsActive == true), "FundingSourceID", "FundingSourceNameAR");
                                ViewBag.IncubationModels = new MultiSelectList(repository.GetQuery<IncubationModel>(f => f.IsActive == true && f.IncubationType.NameEN == Type), "IncubationModelID", "NameAR");
                            }
                            else
                            {
                                ViewBag.FundingSource = new SelectList(repository.GetQuery<FundingSource>(f => f.IsActive == true), "FundingSourceID", "FundingSourceNameEN");
                                ViewBag.IncubationModels = new MultiSelectList(repository.GetQuery<IncubationModel>(f => f.IsActive == true && f.IncubationType.NameEN == Type), "IncubationModelID", "NameEN");
                            }
                            return View(model);
                        }
                    }
                }
                catch (Exception)
                {
                    if (CultureHelper.CurrentCulture == 3)
                    {
                        ViewBag.FundingSource = new SelectList(repository.GetQuery<FundingSource>(f => f.IsActive == true), "FundingSourceID", "FundingSourceNameAR");
                        ViewBag.IncubationModels = new MultiSelectList(repository.GetQuery<IncubationModel>(f => f.IsActive == true && f.IncubationType.NameEN == Type), "IncubationModelID", "NameAR");
                    }
                    else
                    {
                        ViewBag.FundingSource = new SelectList(repository.GetQuery<FundingSource>(f => f.IsActive == true), "FundingSourceID", "FundingSourceNameEN");
                        ViewBag.IncubationModels = new MultiSelectList(repository.GetQuery<IncubationModel>(f => f.IsActive == true && f.IncubationType.NameEN == Type), "IncubationModelID", "NameEN");
                    }
                    return View(model);
                }
            }
            return RedirectToAction("Index", new { Type = Type });
        }


        //

        //24-2025
        [HttpGet]
        public JsonResult GetInvitationDetails(string IncubationAdID, string email)
        {
            var invitation = repository.GetQuery<IncubationPrivateInvitation>()
                                       .Where(i => i.IncubationAdID.ToString() == IncubationAdID && i.Email == email)
                                       .Select(i => new
                                       {
                                           i.ID,
                                           i.Email,
                                           OrganizationName = i.FrontendUser.CorporateApplicationForms.FirstOrDefault().Name
                                       })
                                       .FirstOrDefault();

            return Json(invitation, JsonRequestBehavior.AllowGet);
        }
        //24-2-2025
        [HttpGet]
        public JsonResult CreateNewInvitation(string IncubationAdID, string email, string Type)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(email) && !string.IsNullOrWhiteSpace(IncubationAdID))
                {
                    var IncubationAd = repository.GetByKey<IncubationAdvertising>(Guid.Parse(IncubationAdID));

                    //  التحقق مما إذا كان الإعلان منتهي الصلاحية
                    if (IncubationAd.EndDate < DateTime.Now.AddHours(-1))
                    {
                        return Json("لا يمكن إرسال الدعوة لأن فترة الإعلان قد انتهت.", JsonRequestBehavior.AllowGet);
                    }

                    //  البحث عن أي دعوة سابقة بنفس البريد وحالتها ليست ملغاة
                    var existingInvitation = IncubationAd.IncubationPrivateInvitations
                                                         .FirstOrDefault(e => e.Email == email && e.Status != InvitationStatus.cancel);

                    if (existingInvitation == null)
                    {
                        //  البحث عن المستخدم في `FrontendUser` بناءً على البريد الإلكتروني
                        var frontendUser = repository.GetQuery<FrontendUser>()
                                                    .FirstOrDefault(fu => fu.AspNetUser.Email == email);

                        //  إنشاء سجل جديد للدعوة
                        IncubationPrivateInvitation _Private = new IncubationPrivateInvitation
                        {
                            ID = Guid.NewGuid(),
                            Email = email.Trim(),
                            IncubationAdID = Guid.Parse(IncubationAdID),
                            Status = InvitationStatus.pending, //  تعيين الحالة إلى "pending"
                            FrontendUserID = frontendUser?.UserID ,//  ربط المستخدم
                            //24-2-2025
                          CreatedDate = DateTime.Now // تعيين التاريخ والوقت الحالي

                        };

                        repository.Add(_Private);
                        repository.UnitOfWork.SaveChanges();

                   
                        MailHelper mailHelper = new MailHelper
                        {
                            ToEmail = email.Trim(),
                            IsHtml = true
                        };

                        // اختيار نص البريد الإلكتروني بناءً على `Type`
                        switch (Type)
                        {
                            case "Incubation":
                                mailHelper.Subject = "بدء التقديم على مشروع الاحتضان الكامل";
                                mailHelper.Body = GenerateEmailBody("الاحتضان الكامل", IncubationAd.EndDate);
                                break;

                            case "Acceleration":
                                mailHelper.Subject = "بدء التقديم على مشروع الاحتضان الجزئي";
                                mailHelper.Body = GenerateEmailBody("الاحتضان الجزئي", IncubationAd.EndDate);
                                break;

                            default:
                                return Json("Invalid Type", JsonRequestBehavior.AllowGet);
                        }

                        var fundingSource = repository.GetByKey<FundingSource>(IncubationAd.FundingSourceID);
                        mailHelper.Send(fundingSource.UseCustomThemes ? $"?partner={fundingSource.Nickname}" : "");

                        return Json("Success", JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json("Invitation Already Exists", JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                return Json("Error: " + ex.Message, JsonRequestBehavior.AllowGet);
            }

            return Json("Invalid Data", JsonRequestBehavior.AllowGet);
        }



        //new 19-2-2025

        //[HttpGet]
        //public JsonResult CreateNewInvitation(string IncubationAdID, string email, string Type)
        //{
        //    try
        //    {
        //        if (!string.IsNullOrWhiteSpace(email) && !string.IsNullOrWhiteSpace(IncubationAdID))
        //        {
        //            var IncubationAd = repository.GetByKey<IncubationAdvertising>(Guid.Parse(IncubationAdID));
        //            //20-2-2025
        //            //  البحث عن أي دعوة سابقة بنفس البريد وحالتها ليست ملغاة
        //            var existingInvitation = IncubationAd.IncubationPrivateInvitations
        //                                                 .FirstOrDefault(e => e.Email == email && e.Status != InvitationStatus.cancel);
        //            //var existingInvitation = IncubationAd.IncubationPrivateInvitations.FirstOrDefault(e => e.Email == email);

        //            if (existingInvitation == null)
        //            {

        //                //20-2-2025
        //                // البحث عن المستخدم في `FrontendUser` بناءً على البريد الإلكتروني
        //                var frontendUser = repository.GetQuery<FrontendUser>()
        //                                            .FirstOrDefault(fu => fu.AspNetUser.Email == email);

        //                // إنشاء سجل جديد للدعوة
        //                IncubationPrivateInvitation _Private = new IncubationPrivateInvitation
        //                {
        //                    ID = Guid.NewGuid(),
        //                    Email = email.Trim(),
        //                    IncubationAdID = Guid.Parse(IncubationAdID),
        //                    Status = InvitationStatus.pending, //  تعيين الحالة إلى "pending"
        //                    FrontendUserID = frontendUser?.UserID //  ربط المستخدم إذا وجد
        //                };

        //                repository.Add(_Private);
        //                repository.UnitOfWork.SaveChanges();

        //                // إعداد البريد الإلكتروني
        //                MailHelper mailHelper = new MailHelper
        //                {
        //                    ToEmail = email.Trim(),
        //                    IsHtml = true
        //                };

        //                // اختيار نص البريد الإلكتروني بناءً على `Type`
        //                switch (Type)
        //                {
        //                    case "Incubation":
        //                        mailHelper.Subject = "بدء التقديم على مشروع الاحتضان الكامل";
        //                        mailHelper.Body = GenerateEmailBody("الاحتضان الكامل", IncubationAd.EndDate);
        //                        break;

        //                    case "Acceleration":
        //                        mailHelper.Subject = "بدء التقديم على مشروع الاحتضان الجزئي";
        //                        mailHelper.Body = GenerateEmailBody("الاحتضان الجزئي", IncubationAd.EndDate);
        //                        break;

        //                    default:
        //                        return Json("Invalid Type", JsonRequestBehavior.AllowGet);
        //                }

        //                var fundingSource = repository.GetByKey<FundingSource>(IncubationAd.FundingSourceID);
        //                mailHelper.Send(fundingSource.UseCustomThemes ? $"?partner={fundingSource.Nickname}" : "");

        //                return Json("Success", JsonRequestBehavior.AllowGet);
        //            }
        //            else
        //            {
        //                return Json("Invitation Already Exists", JsonRequestBehavior.AllowGet);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json("Error: " + ex.Message, JsonRequestBehavior.AllowGet);
        //    }

        //    return Json("Invalid Data", JsonRequestBehavior.AllowGet);
        //}


        private string GenerateEmailBody(string incubationType, DateTime endDate)
        {
            return $"<p>المكرمين/ المنظمات غير الربحية الموقرين،</p>"
                   + "<p>السلام عليكم ورحمة الله وبركاته،</p>"
                   + "<p>تهديكم مؤسسة الملك خالد أطيب التحيات،</p>"
                   + $"<p>نحيطكم علماً بأنه تم فتح باب التقديم على مشروع {incubationType} ضمن مشاريع برنامج بناء القدرات.</p>"
                   + $"<p>نأمل منكم التكرم بزيارة البوابة الإلكترونية وتعبئة نموذج الالتحاق بالمشروع في موعد أقصاه: <strong>{endDate:dd/MM/yyyy}</strong></p>"
                   + "<p>مع تمنياتنا لكم بالتوفيق،</p>"
                   + "<p>برنامج بناء القدرات.</p>";
        }



        //end


        //20-2-2025
        [HttpPost]
        public JsonResult CancelInvitation(Guid invitationId)
        {
            try
            {
                // استرجاع الدعوة من قاعدة البيانات
                var invitation = repository.GetByKey<IncubationPrivateInvitation>(invitationId);
                if (invitation == null)
                    return Json(new { success = false, message = "الدعوة غير موجودة" });

                // التحقق مما إذا كانت الدعوة قد ألغيت مسبقًا
                if (invitation.Status == InvitationStatus.cancel)
                    return Json(new { success = false, message = "تم إلغاء الدعوة مسبقًا" });

                // تحديث حالة الدعوة إلى "cancel"
                invitation.Status = InvitationStatus.cancel;
                //  تحديث تاريخ الإلغاء4-3-2025
                invitation.UpdatedDate = DateTime.Now;
                repository.Update(invitation);
                repository.UnitOfWork.SaveChanges();

                // جلب تفاصيل الإعلان المرتبط بالدعوة
                var incubationAd = repository.GetByKey<IncubationAdvertising>(invitation.IncubationAdID);
                if (incubationAd == null)
                    return Json(new { success = false, message = "لم يتم العثور على الإعلان المرتبط" });

                // إعداد البريد الإلكتروني
                MailHelper mailHelper = new MailHelper
                {
                    ToEmail = invitation.Email,
                    Subject = $"إلغاء دعوتكم للتقديم على {incubationAd.Name}",
                    Body = GenerateCancelEmailBody(incubationAd.Name, incubationAd.EndDate),
                    IsHtml = true
                };

                // إرسال البريد الإلكتروني
                mailHelper.Send("");

                return Json(new { success = true, message = "تم إلغاء الدعوة بنجاح وتم إرسال بريد إلكتروني بالتفاصيل" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "حدث خطأ أثناء الإلغاء: " + ex.Message });
            }
        }
        private string GenerateCancelEmailBody(string incubationName, DateTime endDate)
        {
            return $"<p>المكرمين/ المنظمات غير الربحية الموقرين،</p>"
                   + "<p>السلام عليكم ورحمة الله وبركاته،</p>"
                   + $"<p>نود إبلاغكم بأنه تم <strong>إلغاء</strong> الدعوة الخاصة بكم للتقديم على مشروع <strong>{incubationName}</strong>.</p>"
                   + $"<p>كان من المقرر أن ينتهي التقديم لهذا المشروع في: <strong>{endDate:dd/MM/yyyy}</strong>.</p>"
                   + "<p>إذا كان لديكم أي استفسارات، يرجى التواصل معنا.</p>"
                   + "<p>مع تمنياتنا لكم بالتوفيق،</p>"
                   + "<p><strong>برنامج بناء القدرات.</strong></p>";
        }


        public ActionResult Edit(Guid? id, string Type)
        {
            if (id == null || string.IsNullOrWhiteSpace(Type))
                return RedirectToAction("Index", new { Type = Type });

            //var incubationAD = repository.GetByKey<IncubationAdvertising>(id);

            //20-2-2025

            //         var incubationAD = repository.GetQuery<IncubationAdvertising>()
            //.Include(i => i.IncubationPrivateInvitations.Select(inv => inv.FrontendUser.CorporateApplicationForms))
            //.FirstOrDefault(i => i.IncubationAdID == id);
            //var incubationAD = repository.GetQuery<IncubationAdvertising>()
            // .Include(i => i.IncubationPrivateInvitations)
            // .FirstOrDefault(i => i.IncubationAdID == id);
            //24-2-2025
              var incubationAD = repository.GetQuery<IncubationAdvertising>()
                .Include(i => i.IncubationPrivateInvitations) // تحميل الدعوات
                .FirstOrDefault(i => i.IncubationAdID == id);

            if (incubationAD != null)
            {
                // تصفية الدعوات لتشمل أحدث دعوة فقط لكل منظمة
                incubationAD.IncubationPrivateInvitations = incubationAD.IncubationPrivateInvitations
                    .GroupBy(inv => inv.Email) // تجميع حسب البريد الإلكتروني لضمان أحدث دعوة فقط لكل منظمة
                    .Select(g => g.OrderByDescending(inv => inv.CreatedDate).FirstOrDefault()) // اختيار الأحدث
                    .Where(inv => inv.Status != InvitationStatus.cancel) // استبعاد الدعوات الملغاة
                    .ToList();
            }

            // تحميل بيانات المستخدم والجمعية بشكل منفصل
            foreach (var invitation in incubationAD.IncubationPrivateInvitations)
            {
                invitation.FrontendUser = repository.GetQuery<FrontendUser>()
                    .Include(fu => fu.CorporateApplicationForms)
                    .FirstOrDefault(fu => fu.AspNetUser.Email == invitation.Email);
            }
            //

            if (incubationAD == null)
                return HttpNotFound();

            TempData["Type"] = Type;
            ViewBag.Type = Type;

            if (Type == "Incubation")
                ViewBag.Title = App_GlobalResources.General.Advertising + " " + incubationAD.Name + " " + App_GlobalResources.General.Update;
            else if (Type == "Acceleration")
                ViewBag.Title = App_GlobalResources.General.Advertising + " " + incubationAD.Name + " " + App_GlobalResources.General.Update;
            else
                return RedirectToAction("Index", new { Type = Type });

            if (CultureHelper.CurrentCulture == 3)
            {
                ViewBag.IncubationModels = new MultiSelectList(repository.GetQuery<IncubationModel>(f => f.IsActive == true && f.IncubationType.NameEN == Type), "IncubationModelID", "NameAR", incubationAD.IncubationAdvertisingModels.Select(a => a.IncubationModelID).ToArray());
                ViewBag.FundingSource = new SelectList(repository.GetQuery<FundingSource>(f => f.IsActive == true), "FundingSourceID", "FundingSourceNameAR", incubationAD.FundingSourceID);
            }
            else
            {
                ViewBag.IncubationModels = new MultiSelectList(repository.GetQuery<IncubationModel>(f => f.IsActive == true && f.IncubationType.NameEN == Type), "IncubationModelID", "NameEN", incubationAD.IncubationAdvertisingModels.Select(a => a.IncubationModelID).ToArray());
                ViewBag.FundingSource = new SelectList(repository.GetQuery<FundingSource>(f => f.IsActive == true), "FundingSourceID", "FundingSourceNameEN", incubationAD.FundingSourceID);
            }

            ViewBag.StartDate = incubationAD.StartDate.ToString("yyyy-MM-dd");
            ViewBag.EndDate = incubationAD.EndDate.ToString("yyyy-MM-dd");

            //20-2-2025

           
            //  استبعاد الدعوات الملغاة قبل إرسالها للـ View
            incubationAD.IncubationPrivateInvitations = incubationAD.IncubationPrivateInvitations
                                                       .Where(i => i.Status != InvitationStatus.cancel)
                                                       .ToList();
            return View(new IncubationAdvertisingVM() { incubationAdvertising = incubationAD });
        }

        // POST: Grants/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string Type, IncubationAdvertisingVM model)
        {
            if (string.IsNullOrWhiteSpace(Type))
                return RedirectToAction("Index", new { Type = Type });

            if (Type == "Incubation")
                ViewBag.Title = App_GlobalResources.General.Update + " " + model.incubationAdvertising.Name + " " + App_GlobalResources.General.Advertising;
            else if (Type == "Acceleration")
                ViewBag.Title = App_GlobalResources.General.Update + " " + model.incubationAdvertising.Name + " " + App_GlobalResources.General.Advertising;
            else
                return RedirectToAction("Index", new { Type = Type });

            TempData.Keep();
            ViewBag.Type = Type;

            if (ModelState.IsValid)
            {
                repository.Update(model.incubationAdvertising);
                repository.Delete<IncubationAdvertisingModel>(m => m.IncubationAdID == model.incubationAdvertising.IncubationAdID);

                if (model.IncubationAdvertisingModels != null)
                {
                    for (int i = 0; i < model.IncubationAdvertisingModels.Count(); i++)
                    {
                        if (model.IncubationAdvertisingModels[i] != null)
                        {
                            IncubationAdvertisingModel incubationAdModel = new IncubationAdvertisingModel();
                            incubationAdModel.ID = Guid.NewGuid();
                            incubationAdModel.IncubationAdID = model.incubationAdvertising.IncubationAdID;
                            incubationAdModel.IncubationModelID = model.IncubationAdvertisingModels[i];
                            repository.Add(incubationAdModel);
                        }
                    }
                }

                if (model.files != null)
                {
                    string FolderName = User.Identity.Name;
                    string path = Server.MapPath("~/Uploads/" + FolderName + "/");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    for (int i = 0; i < model.files.Count(); i++)
                    {
                        if (model.files[i] != null)
                        {
                            model.files[i].SaveAs(path + Path.GetFileName(model.files[i].FileName));
                            IncubationAdvertisingAttachment _Attach = new IncubationAdvertisingAttachment();
                            _Attach.AttachmentID = Guid.NewGuid();
                            _Attach.IncubationAdID = model.incubationAdvertising.IncubationAdID;
                            _Attach.Name = Path.GetFileName(model.files[i].FileName);
                            _Attach.ScreenName = "Incubation Advertising";
                            _Attach.Size = model.files[i].ContentLength.ToString();
                            _Attach.URL = path + Path.GetFileName(model.files[i].FileName);
                            _Attach.Type = model.files[i].ContentType;
                            repository.Add(_Attach);
                        }
                    }
                }

                repository.UnitOfWork.SaveChanges();
                return RedirectToAction("Index", new { Type = Type });
            }

            ViewBag.Type = Type;
            if (CultureHelper.CurrentCulture == 3)
            {
                ViewBag.FundingSource = new SelectList(repository.GetQuery<FundingSource>(f => f.IsActive == true), "FundingSourceID", "FundingSourceNameAR");
                ViewBag.IncubationModels = new MultiSelectList(repository.GetQuery<IncubationModel>(f => f.IsActive == true && f.IncubationType.NameEN == Type), "IncubationModelID", "NameAR");
            }
            else
            {
                ViewBag.FundingSource = new SelectList(repository.GetQuery<FundingSource>(f => f.IsActive == true), "FundingSourceID", "FundingSourceNameEN");
                ViewBag.IncubationModels = new MultiSelectList(repository.GetQuery<IncubationModel>(f => f.IsActive == true && f.IncubationType.NameEN == Type), "IncubationModelID", "NameEN");
            }
            ViewBag.StartDate = model.incubationAdvertising.StartDate.ToString("yyyy-MM-dd");
            ViewBag.EndDate = model.incubationAdvertising.EndDate.ToString("yyyy-MM-dd");
            return View(model);
        }
        //17-1-2026
        [HttpGet]
        public ActionResult Search(string term)
        {
            term = (term ?? "").Trim();

            var res = repository.GetQuery<AspNetUser>(u =>
                    u.AspNetRoles.Any(r =>
                        r.Name == "Corporation" ||
                        r.Name == "Corporation IndIvidual" ||
                        r.Name == "IndIvidual")
                    && u.FrontendUsers.Any(fu =>
                        fu.CorporateApplicationForms.Any(c => c.Program.ProgramName == "Capacity Building"))
                    && (
                        u.Email.Contains(term)
                        || u.FrontendUsers.Any(fu =>
                            fu.CorporateApplicationForms.Any(c =>
                                c.Program.ProgramName == "Capacity Building"
                                && c.Name.Contains(term)
                            )
                        )
                    )
                )
                .Select(u => new
                {
                    label = u.FrontendUsers
                                .SelectMany(fu => fu.CorporateApplicationForms)
                                .Where(c => c.Program.ProgramName == "Capacity Building")
                                .Select(c => c.Name)
                                .FirstOrDefault()
                            + " - " + u.Email,
                    value = u.Email
                })
                .Distinct()
                .ToList();

            return Json(res, JsonRequestBehavior.AllowGet);
        }

        //public ActionResult Search(string term)
        //{
        //    var res = repository.GetQuery<AspNetUser>(f => f.AspNetRoles.Any(r => r.Name == "Corporation" || r.Name == "Corporation IndIvidual" || r.Name == "IndIvidual") && f.FrontendUsers.Any(w => w.CorporateApplicationForms.Any(c => c.Program.ProgramName == "Capacity Building")) && f.Email.Contains(term)).Select(t => t.Email).ToList();
        //    return Json(res, JsonRequestBehavior.AllowGet);
        //}

        public FileResult Download(Guid id)
        {
            var attachmet = repository.GetByKey<IncubationAdvertisingAttachment>(id);
            byte[] fileBytes = System.IO.File.ReadAllBytes(attachmet.URL);
            string fileName = attachmet.Name;
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        [HttpGet]
        public JsonResult DeleteAttachment(Guid Id)
        {
            try
            {
                var Attachment = repository.GetByKey<IncubationAdvertisingAttachment>(Id);
                repository.Delete(Attachment);
                repository.UnitOfWork.SaveChanges();

                if (System.IO.File.Exists(Attachment.URL))
                {
                    System.IO.File.Delete(Attachment.URL);
                }
                return Json("Deleted Succsesfully", JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json("Deletion Failed", JsonRequestBehavior.AllowGet);
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
        //4-3-2025
        //[HttpGet]
        //public ActionResult CanceledInvitationsView()
        //{
        //    try
        //    {
        //        var canceledInvitations = repository.GetQuery<IncubationPrivateInvitation>()
        //            .Where(i => i.Status == InvitationStatus.cancel && i.IncubationAdvertising.IncubationTypeID != null)
        //            .Select(i => new CanceledIncubationInvitationsReportVM
        //            {
        //                IncubationType = i.IncubationAdvertising.IncubationType.NameEN,
        //                AdvertisingName = i.IncubationAdvertising.Name,
        //                Email = i.Email,
        //                OrganizationName = i.FrontendUser != null ? i.FrontendUser.CorporateApplicationForms.FirstOrDefault().Name : "غير متوفر",
        //                InvitationDate = i.CreatedDate,
        //                CancellationDate = i.UpdatedDate.HasValue ? i.UpdatedDate.Value : i.CreatedDate
        //            }).ToList();

        //        return View(canceledInvitations); // تأكد من تمرير القائمة إلى View
        //    }
        //    catch (Exception ex)
        //    {
        //        return View(new List<CanceledIncubationInvitationsReportVM>()); // تجنب `null` بإرجاع قائمة فارغة عند حدوث خطأ
        //    }
        //}

        [HttpGet]
        public ActionResult CanceledInvitationsView(DateTime? DateFrom, DateTime? DateTo, string IncubationType)
        {
            try
            {
                var query = repository.GetQuery<IncubationPrivateInvitation>()
                    .Where(i => i.Status == InvitationStatus.cancel && i.IncubationAdvertising.IncubationTypeID != null);

                // تطبيق الفلترة على نطاق التاريخ إذا تم إدخال القيم
                if (DateFrom.HasValue)
                    query = query.Where(i => i.CreatedDate >= DateFrom.Value);

                if (DateTo.HasValue)
                    query = query.Where(i => i.CreatedDate <= DateTo.Value);

                // فلترة نوع الاحتضان إذا تم تحديده
                if (!string.IsNullOrEmpty(IncubationType))
                    query = query.Where(i => i.IncubationAdvertising.IncubationType.NameEN == IncubationType);

                var canceledInvitations = query.Select(i => new CanceledIncubationInvitationsReportVM
                {
                
                    IncubationType = CultureHelper.CurrentCulture == 3 ? i.IncubationAdvertising.IncubationType.NameAR : i.IncubationAdvertising.IncubationType.NameEN,
                    //IncubationType = i.IncubationAdvertising.IncubationType.NameEN,
                    AdvertisingName = i.IncubationAdvertising.Name,
                    Email = i.Email,
                    OrganizationName = i.FrontendUser != null ? i.FrontendUser.CorporateApplicationForms.FirstOrDefault().Name : "غير متوفر",
                    InvitationDate = i.CreatedDate,
                    CancellationDate = i.UpdatedDate.HasValue ? i.UpdatedDate.Value : i.CreatedDate
                }).ToList();

                return View(canceledInvitations);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "حدث خطأ أثناء تحميل التقرير: " + ex.Message;
                return View(new List<CanceledIncubationInvitationsReportVM>());
            }
        }

        [AllowAnonymous]
        public ActionResult PublicAdDetails(Guid id)
        {
            var ad = repository.GetQuery<IncubationAdvertising>(a => a.IncubationAdID == id && !a.IsDeleted)
                .FirstOrDefault();

            if (ad == null)
                return HttpNotFound();

            return View("PublicAdDetails", ad);
        }


    }
}
