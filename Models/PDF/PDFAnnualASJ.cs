using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASJ.Models.PDF
{
    [Table("asj_annual_2018")]
    public class PDFAnnualASJ
    {
        [Key]
        
       
       public int OrganizationId { get; set; }
       public string confpop { get; set; }
       public string confpop_estimate { get; set; }
       public string nconfpop { get; set; }
       public string nconfpop_estimate { get; set; }
       public string totpop { get; set; }
       public string totpop_estimate { get; set; }
       public string week { get; set; }
       public string weekn { get; set; }
       public string weekn_estimate { get; set; }
       public string noncitz { get; set; }
       public string noncitz_estimate { get; set; }
       public string adultm { get; set; }
       public string adultm_estimate { get; set; }
       public string adultf { get; set; }
       public string adultf_estimate { get; set; }
       public string juvm { get; set; }
       public string juvm_estimate { get; set; }
       public string juvf { get; set; }
       public string juvf_estimate { get; set; }
       public string totgender { get; set; }
       public string totgender_estimate { get; set; }
       public string adltjuv { get; set; }
       public string adltjuv_estimate { get; set; }
       public string conv { get; set; }
       public string conv_estimate { get; set; }
       public string unconv { get; set; }
       public string unconv_estimate { get; set; }
       public string totconvstatus { get; set; }
       public string totconvstatus_estimate { get; set; }
       public string felony { get; set; }
       public string felony_estimate { get; set; }
       public string misd { get; set; }
       public string misd_estimate { get; set; }
       public string otheroff { get; set; }
       public string otheroff_estimate { get; set; }
       public string otheroffspec { get; set; }
       public string totoff { get; set; }
       public string totoff_estimate { get; set; }
       public string white { get; set; }
       public string white_estimate { get; set; }
       public string black { get; set; }
       public string black_estimate { get; set; }
       public string hisp { get; set; }
       public string hisp_estimate { get; set; }
       public string AIAN { get; set; }
       public string AIAN_estimate { get; set; }
       public string asian { get; set; }
       public string asian_estimate { get; set; }
       public string nhopi { get; set; }
       public string nhopi_estimate { get; set; }
       public string tworace { get; set; }
       public string tworace_estimate { get; set; }
       public string otherrace { get; set; }
       public string otherrace_estimate { get; set; }
       public string otherracespec { get; set; }
       public string racedk { get; set; }
       public string racedk_estimate { get; set; }
       public string racetotal { get; set; }
       public string racetotal_estimate { get; set; }
       public string STUBholdsquex { get; set; }
       public string marshals { get; set; }
       public string marshals_estimate { get; set; }
       public string bop { get; set; }
       public string bop_estimate { get; set; }
       public string ice { get; set; }
       public string ice_estimate { get; set; }
       public string bia { get; set; }
       public string bia_estimate { get; set; }
       public string otherfed { get; set; }
       public string otherfed_estimate { get; set; }
       public string otherfedspec { get; set; }
       public string instatepris { get; set; }
       public string instatepris_estimate { get; set; }
       public string outstatepris { get; set; }
       public string outstatepris_estimate { get; set; }
       public string tribal { get; set; }
       public string tribal_estimate { get; set; }
       public string instatejail { get; set; }
       public string instatejail_estimate { get; set; }
       public string outstatejail { get; set; }
       public string outstatejail_estimate { get; set; }
       public string otherholdtot { get; set; }
       public string otherholdtot_estimate { get; set; }
       public string peakdate { get; set; }
       public string peakpop { get; set; }
       public string peakpop_estimate { get; set; }
       public string adpmale { get; set; }
       public string adpmale_estimate { get; set; }
       public string adpfemale { get; set; }
       public string adpfemale_estimate { get; set; }
       public string adp { get; set; }
       public string adp_estimate { get; set; }
       public string rated { get; set; }
       public string rated_estimate { get; set; }
       public string admismale { get; set; }
       public string admismale_estimate { get; set; }
       public string admisfemale { get; set; }
       public string admisfemale_estimate { get; set; }
       public string admis { get; set; }
       public string admis_estimate { get; set; }
       public string releasemale { get; set; }
       public string releasemale_estimate { get; set; }
       public string releasefemale { get; set; }
       public string releasefemale_estimate { get; set; }
       public string release { get; set; }
       public string release_estimate { get; set; }
       public string emonitor { get; set; }
       public string emonitor_estimate { get; set; }
       public string homedetn { get; set; }
       public string homedetn_estimate { get; set; }
       public string commsrv { get; set; }
       public string commsrv_estimate { get; set; }
       public string dayreport { get; set; }
       public string dayreport_estimate { get; set; }
       public string pretrial { get; set; }
       public string pretrial_estimate { get; set; }
       public string altwork { get; set; }
       public string altwork_estimate { get; set; }
       public string treatment { get; set; }
       public string treatment_estimate { get; set; }
       public string otrnonconf { get; set; }
       public string otrnonconf_estimate { get; set; }
       public string otrnonconfs { get; set; }
       public string nonconfd { get; set; }
       public string nonconfd_estimate { get; set; }
       public string corrstaff { get; set; }
       public string corrstaff_estimate { get; set; }
       public string corrstaffmale { get; set; }
       public string corrstaffmale_estimate { get; set; }
       public string corrstafffemale { get; set; }
       public string corrstafffemale_estimate { get; set; }
       public string otherstaff { get; set; }
       public string otherstaff_estimate { get; set; }
       public string otherstaffmale { get; set; }
       public string otherstaffmale_estimate { get; set; }
       public string otherstafffemale { get; set; }
       public string otherstafffemale_estimate { get; set; }
       public string totalstaff { get; set; }
       public string totalstaff_estimate { get; set; }
       public string form_status { get; set; }
       public string Name { get; set; }
       public string Title { get; set; }
       public string Address { get; set; }
       public string City { get; set; }
       public string State { get; set; }
       public string Zip { get; set; }
       public string Phone { get; set; }
       public string Fax { get; set; }
       public string email { get; set; }


    }
}
