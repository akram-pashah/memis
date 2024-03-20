using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MEMIS.Migrations
{
    public partial class DeptPlanDeptColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DepartmentId",
                table: "DeptPlan",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DeptPlan_DepartmentId",
                table: "DeptPlan",
                column: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_DeptPlan_Departments_DepartmentId",
                table: "DeptPlan",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "intDept");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeptPlan_Departments_DepartmentId",
                table: "DeptPlan");

            migrationBuilder.DropIndex(
                name: "IX_DeptPlan_DepartmentId",
                table: "DeptPlan");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "DeptPlan");
        }
    }
}
