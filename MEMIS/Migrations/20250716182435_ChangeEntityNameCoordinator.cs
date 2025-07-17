using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MEMIS.Migrations
{
    /// <inheritdoc />
    public partial class ChangeEntityNameCoordinator : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Region_AspNetUsers_RegCoordinatorId",
                table: "Region");

            migrationBuilder.RenameColumn(
                name: "RegCoordinatorId",
                table: "Region",
                newName: "CoordinatorId");

            migrationBuilder.RenameIndex(
                name: "IX_Region_RegCoordinatorId",
                table: "Region",
                newName: "IX_Region_CoordinatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Region_AspNetUsers_CoordinatorId",
                table: "Region",
                column: "CoordinatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Region_AspNetUsers_CoordinatorId",
                table: "Region");

            migrationBuilder.RenameColumn(
                name: "CoordinatorId",
                table: "Region",
                newName: "RegCoordinatorId");

            migrationBuilder.RenameIndex(
                name: "IX_Region_CoordinatorId",
                table: "Region",
                newName: "IX_Region_RegCoordinatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Region_AspNetUsers_RegCoordinatorId",
                table: "Region",
                column: "RegCoordinatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
