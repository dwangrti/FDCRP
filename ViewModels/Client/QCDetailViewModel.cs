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
    public class QCDetailViewModel
    {
        [Required(ErrorMessage = "Agency Name or ID is required")]
        public string Agency { get; set; }
        public int OrganizationId { get; set; }
        public int Year { get; set; }
        public Instrument Instrument { get; set; }
        public Organization Organization { get; set; }
        public string QCDetails { get; set; }
        public int Location { get; set; }
        public string CYrange { get; set; }
        public string PYrange { get; set; }
        public DateTime FirstAppeared { get; set; }


    }
}
