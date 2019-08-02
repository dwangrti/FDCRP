using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ASJ.Models
{
    public class OrganizationFacility
    {
        [Key]
        public int OrganizationFacilityId { get; set; }
        public virtual Organization Organization { get; set; }
        public string FacilityName { get; set; }
        public string FacilityAddress { get; set; }
        public string FacilityCity { get; set; }
        public string FacilityState { get; set; }
        public string FacilityZip { get; set; }
        public string FacilityComment { get; set; }
        public bool IsPrivate { get; set; }
        public bool IsRegional { get; set; }
        public bool Inactive { get; set; }
        public int FacilityStatusCode { get; set; }
        public string FacilityNotes { get; set; }


    }
}
