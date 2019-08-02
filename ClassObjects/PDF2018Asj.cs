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
        
    public class PDF2018Asj
    {
        private int OrganizationId { get; set; }
        private int InstrumentId { get; set; }
        private int Year { get; set; }
        private IHostingEnvironment _hostingEnvironment { get; set; }
        private string FileName { get; set; }

        private ASJDbContext dataContext;

        public string downFileName { get; set; }

        public PDF2018Asj(int instrument, int organizationId, int year, ASJDbContext dbContext, IHostingEnvironment HostingEnvironment, string filename)
        {
            //Instrument = instrument;
            //Organization = organization;
            InstrumentId = instrument;
            Year = year;
            this.dataContext = dbContext;
            OrganizationId = organizationId;
            _hostingEnvironment = HostingEnvironment;
            FileName = filename;
        }

       
        
        public byte[] returnPDFdata()
        {
            //string sql = "SELECT * FROM dbo.asj_annual_" + Year.ToString() + " WHERE organizationid = " + OrganizationId.ToString();
            ////List<Models.PDF.PDFAnnualASJ> lstASJ = new List<Models.PDF.PDFAnnualASJ>();
            //var lstASJ = from pd in dataContext.PDFAnnualASJs
            //              .FromSql(sql)
            //              .ToList()
            //             select new { pd };

            var lstASJ = dataContext.PDFAnnualASJs
                .Where(y => y.OrganizationId == OrganizationId)
                .ToList();
                

            MemoryStream m = new MemoryStream();
            PdfReader reader = new PdfReader(FileName);
            PdfStamper stamper = new PdfStamper(reader, m);
            AcroFields fields = stamper.AcroFields;


            if (lstASJ.Count() > 1)
            //throw error.  Should only have 1 row here
            {
                
                
                return null;
            }
            else
            {
                foreach (var item in lstASJ)
                {
                    //Data Supplier!
                    fields.SetField("Name", item.Name);
                    fields.SetField("Title", item.Title);
                    fields.SetField("Address", item.Address);
                    fields.SetField("PhoneNumber", PDFContainer.formatPhone(item.Phone));
                    fields.SetField("FaxNumber", PDFContainer.formatPhone(item.Fax));
                    fields.SetField("City", item.City);
                    fields.SetField("State", item.State);
                    fields.SetField("Zip", item.Zip);
                    fields.SetField("Email", item.email);
                    //fields.SetField("Facility", item.);

                    //Q1
                    fields.SetField("confpop", item.confpop);
                    fields.SetField("confpop_estimate", PDFContainer.convertEstimate(item.confpop_estimate));
                    fields.SetField("nconpop", item.nconfpop);
                    fields.SetField("nconpop_estimate", PDFContainer.convertEstimate(item.nconfpop_estimate));
                    fields.SetField("totpop", item.totpop);
                    fields.SetField("totpop_estimate", PDFContainer.convertEstimate(item.totpop_estimate));

                    //Q2
                    if (item.week == "2")
                    {
                        fields.SetField("weekno", "Yes");
                        fields.SetField("weekyes", "No");
                        fields.SetField("weekn", "");
                        fields.SetField("weekn_estimate", "No");
                    }
                    else if (item.week == "1")
                    {
                        fields.SetField("weekno", "No");
                        fields.SetField("weekyes", "Yes");
                        fields.SetField("weekn", item.weekn);
                        fields.SetField("weekn_estimate", PDFContainer.convertEstimate(item.weekn_estimate));
                    }
                    else
                    {
                        fields.SetField("weekno", "No");
                        fields.SetField("weekyes", "No");
                        fields.SetField("weekn", "");
                        fields.SetField("weekn_estimate", "No");
                    }

                    //Q3
                    fields.SetField("noncitz", item.noncitz);
                    fields.SetField("noncitz_estimate", PDFContainer.convertEstimate(item.noncitz_estimate));

                    //Q4
                    fields.SetField("adultm", item.adultm);
                    fields.SetField("adultm_estimate", PDFContainer.convertEstimate(item.adultm_estimate));
                    fields.SetField("adultf", item.adultf);
                    fields.SetField("adultf_estimate", PDFContainer.convertEstimate(item.adultf_estimate));

                    fields.SetField("juvm", item.juvm);
                    fields.SetField("juvm_estimate", PDFContainer.convertEstimate(item.juvm_estimate));
                    fields.SetField("juvf", item.juvf);
                    fields.SetField("juvf_estimate", PDFContainer.convertEstimate(item.juvf_estimate));

                    fields.SetField("totgender", item.totgender);
                    fields.SetField("totgender_estimate", PDFContainer.convertEstimate(item.totgender_estimate));

                    //Q5
                    fields.SetField("adltjuv", item.adltjuv);
                    fields.SetField("adltjuv_estimate", PDFContainer.convertEstimate(item.adltjuv_estimate));

                    //Q6
                    fields.SetField("conv", item.conv);
                    fields.SetField("conv_estimate", PDFContainer.convertEstimate(item.conv_estimate));
                    fields.SetField("unconv", item.unconv);
                    fields.SetField("unconv_estimate", PDFContainer.convertEstimate(item.unconv_estimate));
                    fields.SetField("totconvstatus", item.totconvstatus);
                    fields.SetField("totconvstatus_estimate", PDFContainer.convertEstimate(item.totconvstatus_estimate));

                    //Q7
                    fields.SetField("felony", item.felony);
                    fields.SetField("felony_estimate", PDFContainer.convertEstimate(item.felony_estimate));
                    fields.SetField("misd", item.misd);
                    fields.SetField("misd_estimate", PDFContainer.convertEstimate(item.misd_estimate));
                    fields.SetField("otheroff", item.otheroff);
                    fields.SetField("otheroffspec", item.otheroffspec);
                    fields.SetField("otheroff_estimate", PDFContainer.convertEstimate(item.otheroff_estimate));
                    fields.SetField("totoff", item.totoff);
                    fields.SetField("totoff_estimate", PDFContainer.convertEstimate(item.totoff_estimate));

                    //Q8
                    fields.SetField("white", item.white);
                    fields.SetField("white_estimate", PDFContainer.convertEstimate(item.white_estimate));
                    fields.SetField("black", item.black);
                    fields.SetField("black_estimate", PDFContainer.convertEstimate(item.black_estimate));
                    fields.SetField("hisp", item.hisp);
                    fields.SetField("hisp_estimate", PDFContainer.convertEstimate(item.hisp_estimate));
                    fields.SetField("AIAN", item.AIAN);
                    fields.SetField("AIAN_estimate", PDFContainer.convertEstimate(item.AIAN_estimate));
                    fields.SetField("asian", item.asian);
                    fields.SetField("asian_estimate", PDFContainer.convertEstimate(item.asian_estimate));
                    fields.SetField("nhopi", item.nhopi);
                    fields.SetField("nhopi_estimate", PDFContainer.convertEstimate(item.nhopi_estimate));
                    fields.SetField("tworace", item.tworace);
                    fields.SetField("tworace_estimate", PDFContainer.convertEstimate(item.tworace_estimate));
                    fields.SetField("otherrace", item.otherrace);
                    fields.SetField("otherracespec", item.otherracespec);
                    fields.SetField("otherrace_estimate", PDFContainer.convertEstimate(item.otheroff_estimate));
                    fields.SetField("racedk", item.racedk);
                    fields.SetField("racedk_estimate", PDFContainer.convertEstimate(item.racedk_estimate));
                    fields.SetField("racetotal", item.racetotal);
                    fields.SetField("racetotal_estimate", PDFContainer.convertEstimate(item.racetotal_estimate));


                    //Q9
                    fields.SetField("marshals", item.marshals);
                    fields.SetField("marshals_estimate", PDFContainer.convertEstimate(item.marshals_estimate));
                    fields.SetField("bop", item.bop);
                    fields.SetField("bop_estimate", PDFContainer.convertEstimate(item.bop_estimate));
                    fields.SetField("ice", item.ice);
                    fields.SetField("ice_estimate", PDFContainer.convertEstimate(item.ice_estimate));
                    fields.SetField("bia", item.bia);
                    fields.SetField("bia_estimate", PDFContainer.convertEstimate(item.bia_estimate));
                    fields.SetField("otherfed", item.otherfed);
                    fields.SetField("otherfedspec", item.otherfedspec);
                    fields.SetField("otherfed_estimate", PDFContainer.convertEstimate(item.otherfed_estimate));
                    fields.SetField("instatepris", item.instatepris);
                    fields.SetField("instatepris_estimate", PDFContainer.convertEstimate(item.instatepris_estimate));
                    fields.SetField("outstatepris", item.outstatepris);
                    fields.SetField("outstatepris_estimate", PDFContainer.convertEstimate(item.outstatepris_estimate));
                    fields.SetField("tribal", item.tribal);
                    fields.SetField("tribal_estimate", PDFContainer.convertEstimate(item.tribal_estimate));
                    fields.SetField("instatejail", item.instatejail);
                    fields.SetField("instatejail_estimate", PDFContainer.convertEstimate(item.instatejail_estimate));
                    fields.SetField("outstatejail", item.outstatejail);
                    fields.SetField("outstatejail_estimate", PDFContainer.convertEstimate(item.outstatejail_estimate));
                    fields.SetField("otherholdtot", item.otherholdtot);
                    fields.SetField("otherholdtot_estimate", PDFContainer.convertEstimate(item.otherholdtot));

                    //Q10
                    fields.SetField("peakdate", item.peakdate);
                    fields.SetField("peakpop", item.peakpop);
                    fields.SetField("peakpop_estimate", PDFContainer.convertEstimate(item.peakpop_estimate));

                    //Q11
                    fields.SetField("adpmale", item.adpmale);
                    fields.SetField("adpmale_estimate", PDFContainer.convertEstimate(item.adpmale_estimate));
                    fields.SetField("adpfemale", item.adpfemale);
                    fields.SetField("adpfemale_estimate", PDFContainer.convertEstimate(item.adpfemale_estimate));
                    fields.SetField("adp", item.adp);
                    fields.SetField("adp_estimate", PDFContainer.convertEstimate(item.adp_estimate));

                    //Q12
                    fields.SetField("rated", item.rated);
                    fields.SetField("rated_estimate", PDFContainer.convertEstimate(item.rated_estimate));

                    //Q13
                    fields.SetField("admismale", item.admismale);
                    fields.SetField("admismale_estimate", PDFContainer.convertEstimate(item.admismale_estimate));
                    fields.SetField("admisfemale", item.admisfemale);
                    fields.SetField("admisfemale_estimate", PDFContainer.convertEstimate(item.admisfemale_estimate));
                    fields.SetField("admis", item.admis);
                    fields.SetField("admis_estimate", PDFContainer.convertEstimate(item.admis_estimate));
                    fields.SetField("releasemale", item.releasemale);
                    fields.SetField("releasemale_estimate", PDFContainer.convertEstimate(item.releasemale_estimate));
                    fields.SetField("releasefemale", item.releasefemale);
                    fields.SetField("releasefemale_estimate", PDFContainer.convertEstimate(item.releasefemale_estimate));
                    fields.SetField("release", item.release);
                    fields.SetField("release_estimate", PDFContainer.convertEstimate(item.release_estimate));

                    //Q14
                    fields.SetField("emonitor", item.emonitor);
                    fields.SetField("emonitor_estimate", PDFContainer.convertEstimate(item.emonitor_estimate));
                    fields.SetField("homedetn", item.homedetn);
                    fields.SetField("homedetn_estimate", PDFContainer.convertEstimate(item.homedetn_estimate));
                    fields.SetField("commsrv", item.commsrv);
                    fields.SetField("commsrv_estimate", PDFContainer.convertEstimate(item.commsrv_estimate));
                    fields.SetField("dayreport", item.dayreport);
                    fields.SetField("dayreport_estimate", PDFContainer.convertEstimate(item.dayreport_estimate));
                    fields.SetField("pretrial", item.pretrial);
                    fields.SetField("pretrial_estimate", PDFContainer.convertEstimate(item.pretrial_estimate));
                    fields.SetField("altwork", item.altwork);
                    fields.SetField("altwork_estimate", PDFContainer.convertEstimate(item.altwork_estimate));
                    fields.SetField("treatment", item.treatment);
                    fields.SetField("treatment_estimate", PDFContainer.convertEstimate(item.treatment_estimate));
                    fields.SetField("otrnonconf", item.otrnonconf);
                    fields.SetField("otrnonconf_estimate", PDFContainer.convertEstimate(item.otrnonconf_estimate));
                    fields.SetField("otrnonconfs", item.otrnonconfs);
                    fields.SetField("nonconfd", item.nonconfd);
                    fields.SetField("nonconfd_estimate", PDFContainer.convertEstimate(item.nonconfd_estimate));

                    //Q15
                    fields.SetField("corrstaff", item.corrstaff);
                    fields.SetField("corrstaff_estimate", PDFContainer.convertEstimate(item.corrstaff_estimate));
                    fields.SetField("corrstaffmale", item.corrstaffmale);
                    fields.SetField("corrstaffmale_estimate", PDFContainer.convertEstimate(item.corrstaffmale_estimate));
                    fields.SetField("corrstaffmale", item.corrstaffmale);
                    fields.SetField("corrstaffmale_estimate", PDFContainer.convertEstimate(item.corrstaffmale_estimate));
                    fields.SetField("corrstafffemale", item.corrstafffemale);
                    fields.SetField("corrstafffemale_estimate", PDFContainer.convertEstimate(item.corrstaffmale_estimate));
                    fields.SetField("otherstaff", item.otherstaff);
                    fields.SetField("otherstaff_estimate", PDFContainer.convertEstimate(item.otherstaff_estimate));
                    fields.SetField("otherstaffmale", item.otherstaffmale);
                    fields.SetField("otherstaffmale_estimate", PDFContainer.convertEstimate(item.otherstaffmale_estimate));
                    fields.SetField("otherstafffemale", item.otherstafffemale);
                    fields.SetField("otherstafffemale_estimate", PDFContainer.convertEstimate(item.otherstafffemale_estimate));
                    fields.SetField("totalstaff", item.totalstaff);
                    fields.SetField("totalstaff_estimate", PDFContainer.convertEstimate(item.totalstaff_estimate));

                    //Done!
                }
                stamper.FormFlattening = true;
                stamper.Close();

                byte[] thePDF = m.ToArray();

                return thePDF;

            }
        }
    }
}
