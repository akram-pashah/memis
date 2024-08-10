using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MEMIS.Migrations
{
    /// <inheritdoc />
    public partial class AssessmentRegionChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "intAssessment",
                table: "ActivityAssessmentRegion",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "actType",
                table: "ActivityAssessment",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ActivityAssessmentRegion_intAssessment",
                table: "ActivityAssessmentRegion",
                column: "intAssessment");

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityAssessmentRegion_ActivityAssessment_intAssessment",
                table: "ActivityAssessmentRegion",
                column: "intAssessment",
                principalTable: "ActivityAssessment",
                principalColumn: "intDeptPlan",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActivityAssessmentRegion_ActivityAssessment_intAssessment",
                table: "ActivityAssessmentRegion");

            migrationBuilder.DropIndex(
                name: "IX_ActivityAssessmentRegion_intAssessment",
                table: "ActivityAssessmentRegion");

            migrationBuilder.DropColumn(
                name: "intAssessment",
                table: "ActivityAssessmentRegion");

            migrationBuilder.DropColumn(
                name: "actType",
                table: "ActivityAssessment");
        }
    }
}
