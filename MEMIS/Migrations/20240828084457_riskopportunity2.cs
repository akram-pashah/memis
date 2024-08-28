using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MEMIS.Migrations
{
    /// <inheritdoc />
    public partial class riskopportunity2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "intDept",
                table: "RiskIdentification",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RiskIdentification_intDept",
                table: "RiskIdentification",
                column: "intDept");

            migrationBuilder.AddForeignKey(
                name: "FK_RiskIdentification_Departments_intDept",
                table: "RiskIdentification",
                column: "intDept",
                principalTable: "Departments",
                principalColumn: "intDept");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RiskIdentification_Departments_intDept",
                table: "RiskIdentification");

            migrationBuilder.DropIndex(
                name: "IX_RiskIdentification_intDept",
                table: "RiskIdentification");

            migrationBuilder.DropColumn(
                name: "intDept",
                table: "RiskIdentification");
        }
    }
}
