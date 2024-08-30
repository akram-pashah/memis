using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MEMIS.Migrations
{
    /// <inheritdoc />
    public partial class SDTandRiskChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RiskSource",
                table: "RiskRegister",
                newName: "Weakness");

            migrationBuilder.RenameColumn(
                name: "RiskConsequence",
                table: "RiskRegister",
                newName: "Supporting_Owners");

            migrationBuilder.RenameColumn(
                name: "RiskCause",
                table: "RiskRegister",
                newName: "ExistingMitigation");

            migrationBuilder.RenameColumn(
                name: "Events",
                table: "RiskRegister",
                newName: "Additional_Mitigation");

            migrationBuilder.AddColumn<string>(
                name: "PropotionWithinTimeline",
                table: "SDTMaster",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "intCategory",
                table: "RiskRegister",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "intDept",
                table: "RiskRegister",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RiskRegister_intCategory",
                table: "RiskRegister",
                column: "intCategory");

            migrationBuilder.CreateIndex(
                name: "IX_RiskRegister_intDept",
                table: "RiskRegister",
                column: "intDept");

            migrationBuilder.AddForeignKey(
                name: "FK_RiskRegister_Departments_intDept",
                table: "RiskRegister",
                column: "intDept",
                principalTable: "Departments",
                principalColumn: "intDept");

         
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RiskRegister_Departments_intDept",
                table: "RiskRegister");

            

            migrationBuilder.DropIndex(
                name: "IX_RiskRegister_intCategory",
                table: "RiskRegister");

            migrationBuilder.DropIndex(
                name: "IX_RiskRegister_intDept",
                table: "RiskRegister");

            migrationBuilder.DropColumn(
                name: "PropotionWithinTimeline",
                table: "SDTMaster");

            migrationBuilder.DropColumn(
                name: "intCategory",
                table: "RiskRegister");

            migrationBuilder.DropColumn(
                name: "intDept",
                table: "RiskRegister");

            migrationBuilder.RenameColumn(
                name: "Weakness",
                table: "RiskRegister",
                newName: "RiskSource");

            migrationBuilder.RenameColumn(
                name: "Supporting_Owners",
                table: "RiskRegister",
                newName: "RiskConsequence");

            migrationBuilder.RenameColumn(
                name: "ExistingMitigation",
                table: "RiskRegister",
                newName: "RiskCause");

            migrationBuilder.RenameColumn(
                name: "Additional_Mitigation",
                table: "RiskRegister",
                newName: "Events");
        }
    }
}
