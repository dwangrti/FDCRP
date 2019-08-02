using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ASJ.Models
{
    public class LookupSummaryStatus
    {
        [Key]
        public int SummaryStatusCodeId { get; set; }
        public string SummaryStatusCodeDescription { get; set; }

    }
}
