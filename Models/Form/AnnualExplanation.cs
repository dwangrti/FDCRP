using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASJ.Models.Form
{
    /// <summary>
    /// this is a proxy table for view asj_annual_2018_explanations
    /// orgnaziayionID actually links to Organization table 
    /// it's made identity ey here so the table can be created =by the EF framework migration, we are not using the table at all.
    /// </summary>
    public class AnnualExplanation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
       public int OrganizationID { get; set; }
       public string explanation_p1 { get; set; }
       public string explanation_p2 { get; set; }
       public string explanation_p3 { get; set; }
       public string explanation_p4 { get; set; }
       public string explanation_p5 { get; set; }
       public string explanation_p6 { get; set; }
       public string explanation_p7 { get; set; }
       public string explanation_p8 { get; set; }
       public string explanation_p9 { get; set; }
       public string explanation_p10 { get; set; }
       public string explanation_p11 { get; set; }
       public string explanation_p12 { get; set; }
       public string explanation_p13 { get; set; }
       public string explanation_p14 { get; set; }
       public string explanation_p15 { get; set; }
       public string explanation_p16 { get; set; }
       public string iDQFU_conpop_explanation { get; set; }
       public string iDQFU_nconpop_explanation { get; set; }
       public string iDQFU_adp_explanation { get; set; }
       public string iDQFU_admis_explanation { get; set; }
       public string iDQFU_release_explanation { get; set; }
       public string iDQFU_rated_explanation { get; set; }
    }
}
