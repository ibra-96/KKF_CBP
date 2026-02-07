using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AlphaPeople.Core;
using System.Transactions;
using System.Configuration;
using AlphaPeople.Repository;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using Microsoft.AspNet.Identity.Owin;
using AlfaPeople.KingKhalidFoundation.Data;
using AlfaPeople.KingKhalidFoundation.Web.Helper;
using AlfaPeople.KingKhalidFoundation.Data.Model;
using AlfaPeople.KingKhalidFoundation.Web.Models;
using System.Data.Entity;
using System.Text;
using System.Net.Mail;
using Newtonsoft.Json;
using Microsoft.Ajax.Utilities;

namespace AlfaPeople.KingKhalidFoundation.Web.Controllers
{
    [Authorize(Roles = "Corporation")]
    public class HomeController : BaseController
    {
        private readonly CommonHelper helper;
        private readonly IRepository repository;
        private ApplicationUserManager _userManager;
        private readonly DateTime dateNow = DateTime.Now.Date;
        private readonly string Logpath = ConfigurationManager.AppSettings["Log"].ToString();

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public HomeController()
        {
            helper = new CommonHelper();
            repository = new Repository(new KingkhaledFoundationDB());
        }

        public HomeController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
        }

        public ActionResult CorporationProfile(string Msg)
        {
            LoadDropDownListCorporationRegistrationForm();
            ViewBag.Msg = Msg;
            ViewBag.lang = CultureHelper.CurrentCulture;
            CorporationRegistrationFormVM model = new CorporationRegistrationFormVM();
            var userId = User.Identity.GetUserId();

            var frontEndId = Guid.Parse(userId);

            AspNetUser CurrentUser = repository.FindOne<AspNetUser>(f => f.Id == userId);

            model._CorporateApplicationForm = new CorporateApplicationForm();
            model._CorporateApplicationForm = repository.FindOne<CorporateApplicationForm>(f => f.FrontendUserID == frontEndId);
            model._RegisterVM = new RegisterVM();
            model._RegisterVM.Username = CurrentUser.UserName;
            model._RegisterVM._RegisterViewModel = new RegisterViewModel();
            model._RegisterVM._RegisterViewModel.Email = CurrentUser.Email;
            model._RegisterVM._RegisterViewModel.Password = CurrentUser.PasswordHash;
            ViewBag.Title = CultureHelper.CurrentCulture != 3 ? model._RegisterVM.Username + " - " + App_GlobalResources.General.Profile : model._CorporateApplicationForm.Name + " - " + App_GlobalResources.General.Profile;
            ViewBag.name = model._RegisterVM.Username;
            ViewBag.role = CurrentUser.AspNetRoles.Select(f => f.Id).FirstOrDefault();
            var imgSrc = (model._CorporateApplicationForm.Picture != null && model._CorporateApplicationForm.Picture.Length > 0)
    ? $"data:image/gif;base64,{Convert.ToBase64String(model._CorporateApplicationForm.Picture)}"
    : Url.Content("~/assets/img/default-avatar.png"); // الصورة الافتراضية

            ViewBag.imgSrc = imgSrc;

            //var img = Convert.ToBase64String(model._CorporateApplicationForm.Picture);
            //ViewBag.imgSrc = String.Format("data:image/gif;base64,{0}", img);
            return View(model);
        }

        public void LoadDropDownListCorporationRegistrationForm()
        {
            ViewBag.RegionID = new SelectList(repository.Get<Region>(f => f.IsActive == true), "RegionID", CultureHelper.CurrentCulture != 3 ? "RegionNameEN" : "RegionNameAR");
            ViewBag.ProgramID = new SelectList(repository.Get<Program>(f => f.IsActive == true), "ProgramID", "ProgramName");
            ViewBag.CorporationsCategoryID = new SelectList(repository.GetAll<CorporationsCategory>(), "CorporationsCategoryID", CultureHelper.CurrentCulture != 3 ? "CorporationsCategoryNameEN" : "CorporationsCategoryNameAR");
            ViewBag.AuthorizationAuthorityID = new SelectList(repository.Get<AuthorizationAuthority>(f => f.IsActive == true), "AuthorizationAuthorityID", CultureHelper.CurrentCulture != 3 ? "AuthorizationAuthorityNameEN" : "AuthorizationAuthorityNameAR");
        }


