using System;
using System.Linq;
using System.Web.Mvc;
using AlphaPeople.Core;
using System.Data.Entity;
using System.Configuration;
using AlphaPeople.Repository;
using System.Collections.Generic;
using AlfaPeople.KingKhalidFoundation.Data;
using AlfaPeople.KingKhalidFoundation.Data.Model;
using AlfaPeople.KingKhaledFoundation.Admin.Web.Models;

namespace AlfaPeople.KingKhaledFoundation.Admin.Web.Controllers
{
    [Authorize(Roles = "Admin, CB Manager, CB Supervisor, CB Analyst")]
    public class FollowUpProjectPlanRequestController : BaseController
    {

        #region Memmbers&Fields
        private readonly IRepository repository;
        private readonly CommonHelper helper;
        private readonly string Logpath = ConfigurationManager.AppSettings["Log"].ToString();
        #endregion

        #region Ctor
        public FollowUpProjectPlanRequestController()
        {
            repository = new Repository(new KingkhaledFoundationDB());
            helper = new CommonHelper();
        }
        #endregion

        #region Action
        // GET: FollowUpProjectPlanRequest
        public ActionResult Index()
        {
            var Entities = repository.GetQuery<FollowUpProjectPlanRequest>(r => r.FollowUpProjectPlanStatus != FollowUpProjectPlanStatus.Pending)
                .Include(r => r.FollowUpProjectPlan)
                .Include(r => r.FollowUpProjectPlan.IncubationWorkshop)
                .Include(r => r.FrontendUser)
                .Include(r => r.FrontendUser.AspNetUser)
                .ToList();
            return View(Entities);
        }

        public ActionResult Details(Guid Id)
        {
            var Entity = repository.GetQuery<FollowUpProjectPlanRequest>(r => r.FollowUpProjectPlanRequestId == Id)
                .Include(r => r.FollowUpProjectPlan)
                .Include(r => r.FollowUpProjectPlan.IncubationWorkshop)
                .Include(r => r.FollowUpProjectPlan.FollowUpProjectPlanAttachments)
                .SingleOrDefault();

            var ViewModel = new FollowUpProjectPlanJoinVM()
            {
                FollowUpProjectPlan = Entity.FollowUpProjectPlan,
                FollowUpProjectPlanRequest = Entity
            };
            var FollowUpPPStatus = new List<SelectListItem> {
               new SelectListItem {Value = ((int)FollowUpProjectPlanStatus.Accept).ToString(), Text =  AlfaPeople.KingKhalidFoundation.Data.Resources.FollowUpProjectPlanStatus.Accept },
               new SelectListItem {Value = ((int)FollowUpProjectPlanStatus.UpdateProjectPlan).ToString(), Text = AlfaPeople.KingKhalidFoundation.Data.Resources.FollowUpProjectPlanStatus.UpdateProjectPlan},
            };
            ViewBag.FollowUpPPStatus = new SelectList(FollowUpPPStatus, "Value", "Text");

            return View(ViewModel);
        }

        public ActionResult Edit(FollowUpProjectPlanJoinVM Model)
        {
            var Entity = repository.GetQuery<FollowUpProjectPlanRequest>(r => r.FollowUpProjectPlanRequestId == Model.FollowUpProjectPlanRequest.FollowUpProjectPlanRequestId)
                .Include(r => r.FollowUpProjectPlan)
                .Include(r => r.FollowUpProjectPlanRequestAttachments)
                .Include(r => r.FollowUpProjectPlan.IncubationWorkshop)
                .Include(r => r.FollowUpProjectPlan.FollowUpProjectPlanAttachments)
                .SingleOrDefault();

            Entity.FollowUpProjectPlanStatus = Model.FollowUpProjectPlanRequest.FollowUpProjectPlanStatus;
            Entity.feedBack = Model.FollowUpProjectPlanRequest.feedBack;
            repository.Update(Entity);
            repository.UnitOfWork.SaveChanges();


            var corperation = Entity.FrontendUser?.AspNetUser?.Email;

            if (corperation != null)
            {
                MailHelper mailHelper = new MailHelper();
                mailHelper.ToEmail = corperation;
                if (Entity.FollowUpProjectPlanStatus == FollowUpProjectPlanStatus.Accept)
                {
                    mailHelper.Subject = " قبول مخرجات ورشة عمل ";
                    mailHelper.Body = "المكرمين/ المنظمات غير الربحية                         الموقرين<br/>"
                                    + "السلام عليكم ورحمة الله وبركاته، <br/>"
                                    + "تهديكم مؤسسة الملك خالد أطيب التحيات، <br/>"
                                    + $"ونفيدكم بقبول مخرجات خطة العمل الخاصة بورشة عمل {Entity.FollowUpProjectPlan?.IncubationWorkshop?.Name}. <br/>"
                                    + "شاكرين ومقدرين تفاعلكم، <br/>"
                                    + " برنامج بناء القدرات. <br/>";
                }
                else if (Entity.FollowUpProjectPlanStatus == FollowUpProjectPlanStatus.UpdateProjectPlan)
                {
                    mailHelper.Subject = "استكمال مخرجات ورشة عمل ";
                    mailHelper.Body = "المكرمين/ المنظمات غير الربحية                         الموقرين<br/>"
                                    + "السلام عليكم ورحمة الله وبركاته، <br/>"
                                    + "تهديكم مؤسسة الملك خالد أطيب التحيات، <br/>"
                                    + $"ونرجو منكم التكرم بزيارة البوابة الإلكترونية للاطلاع على التعديلات المطلوبة لمخرجات ورشة: {Entity.FollowUpProjectPlan?.IncubationWorkshop?.Name}. <br/>"
                                    + "شاكرين ومقدرين تفاعلكم، <br/>"
                                    + " برنامج بناء القدرات. <br/>";
                }

                var fundingSource = repository.GetByKey<FundingSource>(Entity.FollowUpProjectPlan.IncubationWorkshop.FundingSourceID);
                if (fundingSource.UseCustomThemes)
                    mailHelper.Send($"?partner={fundingSource.Nickname}");
                else
                    mailHelper.Send("");
            }

            return RedirectToAction("Index");
        }

        public FileResult DownloadFollowUpProjectPlanAttachments(Guid id)
        {
            var attachmet = repository.GetByKey<FollowUpProjectPlanAttachment>(id);
            byte[] fileBytes = System.IO.File.ReadAllBytes(attachmet.URL);
            string fileName = attachmet.Name;
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        public FileResult DownloadFollowUpProjectPlanRequestAttachments(Guid id)
        {
            var attachmet = repository.GetByKey<FollowUpProjectPlanRequestAttachment>(id);
            byte[] fileBytes = System.IO.File.ReadAllBytes(attachmet.URL);
            string fileName = attachmet.Name;
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
        #endregion
    }
}