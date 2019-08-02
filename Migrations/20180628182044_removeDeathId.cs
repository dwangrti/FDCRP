using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ASJ.Migrations
{
    public partial class removeDeathId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
              migrationBuilder.DropColumn(
                name: "DeathID",
                table: "OrganizationQCDetails");

            migrationBuilder.AlterColumn<int>(
                name: "TextReview",
                table: "OrganizationFollowups",
                nullable: true,
                oldClrType: typeof(int));

          
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
          

            migrationBuilder.AddColumn<int>(
                name: "DeathID",
                table: "OrganizationQCDetails",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "TextReview",
                table: "OrganizationFollowups",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

         
          
        }
    }
}