        public ActionResult dashboard()
        {


            //var userid = User.Identity.GetUserId();
            //var currentuser = repository.GetByKey<AspNetUser>(userid);
            //Guid frontEnd = Guid.Parse(userid);
            //ViewBag.lang = CultureHelper.CurrentCulture;
            //if (currentuser.AspNetRoles.Any(r => r.Name == "Corporation"))
            //{
            //    var img = Convert.ToBase64String(repository.GetQuery<CorporateApplicationForm>(f => f.FrontendUserID == frontEnd).FirstOrDefault().Picture);
            //    ViewBag.imgSrc = String.Format("data:image/gif;base64,{0}", img);
            //    if (repository.GetQuery<FrontendUser>(f => f.FK_AspUser == userid && f.CorporateApplicationForms.Any(g => g.CorporateApplicationStatu.ApplicantStatu.ApplicantStatusName == "Accepted")).ToList().Count != 0)
            //    {
            //        DashboardVM _dashboard = new Models.DashboardVM();
            //        string ProgramName = currentuser.FrontendUsers.FirstOrDefault().CorporateApplicationForms.FirstOrDefault().Program.ProgramName;
            //        ViewBag.ProgramName = ProgramName;

            //        _dashboard._inc = new List<IncubationAdvertising>();

            //        _dashboard._inc = repository.GetQuery<IncubationAdvertising>(f => (f.IsActive == true && (f.EndDate >= dateNow) && f.IncubationType.NameEN == "Incubation" && f.ISPublic == true) ||
            //        (f.IsActive == true && (f.EndDate >= dateNow) && f.IncubationType.NameEN == "Incubation" && (f.ISPublic == false && f.IncubationPrivateInvitations.Any(r => r.Email == currentuser.Email && r.IncubationAdvertising.IncubationType.NameEN == "Incubation")))).ToList();
            //        var joinedinc = repository.GetQuery<IncubationAdvertising>(f => f.IsActive == true && (f.EndDate >= dateNow) && f.IncubationType.NameEN == "Incubation" && f.IncubationProjectProposals.Any(r => r.FrontendUserID == frontEnd)).ToList();
            //        foreach (var item in joinedinc)
            //        {
            //            _dashboard._inc.Remove(item);
            //        }

            //        _dashboard._incAcc = new List<IncubationAdvertising>();
            //        _dashboard._incAcc = repository.GetQuery<IncubationAdvertising>(f => f.IsActive == true && (f.EndDate >= dateNow) && f.IncubationType.NameEN == "Acceleration" && f.ISPublic == true ||
            //        (f.IsActive == true && (f.EndDate >= dateNow) && f.IncubationType.NameEN == "Acceleration" && (f.ISPublic == false && f.IncubationPrivateInvitations.Any(r => r.Email == currentuser.Email && r.IncubationAdvertising.IncubationType.NameEN == "Acceleration")))).ToList();
            //        var joinedACC = repository.GetQuery<IncubationAdvertising>(f => f.IsActive == true && (f.EndDate >= dateNow) && f.IncubationType.NameEN == "Acceleration" && f.IncubationProjectProposals.Any(r => r.FrontendUserID == frontEnd)).ToList();
            //        foreach (var item in joinedACC)
            //        {
            //            _dashboard._incAcc.Remove(item);
            //        }

            //        _dashboard.LstUpdateIncubationProjectProposal = new List<IncubationProjectProposal>();
            //        var incubationProjectProposal = repository.GetQuery<IncubationProjectProposal>(f => f.FrontendUserID == frontEnd && (f.IncubationProjectProposalStatu.NameEN == "Update Project Proposal" || f.IncubationProjectProposalStatu.NameEN == "Draft")).ToList();
            //        for (int i = 0; i < incubationProjectProposal.Count; i++)
            //        {
            //            _dashboard.LstUpdateIncubationProjectProposal.Add(incubationProjectProposal[i]);
            //        }

            //        _dashboard._workShop = new List<IncubationWorkshop>();
            //        _dashboard._workShop = repository.GetQuery<IncubationWorkshop>(f => f.IncubationtWorkshopStatu.NameEN == "Active" && f.LastTimeToApply >= dateNow && (f.ISPublic == true || (f.ISPublic == false && f.WorkshopPrivateInvitations.Any(r => r.Email == currentuser.Email)))).ToList();
            //        var joinedWork = repository.GetQuery<IncubationWorkshop>(f => f.IncubationtWorkshopStatu.NameEN == "Active" && f.WorkshopProjectProposals.Any(r => r.FrontendUserID == frontEnd)).ToList();
            //        foreach (var item in joinedWork)
            //        {
            //            _dashboard._workShop.Remove(item);
            //        }

            //        _dashboard.LstUpdateWorkshopProjectProposal = new List<WorkshopProjectProposal>();
            //        var _lstUpdateWorkshopProjectProposal = repository.GetQuery<WorkshopProjectProposal>(f => f.FrontendUserID == frontEnd && f.WorkshopProjectProposalStatu.NameEN == "Update Project Proposal").ToList();
            //        for (int i = 0; i < _lstUpdateWorkshopProjectProposal.Count; i++)
            //        {
            //            _dashboard.LstUpdateWorkshopProjectProposal.Add(_lstUpdateWorkshopProjectProposal[i]);
            //        }

            //        _dashboard._incubationWorkshopRating = new List<IncubationWorkshop>();
            //        var IncubationWorkshopRating = repository.GetQuery<IncubationWorkshop>(f => f.WorkshopPrivateInvitations.Any(p => p.FrontendUserID == frontEnd && p.InvitationStatus == InvitationStatus.attend) && !f.IncubationWorkshopRatings.Any(r => r.FrontendUserId == frontEnd)).ToList();
            //        for (int i = 0; i < IncubationWorkshopRating.Count; i++)
            //        {
            //            _dashboard._incubationWorkshopRating.Add(IncubationWorkshopRating[i]);
            //        }

            //        _dashboard._followUpProjectPlan = new List<FollowUpProjectPlanRequest>();
            //        var followUpProjectPlan = repository.GetQuery<FollowUpProjectPlanRequest>(f => (f.FrontendUserId == frontEnd && f.FollowUpProjectPlanStatus == FollowUpProjectPlanStatus.Pending) || (f.FrontendUserId == frontEnd && f.FollowUpProjectPlanStatus == FollowUpProjectPlanStatus.UpdateProjectPlan)).ToList();
            //        for (int i = 0; i < followUpProjectPlan.Count; i++)
            //        {
            //            _dashboard._followUpProjectPlan.Add(followUpProjectPlan[i]);
            //        }

            //        ViewBag.Title = App_GlobalResources.General.Dashboard;
            //        return View(_dashboard);
            //    }
            //    return RedirectToAction("CorporationProfile", new { Msg = App_GlobalResources.General.MsgApply });
            //}
            //else
            //{
            //    return View();
            //}
            var userid = User.Identity.GetUserId();
            var currentuser = repository.GetByKey<AspNetUser>(userid);
            Guid frontEnd = Guid.Parse(userid);
            ViewBag.lang = CultureHelper.CurrentCulture;
            ViewBag.Msg = TempData["Msg1"];
            if (currentuser.AspNetRoles.Any(r => r.Name == "Corporation"))
            {
                var img = Convert.ToBase64String(repository.GetQuery<CorporateApplicationForm>(f => f.FrontendUserID == frontEnd).FirstOrDefault().Picture);
                ViewBag.imgSrc = String.Format("data:image/gif;base64,{0}", img);
                if (repository.GetQuery<FrontendUser>(f => f.FK_AspUser == userid && f.CorporateApplicationForms.Any(g => g.CorporateApplicationStatu.ApplicantStatu.ApplicantStatusName == "Accepted")).ToList().Count != 0)
                {
                    DashboardVM _dashboard = new Models.DashboardVM();
                    string ProgramName = currentuser.FrontendUsers.FirstOrDefault().CorporateApplicationForms.FirstOrDefault().Program.ProgramName;
                    ViewBag.ProgramName = ProgramName;

                    _dashboard._inc = new List<IncubationAdvertising>();
                    //20-2-2025
                    //&& r.Status != InvitationStatus.cancel
                    //_dashboard._inc = repository.GetQuery<IncubationAdvertising>(f => (f.IsActive == true && (f.EndDate >= dateNow) && f.IncubationType.NameEN == "Incubation" && f.ISPublic == true) ||
                    //(f.IsActive == true && (f.EndDate >= dateNow) && f.IncubationType.NameEN == "Incubation" && (f.ISPublic == false && f.IncubationPrivateInvitations.Any(r => r.Email == currentuser.Email && r.IncubationAdvertising.IncubationType.NameEN == "Incubation" && r.Status != InvitationStatus.cancel)))).ToList();
                    //24-2-2025
                    //_dashboard._inc = repository.GetQuery<IncubationAdvertising>(f =>
                    //    (f.IsActive == true && !f.IsDeleted && (f.EndDate >= dateNow) && f.IncubationType.NameEN == "Incubation" && f.ISPublic == true) ||
                    //    (f.IsActive == true && !f.IsDeleted && (f.EndDate >= dateNow) && f.IncubationType.NameEN == "Incubation" &&
                    //        (f.ISPublic == false &&
                    //            f.IncubationPrivateInvitations
                    //                .Where(r => r.Email == currentuser.Email && r.IncubationAdvertising.IncubationType.NameEN == "Incubation")
                    //                .OrderByDescending(r => r.CreatedDate) // ترتيب الدعوات الأحدث أولاً
                    //                .FirstOrDefault().Status != InvitationStatus.cancel
                    //        )
                    //    )
                    //).ToList();
                    _dashboard._inc = repository.GetQuery<IncubationAdvertising>(f =>
                (f.IsActive == true && !f.IsDeleted && (f.EndDate >= dateNow) && f.IncubationType.NameEN == "Incubation" && f.ISPublic == true) ||
                (f.IsActive == true && !f.IsDeleted && (f.EndDate >= dateNow) && f.IncubationType.NameEN == "Incubation" &&
                    (f.ISPublic == false &&
                        f.IncubationPrivateInvitations.Any(r => r.Email == currentuser.Email
                                                                && r.IncubationAdvertising.IncubationType.NameEN == "Incubation"
                                                                && r.Status != InvitationStatus.cancel) // التأكد من أن الدعوة ليست ملغاة
                    )
                )
            ).ToList();
                    var joinedinc = repository.GetQuery<IncubationAdvertising>(f => f.IsActive == true && !f.IsDeleted && (f.EndDate >= dateNow) && f.IncubationType.NameEN == "Incubation" && f.IncubationProjectProposals.Any(r => r.FrontendUserID == frontEnd)).ToList();
                    foreach (var item in joinedinc)
                    {
                        _dashboard._inc.Remove(item);
                    }

                    _dashboard._incAcc = new List<IncubationAdvertising>();
                    //24-2-2025
                    //_dashboard._incAcc = repository.GetQuery<IncubationAdvertising>(f =>
                    //    (f.IsActive == true && !f.IsDeleted && (f.EndDate >= dateNow) && f.IncubationType.NameEN == "Acceleration" && f.ISPublic == true) ||
                    //    (f.IsActive == true && !f.IsDeleted && (f.EndDate >= dateNow) && f.IncubationType.NameEN == "Acceleration" &&
                    //        (f.ISPublic == false &&
                    //            f.IncubationPrivateInvitations
                    //                .Where(r => r.Email == currentuser.Email && r.IncubationAdvertising.IncubationType.NameEN == "Acceleration")
                    //                .OrderByDescending(r => r.CreatedDate) // أخذ أحدث دعوة فقط
                    //                .FirstOrDefault().Status != InvitationStatus.cancel // التحقق من الحالة
                    //        )
                    //    )
                    //).ToList();
                     _dashboard._incAcc = repository.GetQuery<IncubationAdvertising>(f =>
                    (f.IsActive == true && !f.IsDeleted && (f.EndDate >= dateNow) && f.IncubationType.NameEN == "Acceleration" && f.ISPublic == true) ||
                    (f.IsActive == true && !f.IsDeleted && (f.EndDate >= dateNow) && f.IncubationType.NameEN == "Acceleration" &&
                        (f.ISPublic == false &&
                            f.IncubationPrivateInvitations.Any(r => r.Email == currentuser.Email
                                                                    && r.IncubationAdvertising.IncubationType.NameEN == "Acceleration"
                                                                    && r.Status != InvitationStatus.cancel) // التأكد من أن الدعوة ليست ملغاة
                        )
                    )
                ).ToList();

                    //_dashboard._incAcc = repository.GetQuery<IncubationAdvertising>(f => f.IsActive == true && (f.EndDate >= dateNow) && f.IncubationType.NameEN == "Acceleration" && f.ISPublic == true ||
                    //(f.IsActive == true && (f.EndDate >= dateNow) && f.IncubationType.NameEN == "Acceleration" && (f.ISPublic == false && f.IncubationPrivateInvitations.Any(r => r.Email == currentuser.Email && r.IncubationAdvertising.IncubationType.NameEN == "Acceleration" && r.Status != InvitationStatus.cancel)))).ToList();
                    var joinedACC = repository.GetQuery<IncubationAdvertising>(f => f.IsActive == true && !f.IsDeleted && (f.EndDate >= dateNow) && f.IncubationType.NameEN == "Acceleration" && f.IncubationProjectProposals.Any(r => r.FrontendUserID == frontEnd)).ToList();
                    foreach (var item in joinedACC)
                    {
                        _dashboard._incAcc.Remove(item);
                    }

                    _dashboard.LstUpdateIncubationProjectProposal = new List<IncubationProjectProposal>();
                    var incubationProjectProposal = repository.GetQuery<IncubationProjectProposal>(f => f.FrontendUserID == frontEnd && (f.IncubationProjectProposalStatu.NameEN == "Update Project Proposal" || f.IncubationProjectProposalStatu.NameEN == "Draft")).ToList();
                    for (int i = 0; i < incubationProjectProposal.Count; i++)
                    {
                        _dashboard.LstUpdateIncubationProjectProposal.Add(incubationProjectProposal[i]);
                    }



                    Guid blRejectedStatusID = Guid.Parse("ff908cf9-4b54-4534-9550-3c6e58e6fc35");//النموذج الاساسي مقبول 
                    Guid blAcceptedStatusID = Guid.Parse("b0990311-3fee-47ac-9624-50260ea39d5f");//النموذج الاساسي مقبول 
                    Guid blPendingStatusID = Guid.Parse("2148FD0B-5B71-4AC0-BCEE-5C1B4E8A6532");  // Pending - النماذج
                    Guid blUpdateStatusID = Guid.Parse("466D2346-7711-4752-B131-A5CCEC7A2B8C");   // Update Baseline Application Form

                    Guid blUpdatedStatusID = Guid.Parse("aa35dba2-56c7-4953-a876-e1b9a3dc2dee");  // Update Project Proposal

                    Guid proposalPendingStatusID = Guid.Parse("a601fba0-27c6-4b54-a96a-0cbd7eaab910");  // Pending - المقترحات
                    Guid proposalUpdateStatusID = Guid.Parse("049ae3b7-0000-423f-8a19-c642b2f20126");  // Update Project Proposal



                    // 24-2-2025
                    //_dashboard._workShop = repository.GetQuery<IncubationWorkshop>(f =>
                    //    f.IncubationtWorkshopStatu.NameEN == "Active" && !f.IsDeleted &&
                    //    f.LastTimeToApply >= dateNow &&
                    //    (
                    //        f.ISPublic == true ||
                    //        (
                    //            f.ISPublic == false &&
                    //            f.WorkshopPrivateInvitations
                    //                .Where(r => r.Email == currentuser.Email)
                    //                .OrderByDescending(r => r.CreatedDate) // جلب أحدث دعوة
                    //                .FirstOrDefault().InvitationStatus != InvitationStatus.cancel // استبعاد الدعوات الملغاة
                    //        )
                    //    )
                    //).ToList();
                    //    _dashboard._workShop = repository.GetQuery<IncubationWorkshop>(f =>
                    //    f.IncubationtWorkshopStatu.NameEN == "Active" && !f.IsDeleted &&
                    //    f.LastTimeToApply >= dateNow &&
                    //    (
                    //        f.ISPublic || // ورش العمل العامة تظهر للجميع
                    //        (
                    //            !f.ISPublic &&
                    //            f.WorkshopPrivateInvitations.Any(r => r.Email == currentuser.Email && r.InvitationStatus != InvitationStatus.cancel)
                    //        )
                    //    )
                    //).ToList();


                    _dashboard._workShop = repository.GetQuery<IncubationWorkshop>(f =>
                        f.IncubationtWorkshopStatu.NameEN == "Active" &&
                        !f.IsDeleted &&
                        f.LastTimeToApply >= dateNow &&
                        (
                            f.ISPublic ||
                            (!f.ISPublic && f.WorkshopPrivateInvitations.Any(r => r.Email == currentuser.Email && r.InvitationStatus != InvitationStatus.cancel))
                        )
                    ).ToList();

                    //_dashboard._workShop = repository.GetQuery<IncubationWorkshop>(f => f.IncubationtWorkshopStatu.NameEN == "Active" && f.LastTimeToApply >= dateNow && (f.ISPublic == true || (f.ISPublic == false && f.WorkshopPrivateInvitations.Any(r => r.Email == currentuser.Email)))).ToList();
                    _dashboard.WorkshopStatuses = new Dictionary<Guid, string>();


                    foreach (var workshop in _dashboard._workShop)
                    {
                        string finalStatus = null;  // بدون قيمة افتراضية

                        var hasAcceptedBLTransaction = repository.GetQuery<IncubationWorkshopBLTransactionsValue>()
                            .Where(j => j.IncubationWorkshopBLTrans.IncubationWorkshopID == workshop.IncubationWorkshopID &&
                                        j.IncubationWorkshopBLTransValStatusID == blAcceptedStatusID &&
                                        j.FrontendUserID == frontEnd)
                            .Any();

                        var hasProposal = repository.GetQuery<WorkshopProjectProposal>()
                            .Where(p => p.IncubationWorkshopID == workshop.IncubationWorkshopID &&
                                        p.FrontendUserID == frontEnd)
                            .Any();

                        if (hasAcceptedBLTransaction && !hasProposal)
                        {
                            finalStatus = "Accepted";
                        }
                        else if (repository.GetQuery<IncubationWorkshopBLTransactionsValue>().Any(j =>
                            j.IncubationWorkshopBLTrans.IncubationWorkshopID == workshop.IncubationWorkshopID &&
                            j.IncubationWorkshopBLTransValStatusID == blRejectedStatusID &&
                            j.FrontendUserID == frontEnd))
                        {
                            finalStatus = "Rejected";
                        }
                        else if (repository.GetQuery<IncubationWorkshopBLTransactionsValue>().Any(j =>
                            j.IncubationWorkshopBLTrans.IncubationWorkshopID == workshop.IncubationWorkshopID &&
                            j.IncubationWorkshopBLTransValStatusID == blUpdatedStatusID &&
                            j.FrontendUserID == frontEnd))
                        {
                            finalStatus = "Updated";
                        }
                        else if (repository.GetQuery<IncubationWorkshopBLTransactionsValue>().Any(j =>
                            j.IncubationWorkshopBLTrans.IncubationWorkshopID == workshop.IncubationWorkshopID &&
                            j.IncubationWorkshopBLTransValStatusID == blPendingStatusID &&
                            j.FrontendUserID == frontEnd))
                        {
                            finalStatus = "Pending";
                        }
                        else if (repository.GetQuery<IncubationWorkshopBLTransactionsValue>().Any(j =>
                            j.IncubationWorkshopBLTrans.IncubationWorkshopID == workshop.IncubationWorkshopID &&
                            j.IncubationWorkshopBLTransValStatusID == blUpdateStatusID &&
                            j.FrontendUserID == frontEnd))
                        {
                            finalStatus = "Update";
                        }
                        else
                        {
                            // التحقق من عدم وجود أي سجل في الجدول لهذه الورشة
                            var noTransactions = !repository.GetQuery<IncubationWorkshopBLTransactionsValue>()
                                .Where(j => j.IncubationWorkshopBLTrans.IncubationWorkshopID == workshop.IncubationWorkshopID &&
                                            j.FrontendUserID == frontEnd)
                                .Any();

                            if (noTransactions)
                            {
                                finalStatus = "Join";
                            }
                        }

                        // إضافة الحالة النهائية للورشة
                        if (!string.IsNullOrEmpty(finalStatus))
                        {
                            _dashboard.WorkshopStatuses[workshop.IncubationWorkshopID] = finalStatus;
                        }
                    }

                    _dashboard.LstUpdateWorkshopProjectProposal = new List<WorkshopProjectProposal>();
                    var _lstUpdateWorkshopProjectProposal = repository.GetQuery<WorkshopProjectProposal>(f => f.FrontendUserID == frontEnd && f.WorkshopProjectProposalStatu.NameEN == "Update Project Proposal").ToList();
                    for (int i = 0; i < _lstUpdateWorkshopProjectProposal.Count; i++)
                    {
                        _dashboard.LstUpdateWorkshopProjectProposal.Add(_lstUpdateWorkshopProjectProposal[i]);
                    }
                    _dashboard._incubationWorkshopRating = repository.GetQuery<IncubationWorkshop>(f =>
                        f.WorkshopPrivateInvitations.Any(p => p.FrontendUserID == frontEnd && p.InvitationStatus == InvitationStatus.attend) &&
                        !f.IncubationWorkshopRatings.Any(r => r.FrontendUserId == frontEnd) &&
                        f.WorkshopPrivateInvitations
                            .Where(r => r.Email == currentuser.Email)
                            .OrderByDescending(r => r.CreatedDate) // ترتيب الأحدث أولاً
                            .FirstOrDefault().InvitationStatus != InvitationStatus.cancel // استبعاد الدعوات الملغاة
                    ).ToList();

                    //_dashboard._incubationWorkshopRating = new List<IncubationWorkshop>();
                    //var IncubationWorkshopRating = repository.GetQuery<IncubationWorkshop>(f => f.WorkshopPrivateInvitations.Any(p => p.FrontendUserID == frontEnd && p.InvitationStatus == InvitationStatus.attend) && !f.IncubationWorkshopRatings.Any(r => r.FrontendUserId == frontEnd)).ToList();
                    //for (int i = 0; i < IncubationWorkshopRating.Count; i++)
                    //{
                    //    _dashboard._incubationWorkshopRating.Add(IncubationWorkshopRating[i]);
                    //}

                    _dashboard._followUpProjectPlan = new List<FollowUpProjectPlanRequest>();
                    var followUpProjectPlan = repository.GetQuery<FollowUpProjectPlanRequest>(f => (f.FrontendUserId == frontEnd && f.FollowUpProjectPlanStatus == FollowUpProjectPlanStatus.Pending) || (f.FrontendUserId == frontEnd && f.FollowUpProjectPlanStatus == FollowUpProjectPlanStatus.UpdateProjectPlan)).ToList();
                    for (int i = 0; i < followUpProjectPlan.Count; i++)
                    {
                        _dashboard._followUpProjectPlan.Add(followUpProjectPlan[i]);
                    }

                    ViewBag.Title = App_GlobalResources.General.Dashboard;
                    if (ModelState.IsValid)
                    {

                        return View(_dashboard);
                    }

                    else
                    {
                        // Log ModelState errors for debugging
                        foreach (var modelStateKey in ModelState.Keys)
                        {
                            var value = ModelState[modelStateKey];
                            foreach (var error in value.Errors)
                            {
                                Console.WriteLine($"Error in {modelStateKey}: {error.ErrorMessage}");
                            }
                        }

                        // Return the same view with the current model to display errors
                        return View(_dashboard);

                    }
                }
                return RedirectToAction("CorporationProfile", new { Msg = App_GlobalResources.General.MsgApply });
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult IncubationProjectProposal(Guid id)
        {
            var frontEnd = Guid.Parse(User.Identity.GetUserId());
            var CorporateProfile = repository.GetQuery<CorporateApplicationForm>(f => f.FrontendUserID == frontEnd).FirstOrDefault();
            var img = Convert.ToBase64String(CorporateProfile.Picture);
            ViewBag.imgSrc = string.Format("data:image/gif;base64,{0}", img);

            var IncAD = repository.GetByKey<IncubationAdvertising>(id);
            var IncBL = repository.GetQuery<IncubationBaseline>(i => i.FrontendUserID == frontEnd).FirstOrDefault();
            var IncPP = repository.GetByKey<IncubationProjectProposal>(id);

            if (IncAD != null)
            {
                if (IncBL == null)
                {
                    IncBL = new IncubationBaseline();
                    ViewBag.IncubBLStatus = "";
                    ViewBag.dateBoardApproval = "";
                }
                else
                {
                    ViewBag.dateBoardApproval = IncBL.DateBoardApproval?.ToString("yyyy-MM-dd");
                    ViewBag.IncubBLStatus = IncBL.IncubationBaselineStatus.NameEN;
                }


                return View(new IncubationProjectProposalVM()
                {
                    IncubationAd = IncAD,
                    IncubationBL = IncBL,
                    incubationPP = new IncubationProjectProposal()
                });
            }
            else if (IncPP != null)
            {
                if (IncBL == null)
                {
                    IncAD = new IncubationAdvertising();
                    IncBL = new IncubationBaseline();
                    ViewBag.IncubBLStatus = "";
                    ViewBag.dateBoardApproval = "";
                }
                else
                {
                    ViewBag.dateBoardApproval = IncBL.DateBoardApproval?.ToString("yyyy-MM-dd");
                    ViewBag.IncubBLStatus = IncBL.IncubationBaselineStatus.NameEN;
                    ViewBag.Msg = string.IsNullOrWhiteSpace(IncBL.Feadback) ? IncPP.Feadback : IncBL.Feadback;
                    IncAD = repository.GetByKey<IncubationAdvertising>(IncPP.IncubationAdID);
                }

                return View(new IncubationProjectProposalVM() { IncubationAd = IncAD, IncubationBL = IncBL, incubationPP = IncPP });
            }
            return RedirectToAction("dashboard", new { Msg = "Something Went Wrong." });
        }

        [HttpPost]
        public ActionResult IncubationProjectProposal(IncubationProjectProposalVM model)
        {
            try
            {
                var frontEnd = Guid.Parse(User.Identity.GetUserId());
                //24-2-2025

                //  البحث عن الدعوة المرتبطة بهذا المستخدم وبنفس الإعلان
                var invitation = repository.GetQuery<IncubationPrivateInvitation>()
                                           .FirstOrDefault(i => i.FrontendUserID == frontEnd && i.IncubationAdID == model.IncubationAd.IncubationAdID);

                if (invitation != null && invitation.Status == InvitationStatus.pending)
                {
                    //  تحديث حالة الدعوة إلى "attend"
                    invitation.Status = InvitationStatus.attend;
                    repository.Update(invitation);
                }
                string FolderName = User.Identity.Name;
                string path = Server.MapPath("~/Uploads/" + FolderName + "/");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                if (model.StrategicPlan != null)
                {
                    model.StrategicPlan.SaveAs(path + Path.GetFileName(model.StrategicPlan.FileName));
                    model.IncubationBL.StrategicPlan = path + Path.GetFileName(model.StrategicPlan.FileName);
                }
                if (model.OperatingPlan != null)
                {
                    model.OperatingPlan.SaveAs(path + Path.GetFileName(model.OperatingPlan.FileName));
                    model.IncubationBL.OperatingPlan = path + Path.GetFileName(model.OperatingPlan.FileName);
                }
                if (model.GovernanceGuide != null)
                {
                    model.GovernanceGuide.SaveAs(path + Path.GetFileName(model.GovernanceGuide.FileName));
                    model.IncubationBL.GovernanceGuide = path + Path.GetFileName(model.GovernanceGuide.FileName);
                }
                if (model.CharteredAccountantNotes != null)
                {
                    model.CharteredAccountantNotes.SaveAs(path + Path.GetFileName(model.CharteredAccountantNotes.FileName));
                    model.IncubationBL.CharteredAccountantNotes = path + Path.GetFileName(model.CharteredAccountantNotes.FileName);
                }
                if (model.AttachFollowUpandEvaluationForms != null)
                {
                    model.AttachFollowUpandEvaluationForms.SaveAs(path + Path.GetFileName(model.AttachFollowUpandEvaluationForms.FileName));
                    model.IncubationBL.AttachFollowUpandEvaluationForms = path + Path.GetFileName(model.AttachFollowUpandEvaluationForms.FileName);
                }

                var IncIBL = repository.GetQuery<IncubationBaseline>(i => i.FrontendUserID == frontEnd).FirstOrDefault();
                var IncPP = repository.GetQuery<IncubationProjectProposal>(i => i.IncubationAdID == model.IncubationAd.IncubationAdID && i.FrontendUserID == frontEnd).FirstOrDefault();
                if (IncIBL != null)
                {
                    model.IncubationBL = (model.IncubationBL.ReasonEstablishingCorporate == null &&
                                          model.IncubationBL.BoardDirectorsCount == null &&
                                          model.IncubationBL.CorporateRevenuesLastYear == null &&
                                          model.IncubationBL.BudgetAllocatedProjects == null &&
                                          model.IncubationBL.BudgetAllocatedCorporateCommunication == null &&
                                          model.IncubationBL.FullTimeStaff == null) ? IncIBL : model.IncubationBL;
                    if (IncPP != null)
                    {
                        if (!model.DraftFlage)
                        {
                            IncPP.IncubationProjectProposalStatusID = repository.Get<IncubationProjectProposalStatu>(s => s.NameEN == "Project Proposal Updated").FirstOrDefault().IncubationProjectProposalStatusID;
                            IncPP.submissionDate = DateTime.Now;

                            model.IncubationBL.IncubationBaselineStatusID = repository.Get<IncubationBaselineStatus>(s => s.NameEN == "Baseline Application Form Updated").FirstOrDefault().IncubationBaselineStatusID;
                            model.IncubationBL.submissionDate = DateTime.Now;

                            repository.Update(IncPP);
                            repository.Update(model.IncubationBL);
                        }
                        else
                        {
                            model.IncubationBL.IncubationBaselineStatusID = repository.Get<IncubationBaselineStatus>(s => s.NameEN == "Draft").FirstOrDefault().IncubationBaselineStatusID;
                            model.IncubationBL.submissionDate = DateTime.Now;
                            repository.Update(model.IncubationBL);
                        }
                    }
                    else
                    {
                        IncPP = new IncubationProjectProposal();
                        if (!model.DraftFlage)
                        {
                            model.IncubationBL.IncubationBaselineStatusID = repository.Get<IncubationBaselineStatus>(s => s.NameEN == "Pending").FirstOrDefault().IncubationBaselineStatusID;

                            IncPP.IncubationProjectProposalID = Guid.NewGuid();
                            IncPP.IncubationAdID = model.IncubationAd.IncubationAdID;
                            IncPP.FrontendUserID = frontEnd;
                            IncPP.IncubationProjectProposalStatusID = repository.Get<IncubationProjectProposalStatu>(s => s.NameEN == "Pending").FirstOrDefault().IncubationProjectProposalStatusID;
                            IncPP.submissionDate = DateTime.Now;
                            IncPP.Feadback = null;

                            repository.Update(model.IncubationBL);
                            repository.Add(IncPP);
                        }
                        else
                        {
                            model.IncubationBL.IncubationBaselineStatusID = repository.Get<IncubationBaselineStatus>(s => s.NameEN == "Draft").FirstOrDefault().IncubationBaselineStatusID;
                            repository.Update(model.IncubationBL);
                        }
                    }
                }
                else
                {
                    if (!model.DraftFlage)
                    {
                        IncPP = new IncubationProjectProposal();
                        model.IncubationBL.FrontendUserID = frontEnd;
                        model.IncubationBL.IncubationBaselineID = Guid.NewGuid();
                        model.IncubationBL.IncubationBaselineStatusID = repository.Get<IncubationBaselineStatus>(s => s.NameEN == "Pending").FirstOrDefault().IncubationBaselineStatusID;
                        model.IncubationBL.submissionDate = DateTime.Now;
                        model.IncubationBL.Feadback = null;

                        IncPP.IncubationProjectProposalID = Guid.NewGuid();
                        IncPP.IncubationAdID = model.IncubationAd.IncubationAdID;
                        IncPP.FrontendUserID = frontEnd;
                        IncPP.IncubationProjectProposalStatusID = repository.Get<IncubationProjectProposalStatu>(s => s.NameEN == "Pending").FirstOrDefault().IncubationProjectProposalStatusID;
                        IncPP.submissionDate = DateTime.Now;
                        IncPP.Feadback = null;

                        repository.Add(model.IncubationBL);
                        repository.Add(IncPP);
                    }
                    else
                    {
                        model.IncubationBL.IncubationBaselineID = Guid.NewGuid();
                        model.IncubationBL.FrontendUserID = frontEnd;
                        model.IncubationBL.IncubationBaselineStatusID = repository.Get<IncubationBaselineStatus>(s => s.NameEN == "Draft").FirstOrDefault().IncubationBaselineStatusID;
                        model.IncubationBL.submissionDate = DateTime.Now;
                        model.IncubationBL.Feadback = null;
                        repository.Add(model.IncubationBL);
                    }
                }
                repository.UnitOfWork.SaveChanges();

                try
                {
                    var IncubationType = repository.GetByKey<IncubationType>(model.IncubationAd.IncubationTypeID);
                    var IncubationBLStatus = repository.GetByKey<IncubationBaselineStatus>(model.IncubationBL.IncubationBaselineStatusID);
                    if ((IncubationBLStatus?.NameEN == "Pending" && IncPP?.IncubationProjectProposalStatu.NameEN == "Pending") || (IncubationBLStatus?.NameEN == "Baseline Application Form Updated" && IncPP?.IncubationProjectProposalStatu.NameEN == "Project Proposal Updated"))
                    {
                        MailHelper mailHelper = new MailHelper();
                        mailHelper.ToEmail = UserManager.GetEmail(User.Identity.GetUserId());
                        mailHelper.Subject = "إفادة";
                        if (IncubationType.NameEN == "Incubation")
                        {
                            mailHelper.Body = "المكرمين/ المنظمات غير الربحية                         الموقرين <br />"
                                                            + "السلام عليكم ورحمة الله وبركاته، <br />"
                                                            + "نتقدم لكم بالشكر على تعبئة نموذج الالتحاق بمشروع الاحتضان الكامل، <br />"
                                                            + "ونحيطكم علماً بأنه تم استلام طلبكم، <br />"
                                                            + "وسيتم إبلاغكم في حال قبول طلبكم، <br />"
                                                            + "مع التحية، <br />"
                                                            + " برنامج بناء القدرات.  <br />";
                        }
                        else
                        {
                            mailHelper.Body = "المكرمين/ المنظمات غير الربحية                         الموقرين <br />"
                                                            + "السلام عليكم ورحمة الله وبركاته، <br />"
                                                            + "نتقدم لكم بالشكر على تعبئة نموذج الالتحاق بمشروع الاحتضان الجزئي، <br />"
                                                            + "ونحيطكم علماً بأنه تم استلام طلبكم، <br />"
                                                            + "وسيتم إبلاغكم في حال قبول طلبكم، <br />"
                                                            + "مع التحية، <br />"
                                                            + " برنامج بناء القدرات.  <br />";
                        }

                        var fundingSource = repository.GetByKey<FundingSource>(model.IncubationAd.FundingSourceID);
                        if (fundingSource.UseCustomThemes)
                            mailHelper.Send($"?partner={fundingSource.Nickname}");
                        else
                            mailHelper.Send("");
                    }
                }
                catch (Exception)
                {
                    return RedirectToAction("dashboard");
                }

                return RedirectToAction("dashboard");
            }
            catch (Exception)
            {
                var frontEnd = Guid.Parse(User.Identity.GetUserId());
                var img = Convert.ToBase64String(repository.GetQuery<CorporateApplicationForm>(f => f.FrontendUserID == frontEnd).FirstOrDefault().Picture);
                ViewBag.imgSrc = string.Format("data:image/gif;base64,{0}", img);
                ViewBag.dateBoardApproval = model.IncubationBL.DateBoardApproval?.ToString("yyyy-MM-dd");
                ViewBag.IncubBLStatus = model.IncubationBL.IncubationBaselineStatus.NameEN;
                model.IncubationAd = repository.GetByKey<IncubationAdvertising>(model.IncubationAd.IncubationAdID);
                var IncPP = repository.GetQuery<IncubationProjectProposal>(i => i.IncubationAdID == model.IncubationAd.IncubationAdID && i.FrontendUserID == frontEnd).FirstOrDefault();
                if (IncPP != null)
                    model.incubationPP = IncPP;

                return View(model);
            }
        }
       
        //[HttpGet]
        //public ActionResult IncWSBaselineAppForm(Guid id)
        //{
        //    var frontEnd = Guid.Parse(User.Identity.GetUserId());
        //    ViewBag.frontEnd = frontEnd;
        //   // التحقق من ملف المنظمة
        //   var corporateProfile = repository.GetQuery<CorporateApplicationForm>(f => f.FrontendUserID == frontEnd).FirstOrDefault();
        //    if (corporateProfile == null)
        //        return RedirectToAction("CorporationProfile", new { Msg = App_GlobalResources.General.MsgApply });

        //    ViewBag.imgSrc = $"data:image/gif;base64,{Convert.ToBase64String(corporateProfile.Picture)}";
        //    ViewBag.lang = CultureHelper.CurrentCulture;

        //    // التحقق من وجود الورشة
        //    var incubationWS = repository.GetByKey<IncubationWorkshop>(id);
        //    if (incubationWS == null)
        //        return RedirectToAction("dashboard", new { Msg = "Workshop not found." });
        //    ViewBag.WorkshopModel = incubationWS.IncubationWorkshopModel?.NameAR ?? "";
        //    ViewBag.StartDate = incubationWS.StartDate != null ? incubationWS.StartDate.ToString("yyyy-MM-dd") : "";
        //    ViewBag.EndDate = incubationWS.EndDate != null ? incubationWS.EndDate.ToString("yyyy-MM-dd") : "";
        //    ViewBag.LastTimeToApply = incubationWS.LastTimeToApply != null ? incubationWS.LastTimeToApply.ToString("yyyy-MM-dd") : "";
        //    ViewBag.Duration = (incubationWS.StartDate != null && incubationWS.EndDate != null)
        //        ? ((incubationWS.EndDate - incubationWS.StartDate).Days + 1).ToString() + ""
        //        : "";
        //    ViewBag.FundingSource = incubationWS.FundingSource?.FundingSourceNameAR ?? "";
        //    ViewBag.Region = incubationWS.Region?.RegionNameAR ?? "";
        //    ViewBag.Governorate = incubationWS.Governorate?.GovernorateAR ?? "";
        //    ViewBag.City = incubationWS.City?.CityNameAR ?? "";
        //    ViewBag.Description = incubationWS.Description ?? "";

        //    ViewBag.WorkshopName = incubationWS.Name;

        //    //ViewBag.Workshop = incubationWS;

        //    ViewBag.WorkshopID = id;
        //    //ViewBag.StartDate = incubationWS.StartDate != null ? incubationWS.StartDate.ToString("yyyy-MM-dd") : "";
        //    //ViewBag.EndDate = incubationWS.EndDate != null ? incubationWS.EndDate.ToString("yyyy-MM-dd") : "";

        //    //ViewBag.Description = incubationWS.Description;
        //    //ViewBag.Consultant = incubationWS.Consultant?.Name; // إذا كان هناك مستشار مرتبط
        //    //ViewBag.Region = incubationWS.Region?.RegionNameAR; // إذا كان هناك منطقة مرتبطة

        //    // التحقق من حالة قبول المستخدم
        //    var isAccepted = repository.GetQuery<FrontendUser>(f =>
        //                f.FK_AspUser == frontEnd.ToString() &&
        //                f.CorporateApplicationForms.Any(g => g.CorporateApplicationStatu.ApplicantStatu.ApplicantStatusName == "Accepted"))
        //                .Any();

        //    if (!isAccepted)
        //        return RedirectToAction("CorporationProfile", new { Msg = App_GlobalResources.General.MsgApply });

        //    // جلب القيم الحالية إذا كانت موجودة
        //    var incIL = repository.GetQuery<IncubationWorkshopBLTransactionsValue>(v =>
        //        v.FrontendUserID == frontEnd &&
        //        v.IncubationWorkshopBLTrans.IncubationWorkshopID == id)
        //        .Include(v => v.IncubationWorkshopBLTrans)
        //        .Include(v => v.IncubationWorkshopBLTrans.Options)
        //        .Include(v => v.OptionValues)
        //        .Include(v => v.IncubationWorkshopBLTransValStatus)
        //        .ToList();

        //    if (incIL.Any())
        //    {
        //        ViewBag.IncubBLStatus = incIL.FirstOrDefault()?.IncubationWorkshopBLTransValStatus?.NameEN;
        //        ViewBag.Msg = incIL.FirstOrDefault()?.Feadback;

        //        // إعادة النموذج المرتبط بالقيم الموجودة
        //        var transType = incIL.FirstOrDefault()?.IncubationWorkshopBLTrans?.IncubationWorkshopBLTransType;

        //        if (transType != null)
        //        {
        //            // **فقط الحقول التي سمح بها الأدمن ستُعرض
        //            transType.IncubationWorkshopBLTrans = transType.IncubationWorkshopBLTrans
        //                .Where(trans => trans.ViewList_Display == "true") //  تصفية الحقول
        //                .ToList();


        //            // إعادة تحميل الخيارات المحددة
        //            foreach (var trans in transType.IncubationWorkshopBLTrans)
        //            {
        //                trans.IncubationWSBLTransactionsValue = incIL
        //                    .Where(v => v.TransID == trans.TransID)
        //                    .ToList();
        //            }
        //        }

        //        return View(transType);
        //    }

        //    // إذا لم تكن هناك قيم، إعادة النموذج الأساسي للورشة
        //    var transactionType = repository.GetQuery<IncubationWorkshopBLTransactionsType>()
        //        .Include(t => t.IncubationWorkshopBLTrans.Select(trans => trans.Options))
        //        .Include(t => t.IncubationWorkshopBLTrans.Select(trans => trans.IncubationWSBLTransactionsValue))
        //        .Include(t => t.IncubationWorkshopBLTrans.Select(trans => trans.IncubationWSBLTransactionsValue.Select(val => val.IncubationWSBLTransValueAttachment)))
        //        .Include(t => t.IncubationWorkshopBLTrans.Select(trans => trans.IncubationWSControlsType))
        //        .FirstOrDefault(t => t.TransTypeName == "IncubationWorkshopBaselineTransactions" &&
        //                             t.IncubationWorkshopBLTrans.Any(trans => trans.IncubationWorkshopID == id));

        //    if (transactionType != null)
        //    {
        //        // **فقط الحقول المسموحة ستُعرض**
        //        transactionType.IncubationWorkshopBLTrans = transactionType.IncubationWorkshopBLTrans
        //            .Where(trans => trans.IncubationWorkshopID == id && trans.ViewList_Display == "true") //  التصنيف حسب إذن العرض
        //            .ToList();
        //        ViewBag.TransTypeName = transactionType.TransTypeName;
        //    //    transactionType.IncubationWorkshopBLTrans = transactionType.IncubationWorkshopBLTrans
        //    //        .Where(trans => trans.IncubationWorkshopID == id)
        //    //        .ToList();
        //    }

        //    return View(transactionType);
        //}
        [HttpGet]
        public ActionResult IncWSBaselineAppForm(Guid id)
        {
            var frontEnd = Guid.Parse(User.Identity.GetUserId());
            ViewBag.frontEnd = frontEnd;
            // التحقق من ملف المنظمة
            var corporateProfile = repository.GetQuery<CorporateApplicationForm>(f => f.FrontendUserID == frontEnd).FirstOrDefault();
            if (corporateProfile == null)
                return RedirectToAction("CorporationProfile", new { Msg = App_GlobalResources.General.MsgApply });

            ViewBag.imgSrc = $"data:image/gif;base64,{Convert.ToBase64String(corporateProfile.Picture)}";
            ViewBag.lang = CultureHelper.CurrentCulture;

            // التحقق من وجود الورشة
            var incubationWS = repository.GetByKey<IncubationWorkshop>(id);
            if (incubationWS == null)
                return RedirectToAction("dashboard", new { Msg = "Workshop not found." });
            ViewBag.WorkshopModel = incubationWS.IncubationWorkshopModel?.NameAR ?? "";
            ViewBag.StartDate = incubationWS.StartDate != null ? incubationWS.StartDate.ToString("yyyy-MM-dd") : "";
            ViewBag.EndDate = incubationWS.EndDate != null ? incubationWS.EndDate.ToString("yyyy-MM-dd") : "";
            ViewBag.LastTimeToApply = incubationWS.LastTimeToApply != null ? incubationWS.LastTimeToApply.ToString("yyyy-MM-dd") : "";
            ViewBag.Duration = (incubationWS.StartDate != null && incubationWS.EndDate != null)
                ? ((incubationWS.EndDate - incubationWS.StartDate).Days + 1).ToString() + ""
                : "";
            ViewBag.FundingSource = incubationWS.FundingSource?.FundingSourceNameAR ?? "";
            ViewBag.Region = incubationWS.Region?.RegionNameAR ?? "";
            ViewBag.Governorate = incubationWS.Governorate?.GovernorateAR ?? "";
            ViewBag.City = incubationWS.City?.CityNameAR ?? "";
            ViewBag.Description = incubationWS.Description ?? "";

            ViewBag.WorkshopName = incubationWS.Name;

            //ViewBag.Workshop = incubationWS;

            ViewBag.WorkshopID = id;

            // التحقق من حالة قبول المستخدم
            var isAccepted = repository.GetQuery<FrontendUser>(f =>
                        f.FK_AspUser == frontEnd.ToString() &&
                        f.CorporateApplicationForms.Any(g => g.CorporateApplicationStatu.ApplicantStatu.ApplicantStatusName == "Accepted"))
                        .Any();

            if (!isAccepted)
                return RedirectToAction("CorporationProfile", new { Msg = App_GlobalResources.General.MsgApply });

            // جلب جميع الحقول للنموذج مع القيم الحالية إن وجدت
            var transactionType = repository.GetQuery<IncubationWorkshopBLTransactionsType>()
                .Include(t => t.IncubationWorkshopBLTrans.Select(trans => trans.Options))
                .Include(t => t.IncubationWorkshopBLTrans.Select(trans => trans.IncubationWSControlsType))
                .FirstOrDefault(t => t.TransTypeName == "IncubationWorkshopBaselineTransactions" &&
                                     t.IncubationWorkshopBLTrans.Any(trans => trans.IncubationWorkshopID == id));

            if (transactionType != null)
            {
                // **جلب جميع الحقول حتى لو لم يكن لديها قيم**
                transactionType.IncubationWorkshopBLTrans = transactionType.IncubationWorkshopBLTrans
                    .Where(trans => trans.IncubationWorkshopID == id && trans.ViewList_Display == "true") // التصفية حسب إذن العرض
                    .ToList();

                // جلب القيم الحالية
                var incIL = repository.GetQuery<IncubationWorkshopBLTransactionsValue>(v =>
                    v.FrontendUserID == frontEnd &&
                    v.IncubationWorkshopBLTrans.IncubationWorkshopID == id)
                    .Include(v => v.IncubationWorkshopBLTrans)
                    .Include(v => v.OptionValues)
                    .Include(v => v.IncubationWorkshopBLTransValStatus)
                    .ToList();

                if (incIL.Any())
                {
                    ViewBag.IncubBLStatus = incIL.FirstOrDefault()?.IncubationWorkshopBLTransValStatus?.NameEN;
                    ViewBag.Msg = incIL.FirstOrDefault()?.Feadback;
                }

                // دمج القيم الحالية مع الحقول المتاحة
                foreach (var trans in transactionType.IncubationWorkshopBLTrans)
                {
                    // العثور على القيم المحفوظة لهذا الحقل
                    trans.IncubationWSBLTransactionsValue = incIL
                        .Where(v => v.TransID == trans.TransID)
                        .ToList();

                    // إذا لم تكن هناك قيم سابقة، أضف كائنًا فارغًا للحفاظ على الحقل في النموذج
                    if (!trans.IncubationWSBLTransactionsValue.Any())
                    {
                        trans.IncubationWSBLTransactionsValue.Add(new IncubationWorkshopBLTransactionsValue
                        {
                            TransValueID = Guid.NewGuid(),
                            Value = "", // لا يوجد قيمة سابقة
                            FrontendUserID = frontEnd,
                            TransID = trans.TransID,
                            SubmissionDate = DateTime.UtcNow,
                            IncubationWorkshopBLTransValStatusID = repository.Get<IncubationWorkshopBLTransValStatus>(s => s.NameEN == "Pending").FirstOrDefault()?.IncubationWorkshopBLTransValStatusID ?? Guid.Empty
                        });
                    }
                }

                ViewBag.TransTypeName = transactionType.TransTypeName;
            }

            return View(transactionType);
        }

        public class TransactionOptions
        {
            public string TransID { get; set; }
            public List<OptionData> Options { get; set; }
        }

        public class OptionData
        {
            public string OptionID { get; set; }
            public string Value { get; set; }
        }
        [HttpGet]
 
        public JsonResult GetWorkshopDetails(Guid id)
        {
            ViewBag.lang = CultureHelper.CurrentCulture; 
            int lang = Convert.ToInt32(ViewBag.lang); 

            var workshopEntity = repository.GetQuery<IncubationWorkshop>()
                .Where(w => w.IncubationWorkshopID == id)
                .FirstOrDefault(); // جلب البيانات أولًا

            if (workshopEntity == null)
            {
                return Json(new { success = false, message = "لم يتم العثور على الورشة." }, JsonRequestBehavior.AllowGet);
            }

            // جلب ملف الإقرار المرتبط
            var commitmentAttachment = repository.GetQuery<IncubationWorkshopAttachment>()
                .FirstOrDefault(a => a.IncubationWorkshopID == id && a.IsCommitmentFile);
            var baseUploadPath = Server.MapPath("~/Uploads/");
           

            // تأكد أن المسار موجود
            string relativePath = null;

            if (commitmentAttachment != null && !string.IsNullOrEmpty(commitmentAttachment.URL))
            {
                if (commitmentAttachment.URL.StartsWith(baseUploadPath))
                {
                    relativePath = commitmentAttachment.URL.Substring(baseUploadPath.Length).Replace("\\", "/");
                }
                else
                {
                    // في حال تم تخزين المسار بطريقة مختلفة
                    relativePath = commitmentAttachment.URL.Replace("\\", "/").Split(new[] { "Uploads/" }, StringSplitOptions.None).LastOrDefault();
                }
            }

            string fileUrl = !string.IsNullOrEmpty(relativePath) ? Url.Content("~/Uploads/" + relativePath) : null;

            // تحويل القيم بعد استرجاع البيانات
            var workshop = new
            {
                Name = workshopEntity.Name,
                WorkshopModel = workshopEntity.IncubationWorkshopModel != null
                    ? (lang == 3 ? (workshopEntity.IncubationWorkshopModel.NameAR ?? "N/A") : (workshopEntity.IncubationWorkshopModel.NameEN ?? "N/A"))
                    : "N/A",
                StartDate = workshopEntity.StartDate.ToString("yyyy-MM-dd"), // التحويل خارج LINQ
                EndDate = workshopEntity.EndDate.ToString("yyyy-MM-dd"), // التحويل خارج LINQ
                LastTimeToApply = workshopEntity.LastTimeToApply.ToString("yyyy-MM-dd"), // التحويل خارج LINQ
                Region = lang == 3
                    ? (workshopEntity.Region != null ? workshopEntity.Region.RegionNameAR ?? "N/A" : "N/A")
                    : (workshopEntity.Region != null ? workshopEntity.Region.RegionNameEN ?? "N/A" : "N/A"),
                Governorate = lang == 3
                    ? (workshopEntity.Governorate != null ? workshopEntity.Governorate.GovernorateAR ?? "N/A" : "N/A")
                    : (workshopEntity.Governorate != null ? workshopEntity.Governorate.GovernorateEN ?? "N/A" : "N/A"),
                City = lang == 3
                    ? (workshopEntity.City != null ? workshopEntity.City.CityNameAR ?? "N/A" : "N/A")
                    : (workshopEntity.City != null ? workshopEntity.City.CityNameEN ?? "N/A" : "N/A"),
                Description = workshopEntity.Description ?? "لا يوجد وصف متاح",
                CommitmentFileUrl = fileUrl,
               CommitmentFileName = commitmentAttachment?.Name


            };

            return Json(new { success = true, data = workshop }, JsonRequestBehavior.AllowGet);
        }





        public ActionResult IncWSBaselineAppForm(IncubationWorkshopBLTransactionsType model, string WorkshopName, Guid WorkshopID, string CheckboxData)
        {

            ViewBag.lang = CultureHelper.CurrentCulture;
            var frontEnd = Guid.Parse(User.Identity.GetUserId());
            try
            {
                var CorporateProfile = repository.GetQuery<CorporateApplicationForm>(f => f.FrontendUserID == frontEnd).FirstOrDefault();
                if (CorporateProfile != null)
                {
                    var img = Convert.ToBase64String(CorporateProfile.Picture);
                    ViewBag.imgSrc = $"data:image/gif;base64,{img}";
                }
                var transType = repository.GetQuery<IncubationWorkshopBLTransactionsType>()
                .Include(t => t.IncubationWorkshopBLTrans.Select(trans => trans.Options))
                .Include(t => t.IncubationWorkshopBLTrans.Select(trans => trans.IncubationWSBLTransactionsValue))
                .Include(t => t.IncubationWorkshopBLTrans.Select(trans => trans.IncubationWSBLTransactionsValue.Select(val => val.IncubationWSBLTransValueAttachment)))
                .Include(t => t.IncubationWorkshopBLTrans.Select(trans => trans.IncubationWSControlsType))
                .FirstOrDefault(t => t.TransTypeName == "IncubationWorkshopBaselineTransactions" &&
                                     t.IncubationWorkshopBLTrans.Any(trans => trans.IncubationWorkshopID == WorkshopID));

                //var transType = repository.FindOne<IncubationWorkshopBLTransactionsType>(t => (t.TransTypeID.ToString() == "2655D2B1-90D2-4C17-B9E7-A0448CD8CB67"));
                if (transType == null)
                {
                    TempData["Msg1"] = "لا يوجد بيانات لتحديثها.";
                   
                    
                    
                    return RedirectToAction("dashboard");
                }

                if(CheckboxData != null)
                { 
             
                var checkboxSelections = JsonConvert.DeserializeObject<List<TransactionOptions>>(CheckboxData);
                if (checkboxSelections != null || checkboxSelections.Any())
                {

                  
                    foreach (var transc in checkboxSelections)
                    {
                        Guid transID = Guid.Parse(transc.TransID);

                        // تحديد الخيارات المختارة
                        var selectedOptions = transc.Options.Where(o => o.Value == "true").ToList();
                        int selectedCount = selectedOptions.Count;


                        var transValue = repository.GetQuery<IncubationWorkshopBLTransactionsValue>()
                        .FirstOrDefault(v => v.TransID == transID && v.FrontendUserID == frontEnd);

                      
                        if (transValue != null)
                        {
                            // حذف الخيارات السابقة
                            var existingOptions = transValue.OptionValues.ToList();
                            foreach (var oldOption in existingOptions)
                            {
                                repository.Delete(oldOption);
                            }

                            // إضافة الخيارات الجديدة
                            foreach (var option in transc.Options.Where(o => o.Value == "true"))
                            {
                                var optionValue = new IncubationWorkshopOptionValues
                                {
                                    ValueID = Guid.NewGuid(),
                                    TransID = transID,
                                    TransValueID = transValue.TransValueID,
                                    OptionID = Guid.Parse(option.OptionID),
                                    Value = option.Value
                                };
                                repository.Add(optionValue);
                            }

                            // تحديث حالة الترانزكشن
                            transValue.SubmissionDate = DateTime.UtcNow;
                            transValue.IncubationWorkshopBLTransValStatusID = repository
                                .Get<IncubationWorkshopBLTransValStatus>(s => s.NameEN == "Baseline Application Form Updated")
                                .FirstOrDefault()
                                .IncubationWorkshopBLTransValStatusID;

                           
                            TempData["Msg"] = "نشكركم على تحديث استمارة تسجيل المرشح من قبلكم ، وسيتم تأكيد قبول المشارك والتواصل معكم قريباً";


                        }
                        else
                        {
                            // إضافة سجل جديد في IncubationWorkshopBLTransactionsValue
                            var newTransValue = new IncubationWorkshopBLTransactionsValue
                            {
                                TransValueID = Guid.NewGuid(),
                                TransID = transID,
                                Value = selectedCount.ToString(),
                                SubmissionDate = DateTime.UtcNow,
                                FrontendUserID = Guid.Parse(User.Identity.GetUserId()),
                                IncubationWorkshopBLTransValStatusID = repository.Get<IncubationWorkshopBLTransValStatus>(s => s.NameEN == "Pending").FirstOrDefault().IncubationWorkshopBLTransValStatusID
                              
                            };

                            repository.Add(newTransValue);

                            // إضافة الخيارات المحددة إلى IncubationWorkshopOptionValues
                            foreach (var option in selectedOptions)
                            {
                                var optionValue = new IncubationWorkshopOptionValues
                                {
                                    ValueID = Guid.NewGuid(),
                                    TransID = transID,
                                    TransValueID = newTransValue.TransValueID,
                                    OptionID = Guid.Parse(option.OptionID),
                                    Value = option.Value
                                };
                                repository.Add(optionValue);
                            }


                            TempData["Msg"] = "نشكركم على تعبئة استمارة تسجيل المرشح من قبلكم ، وسيتم تأكيد قبول المشارك والتواصل معكم قريباً";
                           

                        }
                    }


                }
                    // transaction.Commit();
                }
                repository.UnitOfWork.SaveChanges();
                var formKeys = Request.Form.AllKeys.Where(f => f != "WorkshopName" && f != "WorkshopID" && !Request.Files.AllKeys.Contains(f.Split('_')[0])) .ToList();
                // تحويل بيانات الراديو باتن المرسلة من النموذج إلى Dictionary
                // استخراج فقط الـ Radio Button من الـ Request.Form
                var radioSelections = new Dictionary<Guid, Guid>();

                foreach (var key in Request.Form.AllKeys
                    .Where(k => k.StartsWith("SelectedOptions") && !k.Contains("]["))
                )
                {
                    var value = Request.Form[key]; // القيمة المختارة (OptionID)
                    if (!string.IsNullOrEmpty(value))
                    {
                        var transID = Guid.Parse(key.Split('[')[1].Split(']')[0]); // استخراج TransID
                        var selectedOptionID = Guid.Parse(value); // استخراج OptionID
                        radioSelections[transID] = selectedOptionID;
                    }
                }
                if (radioSelections.Any())
                {
                    foreach (var radioSelection in radioSelections)
                    {
                        var transID = radioSelection.Key; // ID للسؤال
                        var selectedOptionID = radioSelection.Value; // ID للخيار المختار

                        var transValue = repository.GetQuery<IncubationWorkshopBLTransactionsValue>()
                            .FirstOrDefault(v => v.TransID == transID && v.FrontendUserID == frontEnd);

                        if (transValue != null)
                        {
                            // حذف القيم السابقة المرتبطة بنفس الـ TransID لأنه يجب أن يكون هناك خيار واحد فقط
                            var existingOptions = transValue.OptionValues.ToList();
                            foreach (var oldOption in existingOptions)
                            {
                                repository.Delete(oldOption);
                            }

                            // إضافة الخيار الجديد
                            var newOptionValue = new IncubationWorkshopOptionValues
                            {
                                ValueID = Guid.NewGuid(),
                                TransID = transID,
                                TransValueID = transValue.TransValueID,
                                OptionID = selectedOptionID,
                                Value = "true" // التأكيد أن هذا الخيار هو المحدد
                            };
                            repository.Add(newOptionValue);

                            // تحديث حالة الترانزكشن
                            transValue.SubmissionDate = DateTime.UtcNow;
                            transValue.IncubationWorkshopBLTransValStatusID = repository
                                .Get<IncubationWorkshopBLTransValStatus>(s => s.NameEN == "Baseline Application Form Updated")
                                .FirstOrDefault()
                                .IncubationWorkshopBLTransValStatusID;
                        }
                        else
                        {
                            // إضافة سجل جديد إذا لم يكن هناك قيمة محفوظة مسبقًا
                            var newTransValue = new IncubationWorkshopBLTransactionsValue
                            {
                                TransValueID = Guid.NewGuid(),
                                TransID = transID,
                                Value = "1", // يرمز إلى اختيار واحد فقط
                                SubmissionDate = DateTime.UtcNow,
                                FrontendUserID = frontEnd,
                                IncubationWorkshopBLTransValStatusID = repository
                                    .Get<IncubationWorkshopBLTransValStatus>(s => s.NameEN == "Pending")
                                    .FirstOrDefault()
                                    .IncubationWorkshopBLTransValStatusID
                            };
                            repository.Add(newTransValue);

                            // إضافة الخيار المحدد إلى IncubationWorkshopOptionValues
                            var optionValue = new IncubationWorkshopOptionValues
                            {
                                ValueID = Guid.NewGuid(),
                                TransID = transID,
                                TransValueID = newTransValue.TransValueID,
                                OptionID = selectedOptionID,
                                Value = "true"
                            };
                            repository.Add(optionValue);
                        }
                    }
                }
                repository.UnitOfWork.SaveChanges();

                foreach (var key in formKeys)
                {
                    var trans = transType.IncubationWorkshopBLTrans?.FirstOrDefault(f => f.FieldNameEn.Trim().Replace(" ", string.Empty) == key);
                    if (trans == null) continue;
                    var transValue = trans.IncubationWSBLTransactionsValue?.FirstOrDefault(v => v.FrontendUserID == frontEnd && v.IncubationWorkshopBLTrans.IncubationWorkshopID == WorkshopID);
                
                    if (transValue != null)
                    {

                        if (trans.IncubationWSControlsType.IncubationWorkshopControl.ControlsName == "Checkbox")
                        {
                            
                        }


                        else
                        {

                            transValue.Value = Request.Form[key];
                            transValue.FrontendUserID = frontEnd;
                            transValue.SubmissionDate = DateTime.UtcNow;
                            transValue.IncubationWorkshopBLTransValStatusID = repository.Get<IncubationWorkshopBLTransValStatus>(s => s.NameEN == "Baseline Application Form Updated").FirstOrDefault().IncubationWorkshopBLTransValStatusID;
                        }

                        TempData["Msg1"] = "نشكركم على تحديث استمارة تسجيل المرشح من قبلكم ، وسيتم تأكيد قبول المشارك والتواصل معكم قريباً";
                    }
                    else
                    {
                        bool hasPreviousResponses = repository.GetQuery<IncubationWorkshopBLTransactionsValue>()
                          .Any(v => v.FrontendUserID == frontEnd && v.IncubationWorkshopBLTrans.IncubationWorkshopID == WorkshopID);

                        // تحديد الحالة بناءً على وجود إجابات سابقة
                        Guid finalStatus = hasPreviousResponses
                            ? repository.Get<IncubationWorkshopBLTransValStatus>(s => s.NameEN == "Baseline Application Form Updated").FirstOrDefault().IncubationWorkshopBLTransValStatusID
                            : repository.Get<IncubationWorkshopBLTransValStatus>(s => s.NameEN == "Pending").FirstOrDefault().IncubationWorkshopBLTransValStatusID;
                        trans.IncubationWSBLTransactionsValue.Add(new IncubationWorkshopBLTransactionsValue
                        {
                            TransValueID = Guid.NewGuid(),
                            Value = Request.Form[key],
                            FrontendUserID = frontEnd,
                            TransID = trans.TransID,
                            SubmissionDate = DateTime.UtcNow,
                            IncubationWorkshopBLTransValStatusID = repository.Get<IncubationWorkshopBLTransValStatus>(s => s.NameEN == "Pending").FirstOrDefault().IncubationWorkshopBLTransValStatusID
                        });
                        TempData["Msg1"] = "نشكركم على تعبئة استمارة تسجيل المرشح من قبلكم ، وسيتم تأكيد قبول المشارك والتواصل معكم قريباً";

                    }
                }
                repository.UnitOfWork.SaveChanges();
                //repository.UnitOfWork.SaveChanges();

                foreach (var key in Request.Files.AllKeys.Distinct())
                {
                    string FolderName = User.Identity.Name;
                    string path = Server.MapPath("~/Uploads/" + FolderName + "/");
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);
                    var reqfiles = Request.Files.GetMultiple(key)
                            .Where(f => f.ContentLength > 0)
                            .DistinctBy(f => f.FileName)
                            .ToList();
                    //var reqfiles = Request.Files.GetMultiple(key).Where(f => f.ContentLength > 0).ToList();
                    var trans = transType.IncubationWorkshopBLTrans?.FirstOrDefault(f => f.FieldNameEn.Trim().Replace(" ", string.Empty) == key);
                    var selectedTransValue = trans.IncubationWSBLTransactionsValue?.FirstOrDefault(v => v.FrontendUserID == frontEnd && v.IncubationWorkshopBLTrans.IncubationWorkshopID == WorkshopID);

                    if (selectedTransValue?.Value != null && reqfiles.Count > 0)
                    {
                        selectedTransValue.Value = $"{selectedTransValue?.IncubationWSBLTransValueAttachment?.Count + reqfiles.Count} Attached File";
                        selectedTransValue.FrontendUserID = frontEnd;
                        selectedTransValue.SubmissionDate = DateTime.UtcNow;
                        selectedTransValue.IncubationWorkshopBLTransValStatusID = repository.Get<IncubationWorkshopBLTransValStatus>(s => s.NameEN == "Baseline Application Form Updated").FirstOrDefault().IncubationWorkshopBLTransValStatusID;

                        foreach (var item in reqfiles)
                        {
                            item.SaveAs(path + Path.GetFileName(item.FileName));
                            selectedTransValue.IncubationWSBLTransValueAttachment.Add(new IncubationWorkshopBLTransValueAttachment()
                            {
                                TransAttachmentID = Guid.NewGuid(),
                                Name = Path.GetFileName(item.FileName),
                                ScreenName = $"IncWSBaselineAppForm",
                                Type = item.ContentType,
                                Size = item.ContentLength.ToString(),
                                URL = path + Path.GetFileName(item.FileName)
                            });
                        }

                    }
                    else if (selectedTransValue?.Value == null && reqfiles.Count > 0)
                    {
                        var transAttachmentLst = new List<IncubationWorkshopBLTransValueAttachment>();
                        transAttachmentLst.Clear();
                        foreach (var item in reqfiles)
                        {
                            item.SaveAs(path + Path.GetFileName(item.FileName));
                            transAttachmentLst.Add(new IncubationWorkshopBLTransValueAttachment()
                            {
                                TransAttachmentID = Guid.NewGuid(),
                                Name = Path.GetFileName(item.FileName),
                                ScreenName = $"IncWSBaselineAppForm",
                                Type = item.ContentType,
                                Size = item.ContentLength.ToString(),
                                URL = path + Path.GetFileName(item.FileName)
                            });
                        }

                        trans.IncubationWSBLTransactionsValue.Add(new IncubationWorkshopBLTransactionsValue()
                        {
                            TransValueID = Guid.NewGuid(),
                            Value = $"{reqfiles.Count} Attached File",
                            FrontendUserID = frontEnd,
                            SubmissionDate = DateTime.UtcNow,
                            IncubationWorkshopBLTransValStatusID = repository.Get<IncubationWorkshopBLTransValStatus>(s => s.NameEN == "Pending").FirstOrDefault().IncubationWorkshopBLTransValStatusID,
                            IncubationWSBLTransValueAttachment = transAttachmentLst
                        });

                    }
                }

                repository.UnitOfWork.SaveChanges();
                //var _IncubationBaseline = repository.GetQuery<IncubationWorkshopBLTransactionsValue>(v => v.FrontendUserID == frontEnd && v.IncubationWorkshopBLTrans.IncubationWorkshopID == WorkshopID).ToList();
                //_IncubationBaseline.ForEach(v => { v.SubmissionDate = DateTime.UtcNow; v.IncubationWorkshopBLTransValStatusID = transType != null ? repository.Get<IncubationWorkshopBLTransValStatus>(s => s.NameEN == "Baseline Application Form Updated").FirstOrDefault().IncubationWorkshopBLTransValStatusID : repository.Get<IncubationWorkshopBLTransValStatus>(s => s.NameEN == "Pending").FirstOrDefault().IncubationWorkshopBLTransValStatusID; });

                // الحصول على معرف الحالة "Baseline Application Form Updated"
                Guid updatedStatus = repository.Get<IncubationWorkshopBLTransValStatus>(s => s.NameEN == "Baseline Application Form Updated")
                    .FirstOrDefault()?.IncubationWorkshopBLTransValStatusID ?? Guid.Empty;

                // الحصول على معرف الحالة الافتراضية "Pending"
                Guid pendingStatus = repository.Get<IncubationWorkshopBLTransValStatus>(s => s.NameEN == "Pending")
                    .FirstOrDefault()?.IncubationWorkshopBLTransValStatusID ?? Guid.Empty;

                var _IncubationBaseline = repository.GetQuery<IncubationWorkshopBLTransactionsValue>()
                    .Where(v => v.FrontendUserID == frontEnd && v.IncubationWorkshopBLTrans.IncubationWorkshopID == WorkshopID)
                    .ToList();

                // تحديث كل السجلات بالحالة الصحيحة
                _IncubationBaseline.ForEach(v =>
                {
                    v.SubmissionDate = DateTime.UtcNow;
                    v.IncubationWorkshopBLTransValStatusID = (transType != null) ? updatedStatus : pendingStatus;
                });

              


                repository.UnitOfWork.SaveChanges();

              
           
                MailHelper mailHelper = new MailHelper();
                    mailHelper.ToEmail = UserManager.GetEmail(User.Identity.GetUserId());
                    mailHelper.Subject = "إفادة";
                    mailHelper.Body = "السلام عليكم ورحمة الله وبركاته،،<br /><br />" +
                    "تهديكم مؤسسة الملك خالد أطيب التحيات، ونشكركم على تعبئة استمارة تسجيلكم من قبلكم لحضور مشروع&nbsp;" + WorkshopName + "&nbsp;، وسيتم تأكيد قبولكم والتواصل معكم قريباً.<br /><br />" +
                    "وتفضلوا بقبول فائق التحية والتقدير،،<br />" +
                    "فريق برنامج بناء القدرات<br />" +
                    "مؤسسة الملك خالد.";
                //shadia26-11-2024
                //mailHelper.Body = "السادة المنظمات غير الربحية الموقرين،<br />" +
                //"السلام عليكم ورحمة الله وبركاته،<br />" +
                //$"تهديكم مؤسسة الملك خالد أطيب التحيات، ونشكركم على تعبئة استمارة تسجيل المرشح من قبلكم  لحضور مشروع&nbsp;{WorkshopName}&nbsp;، <br />" +
                //"وسيتم تأكيد قبول المشارك والتواصل معكم قريباً.<br /><br />" +
                //"وتفضلوا بقبول فائق التحية والتقدير،<br />" +
                //"فريق برنامج بناء القدرات<br />" +
                //"مؤسسة الملك خالد.";
                mailHelper.Send("");

                //}
                return Json(new { success = true });

               
                // return RedirectToAction("dashboard");
            }

            catch (Exception ex)
            {
                TempData["Msg1"] = "Error";
                return Json(new { success = false });
                // return RedirectToAction("dashboard");
                //return RedirectToAction("dashboard");
            }
            //  TempData["err"] = "There are something is not valid, please try again later or call the administrator.";

        }

        
                      
       
        [HttpGet]
            public ActionResult BaselineApplicationForm()
            {
                var frontEnd = Guid.Parse(User.Identity.GetUserId());
                var CorporateProfile = repository.GetQuery<CorporateApplicationForm>(f => f.FrontendUserID == frontEnd).FirstOrDefault();
                var img = Convert.ToBase64String(CorporateProfile.Picture);
           

                ViewBag.imgSrc = string.Format("data:image/gif;base64,{0}", img);

                if (repository.GetQuery<FrontendUser>(f => f.FK_AspUser == frontEnd.ToString() && f.CorporateApplicationForms.Any(g => g.CorporateApplicationStatu.ApplicantStatu.ApplicantStatusName == "Accepted")).Count() >= 1)
                {
                    var IncIL = repository.GetQuery<IncubationBaseline>(i => i.FrontendUserID == frontEnd).ToList();
                    if (IncIL.Count > 0)
                    {
                        ViewBag.dateBoardApproval = IncIL.FirstOrDefault().DateBoardApproval?.ToString("yyyy-MM-dd");
                        ViewBag.IncubBLStatus = IncIL.FirstOrDefault().IncubationBaselineStatus.NameEN;
                        @ViewBag.Msg = IncIL.FirstOrDefault().Feadback;
                        IncubationBaselineVM IncILVM = new IncubationBaselineVM()
                        {
                            IncubationBL = IncIL.FirstOrDefault()
                        };
                        return View(IncILVM);
                    }
                    ViewBag.IncubBLStatus = "";
                ////shadia
                ////ViewBag.UserNotice = "عزيزي المستخدم،<br />يرجى ملاحظة أن هذا النموذج غير مطلوب إلا في حالة التقدم بطلبات الاحتضان الكامل أو الاحتضان الجزئي.<br />إذا كنت ترغب في الاستمرار، يرجى النقر على 'تأكيد' للانتقال إلى النموذج.";
                //if (IncIL.Count == 0)
                //{
                //    TempData["UserNotice"] = "عزيزي المستخدم،يرجى ملاحظة أن هذا النموذج غير مطلوب إلا في حالة التقدم بطلبات الاحتضان الكامل أو الاحتضان الجزئي.إذا كنت ترغب في الاستمرار، يرجى النقر على 'تأكيد' للانتقال إلى النموذج.";
                //    System.Diagnostics.Debug.WriteLine("TempData[UserNotice]: " + TempData["UserNotice"]);
                //}

             
                    return View(new IncubationBaselineVM() { IncubationBL = new IncubationBaseline() });
                }
           
                return RedirectToAction("CorporationProfile", new { Msg = App_GlobalResources.General.MsgApply });
            }

        [HttpPost]
        public ActionResult BaselineApplicationForm(IncubationBaselineVM model)
        {
            try
            {
                var frontEnd = Guid.Parse(User.Identity.GetUserId());
                string FolderName = User.Identity.Name;
                string path = Server.MapPath("~/Uploads/" + FolderName + "/");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                if (model.StrategicPlan != null)
                {
                    model.StrategicPlan.SaveAs(path + Path.GetFileName(model.StrategicPlan.FileName));
                    model.IncubationBL.StrategicPlan = path + Path.GetFileName(model.StrategicPlan.FileName);
                }
                if (model.OperatingPlan != null)
                {
                    model.OperatingPlan.SaveAs(path + Path.GetFileName(model.OperatingPlan.FileName));
                    model.IncubationBL.OperatingPlan = path + Path.GetFileName(model.OperatingPlan.FileName);
                }
                if (model.GovernanceGuide != null)
                {
                    model.GovernanceGuide.SaveAs(path + Path.GetFileName(model.GovernanceGuide.FileName));
                    model.IncubationBL.GovernanceGuide = path + Path.GetFileName(model.GovernanceGuide.FileName);
                }
                if (model.CharteredAccountantNotes != null)
                {
                    model.CharteredAccountantNotes.SaveAs(path + Path.GetFileName(model.CharteredAccountantNotes.FileName));
                    model.IncubationBL.CharteredAccountantNotes = path + Path.GetFileName(model.CharteredAccountantNotes.FileName);
                }
                if (model.AttachFollowUpandEvaluationForms != null)
                {
                    model.AttachFollowUpandEvaluationForms.SaveAs(path + Path.GetFileName(model.AttachFollowUpandEvaluationForms.FileName));
                    model.IncubationBL.AttachFollowUpandEvaluationForms = path + Path.GetFileName(model.AttachFollowUpandEvaluationForms.FileName);
                }
                model.IncubationBL.FrontendUserID = frontEnd;
                model.IncubationBL.submissionDate = DateTime.Now;
                var IncIL = repository.GetQuery<IncubationBaseline>(i => i.FrontendUserID == frontEnd).ToList();
                if (IncIL.Count > 0)
                {
                    if (!model.DraftFlage)
                        switch (IncIL.FirstOrDefault().IncubationBaselineStatus.NameEN)
                        {
                            case "Draft":
                                model.IncubationBL.IncubationBaselineStatusID = repository.Get<IncubationBaselineStatus>(s => s.NameEN == "Pending").FirstOrDefault().IncubationBaselineStatusID;
                                break;
                            case "Update Baseline Application Form":
                                model.IncubationBL.IncubationBaselineStatusID = repository.Get<IncubationBaselineStatus>(s => s.NameEN == "Baseline Application Form Updated").FirstOrDefault().IncubationBaselineStatusID;
                                model.IncubationBL.Feadback = null;
                                break;
                        }
                    repository.Update(model.IncubationBL);
                }
                else
                {
                    model.IncubationBL.IncubationBaselineStatusID = model.DraftFlage ? repository.Get<IncubationBaselineStatus>(s => s.NameEN == "Draft").FirstOrDefault().IncubationBaselineStatusID : repository.Get<IncubationBaselineStatus>(s => s.NameEN == "Pending").FirstOrDefault().IncubationBaselineStatusID;
                    model.IncubationBL.IncubationBaselineID = Guid.NewGuid();
                    repository.Add(model.IncubationBL);
                }
                repository.UnitOfWork.SaveChanges();
                var IncubationBaselineStatus = repository.GetByKey<IncubationBaselineStatus>(model.IncubationBL.IncubationBaselineStatusID);
                if (IncubationBaselineStatus.NameEN == "Pending" || IncubationBaselineStatus.NameEN == "Baseline Application Form Updated")
                {
                    MailHelper mailHelper = new MailHelper();
                    mailHelper.ToEmail = UserManager.GetEmail(User.Identity.GetUserId());
                    mailHelper.Subject = "إفادة";
                    //mailHelper.Body = "المكرمين/ المنظمات غير الربحية                         الموقرين <br />"
                    //                                + "السلام عليكم ورحمة الله وبركاته، <br />"
                    //                                + "نتقدم لكم بالشكر على تعبئة نموذج الالتحاق بمشاريع ورش العمل، <br />"
                    //                                + "ونحيطكم علماً بأنه تم استلام طلبكم، <br />"
                    //                                + "وسيتم إبلاغكم في حال قبول طلبكم، <br />"
                    //                                + "مع التحية، <br />"
                    //                                + " برنامج بناء القدرات.  <br />";


                    mailHelper.Body = "السادة/ المنظمات غير الربحية                     الموقرين <br />"
                + "السلام عليكم ورحمة الله وبركاته،، <br />"
                + "تهديكم مؤسسة الملك خالد أطيب التحيات، ونشكركم على تعبئة استبانة الوضع الحالي لمنظمتكم، <br />"
                + "ونفيدكم بأنه تم حفظ بياناتكم في قاعدة بيانات برنامج بناء القدرات كخطوة أولى لتصلكم بعدها إعلانات ومستجدات البرنامج من خلال البوابة مستقبلاً. <br />"
                + "<br />"
                + "وتفضلوا بقبول فائق التحية والتقدير،، <br />"
                + "فريق برنامج بناء القدرات <br />"
                + "مؤسسة الملك خالد <br />";


                    mailHelper.Send("");
                }
                return RedirectToAction("dashboard");
            }
            catch (Exception)
            {
                var frontEnd = Guid.Parse(User.Identity.GetUserId());
                var img = Convert.ToBase64String(repository.GetQuery<CorporateApplicationForm>(f => f.FrontendUserID == frontEnd).FirstOrDefault().Picture);
                ViewBag.imgSrc = string.Format("data:image/gif;base64,{0}", img);
                return View(model);
            }
        }
        //public FileResult DownloadCommitmentFile(Guid id)
        //{
        //    var attachment = repository.GetByKey<IncubationWorkshopAttachment>(id);
        //    string filePath = Server.MapPath(attachment.URL); // مسار نسبي

        //    if (!System.IO.File.Exists(filePath))
        //        throw new FileNotFoundException("ملف إقرار الالتزام غير موجود", filePath);

        //    byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
        //    return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, attachment.Name);
        //}
        ////26-3-2025
        //[HttpGet]
        //public ActionResult PreviewCommitmentFile(Guid id)
        //{
        //    //var attachment = repository.GetByKey<IncubationWorkshopAttachment>(id);
        //    //if (attachment == null || !System.IO.File.Exists(attachment.URL))
        //    //{
        //    //    return HttpNotFound("الملف غير موجود.");
        //    //}

        //    //var fileBytes = System.IO.File.ReadAllBytes(attachment.URL);
        //    //var contentType = GetContentType(attachment.URL);
        //    //return File(fileBytes, contentType);

        //        var attachment = repository.GetByKey<IncubationWorkshopAttachment>(id);
        //        string filePath = Server.MapPath(attachment.URL); // مسار نسبي

        //        if (!System.IO.File.Exists(filePath))
        //            throw new FileNotFoundException("ملف إقرار الالتزام غير موجود", filePath);

        //        byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);

        //        // نحدد نوع الملف بناءً على الامتداد
        //        string contentType = GetContentType(filePath);

        //        return File(fileBytes, contentType);


        //}
        ////1-6-2025
        //private string GetContentType(string filePath)
        //{
        //    // الحصول على الامتداد من مسار الملف
        //    var extension = System.IO.Path.GetExtension(filePath).ToLowerInvariant();

        //    switch (extension)
        //    {
        //        case ".pdf":
        //            return "application/pdf";
        //        case ".jpg":
        //        case ".jpeg":
        //            return "image/jpeg";
        //        case ".png":
        //            return "image/png";
        //        case ".gif":
        //            return "image/gif";
        //        case ".txt":
        //            return "text/plain";
        //        case ".doc":
        //            return "application/msword";
        //        case ".docx":
        //            return "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
        //        case ".xls":
        //            return "application/vnd.ms-excel";
        //        case ".xlsx":
        //            return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //        case ".ppt":
        //            return "application/vnd.ms-powerpoint";
        //        case ".pptx":
        //            return "application/vnd.openxmlformats-officedocument.presentationml.presentation";

        //        default:
        //            return "application/octet-stream"; // نوع افتراضي للملفات غير المعروفة
        //    }
        //}
        [HttpGet]
        public FileResult PreviewCommitmentFile1(Guid id)
        {
            var attachment = repository.GetByKey<IncubationWorkshopAttachment>(id);
            if (attachment == null || !System.IO.File.Exists(attachment.URL))
                throw new FileNotFoundException("الملف غير موجود");

            byte[] fileBytes = System.IO.File.ReadAllBytes(attachment.URL);
            string contentType = GetContentType(attachment.URL);

            // لاحظ Content-Disposition: inline
            Response.AppendHeader("Content-Disposition", $"inline; filename={attachment.Name}");

            return File(fileBytes, contentType);
        }

        [HttpGet]
        public ActionResult PreviewCommitmentFile(Guid id)
        {
            var attachment = repository.GetByKey<IncubationWorkshopAttachment>(id);
            if (attachment == null || !System.IO.File.Exists(attachment.URL))
                return HttpNotFound("الملف غير موجود");

            byte[] fileBytes = System.IO.File.ReadAllBytes(attachment.URL);
            string contentType = GetContentType(attachment.URL);

            return File(fileBytes, contentType);
        }
          private string GetContentType(string filePath)
        {
            // الحصول على الامتداد من مسار الملف
            var extension = System.IO.Path.GetExtension(filePath).ToLowerInvariant();

            switch (extension)
            {
                case ".pdf":
                    return "application/pdf";
                case ".jpg":
                case ".jpeg":
                    return "image/jpeg";
                case ".png":
                    return "image/png";
                case ".gif":
                    return "image/gif";
                case ".txt":
                    return "text/plain";
                case ".doc":
                    return "application/msword";
                case ".docx":
                    return "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                case ".xls":
                    return "application/vnd.ms-excel";
                case ".xlsx":
                    return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                case ".ppt":
                    return "application/vnd.ms-powerpoint";
                case ".pptx":
                    return "application/vnd.openxmlformats-officedocument.presentationml.presentation";
                
                default:
                    return "application/octet-stream"; // نوع افتراضي للملفات غير المعروفة
            }
        }

        [HttpGet]
        public ActionResult IncubationWorkshopJoin(Guid id)
        {

            ViewBag.WorkshopID = id;
            try
            {
                // جلب المستخدم الحالي
                var CurrUser = repository.GetByKey<FrontendUser>(Guid.Parse(User.Identity.GetUserId()));
                ViewBag.WorkshopID = id;
                // التحقق من وجود معاملة قبل الوصول إليها
                var workshopTransaction = CurrUser.IncubationWSBLTransactionsValue
                    .FirstOrDefault(t => t.IncubationWorkshopBLTrans.IncubationWorkshopID == id);

                if (workshopTransaction == null || workshopTransaction.IncubationWorkshopBLTransValStatus.NameEN != "Accepted")
                {
                    return RedirectToAction("IncWSBaselineAppForm", "Home", new { id });
                }

                // تجهيز بيانات العرض
                var frontEnd = Guid.Parse(User.Identity.GetUserId());
                var corporateForm = repository.GetQuery<CorporateApplicationForm>(c => c.FrontendUserID == frontEnd)
                                               .FirstOrDefault();

                if (corporateForm != null)
                {
                    var img = Convert.ToBase64String(corporateForm.Picture);
                    ViewBag.imgSrc = $"data:image/gif;base64,{img}";
                }
              
                ViewBag.lang = CultureHelper.CurrentCulture;

                // جلب تفاصيل الورشة
                //var incubationWorkshop = repository.GetByKey<IncubationWorkshop>(id);
                //2-25-2025
                var incubationWorkshop = repository.GetQuery<IncubationWorkshop>(w =>
                 w.IncubationWorkshopID == id && !w.IsDeleted)
                .FirstOrDefault();

                if (incubationWorkshop != null)
                {
                    var commitmentFile = repository.GetQuery<IncubationWorkshopAttachment>()
                    .FirstOrDefault(a => a.IncubationWorkshopID == incubationWorkshop.IncubationWorkshopID && a.IsCommitmentFile == true);

                    ViewBag.CommitmentFileUrl = commitmentFile != null ? commitmentFile.URL : null;
                    ViewBag.CommitmentFileName = commitmentFile?.Name;
                    ViewBag.CommitmentFileID = commitmentFile?.AttachmentID;


                    var viewModel = new IncubationWorkshopJoinVM
                    {
                        incubationWorkshop = incubationWorkshop,
                        WorkshopProjectProposal = new WorkshopProjectProposal(),
                        LstEmployee_WS = new List<EmployeesAttendIncubationWorkShop>()
                    };

                    return View(viewModel);
                }

                // معالجة حالة الخطأ
                return RedirectToAction("dashboard", new { Msg = "Something Went Wrong." });
            }
            catch (Exception ex)
            {
                // سجل الخطأ وارجع إلى لوحة التحكم
                //System.Diagnostics.Debug.WriteLine(ex.Message);
                return RedirectToAction("dashboard", new { Msg = "An unexpected error occurred." });
            }
        }

        
        [HttpPost]
        public ActionResult IncubationWorkshopJoin(IncubationWorkshopJoinVM model)
        {
            ViewBag.lang = CultureHelper.CurrentCulture;
            
            var status = model.WorkshopProjectProposal.WorkshopPP_InvitationStatus == WorkshopPPInvitationStatus.attend;
            if (status && (model.LstEmployee_WS == null || model.LstEmployee_WS?.Count == 0))
            {
                model.incubationWorkshop = repository.GetByKey<IncubationWorkshop>(model.incubationWorkshop.IncubationWorkshopID);
                model.LstEmployee_WS = new List<EmployeesAttendIncubationWorkShop>();
                ViewBag.err = CultureHelper.CurrentCulture != 3 ? "You must have at least one attender" : "يجب أن يكون لديك حاضر واحد على الأقل";
                return View(model);
            }

            var userId = User.Identity.GetUserId();
            var frontEndId = Guid.Parse(userId);

            model.WorkshopProjectProposal.WorkshopProjectProposalID = Guid.NewGuid();
            model.WorkshopProjectProposal.FrontendUserID = frontEndId;
            model.WorkshopProjectProposal.IncubationWorkshopID = model.incubationWorkshop.IncubationWorkshopID;
            if (model.WorkshopProjectProposal.WorkshopPP_InvitationStatus == WorkshopPPInvitationStatus.attend)
            {
                model.WorkshopProjectProposal.WorkshopProjectProposalStatusID = repository.First<WorkshopProjectProposalStatu>(f => f.NameEN == "Pending").WorkshopProjectProposalStatusID;
            }
            else
            {
                model.WorkshopProjectProposal.WorkshopProjectProposalStatusID = repository.First<WorkshopProjectProposalStatu>(f => f.NameEN == "Absent").WorkshopProjectProposalStatusID;
            }

            if (model.LstEmployee_WS != null)
            {
                for (int i = 0; i < model.LstEmployee_WS.Count; i++)
                {
                    model.WorkshopProjectProposal.EmployeesAttendIncubationWorkShops.Add(new EmployeesAttendIncubationWorkShop()
                    {
                        ID = Guid.NewGuid(),
                        Email = model.LstEmployee_WS[i].Email,
                        Name = model.LstEmployee_WS[i].Name,
                        Gender = model.LstEmployee_WS[i].Gender,
                        Mobile = model.LstEmployee_WS[i].Mobile,
                        Position = model.LstEmployee_WS[i].Position,
                        PositionTasks = model.LstEmployee_WS[i].PositionTasks,
                        EducationalQualificationAndSpecialization = model.LstEmployee_WS[i].EducationalQualificationAndSpecialization,
                        WorkshopProjectProposalID = model.WorkshopProjectProposal.WorkshopProjectProposalID
                    });
                }
            }

            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    repository.Add(model.WorkshopProjectProposal);

                    if (model.files != null)
                    {
                        string FolderName = User.Identity.Name;
                        string path = Server.MapPath("~/Uploads/" + FolderName + "/");
                        if (!Directory.Exists(path))
                            Directory.CreateDirectory(path);

                        for (int i = 0; i < model.files.Count(); i++)
                        {
                            if (model.files[i] != null)
                            {
                                model.files[i].SaveAs(path + Path.GetFileName(model.files[i].FileName));
                                WorkshopProjectProposalAttachment _Attach = new WorkshopProjectProposalAttachment();
                                _Attach.AttachmentID = Guid.NewGuid();
                                _Attach.WorkshopProjectProposalID = model.WorkshopProjectProposal.WorkshopProjectProposalID;
                                _Attach.Name = Path.GetFileName(model.files[i].FileName);
                                _Attach.ScreenName = "Incubation workshop Invitation";
                                _Attach.Size = model.files[i].ContentLength.ToString();
                                _Attach.URL = path + Path.GetFileName(model.files[i].FileName);
                                _Attach.Type = model.files[i].ContentType;
                                repository.Add(_Attach);

                            }
                        }
                        ViewBag.Message = "File uploaded successfully.";
                    }

                    var UserMail = repository.GetByKey<AspNetUser>(userId)?.Email;
                    var WorkshopPrivateInvitationQuery = repository.GetQuery<WorkshopPrivateInvitation>(w => w.Email == UserMail && w.IncubationWorkshopID == model.incubationWorkshop.IncubationWorkshopID).FirstOrDefault();

                    if (WorkshopPrivateInvitationQuery == null && model.incubationWorkshop.ISPublic)
                    {
                        WorkshopPrivateInvitation _Private = new WorkshopPrivateInvitation()
                        {
                            WorkshopPrivateInvitationId = Guid.NewGuid(),
                            Email = UserMail,
                            FrontendUserID = frontEndId,
                            //2-25-2025
                            InvitationStatus = InvitationStatus.form_filled, //  تحديث الحالة عند تعبئة النموذج
                            //InvitationStatus = model.WorkshopProjectProposal.WorkshopPP_InvitationStatus == WorkshopPPInvitationStatus.attend ? InvitationStatus.pending : InvitationStatus.cancel,
                            IncubationWorkshopID = model.incubationWorkshop.IncubationWorkshopID
                        };
                        repository.Add(_Private);
                    }
                    //else if (WorkshopPrivateInvitationQuery != null && !WorkshopPrivateInvitationQuery.FrontendUserID.HasValue && !model.incubationWorkshop.ISPublic)
                    //{
                    //    WorkshopPrivateInvitationQuery.FrontendUserID = frontEndId;
                    //    WorkshopPrivateInvitationQuery.InvitationStatus = InvitationStatus.form_filled;
                    //    //2-25-2025
                    //    //WorkshopPrivateInvitationQuery.InvitationStatus = model.WorkshopProjectProposal.WorkshopPP_InvitationStatus == WorkshopPPInvitationStatus.attend ? InvitationStatus.pending : InvitationStatus.cancel;
                    //    repository.Update(WorkshopPrivateInvitationQuery);
                    //}
                    //2-25-20255
                    var invitation = repository.GetQuery<WorkshopPrivateInvitation>(w => w.Email == UserMail
                                     && w.IncubationWorkshopID == model.incubationWorkshop.IncubationWorkshopID)
                                     .FirstOrDefault();

                    if (invitation != null)
                    {
                        invitation.InvitationStatus = InvitationStatus.form_filled; //  
                        repository.Update(invitation);
                    }
                    repository.UnitOfWork.SaveChanges();

                    //
                    if (model.WorkshopProjectProposal.WorkshopPP_InvitationStatus == WorkshopPPInvitationStatus.attend)
                    {
                        MailHelper mailHelper = new MailHelper();
                        mailHelper.ToEmail = UserMail;
                        mailHelper.Subject = $"إفادة";
                        mailHelper.IsHtml = true;
                        mailHelper.Body = "المكرمين/ المنظمات غير الربحية                         الموقرين<br/>"
                                                    + "السلام عليكم ورحمة الله وبركاته، <br/>"
                                                    + "تهديكم مؤسسة الملك خالد أطيب التحيات، <br />"
                                                    + $"ونتقدم لكم بالشكر على تعبئة طلب التسجيل لورشة عمل {model.incubationWorkshop.Name}، <br/>"
                                                    + "وسيتم إفادتكم في حال قبول طلبكم. <br/>"
                                                    + "مع التحية، <br/>"
                                                    + " برنامج بناء القدرات. <br/>";

                        var fundingSource = repository.GetByKey<FundingSource>(model.incubationWorkshop.FundingSourceID);
                        if (fundingSource.UseCustomThemes)
                            mailHelper.Send($"?partner={fundingSource.Nickname}");
                        else
                            mailHelper.Send("");
                    }

                    repository.UnitOfWork.SaveChanges();
                    scope.Complete();
                }
                catch (Exception)
                {
                    scope.Dispose();
                    model.incubationWorkshop = repository.GetByKey<IncubationWorkshop>(model.incubationWorkshop.IncubationWorkshopID);
                    model.LstEmployee_WS = new List<EmployeesAttendIncubationWorkShop>();
                    return View(model);
                }
            }
            return RedirectToAction("dashboard");
        }

        [HttpGet]
        public ActionResult UpdateWorkshopProjectProposal(Guid id)
        {
            var frontEnd = Guid.Parse(User.Identity.GetUserId());
            var img = Convert.ToBase64String(repository.GetQuery<CorporateApplicationForm>(c => c.FrontendUserID == frontEnd).FirstOrDefault().Picture);
            ViewBag.imgSrc = string.Format("data:image/gif;base64,{0}", img);
            ViewBag.lang = CultureHelper.CurrentCulture;
            var _WsPP = repository.GetByKey<WorkshopProjectProposal>(id);
            if (_WsPP != null)
            {
                IncubationWorkshopJoinVM IWJVM = new IncubationWorkshopJoinVM()
                {
                    incubationWorkshop = _WsPP.IncubationWorkshop,
                    WorkshopProjectProposal = _WsPP,
                    LstEmployee_WS = new List<EmployeesAttendIncubationWorkShop>()
                };
                ViewBag.Msg = _WsPP.Feedback;
                return View(IWJVM);
            }
            return View();
        }

        [HttpPost]
        public ActionResult UpdateWorkshopProjectProposal(IncubationWorkshopJoinVM model)
        {
            if (ModelState.IsValid)
            {
                ViewBag.lang = CultureHelper.CurrentCulture;
                var status = model.WorkshopProjectProposal.WorkshopPP_InvitationStatus == WorkshopPPInvitationStatus.attend;
                if (status && (model.LstEmployee_WS == null || model.LstEmployee_WS?.Count == 0))
                {
                    model.incubationWorkshop = repository.GetByKey<IncubationWorkshop>(model.incubationWorkshop.IncubationWorkshopID);
                    model.LstEmployee_WS = new List<EmployeesAttendIncubationWorkShop>();
                    ViewBag.err = CultureHelper.CurrentCulture != 3 ? "You must have at least one attender" : "يجب أن يكون لديك حاضر واحد على الأقل";
                    return View(model);
                }

                var entity = repository.GetByKey<WorkshopProjectProposal>(model.WorkshopProjectProposal.WorkshopProjectProposalID);
                entity.WorkshopPP_InvitationStatus = model.WorkshopProjectProposal.WorkshopPP_InvitationStatus;
                if (model.WorkshopProjectProposal.WorkshopPP_InvitationStatus == WorkshopPPInvitationStatus.attend)
                {
                    entity.WorkshopProjectProposalStatusID = repository.GetQuery<WorkshopProjectProposalStatu>(f => f.NameEN == "Project Proposal Updated").FirstOrDefault().WorkshopProjectProposalStatusID;
                }
                else
                {
                    model.WorkshopProjectProposal.WorkshopProjectProposalStatusID = repository.First<WorkshopProjectProposalStatu>(f => f.NameEN == "Absent").WorkshopProjectProposalStatusID;
                }
                repository.Update(entity);

                if (model.LstEmployee_WS != null)
                {
                    repository.Delete<EmployeesAttendIncubationWorkShop>(d => d.WorkshopProjectProposalID == entity.WorkshopProjectProposalID);
                    for (int i = 0; i < model.LstEmployee_WS.Count; i++)
                    {
                        var AttendIncubWs = new EmployeesAttendIncubationWorkShop()
                        {
                            ID = Guid.NewGuid(),
                            Email = model.LstEmployee_WS[i].Email,
                            Name = model.LstEmployee_WS[i].Name,
                            Gender = model.LstEmployee_WS[i].Gender,
                            Mobile = model.LstEmployee_WS[i].Mobile,
                            Position = model.LstEmployee_WS[i].Position,
                            PositionTasks = model.LstEmployee_WS[i].PositionTasks,
                            EducationalQualificationAndSpecialization = model.LstEmployee_WS[i].EducationalQualificationAndSpecialization,
                            WorkshopProjectProposalID = entity.WorkshopProjectProposalID
                        };
                        repository.Add(AttendIncubWs);
                    }
                }

                using (TransactionScope scope = new TransactionScope())
                {
                    try
                    {
                        if (model.files != null)
                        {
                            string FolderName = User.Identity.Name;
                            string path = Server.MapPath("~/Uploads/" + FolderName + "/");
                            if (!Directory.Exists(path))
                                Directory.CreateDirectory(path);

                            for (int i = 0; i < model.files.Count(); i++)
                            {
                                if (model.files[i] != null)
                                {
                                    model.files[i].SaveAs(path + Path.GetFileName(model.files[i].FileName));
                                    WorkshopProjectProposalAttachment _Attach = new WorkshopProjectProposalAttachment();
                                    _Attach.AttachmentID = Guid.NewGuid();
                                    _Attach.WorkshopProjectProposalID = model.WorkshopProjectProposal.WorkshopProjectProposalID;
                                    _Attach.Name = Path.GetFileName(model.files[i].FileName);
                                    _Attach.ScreenName = "Incubation workshop Invitation";
                                    _Attach.Size = model.files[i].ContentLength.ToString();
                                    _Attach.URL = path + Path.GetFileName(model.files[i].FileName);
                                    _Attach.Type = model.files[i].ContentType;
                                    repository.Add(_Attach);
                                }
                            }
                            ViewBag.Message = "File uploaded successfully.";
                        }

                        var _userId = User.Identity.GetUserId();
                        var FrontEndMail = UserManager.GetEmail(_userId);

                        if (FrontEndMail != null)
                        {
                            MailHelper mailHelper = new MailHelper();
                            mailHelper.ToEmail = FrontEndMail;
                            mailHelper.Subject = $"إفادة";
                            mailHelper.IsHtml = true;
                            mailHelper.Body = "المكرمين/ المنظمات غير الربحية                         الموقرين<br/>"
                                                    + "السلام عليكم ورحمة الله وبركاته، <br/>"
                                                    + "تهديكم مؤسسة الملك خالد أطيب التحيات، <br />"
                                                    + $"ونتقدم لكم بالشكر على تعبئة طلب التسجيل لورشة عمل {model.incubationWorkshop.Name}، <br/>"
                                                    + "وسيتم إفادتكم في حال قبول طلبكم. <br/>"
                                                    + "مع التحية، <br/>"
                                                    + " برنامج بناء القدرات. <br/>";

                            var fundingSource = repository.GetByKey<FundingSource>(model.incubationWorkshop.FundingSourceID);
                            if (fundingSource.UseCustomThemes)
                                mailHelper.Send($"?partner={fundingSource.Nickname}");
                            else
                                mailHelper.Send("");
                        }
                        repository.UnitOfWork.SaveChanges();
                        scope.Complete();
                    }
                    catch (Exception)
                    {
                        scope.Dispose();
                        model.incubationWorkshop = repository.GetByKey<IncubationWorkshop>(model.incubationWorkshop.IncubationWorkshopID);
                        model.LstEmployee_WS = new List<EmployeesAttendIncubationWorkShop>();
                        return View(model);
                    }
                }
                return RedirectToAction("dashboard");
            }
            model.incubationWorkshop = repository.GetByKey<IncubationWorkshop>(model.incubationWorkshop.IncubationWorkshopID);
            model.LstEmployee_WS = new List<EmployeesAttendIncubationWorkShop>();
            return View(model);
        }

        [HttpGet]
        public ActionResult AppendLst_WS()
        {
            var employee_WS = new EmployeesAttendIncubationWorkShop();
            return PartialView("_LstEmployee_WS", employee_WS);
        }

        public FileResult DownloadIncubationAdvertisingAttachments(Guid id)
        {
            var attachmet = repository.GetByKey<IncubationAdvertisingAttachment>(id);
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

        public FileResult Download(Guid id)
        {
            var attachmet = repository.GetByKey<CorporateApplicationFormAttachment>(id);
            byte[] fileBytes = System.IO.File.ReadAllBytes(attachmet.URL);
            string fileName = attachmet.Name;
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        public FileResult DownloadIncubationWorkshopAttachments(Guid id)
        {
            var attachmet = repository.GetByKey<IncubationWorkshopAttachment>(id);
            byte[] fileBytes = System.IO.File.ReadAllBytes(attachmet.URL);
            string fileName = attachmet.Name;
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        //DownloadFollowUpProjectPlanAttachments
        public FileResult DownloadFollowUpProjectPlanAttachments(Guid id)
        {
            var attachmet = repository.GetByKey<FollowUpProjectPlanAttachment>(id);
            byte[] fileBytes = System.IO.File.ReadAllBytes(attachmet.URL);
            string fileName = attachmet.Name;
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        [HttpGet]
        public FileResult DownloadIncWSBLAppAttachment(Guid id)
        {
            try
            {
                var attachmet = repository.GetByKey<IncubationWorkshopBLTransValueAttachment>(id);
                byte[] fileBytes = System.IO.File.ReadAllBytes(attachmet.URL);
                string fileName = attachmet.Name;
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }
            catch (Exception ex)
            {
                return File(Encoding.UTF8.GetBytes(ex.Message), "text/plain", "error.txt");
            }
        }

        [HttpGet]
        public JsonResult DeleteIncWSBLAppAttachment(Guid id)
        {
            try
            {
                var attachment = repository.GetByKey<IncubationWorkshopBLTransValueAttachment>(id);
                var transValue = attachment.IncubationWSBLTransValue;
                repository.Delete(attachment);
                transValue.Value = transValue.IncubationWSBLTransValueAttachment.Count > 0 ? $"{transValue.IncubationWSBLTransValueAttachment.Count} Attached File" : string.Empty;
                repository.UnitOfWork.SaveChanges();

                if (System.IO.File.Exists(attachment.URL))
                {
                    System.IO.File.Delete(attachment.URL);
                }

                return Json("Deleted Succsesfully", JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json("Deletion Failed", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult DeleteAttachment(Guid Id)
        {
            try
            {
                var Attachment = repository.GetByKey<FollowUpProjectPlanAttachment>(Id);
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

        //DownloadWorkshopProjectProposalAttachments
        public FileResult DownloadWorkshopProjectProposalAttachments(Guid id)
        {
            var attachmet = repository.GetByKey<WorkshopProjectProposalAttachment>(id);
            byte[] fileBytes = System.IO.File.ReadAllBytes(attachmet.URL);
            string fileName = attachmet.Name;
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        //DeleteWorkshopProjectProposalAttachments
        [HttpGet]
        public JsonResult DeleteWorkshopProjectProposalAttachments(Guid Id)
        {
            try
            {
                var Attachment = repository.GetByKey<WorkshopProjectProposalAttachment>(Id);
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

        [HttpGet]
        public JsonResult DeleteFollowUpPlanRequestAttachment(Guid Id)
        {
            try
            {
                var Attachment = repository.GetByKey<FollowUpProjectPlanRequestAttachment>(Id);
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

        [HttpGet]
        public ActionResult CreatWorkshopRating(Guid IncubationWorkshopID)
        {
            var frontEnd = Guid.Parse(User.Identity.GetUserId());
            var img = Convert.ToBase64String(repository.GetQuery<CorporateApplicationForm>(c => c.FrontendUserID == frontEnd).FirstOrDefault().Picture);
            ViewBag.imgSrc = string.Format("data:image/gif;base64,{0}", img);
            ViewBag.lang = CultureHelper.CurrentCulture;
            IncubationWorkshopRatingVM Model = new IncubationWorkshopRatingVM();
            Model.IncubationWorkshopID = IncubationWorkshopID;
            Model.IncubationWorkshop = repository.GetByKey<IncubationWorkshop>(IncubationWorkshopID);
            return View(Model);
        }

        [HttpPost]
        public ActionResult CreatWorkshopRating(IncubationWorkshopRatingVM model)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var frontenduser = Guid.Parse(userId);

                repository.Add(new IncubationWorkshopRating()
                {
                    IncubationWorkshopRatingId = Guid.NewGuid(),
                    IncubationWorkshopID = model.IncubationWorkshopID,
                    FrontendUserId = frontenduser,
                    Hosting = model.Hosting,
                    EvaluationDate = DateTime.Now,
                    AbilityToAchieveAGoal = model.AbilityToAchieveAGoal,
                    AbilityToManageDiscussionAndHandleQuestions = model.AbilityToManageDiscussionAndHandleQuestions,
                    AchievingGoal = model.AchievingGoal,
                    BodyLanguage = model.BodyLanguage,
                    ClarityOfVoiceAndTone = model.ClarityOfVoiceAndTone,
                    CommentsOnTrainer = model.CommentsOnTrainer,
                    LinkingTheTrainingMaterialToReality = model.LinkingTheTrainingMaterialToReality,
                    MasteryOfTrainingMaterial = model.MasteryOfTrainingMaterial,
                    MeetTheWorkRequirement = model.MeetTheWorkRequirement,
                    PartcipationReaction = model.PartcipationReaction,
                    Power = model.Power,
                    Weakness = model.Weakness,
                    ReasonForAchievingGoals = model.ReasonForAchievingGoals,
                    ReasonForHosting = model.ReasonForHosting,
                    ReasonForMeetTheWorkRequirement = model.ReasonForMeetTheWorkRequirement,
                    ReasonForPartcipationReaction = model.ReasonForPartcipationReaction,
                    ReasonForTrainingMaterial = model.ReasonForTrainingMaterial,
                    ReasonForTrainingWorkshop = model.ReasonForTrainingWorkshop,
                    ReasonForWorkshopClass = model.ReasonForWorkshopClass,
                    TheAbilityToDeliverInformation = model.TheAbilityToDeliverInformation,
                    TrainingMaterial = model.TrainingMaterial,
                    TrainingWorkshop = model.TrainingWorkshop,
                    WorkshopClass = model.WorkshopClass
                });

                repository.UnitOfWork.SaveChanges();
            }
            catch (Exception)
            {
                var frontEnd = Guid.Parse(User.Identity.GetUserId());
                var img = Convert.ToBase64String(repository.GetQuery<CorporateApplicationForm>(c => c.FrontendUserID == frontEnd).FirstOrDefault().Picture);
                ViewBag.imgSrc = string.Format("data:image/gif;base64,{0}", img);
                ViewBag.lang = CultureHelper.CurrentCulture;
                model.IncubationWorkshop = repository.GetByKey<IncubationWorkshop>(model.IncubationWorkshopID);
                return View(model);
            }

            return RedirectToAction("dashboard");
        }

        [HttpGet]
        public ActionResult DownloadIncubBLAttachment(string URL)
        {
            try
            {
                byte[] fileBytes = System.IO.File.ReadAllBytes(URL);
                string fileName = URL.Substring(URL.LastIndexOf('\\') + 1);
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }
            catch (Exception)
            {
                return Content("Something Went Wrong");
            }
        }

        [HttpGet]
        public JsonResult DeleteIncubBLAttachment(Guid Id, string attach, string URL)
        {
            try
            {
                var IncubBL = repository.GetByKey<IncubationBaseline>(Id);
                switch (attach)
                {
                    case "StrategicPlan":
                        IncubBL.StrategicPlan = "";
                        break;
                    case "OperatingPlan":
                        IncubBL.OperatingPlan = "";
                        break;
                    case "GovernanceGuide":
                        IncubBL.GovernanceGuide = "";
                        break;
                    case "CharteredAccountantNotes":
                        IncubBL.CharteredAccountantNotes = "";
                        break;
                    case "AttachFollowUpandEvaluationForms":
                        IncubBL.AttachFollowUpandEvaluationForms = "";
                        break;
                }
                repository.Update(IncubBL);
                repository.UnitOfWork.SaveChanges();
                if (System.IO.File.Exists(URL))
                {
                    System.IO.File.Delete(URL);
                }
                return Json("Deleted Succsesfully", JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json("Deletion Failed", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult FollowUpProjectPlanJoin(Guid Id)
        {
            var frontEnd = Guid.Parse(User.Identity.GetUserId());
            var img = Convert.ToBase64String(repository.GetQuery<CorporateApplicationForm>(c => c.FrontendUserID == frontEnd).FirstOrDefault().Picture);
            ViewBag.imgSrc = string.Format("data:image/gif;base64,{0}", img);
            ViewBag.lang = CultureHelper.CurrentCulture;
            var Entity = repository.GetByKey<FollowUpProjectPlanRequest>(Id);
            ViewBag.Msg = Entity.feedBack;
            return View(new FollowUpProjectPlanJoinVM() { FollowUpProjectPlan = Entity.FollowUpProjectPlan, FollowUpProjectPlanRequest = Entity });
        }

        [HttpPost]
        public ActionResult FollowUpProjectPlanJoin(FollowUpProjectPlanJoinVM model)
        {
            try
            {
                var Entity = repository.GetByKey<FollowUpProjectPlanRequest>(model.FollowUpProjectPlanRequest.FollowUpProjectPlanRequestId);
                Entity.Notes = model.FollowUpProjectPlanRequest.Notes;
                Entity.FollowUpProjectPlanStatus = FollowUpProjectPlanStatus.ProjectPlanUpdated;
                repository.Update(Entity);

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
                            FollowUpProjectPlanRequestAttachment _Attach = new FollowUpProjectPlanRequestAttachment();
                            _Attach.AttachmentID = Guid.NewGuid();
                            _Attach.FollowUpProjectPlanRequestId = Entity.FollowUpProjectPlanRequestId;
                            _Attach.Name = Path.GetFileName(model.files[i].FileName);
                            _Attach.ScreenName = "Followup Project Plan Request Fornt End";
                            _Attach.Size = model.files[i].ContentLength.ToString();
                            _Attach.URL = path + Path.GetFileName(model.files[i].FileName);
                            _Attach.Type = model.files[i].ContentType;
                            repository.Add(_Attach);
                        }
                    }
                }
                repository.UnitOfWork.SaveChanges();
                var Email = UserManager.GetEmail(User.Identity.GetUserId());
                if (Email != null)
                {
                    MailHelper mailHelper = new MailHelper();
                    mailHelper.ToEmail = Email;
                    mailHelper.IsHtml = true;
                    mailHelper.Subject = "إفادة";
                    mailHelper.Body = "المكرمين/ المنظمات غير الربحية                         الموقرين<br/>"
                                    + "السلام عليكم ورحمة الله وبركاته، <br/>"
                                    + "تهديكم مؤسسة الملك خالد أطيب التحيات، <br />"
                                    + $"ونفيدكم بإستلام مخرجات ورشة عمل: {Entity.FollowUpProjectPlan?.IncubationWorkshop?.Name}، وسيتم افادتكم حال اعتمادها. <br/>"
                                    + "شاكرين ومقدرين تعاونكم، <br/>"
                                    + " برنامج بناء القدرات. <br/>";

                    var fundingSource = repository.GetByKey<FundingSource>(model.FollowUpProjectPlan.IncubationWorkshop.FundingSourceID);
                    if (fundingSource.UseCustomThemes)
                        mailHelper.Send($"?partner={fundingSource.Nickname}");
                    else
                        mailHelper.Send("");
                }
                return RedirectToAction("dashboard");
            }
            catch (Exception)
            {
                var frontEnd = Guid.Parse(User.Identity.GetUserId());
                var img = Convert.ToBase64String(repository.GetQuery<CorporateApplicationForm>(c => c.FrontendUserID == frontEnd).FirstOrDefault().Picture);
                ViewBag.imgSrc = string.Format("data:image/gif;base64,{0}", img);
                ViewBag.lang = CultureHelper.CurrentCulture;
                model.FollowUpProjectPlan = repository.GetByKey<FollowUpProjectPlan>(model.FollowUpProjectPlan.FollowUpProjectPlanId);
                return View(model);
            }
        }
    }
}