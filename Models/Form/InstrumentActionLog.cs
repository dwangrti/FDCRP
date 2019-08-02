using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ASJ.Models.Form
{
    public class InstrumentActionLog
    {
        [Key]
        public int FormActionTrackingId { get; set; }
        public int OrganizationId { get; set; }
        public int Year { get; set; }
        public int InstrumentId { get; set; }
        public int CurrentPage { get; set; }
        public int NextPage { get; set; }
        public string Action { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
