using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using ASJ.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ASJ.ViewModels.Contact
{
    public class ContactsViewModel
    {
        public int OrganizationContactId { get; set; }
        public int OrganizationId { get; set; }
        public int OrganizationYear { get; set; }
        public Organization Organization { get; set; }
        public string Salutation { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }
        public LookupContactType LookupContactType { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Phone { get; set; }
        public string PhoneExt { get; set; }
        public string Email { get; set; }
        public bool PrimaryContact { get; set; }
        public bool AgencyHead { get; set; }
        public bool BackupContact { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string Error { get; set; }
        public int ContactTypeId { get; set; }



    }
}
