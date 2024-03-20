using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MEMIS.Migrations
{
    public partial class strategicintervention : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StrategicIntervention",
                columns: table => new
                {
                    intIntervention = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InterventionCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    InterventionName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    intObjective = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StrategicIntervention", x => x.intIntervention);
                    table.ForeignKey(
                        name: "FK_StrategicIntervention_StrategicObjective_intObjective",
                        column: x => x.intObjective,
                        principalTable: "StrategicObjective",
                        principalColumn: "intObjective");
                });

            migrationBuilder.CreateIndex(
                name: "IX_StrategicIntervention_intObjective",
                table: "StrategicIntervention",
                column: "intObjective");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StrategicIntervention");
        }
    }
}
