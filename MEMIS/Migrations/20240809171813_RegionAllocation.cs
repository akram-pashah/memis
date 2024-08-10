using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MEMIS.Migrations
{
    /// <inheritdoc />
    public partial class RegionAllocation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ActivityAssessRegionId",
                table: "QuaterlyPlans",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_QuaterlyPlans_ActivityAssessRegionId",
                table: "QuaterlyPlans",
                column: "ActivityAssessRegionId");

            migrationBuilder.AddForeignKey(
                name: "FK_QuaterlyPlans_ActivityAssessRegion_ActivityAssessRegionId",
                table: "QuaterlyPlans",
                column: "ActivityAssessRegionId",
                principalTable: "ActivityAssessRegion",
                principalColumn: "intRegionAssess");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuaterlyPlans_ActivityAssessRegion_ActivityAssessRegionId",
                table: "QuaterlyPlans");

            migrationBuilder.DropIndex(
                name: "IX_QuaterlyPlans_ActivityAssessRegionId",
                table: "QuaterlyPlans");

            migrationBuilder.DropColumn(
                name: "ActivityAssessRegionId",
                table: "QuaterlyPlans");
        }
    }
}
