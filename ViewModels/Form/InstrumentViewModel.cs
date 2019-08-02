using ASJ.Models;
using ASJ.Models.Form;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ASJ.ViewModels.Form
{
    public class InstrumentViewModel
    {
        public int OrganizationId { get; set; }
        public int Year { get; set; }
        public int OrgType { get; set; }
        public Instrument Instrument { get; set; }
        public Organization Organization { get; set; }
        public List<QuestionViewModel> Questions { get; set; }
        public bool RTIDataEntry { get; set; } //TVINCENT - this property is used to disable the form validations
        public int SelectedPage { get; set; }
        public int MaxPage { get; set; }  //the max number of pages for the instrument
        public bool formStatus { get; set; }
        public DataSupplier DataSupplier { get; set; }
        public DataSupplierViewModel DatasupplierVM { get; set; }
        [DisplayName("Submission Mode")]
        public string SelectedMode { get; set; }
        public IEnumerable<LookupMode> Modes { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateReceived { get; set; }
        [DisplayName("Summary Status Code")]
        public int SelectedSummaryStatusCodeId { get; set; }
        public IEnumerable<LookupSummaryStatus> SummaryStatuses { get; set; }
    }
}
