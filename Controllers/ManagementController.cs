using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ASJ.Models;
using ASJ.Models.Form;
using ASJ.ViewModels.Management;
using ASJ.ViewModels.Client;
using ASJ.Services;
using ASJ.Utils;
using static ASJ.Startup;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data;

namespace ASJ.Controllers
{
    [Authorize]
    public class ManagementController : BaseController
    {
        private ASJDbContext dataContext;
        private IEmailSender emailSender;
        public int referenceYear;
        private readonly ApplicationVariables _appVariables;
        private readonly UserManager<AppIdentityUser> _userManager;
       

        public ManagementController(ASJDbContext dbContext, UserManager<AppIdentityUser> userManager, IEmailSender emailSender, IOptions<ApplicationVariables> appVariables)
        {
            this.dataContext = dbContext;
            this.emailSender = emailSender;
            _appVariables = appVariables.Value;
            this.referenceYear = Int32.Parse(_appVariables.ReferenceYear);
           _userManager= userManager;
           
        }
        
        public IActionResult Index(string gridType="Index")
        {
            var alManagers = Extensions.GetALManagers(dataContext, _userManager);
            var allUsers = Extensions.GetALs(dataContext, _userManager);
            //var username = User.Identity.Name;
            
            var specialCaseCodes = dataContext.LookupSpecialCaseCodes.ToList();
            var assignUsers = new List<string>();
            assignUsers.Add(" ");
            foreach (var x in alManagers)
            {
                assignUsers.Add(x.UserName);
            }
            foreach (var u in allUsers)
            {
                assignUsers.Add(u.UserName);
            }
            
            var currentUserRoleGroup = Extensions.GetUserRoleGroup(dataContext, this._userManager.GetUserId(User));

            List<OrganizationAction> actions = (from a in dataContext.OrganizationActions where a.OrganizationYear == referenceYear select a).ToList();
            List<OrganizationEvent> events = (from e in dataContext.OrganizationEvents where e.OrganizationYear == referenceYear select e).ToList();
            List<OrganizationContacts> contacts = (from c in dataContext.OrganizationContacts where c.OrganizationYear == referenceYear select c).ToList();
            List<OrganizationFollowup> allFollowups = (from f in dataContext.OrganizationFollowups where (f.OrganizationYear == referenceYear) select f).ToList();
            List<LookupAgencyStatus> statuses = (from f in dataContext.LookupAgencyStatuses select f).ToList();

            List<MainGridViewModel> orgs = (from db in dataContext.Organizations
                                            where (db.Year == referenceYear)
                                            select new MainGridViewModel()
                                            {
                                                //AgencyStatus = db.OrganizationFollowup.AgencyStatusCode,
                                                InstrumentId = db.InstrumentId,
                                                Agency = db.Name,
                                                State = db.State,
                                                OrganizationId = db.OrganizationId,
                                                Year = db.Year,
                                                ActionDue = actions.Where(p => p.OrganizationId == db.OrganizationId).OrderByDescending(p => p.ModifiedOn).FirstOrDefault(),
                                                IsTop150 = db.IsTop150,
                                                Events = events.Where(p => p.OrganizationId == db.OrganizationId && p.SetBy != "SYSTEM").OrderByDescending(o => o.EventDate).ToList(),
                                                Contacts = contacts.Where(p => p.OrganizationId == db.OrganizationId).ToList(),
                                                Followup = allFollowups.Where(p => p.OrganizationId == db.OrganizationId).FirstOrDefault(),
                                                Instrument = db.Instrument,
                                                SpecialCaseCode = db.SpecialCaseCode,
                                                LocalTime = ""
                                                //LocalTime = db.DisplayTime
                                            }).ToList();

            // Uncomment next line if only display cases for current user
            //var models = new List<MainGridViewModel>();

            foreach (var m in orgs)
            {
                m.AgencyStatus = statuses.Where(p => p.AgencyStatusCodeId == m.Followup.AgencyStatusCodeId).FirstOrDefault();

                // Uncomment following lines if only display cases for current user
                //if (currentUserRoleGroup == "AgencyLiaison" && m.Followup.AssignedTo == User.Identity.Name)
                //{
                //    models.Add(m);
                //}

                /*
                 * - jk 20190417
                 * Not sure what this is used for, but doesnt seem to be doing anything other than contributing to grid slowness 
                var resultParameter = new SqlParameter("@result", SqlDbType.VarChar);
                resultParameter.Size = 2000; // some meaningfull value
                resultParameter.Direction = ParameterDirection.Output;
                dataContext.Database.ExecuteSqlCommand("select @result = NROBDattempts FROM dbo.vwOrganizations_NROBD WHERE OrganizationId =" + m.OrganizationId.ToString() + " AND Year = " + m.Year.ToString(), resultParameter);
                m.NR_OBD = Convert.ToInt32(resultParameter.Value == DBNull.Value? 0 : resultParameter.Value);
                */

                //dataContext.Database.ExecuteSqlCommand("select @result = AssignedToNR FROM dbo.OrganizationFollowups WHERE OrganizationId =" + m.OrganizationId.ToString() + " AND OrganizationYear = " + m.Year.ToString(), resultParameter);
                //m.AssignedToNR = Convert.ToString(resultParameter.Value == DBNull.Value ? "" : resultParameter.Value);
            }

            ViewData["assignUsers"] = assignUsers.OrderBy(x => x).ToList() ;
            ViewData["specialCaseCodes"] = specialCaseCodes;
            ViewBag.currentUserRoleGroup = currentUserRoleGroup;
            // Uncomment next line if only display cases for current user
            //return View(gridType, currentUserRoleGroup == "AgencyLiaison" ? models : orgs);
          return View(gridType, orgs);
        }

