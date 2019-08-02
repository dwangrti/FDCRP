using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASJ.Models.Form
{
    public class QuestionIncludeExclude
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public string IncludeExclude { get; set; }
        public string BeforeAfter { get; set; }
        public string DisplayText { get; set; }
        public int OrderNum { get; set; }
    }
}
