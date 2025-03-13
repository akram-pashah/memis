using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MEMIS.Migrations
{
    /// <inheritdoc />
    public partial class annualimpplan_deptdir : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "intDept",
                table: "ProgramImplementationPlan",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "intDir",
                table: "ProgramImplementationPlan",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProgramImplementationPlan_intDept",
                table: "ProgramImplementationPlan",
                column: "intDept");

            migrationBuilder.CreateIndex(
                name: "IX_ProgramImplementationPlan_intDir",
                table: "ProgramImplementationPlan",
                column: "intDir");

            migrationBuilder.AddForeignKey(
                name: "FK_ProgramImplementationPlan_Departments_intDept",
                table: "ProgramImplementationPlan",
                column: "intDept",
                principalTable: "Departments",
                principalColumn: "intDept");

            migrationBuilder.AddForeignKey(
                name: "FK_ProgramImplementationPlan_Directorates_intDir",
                table: "ProgramImplementationPlan",
                column: "intDir",
                principalTable: "Directorates",
                principalColumn: "intDir");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProgramImplementationPlan_Departments_intDept",
                table: "ProgramImplementationPlan");

            migrationBuilder.DropForeignKey(
                name: "FK_ProgramImplementationPlan_Directorates_intDir",
                table: "ProgramImplementationPlan");

            migrationBuilder.DropIndex(
                name: "IX_ProgramImplementationPlan_intDept",
                table: "ProgramImplementationPlan");

            migrationBuilder.DropIndex(
                name: "IX_ProgramImplementationPlan_intDir",
                table: "ProgramImplementationPlan");

            migrationBuilder.DropColumn(
                name: "intDept",
                table: "ProgramImplementationPlan");

            migrationBuilder.DropColumn(
                name: "intDir",
                table: "ProgramImplementationPlan");
        }
    }
}
