using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MEMIS.Migrations
{
    public partial class DeptPlan : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
             

            migrationBuilder.CreateTable(
                name: "DeptPlan",
                columns: table => new
                {
                    intActivity = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StrategicObjective = table.Column<int>(type: "int", nullable: true),
                    strategicIntervention = table.Column<int>(type: "int", nullable: true),
                    StrategicAction = table.Column<int>(type: "int", nullable: true),
                    activity = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    outputIndicator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    baseline = table.Column<double>(type: "float", nullable: true),
                    budgetCode = table.Column<double>(type: "float", nullable: true),
                    unitCost = table.Column<double>(type: "float", nullable: true),
                    Q1Target = table.Column<double>(type: "float", nullable: true),
                    Q1Budget = table.Column<double>(type: "float", nullable: true),
                    Q2Target = table.Column<double>(type: "float", nullable: true),
                    Q2Budget = table.Column<double>(type: "float", nullable: true),
                    Q3Target = table.Column<double>(type: "float", nullable: true),
                    Q3Budget = table.Column<double>(type: "float", nullable: true),
                    Q4Target = table.Column<double>(type: "float", nullable: true),
                    Q4Budget = table.Column<double>(type: "float", nullable: true),
                    comparativeTarget = table.Column<double>(type: "float", nullable: true),
                    justification = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    budgetAmount = table.Column<double>(type: "float", nullable: true),
                    IsVerified = table.Column<bool>(type: "bit", nullable: false),
                    ApprStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeptPlan", x => x.intActivity);
                    table.ForeignKey(
                        name: "FK_DeptPlan_StrategicPlan_StrategicAction",
                        column: x => x.StrategicAction,
                        principalTable: "StrategicPlan",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DeptPlan_StrategicPlan_strategicIntervention",
                        column: x => x.strategicIntervention,
                        principalTable: "StrategicPlan",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DeptPlan_StrategicPlan_StrategicObjective",
                        column: x => x.StrategicObjective,
                        principalTable: "StrategicPlan",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DeptPlan_StrategicAction",
                table: "DeptPlan",
                column: "StrategicAction");

            migrationBuilder.CreateIndex(
                name: "IX_DeptPlan_strategicIntervention",
                table: "DeptPlan",
                column: "strategicIntervention");

            migrationBuilder.CreateIndex(
                name: "IX_DeptPlan_StrategicObjective",
                table: "DeptPlan",
                column: "StrategicObjective");

            
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
             
        }
    }
}
