using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using ASJ.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.RegularExpressions;
using ASJ.Services;
using Microsoft.AspNetCore.Identity;

namespace ASJ.Utils
{
    public class Extensions
    {
        public static IEnumerable<SelectListItem> GetStatesList()
        {
            IList<SelectListItem> states = new List<SelectListItem>
            {
                new SelectListItem() {Text=" ", Value=" "},
                new SelectListItem() {Text="Alabama", Value="AL"},
                new SelectListItem() {Text="Alaska", Value="AK"},
                new SelectListItem() {Text="Arizona", Value="AZ"},
                new SelectListItem() {Text="Arkansas", Value="AR"},
                new SelectListItem() {Text="California", Value="CA"},
                new SelectListItem() {Text="Colorado", Value="CO"},
                new SelectListItem() {Text="Connecticut", Value="CT"},
                new SelectListItem() {Text="District of Columbia", Value="DC"},
                new SelectListItem() {Text="Delaware", Value="DE"},
                new SelectListItem() {Text="Florida", Value="FL"},
                new SelectListItem() {Text="Georgia", Value="GA"},
                new SelectListItem() {Text="Hawaii", Value="HI"},
                new SelectListItem() {Text="Idaho", Value="ID"},
                new SelectListItem() {Text="Illinois", Value="IL"},
                new SelectListItem() {Text="Indiana", Value="IN"},
                new SelectListItem() {Text="Iowa", Value="IA"},
                new SelectListItem() {Text="Kansas", Value="KS"},
                new SelectListItem() {Text="Kentucky", Value="KY"},
                new SelectListItem() {Text="Lousiana", Value="LA"},
                new SelectListItem() {Text="Maine", Value="ME"},
                new SelectListItem() {Text="Maryland", Value="MD"},
                new SelectListItem() {Text="Massachusetts", Value="MA"},
                new SelectListItem() {Text="Michigan", Value="MI"},
                new SelectListItem() {Text="Minnesota", Value="MN"},
                new SelectListItem() {Text="Mississippi", Value="MS"},
                new SelectListItem() {Text="Missouri", Value="MO"},
                new SelectListItem() {Text="Montana", Value="MT"},
                new SelectListItem() {Text="Nebraska", Value="NE"},
                new SelectListItem() {Text="Nevada", Value="NV"},
                new SelectListItem() {Text="New Hampshire", Value="NH"},
                new SelectListItem() {Text="New Jersey", Value="NJ"},
                new SelectListItem() {Text="New Mexico", Value="NM"},
                new SelectListItem() {Text="New York", Value="NY"},
                new SelectListItem() {Text="North Carolina", Value="NC"},
                new SelectListItem() {Text="North Dakota", Value="ND"},
                new SelectListItem() {Text="Ohio", Value="OH"},
                new SelectListItem() {Text="Oklahoma", Value="OK"},
                new SelectListItem() {Text="Oregon", Value="OR"},
                new SelectListItem() {Text="Pennsylvania", Value="PA"},
                new SelectListItem() {Text="Rhode Island", Value="RI"},
                new SelectListItem() {Text="South Carolina", Value="SC"},
                new SelectListItem() {Text="South Dakota", Value="SD"},
                new SelectListItem() {Text="Tennessee", Value="TN"},
                new SelectListItem() {Text="Texas", Value="TX"},
                new SelectListItem() {Text="Utah", Value="UT"},
                new SelectListItem() {Text="Vermont", Value="VT"},
                new SelectListItem() {Text="Virginia", Value="VA"},
                new SelectListItem() {Text="Washington", Value="WA"},
                new SelectListItem() {Text="West Virginia", Value="WV"},
                new SelectListItem() {Text="Wisconsin", Value="Wi"},
                new SelectListItem() {Text="Wyoming", Value="WY"}
            };
            return states;
        }

        public static Organization GetOrganization(ASJDbContext dataContext, int id, int year)
        {
        var org = dataContext.Organizations
                          .Where(b => (b.OrganizationId == id && b.Year == year))
                          .FirstOrDefault();

            return org;
        }

        public static Boolean CheckFormStarted(ASJDbContext dataContext, int id, int year, int iid)
        {
            var res = dataContext.Responses
                         .Where(b => (b.Organization.OrganizationId == id && b.InstrumentId == iid && b.ResponseVariable == "form_status"))
                         .FirstOrDefault();
            return (res != null && res.ResponseValue != null);
        }

        public static string dataStatusDesc(string formstatus)
        {
            string Desc;

            switch (formstatus)
            {
                case "0":
                    Desc = "Started";
                    break;
                case "1":
                    Desc = "In Progress";
                    break;
                case "2":
                    Desc = "Completed";
                    break;
                default:
                    Desc = "Not Started";
                    break;
            }
            return Desc;
        }

        public static bool RespondentSecurityCheck(ClaimsPrincipal user, int orgId)
        {
            bool pass = false;
            if (user.IsInRole("poc"))
            {
                //strip out 2 letter state
                string loggedinUserOrg = Regex.Replace(user.Identity.Name,"[^0-9.]", "");
                if (loggedinUserOrg == orgId.ToString())
                    pass = true;
            }
            else
                pass = true;
            return pass;
        }

        public static List<AppIdentityUser> GetALManagers(ASJDbContext dataContext, UserManager<AppIdentityUser> userManager)
        {
            List<AppIdentityUser> alManagers = new List<AppIdentityUser>();
            
            foreach(UserRoleGroups urg in dataContext.UserRoleGroups.Where(u => u.RoleGroup.RoleGroupName == "ALManager").ToList())
            {
                AppIdentityUser user = userManager.Users.Where(x => x.Id == urg.UserId).FirstOrDefault();
                if(user != null)
                {
                    alManagers.Add(user);
                }
                
            }
            return alManagers;
        }

        public static List<AppIdentityUser> GetALs(ASJDbContext dataContext, UserManager<AppIdentityUser> userManager)
        {
            List<AppIdentityUser> als = new List<AppIdentityUser>();

            foreach (UserRoleGroups urg in dataContext.UserRoleGroups.Where(u => u.RoleGroup.RoleGroupName == "AgencyLiaison").ToList())
            {
                AppIdentityUser user = userManager.Users.Where(x => x.Id == urg.UserId).FirstOrDefault();
                if (user != null)
                {
                    als.Add(user);
                }

            }
            return als;
        }
    
        public static string GetUserRoleGroup(ASJDbContext dataContext, string currentUserId)
        {
            string roleGroup = "";
            
            UserRoleGroups rg = dataContext.UserRoleGroups.Where(u => u.UserId == currentUserId).Include(u => u.RoleGroup).FirstOrDefault();
            if(rg != null)
            {
                roleGroup = rg.RoleGroup.RoleGroupName;
            }

            return roleGroup;
        }

        public static string formatPhoneNumber(string phoneNumber)
        {
            var numConverted = Convert.ToInt64(@Regex.Replace(phoneNumber, @"[^0-9]", "").Substring(0,10));
            return String.Format("{0:(###) ###-####}", numConverted);
        }
    }
}
