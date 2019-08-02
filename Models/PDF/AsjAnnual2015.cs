﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASJ.Models.PDF
{
    [Table("asj_annual_2015")]
    public partial class AsjAnnual2015
    {
        [Key]
        public int AsjAnnualId { get; set; }
        public int OrganizationId { get; set; }
        public string NumDeathsMales { get; set; }
        public string NumDeathsFemales { get; set; }
        public string Confpop { get; set; }
        public bool? ConfpopEstimate { get; set; }
        public string Confpop6e { get; set; }
        public bool? Confpop6eEstimate { get; set; }
        public string Confpop8c { get; set; }
        public bool? Confpop8cEstimate { get; set; }
        public string Confpop9d { get; set; }
        public bool? Confpop9dEstimate { get; set; }
        public string Confpop10j { get; set; }
        public bool? Confpop10jEstimate { get; set; }
        public string Confpopjune { get; set; }
        public bool? ConfpopjuneEstimate { get; set; }
        public string Nconpop { get; set; }
        public bool? NconpopEstimate { get; set; }
        public string Nconpop16i { get; set; }
        public bool? Nconpop16iEstimate { get; set; }
        public string Totpop { get; set; }
        public bool? TotpopEstimate { get; set; }
        public string Week { get; set; }
        public bool? WeekEstimate { get; set; }
        public string Weekn { get; set; }
        public bool? WeeknEstimate { get; set; }
        public string Noncitz { get; set; }
        public bool? NoncitzEstimate { get; set; }
        public string Adultm { get; set; }
        public bool? AdultmEstimate { get; set; }
        public string Adultf { get; set; }
        public bool? AdultfEstimate { get; set; }
        public string Juvm { get; set; }
        public bool? JuvmEstimate { get; set; }
        public string Juvf { get; set; }
        public bool? JuvfEstimate { get; set; }
        public string Totgender { get; set; }
        public bool? TotgenderEstimate { get; set; }
        public string Adltjuv { get; set; }
        public bool? AdltjuvEstimate { get; set; }
        public string Conv { get; set; }
        public bool? ConvEstimate { get; set; }
        public string Convunsent { get; set; }
        public bool? ConvunsentEstimate { get; set; }
        public string Convsent { get; set; }
        public bool? ConvsentEstimate { get; set; }
        public string Unconv { get; set; }
        public bool? UnconvEstimate { get; set; }
        public string Unconvtrial { get; set; }
        public bool? UnconvtrialEstimate { get; set; }
        public string Unconvhold { get; set; }
        public bool? UnconvholdEstimate { get; set; }
        public string Unconvother { get; set; }
        public bool? UnconvotherEstimate { get; set; }
        public string Totconvstatus { get; set; }
        public bool? TotconvstatusEstimate { get; set; }
        public string Felony { get; set; }
        public bool? FelonyEstimate { get; set; }
        public string Misd { get; set; }
        public bool? MisdEstimate { get; set; }
        public string Otheroffspec { get; set; }
        public string Otheroff { get; set; }
        public bool? OtheroffEstimate { get; set; }
        public string Totoff { get; set; }
        public bool? TotoffEstimate { get; set; }
        public string White { get; set; }
        public bool? WhiteEstimate { get; set; }
        public string Black { get; set; }
        public bool? BlackEstimate { get; set; }
        public string Hisp { get; set; }
        public bool? HispEstimate { get; set; }
        public string Aian { get; set; }
        public bool? AianEstimate { get; set; }
        public string Asian { get; set; }
        public bool? AsianEstimate { get; set; }
        public string Nhopi { get; set; }
        public bool? NhopiEstimate { get; set; }
        public string Tworace { get; set; }
        public bool? TworaceEstimate { get; set; }
        public string Otherracespec { get; set; }
        public string Otherrace { get; set; }
        public bool? OtherraceEstimate { get; set; }
        public string Racedk { get; set; }
        public bool? RacedkEstimate { get; set; }
        public string Racetotal { get; set; }
        public bool? RacetotalEstimate { get; set; }
        public string Marshals { get; set; }
        public bool? MarshalsEstimate { get; set; }
        public string Bop { get; set; }
        public bool? BopEstimate { get; set; }
        public string Ice { get; set; }
        public bool? IceEstimate { get; set; }
        public string Bia { get; set; }
        public bool? BiaEstimate { get; set; }
        public string Otherfedspec { get; set; }
        public string Otherfed { get; set; }
        public bool? OtherfedEstimate { get; set; }
        public string Instatepris { get; set; }
        public bool? InstateprisEstimate { get; set; }
        public string Outstatepris { get; set; }
        public bool? OutstateprisEstimate { get; set; }
        public string Tribal { get; set; }
        public bool? TribalEstimate { get; set; }
        public string Instatejail { get; set; }
        public bool? InstatejailEstimate { get; set; }
        public string Outstatejail { get; set; }
        public bool? OutstatejailEstimate { get; set; }
        public string Otherholdtot { get; set; }
        public bool? OtherholdtotEstimate { get; set; }
        public string Peakdate { get; set; }
        public string Peakpop { get; set; }
        public bool? PeakpopEstimate { get; set; }
        public string Adpmale { get; set; }
        public bool? AdpmaleEstimate { get; set; }
        public string Adpfemale { get; set; }
        public bool? AdpfemaleEstimate { get; set; }
        public string Adp { get; set; }
        public bool? AdpEstimate { get; set; }
        public string Rated { get; set; }
        public bool? RatedEstimate { get; set; }
        public string Admismale { get; set; }
        public bool? AdmismaleEstimate { get; set; }
        public string Admisfemale { get; set; }
        public bool? AdmisfemaleEstimate { get; set; }
        public string Admis { get; set; }
        public bool? AdmisEstimate { get; set; }
        public string Releasemale { get; set; }
        public bool? ReleasemaleEstimate { get; set; }
        public string Releasefemale { get; set; }
        public bool? ReleasefemaleEstimate { get; set; }
        public string Release { get; set; }
        public bool? ReleaseEstimate { get; set; }
        public string Emonitor { get; set; }
        public bool? EmonitorEstimate { get; set; }
        public string Homedetn { get; set; }
        public bool? HomedetnEstimate { get; set; }
        public string Commsrv { get; set; }
        public bool? CommsrvEstimate { get; set; }
        public string Dayreport { get; set; }
        public bool? DayreportEstimate { get; set; }
        public string Pretrial { get; set; }
        public bool? PretrialEstimate { get; set; }
        public string Altwork { get; set; }
        public bool? AltworkEstimate { get; set; }
        public string Treatment { get; set; }
        public bool? TreatmentEstimate { get; set; }
        public string Otrnonconfs { get; set; }
        public string Otrnonconf { get; set; }
        public bool? OtrnonconfEstimate { get; set; }
        public string Nonconfd { get; set; }
        public bool? NonconfdEstimate { get; set; }
        public string Cost { get; set; }
        public bool? CostEstimate { get; set; }
        public string Corrstaff { get; set; }
        public bool? CorrstaffEstimate { get; set; }
        public string Corrstaffmale { get; set; }
        public bool? CorrstaffmaleEstimate { get; set; }
        public string Corrstafffemale { get; set; }
        public bool? CorrstafffemaleEstimate { get; set; }
        public string Otherstaff { get; set; }
        public bool? OtherstaffEstimate { get; set; }
        public string Otherstaffmale { get; set; }
        public bool? OtherstaffmaleEstimate { get; set; }
        public string Otherstafffemale { get; set; }
        public bool? OtherstafffemaleEstimate { get; set; }
        public string Totalstaff { get; set; }
        public bool? TotalstaffEstimate { get; set; }
        public string Corrwhite { get; set; }
        public bool? CorrwhiteEstimate { get; set; }
        public string Corrblack { get; set; }
        public bool? CorrblackEstimate { get; set; }
        public string Corrhisp { get; set; }
        public bool? CorrhispEstimate { get; set; }
        public string CorrAian { get; set; }
        public bool? CorrAianEstimate { get; set; }
        public string Corrasian { get; set; }
        public bool? CorrasianEstimate { get; set; }
        public string Corrnhopi { get; set; }
        public bool? CorrnhopiEstimate { get; set; }
        public string Corrtworace { get; set; }
        public bool? CorrtworaceEstimate { get; set; }
        public string Corrotherracespec { get; set; }
        public string Corrotherrace { get; set; }
        public bool? CorrotherraceEstimate { get; set; }
        public string Corrracedk { get; set; }
        public bool? CorrracedkEstimate { get; set; }
        public string Corrracetotal { get; set; }
        public bool? CorrracetotalEstimate { get; set; }
        public DateTime? Createdate { get; set; }
        public DateTime? Lastupdated { get; set; }
        public string FormStatus { get; set; }
        public int FormReceivedById { get; set; }
        public int? SummaryQualityCode { get; set; }
        public DateTime? SummaryQualityCodeTime { get; set; }
        public int? SummaryStatusCode { get; set; }
        public DateTime? SummaryStatusCodeTime { get; set; }
        public string SummaryStatusCodeUser { get; set; }
        public int? AsjstatusCode { get; set; }
        public int? AsjqualityCode { get; set; }
        public int? DcrpstatusCode { get; set; }
        public DateTime? Rtireceived { get; set; }
        public int? TextReview { get; set; }
        public string TextReviewNotes { get; set; }
        public int? PriorTextReview { get; set; }
        public DateTime? SubmittedDate { get; set; }
        public string CreatedUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedUserId { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
