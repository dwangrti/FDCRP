using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASJ.Models.Form
{
    public class Question
    {
        public int QuestionId { get; set; }
        public string Variable { get; set; }
        public virtual LookupQuestionType QuestionType { get; set; }
        public string QuestionText { get; set; }
        public string IntroText { get; set; }
        public int ParentQuestionId { get; set; }
        public int ParentResponseId { get; set; }
        public string ParentResponseValue { get; set; }
        public string DisplayNumber { get; set; }
        public string Indentation { get; set; }
        public int OrderSegment { get; set; }
      //  public int Page { get; set; }, no longer in database
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
