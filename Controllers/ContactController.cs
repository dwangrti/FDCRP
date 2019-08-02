using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ASJ.Utils;
using ASJ.Models;
using ASJ.ViewModels.Contact;
using ASJ.Services;
using static ASJ.Startup;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ASJ.Controllers
{
    [Authorize]
    public class ContactController : BaseController
    { 
        private ASJDbContext dataContext;
        private IEmailSender emailSender;
        public int referenceYear;
        private readonly ApplicationVariables _appVariables;

        public ContactController(ASJDbContext dbContext, IEmailSender emailSender, IOptions<ApplicationVariables> appVariables)
        {
            this.dataContext = dbContext;
            this.emailSender = emailSender;
            _appVariables = appVariables.Value;
            this.referenceYear = Int32.Parse(_appVariables.ReferenceYear);
        }

        // GET: /contact/
        
        [HttpGet]
        public IActionResult Contacts(int organizationId, int year)
        {
            List<ContactsViewModel> models = (from n in dataContext.OrganizationContacts
                                              where (n.Organization.OrganizationId == organizationId && n.Organization.Year == year)
                                              select new ContactsViewModel()
                                              {
                                                  OrganizationContactId = n.OrganizationContactId,
                                                  Salutation = n.Salutation,
                                                  FirstName = n.Firstname,
                                                  LastName = n.LastName,
                                                  Title = n.Title,
                                                  ContactTypeId = n.ContactTypeId,
                                                  Address1 = n.Address1,
                                                  Address2 = n.Address2,
                                                  City = n.City,
                                                  State = n.State,
                                                  Zip = n.Zip,
                                                  Phone = n.Phone,
                                                  PhoneExt = n.PhoneExt,
                                                  Email = n.Email,
                                                  PrimaryContact = n.PrimaryContact,
                                                  AgencyHead = n.AgencyHead,
                                                  BackupContact = n.BackupContact,
                                                  CreatedBy = n.CreatedBy,
                                                  CreatedOn = n.CreatedOn,
                                                  ModifiedBy = n.ModifiedBy,
                                                  ModifiedOn = n.ModifiedOn,
                                                  Organization = n.Organization
                                              }).ToList();

            Organization model = Extensions.GetOrganization(dataContext, organizationId, year);

            ViewData["Organization"] = model;

            return View(models);
        }

        // GET: /<controller>/:id
        public IActionResult Show()
        {
            return View();
        }

        public IActionResult GetAddress(int organizationId)
        {
            var oc = dataContext.OrganizationContacts.Where(c => c.Organization.OrganizationId == organizationId && c.PrimaryContact == true).FirstOrDefault();
            if (oc == null)
            {
                return NotFound();
            }
            return Ok(Json(oc));
        }

        // GET: /<controller>/Create
        [HttpGet]
        public IActionResult CreateContact(int organizationId, int year)
        {
            List<LookupContactType> ContactTypeList = dataContext.LookupContactType.ToList();
            ContactsViewModel model = new ContactsViewModel();
            model.OrganizationId = organizationId;
            model.OrganizationYear = year;
            model.ContactTypeId = 1;

            ViewData["ContactTypeList"] = ContactTypeList;

            return View(model);
        }

        // GET: /<controller>/create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateContact(ContactsViewModel model)
        {
            int OrgId = model.OrganizationId;
            int OrgYear = model.OrganizationYear;
            model.ContactTypeId = 1;//this field is now default as select list commented out in view (needed to avoid FK constraint error)

            Organization Orgn = Extensions.GetOrganization(dataContext, OrgId, OrgYear);

            if (ModelState.IsValid)
            {
                var selectedRec = dataContext.OrganizationContacts.Where(p => p.Organization.OrganizationId == OrgId && p.Organization.Year == OrgYear).ToList();
                if (model.PrimaryContact)
                {
                    selectedRec.ForEach(a => a.PrimaryContact = false);
                    dataContext.SaveChanges();
                }
                if (model.AgencyHead)
                {
                    selectedRec.ForEach(a => a.AgencyHead = false);
                    dataContext.SaveChanges();
                }
                OrganizationContacts org = new OrganizationContacts
                {
                    Organization = Orgn,
                    LastName = model.LastName,
                    Firstname = model.FirstName,
                    Address1 = model.Address1,
                    Address2 = model.Address2,
                    City = model.City,
                    State = model.State,
                    Zip = model.Zip,
                    Title = model.Title,
                    Salutation = model.Salutation,
                    Phone = model.Phone,
                    PhoneExt = model.PhoneExt,
                    Email = model.Email,
                    AgencyHead = model.AgencyHead,
                    PrimaryContact = model.PrimaryContact,
                    BackupContact = model.BackupContact,
                    CreatedBy = @User.Identity.Name,
                    CreatedOn = DateTime.Now,
                    ModifiedBy = @User.Identity.Name,
                    ModifiedOn = DateTime.Now,
                    ContactTypeId = model.ContactTypeId
                };
                dataContext.Add(org);
                dataContext.SaveChanges();
            }

            return RedirectToAction("Contacts", "Contact", new { organizationId = model.OrganizationId, year = model.OrganizationYear });
        }

        // GET: /<controller>/Edit
        [HttpGet]
        public IActionResult EditContact(int contactId)
        {
            IList<LookupContactType> ContactTypeList = dataContext.LookupContactType.ToList();
            ContactsViewModel model = (from n in dataContext.OrganizationContacts
                                       where (n.OrganizationContactId == contactId)
                                       select new ContactsViewModel()
                                       {
                                           OrganizationContactId = n.OrganizationContactId,
                                           Salutation = n.Salutation,
                                           FirstName = n.Firstname,
                                           LastName = n.LastName,
                                           Title = n.Title,
                                           LookupContactType = n.ContactType,
                                           ContactTypeId = n.ContactTypeId,
                                           Address1 = n.Address1,
                                           Address2 = n.Address2,
                                           City = n.City,
                                           State = n.State == null ? n.State : n.State.Trim(),//state had many empty characters so removed
                                           Zip = n.Zip,
                                           Phone = n.Phone,
                                           PhoneExt = n.PhoneExt,
                                           Email = n.Email,
                                           PrimaryContact = n.PrimaryContact,
                                           AgencyHead = n.AgencyHead,
                                           BackupContact = n.BackupContact,
                                           CreatedBy = n.CreatedBy,
                                           CreatedOn = n.CreatedOn,
                                           ModifiedBy = n.ModifiedBy,
                                           ModifiedOn = n.ModifiedOn,
                                           OrganizationId = n.Organization.OrganizationId,
                                           OrganizationYear = n.Organization.Year
                                       }).FirstOrDefault();

            ViewBag.ContactTypeList = ContactTypeList;

            return View(model);
        }

        // PUT: /<controller>/update
        [HttpPost]
        public IActionResult EditContact(ContactsViewModel model)
        {
            int OrgId = model.OrganizationId;
            int OrgYear = model.OrganizationYear;
            model.ContactTypeId = 1;//this field is now default as select list commented out in view (needed to avoid FK constraint error)

            Organization Orgn = Extensions.GetOrganization(dataContext, OrgId, OrgYear);

            if (ModelState.IsValid)
            {
                var selectedRec = dataContext.OrganizationContacts.Where(p => p.Organization.OrganizationId == OrgId && p.Organization.Year == OrgYear).ToList();
                if (model.PrimaryContact)
                {
                    selectedRec.ForEach(a => a.PrimaryContact = false);
                }
                if (model.AgencyHead)
                {
                    selectedRec.ForEach(a => a.AgencyHead = false);
                }
                OrganizationContacts org = dataContext.OrganizationContacts.SingleOrDefault(p => p.OrganizationContactId == model.OrganizationContactId);

                org.LastName = model.LastName;
                org.Firstname = model.FirstName;
                org.Address1 = model.Address1;
                org.Address2 = model.Address2;
                org.City = model.City;
                org.State = model.State;
                org.Zip = model.Zip;
                org.Title = model.Title;
                org.Salutation = model.Salutation;
                org.Phone = model.Phone;
                org.PhoneExt = model.PhoneExt;
                org.Email = model.Email;
                org.AgencyHead = model.AgencyHead;
                org.PrimaryContact = model.PrimaryContact;
                org.BackupContact = model.BackupContact;
                org.ModifiedBy = @User.Identity.Name;
                org.ModifiedOn = DateTime.Now;
                org.ContactTypeId = model.ContactTypeId;
                dataContext.SaveChanges();
            }

            return RedirectToAction("Contacts", "Contact", new { organizationId = model.OrganizationId, year = model.OrganizationYear });
        }

        // GET: /<controller>/delete
        [HttpDelete]
        public IActionResult Delete(ContactsViewModel model)
        {
            OrganizationContacts contact = new OrganizationContacts() { OrganizationContactId = model.OrganizationContactId };
            dataContext.Attach(contact);
            dataContext.Remove(contact);
            dataContext.SaveChanges();

            return RedirectToAction("Contacts", "Contact", new { organizationId = model.OrganizationId, year = model.OrganizationYear });
        }
    }
}
