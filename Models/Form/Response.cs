using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASJ.Models.Form
{
    public class Response
    {
        public int ResponseId { get; set; }
        public Organization Organization { get; set; }
        public virtual Instrument Instrument { get; set; }
        public int InstrumentId { get; set; }
//        public virtual Question Question { get; set; }
        //public int QuestionId { get; set; } //getting a databsae tracking error at random times when saving to database. Error persists even with AsNoTracking used everywhere, trying this so we are only using the ID, not the Quetsion object
        public virtual ResponseOption ResponseOption { get; set; }
        public string ResponseVariable { get; set; }
        public string ResponseValue { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
