using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MEMIS.Migrations
{
    public partial class annulplan4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnnualImplemtationPlan_Departments_DepartmentFkintDept",
                table: "AnnualImplemtationPlan");

            migrationBuilder.DropIndex(
                name: "IX_AnnualImplemtationPlan_DepartmentFkintDept",
                table: "AnnualImplemtationPlan");

            migrationBuilder.DropColumn(
                name: "DepartmentFkintDept",
                table: "AnnualImplemtationPlan");

            migrationBuilder.DropColumn(
                name: "intDept",
                table: "AnnualImplemtationPlan");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DepartmentFkintDept",
                table: "AnnualImplemtationPlan",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "intDept",
                table: "AnnualImplemtationPlan",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_AnnualImplemtationPlan_DepartmentFkintDept",
                table: "AnnualImplemtationPlan",
                column: "DepartmentFkintDept");

            migrationBuilder.AddForeignKey(
                name: "FK_AnnualImplemtationPlan_Departments_DepartmentFkintDept",
                table: "AnnualImplemtationPlan",
                column: "DepartmentFkintDept",
                principalTable: "Departments",
                principalColumn: "intDept");
        }
    }
}
