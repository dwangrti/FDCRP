using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ASJ.Models
{
    public class OrganizationStatus
    {
        public int OrganizationStatusId { get; set; }
        public int AgencyStatusCodeId { get; set; }
        public virtual LookupAgencyStatus AgencyStatusCode { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
