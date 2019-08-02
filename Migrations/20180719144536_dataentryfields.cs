using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ASJ.Migrations
{
    public partial class dataentryfields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GDC",
                table: "Organizations",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateReceived",
                table: "OrganizationFollowups",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SubmissionMode",
                table: "OrganizationFollowups",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "LookupSummaryStatuses",
                columns: table => new
                {
                    SummaryStatusCodeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SummaryStatusCodeDescription = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LookupSummaryStatuses", x => x.SummaryStatusCodeId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LookupSummaryStatuses");

            migrationBuilder.DropColumn(
                name: "GDC",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "DateReceived",
                table: "OrganizationFollowups");

            migrationBuilder.DropColumn(
                name: "SubmissionMode",
                table: "OrganizationFollowups");
        }
    }
}
