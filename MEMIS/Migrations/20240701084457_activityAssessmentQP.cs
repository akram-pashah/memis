using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MEMIS.Migrations
{
    /// <inheritdoc />
    public partial class activityAssessmentQP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ActivityAssessmentId",
                table: "QuaterlyPlans",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_QuaterlyPlans_ActivityAssessmentId",
                table: "QuaterlyPlans",
                column: "ActivityAssessmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_QuaterlyPlans_ActivityAssessment_ActivityAssessmentId",
                table: "QuaterlyPlans",
                column: "ActivityAssessmentId",
                principalTable: "ActivityAssessment",
                principalColumn: "intDeptPlan");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuaterlyPlans_ActivityAssessment_ActivityAssessmentId",
                table: "QuaterlyPlans");

            migrationBuilder.DropIndex(
                name: "IX_QuaterlyPlans_ActivityAssessmentId",
                table: "QuaterlyPlans");

            migrationBuilder.DropColumn(
                name: "ActivityAssessmentId",
                table: "QuaterlyPlans");
        }
    }
}
