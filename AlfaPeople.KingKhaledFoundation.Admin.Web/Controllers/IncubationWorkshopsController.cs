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
using System.Collections.Generic;
using AlfaPeople.KingKhalidFoundation.Data;
using AlfaPeople.KingKhalidFoundation.Data.Model;
using AlfaPeople.KingKhaledFoundation.Admin.Web.Models;
using AlfaPeople.KingKhaledFoundation.Admin.Web.Helper;
using System.Web;
using System.Data.Entity;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Drawing;
using Region = AlfaPeople.KingKhalidFoundation.Data.Model.Region;
using System.Web.UI.HtmlControls;

namespace AlfaPeople.KingKhaledFoundation.Admin.Web.Controllers
{
    [Authorize(Roles = "Admin, CB Manager, CB Supervisor, CB Analyst")]
    public class IncubationWorkshopsController : BaseController
    {
        private readonly IRepository repository;
        private readonly CommonHelper helper;
        private readonly string Logpath = ConfigurationManager.AppSettings["Log"].ToString();

        public IncubationWorkshopsController()
        {
            repository = new Repository(new KingkhaledFoundationDB());
            helper = new CommonHelper();
        }

        // GET: IncubationWorkshops
        public ActionResult Index()
        {
            ViewBag.Title = App_GlobalResources.General.Workshops; //" Workshops";
            //2-25-2025
            //var incubationWorkshops = repository.GetAll<IncubationWorkshop>().ToList();
            var incubationWorkshops = repository.GetQuery<IncubationWorkshop>(w =>
               !w.IsDeleted).ToList();
            return View(incubationWorkshops);
        }
        [AllowAnonymous]
        public ActionResult PublicWorkshopDetails(Guid id)
        {
            var workshop = repository.GetQuery<IncubationWorkshop>(w => w.IncubationWorkshopID == id && !w.IsDeleted)
                .Include(w => w.Region)
                .Include(w => w.City)
                .Include(w => w.Governorate)
                .Include(w => w.Consultant)
                .Include(w => w.FundingSource)
                .Include(w => w.IncubationWorkshopModel)
                .FirstOrDefault();

            if (workshop == null)
                return HttpNotFound();

            return View(workshop);
        }
        //2-25-2025
        [HttpPost]
        public ActionResult DeleteWorkshop(Guid id)
        {
            var workshop = repository.GetByKey<IncubationWorkshop>(id);
            if (workshop == null)
                return Json(new { success = false, message = "Workshop not found." });

            workshop.IsDeleted = true;
            workshop.DeletedDate = DateTime.UtcNow;
            workshop.DeletedBy = Guid.Parse(User.Identity.GetUserId());

            repository.Update(workshop);
            repository.UnitOfWork.SaveChanges();

            return Json(new { success = true, message = "تم حذف ورشة العمل بنجاح." });
        }


        // GET: IncubationWorkshops/Create
        public ActionResult Create()
        {
            ViewBag.Title = App_GlobalResources.General.CreateWorkshop;
            //23-1-2025
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

            if (CultureHelper.CurrentCulture == 3)
            {
                ViewBag.CityID = new SelectList(repository.GetQuery<City>(f => f.IsActive == true), "CityID", "CityNameAR");
                ViewBag.GovernorateID = new SelectList(repository.GetQuery<Governorate>(f => f.IsActive == true), "GovernorateID", "GovernorateAR");
                ViewBag.RegionID = new SelectList(repository.GetQuery<Region>(f => f.IsActive == true), "RegionID", "RegionNameAR");
                ViewBag.IncubationWorkshopModelID = new SelectList(repository.GetQuery<IncubationWorkshopModel>(f => f.IsActive == true), "IncubationWorkshopModeID", "NameAR");
                ViewBag.FundingSource = new SelectList(repository.GetQuery<FundingSource>(f => f.IsActive == true), "FundingSourceID", "FundingSourceNameAR");
            }
            else
            {
                ViewBag.CityID = new SelectList(repository.GetQuery<City>(f => f.IsActive == true), "CityID", "CityNameEN");
                ViewBag.GovernorateID = new SelectList(repository.GetQuery<Governorate>(f => f.IsActive == true), "GovernorateID", "GovernorateEN");
                ViewBag.RegionID = new SelectList(repository.GetQuery<Region>(f => f.IsActive == true), "RegionID", "RegionNameEN");
                ViewBag.IncubationWorkshopModelID = new SelectList(repository.GetQuery<IncubationWorkshopModel>(f => f.IsActive == true), "IncubationWorkshopModeID", "NameEN");
                ViewBag.FundingSource = new SelectList(repository.GetQuery<FundingSource>(f => f.IsActive == true), "FundingSourceID", "FundingSourceNameEN");
            }

            ViewBag.ConsultantID = new SelectList(repository.GetQuery<Consultant>(f => f.IsActive == true), "ConsultantID", "Name");

            return View();
        }