        public IActionResult Agencies()
        {
            var sqlquery = "select g1.OrganizationId, g1.Year, g1.Name, g1.State, g1.Address, g1.City, g1.Zip, g1.Url, g1.IsTop150, g1.IsRegional, g1.IsPrivate, g1.InstrumentId,g1.OrganizationFollowupId, g1.OrganizationStatusId, g1.OrganizationTypeId,  " +
                           "g1.PasswordSecure, g1.UserName, g1.SpecialCaseCode from dbo.Organizations as g1 inner join " +
                           "(Select OrganizationId, State, max(Year) as Year " +
                           "from dbo.Organizations group by OrganizationId, State) as g2 " +
                           "on g2.OrganizationId = g1.OrganizationId AND g2.Year = g1.Year";

            List<Organization> orgs = dataContext.Organizations
                                      .FromSql(sqlquery)
                                      .ToList();

            return View(orgs);
        }

        public IActionResult CreateAgency()
        {
            AgencyProfileViewModel model = new AgencyProfileViewModel();
            model.Year = referenceYear;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAgency(AgencyProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                Organization org = new Organization
                {
                    OrganizationId = 123451672,
                    Year = model.Year,
                    Name = model.Agency,
                    Address = model.Address,
                    City = model.City,
                    State = model.State,
                    Zip = model.Zip,
                    Url = model.Url
                };
            }
            return RedirectToAction("Agency", "Management", new { id = 10956954, year = 2018 });
        }

