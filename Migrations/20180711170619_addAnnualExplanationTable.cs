using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ASJ.Migrations
{
    public partial class addAnnualExplanationTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AnnualExplanations",
                columns: table => new
                {
                    OrganizationID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    explanation_p1 = table.Column<string>(nullable: true),
                    explanation_p10 = table.Column<string>(nullable: true),
                    explanation_p11 = table.Column<string>(nullable: true),
                    explanation_p12 = table.Column<string>(nullable: true),
                    explanation_p13 = table.Column<string>(nullable: true),
                    explanation_p14 = table.Column<string>(nullable: true),
                    explanation_p15 = table.Column<string>(nullable: true),
                    explanation_p16 = table.Column<string>(nullable: true),
                    explanation_p2 = table.Column<string>(nullable: true),
                    explanation_p3 = table.Column<string>(nullable: true),
                    explanation_p4 = table.Column<string>(nullable: true),
                    explanation_p5 = table.Column<string>(nullable: true),
                    explanation_p6 = table.Column<string>(nullable: true),
                    explanation_p7 = table.Column<string>(nullable: true),
                    explanation_p8 = table.Column<string>(nullable: true),
                    explanation_p9 = table.Column<string>(nullable: true),
                    iDQFU_admis_explanation = table.Column<string>(nullable: true),
                    iDQFU_adp_explanation = table.Column<string>(nullable: true),
                    iDQFU_conpop_explanation = table.Column<string>(nullable: true),
                    iDQFU_nconpop_explanation = table.Column<string>(nullable: true),
                    iDQFU_rated_explanation = table.Column<string>(nullable: true),
                    iDQFU_release_explanation = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnnualExplanations", x => x.OrganizationID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnnualExplanations");
        }
    }
}
