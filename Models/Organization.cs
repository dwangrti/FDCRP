using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using ASJ.Models;
using ASJ.Models.Form;
using ASJ.Services;

namespace ASJ.Models
{
    public class Organization
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int OrganizationId { get; set; }
        public int Year { get; set; }
        public LookupOrganizationType OrganizationType { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Url { get; set; }
        public bool IsTop150 { get; set; }
        public int SpecialCaseCode { get; set; }
        public bool IsRegional { get; set; }
        public bool IsPrivate { get; set; }
        public int InstrumentId { get; set; }
        public Instrument Instrument { get; set; }
        public string UserName { get; set; }
        public string PasswordSecure { get; set; }
        public virtual OrganizationFollowup OrganizationFollowup { get; set; }
        public List<OrganizationEvent> OrganizationEvents { get; set; }
        public List<OrganizationContacts> OrganizationContacts { get; set; }
        public List<Response> Responses { get; set; }
        public List<OrganizationFacility> OrganizationFacility { get; set; }
        public int OrganizationTypeId { get; set; }

        public string GDC { get; set; }
        public int PIN { get; set; }

    }
}
