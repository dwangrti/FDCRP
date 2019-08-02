using ASJ.Services;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASJ.Models
{
    public class RoleGroupRoles
    {
        public int Id { get; set; }
        public RoleGroup RoleGroup { get; set; }
        public string RoleId { get; set; }

    }
}