        // POST: IncubationWorkshops/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IncubationWorkshopVM model, List<Guid> SelectedFieldsOfWork, List<Guid> SelectedRegions, HttpPostedFileBase CommitmentFile)
        {

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
                try
                {
                    //23-1-2025
                    var workshopID = Guid.NewGuid();
                    var userId = User.Identity.GetUserId();
                    //2-25-2025
                    model.IncubationWorkshop.IsDeleted = false;
                    model.IncubationWorkshop.IncubationWorkshopID = Guid.NewGuid();
                    model.IncubationWorkshop.IncubationtWorkshopStatusID = repository.FindOne<IncubationtWorkshopStatu>(f => f.NameEN == "Active").IncubationtWorkshopStatusID;
                    if (model.IncubationWorkshop.ISPublic != true)
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

                            var finalList = InvitList.Select(f => f.AspNetUser).ToList();

                            for (int i = 0; i < finalList.Count(); i++)
                            {


                                WorkshopPrivateInvitation _Private = new WorkshopPrivateInvitation()
                                {
                                    WorkshopPrivateInvitationId = Guid.NewGuid(),
                                    Email = finalList[i].Email,
                                    FrontendUserID = Guid.Parse(finalList[i].Id),
                                    InvitationStatus = InvitationStatus.pending,
                                    IncubationWorkshopID = model.IncubationWorkshop.IncubationWorkshopID,
                                    //24-2-2025
                                    CreatedDate = DateTime.Now// تعيين التاريخ والوقت الحالي
                            };
                                repository.Add(_Private);

                                MailHelper mailHelper = new MailHelper();
                                mailHelper.ToEmail = finalList[i].Email;
                                mailHelper.Subject = $"بدء التقديم على ورشة عمل {model.IncubationWorkshop.Name}";
                                mailHelper.IsHtml = true;
                                //shadia
                                //mailHelper.Body = "المكرمين/ المنظمات غير الربحية                         الموقرين<br/>"
                                //                            + "السلام عليكم ورحمة الله وبركاته، <br/>"
                                //                            + "تهديكم مؤسسة الملك خالد أطيب التحيات، <br/> وبناءً على قبول منظمتكم في ورش العمل التدريبية ببرنامج بناء القدرات، عليه، <br/>"
                                //                            + $"نحيطكم علماً بأنه تم فتح باب التقديم على ورشة عمل {model.IncubationWorkshop.Name} ضمن مشاريع برنامج بناء القدرات. <br/>"
                                //                            + $"نأمل منكم التكرم بزيارة البوابة الإلكترونية وتعبئة طلب الالتحاق بورشة العمل وإرسال إقرار الالتزام في موعد اقصاه : {model.IncubationWorkshop.LastTimeToApply.ToString("dd/MM/yyyy")} <br/>"
                                //                            + "مع تمنياتنا لكم بالتوفيق، <br/>"
                                //                            + " برنامج بناء القدرات. <br/>";
                    //          
                                mailHelper.Body = "المكرمين/ المنظمات غير الربحية الموقرين,<br/>"
                                    + "السلام عليكم ورحمة الله وبركاته،<br/>"
                                    + "تهديكم مؤسسة الملك خالد أطيب التحيات،<br/><br/>"
                                    + $"نحيطكم علماً عن بدء التقديم لورشة عمل {model.IncubationWorkshop.Name} ضمن مشاريع برنامج بناء القدرات،<br/>"
                                    + $"عليه نأمل منكم التكرم بزيارة البوابة الإلكترونية وتعبئة طلب الالتحاق بورشة العمل في موعد أقصاه: {model.IncubationWorkshop.LastTimeToApply.ToString("dd/MM/yyyy")}.<br/><br/>"
                                    + "وتفضلوا بقبول فائق التحية والتقدير،<br/>"
                                    + "فريق برنامج بناء القدرات.";

                                //
                                var fundingSource = repository.GetByKey<FundingSource>(model.IncubationWorkshop.FundingSourceID);
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
                            {
                                if (Emails[i].Trim() != "")
                                {
                                    var emailaddres = Emails[i].Trim();
                                    var frontendUserId = repository.GetQuery<AspNetUser>(a => a.Email == emailaddres).ToList();
                                    WorkshopPrivateInvitation _Private = new WorkshopPrivateInvitation()
                                    {
                                        WorkshopPrivateInvitationId = Guid.NewGuid(),
                                        Email = Emails[i].Trim(),
                                        FrontendUserID = frontendUserId.Count == 0 ? (Guid?)null : Guid.Parse(frontendUserId.FirstOrDefault().Id),
                                        InvitationStatus = InvitationStatus.pending,
                                        IncubationWorkshopID = model.IncubationWorkshop.IncubationWorkshopID,
                                        //2-24-2025
                                        CreatedDate = DateTime.Now,
                                    };
                                    repository.Add(_Private);

                                    MailHelper mailHelper = new MailHelper();
                                    mailHelper.ToEmail = Emails[i].Trim();
                                    mailHelper.Subject = $"بدء التقديم على ورشة عمل {model.IncubationWorkshop.Name}";
                                    mailHelper.IsHtml = true;
                                    mailHelper.Body = "المكرمين/ المنظمات غير الربحية الموقرين,<br/>"
                                        + "السلام عليكم ورحمة الله وبركاته،<br/>"
                                        + "تهديكم مؤسسة الملك خالد أطيب التحيات،<br/>"
                                        + $"وبناءً على قبول منظمتكم في ورشة عمل {model.IncubationWorkshop.Name} ضمن مشاريع برنامج بناء القدرات،<br/>"
                                        + "عليه نأمل منكم التكرم بزيارة البوابة الإلكترونية وتعبئة طلب الالتحاق بورشة العمل في موعد أقصاه: "
                                        + $"{model.IncubationWorkshop.LastTimeToApply.ToString("dd/MM/yyyy")}.<br/>"
                                        + "مع تمنياتنا لكم بالتوفيق،<br/>"
                                        + "برنامج بناء القدرات.<br/>";
                                    //mailHelper.Body = "المكرمين/ المنظمات غير الربحية                         الموقرين<br/>"
                                    //                        + "السلام عليكم ورحمة الله وبركاته، <br/>"
                                    //                        + "تهديكم مؤسسة الملك خالد أطيب التحيات، <br/> وبناءً على قبول منظمتكم في ورش العمل التدريبية ببرنامج بناء القدرات، عليه، <br/>"
                                    //                        + $"نحيطكم علماً بأنه تم فتح باب التقديم على ورشة عمل {model.IncubationWorkshop.Name} ضمن مشاريع برنامج بناء القدرات. <br/>"
                                    //                        + $"نأمل منكم التكرم بزيارة البوابة الإلكترونية وتعبئة طلب الالتحاق بورشة العمل وإرسال إقرار الالتزام في موعد اقصاه : {model.IncubationWorkshop.LastTimeToApply.ToString("dd/MM/yyyy")} <br/>"
                                    //                        + "مع تمنياتنا لكم بالتوفيق، <br/>"
                                    //                        + " برنامج بناء القدرات. <br/>";

                                    var fundingSource = repository.GetByKey<FundingSource>(model.IncubationWorkshop.FundingSourceID);
                                    if (fundingSource.UseCustomThemes)
                                    {
                                        mailHelper.Send($"?partner={fundingSource.Nickname}");
                                    }

                                    else {
                                        mailHelper.Send("");
                                    }
                                }
                            }
                        }
                      
                    }
                    else if (model.IncubationWorkshop.ISPublic != false)
                    {
                        var InvitList = repository.GetQuery<FrontendUser>(a => a.IsActive && a.CorporateApplicationForms.FirstOrDefault().Program.ProgramName == "Capacity Building" && a.CorporateApplicationForms.FirstOrDefault().CorporateApplicationStatu.ApplicantStatu.ApplicantStatusName == "Accepted").Select(f => f.AspNetUser).ToList();
                        for (int i = 0; i < InvitList.Count(); i++)
                        {
                            WorkshopPrivateInvitation _Private = new WorkshopPrivateInvitation()
                            {
                                WorkshopPrivateInvitationId = Guid.NewGuid(),
                                Email = InvitList[i].Email,
                                FrontendUserID = Guid.Parse(InvitList[i].Id),
                                InvitationStatus = InvitationStatus.pending,
                                IncubationWorkshopID = model.IncubationWorkshop.IncubationWorkshopID,
                                CreatedDate = DateTime.Now// تعيين التاريخ والوقت الحالي

                            };
                            repository.Add(_Private);

                            MailHelper mailHelper = new MailHelper();
                            mailHelper.ToEmail = InvitList[i].Email;
                            mailHelper.Subject = $"بدء التقديم على ورشة عمل {model.IncubationWorkshop.Name}";
                            mailHelper.IsHtml = true;
                            //shadia
                            //mailHelper.Body = "المكرمين/ المنظمات غير الربحية                         الموقرين<br/>"
                            //                            + "السلام عليكم ورحمة الله وبركاته، <br/>"
                            //                            + "تهديكم مؤسسة الملك خالد أطيب التحيات، <br/> وبناءً على قبول منظمتكم في ورش العمل التدريبية ببرنامج بناء القدرات، عليه، <br/>"
                            //                            + $"نحيطكم علماً بأنه تم فتح باب التقديم على ورشة عمل {model.IncubationWorkshop.Name} ضمن مشاريع برنامج بناء القدرات. <br/>"
                            //                            + $"نأمل منكم التكرم بزيارة البوابة الإلكترونية وتعبئة طلب الالتحاق بورشة العمل وإرسال إقرار الالتزام في موعد اقصاه : {model.IncubationWorkshop.LastTimeToApply.ToString("dd/MM/yyyy")} <br/>"
                            //                            + "مع تمنياتنا لكم بالتوفيق، <br/>"
                            //                            + " برنامج بناء القدرات. <br/>";
                            mailHelper.Body = "المكرمين/ المنظمات غير الربحية الموقرين,<br/>"
                     + "السلام عليكم ورحمة الله وبركاته،<br/>"
                     + "تهديكم مؤسسة الملك خالد أطيب التحيات،<br/><br/>"
                     + $"نحيطكم علماً عن بدء التقديم لورشة عمل {model.IncubationWorkshop.Name} ضمن مشاريع برنامج بناء القدرات،<br/>"
                     + $"عليه نأمل منكم التكرم بزيارة البوابة الإلكترونية وتعبئة طلب الالتحاق بورشة العمل في موعد أقصاه: {model.IncubationWorkshop.LastTimeToApply.ToString("dd/MM/yyyy")}.<br/><br/>"
                     + "وتفضلوا بقبول فائق التحية والتقدير،<br/>"
                     + "فريق برنامج بناء القدرات.";

                            //
                            var fundingSource = repository.GetByKey<FundingSource>(model.IncubationWorkshop.FundingSourceID);
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
                            repository.Add(model.IncubationWorkshop);

                            if (CommitmentFile != null && CommitmentFile.ContentLength > 0)
                            {
                                string FolderName = User.Identity.Name;
                                string path = Server.MapPath("~/Uploads/" + FolderName + "/");
                                if (!Directory.Exists(path))
                                    Directory.CreateDirectory(path);

                                string fileName = Path.GetFileName(CommitmentFile.FileName);
                                string fullPath = Path.Combine(path, fileName);
                                CommitmentFile.SaveAs(fullPath);
                              
                                string baseUrl = Request.Url.GetLeftPart(UriPartial.Authority);
                                string relativePath = "/Uploads/" + FolderName + "/" + fileName;
                                string fullUrl = baseUrl + relativePath;
                                // احفظ الرابط في قاعدة البيانات في جدول مرفقات الورشة
                                repository.Add(new IncubationWorkshopAttachment
                                {
                                    AttachmentID = Guid.NewGuid(),
                                    IncubationWorkshopID = model.IncubationWorkshop.IncubationWorkshopID,
                                    Name = fileName,
                                    Type = CommitmentFile.ContentType,
                                    //URL= fullUrl,
                                    //URL = "/Uploads/" + FolderName + "/" + fileName,

                                    //URL = fullPath,
                                    URL = path + Path.GetFileName(CommitmentFile.FileName),
                                    Size = CommitmentFile.ContentLength.ToString(),
                                    ScreenName = "Workshop Commitment",
                                    IsCommitmentFile = true
                                });
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
                                        IncubationWorkshopAttachment _Attach = new IncubationWorkshopAttachment();
                                        _Attach.AttachmentID = Guid.NewGuid();
                                        _Attach.IncubationWorkshopID = model.IncubationWorkshop.IncubationWorkshopID;
                                        _Attach.Name = Path.GetFileName(model.files[i].FileName);
                                        _Attach.ScreenName = "Incubation WorkShop";
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
                        catch (Exception e)
                        {
                            ViewBag.Title = App_GlobalResources.General.CreateWorkshop;
                            if (CultureHelper.CurrentCulture == 3)
                            {
                                ViewBag.CityID = new SelectList(repository.GetQuery<City>(f => f.IsActive == true), "CityID", "CityNameAR");
                                ViewBag.GovernorateID = new SelectList(repository.GetQuery<Governorate>(f => f.IsActive == true), "GovernorateID", "GovernorateAR");
                                ViewBag.RegionID = new SelectList(repository.GetQuery<Region>(f => f.IsActive == true), "RegionID", "RegionNameAR");
                                ViewBag.IncubationWorkshopModelID = new SelectList(repository.GetQuery<IncubationWorkshopModel>(f => f.IsActive == true), "IncubationWorkshopModeID", "NameAR");
                                ViewBag.FundingSource = new SelectList(repository.GetQuery<FundingSource>(f => f.IsActive == true), "FundingSourceID", "FundingSourceNameAR");
                            }
                            else
                            {
                                ViewBag.CityID = new SelectList(repository.GetQuery<City>(f => f.IsActive == true), "CityID", "CityNameEN");
                                ViewBag.GovernorateID = new SelectList(repository.GetQuery<Governorate>(f => f.IsActive == true), "GovernorateID", "GovernorateEN");
                                ViewBag.RegionID = new SelectList(repository.GetQuery<Region>(f => f.IsActive == true), "RegionID", "RegionNameEN");
                                ViewBag.IncubationWorkshopModelID = new SelectList(repository.GetQuery<IncubationWorkshopModel>(f => f.IsActive == true), "IncubationWorkshopModeID", "NameEN");
                                ViewBag.FundingSource = new SelectList(repository.GetQuery<FundingSource>(f => f.IsActive == true), "FundingSourceID", "FundingSourceNameEN");
                            }
                            ViewBag.ConsultantID = new SelectList(repository.GetQuery<Consultant>(f => f.IsActive == true), "ConsultantID", "Name");
                            scope.Dispose();
                            return View(model);
                        }
                    }
                }
                catch (Exception)
                {
                    ViewBag.Title = App_GlobalResources.General.CreateWorkshop;
                    if (CultureHelper.CurrentCulture == 3)
                    {
                        ViewBag.CityID = new SelectList(repository.GetQuery<City>(f => f.IsActive == true), "CityID", "CityNameAR");
                        ViewBag.GovernorateID = new SelectList(repository.GetQuery<Governorate>(f => f.IsActive == true), "GovernorateID", "GovernorateAR");
                        ViewBag.RegionID = new SelectList(repository.GetQuery<Region>(f => f.IsActive == true), "RegionID", "RegionNameAR");
                        ViewBag.IncubationWorkshopModelID = new SelectList(repository.GetQuery<IncubationWorkshopModel>(f => f.IsActive == true), "IncubationWorkshopModeID", "NameAR");
                        ViewBag.FundingSource = new SelectList(repository.GetQuery<FundingSource>(f => f.IsActive == true), "FundingSourceID", "FundingSourceNameAR");
                    }
                    else
                    {
                        ViewBag.CityID = new SelectList(repository.GetQuery<City>(f => f.IsActive == true), "CityID", "CityNameEN");
                        ViewBag.GovernorateID = new SelectList(repository.GetQuery<Governorate>(f => f.IsActive == true), "GovernorateID", "GovernorateEN");
                        ViewBag.RegionID = new SelectList(repository.GetQuery<Region>(f => f.IsActive == true), "RegionID", "RegionNameEN");
                        ViewBag.IncubationWorkshopModelID = new SelectList(repository.GetQuery<IncubationWorkshopModel>(f => f.IsActive == true), "IncubationWorkshopModeID", "NameEN");
                        ViewBag.FundingSource = new SelectList(repository.GetQuery<FundingSource>(f => f.IsActive == true), "FundingSourceID", "FundingSourceNameEN");
                    }
                    ViewBag.ConsultantID = new SelectList(repository.GetQuery<Consultant>(f => f.IsActive == true), "ConsultantID", "Name");
                    return View(model);
                }
            }
            return RedirectToAction("Index");
        }
        //25-3-2025
        public FileResult ViewCommitment(Guid id)
        {
            var attachment = repository.GetByKey<IncubationWorkshopAttachment>(id);

            if (attachment == null || !attachment.IsCommitmentFile)
                throw new FileNotFoundException("الملف غير موجود");

            string filePath = Server.MapPath(attachment.URL);
            if (!System.IO.File.Exists(filePath))
                throw new FileNotFoundException("الملف غير موجود على السيرفر", filePath);

            return File(filePath, "application/pdf", attachment.Name);
        }

