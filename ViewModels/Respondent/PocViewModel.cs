using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ASJ.Models;
using ASJ.Models.Form;

namespace ASJ.ViewModels.Respondent
{
    public class PocViewModel
    {
        [Required(ErrorMessage = "Agency Name or ID is required")]
        public string Agency { get; set; }
        public int OrganizationId { get; set; }
        public int Year { get; set; }
        public string State { get; set; }
        public string UserName { get; set; }
        public OrganizationFollowup Followup { get; set; }
        public int InstrumentId { get; set; }
        public LookupAgencyStatus LookupAgencyStatus { get; set; }
        public LookupOrganizationType OrganizationType { get; set; }
        public Response Response { get; set; }
        public string dataStatus { get; set; }

    }
}
