using System;
using AlfaPeople.KingKhalidFoundation.Data.Model;

namespace AlfaPeople.KingKhaledFoundation.Admin.Web.Models
{
    public class IncubationProjectProposalVM
    {
        public IncubationBaseline IncubationBL { get; set; }

        public IncubationAdvertising IncubationAd { get; set; }

        public IncubationProjectProposal incubationPP { get; set; }
    }
}