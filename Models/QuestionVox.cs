using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASJ.Models
{
    public enum QuestionType
    {
        Text,
        DateTime
    }
    public class QuestionVox
    {
        public string Name { get; set; }
        public string Type { get; set; }

        public List<string> Variables { get; set; }
    }
}
