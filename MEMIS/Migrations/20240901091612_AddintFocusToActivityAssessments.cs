using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MEMIS.Migrations
{
    /// <inheritdoc />
    public partial class AddintFocusToActivityAssessments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FYEAR",
                table: "ActivityAssessment",
                newName: "Fyear");

            migrationBuilder.AlterColumn<int>(
                name: "Fyear",
                table: "ActivityAssessment",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "intFocus",
                table: "ActivityAssessment",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ActivityAssessment_intFocus",
                table: "ActivityAssessment",
                column: "intFocus");

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityAssessment_FocusArea_intFocus",
                table: "ActivityAssessment",
                column: "intFocus",
                principalTable: "FocusArea",
                principalColumn: "intFocus");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActivityAssessment_FocusArea_intFocus",
                table: "ActivityAssessment");

            migrationBuilder.DropIndex(
                name: "IX_ActivityAssessment_intFocus",
                table: "ActivityAssessment");

            migrationBuilder.DropColumn(
                name: "intFocus",
                table: "ActivityAssessment");

            migrationBuilder.RenameColumn(
                name: "Fyear",
                table: "ActivityAssessment",
                newName: "FYEAR");

            migrationBuilder.AlterColumn<int>(
                name: "FYEAR",
                table: "ActivityAssessment",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
