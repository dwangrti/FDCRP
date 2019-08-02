using ASJ.Models.Form;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASJ.Models.PDF;

namespace ASJ.Models
{
    public  class ASJDbContext : DbContext
    {
        public ASJDbContext(DbContextOptions<ASJDbContext> options)
            : base(options)
        { }
            
        public DbSet<RoleGroup> RoleGroups { get; set; }
        public DbSet<UserRoleGroups> UserRoleGroups { get; set; }
        public DbSet<RoleGroupRoles> RoleGroupRoles { get; set; }

        public DbSet<LookupEvent> LookupEvents { get; set; }
        public DbSet<LookupAgencyStatus> LookupAgencyStatuses { get; set; }
        public DbSet<LookupOrganizationType> LookupOrganizationTypes { get; set; }
        public DbSet<LookupSpecialCaseCode> LookupSpecialCaseCodes { get; set; }
        public DbSet<LookupQuestionType> LookupQuestionTypes { get; set; }
        public DbSet<LookupMode> LookupModes { get; set; }
        public DbSet<LookupSummaryStatus> LookupSummaryStatuses { get; set; }

        public DbSet<Organization> Organizations { get; set; }
        public DbSet<OrganizationEvent> OrganizationEvents { get; set; }
        public DbSet<OrganizationFollowup> OrganizationFollowups { get; set; }
        public DbSet<OrganizationContacts> OrganizationContacts { get; set; }
        public DbSet<OrganizationNotes> OrganizationNotes { get; set; }
        public DbSet<OrganizationQCDetails> OrganizationQCDetails { get; set; }
        public DbSet<OrganizationAction> OrganizationActions { get; set; }

        public DbSet<DataSupplier> DataSuppliers { get; set; }
        public DbSet<OrganizationFacility> OrganizationFacilities { get; set; }

        public DbSet<Instrument> Instruments { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<InstrumentQuestion> InstrumentQuestions { get; set; }
        public DbSet<Response> Responses { get; set; }
        public DbSet<ResponseOption> ResponseOptions { get; set; }
        public DbSet<QuestionIncludeExclude> QuestionIncludesExcludes { get; set; }
        public DbSet<QuestionTotalAddend> QuestionTotalAddends { get; set; }
        public DbSet<InstrumentOrganizationType> InstrumentOrganizationTypes { get; set; }
        public DbSet<InstrumentActionLog> InstrumentActionLogs { get; set; }
        public DbSet<DQFUEmail> DQFUEmails { get; set; }

        public DbSet<PDFAnnualASJ> PDFAnnualASJs { get; set; }
        public DbSet<PDFCognitive> PDFCognitives { get; set; }
  
        public DbSet<DQFUvalue> DQFUvalues { get; set; }

        public DbSet<AnnualExplanation> AnnualExplanations { get; set; }

        public DbSet<LookupContactType> LookupContactType { get; set; }

        //public DbSet<AsjAnnual2015> AsjAnnual2015 { get; set; }
        //public DbSet<AsjAnnual2016> AsjAnnual2016 { get; set; }
        //public DbSet<AsjAnnual2017Reasons> AsjAnnual2017Reasons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LookupEvent>()
               .HasKey(c => c.EventId);
            modelBuilder.Entity<LookupAgencyStatus>()
                           .HasKey(c => c.AgencyStatusCodeId);
            modelBuilder.Entity<LookupOrganizationType>()
                           .HasKey(c => c.OrganizationTypeId);
            modelBuilder.Entity<LookupSpecialCaseCode>()
                           .HasKey(c => c.SpecialCaseCodeId);
            modelBuilder.Entity<LookupQuestionType>()
                                      .HasKey(c => c.QuestionTypeId);


            modelBuilder.Entity<Organization>()
                .HasKey(c => new { c.OrganizationId, c.Year });
            modelBuilder.Entity<OrganizationEvent>()
                .HasKey(c => c.OrganizationEventId);
            modelBuilder.Entity<OrganizationFollowup>()
                        .HasKey(c => c.OrganizationFollowupId);
            modelBuilder.Entity<PDFCognitive>() 
                        .HasKey(c => c.organizationid);
            modelBuilder.Entity<DQFUvalue>()
                .HasKey(c => c.OrganizationID);
            modelBuilder.Entity<AnnualExplanation>()
                .HasKey(c => c.OrganizationID);
        }
        //TVINCENT - We have to have the following ModelCreating override because our database names are going to be singular 
        //           and our property names are going to be plural. 
        //           This loop tells the EF to use the type name for the table name instead of the DBSet property name
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    foreach (var entity in modelBuilder.Model.GetEntityTypes())
        //    {
        //        entity.Relational().TableName = entity.DisplayName();
        //    }
        //}

    }
}
