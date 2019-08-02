using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ASJ.Models
{
    public class RoleGroup
    {
        [Key]
        public int RoleGroupID { get; set; }
        public string RoleGroupName { get; set; }
    }
}
