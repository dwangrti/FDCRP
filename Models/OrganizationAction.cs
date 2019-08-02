using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ASJ.Models
{
    public class OrganizationAction
    {
        [Key]
        public int OrganizationActionId { get; set; }
        public int? OrganizationId { get; set; }
        [ForeignKey("OrganizationId,OrganizationYear")]
        public virtual Organization Organization { get; set; }
        public string AssignedTo { get; set; }
        public string ActionNotes { get; set; }
        public DateTime ActionDueDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public int? OrganizationYear { get; set; }

    }
}
