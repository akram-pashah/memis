using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MEMIS.Migrations
{
    /// <inheritdoc />
    public partial class ChangeEntityNameHead : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Region_AspNetUsers_IntHeadId",
                table: "Region");

            migrationBuilder.RenameColumn(
                name: "IntHeadId",
                table: "Region",
                newName: "HeadId");

            migrationBuilder.RenameIndex(
                name: "IX_Region_IntHeadId",
                table: "Region",
                newName: "IX_Region_HeadId");

            migrationBuilder.AddForeignKey(
                name: "FK_Region_AspNetUsers_HeadId",
                table: "Region",
                column: "HeadId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Region_AspNetUsers_HeadId",
                table: "Region");

            migrationBuilder.RenameColumn(
                name: "HeadId",
                table: "Region",
                newName: "IntHeadId");

            migrationBuilder.RenameIndex(
                name: "IX_Region_HeadId",
                table: "Region",
                newName: "IX_Region_IntHeadId");

            migrationBuilder.AddForeignKey(
                name: "FK_Region_AspNetUsers_IntHeadId",
                table: "Region",
                column: "IntHeadId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
