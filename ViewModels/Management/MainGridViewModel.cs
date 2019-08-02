using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using ASJ.Models;
using ASJ.Models.Form;

namespace ASJ.ViewModels.Management
{
    public class MainGridViewModel
    {
        public int InstrumentId { get; set; }
        public int OrganizationId { get; set; }
        public int Year { get; set; }
        public string Agency { get; set; }
        public string State { get; set; }
        public OrganizationAction ActionDue { get; set; }
        public string LocalTime { get; set; }
        public bool IsTop150 { get; set; }
        public List<OrganizationEvent> Events { get; set; }
        public OrganizationFollowup Followup { get; set; }
        public LookupAgencyStatus AgencyStatus { get; set; }
        public List<OrganizationContacts> Contacts { get; set; }
        public Instrument Instrument { get; set; }
        public DateTime LatestEventDate { get; set; }
        public LookupEvent LookupEvent { get; set; }
        public int ContactTypeId { get; set; }
        public int SpecialCaseCode { get; set; }
        public int NR_OBD { get; set; }
        public string AssignedToNR { get; set; }

    }


}
