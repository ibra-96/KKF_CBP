using System;
using System.ComponentModel.DataAnnotations;

namespace AlfaPeople.KingKhalidFoundation.Data.Model
{
    public class EmployeesAttendIncubationWorkShop
    {
        [Key]
        public Guid ID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Mobile { get; set; }

        [Required]
        public string Position { get; set; }

        [Required]
        public string PositionTasks { get; set; }

        [Required]
        public GenderPerson Gender { get; set; }

        [Required]
        public string EducationalQualificationAndSpecialization { get; set; }

        public Guid WorkshopProjectProposalID { get; set; }

        public virtual WorkshopProjectProposal WorkshopProjectProposal { get; set; }
    }
}