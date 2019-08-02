using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using ASJ.Models;
using ASJ.Services;
using ASJ.Models.Form;

namespace ASJ.ViewModels.Event
{
    public class EventsTrackerViewModel
    {
        public int OrganizationEventId { get; set; }
        public virtual Organization Organization { get; set; }
        public LookupEvent Event { get; set; }
        public string SetBy { get; set; }
        public DateTime EventDate { get; set; }
        public string EventNotes { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string Error { get; set; }
        public int EventId { get; set; }
        public int EventYear { get; set; }
        public OrganizationAction ActionDue { get; set; }

    }      

        
}
