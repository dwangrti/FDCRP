using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using ASJ.Services;

namespace ASJ.Controllers
{
    public abstract partial class BaseController : Controller
    {
        public void GoToHomePage()
        {
            //based on the role of the current user go to the appropriate home page
            
        }
    }
}