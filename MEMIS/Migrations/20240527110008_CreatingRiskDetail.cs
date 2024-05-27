using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MEMIS.Migrations
{
    /// <inheritdoc />
    public partial class CreatingRiskDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Events",
                table: "RiskIdentification");

            migrationBuilder.DropColumn(
                name: "RiskCause",
                table: "RiskIdentification");

            migrationBuilder.DropColumn(
                name: "RiskConsequence",
                table: "RiskIdentification");

            migrationBuilder.DropColumn(
                name: "RiskSource",
                table: "RiskIdentification");

            migrationBuilder.CreateTable(
                name: "RiskDetails",
                columns: table => new
                {
                    RiskDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RiskId = table.Column<int>(type: "int", nullable: false),
                    Event = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RiskSource = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RiskCause = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RiskConsequence = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiskDetails", x => x.RiskDetailId);
                    table.ForeignKey(
                        name: "FK_RiskDetails_RiskIdentification_RiskId",
                        column: x => x.RiskId,
                        principalTable: "RiskIdentification",
                        principalColumn: "RiskId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RiskDetails_RiskId",
                table: "RiskDetails",
                column: "RiskId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RiskDetails");

            migrationBuilder.AddColumn<string>(
                name: "Events",
                table: "RiskIdentification",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RiskCause",
                table: "RiskIdentification",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RiskConsequence",
                table: "RiskIdentification",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RiskSource",
                table: "RiskIdentification",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
