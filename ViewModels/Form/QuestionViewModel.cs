using ASJ.Models.Form;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASJ.ViewModels.Form
{
    public class QuestionViewModel
    {
        public Question Question { get; set; }
        public List<ResponseOption> ResponseOptions { get; set; }
        public List<Response> Responses { get; set; }
        public List<QuestionIncludeExclude> IncludeExcludes { get; set; }
        public string SingleResponse { get; set; }
        public Question EstimateQuestion { get; set; }
        public bool EstimateResponse { get; set; }
        public List<Question> ChildQuestions { get; set;}  //any child question that is not estimate checkbox, this could be more than one
        public int Page { get; set; }  //add page in viewmodel
        public Instrument Instrument { get; set; }
        public string HasFollowupQuestionsClass { get; set; }
        public string ParentQuestionVariable { get; set; }
        public string TotalQuestionClass { get; set; }
        public string AddendQuestionClass { get; set; }
        public int OrderNumber { get; set; }  //from instrumentquestion table

    }
}
