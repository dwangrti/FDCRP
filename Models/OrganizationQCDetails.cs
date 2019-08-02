using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ASJ.Models
{
    public class OrganizationQCDetails
    {
        [Key]
        public int OrganizationQcDetailsId { get; set; }
        public Organization Organization { get; set; }
        public string QCDetails { get; set; }
        //public int DeathID { get; set; }
        public Form.Instrument Instrument {get; set; }
        public int Location { get; set; }
        public string CYrange { get; set; }
        public string PYrange { get; set; }
        public DateTime FirstAppeared { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }


    }
    
}
