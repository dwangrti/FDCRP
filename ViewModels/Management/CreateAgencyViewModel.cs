using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using ASJ.Models;

namespace ASJ.ViewModels.Management
{
    public class CreateAgencyViewModel
    {
        [Required]
        public string Name { get; set; }
        public int OrganizationId { get; set; }
        public int Year { get; set; }
        public LookupOrganizationType OrganizationType { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Url { get; set; }
        public bool IsTop150 { get; set; }
        public int SpecialCaseCode { get; set; }
        public bool IsRegional { get; set; }
        public bool IsPrivate { get; set; }
        public string Error { get; set; }



    }
}
