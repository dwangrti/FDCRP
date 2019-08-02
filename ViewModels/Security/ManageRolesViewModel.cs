using ASJ.Models;
using ASJ.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ASJ.ViewModels.Security
{
    public class ManageRolesViewModel
    {
        public List<AppIdentityRole> AllRoles { get; set; }
        public List<RoleGroup> RoleGroups { get; set; }
        [Display(Name = "Select Role Group")]
        public int SelectedRoleGroupID { get; set; }
        public string SelectedRoleForRoleGroup { get; set; }
        public List<RoleGroupRoleViewModel> RolesForRoleGroup { get; set; }
        public bool NewRoleAdded { get; set; }
        public bool NewRoleError { get; set; }
        public bool NewRoleGroupAdded { get; set; }
        public bool NewRoleGroupError { get; set; }
    }
}
