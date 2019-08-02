using System.Collections.Generic;
using System.IO;
using System.Linq;
using ASJ.Models;
using ASJ.Models.Form;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace ASJ.ClassObjects
{
        
    public class AsjCognitive
    {
        private int OrganizationId { get; set; }
        private int InstrumentId { get; set; }
        private int Year { get; set; }
        private IHostingEnvironment _hostingEnvironment { get; set; }
        private string FileName { get; set; }
        private byte[] pdfData { get; set; }

        private ASJDbContext dataContext;

        public string downFileName { get; set; }

        public AsjCognitive(int instrument, int organizationId, int year, ASJDbContext dbContext, IHostingEnvironment HostingEnvironment, string filename)
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
            var lstCog = dataContext.PDFCognitives
                    .Where(o => o.organizationid == OrganizationId)
                    .ToList();

            GetPDFData(lstCog);

            return pdfData;

        }

        private void GetPDFData(List<Models.PDF.PDFCognitive> lstCog)
        { 

            MemoryStream m = new MemoryStream();
            PdfReader reader = new PdfReader(FileName);
            PdfStamper stamper = new PdfStamper(reader, m);
            AcroFields fields = stamper.AcroFields;


            if (lstCog.Count() > 1)
            //throw error.  Should only have 1 row here
            {
                //Throw error
            }
            else
            {
                foreach (var item in lstCog)
                {
                    //Q1
                    fields.SetField("confpop", item.confpop);
                    fields.SetField("confpop_estimate", PDFContainer.convertEstimate(item.confpop_estimate));

                    //Q2
                    fields.SetField("conf17m", item.conf17m);
                    fields.SetField("conf17m_estimate", PDFContainer.convertEstimate(item.conf17m_estimate));
                    fields.SetField("conf17f", item.conf17f);
                    fields.SetField("conf17f_estimate", PDFContainer.convertEstimate(item.conf17f_estimate));
                    fields.SetField("conf17tot", item.conf17tot);
                    fields.SetField("conf17tot_estimate", PDFContainer.convertEstimate(item.conf17tot_estimate));

                    fields.SetField("conf1824m", item.conf1824m);
                    fields.SetField("conf1824m_estimate", PDFContainer.convertEstimate(item.conf1824m_estimate));
                    fields.SetField("conf1824f", item.conf1824f);
                    fields.SetField("conf1824f_estimate", PDFContainer.convertEstimate(item.conf1824f_estimate));
                    fields.SetField("conf1824tot", item.conf1824tot);
                    fields.SetField("conf1824tot_estimate", PDFContainer.convertEstimate(item.conf1824tot_estimate));

                    fields.SetField("conf2534m", item.conf2534m);
                    fields.SetField("conf2534m_estimate", PDFContainer.convertEstimate(item.conf2534m_estimate));
                    fields.SetField("conf2534f", item.conf2534f);
                    fields.SetField("conf2534f_estimate", PDFContainer.convertEstimate(item.conf2534f_estimate));
                    fields.SetField("conf2534tot", item.conf2534tot);
                    fields.SetField("conf2534tot_estimate", PDFContainer.convertEstimate(item.conf2534tot_estimate));

                    fields.SetField("conf3544m", item.conf3544m);
                    fields.SetField("conf3544m_estimate", PDFContainer.convertEstimate(item.conf3544m_estimate));
                    fields.SetField("conf3544f", item.conf3544f);
                    fields.SetField("conf3544f_estimate", PDFContainer.convertEstimate(item.conf3544f_estimate));
                    fields.SetField("conf3544tot", item.conf3544tot);
                    fields.SetField("conf3544tot_estimate", PDFContainer.convertEstimate(item.conf3544tot_estimate));

                    fields.SetField("conf4554m", item.conf4554m);
                    fields.SetField("conf4554m_estimate", PDFContainer.convertEstimate(item.conf4554m_estimate));
                    fields.SetField("conf4554f", item.conf4554f);
                    fields.SetField("conf4554f_estimate", PDFContainer.convertEstimate(item.conf4554f_estimate));
                    fields.SetField("conf4554tot", item.conf4554tot);
                    fields.SetField("conf4554tot_estimate", PDFContainer.convertEstimate(item.conf4554tot_estimate));

                    fields.SetField("conf54m", item.conf54m);
                    fields.SetField("conf54m_estimate", PDFContainer.convertEstimate(item.conf54m_estimate));
                    fields.SetField("conf54f", item.conf54f);
                    fields.SetField("conf54f_estimate", PDFContainer.convertEstimate(item.conf54f_estimate));
                    fields.SetField("conf54tot", item.conf54tot);
                    fields.SetField("conf54tot_estimate", PDFContainer.convertEstimate(item.conf54tot_estimate));

                    fields.SetField("totconftotm", item.totconftotm);
                    fields.SetField("totconftotm_estimate", PDFContainer.convertEstimate(item.totconftotm_estimate));
                    fields.SetField("totconftotf", item.totconftotf);
                    fields.SetField("totconftotf_estimate", PDFContainer.convertEstimate(item.totconftotf_estimate));
                    fields.SetField("totconftotgrand", item.totconftotgrand);
                    fields.SetField("totconftotgrand_estimate", PDFContainer.convertEstimate(item.totconftotgrand_estimate));

                    //Q3
                    fields.SetField("uscitcount", item.uscitcount);
                    fields.SetField("uscitcount_estimate", PDFContainer.convertEstimate(item.uscitcount_estimate));
                    fields.SetField("nonuscitcount", item.nonuscitcount);
                    fields.SetField("nonuscitcount_estimate", PDFContainer.convertEstimate(item.nonuscitcount_estimate));
                    fields.SetField("nonuscitconv", item.nonuscitconv);
                    fields.SetField("nonuscitconv_estimate", PDFContainer.convertEstimate(item.nonuscitconv_estimate));
                    fields.SetField("nonuscitunconv", item.nonuscitunconv);
                    fields.SetField("nonuscitunconv_estimate", PDFContainer.convertEstimate(item.nonuscitunconv_estimate));
                    fields.SetField("nonuscittotal", item.nonuscittotal);
                    fields.SetField("nonuscittotal_estimate", PDFContainer.convertEstimate(item.nonuscittotal_estimate));

                    //Q4
                    fields.SetField("usborn", item.usborn);
                    fields.SetField("usborn_estimate", PDFContainer.convertEstimate(item.usborn_estimate));
                    fields.SetField("foreignborn", item.foreignborn);
                    fields.SetField("foreignborn_estimate", PDFContainer.convertEstimate(item.foreignborn_estimate));
                    fields.SetField("foreignbornconv", item.foreignbornconv);
                    fields.SetField("foreignbornconv_estimate", PDFContainer.convertEstimate(item.foreignbornconv_estimate));
                    fields.SetField("foreignbornunconv", item.foreignbornunconv);
                    fields.SetField("foreignbornunconv_estimate", PDFContainer.convertEstimate(item.foreignbornunconv_estimate));
                    fields.SetField("foreignborntotal", item.foreignborntotal);
                    fields.SetField("foreignborntotal_estimate", PDFContainer.convertEstimate(item.foreignborntotal_estimate));

                    //Q5
                    fields.SetField("confbench", item.confbench);
                    fields.SetField("confbench_estimate", PDFContainer.convertEstimate(item.confbench_estimate));
                    fields.SetField("confpretrial", item.confpretrial);
                    fields.SetField("confpretrial_estimate", PDFContainer.convertEstimate(item.confpretrial_estimate));
                    fields.SetField("confprobation", item.confprobation);
                    fields.SetField("confprobation_estimate", PDFContainer.convertEstimate(item.confprobation_estimate));
                    fields.SetField("confparole", item.confparole);
                    fields.SetField("confparole_estimate", PDFContainer.convertEstimate(item.confparole_estimate));
                    fields.SetField("confconditional", item.confconditional);
                    fields.SetField("confconditional_estimate", PDFContainer.convertEstimate(item.confconditional_estimate));

                    //Q6
                    fields.SetField("confviol", item.confviol);
                    fields.SetField("confviol_estimate", PDFContainer.convertEstimate(item.confviol_estimate));
                    fields.SetField("confprop", item.confprop);
                    fields.SetField("confprop_estimate", PDFContainer.convertEstimate(item.confprop_estimate));
                    fields.SetField("confdrug", item.confdrug);
                    fields.SetField("confdrug_estimate", PDFContainer.convertEstimate(item.confdrug_estimate));
                    fields.SetField("confdriv", item.confdriv);
                    fields.SetField("confdriv_estimate", PDFContainer.convertEstimate(item.confdriv_estimate));
                    fields.SetField("confweap", item.confweap);
                    fields.SetField("confweap_estimate", PDFContainer.convertEstimate(item.confweap_estimate));
                    fields.SetField("confother", item.confother);
                    fields.SetField("confother_estimate", PDFContainer.convertEstimate(item.confother_estimate));
                    fields.SetField("confnotknown", item.confnotknown);
                    fields.SetField("confnotknown_estimate", PDFContainer.convertEstimate(item.confnotknown_estimate));
                    fields.SetField("totoff", item.totoff);
                    fields.SetField("totoff_estimate", PDFContainer.convertEstimate(item.totoff_estimate));

                    //Q7
                    fields.SetField("opiurine", PDFContainer.convertYesNo(item.opiurine));
                    fields.SetField("opiscreen", PDFContainer.convertYesNo(item.opiscreen));
                    fields.SetField("opioverdose", PDFContainer.convertYesNo(item.opioverdose));
                    fields.SetField("opibehav", PDFContainer.convertYesNo(item.opibehav));
                    fields.SetField("opimedprescrip", PDFContainer.convertYesNo(item.opimedprescription));
                    fields.SetField("opimeddisorder", PDFContainer.convertYesNo(item.opemeddisorder));
                    fields.SetField("opimedwithdrawal", PDFContainer.convertYesNo(item.opimedwithdrawal));
                    fields.SetField("opichronic", PDFContainer.convertYesNo(item.opichronic));
                    fields.SetField("opiacute", PDFContainer.convertYesNo(item.opiacute));
                    fields.SetField("opireversal", PDFContainer.convertYesNo(item.opireversal));
                    fields.SetField("opilink", PDFContainer.convertYesNo(item.opilink));


                    //Q8
                    fields.SetField("newadmis", item.newadmis);
                    fields.SetField("newadmis_estimate", PDFContainer.convertEstimate(item.newadmis_estimate));
                    fields.SetField("newadmisscreen", item.newadmisscreen);
                    fields.SetField("newadmisscreen_estimate", PDFContainer.convertEstimate(item.newadmisscreen_estimate));
                    fields.SetField("newadmisscreenpos", item.newadmisscreenpos);
                    fields.SetField("newadmisscreenpos_estimate", PDFContainer.convertEstimate(item.newadmisscreenpos_estimate));
                    fields.SetField("newadmisscreenposunique", item.newadmisscreenposunique);
                    fields.SetField("newadmisscreenposunique_estimate", PDFContainer.convertEstimate(item.newadmisscreenposunique_estimate));
                    fields.SetField("newadmistreat", item.newadmistreat);
                    fields.SetField("newadmistreat_estimate", PDFContainer.convertEstimate(item.newadmistreat_estimate));
                    fields.SetField("newadmistreatunique", item.newadmistreatunique);
                    fields.SetField("newadmistreatunique_estimate", PDFContainer.convertEstimate(item.newadmistreatunique_estimate));
                    fields.SetField("confmedtreatment", item.confmedtreatment);
                    fields.SetField("confmedtreatment_estimate", PDFContainer.convertEstimate(item.confmedtreatment_estimate));




                    //Done!
                }
                stamper.FormFlattening = true;
                stamper.Close();

               pdfData = m.ToArray();
                                
            }

            }
        }
    }

