using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ASJ.Models
{
    public class LookupAgencyStatus
    {
        [Key]
        public int AgencyStatusCodeId { get; set; }
        public string AgencyStatusCodeDescription { get; set; }

    }
}
