using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASJ.Models.Form;

namespace ASJ.ViewModels.Form
{
    public class DQFUViewModel
    {
        public DQFUvalue Values { get; set; }
        public int InsturmentId { get; set; }


        //public double RatioConpop { get; set; }
        //public double RatioMale { get; set; }
        //public double RatioFemale { get; set; }
        //public double RatioNconpop { get; set; }
        //public double RatioAdp { get; set; }
        //public double RatioAdpMale { get; set; }
        //public double RatioAdpFemale { get; set; }
        //public double RatioRated { get; set; }
        //public double RatioAdmis { get; set; }
        //public double RatioAdmisMale { get; set; }
        //public double RatioAdmisFemale { get; set; }
        //public double RatioRelease { get; set; }
        //public double RatioReleaseMale { get; set; }
        //public double RatioReleaseFemale { get; set; }


        public bool FlagConpop { get; set; }
        public bool FlagMale { get; set; }
        public bool FlagFemale { get; set; }
        public bool FlagNconpop { get; set; }
        public bool FlagAdp { get; set; }
        public bool FlagAdpMale { get; set; }
        public bool FlagAdpFemale { get; set; }
        public bool FlagRated { get; set; }
        public bool FlagAdmis { get; set; }
        public bool FlagAdmisMale { get; set; }
        public bool FlagAdmisFemale { get; set; }
        public bool FlagRelease { get; set; }
        public bool FlagReleaseMale { get; set; }
        public bool FlagReleaseFemale { get; set; }

        public string iDQFU_Conpop_Explanation { get; set; }
        public string iDQFU_Nconpop_Explanation { get; set; }
        public string iDQFU_Adp_Explanationn { get; set; }
        public string iDQFU_Rated_Explanation { get; set; }
        public string iDQFU_Admis_Explanation { get; set; }
        public string iDQFU_Release_Explanation { get; set; }
    }
}
