using System;
using System.Linq;
using System.Web.Mvc;
using AlphaPeople.Core;
using System.Configuration;
using AlphaPeople.Repository;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using AlfaPeople.KingKhalidFoundation.Data;
using AlfaPeople.KingKhalidFoundation.Web.Helper;
using AlfaPeople.KingKhalidFoundation.Data.Model;
using AlfaPeople.KingKhalidFoundation.Web.Models;
using System.Data.Entity;

namespace AlfaPeople.KingKhalidFoundation.Web.Controllers
{
    [Authorize(Roles = "Corporation")]
    public class RequestController : BaseController
    {
        private readonly IRepository repository;
        private readonly CommonHelper helper;
        private string Logpath = ConfigurationManager.AppSettings["Log"].ToString();
        public RequestController()
        {
            repository = new Repository(new KingkhaledFoundationDB());
            helper = new CommonHelper();
        }

        // GET: Request
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var currentUser = repository.GetByKey<AspNetUser>(userId);
            
            
            
            Guid frontid = Guid.Parse(userId);
            ViewBag.lang = CultureHelper.CurrentCulture;

            if (currentUser.AspNetRoles.Any(r => r.Name == "Corporation"))
            {
                if (repository.GetQuery<FrontendUser>(f => f.FK_AspUser == userId && f.CorporateApplicationForms.Any(g => g.CorporateApplicationStatu.ApplicantStatu.ApplicantStatusName == "Accepted")).ToList().Count != 0)
                {
                    //2-20-2025
                    var cancelledInvitations = repository.GetQuery<IncubationPrivateInvitation>()
                   .Include(i => i.IncubationAdvertising)
                   .Include(i => i.IncubationAdvertising.IncubationType)
                   .Where(i => i.Email == currentUser.Email && i.Status == InvitationStatus.cancel&&!i.IncubationAdvertising.IsDeleted)
                   .ToList();

                    //2-25-2025

                    //  جلب الدعوات الملغاة لورش العمل
                    var cancelledWorkshops = repository.GetQuery<WorkshopPrivateInvitation>()
                        .Include(w => w.IncubationWorkshop)
                        .Where(w => w.Email == currentUser.Email && w.InvitationStatus == InvitationStatus.cancel && !w.IncubationWorkshop.IsDeleted)
                        .ToList();


                   
                    var CorForm = repository.GetQuery<CorporateApplicationForm>(f => f.FrontendUserID == frontid).FirstOrDefault();
                    ViewBag.ProgramName = CorForm.Program.ProgramName;

                    var img = Convert.ToBase64String(repository.GetQuery<CorporateApplicationForm>(f => f.FrontendUserID == frontid).FirstOrDefault().Picture);
                    ViewBag.imgSrc = string.Format("data:image/gif;base64,{0}", img);

                    RequestsVM lst = new RequestsVM();
                    //2-25-2025
                    lst.LstCancelledWorkshops = cancelledWorkshops;
                    //2-20-2025
                    lst.LstCancelledInvitations = cancelledInvitations;
                    //
                    var resIncubation = repository.GetQuery<IncubationAdvertising>(f => f.IsActive == true && !f.IsDeleted && f.IncubationProjectProposals.Any(r => r.FrontendUserID == frontid)).ToList();
                    var resWorkshop = repository.GetQuery<IncubationWorkshop>(f => f.IncubationtWorkshopStatu.NameEN == "Active" && !f.IsDeleted && f.WorkshopProjectProposals.Any(r => r.FrontendUserID == frontid)).ToList();
                    lst.LstIncubation = new List<IncubationAdvertising>();
                    lst.LstIWorkshop = new List<IncubationWorkshop>();
                    lst.LstIncubation = resIncubation;
                    lst.LstIWorkshop = resWorkshop;
                    return View(lst);
                }
                return RedirectToAction("CorporationProfile", "Home", new { Msg = App_GlobalResources.General.MsgApply });
            }
            else
            {
                return View();
            }
        }
    }
}