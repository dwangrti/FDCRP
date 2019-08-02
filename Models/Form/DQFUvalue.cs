using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASJ.Models.Form
{
    public class DQFUvalue
    {
        [Key]
        public int OrganizationID { get; set; }
        public int? CURRconfpop { get; set; }
        public int? CURRadultm { get; set; }
        public int? CURRjuvm { get; set; }
        public int? CURRconfmale { get; set; }
        public int? CURRadultf { get; set; }
        public int? CURRjuvf { get; set; }
        public int? CURRconffemale { get; set; }
        public int? CURRnconpop { get; set; }
        public double? CURRadp { get; set; }
        public double? CURRadpmale { get; set; }
        public double? CURRadpfemale { get; set; }
        public int? CURRrated { get; set; }
        public int? CURRadmis { get; set; }
        public int? CURRadmismale { get; set; }
        public int? CURRadmisfemale { get; set; }
        public int? CURRrelease { get; set; }
        public int? CURRreleasemale { get; set; }
        public int? CURRreleasefemale { get; set; }
        public int? PREVconfpop { get; set; }
        public int? PREVadultm { get; set; }
        public int? PREVjuvm { get; set; }
        public int? PREVconfmale { get; set; }
        public int? PREVadultf { get; set; }
        public int? PREVjuvf { get; set; }
        public int? PREVconffemale { get; set; }
        public int? PREVnconpop { get; set; }
        public double? PREVadp { get; set; }
        public double? PREVadpmale { get; set; }
        public double? PREVadpfemale { get; set; }
        public int? PREVrated { get; set; }
        public int? PREVadmis { get; set; }
        public int? PREVadmismale { get; set; }
        public int? PREVadmisfemale { get; set; }
        public int? PREVrelease { get; set; }
        public int? PREVreleasemale { get; set; }
        public int? PREVreleasefemale { get; set; }

    }
}
