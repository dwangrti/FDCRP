using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASJ.Models.Form
{
    public class InstrumentQuestion
    {
        public int InstrumentQuestionId { get; set; }
        public virtual Instrument Instrument { get; set; }
        public virtual Question Question { get; set; }
        public int OrderNumber { get; set; }
        public int Page { get; set; }

    }
}
