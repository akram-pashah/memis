using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MEMIS.Migrations
{
    /// <inheritdoc />
    public partial class risktitle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Additional_Mitigation",
                table: "RiskIdentification");

            migrationBuilder.DropColumn(
                name: "ExistingMitigation",
                table: "RiskIdentification");

            migrationBuilder.DropColumn(
                name: "Opportunity",
                table: "RiskIdentification");

            migrationBuilder.DropColumn(
                name: "Weakness",
                table: "RiskIdentification");

            migrationBuilder.AddColumn<string>(
                name: "RiskTitle",
                table: "RiskIdentification",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RiskTitle",
                table: "RiskIdentification");

            migrationBuilder.AddColumn<string>(
                name: "Additional_Mitigation",
                table: "RiskIdentification",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExistingMitigation",
                table: "RiskIdentification",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Opportunity",
                table: "RiskIdentification",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Weakness",
                table: "RiskIdentification",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
