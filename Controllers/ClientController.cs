using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ASJ.ViewModels.Client;
using Microsoft.EntityFrameworkCore;
using ASJ.Models;
using ASJ.Models.PDF;
using static ASJ.Startup;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using CsvHelper;
using System.IO;
using ASJ.Utils;
using Microsoft.AspNetCore.Authorization;
using ASJ.Services;
using Microsoft.AspNetCore.Identity;
using ASJ.ClassObjects;

namespace ASJ.Controllers
{
    [Authorize]
    public class ClientController : BaseController
    {
        private ASJDbContext dataContext;
        private ASJLegacyContext legacyContext;
        public int referenceYear;
        private readonly ApplicationVariables _appVariables;
        private readonly UserManager<AppIdentityUser> _userManager;
        private readonly IHostingEnvironment _hostingEnvironment;


        public ClientController(ASJDbContext dbContext, ASJLegacyContext legContext, IOptions<ApplicationVariables> appVariables, UserManager<AppIdentityUser> userManager, IHostingEnvironment HostingEnvironment)
        {
            this.dataContext = dbContext;
            this.legacyContext = legContext;
            _appVariables = appVariables.Value;
            this.referenceYear = Int32.Parse(_appVariables.ReferenceYear);
            this._userManager = userManager;
            _hostingEnvironment = HostingEnvironment;
        }

        public IActionResult Index()
        {
            List<OrganizationFollowup> allFollowups = (from f in dataContext.OrganizationFollowups where (f.OrganizationYear == referenceYear) select f).ToList();
            List<Organization> orgs = (from n in dataContext.Organizations
                                       where (n.Year == referenceYear)
                                       select new Organization()
                                       {
                                           OrganizationId = n.OrganizationId,
                                           Name = n.Name,
                                           State = n.State,
                                           Year = n.Year,
                                           OrganizationFollowup = allFollowups.Where(p => p.OrganizationId == n.OrganizationId).FirstOrDefault()
                                       }).OrderBy(x => x.Name).ToList();

                                    //group n by new { n.OrganizationId, n.Name, n.State } into g
                                    //select new Organization()
                                    //{
                                    //    OrganizationId = g.Key.OrganizationId,
                                    //    Name = g.Key.Name,
                                    //    State = g.Key.State,
                                    //    Year = g.Max(t => t.Year)
                                    //}).OrderBy(x => x.Name).ThenByDescending(o => o.Year).ToList();


            return View(orgs);
        }

        public IActionResult DcsReports()
        {
            return View();
        }

        public FileResult ExportCSVData()
        {
            MemoryStream m = new MemoryStream();
            StreamWriter write = new StreamWriter(m);
            //System.IO.TextWriter tw = new System.IO.TextWriter();
            
            var records = dataContext.PDFAnnualASJs
                .ToList();

            var csv = new CsvWriter(write);
            csv.WriteRecords(records);
                       
            
            return File(m.ToArray(), "text/csv", "datafile_asj_2018.csv");

        }

