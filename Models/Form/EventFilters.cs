using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace ASJ.Models.Form
{
    public class EventFilters
    {
        public string[] year_options { get; set; }
        public string[] filter_year { get; set; }
        public string filter_system { get; set; }
        public int organizationId { get; set; }
        public int year { get; set; }
    }
}
