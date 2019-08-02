using ASJ.Models;
using ASJ.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ASJ.ViewModels.Security
{
    public class CreateRoleOrRoleGroupViewModel
    {
        public string NewRole { get; set; }
        public string NewRoleGroup { get; set; }
        public bool NewRoleAdded { get; set; }
        public bool NewRoleError { get; set; }
        public bool NewRoleGroupAdded { get; set; }
        public bool NewRoleGroupError { get; set; }
    }
}
