using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MEMIS.Migrations
{
    public partial class kpiassessment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "IdentifiedDate",
                table: "T_RISKIDENTIFICATION",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.CreateTable(
                name: "KPI_Assessment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    KPIMasterId = table.Column<int>(type: "int", nullable: true),
                    AssessmentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PerformanceIndicator = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FrequencyofReporting = table.Column<int>(type: "int", nullable: false),
                    IndicatorFormulae = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IndicatorDefinition = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FY = table.Column<int>(type: "int", nullable: true),
                    Target = table.Column<double>(type: "float", nullable: true),
                    Numerator = table.Column<double>(type: "float", nullable: true),
                    Denominator = table.Column<double>(type: "float", nullable: true),
                    Rate = table.Column<double>(type: "float", nullable: true),
                    Achieved = table.Column<double>(type: "float", nullable: true),
                    ApprovalStatus = table.Column<int>(type: "int", nullable: true),
                    Justification = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KPI_Assessment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KPI_Assessment_KPI_Id",
                        column: x => x.Id,
                        principalTable: "KPI",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KPI_Assessment");

            migrationBuilder.AlterColumn<DateTime>(
                name: "IdentifiedDate",
                table: "T_RISKIDENTIFICATION",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);
        }
    }
}
