using System;
using System.Web.Mvc;
using AlphaPeople.Core;
using System.Configuration;
using AlphaPeople.Repository;
using AlfaPeople.KingKhalidFoundation.Data;
using AlfaPeople.KingKhalidFoundation.Data.Model;
using System.Data.Entity;
using System.Linq;

namespace AlfaPeople.KingKhalidFoundation.Web.Controllers
{
    [Authorize(Roles = "Corporation")]
    public class IncubationWorkshopRatingController : BaseController
    {
        private readonly IRepository repository;
        private readonly CommonHelper helper;
        private readonly string Logpath = ConfigurationManager.AppSettings["Log"].ToString();

        public IncubationWorkshopRatingController()
        {
            repository = new Repository(new KingkhaledFoundationDB());
            helper = new CommonHelper();
        }

        // GET: IncubationWorkshopRating
        public ActionResult Index()
        {
            return View();
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
    }
}