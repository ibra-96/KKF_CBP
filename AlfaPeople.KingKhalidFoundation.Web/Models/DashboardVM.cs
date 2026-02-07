using System;
using System.Collections.Generic;
using AlfaPeople.KingKhalidFoundation.Data.Model;

namespace AlfaPeople.KingKhalidFoundation.Web.Models
{
    public class DashboardVM
    {
        public List<IncubationAdvertising> _inc { get; set; }
        public List<IncubationAdvertising> _incAcc { get; set; }
        public List<IncubationProjectProposal> LstUpdateIncubationProjectProposal { get; set; }
        public List<IncubationWorkshop> _workShop { get; set; }
        public List<WorkshopProjectProposal> LstUpdateWorkshopProjectProposal { get; set; }
        public List<FollowUpProjectPlanRequest> _followUpProjectPlan { get; set; }
        public List<IncubationWorkshop> _incubationWorkshopRating { get; set; }
        //shadia
        //public List<IncubationWorkshopBLTransactionsValue> JoinedBLWork { get; set; }
        ////
        public Dictionary<Guid, string> WorkshopStatuses { get; set; } = new Dictionary<Guid, string>();
    }

}
