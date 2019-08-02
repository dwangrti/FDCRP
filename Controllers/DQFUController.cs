using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ASJ.Models;
using ASJ.ViewModels.Form;
using ASJ.Models.Form;
using ASJ.ViewModels.Client;
using Microsoft.AspNetCore.Authorization;

namespace ASJ.Controllers
{
    [Authorize]
    public class DQFUController : Controller
    {
        private readonly ASJDbContext dataContext;
        private ASJLegacyContext legacyContext;

        public DQFUController(ASJDbContext context, ASJLegacyContext legContext)
        {
            dataContext = context;
            legacyContext = legContext;
        }

        public IActionResult Index(int organizationId, int iid = 1)
        {
            //security check
            if (!Utils.Extensions.RespondentSecurityCheck(User, organizationId))
                return RedirectToAction("AccessDenied", "Security");

            //save action for poc role only
            Instrument ins = dataContext.Instruments.FirstOrDefault(i => i.InstrumentId == iid);
            if (User.IsInRole("poc"))
            {
                InstrumentActionLog act = new InstrumentActionLog
                {
                    OrganizationId = organizationId,
                    Year = ins.Year,
                    InstrumentId = iid,
                    Action = "iDQFUStart",
                    CurrentPage = 0,
                    NextPage = 0,
                    CreatedBy = User.Identity.Name,
                    CreatedDate = System.DateTime.Now
                };
                dataContext.InstrumentActionLogs.Attach(act);
                dataContext.SaveChanges();
            }

            string sql = "SELECT * FROM [dbo].[iDQFUvalues] where OrganizationID=" + organizationId.ToString();
            DQFUvalue val = dataContext.DQFUvalues.FromSql(sql).FirstOrDefault();
            DQFUViewModel dvm = new DQFUViewModel();
            dvm.Values = val;
            dvm.InsturmentId = iid;
            CalculateAndCheck(dvm);

            //save the flag
            bool iDQFUFlag = false;
            if (dvm.FlagAdmis || dvm.FlagAdmisFemale || dvm.FlagAdmisMale || dvm.FlagAdp || dvm.FlagAdpFemale || dvm.FlagAdpMale || dvm.FlagConpop || dvm.FlagFemale || dvm.FlagMale || dvm.FlagNconpop || dvm.FlagRated
                || dvm.FlagRelease || dvm.FlagReleaseFemale || dvm.FlagReleaseMale)
            {
                iDQFUFlag = true;
            }

            //tetsing
            //dvm.FlagAdmis = true;
            //dvm.FlagAdp = true;
            //dvm.FlagRated = true;
            //dvm.FlagRelease = true;
            //dvm.FlagNconpop = true;

            //retrieve the explanation if any
            Response expResp = dataContext.Responses.Where(p => p.Instrument.InstrumentId == iid && p.ResponseVariable == "iDQFU_conpop_explanation" && p.Organization.OrganizationId == organizationId).FirstOrDefault();
            if (expResp != null)
                dvm.iDQFU_Conpop_Explanation = expResp.ResponseValue;

            expResp = dataContext.Responses.Where(p => p.Instrument.InstrumentId == iid && p.ResponseVariable == "iDQFU_nconpop_explanation" && p.Organization.OrganizationId == organizationId).FirstOrDefault();
            if (expResp != null)
                dvm.iDQFU_Nconpop_Explanation = expResp.ResponseValue;

            expResp = dataContext.Responses.Where(p => p.Instrument.InstrumentId == iid && p.ResponseVariable == "iDQFU_admis_explanation" && p.Organization.OrganizationId == organizationId).FirstOrDefault();
            if (expResp != null)
                dvm.iDQFU_Admis_Explanation = expResp.ResponseValue;

            expResp = dataContext.Responses.Where(p => p.Instrument.InstrumentId == iid && p.ResponseVariable == "iDQFU_adp_explanation" && p.Organization.OrganizationId == organizationId).FirstOrDefault();
            if (expResp != null)
                dvm.iDQFU_Adp_Explanationn = expResp.ResponseValue;

            expResp = dataContext.Responses.Where(p => p.Instrument.InstrumentId == iid && p.ResponseVariable == "iDQFU_release_explanation" && p.Organization.OrganizationId == organizationId).FirstOrDefault();
            if (expResp != null)
                dvm.iDQFU_Release_Explanation = expResp.ResponseValue;

            expResp = dataContext.Responses.Where(p => p.Instrument.InstrumentId == iid && p.ResponseVariable == "iDQFU_rated_explanation" && p.Organization.OrganizationId == organizationId).FirstOrDefault();
            if (expResp != null)
                dvm.iDQFU_Rated_Explanation = expResp.ResponseValue;

            //save it to the respons table
            FormController.SaveResponse(dataContext, ins, organizationId, "iDQFUFlag", iDQFUFlag.ToString(), User.Identity.Name);

            return View(dvm);
        }

