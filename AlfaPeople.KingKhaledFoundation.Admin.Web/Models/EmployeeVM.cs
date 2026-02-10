using System;
using System.ComponentModel.DataAnnotations;
using AlfaPeople.KingKhalidFoundation.Data.Model;

namespace AlfaPeople.KingKhaledFoundation.Admin.Web.Models
{
    public class EmployeeVM
    {
        public string Email { get; set; }
        [Required]
        public string RoleId { get; set; }
        [Required]
        public Guid PositionId { get; set; }

        public BackendUser _BackEndUser { get; set; }
    }
}