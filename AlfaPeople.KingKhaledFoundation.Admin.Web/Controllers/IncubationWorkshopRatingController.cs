using System;
using System.Linq;
using System.Web.Mvc;
using AlphaPeople.Core;
using System.Data.Entity;
using System.Configuration;
using AlphaPeople.Repository;
using AlfaPeople.KingKhalidFoundation.Data;
using AlfaPeople.KingKhalidFoundation.Data.Model;

namespace AlfaPeople.KingKhaledFoundation.Admin.Web.Controllers
{
    [Authorize(Roles = "Admin, CB Manager, CB Supervisor, CB Analyst")]
    public class IncubationWorkshopRatingController : BaseController
    {
        #region MemmbersAndFields
        private readonly IRepository repository;
        private readonly CommonHelper helper;
        private readonly string Logpath = ConfigurationManager.AppSettings["Log"].ToString();
        #endregion

        #region ctor
        public IncubationWorkshopRatingController()
        {
            repository = new Repository(new KingkhaledFoundationDB());
            helper = new CommonHelper();
        }
        #endregion

        #region Controllers
        // GET: IncubationWorkshop
        public ActionResult Index(Guid id)
        {
            var IncubationWorkshopEaluations = repository.GetQuery<IncubationWorkshopRating>().Include(t => t.IncubationWorkshop).Include(t => t.FrontendUser).Where(w => w.IncubationWorkshopID == id).ToList();
            return View(IncubationWorkshopEaluations);
        }

        public ActionResult Detial(Guid id)
        {
            var IncubationWorkshopEaluation = repository.GetQuery<IncubationWorkshopRating>(r => r.IncubationWorkshopRatingId == id).Include(t => t.IncubationWorkshop).Include(t => t.FrontendUser).SingleOrDefault();
            return View(IncubationWorkshopEaluation);
        }
        #endregion

    }
}