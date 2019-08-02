using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ASJ.Models;
using ASJ.Web.ViewModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using ASJ.Services;
using static ASJ.Startup;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Diagnostics;
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;

namespace ASJ.Controllers
{
    public class HomeController : BaseController
    {
       private SignInManager<AppIdentityUser> signInManager;
        private UserManager<AppIdentityUser> userManager;
        private RoleManager<AppIdentityRole> roleManager;
        private ASJDbContext dataContext;
        private readonly ApplicationVariables appVariables;
        private IEmailSender emailSender;

        public HomeController(SignInManager<AppIdentityUser> signInManager, UserManager<AppIdentityUser> userManager, RoleManager<AppIdentityRole> roleManager, ASJDbContext dbContext, IOptions<ApplicationVariables> appVariables, IEmailSender emailSender)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.dataContext = dbContext;
            this.appVariables = appVariables.Value;
            this.emailSender = emailSender;
        }

        public IActionResult Index()
        {
            LoginViewModel model = new LoginViewModel();
            //if (model.ReturnUrl == null && model.Username == null && model.Password == null && model.LoginErrors == null)
            //    model = new LoginViewModel { Username = "", ReturnUrl = model.ReturnUrl };
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {

            ViewData["ReturnUrl"] = returnUrl;

            if (ModelState.IsValid)
            {
                var result = await this.signInManager.PasswordSignInAsync(
                model.Username, model.Password, isPersistent: false, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    AppIdentityUser user = await userManager.FindByNameAsync(model.Username);
                    var roles = await userManager.GetRolesAsync(user);
                    if (roles.Contains("poc"))
                    {
                        //log an event for the org
                        Organization org = dataContext.Organizations.Where(x => x.UserName == model.Username && x.Year.ToString() == appVariables.ReferenceYear).FirstOrDefault();
                        if (org != null)
                        {
                            LookupEvent l = dataContext.LookupEvents.Where(x => x.CodesEventText.Contains("870")).FirstOrDefault();
                            DateTime currentTime = System.DateTime.Now;
                            string userName = "SYSTEM";
                              OrganizationEvent orgEvent = new OrganizationEvent()
                                {
                                    LookupEvent = l,
                                    Organization = org,
                                    EventDate = currentTime,
                                    CreatedOn = currentTime,
                                    ModifiedOn = currentTime,
                                    CreatedBy = userName,
                                    ModifiedBy = userName,
                                    SetBy = userName
                              };
                            dataContext.Add(orgEvent);
                            dataContext.SaveChanges();
                        }
                        return RedirectToAction("Index", "Respondent");
                    }
                    if (roles.Contains("clientmenu"))
                        return RedirectToAction("Index", "Client");
                    if (roles.Contains("dashboard"))
                        return RedirectToAction("Index", "Management");
                }
                else
                {
                    model.LoginErrors = "Incorrect Username/Password";
                }
            }
            return View("Index",model);
        }
        public async Task<IActionResult> Logout()
        {
            await this.signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Landing()
        {
            return View();
        }
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult FAQ()
        {
            
            List<Organization> orgs = (from n in dataContext.Organizations
                                     where(n.UserName == @User.Identity.Name)
                                     select new Organization()
                                     {
                                         OrganizationId = n.OrganizationId,
                                         Name = n.Name,
                                         State = n.State,
                                         Year = n.Year,
                                     }).OrderByDescending(o => o.Year).ToList();

            Organization org = null;
            if (orgs.Count > 0)
            {
                org = orgs[0];
            }          
            ViewData["Org"] = org;
          
            return View();
        }

        public IActionResult Error()
        {
            // Get the details of the exception that occurred
            var exceptionFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            if (exceptionFeature != null)
            {
                // Get which route the exception occurred at
                string routeWhereExceptionOccurred = exceptionFeature.Path;

                // Get the exception that occurred
                Exception exceptionThatOccurred = exceptionFeature.Error;
                try
                {
                    string username = "";
                    if (User != null && User.Identity != null)
                        username = User.Identity.Name;
                    string emailBody = "Error occured in path " + exceptionFeature.Path + "<br />";
                    emailBody = emailBody + "Username = " + username + "<br />";
                    emailBody = emailBody + exceptionThatOccurred.Message.ToString() + "<br />";
                    emailBody = emailBody + exceptionThatOccurred.StackTrace + "<br />";

                    //send email
                    emailSender.SendEmail(appVariables.DeveloperEmails, "Error Occured in " + this.Request.Host.Host, emailBody, "");
                }
                catch(Exception e)
                {
                    //sending the email is also not working
                }
            }

            return View();
        }
    }
}
