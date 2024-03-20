using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MEMIS.Migrations
{
    public partial class misc_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_T_RISKIDENTIFICATION_T_STRATEGICOBJECTIVE_intStrategicObjective",
                table: "T_RISKIDENTIFICATION");

            migrationBuilder.DropTable(
                name: "T_IMPLEMENTATIONPLAN");

            migrationBuilder.DropTable(
                name: "T_STRATEGICOBJECTIVE");

            migrationBuilder.CreateTable(
                name: "StrategicObjective",
                columns: table => new
                {
                    intObjective = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ObjectiveCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    ObjectiveName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    intFocus = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StrategicObjective", x => x.intObjective);
                    table.ForeignKey(
                        name: "FK_StrategicObjective_FocusArea_intFocus",
                        column: x => x.intFocus,
                        principalTable: "FocusArea",
                        principalColumn: "intFocus");
                });

            migrationBuilder.CreateIndex(
                name: "IX_StrategicObjective_intFocus",
                table: "StrategicObjective",
                column: "intFocus");

            migrationBuilder.AddForeignKey(
                name: "FK_T_RISKIDENTIFICATION_StrategicObjective_intStrategicObjective",
                table: "T_RISKIDENTIFICATION",
                column: "intStrategicObjective",
                principalTable: "StrategicObjective",
                principalColumn: "intObjective");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_T_RISKIDENTIFICATION_StrategicObjective_intStrategicObjective",
                table: "T_RISKIDENTIFICATION");

            migrationBuilder.DropTable(
                name: "StrategicObjective");

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
                name: "T_STRATEGICOBJECTIVE",
                columns: table => new
                {
                    intStrategicObjective = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StrategicObjectiveCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StrategicObjectiveName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_STRATEGICOBJECTIVE", x => x.intStrategicObjective);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_T_RISKIDENTIFICATION_T_STRATEGICOBJECTIVE_intStrategicObjective",
                table: "T_RISKIDENTIFICATION",
                column: "intStrategicObjective",
                principalTable: "T_STRATEGICOBJECTIVE",
                principalColumn: "intStrategicObjective");
        }
    }
}