        /// <summary>
        /// Show Quality Control Error details
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="iid"></param>
        /// <returns></returns>
        public IActionResult QCDetail(int organizationId, int iid = 1)
        {
            //security check
            if (!Utils.Extensions.RespondentSecurityCheck(User, organizationId))
                return RedirectToAction("AccessDenied", "Security");

            //save action for poc role only
            Instrument ins = dataContext.Instruments.FirstOrDefault(i => i.InstrumentId == iid);
            //if (User.IsInRole("poc"))
            //{
                List<QCDetailViewModel> detailList = new List<QCDetailViewModel>();

                if (iid != 0 && organizationId != 0)
                {
                    foreach (OrganizationQCDetails d in dataContext.OrganizationQCDetails.Where(p => p.Instrument.InstrumentId == iid && p.Organization.OrganizationId == organizationId).Include(p => p.Organization).Include(p => p.Instrument).AsNoTracking().OrderBy(p => p.Location).ToList())
                    {
                        QCDetailViewModel errorDetail = new QCDetailViewModel();
                        errorDetail.Instrument = d.Instrument;
                        errorDetail.Organization = d.Organization;
                        errorDetail.OrganizationId = d.Organization.OrganizationId;
                        errorDetail.Agency = d.Organization.Name;
                        errorDetail.Location = d.Location;
                        errorDetail.PYrange = d.PYrange;
                        errorDetail.QCDetails = d.QCDetails;
                        errorDetail.Year = d.Instrument.Year;
                        errorDetail.CYrange = d.CYrange;
                        errorDetail.FirstAppeared = d.FirstAppeared;
                        detailList.Add(errorDetail);
                    }
                    return View("QCDetail", detailList);
                }
            //}
            return null;
        }


        /// <summary>
        /// Show user entered reasons for each page and on the DQFU page
        /// using the view, requires too much hard coding, don't like it ....
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="iid"></param>
        /// <returns></returns>
        //public IActionResult QCExplanationOld(int organizationId, int iid = 1)
        //{
        //    //security check
        //    if (!Utils.Extensions.RespondentSecurityCheck(User, organizationId))
        //        return RedirectToAction("AccessDenied", "Security");

        //    //save action for poc role only
        //    Instrument ins = dataContext.Instruments.FirstOrDefault(i => i.InstrumentId == iid);
        //    //if (User.IsInRole("poc"))
        //    //{

        //    if (iid != 0 && organizationId != 0)
        //    {
        //        string sql = "SELECT * FROM [dbo].[asj_annual_2018_explanations] where OrganizationID=" + organizationId.ToString();
        //        AnnualExplanation ae = dataContext.AnnualExplanations.FromSql(sql).FirstOrDefault();
        //        QCExplanationViewModel qvm = new QCExplanationViewModel();
        //        qvm.ExplanationsList = new List<QuestionExplanation>;

        //        qvm.Instrument = ins;
        //        qvm.OrganizationId = organizationId;
        //        qvm.Agency = "";
        //        for (int i=0; i<16; i++)
        //        {
        //            string varName = "explanation_p" + i.ToString();
        //            qvm.ExplanationsList.Add(new QuestionExplanation(i.ToString(), ae.explanation_p1));
        //        }

        //        return View("QCDetail", qvm);
        //    }
        //    //}
        //    return null;
        //}

