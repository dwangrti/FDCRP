using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ASJ.Migrations
{
    public partial class removeOrgFollowup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           
            migrationBuilder.DropIndex(
                name: "IX_Organizations_OrganizationFollowupId",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "OrganizationFollowupId",
                table: "Organizations");

           
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            

           
            migrationBuilder.DropIndex(
                name: "IX_OrganizationFollowups_OrganizationId_OrganizationYear",
                table: "OrganizationFollowups");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                table: "OrganizationFollowups");

            migrationBuilder.DropColumn(
                name: "OrganizationYear",
                table: "OrganizationFollowups");

            migrationBuilder.AddColumn<int>(
                name: "OrganizationFollowupId",
                table: "Organizations",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_OrganizationFollowupId",
                table: "Organizations",
                column: "OrganizationFollowupId",
                unique: true,
                filter: "[OrganizationFollowupId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Organizations_OrganizationFollowups_OrganizationFollowupId",
                table: "Organizations",
                column: "OrganizationFollowupId",
                principalTable: "OrganizationFollowups",
                principalColumn: "OrganizationFollowupId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
