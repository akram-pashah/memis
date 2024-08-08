using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MEMIS.Migrations
{
    /// <inheritdoc />
    public partial class AddDeptId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "intDept",
                table: "ActivityAssessment",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ActivityAssessment_intDept",
                table: "ActivityAssessment",
                column: "intDept");

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityAssessment_Departments_intDept",
                table: "ActivityAssessment",
                column: "intDept",
                principalTable: "Departments",
                principalColumn: "intDept");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActivityAssessment_Departments_intDept",
                table: "ActivityAssessment");

            migrationBuilder.DropIndex(
                name: "IX_ActivityAssessment_intDept",
                table: "ActivityAssessment");

            migrationBuilder.DropColumn(
                name: "intDept",
                table: "ActivityAssessment");
        }
    }
}
