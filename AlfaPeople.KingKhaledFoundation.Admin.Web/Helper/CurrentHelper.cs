using System;
using System.Web;
using System.Linq;
using System.Threading;
using System.Globalization;
using System.Web.SessionState;
using System.Collections.Generic;

namespace AlfaPeople.KingKhaledFoundation.Admin.Web.Helper
{
    public class CultureHelper
    {
        protected HttpSessionState session;

        //constructor   
        public CultureHelper(HttpSessionState httpSessionState)
        {
            session = httpSessionState;
        }
        // Properties  
        public static int CurrentCulture
        {
            get
            {
                if (Thread.CurrentThread.CurrentUICulture.Name == "en-GB")
                {
                    return 0;
                }
                else if (Thread.CurrentThread.CurrentUICulture.Name == "hi-IN")
                {
                    return 1;
                }
                else if (Thread.CurrentThread.CurrentUICulture.Name == "mr-IN")
                {
                    return 2;
                }
                else if (Thread.CurrentThread.CurrentUICulture.Name == "ar-EG")
                {
                    return 3;
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                if (value == 0)
                {
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-GB");
                }
                else if (value == 1)
                {
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo("hi-IN");
                }
                else if (value == 2)
                {
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo("mr-IN");
                }
                else if (value == 3)
                {
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo("ar-EG");
                }
                else
                {
                    Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
                }

                Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture;
            }
        }
    }
}