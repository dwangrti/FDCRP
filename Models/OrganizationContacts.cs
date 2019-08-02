using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using ASJ.Models.Form;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ASJ.Models
{
    public class OrganizationContacts
    {
        [Key]
        public int OrganizationContactId { get; set; }
        [ForeignKey("OrganizationId,OrganizationYear")]
        public Organization Organization { get; set; }
        public string Salutation { get; set; }
        public string Firstname { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }
        public LookupContactType ContactType { get; }
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
        public int ContactTypeId { get; set; }
        public int OrganizationId { get; set; }
        public int OrganizationYear { get; set; }

    }
}
