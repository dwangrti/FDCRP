using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ASJ.Models.Form
{
    public class LookupQuestionType
    {
        public int QuestionTypeId { get; set; }
        public string QuestionTypeDescription { get; set; }
    }
}