        // GET: IncubationWorkshops/Edit/5
        public ActionResult Edit(Guid id)
        {

            if (id == null)
                return View();

            IncubationWorkshop IncubationWS = repository.GetByKey<IncubationWorkshop>(id);

            //25-3-2025
            var commitmentFile = repository.GetQuery<IncubationWorkshopAttachment>()
                     .FirstOrDefault(a => a.IncubationWorkshopID == IncubationWS.IncubationWorkshopID && a.IsCommitmentFile == true);

            ViewBag.CommitmentFileUrl = commitmentFile != null ? commitmentFile.URL : null;
            ViewBag.CommitmentFileName = commitmentFile?.Name;
            ViewBag.CommitmentFileID = commitmentFile?.AttachmentID;

            if (IncubationWS == null)
                return HttpNotFound();

            ViewBag.Title = App_GlobalResources.General.Update + IncubationWS.Name + App_GlobalResources.General.Advertising;

            if (CultureHelper.CurrentCulture == 3)
            {
                ViewBag.CityID = new SelectList(repository.GetQuery<City>(f => f.IsActive == true), "CityID", "CityNameAR", IncubationWS.CityID);
                ViewBag.GovernorateID = new SelectList(repository.GetQuery<Governorate>(f => f.IsActive == true), "GovernorateID", "GovernorateAR", IncubationWS.GovernorateID);
                ViewBag.RegionID = new SelectList(repository.GetQuery<Region>(f => f.IsActive == true), "RegionID", "RegionNameAR", IncubationWS.RegionID);
                ViewBag.IncubationWorkshopModelID = new SelectList(repository.GetQuery<IncubationWorkshopModel>(f => f.IsActive == true), "IncubationWorkshopModeID", "NameAR", IncubationWS.IncubationWorkshopModelID);
                ViewBag.FundingSource = new SelectList(repository.GetQuery<FundingSource>(f => f.IsActive == true), "FundingSourceID", "FundingSourceNameAR", IncubationWS.FundingSourceID);
            }
            else
            {
                ViewBag.CityID = new SelectList(repository.GetQuery<City>(f => f.IsActive == true), "CityID", "CityNameEN", IncubationWS.CityID);
                ViewBag.GovernorateID = new SelectList(repository.GetQuery<Governorate>(f => f.IsActive == true), "GovernorateID", "GovernorateEN", IncubationWS.GovernorateID);
                ViewBag.RegionID = new SelectList(repository.GetQuery<Region>(f => f.IsActive == true), "RegionID", "RegionNameEN", IncubationWS.RegionID);
                ViewBag.IncubationWorkshopModelID = new SelectList(repository.GetQuery<IncubationWorkshopModel>(f => f.IsActive == true), "IncubationWorkshopModeID", "NameEN", IncubationWS.IncubationWorkshopModelID);
                ViewBag.FundingSource = new SelectList(repository.GetQuery<FundingSource>(f => f.IsActive == true), "FundingSourceID", "FundingSourceNameEN", IncubationWS.FundingSourceID);
            }
            ViewBag.ConsultantID = new SelectList(repository.GetQuery<Consultant>(f => f.IsActive == true), "ConsultantID", "Name", IncubationWS.ConsultantID);
            ViewBag.EndDate = IncubationWS.EndDate.ToString("yyyy-MM-dd");
            ViewBag.StartDate = IncubationWS.StartDate.ToString("yyyy-MM-dd");
            ViewBag.lastTimeToApply = IncubationWS.LastTimeToApply.ToString("yyyy-MM-dd");
            //24-2-2025
            //  تصفية الدعوات لاستبعاد الدعوات الملغاة
            IncubationWS.WorkshopPrivateInvitations = IncubationWS.WorkshopPrivateInvitations
                .Where(inv => inv.InvitationStatus != InvitationStatus.cancel)
                .ToList();

            IncubationWorkshopVM IncubationAdVM = new IncubationWorkshopVM();
            IncubationAdVM.IncubationWorkshop = IncubationWS;
            return View(IncubationAdVM);
        }

        // POST: IncubationWorkshops/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(IncubationWorkshopVM model, HttpPostedFileBase CommitmentFile)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (TransactionScope scope = new TransactionScope())
                    {
                        try
                        {
                            if (model.SendInvitation)
                            {
                                //25-2-2025
                                //if (model.IncubationWorkshop.WorkshopPrivateInvitations.Count <= 0)
                                //    model.IncubationWorkshop.WorkshopPrivateInvitations = repository
                                //        .Get<WorkshopPrivateInvitation> (i => i.IncubationWorkshopID ==
                                //        model.IncubationWorkshop.IncubationWorkshopID);
                                if (model.IncubationWorkshop.WorkshopPrivateInvitations.Count <= 0)
                                    model.IncubationWorkshop.WorkshopPrivateInvitations = repository
                                        .Get<WorkshopPrivateInvitation>(i => i.IncubationWorkshopID == 
                                        model.IncubationWorkshop.IncubationWorkshopID)
                                        .Where(i => i.InvitationStatus != InvitationStatus.cancel) //  استبعاد الدعوات الملغاة
                                        .ToList();
                                foreach (var invitation in model.IncubationWorkshop.WorkshopPrivateInvitations)
                                {
                                    MailHelper mailHelper = new MailHelper();
                                    mailHelper.ToEmail = invitation.Email;
                                    mailHelper.Subject = $"بدء التقديم على ورشة عمل {model.IncubationWorkshop.Name}";
                                    mailHelper.IsHtml = true;
                                    mailHelper.Body = "المكرمين/ المنظمات غير الربحية                         الموقرين<br/>"
                                                                + "السلام عليكم ورحمة الله وبركاته، <br/>"
                                                                + "تهديكم مؤسسة الملك خالد أطيب التحيات، <br/> وبناءً على قبول منظمتكم في ورش العمل التدريبية ببرنامج بناء القدرات، عليه، <br/>"
                                                                + $"نحيطكم علماً بأنه تم فتح باب التقديم على ورشة عمل {model.IncubationWorkshop.Name} ضمن مشاريع برنامج بناء القدرات. <br/>"
                                                                + $"نأمل منكم التكرم بزيارة البوابة الإلكترونية وتعبئة طلب الالتحاق بورشة العمل  في موعد اقصاه : {model.IncubationWorkshop.LastTimeToApply.ToString("dd/MM/yyyy")} <br/>"
                                                                + "مع تمنياتنا لكم بالتوفيق، <br/>"
                                                                + " برنامج بناء القدرات. <br/>";

                                    var fundingSource = repository.GetByKey<FundingSource>(model.IncubationWorkshop.FundingSourceID);
                                    if (fundingSource.UseCustomThemes)
                                        mailHelper.Send($"?partner={fundingSource.Nickname}");
                                    else
                                        mailHelper.Send("");
                                }
                            }

                            repository.Update(model.IncubationWorkshop);

                            //25-3-2025
                            if (CommitmentFile != null && CommitmentFile.ContentLength > 0)
                            {
                                string FolderName = User.Identity.Name;
                                string path = Server.MapPath("~/Uploads/" + FolderName + "/");
                           

                                // خزّن fullUrl في قاعدة البيانات

                                if (!Directory.Exists(path))
                                    Directory.CreateDirectory(path);

                                string fileName = Path.GetFileName(CommitmentFile.FileName);
                                string fullPath = Path.Combine(path, fileName);
                                CommitmentFile.SaveAs(fullPath);
                               // string relativeUrl = "/Uploads/" + FolderName + "/" + fileName;
                                string baseUrl = Request.Url.GetLeftPart(UriPartial.Authority);
                                string relativePath = "/Uploads/" + FolderName + "/" + fileName;
                                string fullUrl = baseUrl + relativePath;
                                // حذف الملف القديم ()
                                var oldFile = repository.GetQuery<IncubationWorkshopAttachment>()
                                    .FirstOrDefault(a => a.IncubationWorkshopID == model.IncubationWorkshop.IncubationWorkshopID && a.IsCommitmentFile == true);
                                if (oldFile != null)
                                {
                                    repository.Delete(oldFile);
                                }

                                // إضافة الملف الجديد
                                repository.Add(new IncubationWorkshopAttachment
                                {
                                    AttachmentID = Guid.NewGuid(),
                                    IncubationWorkshopID = model.IncubationWorkshop.IncubationWorkshopID,
                                    Name = fileName,
                                    Type = CommitmentFile.ContentType,
                                    //   URL= fullUrl,
                                    URL = path + Path.GetFileName(CommitmentFile.FileName),
                                    // URL = fullPath,
                                    //URL = "/Uploads/" + FolderName + "/" + fileName,
                                    Size = CommitmentFile.ContentLength.ToString(),
                                    ScreenName = "Workshop Commitment",
                                    IsCommitmentFile = true
                                });
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
                                        IncubationWorkshopAttachment _Attach = new IncubationWorkshopAttachment();
                                        _Attach.AttachmentID = Guid.NewGuid();
                                        _Attach.IncubationWorkshopID = model.IncubationWorkshop.IncubationWorkshopID;
                                        _Attach.Name = Path.GetFileName(model.files[i].FileName);
                                        _Attach.ScreenName = "Update Incubation WorkShop";
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
                            return RedirectToAction("Index");
                        }
                        catch (Exception)
                        {
                            ViewBag.Title = App_GlobalResources.General.Update + model?.IncubationWorkshop?.Name + App_GlobalResources.General.Advertising;
                            if (CultureHelper.CurrentCulture == 3)
                            {
                                ViewBag.CityID = new SelectList(repository.GetQuery<City>(f => f.IsActive == true), "CityID", "CityNameAR");
                                ViewBag.GovernorateID = new SelectList(repository.GetQuery<Governorate>(f => f.IsActive == true), "GovernorateID", "GovernorateAR");
                                ViewBag.RegionID = new SelectList(repository.GetQuery<Region>(f => f.IsActive == true), "RegionID", "RegionNameAR");
                                ViewBag.IncubationWorkshopModelID = new SelectList(repository.GetQuery<IncubationWorkshopModel>(f => f.IsActive == true), "IncubationWorkshopModeID", "NameAR");
                                ViewBag.FundingSource = new SelectList(repository.GetQuery<FundingSource>(f => f.IsActive == true), "FundingSourceID", "FundingSourceNameAR");
                            }
                            else
                            {
                                ViewBag.CityID = new SelectList(repository.GetQuery<City>(f => f.IsActive == true), "CityID", "CityNameEN");
                                ViewBag.GovernorateID = new SelectList(repository.GetQuery<Governorate>(f => f.IsActive == true), "GovernorateID", "GovernorateEN");
                                ViewBag.RegionID = new SelectList(repository.GetQuery<Region>(f => f.IsActive == true), "RegionID", "RegionNameEN");
                                ViewBag.IncubationWorkshopModelID = new SelectList(repository.GetQuery<IncubationWorkshopModel>(f => f.IsActive == true), "IncubationWorkshopModeID", "NameEN");
                                ViewBag.FundingSource = new SelectList(repository.GetQuery<FundingSource>(f => f.IsActive == true), "FundingSourceID", "FundingSourceNameEN");
                            }
                            ViewBag.ConsultantID = new SelectList(repository.GetQuery<Consultant>(f => f.IsActive == true), "ConsultantID", "Name", model.IncubationWorkshop.ConsultantID);
                            ViewBag.EndDate = model.IncubationWorkshop.EndDate.ToString("yyyy-MM-dd");
                            ViewBag.StartDate = model.IncubationWorkshop.StartDate.ToString("yyyy-MM-dd");
                            ViewBag.lastTimeToApply = model.IncubationWorkshop.LastTimeToApply.ToString("yyyy-MM-dd");
                            scope.Dispose();
                            return View(model);
                        }
                    }
                }
                catch (Exception)
                {
                    ViewBag.Title = App_GlobalResources.General.Update + model?.IncubationWorkshop?.Name + App_GlobalResources.General.Advertising;

                    if (CultureHelper.CurrentCulture == 3)
                    {
                        ViewBag.CityID = new SelectList(repository.GetQuery<City>(f => f.IsActive == true), "CityID", "CityNameAR");
                        ViewBag.GovernorateID = new SelectList(repository.GetQuery<Governorate>(f => f.IsActive == true), "GovernorateID", "GovernorateAR");
                        ViewBag.RegionID = new SelectList(repository.GetQuery<Region>(f => f.IsActive == true), "RegionID", "RegionNameAR");
                        ViewBag.IncubationWorkshopModelID = new SelectList(repository.GetQuery<IncubationWorkshopModel>(f => f.IsActive == true), "IncubationWorkshopModeID", "NameAR");
                        ViewBag.FundingSource = new SelectList(repository.GetQuery<FundingSource>(f => f.IsActive == true), "FundingSourceID", "FundingSourceNameAR");
                    }
                    else
                    {
                        ViewBag.CityID = new SelectList(repository.GetQuery<City>(f => f.IsActive == true), "CityID", "CityNameEN");
                        ViewBag.GovernorateID = new SelectList(repository.GetQuery<Governorate>(f => f.IsActive == true), "GovernorateID", "GovernorateEN");
                        ViewBag.RegionID = new SelectList(repository.GetQuery<Region>(f => f.IsActive == true), "RegionID", "RegionNameEN");
                        ViewBag.IncubationWorkshopModelID = new SelectList(repository.GetQuery<IncubationWorkshopModel>(f => f.IsActive == true), "IncubationWorkshopModeID", "NameEN");
                        ViewBag.FundingSource = new SelectList(repository.GetQuery<FundingSource>(f => f.IsActive == true), "FundingSourceID", "FundingSourceNameEN");
                    }
                    ViewBag.ConsultantID = new SelectList(repository.GetQuery<Consultant>(f => f.IsActive == true), "ConsultantID", "Name", model.IncubationWorkshop.ConsultantID);
                    ViewBag.EndDate = model.IncubationWorkshop.EndDate.ToString("yyyy-MM-dd");
                    ViewBag.StartDate = model.IncubationWorkshop.StartDate.ToString("yyyy-MM-dd");
                    ViewBag.lastTimeToApply = model.IncubationWorkshop.LastTimeToApply.ToString("yyyy-MM-dd");
                    return View(model);
                }
            }

