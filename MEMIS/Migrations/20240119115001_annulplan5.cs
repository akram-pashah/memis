using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MEMIS.Migrations
{
    public partial class annulplan5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "intDept",
                table: "AnnualImplemtationPlan",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_AnnualImplemtationPlan_intDept",
                table: "AnnualImplemtationPlan",
                column: "intDept");

            migrationBuilder.AddForeignKey(
                name: "FK_AnnualImplemtationPlan_Departments_intDept",
                table: "AnnualImplemtationPlan",
                column: "intDept",
                principalTable: "Departments",
                principalColumn: "intDept",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnnualImplemtationPlan_Departments_intDept",
                table: "AnnualImplemtationPlan");

            migrationBuilder.DropIndex(
                name: "IX_AnnualImplemtationPlan_intDept",
                table: "AnnualImplemtationPlan");

            migrationBuilder.DropColumn(
                name: "intDept",
                table: "AnnualImplemtationPlan");
        }
    }
}
