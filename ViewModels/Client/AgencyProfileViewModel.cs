using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ASJ.Models;
using ASJ.Models.Form;
using ASJ.Models.PDF;

namespace ASJ.ViewModels.Client
{
    public class AgencyProfileViewModel
    {
        [Required(ErrorMessage = "Agency Name or ID is required")]
        public string Agency { get; set; }
        public int OrganizationId { get; set; }
        public int Year { get; set; }
        public string State { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        public string Url { get; set; }
        public bool IsTop150 { get; set; }
        public int SpecialCaseCode { get; set; }
        public bool IsRegional { get; set; }
        public bool IsPrivate { get; set; }
        public string ReturnUrl { get; set; }
        public List<OrganizationFacility> Facilities { get; set; }
        public List<OrganizationContacts> Contacts { get; set; }
        public ICollection<OrganizationEvent> Events { get; set; }
        public OrganizationFollowup Followup { get; set; }
        public LookupAgencyStatus LookupAgencyStatus { get; set; }
        public int InstrumentId { get; set; }
        public Instrument Instrument { get; set; }
        public LookupOrganizationType OrganizationType { get; set; }
        public Response Response { get; set; }
        public string dataStatus { get; set; }
        public string Error { get; set; }
        public string form_status { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public List<OrganizationQCDetails> QCDetails { get; set; }
        public int Reasons { get; set; }
        public string LoggedInUserEmail { get; set; }
        public int? ASJStatusCode { get; set; }
        public int? ASJQualityCode { get; set; }
        public DateTime ModifiedDate { get; set; }

    }
}