        public IActionResult AgencyProfile(int id)
        {
            //string sqlquery = "SELECT * FROM dbo.vwOrganizationsGridData WHERE OrganizationId = " + id;
            //List<AgencyProfileViewModel> models = (from x in dataContext.Organizations
            //                                .FromSql(sqlquery)
            //                                 .Include(x => x.OrganizationFacility)
            List<AgencyProfileViewModel> models = (from x in dataContext.Organizations.Include(x => x.OrganizationEvents).Include(x => x.Instrument)
                                            .Include(x => x.OrganizationContacts).Include(x => x.Responses)
                                            .Include(x => x.OrganizationFacility)
                                                   where (x.OrganizationId == id)
                                                   select new AgencyProfileViewModel()
                                            {
                                                Agency = x.Name,
                                                OrganizationId = x.OrganizationId,
                                                Year = x.Year,
                                                State = x.State,
                                                Events = x.OrganizationEvents,
                                                //LookupAgencyStatus = x.OrganizationFollowup.AgencyStatusCode,
                                                InstrumentId = x.InstrumentId,
                                                Instrument = x.Instrument,
                                                Contacts = x.OrganizationContacts.OrderByDescending(o => o.AgencyHead).ToList(),
                                                Response = x.Responses.FirstOrDefault(p => p.Organization.OrganizationId == x.OrganizationId && p.InstrumentId == x.InstrumentId && p.ResponseVariable == "iDQFUFlag"),
                                                Reasons = x.Responses.Where(p => p.Organization.OrganizationId == x.OrganizationId && p.InstrumentId == x.InstrumentId && p.ResponseVariable.Contains("explanation_") && p.ResponseValue != null).Count(),
                                                QCDetails = dataContext.OrganizationQCDetails.Where(p => p.Organization.OrganizationId == x.OrganizationId && p.Organization.Year == x.Year).ToList(),
                                                UserName = x.UserName,
                                                Password = x.PasswordSecure,
                                                Facilities = x.OrganizationFacility,
                                                SpecialCaseCode = x.SpecialCaseCode
                                            }).OrderByDescending(o => o.Year).ToList();

            List<OrganizationEvent> Events = new List<OrganizationEvent>();
            foreach (var m in models)
            {
                m.Followup = dataContext.OrganizationFollowups.Where(p => p.Organization.OrganizationId == m.OrganizationId && p.Organization.Year == m.Year).FirstOrDefault();
                m.LookupAgencyStatus = m.Followup.AgencyStatusCode;
                foreach (var evt in m.Events)
                {
                    if (evt.SetBy != "SYSTEM") { 
                      Events.Add(evt);
                    }
                }
                AppIdentityUser user = _userManager.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
                m.LoggedInUserEmail = user.Email;
            }
            List<OrganizationNotes> caseNotes = (from x in dataContext.OrganizationNotes
                                                 where (x.Organization.OrganizationId == id)
                                                 select new OrganizationNotes()
                                                 {
                                                     Note = x.Note,
                                                     CreatedBy = x.CreatedBy,
                                                     CreatedOn = x.CreatedOn,
                                                     ModifiedBy = x.ModifiedBy,
                                                     ModifiedOn = x.ModifiedOn
                                                 }).OrderByDescending(o => o.CreatedOn).ToList();

            ViewData["Events"] = Events.OrderByDescending(o => o.EventDate).ToList();
            ViewData["CaseNotes"] = caseNotes;

            // Populating the form status description
            foreach (var m in models)
            {
                switch (m.Year)
                {
                    case 2015:
                        var asjForm2015 = legacyContext.AsjAnnual2015
                              .Where(o => o.OrganizationId == m.OrganizationId).FirstOrDefault();
                        if (asjForm2015 != null)
                        {
                            m.dataStatus = Extensions.dataStatusDesc(asjForm2015.FormStatus);
                            m.ASJStatusCode = asjForm2015.AsjstatusCode;
                            m.ASJQualityCode = asjForm2015.AsjqualityCode;
                            m.ModifiedDate = asjForm2015.ModifiedDate;
                        }
                        else
                        {
                            m.dataStatus = "Not Started";
                            m.ASJStatusCode = null;
                            m.ASJQualityCode = null;
                            m.ModifiedDate = new DateTime();
                        }
                        break;

                    case 2016:
                        var asjForm2016 = legacyContext.AsjAnnual2016
                              .Where(o => o.OrganizationId == m.OrganizationId).FirstOrDefault();
                        if (asjForm2016 != null)
                        {
                            m.dataStatus = Extensions.dataStatusDesc(asjForm2016.FormStatus);
                            m.ASJStatusCode = asjForm2016.AsjstatusCode;
                            m.ASJQualityCode = asjForm2016.AsjqualityCode;
                            m.ModifiedDate = asjForm2016.ModifiedDate;
                        }
                        else
                        {
                            m.dataStatus = "Not Started";
                            m.ASJStatusCode = null;
                            m.ASJQualityCode = null;
                            m.ModifiedDate = new DateTime();
                        }
                        break;

                    case 2017:
                        var asjForm2017 = legacyContext.AsjAnnual2017
                              .Where(o => o.OrganizationId == m.OrganizationId).FirstOrDefault();
                        if (asjForm2017 != null)
                        {
                            m.dataStatus = Extensions.dataStatusDesc(asjForm2017.FormStatus);
                            m.ASJStatusCode = asjForm2017.AsjstatusCode;
                            m.ASJQualityCode = asjForm2017.AsjqualityCode;
                            m.ModifiedDate = asjForm2017.ModifiedDate;
                        }
                        else
                        {
                            m.dataStatus = "Not Started";
                            m.ASJStatusCode = null;
                            m.ASJQualityCode = null;
                            m.ModifiedDate = new DateTime();
                        }
                        break;

                    case 2018:
                        var asjForm2018 = dataContext.PDFAnnualASJs
                              .Where(o => o.OrganizationId == m.OrganizationId).FirstOrDefault();
                        if (asjForm2018 != null)
                        {
                            m.dataStatus = Extensions.dataStatusDesc(asjForm2018.form_status);
                            m.ASJStatusCode = m.Followup.ASJStatusCode;
                            m.ASJQualityCode = m.Followup.ASJQualityCode;
                        }
                        else
                        {
                            m.dataStatus = "Not Started";
                        }
                        break;

                    case 2019:
                        var asjForm2019 = dataContext.PDFAnnualASJs
                              .Where(o => o.OrganizationId == m.OrganizationId).FirstOrDefault();
                        if (asjForm2019 != null)
                        {
                            m.dataStatus = Extensions.dataStatusDesc(asjForm2019.form_status);
                            m.ASJStatusCode = m.Followup.ASJStatusCode;
                            m.ASJQualityCode = m.Followup.ASJQualityCode;
                        }
                        else
                        {
                            m.dataStatus = "Not Started";
                        }
                        break;
                }
            }

            ViewBag.referenceYear = referenceYear;

            DataSupplier dataSupplier = dataContext.DataSuppliers.Where(p => p.Organization.OrganizationId == id).OrderByDescending(o => o.Organization.Year).FirstOrDefault();
            ViewBag.DSB = dataSupplier;

            return View(models);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AgencyProfile(AgencyProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
            }
            return RedirectToAction("AgencyProfile", "Client", model);
        }
    }
}