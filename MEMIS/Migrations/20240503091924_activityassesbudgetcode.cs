using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MEMIS.Migrations
{
    /// <inheritdoc />
    public partial class activityassesbudgetcode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ActivityAssess",
                columns: table => new
                {
                    intAssess = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    intIntervention = table.Column<int>(type: "int", nullable: true),
                    intAction = table.Column<int>(type: "int", nullable: true),
                    intActivity = table.Column<int>(type: "int", nullable: true),
                    outputIndicator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    baseline = table.Column<double>(type: "float", nullable: true),
                    budgetCode = table.Column<double>(type: "float", nullable: true),
                    comparativeTarget = table.Column<double>(type: "float", nullable: true),
                    justification = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    budgetAmount = table.Column<double>(type: "float", nullable: true),
                    Quarter = table.Column<int>(type: "int", nullable: true),
                    QTarget = table.Column<double>(type: "float", nullable: true),
                    QBudget = table.Column<double>(type: "float", nullable: true),
                    ApprStatus = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityAssess", x => x.intAssess);
                    table.ForeignKey(
                        name: "FK_ActivityAssess_Activity_intActivity",
                        column: x => x.intActivity,
                        principalTable: "Activity",
                        principalColumn: "intActivity");
                    table.ForeignKey(
                        name: "FK_ActivityAssess_StrategicAction_intAction",
                        column: x => x.intAction,
                        principalTable: "StrategicAction",
                        principalColumn: "intAction");
                    table.ForeignKey(
                        name: "FK_ActivityAssess_StrategicIntervention_intIntervention",
                        column: x => x.intIntervention,
                        principalTable: "StrategicIntervention",
                        principalColumn: "intIntervention");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActivityAssess_intAction",
                table: "ActivityAssess",
                column: "intAction");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityAssess_intActivity",
                table: "ActivityAssess",
                column: "intActivity");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityAssess_intIntervention",
                table: "ActivityAssess",
                column: "intIntervention");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActivityAssess");
        }
    }
}
