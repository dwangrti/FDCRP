using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ASJ.Models
{
    public class DQFUEmail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmailId { get; set; }
        public int EmailName { get; set; }
        public int StatusCode { get; set; }
        public string ContentFileName { get; set; }
        public string Subject { get; set; }
        public int Year { get; set; }
    }
}
