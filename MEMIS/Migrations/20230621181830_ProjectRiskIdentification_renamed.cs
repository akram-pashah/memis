using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MEMIS.Migrations
{
    public partial class ProjectRiskIdentification_renamed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectRistIdentification");

            migrationBuilder.CreateTable(
                name: "ProjectRiskIdentification",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Risk = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rank = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ProjectInitiationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectRiskIdentification", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectRiskIdentification_ProjectInitiation_ProjectInitiationId",
                        column: x => x.ProjectInitiationId,
                        principalTable: "ProjectInitiation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectRiskIdentification_ProjectInitiationId",
                table: "ProjectRiskIdentification",
                column: "ProjectInitiationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectRiskIdentification");

            migrationBuilder.CreateTable(
                name: "ProjectRistIdentification",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectInitiationId = table.Column<int>(type: "int", nullable: false),
                    Rank = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Risk = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectRistIdentification", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectRistIdentification_ProjectInitiation_ProjectInitiationId",
                        column: x => x.ProjectInitiationId,
                        principalTable: "ProjectInitiation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectRistIdentification_ProjectInitiationId",
                table: "ProjectRistIdentification",
                column: "ProjectInitiationId");
        }
    }
}
