using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASJ.Models.Form
{
    public class InstrumentOrganizationType
    {
        public int Id { get; set; }
        public int InstrumentYear { get; set; }
        public int OrganizationTypeId { get; set; }
        public int InstrumentId { get; set; }
        public LookupFrameMembership FrameMembership { get; set; }
        
    }


}
