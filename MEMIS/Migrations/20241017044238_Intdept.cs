using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MEMIS.Migrations
{
    /// <inheritdoc />
    public partial class Intdept : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResponsibleParty",
                table: "KPI");

            migrationBuilder.AddColumn<Guid>(
                name: "intDept",
                table: "KPI",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_KPI_intDept",
                table: "KPI",
                column: "intDept");

            migrationBuilder.AddForeignKey(
                name: "FK_KPI_Departments_intDept",
                table: "KPI",
                column: "intDept",
                principalTable: "Departments",
                principalColumn: "intDept");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KPI_Departments_intDept",
                table: "KPI");

            migrationBuilder.DropIndex(
                name: "IX_KPI_intDept",
                table: "KPI");

            migrationBuilder.DropColumn(
                name: "intDept",
                table: "KPI");

            migrationBuilder.AddColumn<string>(
                name: "ResponsibleParty",
                table: "KPI",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
