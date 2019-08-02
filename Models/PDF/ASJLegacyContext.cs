using System;
using ASJ.Models.PDF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ASJ.Models
{
    public partial class ASJLegacyContext : DbContext
    {
        public virtual DbSet<AsjAnnual2015> AsjAnnual2015 { get; set; }
        public virtual DbSet<AsjAnnual2016> AsjAnnual2016 { get; set; }
        public virtual DbSet<AsjAnnual2017> AsjAnnual2017 { get; set; }
        public virtual DbSet<AsjAnnual2017Reasons> AsjAnnual2017Reasons { get; set; }

        public ASJLegacyContext(DbContextOptions<ASJLegacyContext> options)
            : base(options)
        { }

        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AsjAnnual2015>(entity =>
            {
                entity.HasKey(e => e.AsjAnnualId);

                entity.ToTable("asj_annual_2015");

                entity.Property(e => e.AsjAnnualId).HasColumnName("asj_annual_ID");

                entity.Property(e => e.Adltjuv)
                    .HasColumnName("adltjuv")
                    .HasMaxLength(20);

                entity.Property(e => e.AdltjuvEstimate).HasColumnName("adltjuv_estimate");

                entity.Property(e => e.Admis)
                    .HasColumnName("admis")
                    .HasMaxLength(20);

                entity.Property(e => e.AdmisEstimate).HasColumnName("admis_estimate");

                entity.Property(e => e.Admisfemale)
                    .HasColumnName("admisfemale")
                    .HasMaxLength(20);

                entity.Property(e => e.AdmisfemaleEstimate).HasColumnName("admisfemale_estimate");

                entity.Property(e => e.Admismale)
                    .HasColumnName("admismale")
                    .HasMaxLength(20);

                entity.Property(e => e.AdmismaleEstimate).HasColumnName("admismale_estimate");

                entity.Property(e => e.Adp)
                    .HasColumnName("adp")
                    .HasMaxLength(20);

                entity.Property(e => e.AdpEstimate).HasColumnName("adp_estimate");

                entity.Property(e => e.Adpfemale)
                    .HasColumnName("adpfemale")
                    .HasMaxLength(20);

                entity.Property(e => e.AdpfemaleEstimate).HasColumnName("adpfemale_estimate");

                entity.Property(e => e.Adpmale)
                    .HasColumnName("adpmale")
                    .HasMaxLength(20);

                entity.Property(e => e.AdpmaleEstimate).HasColumnName("adpmale_estimate");

                entity.Property(e => e.Adultf)
                    .HasColumnName("adultf")
                    .HasMaxLength(20);

                entity.Property(e => e.AdultfEstimate).HasColumnName("adultf_estimate");

                entity.Property(e => e.Adultm)
                    .HasColumnName("adultm")
                    .HasMaxLength(20);

                entity.Property(e => e.AdultmEstimate).HasColumnName("adultm_estimate");

                entity.Property(e => e.Aian)
                    .HasColumnName("AIAN")
                    .HasMaxLength(20);

                entity.Property(e => e.AianEstimate).HasColumnName("AIAN_estimate");

                entity.Property(e => e.Altwork)
                    .HasColumnName("altwork")
                    .HasMaxLength(20);

                entity.Property(e => e.AltworkEstimate).HasColumnName("altwork_estimate");

                entity.Property(e => e.Asian)
                    .HasColumnName("asian")
                    .HasMaxLength(20);

                entity.Property(e => e.AsianEstimate).HasColumnName("asian_estimate");

                entity.Property(e => e.AsjqualityCode).HasColumnName("ASJQualityCode");

                entity.Property(e => e.AsjstatusCode).HasColumnName("ASJStatusCode");

                entity.Property(e => e.Bia)
                    .HasColumnName("bia")
                    .HasMaxLength(20);

                entity.Property(e => e.BiaEstimate).HasColumnName("bia_estimate");

                entity.Property(e => e.Black)
                    .HasColumnName("black")
                    .HasMaxLength(20);

                entity.Property(e => e.BlackEstimate).HasColumnName("black_estimate");

                entity.Property(e => e.Bop)
                    .HasColumnName("bop")
                    .HasMaxLength(20);

                entity.Property(e => e.BopEstimate).HasColumnName("bop_estimate");

                entity.Property(e => e.Commsrv)
                    .HasColumnName("commsrv")
                    .HasMaxLength(20);

                entity.Property(e => e.CommsrvEstimate).HasColumnName("commsrv_estimate");

                entity.Property(e => e.Confpop)
                    .HasColumnName("confpop")
                    .HasMaxLength(20);

                entity.Property(e => e.Confpop10j)
                    .HasColumnName("confpop_10j")
                    .HasMaxLength(20);

                entity.Property(e => e.Confpop10jEstimate).HasColumnName("confpop_10j_estimate");

                entity.Property(e => e.Confpop6e)
                    .HasColumnName("confpop_6e")
                    .HasMaxLength(20);

                entity.Property(e => e.Confpop6eEstimate).HasColumnName("confpop_6e_estimate");

                entity.Property(e => e.Confpop8c)
                    .HasColumnName("confpop_8c")
                    .HasMaxLength(20);

                entity.Property(e => e.Confpop8cEstimate).HasColumnName("confpop_8c_estimate");

                entity.Property(e => e.Confpop9d)
                    .HasColumnName("confpop_9d")
                    .HasMaxLength(20);

                entity.Property(e => e.Confpop9dEstimate).HasColumnName("confpop_9d_estimate");

                entity.Property(e => e.ConfpopEstimate).HasColumnName("confpop_estimate");

                entity.Property(e => e.Confpopjune)
                    .HasColumnName("confpopjune")
                    .HasMaxLength(20);

                entity.Property(e => e.ConfpopjuneEstimate).HasColumnName("confpopjune_estimate");

                entity.Property(e => e.Conv)
                    .HasColumnName("conv")
                    .HasMaxLength(20);

                entity.Property(e => e.ConvEstimate).HasColumnName("conv_estimate");

                entity.Property(e => e.Convsent)
                    .HasColumnName("convsent")
                    .HasMaxLength(20);

                entity.Property(e => e.ConvsentEstimate).HasColumnName("convsent_estimate");

                entity.Property(e => e.Convunsent)
                    .HasColumnName("convunsent")
                    .HasMaxLength(20);

                entity.Property(e => e.ConvunsentEstimate).HasColumnName("convunsent_estimate");

                entity.Property(e => e.CorrAian)
                    .HasColumnName("corrAIAN")
                    .HasMaxLength(20);

                entity.Property(e => e.CorrAianEstimate).HasColumnName("corrAIAN_estimate");

                entity.Property(e => e.Corrasian)
                    .HasColumnName("corrasian")
                    .HasMaxLength(20);

                entity.Property(e => e.CorrasianEstimate).HasColumnName("corrasian_estimate");

                entity.Property(e => e.Corrblack)
                    .HasColumnName("corrblack")
                    .HasMaxLength(20);

                entity.Property(e => e.CorrblackEstimate).HasColumnName("corrblack_estimate");

                entity.Property(e => e.Corrhisp)
                    .HasColumnName("corrhisp")
                    .HasMaxLength(20);

                entity.Property(e => e.CorrhispEstimate).HasColumnName("corrhisp_estimate");

                entity.Property(e => e.Corrnhopi)
                    .HasColumnName("corrnhopi")
                    .HasMaxLength(20);

                entity.Property(e => e.CorrnhopiEstimate).HasColumnName("corrnhopi_estimate");

                entity.Property(e => e.Corrotherrace)
                    .HasColumnName("corrotherrace")
                    .HasMaxLength(20);

                entity.Property(e => e.CorrotherraceEstimate).HasColumnName("corrotherrace_estimate");

                entity.Property(e => e.Corrotherracespec)
                    .HasColumnName("corrotherracespec")
                    .HasMaxLength(500);

                entity.Property(e => e.Corrracedk)
                    .HasColumnName("corrracedk")
                    .HasMaxLength(20);

                entity.Property(e => e.CorrracedkEstimate).HasColumnName("corrracedk_estimate");

                entity.Property(e => e.Corrracetotal)
                    .HasColumnName("corrracetotal")
                    .HasMaxLength(20);

                entity.Property(e => e.CorrracetotalEstimate).HasColumnName("corrracetotal_estimate");

                entity.Property(e => e.Corrstaff)
                    .HasColumnName("corrstaff")
                    .HasMaxLength(20);

                entity.Property(e => e.CorrstaffEstimate).HasColumnName("corrstaff_estimate");

                entity.Property(e => e.Corrstafffemale)
                    .HasColumnName("corrstafffemale")
                    .HasMaxLength(20);

                entity.Property(e => e.CorrstafffemaleEstimate).HasColumnName("corrstafffemale_estimate");

                entity.Property(e => e.Corrstaffmale)
                    .HasColumnName("corrstaffmale")
                    .HasMaxLength(20);

                entity.Property(e => e.CorrstaffmaleEstimate).HasColumnName("corrstaffmale_estimate");

                entity.Property(e => e.Corrtworace)
                    .HasColumnName("corrtworace")
                    .HasMaxLength(20);

                entity.Property(e => e.CorrtworaceEstimate).HasColumnName("corrtworace_estimate");

                entity.Property(e => e.Corrwhite)
                    .HasColumnName("corrwhite")
                    .HasMaxLength(20);

                entity.Property(e => e.CorrwhiteEstimate).HasColumnName("corrwhite_estimate");

                entity.Property(e => e.Cost)
                    .HasColumnName("cost")
                    .HasMaxLength(20);

                entity.Property(e => e.CostEstimate).HasColumnName("cost_estimate");

                entity.Property(e => e.CreatedDate)
                    .HasColumnName("created_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.CreatedUserId)
                    .IsRequired()
                    .HasColumnName("created_user_id")
                    .HasMaxLength(256);

                entity.Property(e => e.Createdate)
                    .HasColumnName("createdate")
                    .HasColumnType("datetime");

                entity.Property(e => e.Dayreport)
                    .HasColumnName("dayreport")
                    .HasMaxLength(20);

                entity.Property(e => e.DayreportEstimate).HasColumnName("dayreport_estimate");

                entity.Property(e => e.DcrpstatusCode).HasColumnName("DCRPStatusCode");

                entity.Property(e => e.Emonitor)
                    .HasColumnName("emonitor")
                    .HasMaxLength(20);

                entity.Property(e => e.EmonitorEstimate).HasColumnName("emonitor_estimate");

                entity.Property(e => e.Felony)
                    .HasColumnName("felony")
                    .HasMaxLength(20);

                entity.Property(e => e.FelonyEstimate).HasColumnName("felony_estimate");

                entity.Property(e => e.FormReceivedById).HasColumnName("FormReceivedBy_ID");

                entity.Property(e => e.FormStatus)
                    .HasColumnName("form_status")
                    .IsUnicode(false);

                entity.Property(e => e.Hisp)
                    .HasColumnName("hisp")
                    .HasMaxLength(20);

                entity.Property(e => e.HispEstimate).HasColumnName("hisp_estimate");

                entity.Property(e => e.Homedetn)
                    .HasColumnName("homedetn")
                    .HasMaxLength(20);

                entity.Property(e => e.HomedetnEstimate).HasColumnName("homedetn_estimate");

                entity.Property(e => e.Ice)
                    .HasColumnName("ice")
                    .HasMaxLength(20);

                entity.Property(e => e.IceEstimate).HasColumnName("ice_estimate");

                entity.Property(e => e.Instatejail)
                    .HasColumnName("instatejail")
                    .HasMaxLength(20);

                entity.Property(e => e.InstatejailEstimate).HasColumnName("instatejail_estimate");

                entity.Property(e => e.Instatepris)
                    .HasColumnName("instatepris")
                    .HasMaxLength(20);

                entity.Property(e => e.InstateprisEstimate).HasColumnName("instatepris_estimate");

                entity.Property(e => e.Juvf)
                    .HasColumnName("juvf")
                    .HasMaxLength(20);

                entity.Property(e => e.JuvfEstimate).HasColumnName("juvf_estimate");

                entity.Property(e => e.Juvm)
                    .HasColumnName("juvm")
                    .HasMaxLength(20);

                entity.Property(e => e.JuvmEstimate).HasColumnName("juvm_estimate");

                entity.Property(e => e.Lastupdated)
                    .HasColumnName("lastupdated")
                    .HasColumnType("datetime");

                entity.Property(e => e.Marshals)
                    .HasColumnName("marshals")
                    .HasMaxLength(20);

                entity.Property(e => e.MarshalsEstimate).HasColumnName("marshals_estimate");

                entity.Property(e => e.Misd)
                    .HasColumnName("misd")
                    .HasMaxLength(20);

                entity.Property(e => e.MisdEstimate).HasColumnName("misd_estimate");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnName("modified_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.ModifiedUserId)
                    .IsRequired()
                    .HasColumnName("modified_user_id")
                    .HasMaxLength(256);

                entity.Property(e => e.Nconpop)
                    .HasColumnName("nconpop")
                    .HasMaxLength(20);

                entity.Property(e => e.Nconpop16i)
                    .HasColumnName("nconpop_16i")
                    .HasMaxLength(20);

                entity.Property(e => e.Nconpop16iEstimate).HasColumnName("nconpop_16i_estimate");

                entity.Property(e => e.NconpopEstimate).HasColumnName("nconpop_estimate");

                entity.Property(e => e.Nhopi)
                    .HasColumnName("nhopi")
                    .HasMaxLength(20);

                entity.Property(e => e.NhopiEstimate).HasColumnName("nhopi_estimate");

                entity.Property(e => e.Noncitz)
                    .HasColumnName("noncitz")
                    .HasMaxLength(20);

                entity.Property(e => e.NoncitzEstimate).HasColumnName("noncitz_estimate");

                entity.Property(e => e.Nonconfd)
                    .HasColumnName("nonconfd")
                    .HasMaxLength(20);

                entity.Property(e => e.NonconfdEstimate).HasColumnName("nonconfd_estimate");

                entity.Property(e => e.NumDeathsFemales)
                    .HasColumnName("num_deaths_females")
                    .HasMaxLength(50);

                entity.Property(e => e.NumDeathsMales)
                    .HasColumnName("num_deaths_males")
                    .HasMaxLength(50);

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");

                entity.Property(e => e.Otherfed)
                    .HasColumnName("otherfed")
                    .HasMaxLength(20);

                entity.Property(e => e.OtherfedEstimate).HasColumnName("otherfed_estimate");

                entity.Property(e => e.Otherfedspec)
                    .HasColumnName("otherfedspec")
                    .HasMaxLength(500);

                entity.Property(e => e.Otherholdtot)
                    .HasColumnName("otherholdtot")
                    .HasMaxLength(20);

                entity.Property(e => e.OtherholdtotEstimate).HasColumnName("otherholdtot_estimate");

                entity.Property(e => e.Otheroff)
                    .HasColumnName("otheroff")
                    .HasMaxLength(20);

                entity.Property(e => e.OtheroffEstimate).HasColumnName("otheroff_estimate");

                entity.Property(e => e.Otheroffspec)
                    .HasColumnName("otheroffspec")
                    .HasMaxLength(500);

                entity.Property(e => e.Otherrace)
                    .HasColumnName("otherrace")
                    .HasMaxLength(20);

                entity.Property(e => e.OtherraceEstimate).HasColumnName("otherrace_estimate");

                entity.Property(e => e.Otherracespec)
                    .HasColumnName("otherracespec")
                    .HasMaxLength(500);

                entity.Property(e => e.Otherstaff)
                    .HasColumnName("otherstaff")
                    .HasMaxLength(20);

                entity.Property(e => e.OtherstaffEstimate).HasColumnName("otherstaff_estimate");

                entity.Property(e => e.Otherstafffemale)
                    .HasColumnName("otherstafffemale")
                    .HasMaxLength(20);

                entity.Property(e => e.OtherstafffemaleEstimate).HasColumnName("otherstafffemale_estimate");

                entity.Property(e => e.Otherstaffmale)
                    .HasColumnName("otherstaffmale")
                    .HasMaxLength(20);

                entity.Property(e => e.OtherstaffmaleEstimate).HasColumnName("otherstaffmale_estimate");

                entity.Property(e => e.Otrnonconf)
                    .HasColumnName("otrnonconf")
                    .HasMaxLength(20);

                entity.Property(e => e.OtrnonconfEstimate).HasColumnName("otrnonconf_estimate");

                entity.Property(e => e.Otrnonconfs)
                    .HasColumnName("otrnonconfs")
                    .HasMaxLength(500);

                entity.Property(e => e.Outstatejail)
                    .HasColumnName("outstatejail")
                    .HasMaxLength(20);

                entity.Property(e => e.OutstatejailEstimate).HasColumnName("outstatejail_estimate");

                entity.Property(e => e.Outstatepris)
                    .HasColumnName("outstatepris")
                    .HasMaxLength(20);

                entity.Property(e => e.OutstateprisEstimate).HasColumnName("outstatepris_estimate");

                entity.Property(e => e.Peakdate)
                    .HasColumnName("peakdate")
                    .HasMaxLength(20);

                entity.Property(e => e.Peakpop)
                    .HasColumnName("peakpop")
                    .HasMaxLength(20);

                entity.Property(e => e.PeakpopEstimate).HasColumnName("peakpop_estimate");

                entity.Property(e => e.Pretrial)
                    .HasColumnName("pretrial")
                    .HasMaxLength(20);

                entity.Property(e => e.PretrialEstimate).HasColumnName("pretrial_estimate");

                entity.Property(e => e.Racedk)
                    .HasColumnName("racedk")
                    .HasMaxLength(20);

                entity.Property(e => e.RacedkEstimate).HasColumnName("racedk_estimate");

                entity.Property(e => e.Racetotal)
                    .HasColumnName("racetotal")
                    .HasMaxLength(20);

                entity.Property(e => e.RacetotalEstimate).HasColumnName("racetotal_estimate");

                entity.Property(e => e.Rated)
                    .HasColumnName("rated")
                    .HasMaxLength(20);

                entity.Property(e => e.RatedEstimate).HasColumnName("rated_estimate");

                entity.Property(e => e.Release)
                    .HasColumnName("release")
                    .HasMaxLength(20);

                entity.Property(e => e.ReleaseEstimate).HasColumnName("release_estimate");

                entity.Property(e => e.Releasefemale)
                    .HasColumnName("releasefemale")
                    .HasMaxLength(20);

                entity.Property(e => e.ReleasefemaleEstimate).HasColumnName("releasefemale_estimate");

                entity.Property(e => e.Releasemale)
                    .HasColumnName("releasemale")
                    .HasMaxLength(20);

                entity.Property(e => e.ReleasemaleEstimate).HasColumnName("releasemale_estimate");

                entity.Property(e => e.Rtireceived)
                    .HasColumnName("RTIReceived")
                    .HasColumnType("datetime");

                entity.Property(e => e.SubmittedDate)
                    .HasColumnName("submitted_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.SummaryQualityCodeTime).HasColumnType("datetime");

                entity.Property(e => e.SummaryStatusCodeTime).HasColumnType("datetime");

                entity.Property(e => e.SummaryStatusCodeUser).HasMaxLength(255);

                entity.Property(e => e.TextReviewNotes).HasColumnType("text");

                entity.Property(e => e.Totalstaff)
                    .HasColumnName("totalstaff")
                    .HasMaxLength(20);

                entity.Property(e => e.TotalstaffEstimate).HasColumnName("totalstaff_estimate");

                entity.Property(e => e.Totconvstatus)
                    .HasColumnName("totconvstatus")
                    .HasMaxLength(20);

                entity.Property(e => e.TotconvstatusEstimate).HasColumnName("totconvstatus_estimate");

                entity.Property(e => e.Totgender)
                    .HasColumnName("totgender")
                    .HasMaxLength(20);

                entity.Property(e => e.TotgenderEstimate).HasColumnName("totgender_estimate");

                entity.Property(e => e.Totoff)
                    .HasColumnName("totoff")
                    .HasMaxLength(20);

                entity.Property(e => e.TotoffEstimate).HasColumnName("totoff_estimate");

                entity.Property(e => e.Totpop)
                    .HasColumnName("totpop")
                    .HasMaxLength(20);

                entity.Property(e => e.TotpopEstimate).HasColumnName("totpop_estimate");

                entity.Property(e => e.Treatment)
                    .HasColumnName("treatment")
                    .HasMaxLength(20);

                entity.Property(e => e.TreatmentEstimate).HasColumnName("treatment_estimate");

                entity.Property(e => e.Tribal)
                    .HasColumnName("tribal")
                    .HasMaxLength(20);

                entity.Property(e => e.TribalEstimate).HasColumnName("tribal_estimate");

                entity.Property(e => e.Tworace)
                    .HasColumnName("tworace")
                    .HasMaxLength(20);

                entity.Property(e => e.TworaceEstimate).HasColumnName("tworace_estimate");

                entity.Property(e => e.Unconv)
                    .HasColumnName("unconv")
                    .HasMaxLength(20);

                entity.Property(e => e.UnconvEstimate).HasColumnName("unconv_estimate");

                entity.Property(e => e.Unconvhold)
                    .HasColumnName("unconvhold")
                    .HasMaxLength(20);

                entity.Property(e => e.UnconvholdEstimate).HasColumnName("unconvhold_estimate");

                entity.Property(e => e.Unconvother)
                    .HasColumnName("unconvother")
                    .HasMaxLength(20);

                entity.Property(e => e.UnconvotherEstimate).HasColumnName("unconvother_estimate");

                entity.Property(e => e.Unconvtrial)
                    .HasColumnName("unconvtrial")
                    .HasMaxLength(20);

                entity.Property(e => e.UnconvtrialEstimate).HasColumnName("unconvtrial_estimate");

                entity.Property(e => e.Week)
                    .HasColumnName("week")
                    .HasMaxLength(20);

                entity.Property(e => e.WeekEstimate).HasColumnName("week_estimate");

                entity.Property(e => e.Weekn)
                    .HasColumnName("weekn")
                    .HasMaxLength(20);

                entity.Property(e => e.WeeknEstimate).HasColumnName("weekn_estimate");

                entity.Property(e => e.White)
                    .HasColumnName("white")
                    .HasMaxLength(20);

                entity.Property(e => e.WhiteEstimate).HasColumnName("white_estimate");
            });

            modelBuilder.Entity<AsjAnnual2016>(entity =>
            {
                entity.HasKey(e => e.AsjAnnualId);

                entity.ToTable("asj_annual_2016");

                entity.Property(e => e.AsjAnnualId).HasColumnName("asj_annual_ID");

                entity.Property(e => e.Adltjuv)
                    .HasColumnName("adltjuv")
                    .HasMaxLength(20);

                entity.Property(e => e.AdltjuvEstimate).HasColumnName("adltjuv_estimate");

                entity.Property(e => e.Admis)
                    .HasColumnName("admis")
                    .HasMaxLength(20);

                entity.Property(e => e.AdmisEstimate).HasColumnName("admis_estimate");

                entity.Property(e => e.Admisfemale)
                    .HasColumnName("admisfemale")
                    .HasMaxLength(20);

                entity.Property(e => e.AdmisfemaleEstimate).HasColumnName("admisfemale_estimate");

                entity.Property(e => e.Admismale)
                    .HasColumnName("admismale")
                    .HasMaxLength(20);

                entity.Property(e => e.AdmismaleEstimate).HasColumnName("admismale_estimate");

                entity.Property(e => e.Adp)
                    .HasColumnName("adp")
                    .HasMaxLength(20);

                entity.Property(e => e.AdpEstimate).HasColumnName("adp_estimate");

                entity.Property(e => e.Adpfemale)
                    .HasColumnName("adpfemale")
                    .HasMaxLength(20);

                entity.Property(e => e.AdpfemaleEstimate).HasColumnName("adpfemale_estimate");

                entity.Property(e => e.Adpmale)
                    .HasColumnName("adpmale")
                    .HasMaxLength(20);

                entity.Property(e => e.AdpmaleEstimate).HasColumnName("adpmale_estimate");

                entity.Property(e => e.Adultf)
                    .HasColumnName("adultf")
                    .HasMaxLength(20);

                entity.Property(e => e.AdultfEstimate).HasColumnName("adultf_estimate");

                entity.Property(e => e.Adultm)
                    .HasColumnName("adultm")
                    .HasMaxLength(20);

                entity.Property(e => e.AdultmEstimate).HasColumnName("adultm_estimate");

                entity.Property(e => e.Aian)
                    .HasColumnName("AIAN")
                    .HasMaxLength(20);

                entity.Property(e => e.AianEstimate).HasColumnName("AIAN_estimate");

                entity.Property(e => e.Altwork)
                    .HasColumnName("altwork")
                    .HasMaxLength(20);

                entity.Property(e => e.AltworkEstimate).HasColumnName("altwork_estimate");

                entity.Property(e => e.Asian)
                    .HasColumnName("asian")
                    .HasMaxLength(20);

                entity.Property(e => e.AsianEstimate).HasColumnName("asian_estimate");

                entity.Property(e => e.AsjqualityCode).HasColumnName("ASJQualityCode");

                entity.Property(e => e.AsjstatusCode).HasColumnName("ASJStatusCode");

                entity.Property(e => e.Bia)
                    .HasColumnName("bia")
                    .HasMaxLength(20);

                entity.Property(e => e.BiaEstimate).HasColumnName("bia_estimate");

                entity.Property(e => e.Black)
                    .HasColumnName("black")
                    .HasMaxLength(20);

                entity.Property(e => e.BlackEstimate).HasColumnName("black_estimate");

                entity.Property(e => e.Bop)
                    .HasColumnName("bop")
                    .HasMaxLength(20);

                entity.Property(e => e.BopEstimate).HasColumnName("bop_estimate");

                entity.Property(e => e.Commsrv)
                    .HasColumnName("commsrv")
                    .HasMaxLength(20);

                entity.Property(e => e.CommsrvEstimate).HasColumnName("commsrv_estimate");

                entity.Property(e => e.Confpop)
                    .HasColumnName("confpop")
                    .HasMaxLength(20);

                entity.Property(e => e.Confpop10j)
                    .HasColumnName("confpop_10j")
                    .HasMaxLength(20);

                entity.Property(e => e.Confpop10jEstimate).HasColumnName("confpop_10j_estimate");

                entity.Property(e => e.Confpop6e)
                    .HasColumnName("confpop_6e")
                    .HasMaxLength(20);

                entity.Property(e => e.Confpop6eEstimate).HasColumnName("confpop_6e_estimate");

                entity.Property(e => e.Confpop8c)
                    .HasColumnName("confpop_8c")
                    .HasMaxLength(20);

                entity.Property(e => e.Confpop8cEstimate).HasColumnName("confpop_8c_estimate");

                entity.Property(e => e.Confpop9d)
                    .HasColumnName("confpop_9d")
                    .HasMaxLength(20);

                entity.Property(e => e.Confpop9dEstimate).HasColumnName("confpop_9d_estimate");

                entity.Property(e => e.ConfpopEstimate).HasColumnName("confpop_estimate");

                entity.Property(e => e.Confpopjune)
                    .HasColumnName("confpopjune")
                    .HasMaxLength(20);

                entity.Property(e => e.ConfpopjuneEstimate).HasColumnName("confpopjune_estimate");

                entity.Property(e => e.Conv)
                    .HasColumnName("conv")
                    .HasMaxLength(20);

                entity.Property(e => e.ConvEstimate).HasColumnName("conv_estimate");

                entity.Property(e => e.Convsent)
                    .HasColumnName("convsent")
                    .HasMaxLength(20);

                entity.Property(e => e.ConvsentEstimate).HasColumnName("convsent_estimate");

                entity.Property(e => e.Convunsent)
                    .HasColumnName("convunsent")
                    .HasMaxLength(20);

                entity.Property(e => e.ConvunsentEstimate).HasColumnName("convunsent_estimate");

                entity.Property(e => e.CorrAian)
                    .HasColumnName("corrAIAN")
                    .HasMaxLength(20);

                entity.Property(e => e.CorrAianEstimate).HasColumnName("corrAIAN_estimate");

                entity.Property(e => e.Corrasian)
                    .HasColumnName("corrasian")
                    .HasMaxLength(20);

                entity.Property(e => e.CorrasianEstimate).HasColumnName("corrasian_estimate");

                entity.Property(e => e.Corrblack)
                    .HasColumnName("corrblack")
                    .HasMaxLength(20);

                entity.Property(e => e.CorrblackEstimate).HasColumnName("corrblack_estimate");

                entity.Property(e => e.Corrhisp)
                    .HasColumnName("corrhisp")
                    .HasMaxLength(20);

                entity.Property(e => e.CorrhispEstimate).HasColumnName("corrhisp_estimate");

                entity.Property(e => e.Corrnhopi)
                    .HasColumnName("corrnhopi")
                    .HasMaxLength(20);

                entity.Property(e => e.CorrnhopiEstimate).HasColumnName("corrnhopi_estimate");

                entity.Property(e => e.Corrotherrace)
                    .HasColumnName("corrotherrace")
                    .HasMaxLength(20);

                entity.Property(e => e.CorrotherraceEstimate).HasColumnName("corrotherrace_estimate");

                entity.Property(e => e.Corrotherracespec)
                    .HasColumnName("corrotherracespec")
                    .HasMaxLength(500);

                entity.Property(e => e.Corrracedk)
                    .HasColumnName("corrracedk")
                    .HasMaxLength(20);

                entity.Property(e => e.CorrracedkEstimate).HasColumnName("corrracedk_estimate");

                entity.Property(e => e.Corrracetotal)
                    .HasColumnName("corrracetotal")
                    .HasMaxLength(20);

                entity.Property(e => e.CorrracetotalEstimate).HasColumnName("corrracetotal_estimate");

                entity.Property(e => e.Corrstaff)
                    .HasColumnName("corrstaff")
                    .HasMaxLength(20);

                entity.Property(e => e.CorrstaffEstimate).HasColumnName("corrstaff_estimate");

                entity.Property(e => e.Corrstafffemale)
                    .HasColumnName("corrstafffemale")
                    .HasMaxLength(20);

                entity.Property(e => e.CorrstafffemaleEstimate).HasColumnName("corrstafffemale_estimate");

                entity.Property(e => e.Corrstaffmale)
                    .HasColumnName("corrstaffmale")
                    .HasMaxLength(20);

                entity.Property(e => e.CorrstaffmaleEstimate).HasColumnName("corrstaffmale_estimate");

                entity.Property(e => e.Corrtworace)
                    .HasColumnName("corrtworace")
                    .HasMaxLength(20);

                entity.Property(e => e.CorrtworaceEstimate).HasColumnName("corrtworace_estimate");

                entity.Property(e => e.Corrwhite)
                    .HasColumnName("corrwhite")
                    .HasMaxLength(20);

                entity.Property(e => e.CorrwhiteEstimate).HasColumnName("corrwhite_estimate");

                entity.Property(e => e.Cost)
                    .HasColumnName("cost")
                    .HasMaxLength(20);

                entity.Property(e => e.CostEstimate).HasColumnName("cost_estimate");

                entity.Property(e => e.CreatedDate)
                    .HasColumnName("created_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.CreatedUserId)
                    .IsRequired()
                    .HasColumnName("created_user_id")
                    .HasMaxLength(256);

                entity.Property(e => e.Createdate)
                    .HasColumnName("createdate")
                    .HasColumnType("datetime");

                entity.Property(e => e.Dayreport)
                    .HasColumnName("dayreport")
                    .HasMaxLength(20);

                entity.Property(e => e.DayreportEstimate).HasColumnName("dayreport_estimate");

                entity.Property(e => e.DcrpstatusCode).HasColumnName("DCRPStatusCode");

                entity.Property(e => e.Emonitor)
                    .HasColumnName("emonitor")
                    .HasMaxLength(20);

                entity.Property(e => e.EmonitorEstimate).HasColumnName("emonitor_estimate");

                entity.Property(e => e.Felony)
                    .HasColumnName("felony")
                    .HasMaxLength(20);

                entity.Property(e => e.FelonyEstimate).HasColumnName("felony_estimate");

                entity.Property(e => e.FormReceivedById).HasColumnName("FormReceivedBy_ID");

                entity.Property(e => e.FormStatus)
                    .HasColumnName("form_status")
                    .IsUnicode(false);

                entity.Property(e => e.Hisp)
                    .HasColumnName("hisp")
                    .HasMaxLength(20);

                entity.Property(e => e.HispEstimate).HasColumnName("hisp_estimate");

                entity.Property(e => e.Homedetn)
                    .HasColumnName("homedetn")
                    .HasMaxLength(20);

                entity.Property(e => e.HomedetnEstimate).HasColumnName("homedetn_estimate");

                entity.Property(e => e.Ice)
                    .HasColumnName("ice")
                    .HasMaxLength(20);

                entity.Property(e => e.IceEstimate).HasColumnName("ice_estimate");

                entity.Property(e => e.Instatejail)
                    .HasColumnName("instatejail")
                    .HasMaxLength(20);

                entity.Property(e => e.InstatejailEstimate).HasColumnName("instatejail_estimate");

                entity.Property(e => e.Instatepris)
                    .HasColumnName("instatepris")
                    .HasMaxLength(20);

                entity.Property(e => e.InstateprisEstimate).HasColumnName("instatepris_estimate");

                entity.Property(e => e.Juvf)
                    .HasColumnName("juvf")
                    .HasMaxLength(20);

                entity.Property(e => e.JuvfEstimate).HasColumnName("juvf_estimate");

                entity.Property(e => e.Juvm)
                    .HasColumnName("juvm")
                    .HasMaxLength(20);

                entity.Property(e => e.JuvmEstimate).HasColumnName("juvm_estimate");

                entity.Property(e => e.Lastupdated)
                    .HasColumnName("lastupdated")
                    .HasColumnType("datetime");

                entity.Property(e => e.Marshals)
                    .HasColumnName("marshals")
                    .HasMaxLength(20);

                entity.Property(e => e.MarshalsEstimate).HasColumnName("marshals_estimate");

                entity.Property(e => e.Misd)
                    .HasColumnName("misd")
                    .HasMaxLength(20);

                entity.Property(e => e.MisdEstimate).HasColumnName("misd_estimate");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnName("modified_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.ModifiedUserId)
                    .IsRequired()
                    .HasColumnName("modified_user_id")
                    .HasMaxLength(256);

                entity.Property(e => e.Nconpop)
                    .HasColumnName("nconpop")
                    .HasMaxLength(20);

                entity.Property(e => e.Nconpop16i)
                    .HasColumnName("nconpop_16i")
                    .HasMaxLength(20);

                entity.Property(e => e.Nconpop16iEstimate).HasColumnName("nconpop_16i_estimate");

                entity.Property(e => e.NconpopEstimate).HasColumnName("nconpop_estimate");

                entity.Property(e => e.Nhopi)
                    .HasColumnName("nhopi")
                    .HasMaxLength(20);

                entity.Property(e => e.NhopiEstimate).HasColumnName("nhopi_estimate");

                entity.Property(e => e.Noncitz)
                    .HasColumnName("noncitz")
                    .HasMaxLength(20);

                entity.Property(e => e.NoncitzEstimate).HasColumnName("noncitz_estimate");

                entity.Property(e => e.Nonconfd)
                    .HasColumnName("nonconfd")
                    .HasMaxLength(20);

                entity.Property(e => e.NonconfdEstimate).HasColumnName("nonconfd_estimate");

                entity.Property(e => e.NumDeathsFemales)
                    .HasColumnName("num_deaths_females")
                    .HasMaxLength(50);

                entity.Property(e => e.NumDeathsMales)
                    .HasColumnName("num_deaths_males")
                    .HasMaxLength(50);

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");

                entity.Property(e => e.Otherfed)
                    .HasColumnName("otherfed")
                    .HasMaxLength(20);

                entity.Property(e => e.OtherfedEstimate).HasColumnName("otherfed_estimate");

                entity.Property(e => e.Otherfedspec)
                    .HasColumnName("otherfedspec")
                    .HasMaxLength(500);

                entity.Property(e => e.Otherholdtot)
                    .HasColumnName("otherholdtot")
                    .HasMaxLength(20);

                entity.Property(e => e.OtherholdtotEstimate).HasColumnName("otherholdtot_estimate");

                entity.Property(e => e.Otheroff)
                    .HasColumnName("otheroff")
                    .HasMaxLength(20);

                entity.Property(e => e.OtheroffEstimate).HasColumnName("otheroff_estimate");

                entity.Property(e => e.Otheroffspec)
                    .HasColumnName("otheroffspec")
                    .HasMaxLength(500);

                entity.Property(e => e.Otherrace)
                    .HasColumnName("otherrace")
                    .HasMaxLength(20);

                entity.Property(e => e.OtherraceEstimate).HasColumnName("otherrace_estimate");

                entity.Property(e => e.Otherracespec)
                    .HasColumnName("otherracespec")
                    .HasMaxLength(500);

                entity.Property(e => e.Otherstaff)
                    .HasColumnName("otherstaff")
                    .HasMaxLength(20);

                entity.Property(e => e.OtherstaffEstimate).HasColumnName("otherstaff_estimate");

                entity.Property(e => e.Otherstafffemale)
                    .HasColumnName("otherstafffemale")
                    .HasMaxLength(20);

                entity.Property(e => e.OtherstafffemaleEstimate).HasColumnName("otherstafffemale_estimate");

                entity.Property(e => e.Otherstaffmale)
                    .HasColumnName("otherstaffmale")
                    .HasMaxLength(20);

                entity.Property(e => e.OtherstaffmaleEstimate).HasColumnName("otherstaffmale_estimate");

                entity.Property(e => e.Otrnonconf)
                    .HasColumnName("otrnonconf")
                    .HasMaxLength(20);

                entity.Property(e => e.OtrnonconfEstimate).HasColumnName("otrnonconf_estimate");

                entity.Property(e => e.Otrnonconfs)
                    .HasColumnName("otrnonconfs")
                    .HasMaxLength(500);

                entity.Property(e => e.Outstatejail)
                    .HasColumnName("outstatejail")
                    .HasMaxLength(20);

                entity.Property(e => e.OutstatejailEstimate).HasColumnName("outstatejail_estimate");

                entity.Property(e => e.Outstatepris)
                    .HasColumnName("outstatepris")
                    .HasMaxLength(20);

                entity.Property(e => e.OutstateprisEstimate).HasColumnName("outstatepris_estimate");

                entity.Property(e => e.Peakdate)
                    .HasColumnName("peakdate")
                    .HasMaxLength(20);

                entity.Property(e => e.Peakpop)
                    .HasColumnName("peakpop")
                    .HasMaxLength(20);

                entity.Property(e => e.PeakpopEstimate).HasColumnName("peakpop_estimate");

                entity.Property(e => e.Pretrial)
                    .HasColumnName("pretrial")
                    .HasMaxLength(20);

                entity.Property(e => e.PretrialEstimate).HasColumnName("pretrial_estimate");

                entity.Property(e => e.Racedk)
                    .HasColumnName("racedk")
                    .HasMaxLength(20);

                entity.Property(e => e.RacedkEstimate).HasColumnName("racedk_estimate");

                entity.Property(e => e.Racetotal)
                    .HasColumnName("racetotal")
                    .HasMaxLength(20);

                entity.Property(e => e.RacetotalEstimate).HasColumnName("racetotal_estimate");

                entity.Property(e => e.Rated)
                    .HasColumnName("rated")
                    .HasMaxLength(20);

                entity.Property(e => e.RatedEstimate).HasColumnName("rated_estimate");

                entity.Property(e => e.Release)
                    .HasColumnName("release")
                    .HasMaxLength(20);

                entity.Property(e => e.ReleaseEstimate).HasColumnName("release_estimate");

                entity.Property(e => e.Releasefemale)
                    .HasColumnName("releasefemale")
                    .HasMaxLength(20);

                entity.Property(e => e.ReleasefemaleEstimate).HasColumnName("releasefemale_estimate");

                entity.Property(e => e.Releasemale)
                    .HasColumnName("releasemale")
                    .HasMaxLength(20);

                entity.Property(e => e.ReleasemaleEstimate).HasColumnName("releasemale_estimate");

                entity.Property(e => e.Rtireceived)
                    .HasColumnName("RTIReceived")
                    .HasColumnType("datetime");

                entity.Property(e => e.SubmittedDate)
                    .HasColumnName("submitted_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.SummaryQualityCodeTime).HasColumnType("datetime");

                entity.Property(e => e.SummaryStatusCodeTime).HasColumnType("datetime");

                entity.Property(e => e.SummaryStatusCodeUser).HasMaxLength(255);

                entity.Property(e => e.TextReviewNotes).HasColumnType("text");

                entity.Property(e => e.Totalstaff)
                    .HasColumnName("totalstaff")
                    .HasMaxLength(20);

                entity.Property(e => e.TotalstaffEstimate).HasColumnName("totalstaff_estimate");

                entity.Property(e => e.Totconvstatus)
                    .HasColumnName("totconvstatus")
                    .HasMaxLength(20);

                entity.Property(e => e.TotconvstatusEstimate).HasColumnName("totconvstatus_estimate");

                entity.Property(e => e.Totgender)
                    .HasColumnName("totgender")
                    .HasMaxLength(20);

                entity.Property(e => e.TotgenderEstimate).HasColumnName("totgender_estimate");

                entity.Property(e => e.Totoff)
                    .HasColumnName("totoff")
                    .HasMaxLength(20);

                entity.Property(e => e.TotoffEstimate).HasColumnName("totoff_estimate");

                entity.Property(e => e.Totpop)
                    .HasColumnName("totpop")
                    .HasMaxLength(20);

                entity.Property(e => e.TotpopEstimate).HasColumnName("totpop_estimate");

                entity.Property(e => e.Treatment)
                    .HasColumnName("treatment")
                    .HasMaxLength(20);

                entity.Property(e => e.TreatmentEstimate).HasColumnName("treatment_estimate");

                entity.Property(e => e.Tribal)
                    .HasColumnName("tribal")
                    .HasMaxLength(20);

                entity.Property(e => e.TribalEstimate).HasColumnName("tribal_estimate");

                entity.Property(e => e.Tworace)
                    .HasColumnName("tworace")
                    .HasMaxLength(20);

                entity.Property(e => e.TworaceEstimate).HasColumnName("tworace_estimate");

                entity.Property(e => e.Unconv)
                    .HasColumnName("unconv")
                    .HasMaxLength(20);

                entity.Property(e => e.UnconvEstimate).HasColumnName("unconv_estimate");

                entity.Property(e => e.Unconvhold)
                    .HasColumnName("unconvhold")
                    .HasMaxLength(20);

                entity.Property(e => e.UnconvholdEstimate).HasColumnName("unconvhold_estimate");

                entity.Property(e => e.Unconvother)
                    .HasColumnName("unconvother")
                    .HasMaxLength(20);

                entity.Property(e => e.UnconvotherEstimate).HasColumnName("unconvother_estimate");

                entity.Property(e => e.Unconvtrial)
                    .HasColumnName("unconvtrial")
                    .HasMaxLength(20);

                entity.Property(e => e.UnconvtrialEstimate).HasColumnName("unconvtrial_estimate");

                entity.Property(e => e.Week)
                    .HasColumnName("week")
                    .HasMaxLength(20);

                entity.Property(e => e.WeekEstimate).HasColumnName("week_estimate");

                entity.Property(e => e.Weekn)
                    .HasColumnName("weekn")
                    .HasMaxLength(20);

                entity.Property(e => e.WeeknEstimate).HasColumnName("weekn_estimate");

                entity.Property(e => e.White)
                    .HasColumnName("white")
                    .HasMaxLength(20);

                entity.Property(e => e.WhiteEstimate).HasColumnName("white_estimate");
            });

            modelBuilder.Entity<AsjAnnual2017>(entity =>
            {
                entity.HasKey(e => e.AsjAnnualId);

                entity.ToTable("asj_annual_2017");

                entity.Property(e => e.AsjAnnualId).HasColumnName("asj_annual_ID");

                entity.Property(e => e.Adltjuv)
                    .HasColumnName("adltjuv")
                    .HasMaxLength(20);

                entity.Property(e => e.AdltjuvEstimate).HasColumnName("adltjuv_estimate");

                entity.Property(e => e.Admis)
                    .HasColumnName("admis")
                    .HasMaxLength(20);

                entity.Property(e => e.AdmisEstimate).HasColumnName("admis_estimate");

                entity.Property(e => e.Admisfemale)
                    .HasColumnName("admisfemale")
                    .HasMaxLength(20);

                entity.Property(e => e.AdmisfemaleEstimate).HasColumnName("admisfemale_estimate");

                entity.Property(e => e.Admismale)
                    .HasColumnName("admismale")
                    .HasMaxLength(20);

                entity.Property(e => e.AdmismaleEstimate).HasColumnName("admismale_estimate");

                entity.Property(e => e.Adp)
                    .HasColumnName("adp")
                    .HasMaxLength(20);

                entity.Property(e => e.AdpEstimate).HasColumnName("adp_estimate");

                entity.Property(e => e.Adpfemale)
                    .HasColumnName("adpfemale")
                    .HasMaxLength(20);

                entity.Property(e => e.AdpfemaleEstimate).HasColumnName("adpfemale_estimate");

                entity.Property(e => e.Adpmale)
                    .HasColumnName("adpmale")
                    .HasMaxLength(20);

                entity.Property(e => e.AdpmaleEstimate).HasColumnName("adpmale_estimate");

                entity.Property(e => e.Adultf)
                    .HasColumnName("adultf")
                    .HasMaxLength(20);

                entity.Property(e => e.AdultfEstimate).HasColumnName("adultf_estimate");

                entity.Property(e => e.Adultm)
                    .HasColumnName("adultm")
                    .HasMaxLength(20);

                entity.Property(e => e.AdultmEstimate).HasColumnName("adultm_estimate");

                entity.Property(e => e.Aian)
                    .HasColumnName("AIAN")
                    .HasMaxLength(20);

                entity.Property(e => e.AianEstimate).HasColumnName("AIAN_estimate");

                entity.Property(e => e.Altwork)
                    .HasColumnName("altwork")
                    .HasMaxLength(20);

                entity.Property(e => e.AltworkEstimate).HasColumnName("altwork_estimate");

                entity.Property(e => e.Asian)
                    .HasColumnName("asian")
                    .HasMaxLength(20);

                entity.Property(e => e.AsianEstimate).HasColumnName("asian_estimate");

                entity.Property(e => e.AsjqualityCode).HasColumnName("ASJQualityCode");

                entity.Property(e => e.AsjstatusCode).HasColumnName("ASJStatusCode");

                entity.Property(e => e.Bia)
                    .HasColumnName("bia")
                    .HasMaxLength(20);

                entity.Property(e => e.BiaEstimate).HasColumnName("bia_estimate");

                entity.Property(e => e.Black)
                    .HasColumnName("black")
                    .HasMaxLength(20);

                entity.Property(e => e.BlackEstimate).HasColumnName("black_estimate");

                entity.Property(e => e.Bop)
                    .HasColumnName("bop")
                    .HasMaxLength(20);

                entity.Property(e => e.BopEstimate).HasColumnName("bop_estimate");

                entity.Property(e => e.Commsrv)
                    .HasColumnName("commsrv")
                    .HasMaxLength(20);

                entity.Property(e => e.CommsrvEstimate).HasColumnName("commsrv_estimate");

                entity.Property(e => e.Confpop)
                    .HasColumnName("confpop")
                    .HasMaxLength(20);

                entity.Property(e => e.Confpop4e)
                    .HasColumnName("confpop_4e")
                    .HasMaxLength(20);

                entity.Property(e => e.Confpop4eEstimate).HasColumnName("confpop_4e_estimate");

                entity.Property(e => e.Confpop6c)
                    .HasColumnName("confpop_6c")
                    .HasMaxLength(20);

                entity.Property(e => e.Confpop6cEstimate).HasColumnName("confpop_6c_estimate");

                entity.Property(e => e.Confpop7d)
                    .HasColumnName("confpop_7d")
                    .HasMaxLength(20);

                entity.Property(e => e.Confpop7dEstimate).HasColumnName("confpop_7d_estimate");

                entity.Property(e => e.Confpop8j)
                    .HasColumnName("confpop_8j")
                    .HasMaxLength(20);

                entity.Property(e => e.Confpop8jEstimate).HasColumnName("confpop_8j_estimate");

                entity.Property(e => e.ConfpopEstimate).HasColumnName("confpop_estimate");

                entity.Property(e => e.Confpopjune)
                    .HasColumnName("confpopjune")
                    .HasMaxLength(20);

                entity.Property(e => e.ConfpopjuneEstimate).HasColumnName("confpopjune_estimate");

                entity.Property(e => e.Conv)
                    .HasColumnName("conv")
                    .HasMaxLength(20);

                entity.Property(e => e.ConvEstimate).HasColumnName("conv_estimate");

                entity.Property(e => e.Convsent)
                    .HasColumnName("convsent")
                    .HasMaxLength(20);

                entity.Property(e => e.ConvsentEstimate).HasColumnName("convsent_estimate");

                entity.Property(e => e.Convunsent)
                    .HasColumnName("convunsent")
                    .HasMaxLength(20);

                entity.Property(e => e.ConvunsentEstimate).HasColumnName("convunsent_estimate");

                entity.Property(e => e.CorrAian)
                    .HasColumnName("corrAIAN")
                    .HasMaxLength(20);

                entity.Property(e => e.CorrAianEstimate).HasColumnName("corrAIAN_estimate");

                entity.Property(e => e.Corrasian)
                    .HasColumnName("corrasian")
                    .HasMaxLength(20);

                entity.Property(e => e.CorrasianEstimate).HasColumnName("corrasian_estimate");

                entity.Property(e => e.Corrblack)
                    .HasColumnName("corrblack")
                    .HasMaxLength(20);

                entity.Property(e => e.CorrblackEstimate).HasColumnName("corrblack_estimate");

                entity.Property(e => e.Corrhisp)
                    .HasColumnName("corrhisp")
                    .HasMaxLength(20);

                entity.Property(e => e.CorrhispEstimate).HasColumnName("corrhisp_estimate");

                entity.Property(e => e.Corrnhopi)
                    .HasColumnName("corrnhopi")
                    .HasMaxLength(20);

                entity.Property(e => e.CorrnhopiEstimate).HasColumnName("corrnhopi_estimate");

                entity.Property(e => e.Corrotherrace)
                    .HasColumnName("corrotherrace")
                    .HasMaxLength(20);

                entity.Property(e => e.CorrotherraceEstimate).HasColumnName("corrotherrace_estimate");

                entity.Property(e => e.Corrotherracespec)
                    .HasColumnName("corrotherracespec")
                    .HasMaxLength(500);

                entity.Property(e => e.Corrracedk)
                    .HasColumnName("corrracedk")
                    .HasMaxLength(20);

                entity.Property(e => e.CorrracedkEstimate).HasColumnName("corrracedk_estimate");

                entity.Property(e => e.Corrracetotal)
                    .HasColumnName("corrracetotal")
                    .HasMaxLength(20);

                entity.Property(e => e.CorrracetotalEstimate).HasColumnName("corrracetotal_estimate");

                entity.Property(e => e.Corrstaff)
                    .HasColumnName("corrstaff")
                    .HasMaxLength(20);

                entity.Property(e => e.CorrstaffEstimate).HasColumnName("corrstaff_estimate");

                entity.Property(e => e.Corrstafffemale)
                    .HasColumnName("corrstafffemale")
                    .HasMaxLength(20);

                entity.Property(e => e.CorrstafffemaleEstimate).HasColumnName("corrstafffemale_estimate");

                entity.Property(e => e.Corrstaffmale)
                    .HasColumnName("corrstaffmale")
                    .HasMaxLength(20);

                entity.Property(e => e.CorrstaffmaleEstimate).HasColumnName("corrstaffmale_estimate");

                entity.Property(e => e.Corrtworace)
                    .HasColumnName("corrtworace")
                    .HasMaxLength(20);

                entity.Property(e => e.CorrtworaceEstimate).HasColumnName("corrtworace_estimate");

                entity.Property(e => e.Corrwhite)
                    .HasColumnName("corrwhite")
                    .HasMaxLength(20);

                entity.Property(e => e.CorrwhiteEstimate).HasColumnName("corrwhite_estimate");

                entity.Property(e => e.Cost)
                    .HasColumnName("cost")
                    .HasMaxLength(20);

                entity.Property(e => e.CostEstimate).HasColumnName("cost_estimate");

                entity.Property(e => e.CreatedDate)
                    .HasColumnName("created_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.CreatedUserId)
                    .IsRequired()
                    .HasColumnName("created_user_id")
                    .HasMaxLength(256);

                entity.Property(e => e.Createdate)
                    .HasColumnName("createdate")
                    .HasColumnType("datetime");

                entity.Property(e => e.Dayreport)
                    .HasColumnName("dayreport")
                    .HasMaxLength(20);

                entity.Property(e => e.DayreportEstimate).HasColumnName("dayreport_estimate");

                entity.Property(e => e.DcrpstatusCode).HasColumnName("DCRPStatusCode");

                entity.Property(e => e.Emonitor)
                    .HasColumnName("emonitor")
                    .HasMaxLength(20);

                entity.Property(e => e.EmonitorEstimate).HasColumnName("emonitor_estimate");

                entity.Property(e => e.Felony)
                    .HasColumnName("felony")
                    .HasMaxLength(20);

                entity.Property(e => e.FelonyEstimate).HasColumnName("felony_estimate");

                entity.Property(e => e.FormReceivedById).HasColumnName("FormReceivedBy_ID");

                entity.Property(e => e.FormStatus).HasColumnName("form_status");

                entity.Property(e => e.Hisp)
                    .HasColumnName("hisp")
                    .HasMaxLength(20);

                entity.Property(e => e.HispEstimate).HasColumnName("hisp_estimate");

                entity.Property(e => e.Homedetn)
                    .HasColumnName("homedetn")
                    .HasMaxLength(20);

                entity.Property(e => e.HomedetnEstimate).HasColumnName("homedetn_estimate");

                entity.Property(e => e.Ice)
                    .HasColumnName("ice")
                    .HasMaxLength(20);

                entity.Property(e => e.IceEstimate).HasColumnName("ice_estimate");

                entity.Property(e => e.Instatejail)
                    .HasColumnName("instatejail")
                    .HasMaxLength(20);

                entity.Property(e => e.InstatejailEstimate).HasColumnName("instatejail_estimate");

                entity.Property(e => e.Instatepris)
                    .HasColumnName("instatepris")
                    .HasMaxLength(20);

                entity.Property(e => e.InstateprisEstimate).HasColumnName("instatepris_estimate");

                entity.Property(e => e.Juvf)
                    .HasColumnName("juvf")
                    .HasMaxLength(20);

                entity.Property(e => e.JuvfEstimate).HasColumnName("juvf_estimate");

                entity.Property(e => e.Juvm)
                    .HasColumnName("juvm")
                    .HasMaxLength(20);

                entity.Property(e => e.JuvmEstimate).HasColumnName("juvm_estimate");

                entity.Property(e => e.Lastupdated)
                    .HasColumnName("lastupdated")
                    .HasColumnType("datetime");

                entity.Property(e => e.Marshals)
                    .HasColumnName("marshals")
                    .HasMaxLength(20);

                entity.Property(e => e.MarshalsEstimate).HasColumnName("marshals_estimate");

                entity.Property(e => e.Misd)
                    .HasColumnName("misd")
                    .HasMaxLength(20);

                entity.Property(e => e.MisdEstimate).HasColumnName("misd_estimate");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnName("modified_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.ModifiedUserId)
                    .IsRequired()
                    .HasColumnName("modified_user_id")
                    .HasMaxLength(256);

                entity.Property(e => e.Nconpop)
                    .HasColumnName("nconpop")
                    .HasMaxLength(20);

                entity.Property(e => e.Nconpop14i)
                    .HasColumnName("nconpop_14i")
                    .HasMaxLength(20);

                entity.Property(e => e.Nconpop14iEstimate).HasColumnName("nconpop_14i_estimate");

                entity.Property(e => e.NconpopEstimate).HasColumnName("nconpop_estimate");

                entity.Property(e => e.Nhopi)
                    .HasColumnName("nhopi")
                    .HasMaxLength(20);

                entity.Property(e => e.NhopiEstimate).HasColumnName("nhopi_estimate");

                entity.Property(e => e.Noncitz)
                    .HasColumnName("noncitz")
                    .HasMaxLength(20);

                entity.Property(e => e.NoncitzEstimate).HasColumnName("noncitz_estimate");

                entity.Property(e => e.Nonconfd)
                    .HasColumnName("nonconfd")
                    .HasMaxLength(20);

                entity.Property(e => e.NonconfdEstimate).HasColumnName("nonconfd_estimate");

                entity.Property(e => e.NumDeathsFemales)
                    .HasColumnName("num_deaths_females")
                    .HasMaxLength(50);

                entity.Property(e => e.NumDeathsMales)
                    .HasColumnName("num_deaths_males")
                    .HasMaxLength(50);

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");

                entity.Property(e => e.Otherfed)
                    .HasColumnName("otherfed")
                    .HasMaxLength(20);

                entity.Property(e => e.OtherfedEstimate).HasColumnName("otherfed_estimate");

                entity.Property(e => e.Otherfedspec)
                    .HasColumnName("otherfedspec")
                    .HasMaxLength(500);

                entity.Property(e => e.Otherholdtot)
                    .HasColumnName("otherholdtot")
                    .HasMaxLength(20);

                entity.Property(e => e.OtherholdtotEstimate).HasColumnName("otherholdtot_estimate");

                entity.Property(e => e.Otheroff)
                    .HasColumnName("otheroff")
                    .HasMaxLength(20);

                entity.Property(e => e.OtheroffEstimate).HasColumnName("otheroff_estimate");

                entity.Property(e => e.Otheroffspec)
                    .HasColumnName("otheroffspec")
                    .HasMaxLength(500);

                entity.Property(e => e.Otherrace)
                    .HasColumnName("otherrace")
                    .HasMaxLength(20);

                entity.Property(e => e.OtherraceEstimate).HasColumnName("otherrace_estimate");

                entity.Property(e => e.Otherracespec)
                    .HasColumnName("otherracespec")
                    .HasMaxLength(500);

                entity.Property(e => e.Otherstaff)
                    .HasColumnName("otherstaff")
                    .HasMaxLength(20);

                entity.Property(e => e.OtherstaffEstimate).HasColumnName("otherstaff_estimate");

                entity.Property(e => e.Otherstafffemale)
                    .HasColumnName("otherstafffemale")
                    .HasMaxLength(20);

                entity.Property(e => e.OtherstafffemaleEstimate).HasColumnName("otherstafffemale_estimate");

                entity.Property(e => e.Otherstaffmale)
                    .HasColumnName("otherstaffmale")
                    .HasMaxLength(20);

                entity.Property(e => e.OtherstaffmaleEstimate).HasColumnName("otherstaffmale_estimate");

                entity.Property(e => e.Otrnonconf)
                    .HasColumnName("otrnonconf")
                    .HasMaxLength(20);

                entity.Property(e => e.OtrnonconfEstimate).HasColumnName("otrnonconf_estimate");

                entity.Property(e => e.Otrnonconfs)
                    .HasColumnName("otrnonconfs")
                    .HasMaxLength(500);

                entity.Property(e => e.Outstatejail)
                    .HasColumnName("outstatejail")
                    .HasMaxLength(20);

                entity.Property(e => e.OutstatejailEstimate).HasColumnName("outstatejail_estimate");

                entity.Property(e => e.Outstatepris)
                    .HasColumnName("outstatepris")
                    .HasMaxLength(20);

                entity.Property(e => e.OutstateprisEstimate).HasColumnName("outstatepris_estimate");

                entity.Property(e => e.Peakdate)
                    .HasColumnName("peakdate")
                    .HasMaxLength(20);

                entity.Property(e => e.Peakpop)
                    .HasColumnName("peakpop")
                    .HasMaxLength(20);

                entity.Property(e => e.PeakpopEstimate).HasColumnName("peakpop_estimate");

                entity.Property(e => e.Pretrial)
                    .HasColumnName("pretrial")
                    .HasMaxLength(20);

                entity.Property(e => e.PretrialEstimate).HasColumnName("pretrial_estimate");

                entity.Property(e => e.Racedk)
                    .HasColumnName("racedk")
                    .HasMaxLength(20);

                entity.Property(e => e.RacedkEstimate).HasColumnName("racedk_estimate");

                entity.Property(e => e.Racetotal)
                    .HasColumnName("racetotal")
                    .HasMaxLength(20);

                entity.Property(e => e.RacetotalEstimate).HasColumnName("racetotal_estimate");

                entity.Property(e => e.Rated)
                    .HasColumnName("rated")
                    .HasMaxLength(20);

                entity.Property(e => e.RatedEstimate).HasColumnName("rated_estimate");

                entity.Property(e => e.Release)
                    .HasColumnName("release")
                    .HasMaxLength(20);

                entity.Property(e => e.ReleaseEstimate).HasColumnName("release_estimate");

                entity.Property(e => e.Releasefemale)
                    .HasColumnName("releasefemale")
                    .HasMaxLength(20);

                entity.Property(e => e.ReleasefemaleEstimate).HasColumnName("releasefemale_estimate");

                entity.Property(e => e.Releasemale)
                    .HasColumnName("releasemale")
                    .HasMaxLength(20);

                entity.Property(e => e.ReleasemaleEstimate).HasColumnName("releasemale_estimate");

                entity.Property(e => e.Rtireceived)
                    .HasColumnName("RTIReceived")
                    .HasColumnType("datetime");

                entity.Property(e => e.SubmittedDate)
                    .HasColumnName("submitted_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.SummaryQualityCodeTime).HasColumnType("datetime");

                entity.Property(e => e.SummaryStatusCodeTime).HasColumnType("datetime");

                entity.Property(e => e.SummaryStatusCodeUser).HasMaxLength(255);

                entity.Property(e => e.TextReviewNotes).HasColumnType("text");

                entity.Property(e => e.Totalstaff)
                    .HasColumnName("totalstaff")
                    .HasMaxLength(20);

                entity.Property(e => e.TotalstaffEstimate).HasColumnName("totalstaff_estimate");

                entity.Property(e => e.Totconvstatus)
                    .HasColumnName("totconvstatus")
                    .HasMaxLength(20);

                entity.Property(e => e.TotconvstatusEstimate).HasColumnName("totconvstatus_estimate");

                entity.Property(e => e.Totgender)
                    .HasColumnName("totgender")
                    .HasMaxLength(20);

                entity.Property(e => e.TotgenderEstimate).HasColumnName("totgender_estimate");

                entity.Property(e => e.Totoff)
                    .HasColumnName("totoff")
                    .HasMaxLength(20);

                entity.Property(e => e.TotoffEstimate).HasColumnName("totoff_estimate");

                entity.Property(e => e.Totpop)
                    .HasColumnName("totpop")
                    .HasMaxLength(20);

                entity.Property(e => e.TotpopEstimate).HasColumnName("totpop_estimate");

                entity.Property(e => e.Treatment)
                    .HasColumnName("treatment")
                    .HasMaxLength(20);

                entity.Property(e => e.TreatmentEstimate).HasColumnName("treatment_estimate");

                entity.Property(e => e.Tribal)
                    .HasColumnName("tribal")
                    .HasMaxLength(20);

                entity.Property(e => e.TribalEstimate).HasColumnName("tribal_estimate");

                entity.Property(e => e.Tworace)
                    .HasColumnName("tworace")
                    .HasMaxLength(20);

                entity.Property(e => e.TworaceEstimate).HasColumnName("tworace_estimate");

                entity.Property(e => e.Unconv)
                    .HasColumnName("unconv")
                    .HasMaxLength(20);

                entity.Property(e => e.UnconvEstimate).HasColumnName("unconv_estimate");

                entity.Property(e => e.Unconvhold)
                    .HasColumnName("unconvhold")
                    .HasMaxLength(20);

                entity.Property(e => e.UnconvholdEstimate).HasColumnName("unconvhold_estimate");

                entity.Property(e => e.Unconvother)
                    .HasColumnName("unconvother")
                    .HasMaxLength(20);

                entity.Property(e => e.UnconvotherEstimate).HasColumnName("unconvother_estimate");

                entity.Property(e => e.Unconvtrial)
                    .HasColumnName("unconvtrial")
                    .HasMaxLength(20);

                entity.Property(e => e.UnconvtrialEstimate).HasColumnName("unconvtrial_estimate");

                entity.Property(e => e.Week)
                    .HasColumnName("week")
                    .HasMaxLength(20);

                entity.Property(e => e.WeekEstimate).HasColumnName("week_estimate");

                entity.Property(e => e.Weekn)
                    .HasColumnName("weekn")
                    .HasMaxLength(20);

                entity.Property(e => e.WeeknEstimate).HasColumnName("weekn_estimate");

                entity.Property(e => e.White)
                    .HasColumnName("white")
                    .HasMaxLength(20);

                entity.Property(e => e.WhiteEstimate).HasColumnName("white_estimate");
            });

            modelBuilder.Entity<AsjAnnual2017Reasons>(entity =>
            {
                entity.HasKey(e => e.AsjAnnualReasonId);

                entity.ToTable("asj_annual_2017_reasons");

                entity.Property(e => e.AsjAnnualReasonId).HasColumnName("asj_annual_reason_ID");

                entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");

                entity.Property(e => e.Question)
                    .HasColumnName("Question")
                    .HasMaxLength(5);

                entity.Property(e => e.Reason)
                    .HasColumnName("Reason")
                    .HasMaxLength(500);

                entity.Property(e => e.LastUpdated)
                    .HasColumnName("LastUpdated")
                    .HasColumnType("datetime");

                entity.Property(e => e.CreatedDate)
                    .HasColumnName("created_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.CreatedUserId)
                    .IsRequired()
                    .HasColumnName("created_user_id")
                    .HasMaxLength(256);

                entity.Property(e => e.ModifiedDate)
                    .HasColumnName("modified_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.ModifiedUserId)
                    .IsRequired()
                    .HasColumnName("modified_user_id")
                    .HasMaxLength(256);

            });

        }
    }
}
