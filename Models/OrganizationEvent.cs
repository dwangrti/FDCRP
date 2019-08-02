using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ASJ.Models
{
    public class OrganizationEvent
    {
        public int OrganizationEventId { get; set; }
        [ForeignKey("OrganizationId,OrganizationYear")]
        public virtual Organization Organization { get; set; }
        public LookupEvent LookupEvent { get; set; }
        public string SetBy { get; set; }
        public DateTime EventDate { get; set; }
        public string EventNotes { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public int EventId { get; set; }
        public int OrganizationId { get; set; }
        public int OrganizationYear { get; set; }

    }
}
