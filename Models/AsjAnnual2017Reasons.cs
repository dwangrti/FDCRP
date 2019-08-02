using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASJ.Models
{
    [Table("asj_annual_2017_reasons")]
    public partial class AsjAnnual2017Reasons
    {
        [Key]
        public int AsjAnnualReasonId { get; set; }
        public int OrganizationId { get; set; }
        public string Question { get; set; }
        public string Reason { get; set; }
        public DateTime? LastUpdated { get; set; }
        public string CreatedUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedUserId { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
