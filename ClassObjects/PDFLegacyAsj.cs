using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using ASJ.Models;
using ASJ.Models.Form;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace ASJ.ClassObjects
{

    public class PDFLegacyAsj
    {
        private int OrganizationId { get; set; }
        private int InstrumentId { get; set; }
        private int Year { get; set; }
        private IHostingEnvironment _hostingEnvironment { get; set; }
        private string FileName { get; set; }
        private byte[] pdfData { get; set; }

        private ASJLegacyContext dataContext;
        private ASJDbContext asjContext;

        public string downFileName { get; set; }

        public PDFLegacyAsj(int instrument, int organizationId, int year, ASJLegacyContext dbContext, ASJDbContext AsjDbContext, IHostingEnvironment HostingEnvironment, string filename)
        {
            //Instrument = instrument;
            //Organization = organization;
            InstrumentId = instrument;
            Year = year;
            dataContext = dbContext;
            asjContext = AsjDbContext;

            OrganizationId = organizationId;
            _hostingEnvironment = HostingEnvironment;
            FileName = filename;
        }

        public byte[] ReturnPDFdata()
        {

            if (Year == 2017)
            {
                var lst2017 = dataContext.AsjAnnual2017
                    .Where(o => o.OrganizationId == OrganizationId)
                    .ToList();

                GetPDFData(Year, lst2017);
            }
            else if (Year == 2016)
            {
                var lst2016 = dataContext.AsjAnnual2016
                    .Where(o => o.OrganizationId == OrganizationId)
                    .ToList();

                GetPDFData(Year, lst2016);
            }
            else if (Year == 2015)
            {
                var lst2015 = dataContext.AsjAnnual2015
                    .Where(o => o.OrganizationId == OrganizationId)
                    .ToList();

                GetPDFData(Year, lst2015);
            }
            return pdfData;
        }

        private void GetPDFData(int year, IEnumerable<dynamic> lstASJ)
        {

            MemoryStream m = new MemoryStream();
            PdfReader reader = new PdfReader(FileName);
            PdfStamper stamper = new PdfStamper(reader, m);
            AcroFields fields = stamper.AcroFields;


            if (lstASJ.Count() > 1)
            //throw error.  Should only have 1 row here
            {
                //Throw error. 
                return;
            }
            else
            {
                foreach (var item in lstASJ)
                {

                    var datasupplier = asjContext.DataSuppliers
                            .Include(o => o.Organization)
                            .Where(x => x.Organization.OrganizationId == OrganizationId && x.Organization.Year == Year)
                            .FirstOrDefault();

                    if (datasupplier != null)
                    {
                        //Data Supplier!
                        fields.SetField("Name", datasupplier.Name);
                        fields.SetField("Title", datasupplier.Title);
                        fields.SetField("Address", datasupplier.Address);
                        fields.SetField("PhoneNumber", PDFContainer.formatPhone(datasupplier.Phone));
                        fields.SetField("FaxNumber", PDFContainer.formatPhone(datasupplier.Fax));
                        fields.SetField("City", datasupplier.City);
                        fields.SetField("State", datasupplier.State);
                        fields.SetField("Zip", datasupplier.Zip);
                        fields.SetField("Email", datasupplier.email);
                        //fields.SetField("Facility", item.);
                    }

                    if (year <= 2016)
                                           
                        {


                            //Q1
                            fields.SetField("num_deaths_males", item.NumDeathsMales);
                            fields.SetField("num_deaths_females", item.NumDeathsFemales);
                            //Q2
                            fields.SetField("confpopjune", item.Confpopjune);
                            fields.SetField("confpopjune_estimate", item.ConfpopjuneEstimate == true ? "Yes" : "No");
                        }

                    if (year == 2017)
                    {
                        //Q2
                        if (item.Week == "0")
                        {
                            fields.SetField("weekno", "Yes");
                            fields.SetField("weekyes", "No");
                            fields.SetField("weekn", "");
                            fields.SetField("weekn_estimate", "No");
                        }
                        else if (item.Week == "1")
                        {
                            fields.SetField("weekno", "No");
                            fields.SetField("weekyes", "Yes");
                            fields.SetField("weekn", item.Weekn);
                            fields.SetField("weekn_estimate", item.WeeknEstimate == true ? "Yes" : "No");
                        }
                        else
                        {
                            fields.SetField("weekno", "No");
                            fields.SetField("weekyes", "No");
                            fields.SetField("weekn", "");
                            fields.SetField("weekn_estimate", "No");
                        }

                    }
                        //Q3
                        fields.SetField("confpop", item.Confpop);
                        fields.SetField("confpop_estimate", item.ConfpopEstimate == true ? "Yes" : "No");
                        fields.SetField("nconpop", item.Nconpop);
                        fields.SetField("nconpop_estimate", item.NconpopEstimate == true ? "Yes" : "No");
                        fields.SetField("totpop", item.Totpop);
                        fields.SetField("totpop_estimate", item.TotpopEstimate == true ? "Yes" : "No");



                        //Q5
                        fields.SetField("noncitz", item.Noncitz);
                        fields.SetField("noncitz_estimate", item.NoncitzEstimate == true ? "Yes" : "No");
                        //Q6
                        fields.SetField("adultm", item.Adultm);
                        fields.SetField("adultm_estimate", item.AdultmEstimate == true ? "Yes" : "No");
                        fields.SetField("adultf", item.Adultf);
                        fields.SetField("adultf_estimate", item.AdultfEstimate == true ? "Yes" : "No");
                        fields.SetField("juvm", item.Juvm);
                        fields.SetField("juvm_estimate", item.JuvmEstimate == true ? "Yes" : "No");
                        fields.SetField("juvf", item.Juvf);
                        fields.SetField("juvf_estimate", item.JuvfEstimate == true ? "Yes" : "No");
                        if (year <= 2016)
                        {
                            fields.SetField("confpop_6e", item.Confpop6e);
                            fields.SetField("confpop_6e_estimate", item.Confpop6eEstimate == true ? "Yes" : "No");
                        }
                        else
                        {
                            fields.SetField("confpop_4e", item.Confpop4e);
                            fields.SetField("confpop_4e_estimate", item.Confpop4eEstimate == true ? "Yes" : "No");

                        }
                        //Q7
                        fields.SetField("adltjuv", item.Adltjuv);
                        fields.SetField("adltjuv_estimate", item.AdltjuvEstimate == true ? "Yes" : "No");
                        //Q8
                        fields.SetField("conv", item.Conv);
                        fields.SetField("conv_estimate", item.ConvEstimate == true ? "Yes" : "No");
                        fields.SetField("unconv", item.Unconv);
                        fields.SetField("unconv_estimate", item.UnconvEstimate == true ? "Yes" : "No");
                        if (year <= 2016)
                        {
                            fields.SetField("confpop_8c", item.Confpop8c);
                            fields.SetField("confpop_8c_estimate", item.Confpop8cEstimate == true ? "Yes" : "No");
                        }
                        else
                        {
                            fields.SetField("confpop_6c", item.Confpop6c);
                            fields.SetField("confpop_6c_estimate", item.Confpop6cEstimate == true ? "Yes" : "No");
                        }


                        //Q9
                        fields.SetField("felony", item.Felony);
                        fields.SetField("felony_estimate", item.FelonyEstimate == true ? "Yes" : "No");
                        fields.SetField("misd", item.Misd);
                        fields.SetField("misd_estimate", item.MisdEstimate == true ? "Yes" : "No");
                        if (year <= 2016)
                        {
                            fields.SetField("confpop_9d", item.Confpop9d);
                            fields.SetField("confpop_9d_estimate", item.Confpop9dEstimate == true ? "Yes" : "No");
                        }
                        else
                        {
                            fields.SetField("confpop_7d", item.Confpop7d);
                            fields.SetField("confpop_7d_estimate", item.Confpop7dEstimate == true ? "Yes" : "No");
                        }


                        //if (item.otheroffspec != "")
                        //{
                        fields.SetField("otheroff", item.Otheroff);
                        fields.SetField("otheroff_estimate", item.OtheroffEstimate == true ? "Yes" : "No");
                        fields.SetField("otheroffspec", item.Otheroffspec);
                        //}

                        //Q10           
                        fields.SetField("white", item.White);
                        fields.SetField("white_estimate", item.WhiteEstimate == true ? "Yes" : "No");
                        fields.SetField("black", item.Black);
                        fields.SetField("black_estimate", item.BlackEstimate == true ? "Yes" : "No");
                        fields.SetField("hisp", item.Hisp);
                        fields.SetField("hisp_estimate", item.HispEstimate == true ? "Yes" : "No");
                        fields.SetField("AIAN", item.Aian);
                        fields.SetField("AIAN_estimate", item.AianEstimate == true ? "Yes" : "No");
                        fields.SetField("asian", item.Asian);
                        fields.SetField("asian_estimate", item.AsianEstimate == true ? "Yes" : "No");
                        fields.SetField("nhopi", item.Nhopi);
                        fields.SetField("nhopi_estimate", item.NhopiEstimate == true ? "Yes" : "No");
                        fields.SetField("tworace", item.Tworace);
                        fields.SetField("tworace_estimate", item.TworaceEstimate == true ? "Yes" : "No");
                        if (year <= 2016)
                        {
                            fields.SetField("confpop_10j", item.Confpop10j);
                            fields.SetField("confpop_10j_estimate", item.Confpop10jEstimate == true ? "Yes" : "No");
                        }
                        else
                        {
                            fields.SetField("confpop_8j", item.Confpop8j);
                            fields.SetField("confpop_8j_estimate", item.Confpop8jEstimate == true ? "Yes" : "No");

                        }

                        //if (item.otherracespec != "")
                        //{
                        fields.SetField("otherracespec", item.Otherracespec);
                        fields.SetField("otherrace", item.Otherrace);
                        fields.SetField("otherrace_estimate", item.OtherraceEstimate == true ? "Yes" : "No");
                        //}

                        fields.SetField("racedk", item.Racedk);
                        fields.SetField("racedk_estimate", item.RacedkEstimate == true ? "Yes" : "No");

                        //Q11
                        fields.SetField("marshals", item.Marshals);
                        fields.SetField("marshals_estimate", item.MarshalsEstimate == true ? "Yes" : "No");
                        fields.SetField("bop", item.Bop);
                        fields.SetField("bop_estimate", item.BopEstimate == true ? "Yes" : "No");
                        fields.SetField("ice", item.Ice);
                        fields.SetField("ice_estimate", item.IceEstimate == true ? "Yes" : "No");
                        fields.SetField("bia", item.Bia);
                        fields.SetField("bia_estimate", item.BiaEstimate == true ? "Yes" : "No");

                        //if (item.otherfedspec != "")
                        //{
                        fields.SetField("otherfedspec", item.Otherfedspec);
                        fields.SetField("otherfed", item.Otherfed);
                        fields.SetField("otherfed_estimate", item.OtherfedEstimate == true ? "Yes" : "No");
                        //}

                        fields.SetField("instatepris", item.Instatepris);
                        fields.SetField("instatepris_estimate", item.InstateprisEstimate == true ? "Yes" : "No");
                        fields.SetField("outstatepris", item.Outstatepris);
                        fields.SetField("outstatepris_estimate", item.OutstateprisEstimate == true ? "Yes" : "No");

                        fields.SetField("tribal", item.Tribal);
                        fields.SetField("tribal_estimate", item.TribalEstimate == true ? "Yes" : "No");

                        fields.SetField("instatejail", item.Instatejail);
                        fields.SetField("instatejail_estimate", item.InstatejailEstimate == true ? "Yes" : "No");
                        fields.SetField("outstatejail", item.Outstatejail);
                        fields.SetField("outstatejail_estimate", item.OutstatejailEstimate == true ? "Yes" : "No");
                        fields.SetField("otherholdtot", item.Otherholdtot);
                        fields.SetField("otherholdtot_estimate", item.OtherholdtotEstimate == true ? "Yes" : "No");

                        fields.SetField("peakdate", item.Peakdate);
                        fields.SetField("peakpop", item.Peakpop);
                        fields.SetField("peakpop_estimate", item.PeakpopEstimate == true ? "Yes" : "No");

                        //Q13
                        fields.SetField("adpmale", item.Adpmale);
                        fields.SetField("adpmale_estimate", item.AdpmaleEstimate == true ? "Yes" : "No");
                        fields.SetField("adpfemale", item.Adpfemale);
                        fields.SetField("adpfemale_estimate", item.AdpfemaleEstimate == true ? "Yes" : "No");
                        fields.SetField("adp", item.Adp);
                        fields.SetField("adp_estimate", item.AdpEstimate == true ? "Yes" : "No");

                        //Q14
                        fields.SetField("rated", item.Rated);
                        fields.SetField("rated_estimate", item.RatedEstimate == true ? "Yes" : "No");

                        //Q15
                        fields.SetField("admismale", item.Admismale);
                        fields.SetField("admismale_estimate", item.AdmismaleEstimate == true ? "Yes" : "No");
                        fields.SetField("admisfemale", item.Admisfemale);
                        fields.SetField("admisfemale_estimate", item.AdmisfemaleEstimate == true ? "Yes" : "No");
                        fields.SetField("admis", item.Admis);
                        fields.SetField("admis_estimate", item.AdmisEstimate == true ? "Yes" : "No");

                        fields.SetField("releasemale", item.Releasemale);
                        fields.SetField("releasemale_estimate", item.ReleasemaleEstimate == true ? "Yes" : "No");
                        fields.SetField("releasefemale", item.Releasefemale);
                        fields.SetField("releasefemale_estimate", item.ReleasefemaleEstimate == true ? "Yes" : "No");
                        fields.SetField("release", item.Release);
                        fields.SetField("release_estimate", item.ReleaseEstimate == true ? "Yes" : "No");

                        //Q16
                        fields.SetField("emonitor", item.Emonitor);
                        fields.SetField("emonitor_estimate", item.EmonitorEstimate == true ? "Yes" : "No");
                        fields.SetField("homedetn", item.Homedetn);
                        fields.SetField("homedetn_estimate", item.HomedetnEstimate == true ? "Yes" : "No");
                        fields.SetField("commsrv", item.Commsrv);
                        fields.SetField("commsrv_estimate", item.CommsrvEstimate == true ? "Yes" : "No");
                        fields.SetField("dayreport", item.Dayreport);
                        fields.SetField("dayreport_estimate", item.DayreportEstimate == true ? "Yes" : "No");
                        fields.SetField("pretrial", item.Pretrial);
                        fields.SetField("pretrial_estimate", item.PretrialEstimate == true ? "Yes" : "No");
                        fields.SetField("altwork", item.Altwork);
                        fields.SetField("altwork_estimate", item.AltworkEstimate == true ? "Yes" : "No");
                        fields.SetField("treatment", item.Treatment);
                        fields.SetField("treatment_estimate", item.TreatmentEstimate == true ? "Yes" : "No");

                        //if (item.otrnonconfs != "")
                        //{
                        fields.SetField("otrnonconfs", item.Otrnonconfs);
                        fields.SetField("otrnonconf", item.Otrnonconf);
                        fields.SetField("otrnonconf_estimate", item.OtrnonconfEstimate == true ? "Yes" : "No");
                        //}

                        fields.SetField("corrstaff", item.Corrstaff);
                        fields.SetField("corrstaff_estimate", item.CorrstaffEstimate == true ? "Yes" : "No");
                        fields.SetField("corrstaffmale", item.Corrstaffmale);
                        fields.SetField("corrstaffmale_estimate", item.CorrstaffmaleEstimate == true ? "Yes" : "No");
                        fields.SetField("corrstafffemale", item.Corrstafffemale);
                        fields.SetField("corrstafffemale_estimate", item.CorrstafffemaleEstimate == true ? "Yes" : "No");

                        fields.SetField("otherstaff", item.Otherstaff);
                        fields.SetField("otherstaff_estimate", item.OtherstaffEstimate == true ? "Yes" : "No");
                        fields.SetField("otherstaffmale", item.Otherstaffmale);
                        fields.SetField("otherstaffmale_estimate", item.OtherstaffmaleEstimate == true ? "Yes" : "No");
                        fields.SetField("otherstafffemale", item.Otherstafffemale);
                        fields.SetField("otherstafffemale_estimate", item.OtherstafffemaleEstimate == true ? "Yes" : "No");
                        fields.SetField("totalstaff", item.Totalstaff);
                        fields.SetField("totalstaff_estimate", item.TotalstaffEstimate == true ? "Yes" : "No");
                        if (year <= 2016)
                        {
                            fields.SetField("nconpop_16i", item.Nconpop16i);
                            fields.SetField("nconpop_16i_estimate", item.Nconpop16iEstimate == true ? "Yes" : "No");
                        }
                        else
                        {
                            fields.SetField("nconpop_14i", item.Nconpop14i);
                            fields.SetField("nconpop_14i_estimate", item.Nconpop14iEstimate == true ? "Yes" : "No");
                        }

                        // flatten form fields and close document
                        stamper.FormFlattening = true;
                        stamper.Close();

                        pdfData = m.ToArray();
                    }


                }

            }
        }
    }

