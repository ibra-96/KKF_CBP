using System;
using System.Web.Mvc;
using AlfaPeople.KingKhaledFoundation.Admin.Web.Helper;

namespace AlfaPeople.KingKhaledFoundation.Admin.Web.Controllers
{
    public class BaseController : Controller
    {
        //[AllowAnonymous]
        protected override void ExecuteCore()
        {
            int culture = 0;
            if (Session == null || Session["CurrentCulture"] == null)
            {
                int.TryParse(System.Configuration.ConfigurationManager.AppSettings["Culture"], out culture);
                Session["CurrentCulture"] = culture;
            }
            else
            {
                culture = (int)Session["CurrentCulture"];
            }
            // calling CultureHelper class properties for setting  
            CultureHelper.CurrentCulture = culture;

            base.ExecuteCore();
        }

        // GET: Base
        //[AllowAnonymous]
        public ActionResult ChangeCurrentCulture(int id)
        {
            //  
            // Change the current culture for this user.  
            //  
            CultureHelper.CurrentCulture = id;
            //  
            // Cache the new current culture into the user HTTP session.   
            //  
            Session["CurrentCulture"] = id;
            //  
            // Redirect to the same page from where the request was made!   
            //  
            return Redirect(Request.UrlReferrer.ToString());
        }

        protected override bool DisableAsyncSupport
        {
            get { return true; }
        }
    }
}