using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MEMIS.Migrations
{
    /// <inheritdoc />
    public partial class QuarterlyRiskActionImpStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "QuarterlyRiskActions");

            migrationBuilder.AddColumn<int>(
                name: "ImpStatusId",
                table: "QuarterlyRiskActions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_QuarterlyRiskActions_ImpStatusId",
                table: "QuarterlyRiskActions",
                column: "ImpStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_QuarterlyRiskActions_ImplementationStatus_ImpStatusId",
                table: "QuarterlyRiskActions",
                column: "ImpStatusId",
                principalTable: "ImplementationStatus",
                principalColumn: "ImpStatusId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuarterlyRiskActions_ImplementationStatus_ImpStatusId",
                table: "QuarterlyRiskActions");

            migrationBuilder.DropIndex(
                name: "IX_QuarterlyRiskActions_ImpStatusId",
                table: "QuarterlyRiskActions");

            migrationBuilder.DropColumn(
                name: "ImpStatusId",
                table: "QuarterlyRiskActions");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "QuarterlyRiskActions",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