        /// <summary>
        /// Show user entered reasons for each page and on the DQFU page
        /// Since we combined Q14a and Q14b into one page, the order is off by one
        /// p14 was not used,        Page 14 - p15        Page 15 - p16
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="iid"></param>
        /// <returns></returns>
        public IActionResult QCExplanation(int organizationId, int iid = 1, int previid=0)
        {
            //security check
            if (!Utils.Extensions.RespondentSecurityCheck(User, organizationId))
                return RedirectToAction("AccessDenied", "Security");

            //save action for poc role only
            Instrument ins = dataContext.Instruments.FirstOrDefault(i => i.InstrumentId == iid);

            Instrument prevIns = null;
            List<AsjAnnual2017Reasons> prevReasons = null;
            //get the preious year instrument, if it is 0, then it's 2018 which previous data in asj_annual_2017_reasons table
            if (previid>0)
            {
                prevIns = dataContext.Instruments.FirstOrDefault(i => i.InstrumentId == previid);
            }
            else
            {
               prevReasons = legacyContext.AsjAnnual2017Reasons.Where(p => p.OrganizationId == organizationId).ToList();
            }

            Organization org = dataContext.Organizations.FirstOrDefault(i => i.OrganizationId == organizationId);
            QCExplanationViewModel qvm = new QCExplanationViewModel();
            qvm.ExplanationsList = new List<QuestionExplanation>();
            qvm.Instrument = ins;
            qvm.Agency = org.Name;
            qvm.OrganizationId = organizationId;

            List<Response> responses = dataContext.Responses.Where(p => p.Instrument.InstrumentId == iid && p.Organization.OrganizationId == organizationId && p.ResponseVariable.Contains("explanation_")).OrderBy(p => p.ResponseVariable).ToList();
            List<Response> prevRsponses = null;
            if (previid>0)
            {
                prevRsponses = dataContext.Responses.Where(p => p.Instrument.InstrumentId == previid && p.Organization.OrganizationId == organizationId && p.ResponseVariable.Contains("explanation_")).OrderBy(p => p.ResponseVariable).ToList();
            }
            //15 is ordered before 2, 3 4, etc so we had to do this
            if (responses != null && responses.Count > 0 || (prevRsponses != null && prevRsponses.Count > 0))
            {
                for (int i = 1; i <=16; i++)
                {
                    if (i == 14) //13a and 13b are now on the same page so no page 14, page 15 Q14 and page 16 is q15
                        continue;

                    QuestionExplanation qe = new QuestionExplanation(i.ToString(), "", "");
                    string varName = "explanation_p" + i.ToString();
                    string oldVarName =  i.ToString();
                    if (i == 13)
                        oldVarName = "13a";
                    if (i > 14)
                    {
                        qe = new QuestionExplanation((i-1).ToString(), "", "");
                        oldVarName = (i-1).ToString();
                    }

                    if (responses != null)
                    {
                        foreach (Response r in responses)
                        {
                            if (r.ResponseVariable == varName)
                            {
                                if (r.ResponseValue != null && r.ResponseValue.Trim() != "")
                                {
                                    qe.Explananation = r.ResponseValue.Trim();
                                    break;
                                }
                            }
                        }
                    }

                    if (prevRsponses != null)
                    {
                        foreach (Response r in prevRsponses)
                        {
                            if (r.ResponseVariable == varName)
                            {
                                if (r.ResponseValue != null && r.ResponseValue.Trim() != "")
                                {
                                    qe.PrevExplananation = r.ResponseValue.Trim();
                                    break;
                                }
                            }
                        }
                    }

                    if (prevReasons != null)
                    {
                        foreach (AsjAnnual2017Reasons r in prevReasons)
                        {
                            if (r.Question == oldVarName && i != 13)
                            {
                                if (r.Reason != null && r.Reason.Trim() != "")
                                {
                                    qe.PrevExplananation = r.Reason.Trim();
                                    break;
                                }
                            }
                            else if (i == 13)
                            {
                                if (r.Question == "13a" && r.Reason != null && r.Reason.Trim() != "")
                                    qe.PrevExplananation = r.Reason.Trim();
                                if (r.Question == "13b" && r.Reason != null && r.Reason.Trim() != "")
                                    qe.PrevExplananation = qe.PrevExplananation + r.Reason.Trim();
                            }
                        }
                    }

                    if (qe.Explananation != "" || qe.PrevExplananation != "")
                        qvm.ExplanationsList.Add(qe);
                }
            }


            //handle the DQFU explnanations, not available for 2017
            responses = dataContext.Responses.Where(p => p.Instrument.InstrumentId == iid && p.Organization.OrganizationId == organizationId && p.ResponseVariable.Contains("iDQFU_")).ToList();
            if (previid > 0)
            {
                prevRsponses = dataContext.Responses.Where(p => p.Instrument.InstrumentId == previid && p.Organization.OrganizationId == organizationId && p.ResponseVariable.Contains("iDQFU_")).ToList();
            }

            if ((responses != null && responses.Count > 0) || (prevRsponses!=null && prevRsponses.Count>0))
            {
                //let's define the 6 elements first
                QuestionExplanation q1 = new QuestionExplanation("Q1: Confined Count", "", "");
                QuestionExplanation q11 = new QuestionExplanation("Q11: Average Daily Population", "", "");
                QuestionExplanation q12 = new QuestionExplanation("Q12: Rated Capacity", "", "");
                QuestionExplanation q13a = new QuestionExplanation("Q13A: Admitted Count", "", "");
                QuestionExplanation q13b = new QuestionExplanation("Q13B: Discharged Count", "", "");
                QuestionExplanation q14 = new QuestionExplanation("Q14: Not Confined Count", "", "");

                if (responses != null)
                {
                    foreach (Response r in responses)
                    {
                        switch (r.ResponseVariable)
                        {
                            case "iDQFU_conpop_explanation":
                                // qe.QuestionId = "Q1: Confined Count";
                                q1.Explananation = r.ResponseValue.Trim();
                                break;
                            case "iDQFU_nconpop_explanation":
                                // qe.QuestionId = "Q14: Not Confined Count";
                                q14.Explananation = r.ResponseValue.Trim();
                                break;
                            case "iDQFU_adp_explanation":
                                //qe.QuestionId = "Q11: Average Daily Population";
                                q11.Explananation = r.ResponseValue.Trim();
                                break;
                            case "iDQFU_admis_explanation":
                                //qe.QuestionId = "Q13A: Admitted Count";
                                q13a.Explananation = r.ResponseValue.Trim();
                                break;
                            case "iDQFU_release_explanation":
                                //qe.QuestionId = "Q13B: Discharged Count";
                                q13b.Explananation = r.ResponseValue.Trim();
                                break;
                            case "iDQFU_rated_explanation":
                                //qe.QuestionId = "Q12: Rated Capacity";
                                q12.Explananation = r.ResponseValue.Trim();
                                break;
                            default:
                                break;
                        }
                    }
                }

                if (prevRsponses != null)
                {
                    foreach (Response r in prevRsponses)
                    {
                        switch (r.ResponseVariable)
                        {
                            case "iDQFU_conpop_explanation":
                                // qe.QuestionId = "Q1: Confined Count";
                                q1.PrevExplananation = r.ResponseValue.Trim();
                                break;
                            case "iDQFU_nconpop_explanation":
                                // qe.QuestionId = "Q14: Not Confined Count";
                                q14.PrevExplananation = r.ResponseValue.Trim();
                                break;
                            case "iDQFU_adp_explanation":
                                //qe.QuestionId = "Q11: Average Daily Population";
                                q11.PrevExplananation = r.ResponseValue.Trim();
                                break;
                            case "iDQFU_admis_explanation":
                                //qe.QuestionId = "Q13A: Admitted Count";
                                q13a.PrevExplananation = r.ResponseValue.Trim();
                                break;
                            case "iDQFU_release_explanation":
                                //qe.QuestionId = "Q13B: Discharged Count";
                                q13b.PrevExplananation = r.ResponseValue.Trim();
                                break;
                            case "iDQFU_rated_explanation":
                                //qe.QuestionId = "Q12: Rated Capacity";
                                q12.PrevExplananation = r.ResponseValue.Trim();
                                break;
                            default:
                                break;
                        }
                    }
                }

                if (q1.Explananation != "" || q1.PrevExplananation != "")
                    qvm.ExplanationsList.Add(q1);
                if (q11.Explananation != "" || q11.PrevExplananation != "")
                    qvm.ExplanationsList.Add(q11);
                if (q12.Explananation != "" || q12.PrevExplananation != "")
                    qvm.ExplanationsList.Add(q12);
                if (q13a.Explananation != "" || q13a.PrevExplananation != "")
                    qvm.ExplanationsList.Add(q13a);
                if (q13b.Explananation != "" || q13b.PrevExplananation != "")
                    qvm.ExplanationsList.Add(q13b);
                if (q14.Explananation != "" || q14.PrevExplananation != "")
                    qvm.ExplanationsList.Add(q14);
            }

            return View("QCExplanation", qvm);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveReason(DQFUViewModel model)
        {
            //check to see what reasons are provided 
            Instrument ins = dataContext.Instruments.AsNoTracking().SingleOrDefault(p => p.InstrumentId == model.InsturmentId);
            if (model.iDQFU_Conpop_Explanation !=null && model.iDQFU_Conpop_Explanation.Length>0)
            {
                FormController.SaveResponse(dataContext, ins, model.Values.OrganizationID, "iDQFU_conpop_explanation", model.iDQFU_Conpop_Explanation, User.Identity.Name);
            }

            if (model.iDQFU_Nconpop_Explanation != null && model.iDQFU_Nconpop_Explanation.Length > 0)
            {
                FormController.SaveResponse(dataContext, ins, model.Values.OrganizationID, "iDQFU_nconpop_explanation", model.iDQFU_Nconpop_Explanation, User.Identity.Name);
            }

            if (model.iDQFU_Admis_Explanation != null && model.iDQFU_Admis_Explanation.Length > 0)
            {
                FormController.SaveResponse(dataContext, ins, model.Values.OrganizationID, "iDQFU_admis_explanation", model.iDQFU_Admis_Explanation, User.Identity.Name);
            }

            if (model.iDQFU_Adp_Explanationn != null && model.iDQFU_Adp_Explanationn.Length > 0)
            {
                FormController.SaveResponse(dataContext, ins, model.Values.OrganizationID, "iDQFU_adp_explanation", model.iDQFU_Adp_Explanationn, User.Identity.Name);
            }

            if (model.iDQFU_Release_Explanation != null && model.iDQFU_Release_Explanation.Length > 0)
            {
                FormController.SaveResponse(dataContext, ins, model.Values.OrganizationID, "iDQFU_release_explanation", model.iDQFU_Release_Explanation, User.Identity.Name);
            }

            if (model.iDQFU_Rated_Explanation != null && model.iDQFU_Rated_Explanation.Length > 0)
            {
                FormController.SaveResponse(dataContext, ins, model.Values.OrganizationID, "iDQFU_rated_explanation", model.iDQFU_Rated_Explanation, User.Identity.Name);
            }
            //save action for poc role only
            if (User.IsInRole("poc"))
            {
                InstrumentActionLog act = new InstrumentActionLog
                {
                    OrganizationId = model.Values.OrganizationID,
                    Year = ins.Year,
                    InstrumentId = ins.InstrumentId,
                    Action = "iDQFUSave",
                    CurrentPage = 0,
                    NextPage = 0,
                    CreatedBy = User.Identity.Name,
                    CreatedDate = System.DateTime.Now
                };
                dataContext.InstrumentActionLogs.Attach(act);
                dataContext.SaveChanges();
            }
            return View("Confirmation");
            //return RedirectToAction("Index", new { organizationId = model.Values.OrganizationID, iid = model.InsturmentId });
        }


        private void CalculateAndCheck(DQFUViewModel dvm)
        {
            DQFUvalue val = dvm.Values;

            //calculate Confined ratios, check for nulls
            if (val.CURRconfpop != null && val.PREVconfpop != null)
                dvm.FlagConpop = CheckRule(val.CURRconfpop.GetValueOrDefault(), val.PREVconfpop.GetValueOrDefault());

            //calculate Confined male ratios
            if (val.CURRadultm != null && val.CURRjuvm != null && val.PREVadultm != null && val.PREVjuvm != null)
                dvm.FlagMale = CheckRule(val.CURRadultm.GetValueOrDefault() + val.CURRjuvm.GetValueOrDefault(), val.PREVadultm.GetValueOrDefault() + val.PREVjuvm.GetValueOrDefault());

            //calculate Confined female ratios
            if (val.CURRadultf != null && val.CURRjuvf != null && val.PREVadultf != null && val.PREVjuvf != null)
                dvm.FlagFemale = CheckRule(val.CURRadultf.GetValueOrDefault() + val.CURRjuvf.GetValueOrDefault(), val.PREVadultf.GetValueOrDefault() + val.PREVjuvf.GetValueOrDefault());

            //calculate Nonconfined ratios
            if (val.CURRnconpop != null && val.PREVnconpop != null)
                dvm.FlagNconpop = CheckRule(val.CURRnconpop.GetValueOrDefault(), val.PREVnconpop.GetValueOrDefault());

            //calculate ADP ratios, these could have used a common method
            if (val.CURRadp != null && val.PREVadp != null)
                dvm.FlagAdp = CheckRule(val.CURRadp.GetValueOrDefault(), val.PREVadp.GetValueOrDefault());

            //calculate ADP Male ratios, could share a common method
            if (val.CURRadpmale != null && val.PREVadpmale != null)
                dvm.FlagAdpMale = CheckRule(val.CURRadpmale.GetValueOrDefault(), val.PREVadpmale.GetValueOrDefault());

            //calculate ADP female ratios, could share a common method
            if (val.CURRadpfemale != null && val.PREVadpfemale != null)
                dvm.FlagAdpFemale = CheckRule(val.CURRadpfemale.GetValueOrDefault(), val.PREVadpfemale.GetValueOrDefault());

            //calculate Rated ratios, could share a common method
            if (val.CURRrated != null && val.PREVrated != null)
                dvm.FlagRated = CheckRule(val.CURRrated.GetValueOrDefault(), val.PREVrated.GetValueOrDefault());

            //calculate Admission
            //dvm.RatioAdmis = CalculateRatio(val.CURRadmis, val.PREVadmis);
            if (val.CURRadmis != null && val.PREVadmis != null)
                dvm.FlagAdmis = CheckRule2(val.CURRadmis.GetValueOrDefault(), val.PREVadmis.GetValueOrDefault());
            if (val.CURRadmismale != null && val.PREVadmismale != null)
                dvm.FlagAdmisMale = CheckRule2(val.CURRadmismale.GetValueOrDefault(), val.PREVadmismale.GetValueOrDefault());
            if (val.CURRadmisfemale != null && val.PREVadmisfemale != null)
                dvm.FlagAdmisFemale = CheckRule2(val.CURRadmisfemale.GetValueOrDefault(), val.PREVadmisfemale.GetValueOrDefault());

            //calculate Release
            if (val.CURRrelease != null && val.PREVrelease != null)
                dvm.FlagRelease = CheckRule2(val.CURRrelease.GetValueOrDefault(), val.PREVrelease.GetValueOrDefault());
            if (val.CURRreleasemale != null && val.PREVreleasemale != null)
                dvm.FlagReleaseMale = CheckRule2(val.CURRreleasemale.GetValueOrDefault(), val.PREVreleasemale.GetValueOrDefault());
            if (val.CURRreleasefemale != null && val.PREVreleasefemale != null)
                dvm.FlagReleaseFemale = CheckRule2(val.CURRreleasefemale.GetValueOrDefault(), val.PREVreleasefemale.GetValueOrDefault());
        }


        private double CalculateRatio(int curr, int prev)
        {
            return 100 * Math.Abs(curr - prev) / Math.Max(Math.Min(curr, prev), 1);
        }


        private bool CheckRule(int curr, int prev)
        {
            double ratio = 100 * Math.Abs(curr - prev) / Math.Max(Math.Min(curr, prev), 1);
            if (((curr >= 0 && curr <= 20) && Math.Abs(curr - prev) >= 20) ||
               ((curr > 20 && curr <= 50) && ratio >= 100) ||
               ((curr > 50 && curr <= 100) && ratio >= 75) ||
               ((curr > 100 && curr <= 250) && ratio >= 50) ||
               ((curr > 250 && curr <= 500) && ratio >= 40) ||
               ((curr > 500 && curr <= 1000) && ratio >= 30) ||
               (curr > 1000 && ratio >= 20))
            {
                return true;
            }
            else
                return false;
        }

        private bool CheckRule(double curr, double prev)
        {
            double ratio = 100 * Math.Abs(curr - prev) / Math.Max(Math.Min(curr, prev), 1);
            if (((curr >= 0 && curr <= 20) && Math.Abs(curr - prev) >= 20) ||
               ((curr > 20 && curr <= 50) && ratio >= 100) ||
               ((curr > 50 && curr <= 100) && ratio >= 75) ||
               ((curr > 100 && curr <= 250) && ratio >= 50) ||
               ((curr > 250 && curr <= 500) && ratio >= 40) ||
               ((curr > 500 && curr <= 1000) && ratio >= 30) ||
               (curr > 1000 && ratio >= 20))
            {
                return true;
            }
            else
                return false;
        }

        private bool CheckRule2(int curr, int prev)
        {
            double ratio = 100 * Math.Abs(curr - prev) / Math.Max(Math.Min(curr, prev), 1);
            if (((curr >= 0 && curr <= 20) && Math.Abs(curr - prev) >= 50) ||
               ((curr > 20 && curr <= 50) && ratio >= 200) ||
               ((curr > 50 && curr <= 100) && ratio >= 150) ||
               ((curr > 100 && curr <= 250) && ratio >= 100) ||
               ((curr > 250 && curr <= 500) && ratio >= 80) ||
               ((curr > 500 && curr <= 1000) && ratio >= 60) ||
               (curr > 1000 && ratio >= 40))
            {
                return true;
            }
            else
                return false;
        }

 
    }
}
