using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;

namespace ASJ.Models
{
    public class Extraction
    {
        public string Name { get; set; }
        public int SurveyId { get; set; }
        public string Language { get; set; }
        public string[] Variables { get; set; }
        public string Description { get; set; }
        public string ExtractFormat { get; set; } // Access, Acuity4Survey, CSV, SAV, Excel
        public CaseFilter Filter { get; set; }

        public bool IncludeOpenEnds { get; set; } //Include open-end answers for choices
        public bool IncludeConnectionHistory { get; set; } //Include the connection history of the respondent
        public bool IncludeLabels { get; set; } //Access format. Includes the label with the reponse
        public bool StripHtmlFromLabels { get; set; } //Remove the html formatting of labels

        //public FieldDelimiterType FieldDelimiter	FieldDelimiterType	No	Field delimiter used between values. Default: Comma
        //public EncodingType Encoding	EncodingType	No	Specify the encoding of answers: Default: Windows1252

        public bool EncloseValuesInDoubleQuotes { get; set; } //Put values between double quotes
        public bool IncludeHeader { get; set; } //Output the header as the first row
        public bool UseChoiceLabels { get; set; } //Use choice labels instead of choice codes (codes are the default)
        public bool MergeOpenEnds { get; set; } //Merge multiple open-ends into one cell
        public bool DichotomizedMultiple { get; set; } //SAV format. For multiple response questions create a variable for each choice
        public bool DichotomizedEmptyWhenNoAnswer { get; set; } //SAV format. Set the dichtomized variable to empty when no answer provided instead of 0
        public bool UseNegativeIntegersForEmptyAnswers { get; set; } //SAV format. Replaces empty numeric value with -9998 and non numeric with -9999
        public bool DapresyDataFormat { get; set; }
    }

    public class CaseFilter
    {
        public int Id { get; set; }
        public LastActivityFilter LastActivity { get; set; }

        //public List<string> DispositionResults = new List<string>();
        //public byte EmailStatus { get; set; }
        //public List<string> Languages = new List<string>();

        public string Expression { get; set; }
    }

    public class LastActivityFilter
    {
        public bool UseCurrentDate { get; set; }
        public DateTime? Begin { get; set; }
        public DateTime? End { get; set; }
    }

}