            ViewBag.Title = App_GlobalResources.General.Update + model?.IncubationWorkshop?.Name + App_GlobalResources.General.Advertising;

            if (CultureHelper.CurrentCulture == 3)
            {
                ViewBag.CityID = new SelectList(repository.GetQuery<City>(f => f.IsActive == true), "CityID", "CityNameAR");
                ViewBag.GovernorateID = new SelectList(repository.GetQuery<Governorate>(f => f.IsActive == true), "GovernorateID", "GovernorateAR");
                ViewBag.RegionID = new SelectList(repository.GetQuery<Region>(f => f.IsActive == true), "RegionID", "RegionNameAR");
                ViewBag.IncubationWorkshopModelID = new SelectList(repository.GetQuery<IncubationWorkshopModel>(f => f.IsActive == true), "IncubationWorkshopModeID", "NameAR");
                ViewBag.FundingSource = new SelectList(repository.GetQuery<FundingSource>(f => f.IsActive == true), "FundingSourceID", "FundingSourceNameAR");
            }
            else
            {
                ViewBag.CityID = new SelectList(repository.GetQuery<City>(f => f.IsActive == true), "CityID", "CityNameEN");
                ViewBag.GovernorateID = new SelectList(repository.GetQuery<Governorate>(f => f.IsActive == true), "GovernorateID", "GovernorateEN");
                ViewBag.RegionID = new SelectList(repository.GetQuery<Region>(f => f.IsActive == true), "RegionID", "RegionNameEN");
                ViewBag.IncubationWorkshopModelID = new SelectList(repository.GetQuery<IncubationWorkshopModel>(f => f.IsActive == true), "IncubationWorkshopModeID", "NameEN");
                ViewBag.FundingSource = new SelectList(repository.GetQuery<FundingSource>(f => f.IsActive == true), "FundingSourceID", "FundingSourceNameEN");
            }
            ViewBag.ConsultantID = new SelectList(repository.GetQuery<Consultant>(f => f.IsActive == true), "ConsultantID", "Name", model.IncubationWorkshop.ConsultantID);
            ViewBag.EndDate = model.IncubationWorkshop.EndDate.ToString("yyyy-MM-dd");
            ViewBag.StartDate = model.IncubationWorkshop.StartDate.ToString("yyyy-MM-dd");
            ViewBag.lastTimeToApply = model.IncubationWorkshop.LastTimeToApply.ToString("yyyy-MM-dd");
            return View(model);
        }

        // GET: IncubationWorkshops/Edit/5
        [HttpGet]
        public ActionResult IncubationWorkshopEvaluationReport(Guid workshopId)
        {
            if (workshopId == null)
                return View();

            var workshopRating = repository.Get<IncubationWorkshopRating>(r => r.IncubationWorkshopID == workshopId);

            var wsEvaluationReportVM = new IncubationWorkshopEvaluationReportVM()
            {
                HowDoYouEvaluateTheWorkshop = new TrainingWorkshopVM()
                {
                    VeryGood = workshopRating.Where(v => v.IncubationWorkshopID == workshopId && v.TrainingWorkshop == TrainingWorkshop.VeryGood).Count(),
                    Average = workshopRating.Where(v => v.IncubationWorkshopID == workshopId && v.TrainingWorkshop == TrainingWorkshop.Average).Count(),
                    Bad = workshopRating.Where(v => v.IncubationWorkshopID == workshopId && v.TrainingWorkshop == TrainingWorkshop.Bad).Count()
                },
                WorkshopAchieveObjectives = new AchievingGoalVM()
                {
                    GoalsAchievedCompletely = workshopRating.Where(v => v.IncubationWorkshopID == workshopId && v.AchievingGoal == AchievingGoal.GoalsAchievedCompletely).Count(),
                    TheGoalsWerePartiallyAchieved = workshopRating.Where(v => v.IncubationWorkshopID == workshopId && v.AchievingGoal == AchievingGoal.TheGoalsWerePartiallyAchieved).Count(),
                    DidNotAchieveGoals = workshopRating.Where(v => v.IncubationWorkshopID == workshopId && v.AchievingGoal == AchievingGoal.DidNotAchieveGoals).Count()
                },
                IsWorkshopUseful = new MeetTheWorkRequirementVM()
                {
                    Useful = workshopRating.Where(v => v.IncubationWorkshopID == workshopId && v.MeetTheWorkRequirement == MeetTheWorkRequirement.Useful).Count(),
                    SomewhatUseful = workshopRating.Where(v => v.IncubationWorkshopID == workshopId && v.MeetTheWorkRequirement == MeetTheWorkRequirement.SomewhatUseful).Count(),
                    Unuseful = workshopRating.Where(v => v.IncubationWorkshopID == workshopId && v.MeetTheWorkRequirement == MeetTheWorkRequirement.UnUseful).Count()
                },
                TrainingMaterial = new TrainingWorkshopVM()
                {
                    VeryGood = workshopRating.Where(v => v.IncubationWorkshopID == workshopId && v.TrainingMaterial == TrainingMaterial.VeryGood).Count(),
                    Average = workshopRating.Where(v => v.IncubationWorkshopID == workshopId && v.TrainingMaterial == TrainingMaterial.Average).Count(),
                    Bad = workshopRating.Where(v => v.IncubationWorkshopID == workshopId && v.TrainingMaterial == TrainingMaterial.Bad).Count()
                },
                HowDoYouEvaluateParticpation = new TrainingWorkshopVM()
                {
                    VeryGood = workshopRating.Where(v => v.IncubationWorkshopID == workshopId && v.PartcipationReaction == PartcipationReaction.VeryGood).Count(),
                    Average = workshopRating.Where(v => v.IncubationWorkshopID == workshopId && v.PartcipationReaction == PartcipationReaction.Average).Count(),
                    Bad = workshopRating.Where(v => v.IncubationWorkshopID == workshopId && v.PartcipationReaction == PartcipationReaction.Bad).Count()
                },
                TheAbilityToDeliverTheInformation = new TrainerRatingVM()
                {
                    Excellent = workshopRating.Where(v => v.IncubationWorkshopID == workshopId && v.TheAbilityToDeliverInformation == TrainerRating.Excellent).Count(),
                    VGood = workshopRating.Where(v => v.IncubationWorkshopID == workshopId && v.TheAbilityToDeliverInformation == TrainerRating.VGood).Count(),
                    Good = workshopRating.Where(v => v.IncubationWorkshopID == workshopId && v.TheAbilityToDeliverInformation == TrainerRating.Good).Count(),
                    OK = workshopRating.Where(v => v.IncubationWorkshopID == workshopId && v.TheAbilityToDeliverInformation == TrainerRating.OK).Count(),
                    Bad = workshopRating.Where(v => v.IncubationWorkshopID == workshopId && v.TheAbilityToDeliverInformation == TrainerRating.Bad).Count()
                },
                BodyLanguage = new TrainerRatingVM()
                {
                    Excellent = workshopRating.Where(v => v.IncubationWorkshopID == workshopId && v.BodyLanguage == TrainerRating.Excellent).Count(),
                    VGood = workshopRating.Where(v => v.IncubationWorkshopID == workshopId && v.BodyLanguage == TrainerRating.VGood).Count(),
                    Good = workshopRating.Where(v => v.IncubationWorkshopID == workshopId && v.BodyLanguage == TrainerRating.Good).Count(),
                    OK = workshopRating.Where(v => v.IncubationWorkshopID == workshopId && v.BodyLanguage == TrainerRating.OK).Count(),
                    Bad = workshopRating.Where(v => v.IncubationWorkshopID == workshopId && v.BodyLanguage == TrainerRating.Bad).Count()
                },
                ClarityOfVoiceAndTone = new TrainerRatingVM()
                {
                    Excellent = workshopRating.Where(v => v.IncubationWorkshopID == workshopId && v.ClarityOfVoiceAndTone == TrainerRating.Excellent).Count(),
                    VGood = workshopRating.Where(v => v.IncubationWorkshopID == workshopId && v.ClarityOfVoiceAndTone == TrainerRating.VGood).Count(),
                    Good = workshopRating.Where(v => v.IncubationWorkshopID == workshopId && v.ClarityOfVoiceAndTone == TrainerRating.Good).Count(),
                    OK = workshopRating.Where(v => v.IncubationWorkshopID == workshopId && v.ClarityOfVoiceAndTone == TrainerRating.OK).Count(),
                    Bad = workshopRating.Where(v => v.IncubationWorkshopID == workshopId && v.ClarityOfVoiceAndTone == TrainerRating.Bad).Count()
                },
                MasteryOfTrainingMaterial = new TrainerRatingVM()
                {
                    Excellent = workshopRating.Where(v => v.IncubationWorkshopID == workshopId && v.MasteryOfTrainingMaterial == TrainerRating.Excellent).Count(),
                    VGood = workshopRating.Where(v => v.IncubationWorkshopID == workshopId && v.MasteryOfTrainingMaterial == TrainerRating.VGood).Count(),
                    Good = workshopRating.Where(v => v.IncubationWorkshopID == workshopId && v.MasteryOfTrainingMaterial == TrainerRating.Good).Count(),
                    OK = workshopRating.Where(v => v.IncubationWorkshopID == workshopId && v.MasteryOfTrainingMaterial == TrainerRating.OK).Count(),
                    Bad = workshopRating.Where(v => v.IncubationWorkshopID == workshopId && v.MasteryOfTrainingMaterial == TrainerRating.Bad).Count()
                },
                AbilityToManageDiscussionAndHandleQuestions = new TrainerRatingVM()
                {
                    Excellent = workshopRating.Where(v => v.IncubationWorkshopID == workshopId && v.AbilityToManageDiscussionAndHandleQuestions == TrainerRating.Excellent).Count(),
                    VGood = workshopRating.Where(v => v.IncubationWorkshopID == workshopId && v.AbilityToManageDiscussionAndHandleQuestions == TrainerRating.VGood).Count(),
                    Good = workshopRating.Where(v => v.IncubationWorkshopID == workshopId && v.AbilityToManageDiscussionAndHandleQuestions == TrainerRating.Good).Count(),
                    OK = workshopRating.Where(v => v.IncubationWorkshopID == workshopId && v.AbilityToManageDiscussionAndHandleQuestions == TrainerRating.OK).Count(),
                    Bad = workshopRating.Where(v => v.IncubationWorkshopID == workshopId && v.AbilityToManageDiscussionAndHandleQuestions == TrainerRating.Bad).Count()
                },
                LinkingTheTrainingMaterialToReality = new TrainerRatingVM()
                {
                    Excellent = workshopRating.Where(v => v.IncubationWorkshopID == workshopId && v.LinkingTheTrainingMaterialToReality == TrainerRating.Excellent).Count(),
                    VGood = workshopRating.Where(v => v.IncubationWorkshopID == workshopId && v.LinkingTheTrainingMaterialToReality == TrainerRating.VGood).Count(),
                    Good = workshopRating.Where(v => v.IncubationWorkshopID == workshopId && v.LinkingTheTrainingMaterialToReality == TrainerRating.Good).Count(),
                    OK = workshopRating.Where(v => v.IncubationWorkshopID == workshopId && v.LinkingTheTrainingMaterialToReality == TrainerRating.OK).Count(),
                    Bad = workshopRating.Where(v => v.IncubationWorkshopID == workshopId && v.LinkingTheTrainingMaterialToReality == TrainerRating.Bad).Count()
                },
                AbilityToAchieveAGoal = new TrainerRatingVM()
                {
                    Excellent = workshopRating.Where(v => v.IncubationWorkshopID == workshopId && v.AbilityToAchieveAGoal == TrainerRating.Excellent).Count(),
                    VGood = workshopRating.Where(v => v.IncubationWorkshopID == workshopId && v.AbilityToAchieveAGoal == TrainerRating.VGood).Count(),
                    Good = workshopRating.Where(v => v.IncubationWorkshopID == workshopId && v.AbilityToAchieveAGoal == TrainerRating.Good).Count(),
                    OK = workshopRating.Where(v => v.IncubationWorkshopID == workshopId && v.AbilityToAchieveAGoal == TrainerRating.OK).Count(),
                    Bad = workshopRating.Where(v => v.IncubationWorkshopID == workshopId && v.AbilityToAchieveAGoal == TrainerRating.Bad).Count()
                },
                WorkshopClass = new WorkshopClassVM()
                {
                    Fit = workshopRating.Where(v => v.IncubationWorkshopID == workshopId && v.WorkshopClass == WorkshopClass.Fit).Count(),
                    FairlyAppropriate = workshopRating.Where(v => v.IncubationWorkshopID == workshopId && v.WorkshopClass == WorkshopClass.FairlyAppropriate).Count(),
                    Unfit = workshopRating.Where(v => v.IncubationWorkshopID == workshopId && v.WorkshopClass == WorkshopClass.Unfit).Count()
                },
                Hosting = new WorkshopClassVM()
                {
                    Fit = workshopRating.Where(v => v.IncubationWorkshopID == workshopId && v.Hosting == WorkshopClass.Fit).Count(),
                    FairlyAppropriate = workshopRating.Where(v => v.IncubationWorkshopID == workshopId && v.Hosting == WorkshopClass.FairlyAppropriate).Count(),
                    Unfit = workshopRating.Where(v => v.IncubationWorkshopID == workshopId && v.Hosting == WorkshopClass.Unfit).Count()
                }
            };

            return View(wsEvaluationReportVM);
        }

        [HttpGet]
        public ActionResult WorkshopInvitations(Guid workshopId)
        {
            List<WorkshopInvitationsVM> wsInvitationVM = new List<WorkshopInvitationsVM>();
            var Invitation = repository.GetQuery<WorkshopPrivateInvitation>(p => p.IncubationWorkshopID == workshopId).ToList();
            string CorpName = null;
            string invitationStatus = null;
            foreach (var item in Invitation)
            {
                var corpAcc = repository.GetQuery<AspNetUser>(a => a.Email == item.Email).ToList();
                if (corpAcc.Count != 0)
                {
                    CorpName = corpAcc.FirstOrDefault().UserName;
                    var CorpId = Guid.Parse(corpAcc.FirstOrDefault().Id);
                    var CorpWsPP = repository.GetQuery<WorkshopProjectProposal>(p => p.FrontendUserID == CorpId && p.IncubationWorkshopID == item.IncubationWorkshopID).ToList();
                    if (CorpWsPP.Count > 0)
                    {
                        switch (CorpWsPP.FirstOrDefault().WorkshopProjectProposalStatu.NameEN)
                        {
                            case "Accepted":
                                var wsId = CorpWsPP.FirstOrDefault().IncubationWorkshopID;
                                var fuId = CorpWsPP.FirstOrDefault().FrontendUserID;
                                var aStatus = repository.GetQuery<WorkshopPrivateInvitation>(p => p.IncubationWorkshopID == wsId
                                && p.FrontendUserID == fuId).ToList().All(a => a.InvitationStatus == InvitationStatus.absent);
                                invitationStatus = aStatus == true ? "Absent" : "Attend";
                                break;
                            case "Rejected":
                                invitationStatus = "Rejected";
                                break;
                            case "Absent":
                                invitationStatus = "Cancel";
                                break;
                            default:
                                invitationStatus = CorpWsPP.FirstOrDefault().WorkshopProjectProposalStatu.NameEN == "Update Project Proposal" ? "Pending" : "WaitingList";
                                break;
                        }
                    }
                    else
                    {
                        invitationStatus = "Pending";
                    }

                }
                wsInvitationVM.Add(new WorkshopInvitationsVM()
                {
                    WorkshopName = item.IncubationWorkshop.Name,
                    WorkshopModel = item.IncubationWorkshop.IncubationWorkshopModel.NameEN,
                    CorporateName = CorpName == null ? item.Email : CorpName,
                    InvitationStatus = invitationStatus == null ? "Pending" : invitationStatus
                });
                CorpName = null;
                invitationStatus = null;
            }

            return View(wsInvitationVM);
        }

        [HttpGet]
        public ActionResult AttendeesList(Guid workshopId)
        {
            var res = repository.GetQuery<WorkshopPrivateInvitation>(w => w.IncubationWorkshopID == workshopId && w.IncubationWorkshop.WorkshopProjectProposals.Where(p => p.WorkshopProjectProposalStatu.NameEN == "Accepted" && p.WorkshopPP_InvitationStatus == WorkshopPPInvitationStatus.attend).Any(a => a.FrontendUser.AspNetUser.Email == w.Email)).ToList();
            
            return View(res);
        }



        // 24-2025 - جلب تفاصيل دعوة ورشة العمل
        [HttpGet]
        public JsonResult GetInvitationDetails(string workshopID, string email)
        {
            var invitation = repository.GetQuery<WorkshopPrivateInvitation>()
                                       .Where(i => i.IncubationWorkshopID.ToString() == workshopID && i.Email == email)
                                       .Select(i => new
                                       {
                                           i.WorkshopPrivateInvitationId,  
                                   i.Email,
                                   OrganizationName = i.FrontendUser.CorporateApplicationForms.FirstOrDefault().Name ?? "غير متوفر"
                                       })
                                       .FirstOrDefault();

            return Json(invitation, JsonRequestBehavior.AllowGet);
        }
        //4-3-2025
        [HttpGet]
        public ActionResult CanceledWorkshopInvitationsReport(string DateFrom, string DateTo, string WorkshopType)
        {
            var canceledInvitations = repository.GetQuery<WorkshopPrivateInvitation>()
                .Where(i => i.InvitationStatus == InvitationStatus.cancel);

            // تصفية حسب نوع الورشة
            if (!string.IsNullOrEmpty(WorkshopType))
            {
                canceledInvitations = canceledInvitations.Where(i => i.IncubationWorkshop.IncubationWorkshopModel.NameEN == WorkshopType);
            }

            // تصفية حسب تاريخ الإلغاء
            if (!string.IsNullOrEmpty(DateFrom))
            {
                DateTime fromDate = DateTime.Parse(DateFrom);
                canceledInvitations = canceledInvitations.Where(i => i.CreatedDate >= fromDate);
            }
            if (!string.IsNullOrEmpty(DateTo))
            {
                DateTime toDate = DateTime.Parse(DateTo);
                canceledInvitations = canceledInvitations.Where(i => i.CreatedDate <= toDate);
            }

            var result = canceledInvitations.Select(i => new CanceledWorkshopInvitationsReportVM
            {
                WorkshopType = CultureHelper.CurrentCulture == 3 ? i.IncubationWorkshop.IncubationWorkshopModel.NameAR : i.IncubationWorkshop.IncubationWorkshopModel.NameEN,
                WorkshopName = i.IncubationWorkshop.Name,
                Email = i.Email,
                OrganizationName = i.FrontendUser != null ? i.FrontendUser.CorporateApplicationForms.FirstOrDefault().Name : "غير متوفر",
                InvitationDate = i.CreatedDate,
                CancellationDate = i.UpdatedDate.HasValue ? i.UpdatedDate.Value : (DateTime?)null // تاريخ الإلغاء
            }).ToList();

            return View(result);
        }

        //2-24-2025

        [HttpPost]
        public JsonResult CancelInvitation(Guid invitationId)
        {
            try
            {
                
                var invitation = repository.GetByKey<WorkshopPrivateInvitation>(invitationId);

                //  التحقق مما إذا كان الإعلان منتهي الصلاحية
                if (invitation.IncubationWorkshop.EndDate < DateTime.Now)
                {
                    return Json(" لا يمكن إرسال الدعوة لأن فترة الإعلان قد انتهت.", JsonRequestBehavior.AllowGet);
                }
                if (invitation == null)
                    return Json(new { success = false, message = "الدعوة غير موجودة" });

                // التحقق مما إذا كانت الدعوة قد ألغيت مسبقًا
                if (invitation.InvitationStatus == InvitationStatus.cancel)
                    return Json(new { success = false, message = "تم إلغاء الدعوة مسبقًا" });

                // تحديث حالة الدعوة إلى "cancel"
                invitation.InvitationStatus = InvitationStatus.cancel;
                invitation.UpdatedDate = DateTime.Now;
                repository.Update(invitation);
                repository.UnitOfWork.SaveChanges();

                // جلب تفاصيل ورشة العمل المرتبطة بالدعوة
                var workshop = repository.GetByKey<IncubationWorkshop>(invitation.IncubationWorkshopID);
                if (workshop == null)
                    return Json(new { success = false, message = "لم يتم العثور على ورشة العمل المرتبطة" });

                // إعداد البريد الإلكتروني
                MailHelper mailHelper = new MailHelper
                {
                    ToEmail = invitation.Email,
                    Subject = $"إلغاء دعوتكم لحضور ورشة العمل: {workshop.Name}",
                    Body = GenerateCancelEmailBody(workshop.Name, workshop.StartDate),
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



        private string GenerateCancelEmailBody(string workshopName, DateTime startDate)
        {
            return $"<p>المكرمين/ المنظمات غير الربحية الموقرين،</p>"
                   + "<p>السلام عليكم ورحمة الله وبركاته،</p>"
                   + $"<p>نود إبلاغكم بأنه تم <strong>إلغاء</strong> دعوتكم لحضور ورشة العمل <strong>{workshopName}</strong>.</p>"
                   + $"<p>كان من المقرر أن تبدأ الورشة في: <strong>{startDate:dd/MM/yyyy}</strong>.</p>"
                   + "<p>إذا كان لديكم أي استفسارات، يرجى التواصل معنا.</p>"
                   + "<p>مع تمنياتنا لكم بالتوفيق،</p>"
                   + "<p><strong>برنامج بناء القدرات.</strong></p>";
        }



        //4-3-2025
        [HttpGet]
        public JsonResult AcceptRejectAttend(Guid Id, string RequestType)
        {
            var Att = repository.GetByKey<WorkshopPrivateInvitation>(Id);
            if (Att != null)
            {
                Att.InvitationStatus = RequestType == "Accepted" ? InvitationStatus.attend : InvitationStatus.absent;
                repository.Update(Att);
                repository.UnitOfWork.SaveChanges();

                if (RequestType == "Accepted")
                {
                    MailHelper mailHelper = new MailHelper();
                    mailHelper.ToEmail = Att.Email;
                    mailHelper.Subject = $"رأيكم يهمنا";
                    mailHelper.IsHtml = true;
                    mailHelper.Body = "المكرمين/ المنظمات غير الربحية                         الموقرين<br/>"
                                            + "السلام عليكم ورحمة الله وبركاته، <br/>"
                                            + "تهديكم مؤسسة الملك خالد أطيب التحيات، <br/>"
                                            + $"ونشكر لكم حضوركم لورشة عمل {Att.IncubationWorkshop.Name}. <br/>"
                                            + "ولأهمية التقييم في التطوير والتحسين، <br/>"
                                            + "نرجو منكم التكرم بزيارة البوابة الإلكترونية لتعبئة نموذج تقييم ورشة العمل. <br/>"
                                            + "شاكرين ومقدرين تعاونكم، <br/>"
                                            + " برنامج بناء القدرات. <br/>";

                    var fundingSource = repository.GetByKey<FundingSource>(Att.IncubationWorkshop.FundingSourceID);
                    if (fundingSource.UseCustomThemes)
                        mailHelper.Send($"?partner={fundingSource.Nickname}");
                    else
                        mailHelper.Send("");
                }
                else
                {
                    MailHelper mailHelper = new MailHelper();
                    mailHelper.ToEmail = Att.Email;
                    mailHelper.Subject = $"تنويه";
                    mailHelper.IsHtml = true;
                    mailHelper.Body = "المكرمين/ المنظمات غير الربحية                         الموقرين<br/>"
                                            + "السلام عليكم ورحمة الله وبركاته، <br/>"
                                            + "تهديكم مؤسسة الملك خالد أطيب التحيات، <br/>"
                                            + $"ويؤسفنا عدم حضور المرشح/المرشحين من طرفكم لورشة عمل {Att.IncubationWorkshop.Name} بعد تأكيدكم المسبق للحضور، عليه، <br/>"
                                            + "يرجى التكرم بإرسال بريد إلكتروني على البريد CBP@KKF.org.sa لتوضيح أسباب عدم مشاركتكم. <br/>"
                                            + "شاكرين ومقدرين تعاونكم، <br/>"
                                            + " برنامج بناء القدرات. <br/>";

                    var fundingSource = repository.GetByKey<FundingSource>(Att.IncubationWorkshop.FundingSourceID);
                    if (fundingSource.UseCustomThemes)
                        mailHelper.Send($"?partner={fundingSource.Nickname}");
                    else
                        mailHelper.Send("");
                }
            }
            return Json(RequestType, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ReverseAttend(Guid Id)
        {
            var Att = repository.GetQuery<WorkshopPrivateInvitation>(f => f.WorkshopPrivateInvitationId == Id).FirstOrDefault();
            if (Att != null)
            {
                Att.InvitationStatus = InvitationStatus.pending;
                repository.Update(Att);
                repository.UnitOfWork.SaveChanges();
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetDataEmp(Guid workshoopId, Guid FrontId)
        {
            var res = repository.GetQuery<EmployeesAttendIncubationWorkShop>(f => f.WorkshopProjectProposal.IncubationWorkshop.WorkshopPrivateInvitations.Any(t => t.FrontendUserID == FrontId && t.IncubationWorkshopID == workshoopId) && f.WorkshopProjectProposal.FrontendUserID == FrontId)
                .Select(f => new { Name = f.Name, Gender = f.Gender, Email = f.Email, Mobile = f.Mobile, EducationalQualificationAndSpecialization = f.EducationalQualificationAndSpecialization, Position = f.Position, PositionTasks = f.PositionTasks, lstAttachment = f.WorkshopProjectProposal.WorkshopProjectProposalAttachments.Select(c => new { AttachmentID = c.AttachmentID, Name = c.Name, Type = c.Type, Size = c.Size }) }).ToList();
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        //2-24-2025
        private string GenerateWorkshopEmailBody(string workshopName, DateTime lastTimeToApply)
        {
            return $"المكرمين/ المنظمات غير الربحية الموقرين،<br/>"
                   + "السلام عليكم ورحمة الله وبركاته، <br/>"
                   + "تهديكم مؤسسة الملك خالد أطيب التحيات، <br/>"
                   + "وبناءً على قبول منظمتكم في ورش العمل التدريبية ببرنامج بناء القدرات، عليه، <br/>"
                   + $"نحيطكم علماً بأنه تم فتح باب التقديم على ورشة عمل <strong>{workshopName}</strong> ضمن مشاريع برنامج بناء القدرات.<br/>"
                   + $"نأمل منكم التكرم بزيارة البوابة الإلكترونية وتعبئة طلب الالتحاق بورشة العمل وإرسال إقرار الالتزام في موعد أقصاه: <strong>{lastTimeToApply:dd/MM/yyyy}</strong> <br/>"
                   + "مع تمنياتنا لكم بالتوفيق،<br/>"
                   + "<strong>برنامج بناء القدرات.</strong>";
        }
        // 24-2-2025
        [HttpGet]
        public JsonResult CreateNewInvitation(string workshopID, string email)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(email) && !string.IsNullOrWhiteSpace(workshopID))
                {
                    var incubationWorkshop = repository.GetByKey<IncubationWorkshop>(Guid.Parse(workshopID));

                    ////  التحقق مما إذا كانت فترة التقديم على الورشة قد انتهت
                    //if (incubationWorkshop.LastTimeToApply < DateTime.Now)
                    //{
                    //    return Json(" لا يمكن إرسال الدعوة لأن فترة التقديم قد انتهت.", JsonRequestBehavior.AllowGet);
                    //}
                    if (incubationWorkshop.LastTimeToApply < DateTime.Now.AddHours(-1))
                    {
                        return Json(" لا يمكن إرسال الدعوة لأن فترة التقديم قد انتهت.", JsonRequestBehavior.AllowGet);
                    }

                    //  البحث عن أي دعوة سابقة بنفس البريد وحالتها ليست ملغاة
                    var existingInvitation = incubationWorkshop.WorkshopPrivateInvitations
                                                               .FirstOrDefault(e => e.Email == email && e.InvitationStatus != InvitationStatus.cancel);

                    if (existingInvitation == null)
                    {
                        //  البحث عن المستخدم في `FrontendUser` بناءً على البريد الإلكتروني
                        var frontendUser = repository.GetQuery<FrontendUser>()
                                                    .FirstOrDefault(fu => fu.AspNetUser.Email == email);

                        //  إنشاء سجل جديد للدعوة
                        WorkshopPrivateInvitation _Private = new WorkshopPrivateInvitation
                        {
                            WorkshopPrivateInvitationId = Guid.NewGuid(),
                            Email = email.Trim(),
                            IncubationWorkshopID = incubationWorkshop.IncubationWorkshopID,
                            InvitationStatus = InvitationStatus.pending, // تعيين الحالة "pending"
                            FrontendUserID = frontendUser?.UserID, // ربط المستخدم إذا كان موجودًا
                            CreatedDate = DateTime.Now // تعيين التاريخ والوقت الحالي
                        };

                        repository.Add(_Private);
                        repository.UnitOfWork.SaveChanges();

                      
                        MailHelper mailHelper = new MailHelper
                        {
                            ToEmail = email.Trim(),
                            IsHtml = true,
                            Subject = $"بدء التقديم على ورشة عمل {incubationWorkshop.Name}",
                            Body = GenerateWorkshopEmailBody(incubationWorkshop.Name, incubationWorkshop.LastTimeToApply)
                        };

                        var fundingSource = repository.GetByKey<FundingSource>(incubationWorkshop.FundingSourceID);
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


        //[HttpGet]
        //public JsonResult CreateNewInvitation(string workshopID, string email)
        //{
        //    try
        //    {
        //        if (!string.IsNullOrWhiteSpace(email) || !string.IsNullOrWhiteSpace(workshopID))
        //        {
        //            var incubationWorkshop = repository.GetByKey<IncubationWorkshop>(Guid.Parse(workshopID));
        //            var frontendUserId = repository.GetQuery<AspNetUser>(a => a.Email == email).ToList();
        //            if (!incubationWorkshop.WorkshopPrivateInvitations.Any(e => e.Email == email))
        //            {
        //                WorkshopPrivateInvitation _Private = new WorkshopPrivateInvitation()
        //                {
        //                    Email = email,
        //                    InvitationStatus = InvitationStatus.pending,
        //                    WorkshopPrivateInvitationId = Guid.NewGuid(),
        //                    IncubationWorkshopID = incubationWorkshop.IncubationWorkshopID,
        //                    FrontendUserID = frontendUserId.Count == 0 ? (Guid?)null : Guid.Parse(frontendUserId.FirstOrDefault().Id),
        //                    //2-24-2025
        //                   CreatedDate = DateTime.Now,
        //                };
        //                repository.Add(_Private);
        //                repository.UnitOfWork.SaveChanges();

        //                MailHelper mailHelper = new MailHelper()
        //                {
        //                    IsHtml = true,
        //                    ToEmail = email,
        //                    Subject = $"بدء التقديم على ورشة عمل {incubationWorkshop.Name}",
        //                    Body = "المكرمين/ المنظمات غير الربحية                         الموقرين<br/>"
        //                         + "السلام عليكم ورحمة الله وبركاته، <br/>"
        //                         + "تهديكم مؤسسة الملك خالد أطيب التحيات، <br/> وبناءً على قبول منظمتكم في ورش العمل التدريبية ببرنامج بناء القدرات، عليه، <br/>"
        //                         + $"نحيطكم علماً بأنه تم فتح باب التقديم على ورشة عمل {incubationWorkshop.Name} ضمن مشاريع برنامج بناء القدرات. <br/>"
        //                         + $"نأمل منكم التكرم بزيارة البوابة الإلكترونية وتعبئة طلب الالتحاق بورشة العمل وإرسال إقرار الالتزام في موعد اقصاه : {incubationWorkshop.LastTimeToApply.ToString("dd/MM/yyyy")} <br/>"
        //                         + "مع تمنياتنا لكم بالتوفيق، <br/>"
        //                         + " برنامج بناء القدرات. <br/>"
        //                };

        //                var fundingSource = repository.GetByKey<FundingSource>(incubationWorkshop.FundingSourceID);
        //                if (fundingSource.UseCustomThemes)
        //                    mailHelper.Send($"?partner={fundingSource.Nickname}");
        //                else
        //                    mailHelper.Send("");

        //                return Json("Done", JsonRequestBehavior.AllowGet);
        //            }
        //            else
        //                return Json("Error", JsonRequestBehavior.AllowGet);
        //        }
        //        else
        //            return Json("Error", JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception)
        //    {
        //        return Json("Error", JsonRequestBehavior.AllowGet);
        //    }
        //}

        [HttpGet]
        public FileResult DownloadAttend(Guid id)
        {
            var attachmet = repository.GetByKey<WorkshopProjectProposalAttachment>(id);
            byte[] fileBytes = System.IO.File.ReadAllBytes(attachmet.URL);
            string fileName = attachmet.Name;
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
     //   17-1-2026
        [HttpGet]
        public ActionResult Search(string term)
        {
            term = (term ?? "").Trim();

            var res = repository.GetQuery<AspNetUser>(u =>
                    u.AspNetRoles.Any(r => r.Name == "Corporation" || r.Name == "Corporation IndIvidual" || r.Name == "IndIvidual")
                    && u.FrontendUsers.Any(fu => fu.CorporateApplicationForms.Any(c => c.Program.ProgramName == "Capacity Building"))
                    && (
                        u.Email.Contains(term)
                        || u.FrontendUsers.Any(fu =>
                            fu.CorporateApplicationForms.Any(c => c.Name.Contains(term))
                        )
                    )
                )
                .Select(u => new
                {
       
            label =
                        u.FrontendUsers
                         .SelectMany(fu => fu.CorporateApplicationForms)
                         .Where(c => c.Program.ProgramName == "Capacity Building")
                         .Select(c => c.Name)
                         .FirstOrDefault()
                        + " - " + u.Email,

       
            value = u.Email
                })
                .Distinct()
                .Take(20)
                .ToList();

            return Json(res, JsonRequestBehavior.AllowGet);
        }

        //[HttpGet]
        //public ActionResult Search(string term)
        //{
        //    var res = repository.GetQuery<AspNetUser>(f => f.AspNetRoles.Any(r => r.Name == "Corporation" || r.Name == "Corporation IndIvidual" || r.Name == "IndIvidual") && f.FrontendUsers.Any(w => w.CorporateApplicationForms.Any(c => c.Program.ProgramName == "Capacity Building")) && f.Email.Contains(term)).Select(t => t.Email).ToList();
        //    return Json(res, JsonRequestBehavior.AllowGet);
        //}

        [HttpGet]
        public JsonResult GetGovernorates(Guid Region)
        {
            if (!string.IsNullOrWhiteSpace(Region.ToString()) && Region.ToString().Length != 0)
            {
                IEnumerable<SelectListItem> Sectors = repository.Get<Governorate>(f => f.RegionID == Region && f.IsActive == true)
                     .Select(n =>
                      new SelectListItem
                      {
                          Value = n.GovernorateID.ToString(),
                          Text = CultureHelper.CurrentCulture != 3 ? n.GovernorateEN : n.GovernorateAR
                      }).ToList();

                return Json(new SelectList(Sectors, "Value", "Text"), JsonRequestBehavior.AllowGet);
            }
            return null;
        }

        [HttpGet]
        public JsonResult GetCities(Guid Governorate)
        {
            if (!string.IsNullOrWhiteSpace(Governorate.ToString()) && Governorate.ToString().Length != 0)
            {
                IEnumerable<SelectListItem> Sectors = repository.Get<City>(f => f.GovernorateID == Governorate && f.IsActive == true)
                     .Select(n =>
                      new SelectListItem
                      {
                          Value = n.CityID.ToString(),
                          Text = CultureHelper.CurrentCulture != 3 ? n.CityNameEN : n.CityNameAR
                      }).ToList();

                return Json(new SelectList(Sectors, "Value", "Text"), JsonRequestBehavior.AllowGet);
            }
            return null;
        }

        [HttpGet]
        public JsonResult DeleteAttachment(Guid Id)
        {
            try
            {
                var Attachment = repository.GetByKey<IncubationWorkshopAttachment>(Id);
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
        public FileResult DownloadCommitmentFile(Guid id)
        {
            var attachment = repository.GetByKey<IncubationWorkshopAttachment>(id);
            string filePath = Server.MapPath(attachment.URL);

            if (!System.IO.File.Exists(filePath))
                throw new FileNotFoundException("ملف إقرار الالتزام غير موجود", filePath);

            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, attachment.Name);
        }

        public FileResult Download(Guid id)
        {
            var attachmet = repository.GetByKey<IncubationWorkshopAttachment>(id);
            byte[] fileBytes = System.IO.File.ReadAllBytes(attachmet.URL);
            string fileName = attachmet.Name;
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }



        // 2-2-2026

[HttpGet]
    [Authorize(Roles = "Admin, CB Manager, CB Supervisor, CB Analyst")]
    public ActionResult ExportAttendanceToExcel(Guid workshopId)
    {
        try
        {
            var isAr = (CultureHelper.CurrentCulture == 3);
            string L(string ar, string en) => isAr ? ar : en;

            var workshop = repository.GetByKey<IncubationWorkshop>(workshopId);
            if (workshop == null) return HttpNotFound("Workshop not found");

        
            var attendingCorpIds = repository.GetQuery<WorkshopPrivateInvitation>()
                .Where(i =>
                    i.IncubationWorkshopID == workshopId
                    && i.InvitationStatus == InvitationStatus.attend
                    && i.FrontendUserID.HasValue
                )
                .Select(i => i.FrontendUserID.Value)
                .Distinct()
                .ToList();

        
            if (!attendingCorpIds.Any())
            {
             
                var emptyDt = new DataTable();
                emptyDt.Columns.Add(L("الجمعية", "Organization"));
                emptyDt.Columns.Add(L("اسم المشارك", "Participant Name"));
                emptyDt.Columns.Add(L("المنصب", "Position"));
                emptyDt.Columns.Add(L("البريد الإلكتروني", "Email"));
                emptyDt.Columns.Add(L("رقم الجوال", "Mobile"));
                emptyDt.Columns.Add(L("الحالة", "Status"));

                var emptyGv = new GridView { DataSource = emptyDt };
                emptyGv.DataBind();

                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", $"attachment; filename={workshop.Name}-AttendanceReport.xls");
                Response.ContentType = "application/ms-excel";
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Write("<meta http-equiv='Content-Type' content='text/html; charset=UTF-8'>");

                using (var sw0 = new StringWriter())
                using (var htw0 = new HtmlTextWriter(sw0))
                {
                    emptyGv.RenderControl(htw0);
                    Response.Output.Write(sw0.ToString());
                }

                Response.Flush();
                Response.End();
                return new EmptyResult();
            }

           
            var rows = repository.GetQuery<EmployeesAttendIncubationWorkShop>()
                .Where(e =>
                    e.WorkshopProjectProposal.IncubationWorkshopID == workshopId
                    && attendingCorpIds.Contains(e.WorkshopProjectProposal.FrontendUserID)
                )
                .Select(e => new
                {
                    Organization =
                        e.WorkshopProjectProposal.FrontendUser.CorporateApplicationForms
                            .Select(c => c.Name)
                            .FirstOrDefault(),

                    Name = e.Name,
                    Position = e.Position,
                    Email = e.Email,
                    Mobile = e.Mobile
                })
                .ToList()
                .Select(x => new
                {
                    Organization = string.IsNullOrWhiteSpace(x.Organization) ? L("غير معروف", "Unknown") : x.Organization,
                    x.Name,
                    x.Position,
                    x.Email,
                    x.Mobile,
                    Status = L("شارك", "Attend") 
            })
                .OrderBy(x => x.Organization)
                .ThenBy(x => x.Name)
                .ToList();

 
            var dt = new DataTable();
            dt.Columns.Add(L("الجمعية", "Organization"));
            dt.Columns.Add(L("اسم المشارك", "Participant Name"));
            dt.Columns.Add(L("المنصب", "Position"));
            dt.Columns.Add(L("البريد الإلكتروني", "Email"));
            dt.Columns.Add(L("رقم الجوال", "Mobile"));
            dt.Columns.Add(L("الحالة", "Status"));

            foreach (var r in rows)
            {
                var dr = dt.NewRow();
                dr[0] = r.Organization;
                dr[1] = r.Name;
                dr[2] = r.Position;
                dr[3] = r.Email;
                dr[4] = r.Mobile;
                dr[5] = r.Status;
                dt.Rows.Add(dr);
            }

            var gv = new GridView { DataSource = dt };
            gv.DataBind();

           

            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", $"attachment; filename={workshop.Name}-AttendanceReport.xls");
            Response.ContentType = "application/ms-excel";
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.Write("<meta http-equiv='Content-Type' content='text/html; charset=UTF-8'>");

            using (var sw = new StringWriter())
            using (var htw = new HtmlTextWriter(sw))
            {
                gv.RenderControl(htw);
                Response.Output.Write(sw.ToString());
            }

            Response.Flush();
            Response.End();
            return new EmptyResult();
        }
        catch (System.Threading.ThreadAbortException)
        {
            return new EmptyResult();
        }
        catch (Exception ex)
        {
            return View("Error", new HandleErrorInfo(ex, "IncubationWorkshops", "ExportAttendanceToExcel"));
        }
    }

    //[HttpGet]
    //    [Authorize(Roles = "Admin, CB Manager, CB Supervisor, CB Analyst")]
    //    public ActionResult ExportAttendanceToExcel(Guid workshopId)
    //    {
    //        try
    //        {
    //            var isAr = (CultureHelper.CurrentCulture == 3);
    //            string L(string ar, string en) => isAr ? ar : en;

    //            var workshop = repository.GetByKey<IncubationWorkshop>(workshopId);
    //            if (workshop == null) return HttpNotFound("Workshop not found");
    //            var rows = repository.GetQuery<EmployeesAttendIncubationWorkShop>()
    //                .Where(e =>
    //                    e.WorkshopProjectProposal.IncubationWorkshopID == workshopId
    //                    && e.WorkshopProjectProposal.WorkshopProjectProposalStatu.NameEN == "Accepted"
    //                    && e.WorkshopProjectProposal.WorkshopPP_InvitationStatus == WorkshopPPInvitationStatus.attend
    //                )
    //                .Select(e => new
    //                {
    //                    Organization =
    //                        e.WorkshopProjectProposal.FrontendUser.CorporateApplicationForms
    //                            .Select(c => c.Name)
    //                            .FirstOrDefault(),

    //                    Name = e.Name,
    //                    Position = e.Position,
    //                    Email = e.Email,
    //                    Mobile = e.Mobile
    //                })
    //                .ToList()
    //                .Select(x => new
    //                {
    //                    Organization = string.IsNullOrWhiteSpace(x.Organization) ? L("غير معروف", "Unknown") : x.Organization,
    //                    x.Name,
    //                    x.Position,
    //                    x.Email,
    //                    x.Mobile,
    //                    Status = L("شارك", "Attend")
    //                })
    //                .OrderBy(x => x.Organization)  
    //                .ThenBy(x => x.Name)
    //                .ToList();


    //            var dt = new DataTable();
    //            dt.Columns.Add(L("الجمعية", "Organization"));
    //            dt.Columns.Add(L("اسم المشارك", "Participant Name"));
    //            dt.Columns.Add(L("المنصب", "Position"));
    //            dt.Columns.Add(L("البريد الإلكتروني", "Email"));
    //            dt.Columns.Add(L("رقم الجوال", "Mobile"));
    //            dt.Columns.Add(L("الحالة", "Status"));

    //            foreach (var r in rows)
    //            {
    //                var dr = dt.NewRow();
    //                dr[0] = r.Organization;
    //                dr[1] = r.Name;
    //                dr[2] = r.Position;
    //                dr[3] = r.Email;
    //                dr[4] = r.Mobile;
    //                dr[5] = r.Status; 
    //                dt.Rows.Add(dr);
    //            }

    //            var gv = new GridView { DataSource = dt };
    //            gv.DataBind();

    //            Response.ClearContent();
    //            Response.Buffer = true;
    //            Response.AddHeader("content-disposition", $"attachment; filename={workshop.Name}-AttendanceReport.xls");
    //            Response.ContentType = "application/ms-excel";
    //            Response.ContentEncoding = System.Text.Encoding.UTF8;
    //            Response.Write("<meta http-equiv='Content-Type' content='text/html; charset=UTF-8'>");

    //            using (var sw = new StringWriter())
    //            using (var htw = new HtmlTextWriter(sw))
    //            {
    //                gv.RenderControl(htw);
    //                Response.Output.Write(sw.ToString());
    //            }

    //            Response.Flush();
    //            Response.End();
    //            return new EmptyResult();
    //        }
    //        catch (System.Threading.ThreadAbortException)
    //        {
    //            return new EmptyResult();
    //        }
    //        catch (Exception ex)
    //        {
    //            return View("Error", new HandleErrorInfo(ex, "IncubationWorkshops", "ExportAttendanceToExcel"));
    //        }
    //    }




    private string GetStatusText(
            bool isAr,
            string proposalStatusEN,
            string proposalStatusAR,
            WorkshopPPInvitationStatus invitationStatus,
            string L_Attend,
            string L_Absent
        )
        {
      
            if (string.Equals(proposalStatusEN, "Accepted", StringComparison.OrdinalIgnoreCase))
            {
                if (invitationStatus == WorkshopPPInvitationStatus.attend)
                    return L_Attend;

         
                return L_Absent;
            }

         
            return isAr
                ? (string.IsNullOrWhiteSpace(proposalStatusAR) ? proposalStatusEN : proposalStatusAR)
                : proposalStatusEN;
        }

        ////2-2-2026
        //[HttpGet]
        //[Authorize(Roles = "Admin, CB Manager, CB Supervisor, CB Analyst")]
        //public ActionResult ExportAttendanceToExcel(Guid workshopId)
        //{
        //    try
        //    {
        //        ViewBag.lang = CultureHelper.CurrentCulture;


        //        var workshop = repository.GetByKey<IncubationWorkshop>(workshopId);
        //        if (workshop == null)
        //            return HttpNotFound("Workshop not found");


        //        var mergedData =
        //            repository.GetQuery<EmployeesAttendIncubationWorkShop>()
        //            .Where(e =>
        //                e.WorkshopProjectProposal.IncubationWorkshopID == workshopId

        //            )
        //            .Select(e => new
        //            {
        //                WorkshopName = workshop.Name,

        //                NominationOrganization =
        //                    e.WorkshopProjectProposal.FrontendUser.CorporateApplicationForms
        //                        .Select(c => c.Name)
        //                        .FirstOrDefault(),

        //                NominationName = e.Name,
        //                NominationPosition = e.Position,
        //                NominationEmail = e.Email,
        //                NominationMobile = e.Mobile
        //            })
        //            .ToList();


        //        var gv = new GridView
        //        {
        //            DataSource = mergedData
        //        };
        //        gv.DataBind();


        //        Response.ClearContent();
        //        Response.Buffer = true;
        //        Response.AddHeader("content-disposition", "attachment; filename=" + workshop.Name + "-AttendanceReport.xls");
        //        Response.ContentType = "application/ms-excel";
        //        Response.ContentEncoding = System.Text.Encoding.UTF8;


        //        Response.Write("<meta http-equiv='Content-Type' content='text/html; charset=UTF-8'>");

        //        using (var sw = new StringWriter())
        //        using (var htw = new HtmlTextWriter(sw))
        //        {
        //            gv.RenderControl(htw);
        //            Response.Output.Write(sw.ToString());
        //        }

        //        Response.Flush();
        //        Response.End();


        //        return new EmptyResult();
        //    }
        //    catch (System.Threading.ThreadAbortException)
        //    {

        //        return new EmptyResult();
        //    }
        //    catch (Exception ex)
        //    {
        //        return View("Error", new HandleErrorInfo(ex, "IncubationWorkshops", "ExportAttendanceToExcel"));
        //    }
        //}
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