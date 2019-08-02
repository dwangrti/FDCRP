using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ASJ.Models;
using ASJ.Models.Form;
using ASJ.ViewModels.Event;
using ASJ.ViewModels.Management;
using ASJ.Services;
using ASJ.Utils;
using static ASJ.Startup;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;

namespace ASJ.Controllers
{
    [Authorize]
    public class EventController : BaseController
    {
        private ASJDbContext dataContext;
        private IEmailSender emailSender;
        public int referenceYear;
        private readonly ApplicationVariables _appVariables;

        public EventController(ASJDbContext dbContext, IEmailSender emailSender, IOptions<ApplicationVariables> appVariables)
        {
            this.dataContext = dbContext;
            this.emailSender = emailSender; 
            _appVariables = appVariables.Value;
            this.referenceYear = Int32.Parse(_appVariables.ReferenceYear);
        }
        public IActionResult Index()
        {
            //var sqlquery = "select g1.OrganizationId, g1.Year, g1.Name, g1.State, g1.Address, g1.City, g1.Zip, g1.Url, g1.IsTop150, g1.IsRegional, g1.IsPrivate, g1.InstrumentId,g1.OrganizationFollowupId, g1.OrganizationStatusId, g1.OrganizationTypeId,  " +
            //               "g1.PasswordSecure, g1.UserName, g1.SpecialCaseCode from dbo.Organizations as g1 inner join " +
            //               "(Select OrganizationId, State, max(Year) as Year " +
            //               "from dbo.Organizations group by OrganizationId, State) as g2 " +
            //               "on g2.OrganizationId = g1.OrganizationId AND g2.Year = g1.Year";

            List<MainGridViewModel> orgs = (from db in dataContext.Organizations
                                      //.FromSql(sqlquery)
                                      .Include(x => x.OrganizationEvents)
                                      .Include(x => x.OrganizationContacts).Include(x => x.Instrument)
                                            where (db.Year == referenceYear)
                                            select new MainGridViewModel()
                                            {
                                                //AgencyStatus = db.OrganizationFollowup.AgencyStatusCode,
                                                InstrumentId = db.InstrumentId,
                                                Agency = db.Name,
                                                State = db.State,
                                                OrganizationId = db.OrganizationId,
                                                Year = db.Year,
                                                ActionDue = dataContext.OrganizationActions.Where(p => p.Organization.OrganizationId == db.OrganizationId).OrderByDescending(p => p.ModifiedOn).FirstOrDefault(),
                                                IsTop150 = db.IsTop150,
                                                Events = db.OrganizationEvents.OrderByDescending(o => o.EventDate).ToList(),
                                                Contacts = db.OrganizationContacts,
                                                Instrument = db.Instrument,
                                                //Followup = db.OrganizationFollowup
                                            }).ToList();
            foreach (var m in orgs)
            {
                m.Followup = dataContext.OrganizationFollowups.Where(p => p.Organization.OrganizationId == m.OrganizationId && p.Organization.Year == m.Year).FirstOrDefault();
                m.AgencyStatus = dataContext.LookupAgencyStatuses.Where(p => p.AgencyStatusCodeId == m.Followup.AgencyStatusCodeId).FirstOrDefault();
            }
            return View(orgs);
        }

