using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AlfaPeople.KingKhalidFoundation.Data.Model
{
    public class WorkshopProjectProposal
    {
        public WorkshopProjectProposal()
        {
            WorkshopProjectProposalAttachments = new HashSet<WorkshopProjectProposalAttachment>();
            EmployeesAttendIncubationWorkShops = new HashSet<EmployeesAttendIncubationWorkShop>();
        }

        public Guid WorkshopProjectProposalID { get; set; }

        public Guid WorkshopProjectProposalStatusID { get; set; }

        public Guid IncubationWorkshopID { get; set; }

        public Guid FrontendUserID { get; set; }

        [Required]
        public WorkshopPPInvitationStatus WorkshopPP_InvitationStatus { get; set; }

        public string Feedback { get; set; }

        public virtual FrontendUser FrontendUser { get; set; }

        public virtual WorkshopProjectProposalStatu WorkshopProjectProposalStatu { get; set; }

        public virtual IncubationWorkshop IncubationWorkshop { get; set; }

        public virtual ICollection<WorkshopProjectProposalAttachment> WorkshopProjectProposalAttachments { get; set; }

        public virtual ICollection<EmployeesAttendIncubationWorkShop> EmployeesAttendIncubationWorkShops { get; set; }
    }
}