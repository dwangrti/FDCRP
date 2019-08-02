using ASJ.Models;
using ASJ.Services;
using Microsoft.AspNetCore.Hosting.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ASJ.Utils
{
    public class DQFUEmails
    {
        //pass in agencystatus code, to address, orgid, year
        //get the email for agencystatus code from table
        //parse the placeholders
        //construct email
        //send email
        public static void SendDQFUEmail(ASJDbContext dataContext, int organizationId, int year, string sendTo, IEmailSender emailSender, AppIdentityUser user)
        {
            //get agency status code
            Organization thisOrg = dataContext.Organizations.Where(x => x.OrganizationId == organizationId && x.Year == year).FirstOrDefault();
            OrganizationFollowup fol = dataContext.OrganizationFollowups.FirstOrDefault(x => x.Organization.OrganizationId == organizationId && x.Organization.Year == year);

            int agencystatuscode = 0;
            if(fol != null)
                agencystatuscode = dataContext.OrganizationFollowups.FirstOrDefault(x => x.Organization.OrganizationId == organizationId && x.Organization.Year == year).AgencyStatusCodeId;

            //get the email associated with this agency
            DQFUEmail email = dataContext.DQFUEmails.FirstOrDefault(x => x.StatusCode == agencystatuscode && x.Year == year);

            if (email != null)
            {
                //get the email content from the file
                var fileContents = System.IO.File.ReadAllText("wwwroot/emailtemplates/" + email.ContentFileName);

                //----------------------------------------------
                //parse placeholders in subject
                string subject = email.Subject;
                //organizationid
                subject = subject.Replace("[[AgencyID]]", organizationId.ToString());


                //-----------------------------------------------
                //parse placeholders in body

                //use the data supplier info - if thats not available, then use the POC info
                DataSupplier ds = dataContext.DataSuppliers.Where(x => x.Organization.OrganizationId == organizationId && x.Organization.Year == year).FirstOrDefault();
                if (ds != null && ds.Name != null && ds.Name != "")
                {
                    fileContents = fileContents.Replace("[[salutation]]", ds.Title);
                    fileContents = fileContents.Replace("[[POClastName]]", ds.Name);
                }
                else
                {
                    OrganizationContacts primaryContact = dataContext.OrganizationContacts.Where(x => x.Organization.OrganizationId == organizationId && x.Organization.Year == year && x.PrimaryContact).FirstOrDefault();

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
                //errors
                if (fileContents.Contains("[[errorList]]"))
                {
                    List<OrganizationQCDetails> errs = dataContext.OrganizationQCDetails.Where(x => x.Organization.OrganizationId == organizationId && x.Organization.Year == year).ToList();
                    string errors = "<ul>";
                    if (errs.Count == 0)
                        errors = errors + "<li>No errors</li></ul>";
                    else
                    {
                        foreach (OrganizationQCDetails err in errs)
                        {
                            errors = errors + "<li>" + err.QCDetails.ToString() + "</li>";
                        }
                        errors = errors + "</ul>";
                    }
                    fileContents = fileContents.Replace("[[errorList]]", errors);
                }
                //username
                fileContents = fileContents.Replace( "[[UserName]]", thisOrg.UserName);

                //password
                fileContents = fileContents.Replace( "[[Password]]", thisOrg.PasswordSecure);

                //AL Extension
                fileContents = fileContents.Replace( "[[alExtension]]", user.PhoneNumber);

                //AL Name
                fileContents = fileContents.Replace( "[[alName]]", user.DisplayName);

                //------------------------------------------------
                //Construct email and send 
                emailSender.SendEmail(sendTo, subject, fileContents, "doj-dcra@rti.org");

                //-------------------------------------------------
            }
        }
     }
}
