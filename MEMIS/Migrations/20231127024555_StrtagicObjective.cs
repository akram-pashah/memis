using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MEMIS.Migrations
{
    public partial class StrtagicObjective : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "T_STRATEGICOBJECTIVE");
        }
    }
}
