using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ASJ.Models
{
    public class LookupFrameMembership
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int FrameMembershipId { get; set; }
        public string FrameMembershipDescription { get; set; }

    }
}
