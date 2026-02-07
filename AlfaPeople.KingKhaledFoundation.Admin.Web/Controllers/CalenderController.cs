using System;
using System.Linq;
using System.Web.Mvc;
using AlphaPeople.Core;
using System.Configuration;
using AlphaPeople.Repository;
using System.Collections.Generic;
using AlfaPeople.KingKhalidFoundation.Data;
using AlfaPeople.KingKhalidFoundation.Data.Model;

namespace AlfaPeople.KingKhaledFoundation.Admin.Web.Controllers
{
    [Authorize(Roles = "Admin, CB Manager, CB Supervisor, CB Analyst")]
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
            return View();
        }

        [HttpGet]
        public JsonResult GetActiveEvents()
        {
            var resIncubation = repository.GetQuery<Incubation>(f => f.IncubationStatus.NameEN == "Active"&&!f.IncubationAdvertising.IsDeleted && f.IncubationType.NameEN == "Incubation").Select(f => new { title = f.Name, start = f.StartDate, end = f.EndDate, EventID = f.IncubationID.ToString(), Type = "Incubation" }).ToList();
            List<Event> lstevents = new List<Event>();
            for (int i = 0; i < resIncubation.Count; i++)
            {
                lstevents.Add(new Event { title = resIncubation[i].title, start = string.Format("{0:yyyy-MM-dd}", resIncubation[i].start), end = string.Format("{0:yyyy-MM-dd}", resIncubation[i].end.AddDays(1)), className = "event-green", allDay = true, EventID = resIncubation[i].EventID + "/" + resIncubation[i].Type });
            }

            var Lstev = repository.GetQuery<Events>(f => f.IsActive == true).ToList();
            for (int i = 0; i < Lstev.Count; i++)
            {
                lstevents.Add(new Event { title = Lstev[i].EventNameEN, start = string.Format("{0:yyyy-MM-dd}", Lstev[i].StartDate), end = string.Format("{0:yyyy-MM-dd}", Lstev[i].EndDate.AddDays(1)), className = "event-orange", allDay = true });
            }
            var resAcc = repository.GetQuery<Incubation>(f => f.IncubationStatus.NameEN == "Active" && f.IncubationType.NameEN == "Acceleration").Select(f => new { title = f.Name, start = f.StartDate, end = f.EndDate, EventID = f.IncubationID.ToString(), Type = "Incubation" }).ToList();
            for (int i = 0; i < resAcc.Count; i++)
            {
                lstevents.Add(new Event { title = resAcc[i].title, start = string.Format("{0:yyyy-MM-dd}", resAcc[i].start), end = string.Format("{0:yyyy-MM-dd}", resAcc[i].end.AddDays(1)), className = "event-azure", allDay = true, EventID = resAcc[i].EventID + "/" + resAcc[i].Type });
            }
            var resWorkshop = repository.GetQuery<IncubationWorkshop>(f => f.IncubationtWorkshopStatu.NameEN == "Active"&&!f.IsDeleted).Select(f => new { title = f.Name, start = f.StartDate, end = f.EndDate, EventID = f.IncubationWorkshopID.ToString(), Type = "IncubationWorkshop" }).ToList();
            for (int i = 0; i < resWorkshop.Count; i++)
            {
                lstevents.Add(new Event { title = resWorkshop[i].title, start = string.Format("{0:yyyy-MM-dd}", resWorkshop[i].start), end = string.Format("{0:yyyy-MM-dd}", resWorkshop[i].end.AddDays(1)), className = "event-default", allDay = true, EventID = resWorkshop[i].EventID + "/" + resWorkshop[i].Type });
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
                var res = repository.GetQuery<Incubation>(f => f.IncubationID == GId).Select(g => new { Name = g.Name, StartDate = g.StartDate, EndDate = g.EndDate, Description = g.ProjectDetails }).FirstOrDefault();
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