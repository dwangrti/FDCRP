using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using ASJ.Models;
using ASJ.Models.Form;
using ASJ.Services;


namespace ASJ.Models.PDF
{
    [Table("pivot_responses_cogtest")]
    public class PDFCognitive
    {

        [Key]
        public int organizationid { get; set; }
        public string confpop { get; set; }
        public string confpop_estimate { get; set; }
        public string introconfdemographics { get; set; }
        public string conf17m { get; set; }
        public string conf17m_estimate { get; set; }
        public string conf17f { get; set; }
        public string conf17f_estimate { get; set; }
        public string conf17tot { get; set; }
        public string conf17tot_estimate { get; set; }
        public string conf1824m { get; set; }
        public string conf1824m_estimate { get; set; }
        public string conf1824f { get; set; }
        public string conf1824f_estimate { get; set; }
        public string conf1824tot { get; set; }
        public string conf1824tot_estimate { get; set; }
        public string conf2534m { get; set; }
        public string conf2534m_estimate { get; set; }
        public string conf2534f { get; set; }
        public string conf2534f_estimate { get; set; }
        public string conf2534tot { get; set; }
        public string conf2534tot_estimate { get; set; }
        public string conf3544f { get; set; }
        public string conf3544f_estimate { get; set; }
        public string conf3544m { get; set; }
        public string conf3544m_estimate { get; set; }
        public string conf3544tot { get; set; }
        public string conf3544tot_estimate { get; set; }
        public string conf4554m { get; set; }
        public string conf4554m_estimate { get; set; }
        public string conf4554f { get; set; }
        public string conf4554f_estimate { get; set; }
        public string conf4554tot { get; set; }
        public string conf4554tot_estimate { get; set; }
        public string conf54m { get; set; }
        public string conf54m_estimate { get; set; }
        public string conf54f { get; set; }
        public string conf54f_estimate { get; set; }
        public string conf54tot { get; set; }
        public string conf54tot_estimate { get; set; }
        public string totconftotm { get; set; }
        public string totconftotm_estimate { get; set; }
        public string totconftotf { get; set; }
        public string totconftotf_estimate { get; set; }
        public string totconftotgrand { get; set; }
        public string totconftotgrand_estimate { get; set; }
        public string uscitcount { get; set; }
        public string uscitcount_estimate { get; set; }
        public string nonuscitcount { get; set; }
        public string nonuscitcount_estimate { get; set; }
        public string nonuscitzero { get; set; }
        public string nonuscitconv { get; set; }
        public string nonuscitconv_estimate { get; set; }
        public string nonuscitunconv { get; set; }
        public string nonuscitunconv_estimate { get; set; }
        public string nonuscittotal { get; set; }
        public string nonuscittotal_estimate { get; set; }
        public string usborn { get; set; }
        public string usborn_estimate { get; set; }
        public string foreignborn { get; set; }
        public string foreignborn_estimate { get; set; }
        public string foreignbornzero { get; set; }
        public string foreignbornconv { get; set; }
        public string foreignbornconv_estimate { get; set; }
        public string foreignbornunconv { get; set; }
        public string foreignbornunconv_estimate { get; set; }
        public string foreignborntotal { get; set; }
        public string foreignborntotal_estimate { get; set; }
        public string confbench { get; set; }
        public string confbench_estimate { get; set; }
        public string confpretrial { get; set; }
        public string confpretrial_estimate { get; set; }
        public string confprobation { get; set; }
        public string confprobation_estimate { get; set; }
        public string confparole { get; set; }
        public string confparole_estimate { get; set; }
        public string confconditional { get; set; }
        public string confconditional_estimate { get; set; }
        public string confviol { get; set; }
        public string confviol_estimate { get; set; }
        public string confprop { get; set; }
        public string confprop_estimate { get; set; }
        public string confdrug { get; set; }
        public string confdrug_estimate { get; set; }
        public string confdriv { get; set; }
        public string confdriv_estimate { get; set; }
        public string confweap { get; set; }
        public string confweap_estimate { get; set; }
        public string confother { get; set; }
        public string confother_estimate { get; set; }
        public string confnotknown { get; set; }
        public string confnotknown_estimate { get; set; }
        public string totoff { get; set; }
        public string totoff_estimate { get; set; }
        public string sectioniitext { get; set; }
        public string opiurine { get; set; }
        public string opiscreen { get; set; }
        public string opioverdose { get; set; }
        public string opibehav { get; set; }
        public string opimedprescription { get; set; }
        public string opemeddisorder { get; set; }
        public string opimedwithdrawal { get; set; }
        public string opichronic { get; set; }
        public string opiacute { get; set; }
        public string opireversal { get; set; }
        public string opilink { get; set; }
        public string newadmis { get; set; }
        public string newadmis_estimate { get; set; }
        public string newadmisscreen { get; set; }
        public string newadmisscreen_estimate { get; set; }
        public string newadmisscreenpos { get; set; }
        public string newadmisscreenpos_estimate { get; set; }
        public string newadmisscreenposunique { get; set; }
        public string newadmisscreenposunique_estimate { get; set; }
        public string newadmistreat { get; set; }
        public string newadmistreat_estimate { get; set; }
        public string newadmistreatunique { get; set; }
        public string newadmistreatunique_estimate { get; set; }
        public string confmedtreatment { get; set; }
        public string confmedtreatment_estimate { get; set; }
    }
}
