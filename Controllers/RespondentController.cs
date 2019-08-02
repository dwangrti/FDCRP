using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ASJ.ViewModels.Respondent;
using ASJ.ViewModels.Client;
using ASJ.Models;
using ASJ.ClassObjects;
using static ASJ.Startup;
using ASJ.Utils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;
using ASJ.Models.Form;

namespace ASJ.Controllers
{
    [Authorize]
    public class RespondentController : BaseController
    {
        private ASJDbContext dataContext;
        private ASJLegacyContext legacyContext;
        public int referenceYear;
        private readonly ApplicationVariables _appVariables;
        private readonly IHostingEnvironment _hostingEnvironment;

        public RespondentController(ASJDbContext dbContext, ASJLegacyContext legContext, IHostingEnvironment HostingEnvironment, IOptions<ApplicationVariables> appVariables)
        {
            this.dataContext = dbContext;
            legacyContext = legContext;
            _appVariables = appVariables.Value;
            this.referenceYear = Int32.Parse(_appVariables.ReferenceYear);
            _hostingEnvironment = HostingEnvironment;

        }

        public IActionResult Index()
        {   
            List<PocViewModel> models = (from x in dataContext.Organizations
                         .Include(x => x.OrganizationType).Include(x => x.Responses).Include(x => x.Responses)
                         where (x.UserName == @User.Identity.Name)
                                  select new PocViewModel()
                                  {
                                        Agency = x.Name,
                                        OrganizationId = x.OrganizationId,
                                        Year = x.Year,
                                        State = x.State,
                                        UserName = x.UserName,
                                        //LookupAgencyStatus = x.OrganizationFollowup.AgencyStatusCode,
                                        //Followup = x.OrganizationFollowup,
                                        OrganizationType = x.OrganizationType
                                  }).OrderByDescending(o => o.Year).ToList();

            foreach (var m in models)
            {
                m.Followup = dataContext.OrganizationFollowups.Where(p => p.Organization.OrganizationId == m.OrganizationId && p.Organization.Year == m.Year).FirstOrDefault();
                m.LookupAgencyStatus = m.Followup.AgencyStatusCode;
                //set the responses for the model by getting the instrument id based on the org type
                InstrumentOrganizationType instr = dataContext.InstrumentOrganizationTypes.FirstOrDefault(x => x.InstrumentYear == Convert.ToInt32(m.Year) && x.OrganizationTypeId == Convert.ToInt32(m.OrganizationType.OrganizationTypeId));
                if (instr != null)
                {
                    m.InstrumentId = instr.InstrumentId;
                    m.Response = dataContext.Responses.FirstOrDefault(p => p.Organization.OrganizationId == m.OrganizationId && p.InstrumentId == instr.InstrumentId && p.ResponseVariable == "iDQFUFlag");
                }

                

                switch (m.Year)
                {
                    case 2015:
                        var asjForm2015 = legacyContext.AsjAnnual2015
                              .Where(o => o.OrganizationId == m.OrganizationId).FirstOrDefault();
                        if (asjForm2015 != null)
                        {
                            m.dataStatus = Extensions.dataStatusDesc(asjForm2015.FormStatus);
                            if (FormController.GetLastPage(this.dataContext, m.InstrumentId, m.OrganizationId, m.Year) >= 15)
                                m.dataStatus = "Final";
                        }
                        else
                        {
                            m.dataStatus = "Not Started";
                        }
                        break;

                    case 2016:
                        var asjForm2016 = legacyContext.AsjAnnual2016
                              .Where(o => o.OrganizationId == m.OrganizationId).FirstOrDefault();
                        if (asjForm2016 != null)
                        {
                            m.dataStatus = Extensions.dataStatusDesc(asjForm2016.FormStatus);
                            if (FormController.GetLastPage(this.dataContext, m.InstrumentId, m.OrganizationId, m.Year) >= 15)
                                m.dataStatus = "Final";
                        }
                        else
                        {
                            m.dataStatus = "Not Started";
                        }
                        break;

                    case 2017:
                        var asjForm2017 = legacyContext.AsjAnnual2017
                              .Where(o => o.OrganizationId == m.OrganizationId).FirstOrDefault();
                        if (asjForm2017 != null)
                        {
                            m.dataStatus = Extensions.dataStatusDesc(asjForm2017.FormStatus);
                            if (FormController.GetLastPage(this.dataContext, m.InstrumentId, m.OrganizationId, m.Year) >= 15)
                                m.dataStatus = "Final";
                        }
                        else
                        {
                            m.dataStatus = "Not Started";
                        }
                        break;

                    case 2018:
                        var asjForm2018 = dataContext.PDFAnnualASJs
                              .Where(o => o.OrganizationId == m.OrganizationId).FirstOrDefault();
                        if (asjForm2018 != null)
                        {
                            m.dataStatus = Extensions.dataStatusDesc(asjForm2018.form_status);
                            if (FormController.GetLastPage(this.dataContext, m.InstrumentId, m.OrganizationId, m.Year) >= 15)
                                m.dataStatus = "Final";
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
                            if (FormController.GetLastPage(this.dataContext, m.InstrumentId, m.OrganizationId, m.Year) >= 15)
                                m.dataStatus = "Final";
                        }
                        else
                        {
                            m.dataStatus = "Not Started";
                        }
                        break;
                }

                //do something else for cognitive
                if(m.OrganizationType.Description.Contains("Cognitive"))
                {
                    string sql = "SELECT * FROM dbo.pivot_responses_cogtest WHERE organizationid = " + m.OrganizationId.ToString();
                    var cogForm = (from pd in dataContext.PDFCognitives
                                   .FromSql(sql)
                                   select new DataCollectionViewModel()
                                   {
                                       OrganizationId = m.OrganizationId,
                                       Year = m.Year,
                                       InstrumentId = m.InstrumentId,
                                       form_status = "In Progress",
                                   }).FirstOrDefault();

                    if (cogForm != null)
                    {
                        m.dataStatus = "In Progress";

                        //hardcoding the last page number and a new status
                        //get last page of the instrument
                        int maxPage = dataContext.InstrumentQuestions.OrderByDescending(p => p.Page).Where(p => p.Instrument.InstrumentId == m.InstrumentId).AsNoTracking().FirstOrDefault().Page;
                        if (FormController.GetLastPage(this.dataContext, m.InstrumentId, m.OrganizationId, m.Year) >= maxPage)
                            m.dataStatus = "Final";
                    }
                    else
                    {
                        m.dataStatus = "Not Started";
                    }
                }
                ViewBag.OrganizationID = m.OrganizationId;
                if (m.Year == referenceYear)
                    ViewBag.InstrumentId = m.InstrumentId;
            }

            Organization org = (from o in dataContext.Organizations
                                where o.OrganizationId == models[0].OrganizationId
                                    && o.Year == referenceYear
                                select o).FirstOrDefault();
            Instrument inst = (from i in dataContext.Instruments
                               where i.InstrumentId == org.InstrumentId
                               select i).FirstOrDefault();

            ASJ.ViewModels.Form.DataSupplierViewModel dsbvm = new ASJ.ViewModels.Form.DataSupplierViewModel();
            dsbvm.Organization = org;
            dsbvm.Instrument = inst;

            var ds = dataContext.DataSuppliers.Where(s => s.Organization == org && s.Instrument == inst)
                .FirstOrDefault();

            if (ds != null)
            {
                dsbvm.DataSupplierId = ds.DataSupplierId;
                dsbvm.Address = ds.Address;
                dsbvm.City = ds.City;
                dsbvm.email = ds.email;
                dsbvm.Fax = ds.Fax;
                dsbvm.Name = ds.Name;
                dsbvm.Phone = ds.Phone;
                dsbvm.State = ds.State;
                dsbvm.Title = ds.Title;
                dsbvm.Zip = ds.Zip;
                dsbvm.FacilityName = ds.FacilityName;
            }

            ViewBag.DatasupplierVM = dsbvm;

            ViewBag.referenceYear = referenceYear;
           
            return View(models);
        }

        public FileResult ExportPDF(int instrumentId, int organizationId, int year)
        {
           PDFContainer pdf = new PDFContainer(instrumentId, organizationId, year, dataContext, legacyContext, _hostingEnvironment);
                      
            return File(pdf.GetPDFData(), "application/pdf", pdf.downFileName);
        }
    }
}