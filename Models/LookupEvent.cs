using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ASJ.Models
{
    public class LookupEvent
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int EventId { get; set; }
        public string CodesEventText { get; set; }
        public bool isActive { get; set; }
        //public int AnnualFormType { get; set; }


    }
}
