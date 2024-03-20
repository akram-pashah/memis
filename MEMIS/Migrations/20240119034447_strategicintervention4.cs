using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MEMIS.Migrations
{
    public partial class strategicintervention4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StrategicAction_StrategicAction_intIntervention",
                table: "StrategicAction");

            migrationBuilder.AddForeignKey(
                name: "FK_StrategicAction_StrategicIntervention_intIntervention",
                table: "StrategicAction",
                column: "intIntervention",
                principalTable: "StrategicIntervention",
                principalColumn: "intIntervention");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StrategicAction_StrategicIntervention_intIntervention",
                table: "StrategicAction");

            migrationBuilder.AddForeignKey(
                name: "FK_StrategicAction_StrategicAction_intIntervention",
                table: "StrategicAction",
                column: "intIntervention",
                principalTable: "StrategicAction",
                principalColumn: "intAction");
        }
    }
}
