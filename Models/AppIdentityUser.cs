using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASJ.Services
{
    public class AppIdentityUser : IdentityUser
    {
        public string DisplayName { get; set; }
    }
}
