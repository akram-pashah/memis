using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MEMIS.Migrations
{
    /// <inheritdoc />
    public partial class AddingIdentifiedRiskstoActivityAssess : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IdentifiedRisks",
                table: "ActivityAssess",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdentifiedRisks",
                table: "ActivityAssess");
        }
    }
}
