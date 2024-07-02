using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MEMIS.Migrations
{
    /// <inheritdoc />
    public partial class QPForeignKeyNUll : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuaterlyPlans_ActivityAssess_ActivityAccessId",
                table: "QuaterlyPlans");

            migrationBuilder.DropForeignKey(
                name: "FK_QuaterlyPlans_DeptPlan_DeptPlanId",
                table: "QuaterlyPlans");

            migrationBuilder.AlterColumn<int>(
                name: "DeptPlanId",
                table: "QuaterlyPlans",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ActivityAccessId",
                table: "QuaterlyPlans",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_QuaterlyPlans_ActivityAssess_ActivityAccessId",
                table: "QuaterlyPlans",
                column: "ActivityAccessId",
                principalTable: "ActivityAssess",
                principalColumn: "intAssess");

            migrationBuilder.AddForeignKey(
                name: "FK_QuaterlyPlans_DeptPlan_DeptPlanId",
                table: "QuaterlyPlans",
                column: "DeptPlanId",
                principalTable: "DeptPlan",
                principalColumn: "intActivity");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuaterlyPlans_ActivityAssess_ActivityAccessId",
                table: "QuaterlyPlans");

            migrationBuilder.DropForeignKey(
                name: "FK_QuaterlyPlans_DeptPlan_DeptPlanId",
                table: "QuaterlyPlans");

            migrationBuilder.AlterColumn<int>(
                name: "DeptPlanId",
                table: "QuaterlyPlans",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ActivityAccessId",
                table: "QuaterlyPlans",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_QuaterlyPlans_ActivityAssess_ActivityAccessId",
                table: "QuaterlyPlans",
                column: "ActivityAccessId",
                principalTable: "ActivityAssess",
                principalColumn: "intAssess",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuaterlyPlans_DeptPlan_DeptPlanId",
                table: "QuaterlyPlans",
                column: "DeptPlanId",
                principalTable: "DeptPlan",
                principalColumn: "intActivity",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
