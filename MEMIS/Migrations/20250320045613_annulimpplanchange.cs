using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MEMIS.Migrations
{
    /// <inheritdoc />
    public partial class annulimpplanchange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "budgetAmount",
                table: "AnnualImplemtationPlan",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "budgetCode",
                table: "AnnualImplemtationPlan",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "intyear",
                table: "AnnualImplemtationPlan",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AnnualImplemtationPlan_intyear",
                table: "AnnualImplemtationPlan",
                column: "intyear");

            migrationBuilder.AddForeignKey(
                name: "FK_AnnualImplemtationPlan_M_FYEAR_intyear",
                table: "AnnualImplemtationPlan",
                column: "intyear",
                principalTable: "M_FYEAR",
                principalColumn: "intyear");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnnualImplemtationPlan_M_FYEAR_intyear",
                table: "AnnualImplemtationPlan");

            migrationBuilder.DropIndex(
                name: "IX_AnnualImplemtationPlan_intyear",
                table: "AnnualImplemtationPlan");

            migrationBuilder.DropColumn(
                name: "budgetAmount",
                table: "AnnualImplemtationPlan");

            migrationBuilder.DropColumn(
                name: "budgetCode",
                table: "AnnualImplemtationPlan");

            migrationBuilder.DropColumn(
                name: "intyear",
                table: "AnnualImplemtationPlan");
        }
    }
}
