using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASJ.Models.Form
{
    public class Instrument
    {
        public int InstrumentId { get; set; }
        public string Name { get; set; }
        public int Year { get; set; }
        public string IntroText { get; set; }
        public string FormNumber { get; set; }
        public DateTime DueDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        
                

    }
}
