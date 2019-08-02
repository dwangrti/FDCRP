using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASJ.Models
{
    public class LookupMode
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ModeId { get; set; }
        public string ModeText { get; set; }
    }
}
