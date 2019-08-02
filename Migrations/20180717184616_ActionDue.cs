using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ASJ.Migrations
{
    public partial class ActionDue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OrganizationActions",
                columns: table => new
                {
                    OrganizationActionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ActionNotes = table.Column<string>(nullable: true),
                    AssignedTo = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: false),
                    OrganizationId = table.Column<int>(nullable: true),
                    OrganizationYear = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationActions", x => x.OrganizationActionId);
                    table.ForeignKey(
                        name: "FK_OrganizationActions_Organizations_OrganizationId_OrganizationYear",
                        columns: x => new { x.OrganizationId, x.OrganizationYear },
                        principalTable: "Organizations",
                        principalColumns: new[] { "OrganizationId", "Year" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationActions_OrganizationId_OrganizationYear",
                table: "OrganizationActions",
                columns: new[] { "OrganizationId", "OrganizationYear" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrganizationActions");
        }
    }
}
