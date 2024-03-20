using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MEMIS.Migrations
{
    public partial class RiskRegister : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RiskRegister",
                columns: table => new
                {
                    RiskRefID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdentifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StrategicObjective = table.Column<int>(type: "int", nullable: true),
                    FocusArea = table.Column<int>(type: "int", nullable: true),
                    Activity = table.Column<int>(type: "int", nullable: false),
                    BudgetCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RiskDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Events = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RiskSource = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RiskCause = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RiskConsequence = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RiskOwner = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RiskConsequenceId = table.Column<int>(type: "int", nullable: false),
                    RiskLikelihoodId = table.Column<int>(type: "int", nullable: false),
                    RiskScore = table.Column<int>(type: "int", nullable: false),
                    RiskRank = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EvalCriteria = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RiskId = table.Column<int>(type: "int", nullable: true),
                    AdditionalMitigation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Opportunity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReviewDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApprStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiskRegister", x => x.RiskRefID);
                    table.ForeignKey(
                        name: "FK_RiskRegister_DepartmentPlan_Activity",
                        column: x => x.Activity,
                        principalTable: "DepartmentPlan",
                        principalColumn: "intDeptPlan",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RiskRegister_RiskIdentification_RiskId",
                        column: x => x.RiskId,
                        principalTable: "RiskIdentification",
                        principalColumn: "RiskId");
                    table.ForeignKey(
                        name: "FK_RiskRegister_StrategicPlan_FocusArea",
                        column: x => x.FocusArea,
                        principalTable: "StrategicPlan",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RiskRegister_StrategicPlan_StrategicObjective",
                        column: x => x.StrategicObjective,
                        principalTable: "StrategicPlan",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_RiskRegister_Activity",
                table: "RiskRegister",
                column: "Activity");

            migrationBuilder.CreateIndex(
                name: "IX_RiskRegister_FocusArea",
                table: "RiskRegister",
                column: "FocusArea");

            migrationBuilder.CreateIndex(
                name: "IX_RiskRegister_RiskId",
                table: "RiskRegister",
                column: "RiskId");

            migrationBuilder.CreateIndex(
                name: "IX_RiskRegister_StrategicObjective",
                table: "RiskRegister",
                column: "StrategicObjective");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RiskRegister");
        }
    }
}
