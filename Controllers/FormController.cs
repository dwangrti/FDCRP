using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using ASJ.Models;
using ASJ.Services;
using ASJ.Models.Form;
using ASJ.ViewModels.Form;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using ASJ.Utils;
using Microsoft.AspNetCore.Authorization;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http.Extensions;
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;
using Microsoft.AspNetCore.Identity;

namespace ASJ.Controllers
{
    [Authorize]
    public class FormController : BaseController
    {
        private ASJDbContext dataContext;
        private IEmailSender emailSender;
        private readonly UserManager<AppIdentityUser> _userManager;

        private const int CJ5_INSTRUMENT_ID = 1;
        private const int COGTEST_INSTRUMENT_ID = 3;

        public FormController(ASJDbContext dbContext, IEmailSender emailSender, UserManager<AppIdentityUser> userManager)
        {
            this.dataContext = dbContext;
            this.emailSender = emailSender;
            _userManager = userManager;

        }


        public IActionResult Instructions(int instrumentId, int organizationId, int year)
        {
            Organization org = (from o in dataContext.Organizations
                                where o.OrganizationId == organizationId
                                  && o.Year == year
                                select o).FirstOrDefault();

            OrganizationContacts orgcontact = (from o in dataContext.OrganizationContacts
                                where o.OrganizationId == organizationId
                                  && o.OrganizationYear == year
                                select o).FirstOrDefault();

            ViewBag.OrgType = org.OrganizationTypeId;
            ViewBag.InstrumentId = instrumentId;
            ViewBag.OrganizationId = organizationId;
            ViewBag.OrgORI = organizationId*10 + 1;
            ViewBag.Name = org.Name;
            ViewBag.PIN = org.PIN;
            ViewBag.Lname = orgcontact.LastName;
            ViewBag.Year = year;

            return View();
        }

        public IActionResult Index(int instrumentId, int organizationId, int year, int page = 1, Boolean resume = false, Boolean RTIdataEntry = false)
        {
            //security check
            if (!Utils.Extensions.RespondentSecurityCheck(User, organizationId))
                return RedirectToAction("AccessDenied", "Security");
            InstrumentViewModel instVM = new InstrumentViewModel();

            if (instrumentId != 0 && organizationId != 0 && year != 0)
            {
                //get the instrument
                instVM.Instrument = dataContext.Instruments.AsNoTracking().SingleOrDefault(p => p.InstrumentId == instrumentId);
                //set the current organization and year
                instVM.OrganizationId = organizationId;
                instVM.Year = year;
                instVM.formStatus = Extensions.CheckFormStarted(dataContext, organizationId, year, instrumentId);
                //since this is the first time on the page set the current page to 1
                instVM.SelectedPage = page;
                instVM.RTIDataEntry = RTIdataEntry;
                if(!User.IsInRole("poc"))
                {
                    instVM.RTIDataEntry = true;

                }

             
                //if resume, we need to find the page number where the user left off by the last ModifiedOn timestamp
                if (resume)
                {
                    int lastPage = GetLastPage(this.dataContext, instrumentId, organizationId, year);
                    //  int lastPage = GetLastPage(dataContext, instrumentId, organizationId);
                    int maxPage = dataContext.InstrumentQuestions.OrderByDescending(p => p.Page).Where(p => p.Instrument.InstrumentId == instrumentId).AsNoTracking().FirstOrDefault().Page;
                    if (lastPage > 0)
                    {
                        
                        if(lastPage == maxPage)
                            instVM.SelectedPage = lastPage; //setting to same page 
                        else
                            instVM.SelectedPage = lastPage + 1; //starting the next page 
                    }
                }

                //if seletedPage greater than 1, let's set the SessionVariables from database
                if (instVM.SelectedPage > 1)
                    SetSessionvariables(null, instrumentId, organizationId);


                //to be efficient, lets get questions for the specified page only
                //set the Instrument Type for CJ5, CJ5A as they are sharing the same instrument
                Organization currentOrg = dataContext.Organizations.AsNoTracking().Where(p => p.OrganizationId == organizationId && p.Year == year).Include(p => p.OrganizationType).FirstOrDefault();
                instVM.Organization = currentOrg;
                instVM.OrgType = currentOrg.OrganizationType.OrganizationTypeId;
                instVM.Instrument.FormNumber = instVM.Instrument.IntroText;

                instVM.Questions = GetQuestions(instVM.Instrument, instVM.SelectedPage, organizationId, year);
                //for each question, find the child questions that have a non null response value pattern

                //Trying to hook in the data from datasupplier. 
                instVM.DataSupplier = dataContext.DataSuppliers.Where(s => s.Organization == currentOrg && s.Instrument == instVM.Instrument)
                    .FirstOrDefault();

                instVM.DatasupplierVM = getDatSupplierVM(currentOrg, instVM.Instrument);

                //data entry info
                instVM.Modes = dataContext.LookupModes;
                instVM.SummaryStatuses = dataContext.LookupSummaryStatuses;

                OrganizationFollowup orgFoll = dataContext.OrganizationFollowups.Where(r => r.Organization == currentOrg).FirstOrDefault();

                if (orgFoll != null)
                {
                    instVM.SelectedMode = orgFoll.SubmissionMode;
                    instVM.DateReceived = orgFoll.DateReceived.HasValue ? orgFoll.DateReceived.Value : DateTime.MinValue;
                    instVM.SelectedSummaryStatusCodeId = orgFoll.SummaryStatusCode;
                }

                foreach (QuestionViewModel q in instVM.Questions)
                {
                    //find child questions
                    if (q.ChildQuestions != null)
                    {
                        foreach (Question cq in q.ChildQuestions.Where(x => x.ParentResponseValue != null).ToList())
                        {
                            q.HasFollowupQuestionsClass = "HasFollowUp";

                        }

                    }

                }
                ViewData["Title"] = instVM.Instrument.FormNumber;


                //if (instVM.SelectedPage ==1)
                //    SaveResponse(dataContext, instVM.Instrument, organizationId, "form_status", "0", User.Identity.Name);  //variable=form_status, status =0 -- started, status=1 --- reached the final question and saved, status=2 -- completed
            }
            else
            {
                //they got here through an unknown way
                return RedirectToAction("AccessDenied", "Security");
            }

            //save action for poc role only
            if (User.IsInRole("poc"))
            {
                string action = "start";
                if (resume)
                    action = "resume";
                InstrumentActionLog act = new InstrumentActionLog
                {
                    OrganizationId = instVM.OrganizationId,
                    Year = instVM.Year,
                    InstrumentId = instVM.Instrument.InstrumentId,
                    Action = action,
                    CurrentPage= 0,
                    NextPage = instVM.SelectedPage,
                    CreatedBy = User.Identity.Name,
                    CreatedDate = System.DateTime.Now
                };
                dataContext.InstrumentActionLogs.Attach(act);
                dataContext.SaveChanges();
            }
            TempData["SelectedPage"] = instVM.SelectedPage;
            return View(instVM);
        }

