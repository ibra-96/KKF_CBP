using AlfaPeople.KingKhalidFoundation.Data.Model;
using System;
using System.Collections.Generic;

namespace AlfaPeople.KingKhaledFoundation.Admin.Web.Models
{
   
    public class IWSBLTransactionsVM
    {
        public string TransID { get; set; }
        public string TransTypeID { get; set; }
        public string ControlID { get; set; }
        public string ControlTypeID { get; set; }
        public string FieldNameEn { get; set; }
        public string FieldNameAr { get; set; }
        public string LookupTransTypeID { get; set; }
        public string LookupTransID { get; set; }
        public string IsRequired { get; set; }
        public string ViewList_Display { get; set; }
        //public string ViewList_Display { get; set; }
        public string RankNumber { get; set; }
        //shadia
        // إضافة معرف الورشة لتحديد الورشة المرتبطة بالحقل
        public Guid? WorkshopID { get; set; }
        public List<IncubationWorkshopControlOptionVM> Options { get; set; } = new List<IncubationWorkshopControlOptionVM>();
        


    }

}