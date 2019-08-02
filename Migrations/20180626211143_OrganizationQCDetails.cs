using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ASJ.Migrations
{
    public partial class OrganizationQCDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           

            migrationBuilder.CreateTable(
                name: "OrganizationQCDetails",
                columns: table => new
                {
                    OrganizationQcDetailsId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CYrange = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    DeathID = table.Column<int>(nullable: false),
                    FirstAppeared = table.Column<DateTime>(nullable: false),
                    InstrumentId = table.Column<int>(nullable: true),
                    Location = table.Column<int>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: false),
                    OrganizationId = table.Column<int>(nullable: true),
                    OrganizationYear = table.Column<int>(nullable: true),
                    PYrange = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationQCDetails", x => x.OrganizationQcDetailsId);
                    table.ForeignKey(
                        name: "FK_OrganizationQCDetails_Instruments_InstrumentId",
                        column: x => x.InstrumentId,
                        principalTable: "Instruments",
                        principalColumn: "InstrumentId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrganizationQCDetails_Organizations_OrganizationId_OrganizationYear",
                        columns: x => new { x.OrganizationId, x.OrganizationYear },
                        principalTable: "Organizations",
                        principalColumns: new[] { "OrganizationId", "Year" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationQCDetails_InstrumentId",
                table: "OrganizationQCDetails",
                column: "InstrumentId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationQCDetails_OrganizationId_OrganizationYear",
                table: "OrganizationQCDetails",
                columns: new[] { "OrganizationId", "OrganizationYear" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrganizationQCDetails");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "asj_annual_2018");

            migrationBuilder.DropColumn(
                name: "City",
                table: "asj_annual_2018");

            migrationBuilder.DropColumn(
                name: "Fax",
                table: "asj_annual_2018");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "asj_annual_2018");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "asj_annual_2018");

            migrationBuilder.DropColumn(
                name: "State",
                table: "asj_annual_2018");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "asj_annual_2018");

            migrationBuilder.DropColumn(
                name: "Zip",
                table: "asj_annual_2018");

            migrationBuilder.DropColumn(
                name: "email",
                table: "asj_annual_2018");
        }
    }
}
