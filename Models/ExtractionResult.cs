using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;

namespace ASJ.Models
{
    public enum ExtractionStatus
    {
        NotScheduled,
        Pending,
        Running,
        Stopped,
        Completed,
        Failed,
        PendingTest
    }

    public class SurveyExtractionsResult
    {
        public IList<ExtractionResult> Extractions;
    }
    public class ExtractionResult
    {
        public int ExtractionId { get; set; }
        public string Name { get; set; }
        public string Status { get; set; } // NotScheduled, Pending, Running, Stopped, Completed, Failed, PendingTest
        public int FileId { get; set; }
    }
}
