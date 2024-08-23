using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MEMIS.Migrations
{
    /// <inheritdoc />
    public partial class ProgramImplementationPlanChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Baseline",
                table: "ProgramImplementationPlan");

            migrationBuilder.DropColumn(
                name: "DateofAction",
                table: "ProgramImplementationPlan");

            migrationBuilder.RenameColumn(
                name: "Activity",
                table: "ProgramImplementationPlan",
                newName: "OutputTarget");

            migrationBuilder.AddColumn<string>(
                name: "MeansofVerification",
                table: "ProgramImplementationPlan",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "intAction",
                table: "ProgramImplementationPlan",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "intActivity",
                table: "ProgramImplementationPlan",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "intIntervention",
                table: "ProgramImplementationPlan",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "intObjective",
                table: "ProgramImplementationPlan",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProgramImplementationPlan_intAction",
                table: "ProgramImplementationPlan",
                column: "intAction");

            migrationBuilder.CreateIndex(
                name: "IX_ProgramImplementationPlan_intActivity",
                table: "ProgramImplementationPlan",
                column: "intActivity");

            migrationBuilder.CreateIndex(
                name: "IX_ProgramImplementationPlan_intIntervention",
                table: "ProgramImplementationPlan",
                column: "intIntervention");

            migrationBuilder.CreateIndex(
                name: "IX_ProgramImplementationPlan_intObjective",
                table: "ProgramImplementationPlan",
                column: "intObjective");

            migrationBuilder.AddForeignKey(
                name: "FK_ProgramImplementationPlan_Activity_intActivity",
                table: "ProgramImplementationPlan",
                column: "intActivity",
                principalTable: "Activity",
                principalColumn: "intActivity");

            migrationBuilder.AddForeignKey(
                name: "FK_ProgramImplementationPlan_StrategicAction_intAction",
                table: "ProgramImplementationPlan",
                column: "intAction",
                principalTable: "StrategicAction",
                principalColumn: "intAction");

            migrationBuilder.AddForeignKey(
                name: "FK_ProgramImplementationPlan_StrategicIntervention_intIntervention",
                table: "ProgramImplementationPlan",
                column: "intIntervention",
                principalTable: "StrategicIntervention",
                principalColumn: "intIntervention");

            migrationBuilder.AddForeignKey(
                name: "FK_ProgramImplementationPlan_StrategicObjective_intObjective",
                table: "ProgramImplementationPlan",
                column: "intObjective",
                principalTable: "StrategicObjective",
                principalColumn: "intObjective");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProgramImplementationPlan_Activity_intActivity",
                table: "ProgramImplementationPlan");

            migrationBuilder.DropForeignKey(
                name: "FK_ProgramImplementationPlan_StrategicAction_intAction",
                table: "ProgramImplementationPlan");

            migrationBuilder.DropForeignKey(
                name: "FK_ProgramImplementationPlan_StrategicIntervention_intIntervention",
                table: "ProgramImplementationPlan");

            migrationBuilder.DropForeignKey(
                name: "FK_ProgramImplementationPlan_StrategicObjective_intObjective",
                table: "ProgramImplementationPlan");

            migrationBuilder.DropIndex(
                name: "IX_ProgramImplementationPlan_intAction",
                table: "ProgramImplementationPlan");

            migrationBuilder.DropIndex(
                name: "IX_ProgramImplementationPlan_intActivity",
                table: "ProgramImplementationPlan");

            migrationBuilder.DropIndex(
                name: "IX_ProgramImplementationPlan_intIntervention",
                table: "ProgramImplementationPlan");

            migrationBuilder.DropIndex(
                name: "IX_ProgramImplementationPlan_intObjective",
                table: "ProgramImplementationPlan");

            migrationBuilder.DropColumn(
                name: "MeansofVerification",
                table: "ProgramImplementationPlan");

            migrationBuilder.DropColumn(
                name: "intAction",
                table: "ProgramImplementationPlan");

            migrationBuilder.DropColumn(
                name: "intActivity",
                table: "ProgramImplementationPlan");

            migrationBuilder.DropColumn(
                name: "intIntervention",
                table: "ProgramImplementationPlan");

            migrationBuilder.DropColumn(
                name: "intObjective",
                table: "ProgramImplementationPlan");

            migrationBuilder.RenameColumn(
                name: "OutputTarget",
                table: "ProgramImplementationPlan",
                newName: "Activity");

            migrationBuilder.AddColumn<long>(
                name: "Baseline",
                table: "ProgramImplementationPlan",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateofAction",
                table: "ProgramImplementationPlan",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
