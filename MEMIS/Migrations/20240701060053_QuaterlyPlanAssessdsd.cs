using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MEMIS.Migrations
{
    /// <inheritdoc />
    public partial class QuaterlyPlanAssessdsd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DeptPlanId",
                table: "QuaterlyPlans",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "QActual",
                table: "QuaterlyPlans",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "QAmtSpent",
                table: "QuaterlyPlans",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "QJustification",
                table: "QuaterlyPlans",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_QuaterlyPlans_DeptPlanId",
                table: "QuaterlyPlans",
                column: "DeptPlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_QuaterlyPlans_DeptPlan_DeptPlanId",
                table: "QuaterlyPlans",
                column: "DeptPlanId",
                principalTable: "DeptPlan",
                principalColumn: "intActivity",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuaterlyPlans_DeptPlan_DeptPlanId",
                table: "QuaterlyPlans");

            migrationBuilder.DropIndex(
                name: "IX_QuaterlyPlans_DeptPlanId",
                table: "QuaterlyPlans");

            migrationBuilder.DropColumn(
                name: "DeptPlanId",
                table: "QuaterlyPlans");

            migrationBuilder.DropColumn(
                name: "QActual",
                table: "QuaterlyPlans");

            migrationBuilder.DropColumn(
                name: "QAmtSpent",
                table: "QuaterlyPlans");

            migrationBuilder.DropColumn(
                name: "QJustification",
                table: "QuaterlyPlans");
        }
    }
}
