using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;

namespace ASJ.Models
{
    public class ImportSampleResult
    {
        public int SampleId { get; set; }
        public string Status { get; set; } // NotScheduled, Pending, Running, Stopped, Completed, Failed, PendingTest

    }
}
