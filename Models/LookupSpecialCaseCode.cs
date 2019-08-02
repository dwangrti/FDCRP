using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ASJ.Models
{
    public class LookupSpecialCaseCode
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SpecialCaseCodeId { get; set; }

        public string SpecialCaseCodeDescripion { get; set; }
    }
}
