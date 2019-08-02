using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASJ.Models.Form
{
    public class ResponseOption
    {
        public int ResponseOptionId { get; set; }
        public virtual Question Question { get; set; }
        public string ResponseOptionText { get; set; }
        public int OrderNumber { get; set; }
        public int ResponseOptionValue { get; set; }
    }
}
