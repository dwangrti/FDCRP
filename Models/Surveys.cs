using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASJ.Models
{
    public class UserSurveys
    {
        public IEnumerable<Survey> Surveys { get; set; }
    }

    public class Survey
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
