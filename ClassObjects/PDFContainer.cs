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
    public class PDFContainer
    {
        private int OrganizationId { get; set; }
        private int InstrumentId { get; set; }
        private int Year { get; set; }
        private IHostingEnvironment _hostingEnvironment {get;set;}

        private ASJDbContext dataContext;
        private ASJLegacyContext legacyDataContext;

        public string downFileName { get; set; }

      
        public PDFContainer(int instrument, int organizationId, int year, ASJDbContext dbContext, ASJLegacyContext legContext, IHostingEnvironment HostingEnvironment)
        {
            InstrumentId = instrument;
            Year = year;
            dataContext = dbContext;
            legacyDataContext = legContext;
            OrganizationId = organizationId;
            _hostingEnvironment = HostingEnvironment;

        }
       
        public byte[] GetPDFData()
        {
            //Fill the values on the form. 
            int agencyType = dataContext.Organizations
               .Where(o => o.OrganizationId == OrganizationId && o.Year == Year)
               .Select(a => a.OrganizationType.OrganizationTypeId).SingleOrDefault();

            string fileName = getFileNameByYearAndOrgType(OrganizationId, Year);
            
            //Hook for cognitive only 
            if (Year == 2018 && agencyType == 4)
            {
                AsjCognitive pdfCog = new AsjCognitive (InstrumentId, OrganizationId, Year, dataContext, _hostingEnvironment, fileName);
                return pdfCog.returnPDFdata();
            }
            else if (Year == 2018)
            {
                PDF2018Asj pdf2018 = new PDF2018Asj(InstrumentId, OrganizationId, Year, dataContext, _hostingEnvironment, fileName);
                return pdf2018.returnPDFdata();
            }
            //Legacy PDF code and objects.  This shouldn't change in the future. 
            else if (Year <= 2017)
            {
                PDFLegacyAsj pdfLegacy = new PDFLegacyAsj(InstrumentId, OrganizationId, Year, legacyDataContext, dataContext, _hostingEnvironment, fileName);
                return pdfLegacy.ReturnPDFdata();

            }
            else
            {
                return null;
            }
      
        }
      
            //Get the correct form for this agency type and year
       
        public static string convertEstimate(string est)
        {
            if (est != null)
            {
                switch (est.ToUpper())
                {
                    case "TRUE":
                        return "Yes";
                    case "FALSE":
                        return "No";
                    default:
                        return "No";
                }
            }
            else
                return "No";
        }
        public static string convertYesNo(string est)
        {
            if (est != null)
            {
                switch (est.ToUpper())
                {
                    case "1":
                        return "y";
                    case "2":
                        return "n";
                    default:
                        return "";
                }
            }
            else
                return "No";
        }
        private string getFileNameByYearAndOrgType(int organizationId, int year)
        {

            int agencyType = dataContext.Organizations
                .Where(o => o.OrganizationId == organizationId && o.Year == year)
                .Select(a => a.OrganizationType.OrganizationTypeId).SingleOrDefault();

            string filename = string.Format("{0}_{1}_fillable", getFileNameRoot(agencyType, year), year);
            string filepath = string.Format("{0}\\pdfs\\{1}.pdf", _hostingEnvironment.WebRootPath, filename);

           

            return filepath;
        }

        private static string getFileNameRoot(int agencyType, int year)
        {
            string formRoot = "";

            switch (agencyType)
            {
                case 2:
                    if(year <= 2016)
                        formRoot = "CJ9A5";
                    else
                        formRoot = "CJ5";
                    break;
                case 3:
                    if (year <= 2016)
                        formRoot = "CJ10A5";
                    else 
                        formRoot = "CJ5A";
                    break;
                case 4:
                    formRoot = "COG";
                    break;
                default:
                    formRoot = "";
                    break;

            }
            return formRoot;
        }
        public static string formatPhone(string p)
        {
            try
            {
                p = Regex.Replace(p, @"(\d{3})(\d{3})(\d{4})", "$1-$2-$3");
                return p;
            }
            catch
            {
                return p;
            }
        }





    }
}
