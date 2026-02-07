using System;
using System.Web;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AlfaPeople.KingKhalidFoundation.Data.Model;

namespace AlfaPeople.KingKhalidFoundation.Web.Models
{
    public class IncubationWorkshopJoinVM
    {
        [Display(Name = "Browse File")]
        public HttpPostedFileBase[] files { get; set; }

        public IncubationWorkshop incubationWorkshop { get; set; }

        public WorkshopProjectProposal WorkshopProjectProposal { get; set; }

        public List<EmployeesAttendIncubationWorkShop> LstEmployee_WS { get; set; }
    }
}