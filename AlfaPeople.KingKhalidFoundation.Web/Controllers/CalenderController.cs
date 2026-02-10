using System;
using System.Linq;
using System.Web.Mvc;
using AlphaPeople.Core;
using System.Configuration;
using AlphaPeople.Repository;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using AlfaPeople.KingKhalidFoundation.Data;
using AlfaPeople.KingKhalidFoundation.Data.Model;

namespace AlfaPeople.KingKhalidFoundation.Web.Controllers
{
    [Authorize(Roles = "Corporation")]
    public class CalenderController : BaseController
    {
        private readonly IRepository repository;
        private readonly CommonHelper helper;
        private readonly string Logpath = ConfigurationManager.AppSettings["Log"].ToString();
        public CalenderController()
        {
            helper = new CommonHelper();
            repository = new Repository(new KingkhaledFoundationDB());
        }

        // GET: Calender
        public ActionResult Index()
        {
            var UserId = User.Identity.GetUserId();
            var currentuser = repository.GetByKey<AspNetUser>(UserId);
            Guid frontid = Guid.Parse(UserId);

            if (currentuser.AspNetRoles.Any(r => r.Name == "Corporation"))
            {
                var img = Convert.ToBase64String(repository.GetQuery<CorporateApplicationForm>(f => f.FrontendUserID == frontid).FirstOrDefault().Picture);
                ViewBag.imgSrc = String.Format("data:image/gif;base64,{0}", img);
                if (repository.GetQuery<FrontendUser>(f => f.FK_AspUser == UserId && f.CorporateApplicationForms.Any(g => g.CorporateApplicationStatu.ApplicantStatu.ApplicantStatusName == "Accepted")).ToList().Count != 0)
                {
                    return View();
                }
                return RedirectToAction("CorporationProfile", "Home", new { Msg = App_GlobalResources.General.MsgApply });
            }
            else if (currentuser.AspNetRoles.Any(r => r.Name == "IndIvidual"))
            {
                var img = Convert.ToBase64String(repository.GetQuery<IndividualApplicationForm>(f => f.FrontendUserID == frontid).FirstOrDefault().Picture);
                ViewBag.imgSrc = String.Format("data:image/gif;base64,{0}", img);
                if (repository.GetQuery<FrontendUser>(f => f.FK_AspUser == UserId && f.IndividualApplicationForms.Any(g => g.IndividualApplicantStatu.ApplicantStatu.ApplicantStatusName == "Accepted")).ToList().Count != 0)
                {
                    return View();
                }
                return RedirectToAction("IndIvidualProfile", "Home");
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        public JsonResult GetActiveEvents()
        {
            var UserId = User.Identity.GetUserId();
            Guid frontid = Guid.Parse(UserId);
            var resIncubation = repository.GetQuery<IncubationAdvertising>(f => f.IsActive == true && !f.IsDeleted && f.IncubationType.NameEN == "Incubation" && f.IncubationProjectProposals.Any(r => r.IncubationProjectProposalStatu.NameEN == "Accepted" && r.FrontendUserID == frontid)).Select(f => new { title = f.Name, start = f.StartDate, end = f.EndDate, EventID = f.IncubationAdID.ToString(), Type = "Incubation" }).ToList();
            List<Event> lstevents = new List<Controllers.CalenderController.Event>();
            for (int i = 0; i < resIncubation.Count; i++)
            {
                lstevents.Add(new Event { title = resIncubation[i].title, start = string.Format("{0:yyyy-MM-dd}", resIncubation[i].start), EventID = resIncubation[i].EventID + "/" + resIncubation[i].Type, end = string.Format("{0:yyyy-MM-dd}", resIncubation[i].end.AddDays(1)), className = "event-green", allDay = true });
            }

            var Lstev = repository.GetQuery<Events>(f => f.IsActive == true).ToList();
            for (int i = 0; i < Lstev.Count; i++)
            {
                lstevents.Add(new Event { title = Lstev[i].EventNameEN, EventID = Lstev[i].EventID.ToString() + "/Events", start = string.Format("{0:yyyy-MM-dd}", Lstev[i].StartDate), end = string.Format("{0:yyyy-MM-dd}", Lstev[i].EndDate.AddDays(1)), className = "event-orange", allDay = true });
            }
            var resAcc = repository.GetQuery<IncubationAdvertising>(f => f.IsActive == true && !f.IsDeleted && f.IncubationType.NameEN == "Acceleration" && f.IncubationProjectProposals.Any(r => r.IncubationProjectProposalStatu.NameEN == "Accepted" && r.FrontendUserID == frontid)).Select(f => new { title = f.Name, start = f.StartDate, end = f.EndDate, EventID = f.IncubationAdID.ToString(), Type = "Incubation" }).ToList();
            for (int i = 0; i < resAcc.Count; i++)
            {
                lstevents.Add(new Event { title = resAcc[i].title, start = string.Format("{0:yyyy-MM-dd}", resAcc[i].start), EventID = resAcc[i].EventID, end = string.Format("{0:yyyy-MM-dd}", resAcc[i].end.AddDays(1)), className = "event-azure", allDay = true });
            }
            var resWorkshop = repository.GetQuery<IncubationWorkshop>(f => f.IncubationtWorkshopStatu.NameEN == "Active" && !f.IsDeleted && f.WorkshopProjectProposals.Any(r => r.WorkshopProjectProposalStatu.NameEN == "Accepted" && r.FrontendUserID == frontid)).Select(f => new { title = f.Name, start = f.StartDate, end = f.EndDate, EventID = f.IncubationWorkshopID.ToString(), Type = "IncubationWorkshop" }).ToList();
            for (int i = 0; i < resWorkshop.Count; i++)
            {
                lstevents.Add(new Event { title = resWorkshop[i].title, start = string.Format("{0:yyyy-MM-dd}", resWorkshop[i].start), end = string.Format("{0:yyyy-MM-dd}", resWorkshop[i].end.AddDays(1)), EventID = resWorkshop[i].EventID + "/" + resWorkshop[i].Type, className = "event-default", allDay = true });
            }

            return Json(lstevents, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetDetails(string Id)
        {
            string[] Lst = Id.Split('/');
            if (Lst[1] == "Incubation")
            {
                Guid GId = Guid.Parse(Lst[0]);
                var res = repository.GetQuery<IncubationAdvertising>(f => f.IncubationAdID == GId).Select(g => new { Name = g.Name, StartDate = g.StartDate, EndDate = g.EndDate, Description = g.AdvertisingDetails }).FirstOrDefault();
                return Json(res, JsonRequestBehavior.AllowGet);
            }
            else if (Lst[1] == "IncubationWorkshop")
            {
                Guid GId = Guid.Parse(Lst[0]);
                var res = repository.GetQuery<IncubationWorkshop>(f => f.IncubationWorkshopID == GId).Select(g => new { Name = g.Name, StartDate = g.StartDate, EndDate = g.EndDate, Description = g.Description }).FirstOrDefault();
                return Json(res, JsonRequestBehavior.AllowGet);
            }
            else if (Lst[1] == "Events")
            {
                Guid GId = Guid.Parse(Lst[0]);
                var eve = repository.GetQuery<Events>(f => f.EventID == GId).Select(g => new { Name = g.EventNameEN, StartDate = g.StartDate, EndDate = g.EndDate, Description = g.Description }).FirstOrDefault();
                return Json(eve, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        class Event
        {
            public string EventID { get; set; }
            public string title { get; set; }
            public string start { get; set; }
            public string end { get; set; }
            public string className { get; set; }
            public bool allDay { get; set; }
        }
    }
}