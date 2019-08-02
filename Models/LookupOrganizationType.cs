using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ASJ.Models.Form;

namespace ASJ.Models
{
    public class LookupOrganizationType
    {
        [Key]
        public int OrganizationTypeId { get; set; }
        public int Year { get; set; }
        public string Description { get; set; }
        public int AnnualFormType { get; set; }
        public string FormTypeDescription { get; set; }
        public Instrument Instrument { get; set; }

    }
}
