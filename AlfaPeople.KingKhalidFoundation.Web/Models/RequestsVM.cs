using System;
using System.Collections.Generic;
using AlfaPeople.KingKhalidFoundation.Data.Model;

namespace AlfaPeople.KingKhalidFoundation.Web.Models
{
    public class RequestsVM
    {
        public List<IncubationAdvertising> LstIncubation { get; set; }
        public List<IncubationWorkshop> LstIWorkshop { get; set; }
        //20-2-2025
        //  إضافة قائمة لحفظ الدعوات الملغاة
        public List<IncubationPrivateInvitation> LstCancelledInvitations { get; set; }
        //25-2-2025
        public List<WorkshopPrivateInvitation> LstCancelledWorkshops { get; set; } //  الورش الملغاة
    }
}