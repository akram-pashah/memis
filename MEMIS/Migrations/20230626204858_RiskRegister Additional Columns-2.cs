using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MEMIS.Migrations
{
    public partial class RiskRegisterAdditionalColumns2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ActionTaken",
                table: "RiskRegister",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ActualBy",
                table: "RiskRegister",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ActualDate",
                table: "RiskRegister",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActionTaken",
                table: "RiskRegister");

            migrationBuilder.DropColumn(
                name: "ActualBy",
                table: "RiskRegister");

            migrationBuilder.DropColumn(
                name: "ActualDate",
                table: "RiskRegister");
        }
    }
}
