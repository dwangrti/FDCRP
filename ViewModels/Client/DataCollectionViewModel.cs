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
    public class DataCollectionViewModel
    {
        public int OrganizationId { get; set; }
        public int Year { get; set; }
        public int InstrumentId { get; set; }
        public string form_status { get; set; }
        public string dataStatus { get; set; }
        public int AsjStatusCode { get; set; }
    }
}
