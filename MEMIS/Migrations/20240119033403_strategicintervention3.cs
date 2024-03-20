using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MEMIS.Migrations
{
    public partial class strategicintervention3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StrategicAction",
                columns: table => new
                {
                    intAction = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    actionCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    actionName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    intIntervention = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StrategicAction", x => x.intAction);
                    table.ForeignKey(
                        name: "FK_StrategicAction_StrategicAction_intIntervention",
                        column: x => x.intIntervention,
                        principalTable: "StrategicAction",
                        principalColumn: "intAction");
                });

            migrationBuilder.CreateIndex(
                name: "IX_StrategicAction_intIntervention",
                table: "StrategicAction",
                column: "intIntervention");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StrategicAction");
        }
    }
}
