using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ASJ.Models
{
    public class LookupContactType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ContactTypeId { get; set; }
        public string ContacTypeDescription { get; set; }


    }
}
