using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASJ.Models
{
    public class DataSupplier
    {

        public int DataSupplierId { get; set; }
        public Organization Organization { get; set; }
        //public int Year { get; set; }
        public Form.Instrument Instrument { get; set; }

        public string Name { get; set; }
        public string Title { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string email { get; set; }
        public string FacilityName { get; set; }
           

    }
}