        public IActionResult EventsTracker(int organizationId, int year)
        {
            List<LookupEvent> events = (from x in dataContext.LookupEvents
                                        select new LookupEvent()
                                        {
                                            EventId = x.EventId,
                                            CodesEventText = x.CodesEventText,
                                            isActive = x.isActive
                                        })
                                        .Where(x => x.isActive == true)
                                        .ToList();

            List<OrganizationNotes> caseNotes = (from x in dataContext.OrganizationNotes
                                                 where (x.Organization.OrganizationId == organizationId)
                                                 select new OrganizationNotes()
                                                 {
                                                     Note = x.Note,
                                                     CreatedBy = x.CreatedBy,
                                                     CreatedOn = x.CreatedOn,
                                                     ModifiedBy = x.ModifiedBy,
                                                     ModifiedOn = x.ModifiedOn
                                                 }).OrderByDescending(o => o.CreatedOn).ToList();

            List<EventsTrackerViewModel> years = (from x in dataContext.OrganizationEvents
                                                  where (x.Organization.OrganizationId == organizationId)
                                                  select new EventsTrackerViewModel()
                                                  {
                                                      EventYear = x.EventDate.Year
                                                  }).Distinct().ToList();

            var EventYears = new List<string>();
            foreach (var evt in years)
            {
                EventYears.Add(evt.EventYear.ToString());
            }
            string[] filter_years = EventYears.ToArray();

            EventFilters filters = new EventFilters()
            {
                year_options = filter_years,
                filter_year = filter_years,
                filter_system = "no"
            };
            var sqlquery = "select * from dbo.OrganizationEvents as g where g.OrganizationId = " + organizationId + " and (g.SetBy != 'SYSTEM' OR g.SetBy is NULL)";

            //EventsTrackerViewModel model = new EventsTrackerViewModel();
            List<EventsTrackerViewModel> models = (from n in dataContext.OrganizationEvents
                                            .FromSql(sqlquery)
                                            .Include(n => n.LookupEvent)
                                            .Where(r => filters.filter_year.Any(f => r.EventDate.Year.ToString().Equals(f)))
                                                   select new EventsTrackerViewModel()
                                                   {
                                                       OrganizationEventId = n.OrganizationEventId,
                                                       SetBy = n.SetBy,
                                                       EventDate = n.EventDate,
                                                       EventNotes = n.EventNotes,
                                                       CreatedBy = n.CreatedBy,
                                                       CreatedOn = n.CreatedOn,
                                                       ModifiedBy = n.ModifiedBy,
                                                       ModifiedOn = n.ModifiedOn,
                                                       Event = n.LookupEvent,
                                                       EventId = n.EventId,
                                                       ActionDue = dataContext.OrganizationActions.Where(p => p.Organization.OrganizationId == organizationId).OrderByDescending(p => p.ModifiedOn).FirstOrDefault(),
                                                       Organization = n.Organization
                                                   }).OrderByDescending(o => o.EventDate).ToList();

            Organization model = Extensions.GetOrganization(dataContext, organizationId, year);
            OrganizationFollowup followup = dataContext.OrganizationFollowups.Where(p => p.Organization.OrganizationId == organizationId && p.Organization.Year == year).FirstOrDefault();
            OrganizationContacts primaryContact = dataContext.OrganizationContacts.Where(p => p.Organization.OrganizationId == organizationId && p.Organization.Year == year && p.PrimaryContact == true).FirstOrDefault();
            DataSupplier dataSupplier = dataContext.DataSuppliers.Where(p => p.Organization.OrganizationId == organizationId && p.Organization.Year == year).FirstOrDefault();

            ViewBag.SupervisorAlert = followup?.SupervisorAlert;
            ViewBag.POC = primaryContact;
            ViewBag.DSB = dataSupplier;
            ViewData["Organization"] = model;
            ViewData["EventFilters"] = filters;
            ViewData["EventList"] = events;
            ViewData["CaseNotes"] = caseNotes;
             
            return View(models);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        // ToDos - Refactor to keep DRY with the EventTracker action above
        public IActionResult EventsFilter(int organizationId, int year)
        {
            List<LookupEvent> events = (from x in dataContext.LookupEvents
                                        select new LookupEvent()
                                        {
                                            EventId = x.EventId,
                                            CodesEventText = x.CodesEventText
                                        }).ToList();

            List<OrganizationNotes> caseNotes = (from x in dataContext.OrganizationNotes
                                                 where (x.Organization.OrganizationId == organizationId)
                                                 select new OrganizationNotes()
                                                 {
                                                     Note = x.Note,
                                                     CreatedBy = x.CreatedBy,
                                                     CreatedOn = x.CreatedOn,
                                                     ModifiedBy = x.ModifiedBy,
                                                     ModifiedOn = x.ModifiedOn
                                                 }).OrderByDescending(o => o.CreatedOn).ToList();


            List<EventsTrackerViewModel> years = (from x in dataContext.OrganizationEvents
                                                  where (x.Organization.OrganizationId == organizationId)
                                                  select new EventsTrackerViewModel()
                                                  {
                                                      EventYear = x.EventDate.Year
                                                  }).Distinct().ToList();

            var EventYears = new List<string>();
            foreach (var evt in years)
            {
                EventYears.Add(evt.EventYear.ToString());
            }
            //string[] filter_years = EventYears.ToArray();

            EventFilters filters = new EventFilters()
            {
                year_options = EventYears.ToArray(),
                filter_year = Request.Form["filter_year"],
                filter_system = Request.Form["filter_system"]
            };

            var sqlquery = "select * from dbo.OrganizationEvents as g where g.OrganizationId = " + organizationId;
            if (filters.filter_system == "no")
            {
                sqlquery += " and (g.SetBy != 'SYSTEM' OR g.SetBy is NULL)";
            }

            List<EventsTrackerViewModel> models = (from n in dataContext.OrganizationEvents
                                                .FromSql(sqlquery)
                                                .Include(n => n.LookupEvent)
                                                .Where(r => filters.filter_year.Any(f => r.EventDate.Year.ToString().Equals(f)))
                                                   select new EventsTrackerViewModel()
                                                   {
                                                       OrganizationEventId = n.OrganizationEventId,
                                                       SetBy = n.SetBy,
                                                       EventDate = n.EventDate,
                                                       EventNotes = n.EventNotes,
                                                       CreatedBy = n.CreatedBy,
                                                       CreatedOn = n.CreatedOn,
                                                       ModifiedBy = n.ModifiedBy,
                                                       ModifiedOn = n.ModifiedOn,
                                                       Event = n.LookupEvent,
                                                       EventId = n.EventId,
                                                       Organization = n.Organization
                                                   }).OrderByDescending(o => o.EventDate).ToList();

            Organization model = Extensions.GetOrganization(dataContext, organizationId, year);
            OrganizationFollowup followup = dataContext.OrganizationFollowups.Where(p => p.Organization.OrganizationId == organizationId && p.Organization.Year == year).FirstOrDefault();
            OrganizationContacts primaryContact = dataContext.OrganizationContacts.Where(p => p.Organization.OrganizationId == organizationId && p.Organization.Year == year && p.PrimaryContact == true).FirstOrDefault();
            DataSupplier dataSupplier = dataContext.DataSuppliers.Where(p => p.Organization.OrganizationId == organizationId && p.Organization.Year == year).FirstOrDefault();

            ViewBag.SupervisorAlert = followup?.SupervisorAlert;
            ViewBag.POC = primaryContact;
            ViewBag.DSB = dataSupplier;

            ViewData["Organization"] = model;
            ViewData["EventFilters"] = filters;
            ViewData["EventList"] = events;
            ViewData["CaseNotes"] = caseNotes;

            return View("EventsTracker", models);
        }

        public IActionResult EventEdit(int OrgEventId)
        {
            //EventsTrackerViewModel model = new EventsTrackerViewModel();
            EventsTrackerViewModel model = (from n in dataContext.OrganizationEvents
                                            .Include(n => n.LookupEvent)
                                            where n.OrganizationEventId == OrgEventId
                                            select new EventsTrackerViewModel()
                                            {
                                                OrganizationEventId = n.OrganizationEventId,
                                                SetBy = n.SetBy,
                                                EventDate = n.EventDate,
                                                EventNotes = n.EventNotes,
                                                CreatedBy = n.CreatedBy,
                                                CreatedOn = n.CreatedOn,
                                                ModifiedBy = n.ModifiedBy,
                                                ModifiedOn = n.ModifiedOn,
                                                Event = n.LookupEvent,
                                                Organization = n.Organization
                                            }).FirstOrDefault();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EventEdit(EventsTrackerViewModel model)
        {
            if (ModelState.IsValid)
            {
            }
            return RedirectToAction("EventsTracker", "Event", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EventAdd()
        {
            var orgId = Int32.Parse(Request.Form["OrganizationId"]);
            var orgYear = Int32.Parse(Request.Form["Year"]);
            var notes = Request.Form["Notes"];
            var eventCode = Int32.Parse(Request.Form["codeDesc"]);

            if (ModelState.IsValid)
            {
                var org = Extensions.GetOrganization(dataContext, orgId, orgYear);

                // Save Event record
                var OrgEvent = new OrganizationEvent()
                {
                    CreatedBy = User.Identity.Name,
                    CreatedOn = DateTime.Now,
                    EventDate = DateTime.Now,
                    EventNotes = notes,
                    ModifiedBy = User.Identity.Name,
                    ModifiedOn = DateTime.Now,
                    Organization = org,
                    SetBy = User.Identity.Name,
                    EventId = eventCode
                };
                dataContext.Add(OrgEvent);
                dataContext.SaveChanges();

            }
            return RedirectToAction("EventsTracker", "Event", new { organizationId = orgId, year = orgYear });
        }

        public IActionResult Events(int organizationId, int year)
        {
            List<EventsTrackerViewModel> models = (from n in dataContext.OrganizationEvents
                                                   where (n.Organization.OrganizationId == organizationId && n.Organization.Year == year)
                                                   select new EventsTrackerViewModel()
                                                   {
                                                       OrganizationEventId = n.OrganizationEventId,
                                                       SetBy = n.SetBy,
                                                       EventDate = n.EventDate,
                                                       EventNotes = n.EventNotes,
                                                       CreatedBy = n.CreatedBy,
                                                       CreatedOn = n.CreatedOn,
                                                       ModifiedBy = n.ModifiedBy,
                                                       ModifiedOn = n.ModifiedOn,
                                                       Event = n.LookupEvent,
                                                       EventId = n.EventId,
                                                       Organization = n.Organization
                                                   }).OrderByDescending(o => o.EventDate).ToList();

            Organization model = Extensions.GetOrganization(dataContext, organizationId, year);

            ViewData["Organization"] = model;

            return View(models);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NotesAdd()
        {
            var orgId = Int32.Parse(Request.Form["OrganizationId"]);
            var orgYear = Int32.Parse(Request.Form["Year"]);
            var notes = Request.Form["Notes"];

            if (ModelState.IsValid)
            {
                var org = Extensions.GetOrganization(dataContext, orgId, orgYear);

                // Save Event record
                var OrgNotes = new OrganizationNotes()
                {
                    CreatedBy = User.Identity.Name,
                    CreatedOn = DateTime.Now,
                    Note = notes,
                    ModifiedBy = User.Identity.Name,
                    ModifiedOn = DateTime.Now,
                    Organization = org,
                };
                dataContext.Add(OrgNotes);
                dataContext.SaveChanges();

            }
            return RedirectToAction("EventsTracker", "Event", new { organizationId = orgId, year = orgYear });
        }
        [HttpPost]
        public JsonResult AlertUpdate([FromBody]OrgData Orgdata)
        {
            var orgId = Int32.Parse(Orgdata.id);
            var orgYear = Int32.Parse(Orgdata.year);
            var s_alert = Orgdata.super_alert;

            OrganizationFollowup followup = dataContext.OrganizationFollowups.Where(p => p.Organization.OrganizationId == orgId && p.Organization.Year == orgYear).FirstOrDefault();
            if (followup != null)
            {
                followup.SupervisorAlert = s_alert;
                dataContext.Update(followup);
                dataContext.SaveChanges();
            }

            return Json(Orgdata);
        }

        public class OrgData
        {
            public string id { get; set; }
            public string year { get; set; }
            public Boolean super_alert { get; set; }
        }

    }
}