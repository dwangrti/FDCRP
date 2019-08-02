using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ASJ.Models;
using ASJ.Models.Form;
using ASJ.Models.PDF;

namespace ASJ.ViewModels.Client
{
    public class QCExplanationViewModel
    {
        [Required(ErrorMessage = "Agency Name or ID is required")]
        public string Agency { get; set; }
        public int OrganizationId { get; set; }
        public Instrument Instrument { get; set; }

        public List<QuestionExplanation> ExplanationsList { get; set; }  

        public QCExplanationViewModel()
        {
            ExplanationsList =new List<QuestionExplanation>();
        }
    }


    public class QuestionExplanation
    {
        public string QuestionId { get; set; }
        public string Explananation { get; set; }
        public string PrevExplananation { get; set; }

        public QuestionExplanation(string qid, string exp, string prevExp)
        {
            this.QuestionId = qid;
            this.Explananation = exp;
            this.PrevExplananation = prevExp;
        }
    }
}
