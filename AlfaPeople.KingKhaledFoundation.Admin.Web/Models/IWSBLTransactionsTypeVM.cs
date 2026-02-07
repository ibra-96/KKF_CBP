using System;
using System.Collections.Generic;
using System.Web.Mvc;
using AlfaPeople.KingKhalidFoundation.Data.Model;

namespace AlfaPeople.KingKhaledFoundation.Admin.Web.Models
{
    public class IWSBLTransactionsTypeVM
    {
        public Guid TransTypeID { get; set; }
        public string TransTypeName { get; set; }
        public string IsMasterData { get; set; }
        public List<IWSBLTransactionsVM> LstTransaction { get; set; } = new List<IWSBLTransactionsVM>();
        //shadia
        // تحديد ورشة العمل المرتبطة بهذا النوع من الحقول
        public Guid? WorkshopID { get; set; }

        // قائمة الورش لاختيار الورشة المناسبة
        public List<SelectListItem> Workshops { get; set; }
        //public List<IncubationWorkshopControlOptionVM> Options { get; set; } = new List<IncubationWorkshopControlOptionVM>();
        // يحتوي على المعاملات مع الخيارات المضمنة

    }
}
