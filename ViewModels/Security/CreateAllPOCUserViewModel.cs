using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using ASJ.Models;

namespace ASJ.ViewModels.Security
{
    public class CreateAllPOCUserViewModel
    {
        
        [Required]
        public string Password { get; set; }
        public List<RoleGroup> RoleGroups { get; set; }
        public int SelectedRoleGroupID { get; set; }
        public string Error { get; set; }
        public bool BulkCreate { get; set; }

    }
}
