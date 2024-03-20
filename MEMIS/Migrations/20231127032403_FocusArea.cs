using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MEMIS.Migrations
{
    public partial class FocusArea : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "T_FOCUSAREA",
                columns: table => new
                {
                    intFocusArea = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FocusAreaCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FocusAreaName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_FOCUSAREA", x => x.intFocusArea);
                });

            migrationBuilder.CreateTable(
                name: "T_IMPLEMENTATIONPLAN",
                columns: table => new
                {
                    intActivity = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActivityCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ActivityName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_IMPLEMENTATIONPLAN", x => x.intActivity);
                });

            migrationBuilder.CreateTable(
                name: "T_RISKIDENTIFICATION",
                columns: table => new
                {
                    intRisk = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdentifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    intStrategicObjective = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_RISKIDENTIFICATION", x => x.intRisk);
                    table.ForeignKey(
                        name: "FK_T_RISKIDENTIFICATION_T_STRATEGICOBJECTIVE_intStrategicObjective",
                        column: x => x.intStrategicObjective,
                        principalTable: "T_STRATEGICOBJECTIVE",
                        principalColumn: "intStrategicObjective");
                });

            migrationBuilder.CreateIndex(
                name: "IX_T_RISKIDENTIFICATION_intStrategicObjective",
                table: "T_RISKIDENTIFICATION",
                column: "intStrategicObjective");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "T_FOCUSAREA");

            migrationBuilder.DropTable(
                name: "T_IMPLEMENTATIONPLAN");

            migrationBuilder.DropTable(
                name: "T_RISKIDENTIFICATION");
        }
    }
}