        public IActionResult Agency(int id, int year)
        {

          AgencyProfileViewModel model = (from x in dataContext.Organizations.Include(x => x.OrganizationEvents).Include(x => x.Instrument)
                                            .Include(x => x.OrganizationContacts)
                                            where (x.OrganizationId == id && x.Year == year)
                                            select new AgencyProfileViewModel()
                                            {
                                                Agency = x.Name,
                                                OrganizationId = x.OrganizationId,
                                                Year = x.Year,
                                                Address = x.Address,
                                                City = x.City,
                                                State = x.State,
                                                Zip = x.Zip,
                                                Url = x.Url,
                                                IsTop150 = x.IsTop150,
                                                IsRegional = x.IsRegional,
                                                IsPrivate = x.IsPrivate,
                                                Events = x.OrganizationEvents,
                                               //LookupAgencyStatus = x.OrganizationFollowup.AgencyStatusCode,
                                                InstrumentId = x.InstrumentId,
                                                Instrument = x.Instrument,
                                                Followup = dataContext.OrganizationFollowups.Where(p => p.Organization.OrganizationId == x.OrganizationId && p.Organization.Year == x.Year).FirstOrDefault(),
                                                LookupAgencyStatus = dataContext.OrganizationFollowups.Where(p => p.Organization.OrganizationId == x.OrganizationId && p.Organization.Year == x.Year).FirstOrDefault().AgencyStatusCode,
                                                Contacts = x.OrganizationContacts.OrderByDescending(o => o.AgencyHead).ToList()
                                            }).FirstOrDefault();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Assignment()
        {
            var orgId = Int32.Parse(Request.Form["organizationId"]);
            var orgYear = Int32.Parse(Request.Form["year"]);
            var gridtype = Request.Form["gridtype"];
            var AssignedTo = Request.Form["AssignedTo"];
            var MaxAttempts = Int32.Parse(Request.Form["MaxAttempts"]);
            var MaxAttemptsNR = Int32.Parse(Request.Form["MaxAttemptsNR"]);
            var SpecialCaseCode = Int32.Parse(Request.Form["SpecialCaseCode"]);

            if (ModelState.IsValid)
            {
                Organization Org = dataContext.Organizations
                          .Where(p => p.OrganizationId == orgId && p.Year == orgYear).FirstOrDefault();
                OrganizationFollowup orgFollowup = dataContext.OrganizationFollowups
                          .Where(p => p.Organization.OrganizationId == orgId && p.Organization.Year == orgYear).FirstOrDefault();

                if (orgFollowup != null)
                {
                    switch (gridtype)
                    {
                        case "NR":
                            orgFollowup.AssignedToNR = AssignedTo;
                            orgFollowup.NRMaxAttempts = MaxAttemptsNR;
                            break;
                        case "Dqfu":
                            orgFollowup.AssignedTo = AssignedTo;
                            orgFollowup.MaxAttempts = MaxAttempts;
                            break;
                        default:
                            orgFollowup.AssignedTo = AssignedTo;
                            orgFollowup.MaxAttempts = MaxAttempts;
                            orgFollowup.AssignedToNR = Request.Form["AssignedToNR"];
                            orgFollowup.NRMaxAttempts = Int32.Parse(Request.Form["MaxAttemptsNR"]);
                            break;
                    }
                    orgFollowup.ModifiedBy = User.Identity.Name;
                    orgFollowup.ModifiedOn = DateTime.Now;

                    dataContext.SaveChanges();
                }

                if (Org.SpecialCaseCode != SpecialCaseCode)
                {
                    Org.SpecialCaseCode = SpecialCaseCode;
                    dataContext.SaveChanges();
                }
            }
            return RedirectToAction("Index", "Management", new { gridType = gridtype});
        }

        public IActionResult SendDQFUEmail(int organizationId, int year)
        {
            //email test
            AppIdentityUser user = _userManager.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            Utils.DQFUEmails.SendDQFUEmail(dataContext, organizationId, year, user.Email, emailSender, user);

            return RedirectToAction("AgencyProfile", "Client", new { id = organizationId, year = year });
        }

        /// <summary>
        /// Quick contact view, didn't create ViewModel as it is not necessary
        /// </summary>
        /// <returns></returns>
        public IActionResult QuickContact()
        {
            List<OrganizationContacts> ocList = dataContext.OrganizationContacts.Where(p => p.PrimaryContact == true || p.AgencyHead == true).Where(p => p.Organization.Year == this.referenceYear).Include(p => p.Organization).AsNoTracking().ToList();
            return View("QuickContact", ocList);
        }

        /// <summary>
        /// Create the ActionDue form
        /// </summary>
        /// <returns></returns>
        public IActionResult CreateActionDue(int organizationId)
        {
            Organization org = dataContext.Organizations.Where(p => p.OrganizationId == organizationId && p.Year == referenceYear).AsNoTracking().FirstOrDefault();
            OrganizationAction oa = new OrganizationAction();
            oa.Organization = org;
            //default Action Due Date to current date time
            oa.ActionDueDate = DateTime.Now;
            return View("ActionDue", oa);
        }

        /// <summary>
        /// get the action due details, not used
        /// </summary>
        /// <returns></returns>
        public string GetActionDue(int organizationId)
        {
            OrganizationAction oa = dataContext.OrganizationActions.Where(p => p.OrganizationId == organizationId && p.OrganizationYear == referenceYear).OrderByDescending(p => p.ModifiedOn).FirstOrDefault();
            if (oa != null)
            {
                String date = oa.ActionDueDate.ToShortDateString();
                String note = oa.ActionNotes;

                return date + " " + note;
            }
            else
                return null;
        }


        /// <summary>
        /// Edit the ActionDue form
        /// </summary>
        /// <returns></returns>
        public IActionResult EditActionDue(int id)
        {
            OrganizationAction oa = dataContext.OrganizationActions.Where(p => p.OrganizationActionId == id).Include(p=>p.Organization).FirstOrDefault();
            return View("ActionDue", oa);
        }

        /// <summary>
        /// Delete the ActionDue form
        /// </summary>
        /// <returns></returns>
        public IActionResult DeleteActionDue(int OrganizationActionId)
        {
            OrganizationAction oa = new OrganizationAction() { OrganizationActionId = OrganizationActionId };
            dataContext.OrganizationActions.Attach(oa);
            dataContext.OrganizationActions.Remove(oa);
            dataContext.SaveChanges();
            return Content("Your action has been deleted.");
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        public IActionResult SaveActionDue(OrganizationAction oa)
        {
            if (ModelState.IsValid)
            {
                if (oa.OrganizationActionId >0)
                {
                    OrganizationAction old = dataContext.OrganizationActions.Where(p => p.OrganizationActionId == oa.OrganizationActionId).FirstOrDefault();
                    old.ModifiedOn = DateTime.Now;
                    old.ModifiedBy = User.Identity.Name;
                    old.ActionDueDate = oa.ActionDueDate;
                    old.ActionNotes = oa.ActionNotes;
                    dataContext.OrganizationActions.Update(old);
                    dataContext.SaveChanges();
                }
                else
                {
                    //add new
                    oa.CreatedOn = DateTime.Now;
                    oa.CreatedBy = User.Identity.Name;
                    oa.ModifiedOn = DateTime.Now;
                    oa.ModifiedBy = User.Identity.Name;
                    oa.OrganizationYear = referenceYear;
                    dataContext.OrganizationActions.Attach(oa);
                    dataContext.SaveChanges();
                }
            }

            return RedirectToAction("Index");
        }
    }
}