        private DataSupplierViewModel getDatSupplierVM(Organization currentOrg, Instrument instrument)
        {
            DataSupplierViewModel dsvm = new DataSupplierViewModel();
            var ds = dataContext.DataSuppliers.Where(s => s.Organization == currentOrg && s.Instrument == instrument)
                .FirstOrDefault();

            dsvm.Instrument = instrument;
            dsvm.Organization = currentOrg;

            if (ds != null)
            {

                dsvm.DataSupplierId = ds.DataSupplierId;
                dsvm.Address = ds.Address;
                dsvm.City = ds.City;
                dsvm.email = ds.email;
                dsvm.Fax = ds.Fax;
                dsvm.Name = ds.Name;
                dsvm.Phone = ds.Phone;
                dsvm.State = ds.State;
                dsvm.Title = ds.Title;
                dsvm.Zip = ds.Zip;
                dsvm.FacilityName = ds.FacilityName;
            }

            return dsvm;

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Back(InstrumentViewModel model)
        {
            InstrumentViewModel m = BackFunction(model);
            return View("Index", model);
        }
        private InstrumentViewModel BackFunction(InstrumentViewModel model)
        {
            model.SelectedPage = Convert.ToInt32(TempData["SelectedPage"]);
            int currentPage = model.SelectedPage;
            //reload the questions for the previous page
            if (model.SelectedPage >= 2)
            {
                //get the instrument based on the passed back instrumentid
                model.SelectedPage = model.SelectedPage - 1;
                //COGTEST SKIPS
                //Question 9 (Page 9) from Question 8
                //Get the values of 7e and f
                if (model.SelectedPage == 9 && model.Instrument.InstrumentId == 3)
                {
                    Response resp7e = new Response();
                    Response resp7f = new Response();

                    if (dataContext.Responses.Any(p => p.Instrument.InstrumentId == model.Instrument.InstrumentId && p.Organization.OrganizationId == model.OrganizationId && p.Organization.Year == model.Year && p.ResponseVariable == "opimedprescription"))
                    {
                        resp7e = dataContext.Responses.Where(p => p.Instrument.InstrumentId == model.Instrument.InstrumentId && p.Organization.OrganizationId == model.OrganizationId && p.Organization.Year == model.Year && p.ResponseVariable == "opimedprescription").FirstOrDefault();
                    }
                    if (dataContext.Responses.Any(p => p.Instrument.InstrumentId == model.Instrument.InstrumentId && p.Organization.OrganizationId == model.OrganizationId && p.Organization.Year == model.Year && p.ResponseVariable == "opemeddisorder"))
                    {
                        resp7f = dataContext.Responses.Where(p => p.Instrument.InstrumentId == model.Instrument.InstrumentId && p.Organization.OrganizationId == model.OrganizationId && p.Organization.Year == model.Year && p.ResponseVariable == "opemeddisorder").FirstOrDefault();
                    }
                    if (resp7e != null && resp7e.ResponseValue == "2" && resp7f != null && resp7f.ResponseValue == "2")
                        model.SelectedPage = 8; //since there are only 9 pages in this instrument, setting this to 9 before the last page check will automatically complete the form.

                }
                //COGTEST SKIPS
                //Question 8 (Page 8) from Question 7
                //Get the values of 7b and g
                if (model.SelectedPage == 8 && model.Instrument.InstrumentId == 3)
                {
                    Response resp7b = new Response();
                    Response resp7g = new Response();

                    if (dataContext.Responses.Any(p => p.Instrument.InstrumentId == model.Instrument.InstrumentId && p.Organization.OrganizationId == model.OrganizationId && p.Organization.Year == model.Year && p.ResponseVariable == "opiscreen"))
                    {
                        resp7b = dataContext.Responses.Where(p => p.Instrument.InstrumentId == model.Instrument.InstrumentId && p.Organization.OrganizationId == model.OrganizationId && p.Organization.Year == model.Year && p.ResponseVariable == "opiscreen").FirstOrDefault();
                    }
                    if (dataContext.Responses.Any(p => p.Instrument.InstrumentId == model.Instrument.InstrumentId && p.Organization.OrganizationId == model.OrganizationId && p.Organization.Year == model.Year && p.ResponseVariable == "opimedwithdrawal"))
                    {
                        resp7g = dataContext.Responses.Where(p => p.Instrument.InstrumentId == model.Instrument.InstrumentId && p.Organization.OrganizationId == model.OrganizationId && p.Organization.Year == model.Year && p.ResponseVariable == "opimedwithdrawal").FirstOrDefault();
                    }
                    if (resp7b != null && resp7b.ResponseValue == "2" && resp7g != null && resp7g.ResponseValue == "2")
                        model.SelectedPage = 7; //if we set this to 8 now, the next step on decrementing the page will effectively skip question 8.


                }
                //ASJ Skips - Q14
                //from q15, if the answer to 1b is 0 then skip Q14
                if (model.SelectedPage == 14 && model.Instrument.InstrumentId != 3)
                {
                    if (dataContext.Responses.Any(p => p.Instrument.InstrumentId == model.Instrument.InstrumentId && p.Organization.OrganizationId == model.OrganizationId && p.Organization.Year == model.Year && p.ResponseVariable == "nconfpop"))
                    {
                        Response resp1b = dataContext.Responses.Where(p => p.Instrument.InstrumentId == model.Instrument.InstrumentId && p.Organization.OrganizationId == model.OrganizationId && p.Organization.Year == model.Year && p.ResponseVariable == "nconfpop").FirstOrDefault();
                        if (resp1b != null && resp1b.ResponseValue != null && resp1b.ResponseValue != "" && resp1b.ResponseValue == "0")
                        {
                            model.SelectedPage = 13;
                        }
                    }

                }
                //ASJ Skips - Q5
                //from quesion 6, if 4c or 4d has values more than 0 then show Q5 otherwise skip it
                if (model.SelectedPage == 5 && model.Instrument.InstrumentId != 3)
                {
                    bool maleValue = false;
                    bool femaleValue = false;
                    if (dataContext.Responses.Any(p => p.Instrument.InstrumentId == model.Instrument.InstrumentId && p.Organization.OrganizationId == model.OrganizationId && p.Organization.Year == model.Year && p.ResponseVariable == "juvm"))
                    {
                        Response resp4c = dataContext.Responses.Where(p => p.Instrument.InstrumentId == model.Instrument.InstrumentId && p.Organization.OrganizationId == model.OrganizationId && p.Organization.Year == model.Year && p.ResponseVariable == "juvm").FirstOrDefault();
                        if (resp4c != null && resp4c.ResponseValue != null && resp4c.ResponseValue != "0")
                        {
                            maleValue = true;
                        }
                    }

                    if (dataContext.Responses.Any(p => p.Instrument.InstrumentId == model.Instrument.InstrumentId && p.Organization.OrganizationId == model.OrganizationId && p.Organization.Year == model.Year && p.ResponseVariable == "juvf"))
                    {
                        Response resp4c = dataContext.Responses.Where(p => p.Instrument.InstrumentId == model.Instrument.InstrumentId && p.Organization.OrganizationId == model.OrganizationId && p.Organization.Year == model.Year && p.ResponseVariable == "juvf").FirstOrDefault();
                        if (resp4c != null && resp4c.ResponseValue != null && resp4c.ResponseValue != "0")
                        {
                            femaleValue = true;
                        }
                    }
                    if (!maleValue && !femaleValue)
                    {
                        model.SelectedPage = 4;
                    }
                }
                Instrument currentInstrument = dataContext.Instruments.AsNoTracking().Where(p => p.InstrumentId == model.Instrument.InstrumentId).FirstOrDefault();

                model.Questions = GetQuestions(model.Instrument, model.SelectedPage, model.OrganizationId, model.Year);
                //update formstatus
                model.formStatus = Extensions.CheckFormStarted(dataContext, model.OrganizationId, model.Year, model.Instrument.InstrumentId);
                //reset organization
                model.Organization = dataContext.Organizations.Where(x => x.OrganizationId == model.OrganizationId).Include(x => x.OrganizationType).FirstOrDefault();
                //for each question, find the child questions that have a non null response value pattern
                foreach (QuestionViewModel q in model.Questions)
                {
                    //find child questions
                    if (q.ChildQuestions != null)
                    {
                        foreach (Question cq in q.ChildQuestions.Where(x => x.ParentResponseValue != null).ToList())
                        {
                            //RemoveChildQuestions(qvm, model.Questions, cq);
                            q.HasFollowupQuestionsClass = "HasFollowUp";


                        }
                    }

                }
                //Trying to hook in the data from datasupplier. 
                model.DataSupplier = dataContext.DataSuppliers.Where(s => s.Organization.OrganizationId == model.OrganizationId && s.Instrument == model.Instrument)
                    .FirstOrDefault();
                Organization org = dataContext.Organizations.FirstOrDefault(s => s.OrganizationId == model.OrganizationId && s.Year == model.Year);
                if (org != null)
                    model.DatasupplierVM = getDatSupplierVM(org, currentInstrument);

                //first save to action log for poc role only
                if (User.IsInRole("poc"))
                {
                    InstrumentActionLog act = new InstrumentActionLog
                    {
                        OrganizationId = model.OrganizationId,
                        Year = model.Year,
                        InstrumentId = model.Instrument.InstrumentId,
                        Action = "back",
                        CurrentPage = currentPage,
                        NextPage = model.SelectedPage,
                        CreatedBy = User.Identity.Name,
                        CreatedDate = System.DateTime.Now
                    };
                    dataContext.InstrumentActionLogs.Attach(act);
                    dataContext.SaveChanges();
                }
                model.Instrument = currentInstrument;

                //Data Entry fields load
                //data entry info
                model.Modes = dataContext.LookupModes;
                model.SummaryStatuses = dataContext.LookupSummaryStatuses;

                OrganizationFollowup orgFoll = dataContext.OrganizationFollowups.Where(r => r.Organization == org).FirstOrDefault();

                if (orgFoll != null)
                {
                    model.SelectedMode = orgFoll.SubmissionMode;
                    model.DateReceived = orgFoll.DateReceived.HasValue ? orgFoll.DateReceived.Value : DateTime.MinValue;
                    model.SelectedSummaryStatusCodeId = orgFoll.SummaryStatusCode;
                }

                TempData["SelectedPage"] = model.SelectedPage;
                ModelState.Clear();
            }
            return model;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Next(InstrumentViewModel model)
        {
            model.SelectedPage = Convert.ToInt32(TempData["SelectedPage"]);
            //get the instrument based on the passed back instrumentid
            Instrument currentInstrument = dataContext.Instruments.AsNoTracking().Where(p => p.InstrumentId == model.Instrument.InstrumentId).FirstOrDefault();
            Organization currentOrg = dataContext.Organizations.AsNoTracking().Where(p => p.OrganizationId == model.OrganizationId && p.Year == model.Year).Include(p => p.OrganizationType).FirstOrDefault();
            currentInstrument.FormNumber = currentInstrument.IntroText;
            ViewData["Title"] = currentInstrument.FormNumber;
            model.Instrument = currentInstrument;
            model.OrgType = currentOrg.OrganizationType.OrganizationTypeId;
            model.Organization = currentOrg;
            //this will be used by the button submit on each page
            if (ModelState.IsValid || model.RTIDataEntry)
            {
                int currentPage = model.SelectedPage;
                
                //if it gets here then all validations (form and jquery) have been satisfied
                //save the responses for the questions - update or insert
                //get the questions for the current page number
                // foreach (QuestionViewModel q in model.Questions.Where(p => p.Question.Page == model.SelectedPage).ToList())
                if (model.Questions != null)
                {
                    foreach (QuestionViewModel q in model.Questions)
                    {
                        //dont save the estimate checkbox - we are handling that seperately
                        if (q.Question.QuestionType.QuestionTypeDescription != "EstimateCheckbox")
                        {
                           
                            SaveResponse(dataContext, currentInstrument, model.OrganizationId, q.Question.Variable, q.SingleResponse, User.Identity.Name);

                            //Save Sessions variables for validation purpose, ignore COGTEST
                            if (currentInstrument.InstrumentId != COGTEST_INSTRUMENT_ID)
                                SetSessionvariables(q);

                            //save the estimate response if any                            
                            if (q.EstimateQuestion != null)
                            {
                                SaveResponse(dataContext, currentInstrument, model.OrganizationId, q.EstimateQuestion.Variable, q.EstimateResponse.ToString(), User.Identity.Name);
                            }
                        }
                        // }

                        // Check if Question 1 was previously completed.
                        Boolean formStarted = Extensions.CheckFormStarted(dataContext, model.OrganizationId, model.Year, model.Instrument.InstrumentId);
                        if (!formStarted && model.SelectedPage == 1)
                            SaveResponse(dataContext, model.Instrument, model.OrganizationId, "form_status", "0", User.Identity.Name);  //variable=form_status, status =0 -- started, status=1 --- reached the final question and saved, status=2 -- completed
                    }

                    

                }

                //use the common function for skip logic checking
                model.SelectedPage = CheckSkips(this.dataContext, model.SelectedPage, model.Instrument.InstrumentId, model.OrganizationId, model.Year);

                //if the current page has reached the max then redirect to the home page
                //get the max page number from all the questions for the instrument
                model.MaxPage = dataContext.InstrumentQuestions.OrderByDescending(p => p.Page).Where(p => p.Instrument.InstrumentId == model.Instrument.InstrumentId).AsNoTracking().FirstOrDefault().Page;
                if (model.SelectedPage >= model.MaxPage)
                {
                    //if the instrument is cogtest then there skip the submission parts, there is only one submission
                    //hardcoding submission to 1
                    if (model.Instrument.InstrumentId == 3)//cogtest
                    {
                        SaveResponse(dataContext, currentInstrument, model.OrganizationId, "form_status", "2", User.Identity.Name);  //status=2, completed
                        if (User.IsInRole("poc"))  //not sure if this is right
                            return RedirectToAction("CogTestFinal", "Form", new { organizationId = model.OrganizationId, instrumentId = model.Instrument.InstrumentId, year = model.Year });
                        else if (User.IsInRole("client"))
                            return RedirectToAction("Index", "Client", new { organizationId = model.OrganizationId, iid = model.Instrument.InstrumentId });
                        else
                        {
                            //don;t know if admin should see this too
                            return RedirectToAction("Index", "DQFU", new { organizationId = model.OrganizationId, iid = model.Instrument.InstrumentId });
                        }
                    }
                    else
                    {
                        if (model.Questions[0].Question.Variable.ToLower() == "submission" && model.Questions[0].SingleResponse == "1")
                        {
                            SaveResponse(dataContext, currentInstrument, model.OrganizationId, "form_status", "2", User.Identity.Name);  //status=2, completed
                            if (User.IsInRole("poc"))  //not sure if this is right
                                return RedirectToAction("Index", "DQFU", new { organizationId = model.OrganizationId, iid = model.Instrument.InstrumentId });
                            else if (User.IsInRole("client"))
                                return RedirectToAction("Index", "DQFU", new { organizationId = model.OrganizationId, iid = model.Instrument.InstrumentId });
                            else
                            {
                                //don;t know if admin should see this too
                                return RedirectToAction("Index", "DQFU", new { organizationId = model.OrganizationId, iid = model.Instrument.InstrumentId });
                            }
                        }
                        else if (model.Questions[0].Question.Variable.ToLower() == "submission" && model.Questions[0].SingleResponse == "2")
                        {
                            SaveResponse(dataContext, currentInstrument, model.OrganizationId, "form_status", "1", User.Identity.Name);  //status=1, saved but not completed 
                            if (User.IsInRole("poc"))  //not sure if this is right
                                return RedirectToAction("Index", "DQFU", new { organizationId = model.OrganizationId, iid = model.Instrument.InstrumentId });
                            else if (User.IsInRole("client"))
                                return RedirectToAction("Index", "DQFU", new { organizationId = model.OrganizationId, iid = model.Instrument.InstrumentId });
                            else
                            {
                                //don;t know if admin should see this too
                                return RedirectToAction("Index", "DQFU", new { organizationId = model.OrganizationId, iid = model.Instrument.InstrumentId });
                            }
                        }
                        else if (model.Questions[0].Question.Variable.ToLower() == "submission" && model.Questions[0].SingleResponse == "3")
                        {
                            TempData["SelectedPage"] = model.SelectedPage;
                            InstrumentViewModel m = BackFunction(model);
                            return View("Index", model);
                           
                        }
                    }
                }

               
            
                //save followup data
                SaveFollowupData(dataContext, currentInstrument, model.OrganizationId, User.Identity.Name);  //status=1, saved but not completed 

                //increment page to the next page
                model.SelectedPage = model.SelectedPage + 1;
                //reload the questions for the correct page
                model.Questions = GetQuestions(model.Instrument, model.SelectedPage, model.OrganizationId, model.Year);
                //update formstatus
                model.formStatus = Extensions.CheckFormStarted(dataContext, model.OrganizationId, model.Year, model.Instrument.InstrumentId);

                //for each question, find the child questions that have a non null response value pattern
                foreach (QuestionViewModel q in model.Questions)
                {
                    //find child questions
                    if (q.ChildQuestions != null)
                    {
                        foreach (Question cq in q.ChildQuestions.Where(x => x.ParentResponseValue != null).ToList())
                        {

                            // RemoveChildQuestions(qvm, model.Questions, cq);
                            q.HasFollowupQuestionsClass = "HasFollowUp";


                        }
                    }

                }

                //Trying to hook in the data from datasupplier. 
                model.DataSupplier = dataContext.DataSuppliers.Where(s => s.Organization == currentOrg && s.Instrument == model.Instrument)
                    .FirstOrDefault();

                model.DatasupplierVM = getDatSupplierVM(currentOrg, model.Instrument);

                TempData["SelectedPage"] = model.SelectedPage;
                ModelState.Clear();

                //Data Entry fields load
                //data entry info
                model.Modes = dataContext.LookupModes;
                model.SummaryStatuses = dataContext.LookupSummaryStatuses;

                OrganizationFollowup orgFoll = dataContext.OrganizationFollowups.Where(r => r.Organization == currentOrg).FirstOrDefault();

                if (orgFoll != null)
                {
                    model.SelectedMode = orgFoll.SubmissionMode;
                    model.DateReceived = orgFoll.DateReceived.HasValue ? orgFoll.DateReceived.Value : DateTime.MinValue;
                    model.SelectedSummaryStatusCodeId = orgFoll.SummaryStatusCode;
                }

                //dsb
                //Trying to hook in the data from datasupplier. 
                model.DataSupplier = dataContext.DataSuppliers.Where(s => s.Organization.OrganizationId == model.OrganizationId && s.Instrument == model.Instrument)
                    .FirstOrDefault();
                Organization org = dataContext.Organizations.FirstOrDefault(s => s.OrganizationId == model.OrganizationId && s.Year == model.Year);
                if (org != null)
                    model.DatasupplierVM = getDatSupplierVM(org, currentInstrument);


                //first save to action log for poc role only
                if (User.IsInRole("poc"))
                {
                    InstrumentActionLog act = new InstrumentActionLog
                    {
                        OrganizationId = model.OrganizationId,
                        Year = model.Year,
                        InstrumentId = model.Instrument.InstrumentId,
                        Action = "next",
                        CurrentPage = currentPage,
                        NextPage = model.SelectedPage,
                        CreatedBy = User.Identity.Name,
                        CreatedDate = System.DateTime.Now
                    };
                    dataContext.InstrumentActionLogs.Attach(act);
                    dataContext.SaveChanges();
                }
            }
            return View("Index", model);
        }


        private List<QuestionViewModel> GetQuestions(Instrument currentInstrument, int selectedPage, int orgId, int year)
        {
            List<QuestionViewModel> qvmList = new List<QuestionViewModel>();
            //for each of the page, get the questions and create the view model
            foreach (InstrumentQuestion q in dataContext.InstrumentQuestions.Where(p => p.Instrument.InstrumentId == currentInstrument.InstrumentId && p.Page == selectedPage).Include(p => p.Question).Include(p => p.Question.QuestionType).AsNoTracking().OrderBy(p => p.OrderNumber).ToList())
            {
                QuestionViewModel qvm = new QuestionViewModel();
                qvm.Instrument = currentInstrument;
                qvm.Question = q.Question;
                qvm.Page = q.Page;
                qvm.ResponseOptions = dataContext.ResponseOptions.Where(p => p.Question.QuestionId == q.Question.QuestionId).OrderBy(p => p.OrderNumber).AsNoTracking().ToList();
                //need organization id to retrieve the proper answer
                qvm.Responses = dataContext.Responses.Where(p => p.Instrument.InstrumentId == currentInstrument.InstrumentId && p.ResponseVariable == q.Question.Variable && p.Organization.OrganizationId == orgId).ToList();
                qvm.IncludeExcludes = dataContext.QuestionIncludesExcludes.Where(p => p.QuestionId == q.Question.QuestionId).AsNoTracking().ToList();
                //if the type of question is not a multiple option then there is only one possible answer. So lets load the singleresponse property with this value
                if (qvm.Question.QuestionType.QuestionTypeDescription != "MultipleOptions")
                {
                    foreach (Response r in qvm.Responses)
                    {
                        qvm.SingleResponse = r.ResponseValue;
                    }
                }
                else
                {
                    //for now we don;t have any question that has multiple answers
                    //need to figure out a way to load the multiple answers for the fields on the view
                }

                //check if this question is a total question
                if (dataContext.QuestionTotalAddends.Any(x => x.TotalQuestionId == q.Question.QuestionId))
                {
                    qvm.TotalQuestionClass = "totalbox tot_" + q.Question.Variable;
                }
                else if (dataContext.QuestionTotalAddends.Any(x => x.AddendQuestionId == q.Question.QuestionId))
                {
                    //get the total question
                    int tot = dataContext.QuestionTotalAddends.Where(x => x.AddendQuestionId == q.Question.QuestionId).FirstOrDefault().TotalQuestionId;
                    Question totQ = dataContext.Questions.Where(x => x.QuestionId == tot).FirstOrDefault();
                    qvm.TotalQuestionClass = "addendbox tot_" + totQ.Variable;
                }
                else
                {
                    qvm.TotalQuestionClass = "";
                }
                //get the estimate checkbox question if any
                Question estIQ = dataContext.Questions.Where(p => p.ParentQuestionId == q.Question.QuestionId && p.QuestionType.QuestionTypeDescription == "EstimateCheckbox").AsNoTracking().FirstOrDefault();
                if (estIQ != null)
                {
                    qvm.EstimateQuestion = estIQ;
                    Response estResp = dataContext.Responses.Where(p => p.Instrument.InstrumentId == currentInstrument.InstrumentId && p.ResponseVariable == qvm.EstimateQuestion.Variable && p.Organization.OrganizationId == orgId).FirstOrDefault();
                    if (estResp != null)
                        qvm.EstimateResponse = Convert.ToBoolean(estResp.ResponseValue);
                }

                //check to see if the question has any child question (Estimate Checkbox excluded)
                List<Question> childIQs = dataContext.Questions.Where(p => p.ParentQuestionId == q.Question.QuestionId && p.QuestionType.QuestionTypeDescription != "EstimateCheckbox").Include(p => p.QuestionType).AsNoTracking().OrderBy(p => p.OrderSegment).ToList();
                if (childIQs != null && childIQs.Count > 0)
                {
                    qvm.ChildQuestions = childIQs;
                }
                //set the parent variable value
                Question parentQuestion = dataContext.Questions.Where(p => p.QuestionId == qvm.Question.ParentQuestionId).FirstOrDefault();
                if (parentQuestion != null)
                    qvm.ParentQuestionVariable = parentQuestion.Variable;
                //question text change based on the FormNumber
                ReplaceFillInText(qvm, orgId, year);
                ReplaceAnswerReferenceText(qvm, currentInstrument.InstrumentId, orgId, year);

                qvmList.Add(qvm);
            }
            return qvmList;
        }

        /// <summary>
        /// Save Q1A (variable: confpop), Q4A to Q4D in session variable since we need them for validation
        /// </summary>
        /// <param name="q"></param>
        private void SetSessionvariables(QuestionViewModel q, int iid = 0, int orgId = 0)
        {
            if (q == null)
            {
                //retrieve the response data from 
                Response r = dataContext.Responses.Where(p => p.Instrument.InstrumentId == iid && p.ResponseVariable == "confpop" && p.Organization.OrganizationId == orgId).FirstOrDefault();
                if (r != null && r.ResponseValue != null)
                    HttpContext.Session.SetString("Q1A", r.ResponseValue.ToString());

                r = dataContext.Responses.Where(p => p.Instrument.InstrumentId == iid && p.ResponseVariable == "nconfpop" && p.Organization.OrganizationId == orgId).FirstOrDefault();
                if (r != null && r.ResponseValue != null)
                    HttpContext.Session.SetString("Q1B", r.ResponseValue.ToString());

                r = dataContext.Responses.Where(p => p.Instrument.InstrumentId == iid && p.ResponseVariable == "adultm" && p.Organization.OrganizationId == orgId).FirstOrDefault();
                if (r != null && r.ResponseValue != null)
                    HttpContext.Session.SetString("Q4A", r.ResponseValue.ToString());

                r = dataContext.Responses.Where(p => p.Instrument.InstrumentId == iid && p.ResponseVariable == "adultf" && p.Organization.OrganizationId == orgId).FirstOrDefault();
                if (r != null && r.ResponseValue != null)
                    HttpContext.Session.SetString("Q4B", r.ResponseValue.ToString());

                r = dataContext.Responses.Where(p => p.Instrument.InstrumentId == iid && p.ResponseVariable == "juvm" && p.Organization.OrganizationId == orgId).FirstOrDefault();
                if (r != null && r.ResponseValue != null)
                    HttpContext.Session.SetString("Q4C", r.ResponseValue.ToString());

                r = dataContext.Responses.Where(p => p.Instrument.InstrumentId == iid && p.ResponseVariable == "juvf" && p.Organization.OrganizationId == orgId).FirstOrDefault();
                if (r != null && r.ResponseValue != null)
                    HttpContext.Session.SetString("Q4D", r.ResponseValue.ToString());

                r = dataContext.Responses.Where(p => p.Instrument.InstrumentId == iid && p.ResponseVariable == "adpmale" && p.Organization.OrganizationId == orgId).FirstOrDefault();
                if (r != null && r.ResponseValue != null)
                    HttpContext.Session.SetString("Q11A", r.ResponseValue.ToString());

                r = dataContext.Responses.Where(p => p.Instrument.InstrumentId == iid && p.ResponseVariable == "adpfemale" && p.Organization.OrganizationId == orgId).FirstOrDefault();
                if (r != null && r.ResponseValue != null)
                    HttpContext.Session.SetString("Q11B", r.ResponseValue.ToString());
            }
            else
            {

                //Save the followings responses in session variable since we need it for validation
                if (String.Compare(q.Question.Variable, "confpop", true) == 0 && q.SingleResponse != null)
                    HttpContext.Session.SetString("Q1A", q.SingleResponse);  //use Q1A to match the validation documentation

                if (String.Compare(q.Question.Variable, "nconfpop", true) == 0 && q.SingleResponse != null)
                    HttpContext.Session.SetString("Q1B", q.SingleResponse);  //use Q1B to match the validation documentation

                if (String.Compare(q.Question.Variable, "adultm", true) == 0 && q.SingleResponse != null)
                    HttpContext.Session.SetString("Q4A", q.SingleResponse);  //use Q4A to match the validation documentation

                if (String.Compare(q.Question.Variable, "adultf", true) == 0 && q.SingleResponse != null)
                    HttpContext.Session.SetString("Q4B", q.SingleResponse);  //use Q4B to match the validation documentation

                if (String.Compare(q.Question.Variable, "juvm", true) == 0 && q.SingleResponse != null)
                    HttpContext.Session.SetString("Q4C", q.SingleResponse);  //use Q4C to match the validation documentation

                if (String.Compare(q.Question.Variable, "juvf", true) == 0 && q.SingleResponse != null)
                    HttpContext.Session.SetString("Q4D", q.SingleResponse);  //use Q4D to match the validation documentation

                if (String.Compare(q.Question.Variable, "adpmale", true) == 0 && q.SingleResponse != null)
                    HttpContext.Session.SetString("Q11A", q.SingleResponse);  //use Q11A to match the validation documentation

                if (String.Compare(q.Question.Variable, "adpfemale", true) == 0 && q.SingleResponse != null)
                    HttpContext.Session.SetString("Q11B", q.SingleResponse);  //use Q11B to match the validation documentation
            }
        }

        /// <summary>
        /// changed the method from private to static public so iDQFU can call it.  
        /// Don't like this solution much, 
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="instrumentId"></param>
        /// <param name="year"></param>
        /// <param name="statuscode"></param>
        public static void SaveResponse(ASJDbContext dataContext, Instrument ins, int organizationId, string variableName, string variableValue, string username)
        {
            //save  the form started date and modified date in the followup table
            Organization currentOrg = dataContext.Organizations.AsNoTracking().Where(p => p.OrganizationId == organizationId && p.Year == ins.Year).FirstOrDefault();

            if (dataContext.Responses.Any(p => p.Instrument.InstrumentId == ins.InstrumentId && p.Organization.OrganizationId == organizationId && p.Organization.Year == ins.Year && p.ResponseVariable == variableName))
            {
                //exists in the database - need to update
                Response resp = dataContext.Responses.Where(p => p.Instrument.InstrumentId == ins.InstrumentId && p.Organization.OrganizationId == organizationId && p.Organization.Year == ins.Year && p.ResponseVariable == variableName).FirstOrDefault();
                //Add code to store question variables text
                resp.ResponseVariable = variableName;
                resp.ResponseValue = variableValue;
                resp.ModifiedBy = username;// User.FindFirstValue(ClaimTypes.NameIdentifier);
                resp.ModifiedOn = System.DateTime.Now;
                dataContext.Responses.Update(resp);
                dataContext.SaveChanges();
            }
            else
            {
                //get the single response
                Response currResponse = new Response
                {
                    Instrument = ins,
                    Organization = currentOrg,
                    ResponseVariable = variableName,
                    ResponseValue = variableValue,

                    //add modified date and time
                    ModifiedBy = username,
                    ModifiedOn = System.DateTime.Now,
                    //set the created info - this will only be used while inserting
                    CreatedBy = username,
                    CreatedOn = System.DateTime.Now
                };
                //insert the new response into the databas
                dataContext.Responses.Attach(currResponse);
                dataContext.SaveChanges();
            }

        }
        public static void SaveFollowupData(ASJDbContext dataContext, Instrument ins, int organizationId, string username)
        {
            //save  the form started date and modified date in the followup table
            Organization currentOrg = dataContext.Organizations.Where(p => p.OrganizationId == organizationId && p.Year == ins.Year).FirstOrDefault();
            OrganizationFollowup followup = dataContext.OrganizationFollowups.Where(p => p.Organization.OrganizationId == organizationId && p.Organization.Year == ins.Year).FirstOrDefault();
            if (followup == null)
            {
                //create a row and set the created date
                //OrganizationFollowup followup = new OrganizationFollowup()
                followup = new OrganizationFollowup()
                {
                    FormCreatedDate = System.DateTime.Now,
                    FormModifiedDate = System.DateTime.Now,
                    CreatedOn = System.DateTime.Now,
                    ModifiedOn = System.DateTime.Now,
                    CreatedBy = username,
                    ModifiedBy = username,
                    Organization = currentOrg
                };
                dataContext.OrganizationFollowups.Attach(followup);
                dataContext.SaveChanges();
                //currentOrg.OrganizationFollowup = followup;
                //dataContext.Update(currentOrg);
                //dataContext.SaveChanges();
            }
            else
            {
                //OrganizationFollowup followup = dataContext.OrganizationFollowups.Where(p => p.OrganizationFollowupId == currentOrg.OrganizationFollowup.OrganizationFollowupId).FirstOrDefault();
                if (!followup.FormCreatedDate.HasValue)
                    followup.FormCreatedDate = System.DateTime.Now;
                followup.FormModifiedDate = System.DateTime.Now;
                followup.ModifiedOn = System.DateTime.Now;
                followup.ModifiedBy = username;
                dataContext.OrganizationFollowups.Update(followup);
                dataContext.SaveChanges();
            }
        }

        /// <summary>
        /// Replace text based on the organization type. The texts are in the database in the pattern [type2text|type1text]
        /// this function uses the first text or the second text based on org type. 
        /// If the text fill in changes, the change needs to be made in the database fields.
        /// </summary>
        /// <param name="qvm"></param>
        /// <param name="organizationId"></param>
        /// <param name="year"></param>
        private void ReplaceFillInText(QuestionViewModel qvm, int organizationId, int year)
        {
            //get the org type
            var org = dataContext.Organizations
                .AsNoTracking()
                .Include(p => p.OrganizationType)
                .Where(o => o.OrganizationId == organizationId && o.Year == year)
                .FirstOrDefault();
            if (org != null)
            {
                if (org.OrganizationType != null)
                {
                    int orgType = org.OrganizationType.OrganizationTypeId;
                    string patternToMatch = @"\[([^]]+)*\]";
                    string splitBy = @"(\|)";
                    //question text
                    if (qvm.Question.QuestionText != null)
                    {
                        if (Regex.IsMatch(qvm.Question.QuestionText, patternToMatch))
                        {
                            foreach (Match m in Regex.Matches(qvm.Question.QuestionText, patternToMatch))
                            {
                                string textToReplace = m.Value;

                                //split by | into an array
                                string[] splits = Regex.Split(textToReplace, splitBy);
                                if (orgType == 2)
                                {
                                    qvm.Question.QuestionText = qvm.Question.QuestionText.Replace(textToReplace, Regex.Replace(Regex.Replace(splits[0].ToString(), @"\[", ""), @"\]", ""));
                                }
                                else if (orgType == 3)
                                {
                                    qvm.Question.QuestionText = qvm.Question.QuestionText.Replace(textToReplace, Regex.Replace(Regex.Replace(splits[2].ToString(), @"\[", ""), @"\]", ""));
                                }
                            }
                        }
                    }
                    //intro text
                    if (qvm.Question.IntroText != null)
                    {

                        if (Regex.IsMatch(qvm.Question.IntroText, patternToMatch))
                        {
                            foreach (Match m in Regex.Matches(qvm.Question.IntroText, patternToMatch))
                            {
                                string textToReplace = m.Value;
                                //split by | into an array
                                string[] splits = Regex.Split(textToReplace, splitBy);
                                if (orgType == 2)
                                {
                                    qvm.Question.IntroText = qvm.Question.IntroText.Replace(textToReplace, Regex.Replace(Regex.Replace(splits[0].ToString(), @"\[", ""), @"\]", ""));
                                }
                                else if (orgType == 3)
                                {
                                    qvm.Question.IntroText = qvm.Question.IntroText.Replace(textToReplace, Regex.Replace(Regex.Replace(splits[2].ToString(), @"\[", ""), @"\]", ""));
                                }
                            }
                        }
                    }
                    //include exclude
                    foreach (QuestionIncludeExclude qie in qvm.IncludeExcludes)
                    {
                        if (qie.DisplayText != null)
                        {

                            if (Regex.IsMatch(qie.DisplayText, patternToMatch))
                            {
                                foreach (Match m in Regex.Matches(qie.DisplayText, patternToMatch))
                                {
                                    string textToReplace = m.Value;
                                    //split by | into an array
                                    string[] splits = Regex.Split(textToReplace, splitBy);
                                    if (orgType == 2)
                                    {
                                        qie.DisplayText = qie.DisplayText.Replace(textToReplace, Regex.Replace(Regex.Replace(splits[0].ToString(), @"\[", ""), @"\]", ""));
                                    }
                                    else if (orgType == 3)
                                    {
                                        qie.DisplayText = qie.DisplayText.Replace(textToReplace, Regex.Replace(Regex.Replace(splits[2].ToString(), @"\[", ""), @"\]", ""));
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        private void ReplaceAnswerReferenceText(QuestionViewModel qvm, int instrumentId, int organizationId, int year)
        {
            //find variable name
            string patternToMatch = @"{tot_.*}";

            //question text
            if (qvm.Question.QuestionText != null)
            {
                if (Regex.IsMatch(qvm.Question.QuestionText, patternToMatch))
                {
                    foreach (Match m in Regex.Matches(qvm.Question.QuestionText, patternToMatch))
                    {
                        string textToReplace = m.Value;

                        //get the variable
                        string variable = Regex.Replace(textToReplace, "tot_", "");
                        variable = Regex.Replace(variable, @"\{", "");
                        variable = Regex.Replace(variable, @"\}", "");

                        //get the org type
                        Response resp = dataContext.Responses.Where(p => p.Instrument.InstrumentId == instrumentId && p.ResponseVariable == variable && p.Organization.OrganizationId == organizationId).FirstOrDefault();

                        if (resp != null && resp.ResponseValue != null)
                        {
                            qvm.Question.QuestionText = qvm.Question.QuestionText.Replace(textToReplace, resp.ResponseValue.ToString());

                        }
                        else
                        {
                            qvm.Question.QuestionText = qvm.Question.QuestionText.Replace(", " + textToReplace, "");
                        }
                    }
                }
            }

        }


        [ValidateAntiForgeryToken]
        public IActionResult SaveDSB(DataSupplierViewModel model)
        {
            DataSupplier dataSupplier = dataContext.DataSuppliers.SingleOrDefault(m => m.DataSupplierId == model.DataSupplierId);
            if (model.Organization != null && model.Instrument != null)
            {
                Organization org = dataContext.Organizations.SingleOrDefault(m => m.OrganizationId == model.Organization.OrganizationId && m.Year == model.Organization.Year);
                Instrument inst = dataContext.Instruments.SingleOrDefault(m => m.InstrumentId == model.Instrument.InstrumentId);
                if (dataSupplier == null)
                {
                    dataSupplier = new DataSupplier
                    {
                        //create
                        Name = model.Name,
                        Address = model.Address,
                        City = model.City,
                        State = model.State,
                        Zip = model.Zip,
                        Phone = model.Phone,
                        Fax = model.Fax,
                        email = model.email,
                        Title = model.Title,
                        Instrument = inst,
                        Organization = org,
                        FacilityName = model.FacilityName
                    };
                    dataContext.DataSuppliers.Add(dataSupplier);
                    dataContext.SaveChanges();
                }
                else
                {
                    //update
                    dataSupplier.Name = model.Name;
                    dataSupplier.Address = model.Address;
                    dataSupplier.City = model.City;
                    dataSupplier.State = model.State;
                    dataSupplier.Zip = model.Zip;
                    dataSupplier.Phone = model.Phone;
                    dataSupplier.Fax = model.Fax;
                    dataSupplier.email = model.email;
                    dataSupplier.Title = model.Title;
                    dataSupplier.FacilityName = model.FacilityName;

                    dataContext.Update(dataSupplier);
                    dataContext.SaveChanges();
                }
                //throw new Exception("stop");
                return new JsonResult("Information Saved.");
            }
            else
            {
                //throw new Exception("stop");
                return new JsonResult("Error");
            }

        }

        /// <summary>
        /// Get the last page 
        /// </summary>
        /// <returns></returns>
        public static int GetLastPage(ASJDbContext dataContext, int iid, int orgId, int year)
       // private int GetLastPage(int iid, int orgId, int year)
        {
            int lastPage = 0;
            Response resp = dataContext.Responses.Where(p => p.Instrument.InstrumentId == iid && p.Organization.OrganizationId == orgId && p.ResponseVariable != null && p.ResponseVariable!="form_status" && !p.ResponseVariable.ToLower().Contains("dqfu")).OrderByDescending(p => p.ModifiedOn).FirstOrDefault();
            if (resp != null)
            {
                //get the qid from the responsevariable
                Question q = dataContext.Questions.Where(p => p.Variable == resp.ResponseVariable).FirstOrDefault();
                //get the page number for qid
                InstrumentQuestion iq = dataContext.InstrumentQuestions.Where(p => p.Question.QuestionId == q.QuestionId && p.Instrument.InstrumentId == iid).FirstOrDefault();
                if (iq != null)
                    lastPage = iq.Page;
                //check skip logic 
                lastPage = CheckSkips(dataContext, lastPage, iid, orgId, year);
                //skip function assumes that the page number is going to be incremented, so add +1
                lastPage = lastPage + 1;
            }

            return lastPage;
        }

        /// <summary>
        /// Check the skip logic, may need to skip a page, return the next page after skip logic evaluation
        /// </summary>
        /// <param name="page"></param>
        /// <param name="iid"></param>
        /// <param name="orgId"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public static int CheckSkips(ASJDbContext dataContext, int page, int iid, int orgId, int year)
        {
            int nextPage = page;

            

            //COGTEST SKIPS
            //Question 8 (Page 8) from Question 7
            //Get the values of 7b and g
            if (page == 7 && iid == 3)
            {
                Response resp7b = new Response();
                Response resp7g = new Response();
                if (dataContext.Responses.Any(p => p.Instrument.InstrumentId == iid && p.Organization.OrganizationId == orgId && p.Organization.Year == year && p.ResponseVariable == "opiscreen"))
                {
                    resp7b = dataContext.Responses.Where(p => p.Instrument.InstrumentId == iid && p.Organization.OrganizationId == orgId && p.Organization.Year == year && p.ResponseVariable == "opiscreen").FirstOrDefault();
                }
                if (dataContext.Responses.Any(p => p.Instrument.InstrumentId == iid && p.Organization.OrganizationId == orgId && p.Organization.Year == year && p.ResponseVariable == "opimedwithdrawal"))
                {
                    resp7g = dataContext.Responses.Where(p => p.Instrument.InstrumentId == iid && p.Organization.OrganizationId == orgId && p.Organization.Year == year && p.ResponseVariable == "opimedwithdrawal").FirstOrDefault();
                }
                if (resp7b != null && resp7b.ResponseValue == "2" && resp7g != null && resp7g.ResponseValue == "2")
                    nextPage = 8;//the next step in incrementing page will effectively skip page 8

                
            }

            //COGTEST SKIPS - hardcoded
            //Question 9 (Page 9) from Question 8
            //Get the values of 7e and f
            if ((page == 8 || nextPage == 8) && iid == 3)
            {
                Response resp7e = new Response();
                Response resp7f = new Response();

                if (dataContext.Responses.Any(p => p.Instrument.InstrumentId == iid && p.Organization.OrganizationId == orgId && p.Organization.Year == year && p.ResponseVariable == "opimedprescription"))
                {
                    resp7e = dataContext.Responses.Where(p => p.Instrument.InstrumentId == iid && p.Organization.OrganizationId == orgId && p.Organization.Year == year && p.ResponseVariable == "opimedprescription").FirstOrDefault();
                }
                if (dataContext.Responses.Any(p => p.Instrument.InstrumentId == iid && p.Organization.OrganizationId == orgId && p.Organization.Year == year && p.ResponseVariable == "opemeddisorder"))
                {
                    resp7f = dataContext.Responses.Where(p => p.Instrument.InstrumentId == iid && p.Organization.OrganizationId == orgId && p.Organization.Year == year && p.ResponseVariable == "opemeddisorder").FirstOrDefault();
                }
                if (resp7e != null && resp7e.ResponseValue == "2" && resp7f != null && resp7f.ResponseValue == "2")
                    nextPage = 9;//since there are only 9 pages in this instrument, setting this to 9 before the last page check will automatically complete the form.
            }
            //ASJ Skips - Q14
            //from q13, if the answer to 1b is 0 then skip Q14
            if (page == 13 && iid != 3)
            {
                if (dataContext.Responses.Any(p => p.Instrument.InstrumentId == iid && p.Organization.OrganizationId == orgId && p.Organization.Year == year && p.ResponseVariable == "nconfpop"))
                {
                    Response resp1b = dataContext.Responses.Where(p => p.Instrument.InstrumentId == iid && p.Organization.OrganizationId == orgId && p.Organization.Year == year && p.ResponseVariable == "nconfpop").FirstOrDefault();
                    if (resp1b != null && resp1b.ResponseValue != null && resp1b.ResponseValue != "" && resp1b.ResponseValue == "0")
                    {
                        nextPage = 14; //if we set this to 14 now, the next step of incrementing the page will effectively skip question 14.
                    }
                }

            }
            //ASJ Skips - Q5
            //from quesion 4, if 4c or 4d has values more than 0 then show Q5 otherwise skip it
            if (page == 4 && iid != 3)
            {
                bool maleValue = true;
                bool femaleValue = true;
                if (dataContext.Responses.Any(p => p.Instrument.InstrumentId == iid && p.Organization.OrganizationId == orgId && p.Organization.Year == year && p.ResponseVariable == "juvm"))
                {
                    Response resp4c = dataContext.Responses.Where(p => p.Instrument.InstrumentId == iid && p.Organization.OrganizationId == orgId && p.Organization.Year == year && p.ResponseVariable == "juvm").FirstOrDefault();
                    if (resp4c == null || resp4c.ResponseValue == null || resp4c.ResponseValue == "" || resp4c.ResponseValue == "0")
                    {
                        maleValue = false; //if we set this to 5 now, the next step of incrementing the page will effectively skip question 5
                    }
                }
                if (dataContext.Responses.Any(p => p.Instrument.InstrumentId == iid && p.Organization.OrganizationId == orgId && p.Organization.Year == year && p.ResponseVariable == "juvf"))
                {
                    Response resp4c = dataContext.Responses.Where(p => p.Instrument.InstrumentId == iid && p.Organization.OrganizationId == orgId && p.Organization.Year == year && p.ResponseVariable == "juvf").FirstOrDefault();
                    if (resp4c == null || resp4c.ResponseValue == null || resp4c.ResponseValue == "" || resp4c.ResponseValue == "0")
                    {
                        femaleValue = false;  //if we set this to 5 now, the next step of incrementing the page will effectively skip question 5
                    }
                }
                if (!maleValue && !femaleValue)
                {
                    nextPage = 5;
                }
            }
            return nextPage;
        }
        public IActionResult CogTestFinal(int instrumentId, int organizationId, int year)
        {
            Organization org = dataContext.Organizations.Where(o => o.OrganizationId == organizationId && o.Year == year).FirstOrDefault();
            ViewBag.Agency = org.Name;
            ViewBag.State = org.State;
            ViewBag.InstrumentId = instrumentId;
            ViewBag.OrganizationId = organizationId;
            ViewBag.Year = year;

            return View();
        }

        public IActionResult SaveDEDateReceived(string organizationId, string year, string dateReceived)
        {
            Organization currentOrg = dataContext.Organizations.Where(o => o.OrganizationId == Convert.ToInt32(organizationId) && o.Year == Convert.ToInt32(year)).FirstOrDefault();
            if (currentOrg != null)
            {
                OrganizationFollowup foll = dataContext.OrganizationFollowups.FirstOrDefault(x => x.Organization == currentOrg);
                if(foll != null)
                {
                    foll.DateReceived = Convert.ToDateTime(dateReceived);
                    foll.ModifiedBy = User.Identity.Name;
                    foll.ModifiedOn = System.DateTime.Now;

                    dataContext.OrganizationFollowups.Update(foll);
                    dataContext.SaveChanges();
                }
            }
             return new JsonResult("Information Saved.");
        }

        public IActionResult SaveDESubmissionMode(string organizationId, string year, string submissionMode)
        {
            Organization currentOrg = dataContext.Organizations.Where(o => o.OrganizationId == Convert.ToInt32(organizationId) && o.Year == Convert.ToInt32(year)).FirstOrDefault();
            if (currentOrg != null)
            {
                OrganizationFollowup foll = dataContext.OrganizationFollowups.FirstOrDefault(x => x.Organization == currentOrg);
                if (foll != null)
                {
                    foll.SubmissionMode =submissionMode;
                    foll.ModifiedBy = User.Identity.Name;
                    foll.ModifiedOn = System.DateTime.Now;

                    dataContext.OrganizationFollowups.Update(foll);
                    dataContext.SaveChanges();
                }
            }
            return new JsonResult("Information Saved.");
        }

        public IActionResult SaveDESummaryStatus(string organizationId, string year, string summaryStatusId)
        {
            Organization currentOrg = dataContext.Organizations.Where(o => o.OrganizationId == Convert.ToInt32(organizationId) && o.Year == Convert.ToInt32(year)).FirstOrDefault();
            if (currentOrg != null)
            {
                OrganizationFollowup foll = dataContext.OrganizationFollowups.FirstOrDefault(x => x.Organization == currentOrg);
                if (foll != null)
                {
                    foll.SummaryStatusCode = Convert.ToInt32(summaryStatusId);
                    foll.SummaryStatusCodeBy = User.Identity.Name;
                    foll.SummaryStatusCodeDate = System.DateTime.Now;
                    foll.ModifiedBy = User.Identity.Name;
                    foll.ModifiedOn = System.DateTime.Now;
                    dataContext.OrganizationFollowups.Update(foll);
                    dataContext.SaveChanges();
                }
            }
            return new JsonResult("Information Saved.");
        }

        public IActionResult SendConfirmationEmail(int organizationId, int year)
        {
            //get agency status code
            Organization thisOrg = dataContext.Organizations.Where(x => x.OrganizationId == organizationId && x.Year == year).FirstOrDefault();
            OrganizationFollowup fol = dataContext.OrganizationFollowups.FirstOrDefault(x => x.Organization.OrganizationId == organizationId && x.Organization.Year == year);

            int agencystatuscode = 0;
            if (fol != null)
                agencystatuscode = dataContext.OrganizationFollowups.FirstOrDefault(x => x.Organization.OrganizationId == organizationId && x.Organization.Year == year).AgencyStatusCodeId;
            //get the email content from the file
            var fileContents = System.IO.File.ReadAllText("wwwroot/emailtemplates/2018_Confirmation.html");

            //----------------------------------------------
            //parse placeholders in subject
            string subject = "Confirming receipt of the " + year.ToString() + " Annual Summary of Jails form | [[AgencyID]]";
            //organizationid
            subject = subject.Replace("[[AgencyID]]", organizationId.ToString());


            //-----------------------------------------------
            //parse placeholders in body

            //use the data supplier info - if thats not available, then use the POC info
            DataSupplier ds = dataContext.DataSuppliers.Where(x => x.Organization.OrganizationId == organizationId && x.Organization.Year == year).FirstOrDefault();
            OrganizationContacts primaryContact = dataContext.OrganizationContacts.Where(x => x.Organization.OrganizationId == organizationId && x.Organization.Year == year && x.PrimaryContact).FirstOrDefault();

            if (ds != null && ds.Name != null && ds.Name != "")
            {
                fileContents = fileContents.Replace("[[salutation]]", ds.Title);
                fileContents = fileContents.Replace("[[POClastName]]", ds.Name);
            }
            else
            {
             
                //salutation (from primary contact)
                if (primaryContact != null)
                {
                    fileContents = fileContents.Replace("[[salutation]]", primaryContact.Salutation);
                }

                //poc last name
                if (primaryContact != null)
                {
                    fileContents = fileContents.Replace("[[POClastName]]", primaryContact.LastName);
                }
            }
           
            //username
            fileContents = fileContents.Replace("[[UserName]]", thisOrg.UserName);

            //password
            fileContents = fileContents.Replace("[[Password]]", thisOrg.PasswordSecure);

            //------------------------------------------------
            //Construct email and send 
            string sendTo = primaryContact.Email;
            string cc = "doj-dcra@rti.org";
            AppIdentityUser user = _userManager.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            if (Request.Host.ToString().ToLower().Contains("localhost") || Request.Host.ToString().ToLower().Contains("dev") || Request.Host.ToString().ToLower().Contains("stage"))
            {
                sendTo = user.Email;
                cc = "";
            }
            if(sendTo == null || sendTo.Trim() == "")
                return new JsonResult("Email Adress for this agency's(" + organizationId.ToString() + ") primary contact is missing. ");
            else
                emailSender.SendEmail(sendTo, subject, fileContents, cc);


            return new JsonResult("Email sent to primary contact.");
        }
        #region Validations

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Validate_confpopjune(string confpopjune, int instrumentID, int organizationID, int year)
        {
            bool valResult = true;
            if (confpopjune != "" && confpopjune != null)
            {
                int ConfPopJune = Convert.ToInt32(confpopjune);
                //get the values for the first 2 fields
                Response r1 = null;// dataContext.Responses.Where(p => p.Instrument.InstrumentId == instrumentID && p.Question.Variable == "num_deaths_males" && p.Organization.OrganizationId == organizationID && p.Organization.Year == year).FirstOrDefault();
                Response r2 = null;// dataContext.Responses.Where(p => p.Instrument.InstrumentId == instrumentID && p.Question.Variable == "num_deaths_females" && p.Organization.OrganizationId == organizationID && p.Organization.Year == year).FirstOrDefault();
                if (r1 != null && r2 != null)
                {
                    int total = Convert.ToInt32(r1.ResponseValue) + Convert.ToInt32(r2.ResponseValue);

                    if (ConfPopJune > total)
                    {
                        valResult = false;

                    }
                }
                else { valResult = false; }
            }
            else
            {
                valResult = false;

            }
            //  if (valResult)
            return this.Json(new { success = true });

            //else

            //return this.Json(new { success = false, message = "The value should not be greater than the sum of the values entered in questions 1a and 1b.!" });

        }
        #endregion
    }
}
