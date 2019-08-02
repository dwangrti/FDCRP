using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ASJ.ViewModels.Form
{
    public class DataSupplierViewModel
    {
        public int DataSupplierId { get; set; }
        public Models.Organization Organization { get; set; }
        //public int Year { get; set; }
        public Models.Form.Instrument Instrument { get; set; }

        public string Name { get; set; }
        public string Title { get; set; }

        [Display(Name ="Official Address")]
        public string Address { get; set; }
        public string City { get; set; }

        public string State { get; set; }

        [DataType(DataType.PostalCode)]
        public string Zip { get; set; }
        
        [Display(Name ="Telephone")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string Fax { get; set; }

        [Display(Name="Email")]
        [DataType(DataType.EmailAddress)]
        public string email { get; set; }

        [Display(Name = "Facility Name")]
        public string FacilityName { get; set; }
      
    }
}
