using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MEMIS.Migrations
{
    /// <inheritdoc />
    public partial class ActivityAssessmentRegionQP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ActivityAssessmentRegionId",
                table: "QuaterlyPlans",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_QuaterlyPlans_ActivityAssessmentRegionId",
                table: "QuaterlyPlans",
                column: "ActivityAssessmentRegionId");

            migrationBuilder.AddForeignKey(
                name: "FK_QuaterlyPlans_ActivityAssessmentRegion_ActivityAssessmentRegionId",
                table: "QuaterlyPlans",
                column: "ActivityAssessmentRegionId",
                principalTable: "ActivityAssessmentRegion",
                principalColumn: "intRegionAssess");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuaterlyPlans_ActivityAssessmentRegion_ActivityAssessmentRegionId",
                table: "QuaterlyPlans");

            migrationBuilder.DropIndex(
                name: "IX_QuaterlyPlans_ActivityAssessmentRegionId",
                table: "QuaterlyPlans");

            migrationBuilder.DropColumn(
                name: "ActivityAssessmentRegionId",
                table: "QuaterlyPlans");
        }
    }
}
