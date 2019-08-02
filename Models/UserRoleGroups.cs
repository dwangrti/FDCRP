using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASJ.Services;
using Microsoft.AspNetCore.Identity;

namespace ASJ.Models
{
    public class UserRoleGroups
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public RoleGroup RoleGroup { get; set; }

    }
}
