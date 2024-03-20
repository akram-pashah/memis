using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MEMIS.Migrations
{
    public partial class KPIMaster : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.CreateTable(
                name: "KPI",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StrategicObjective = table.Column<int>(type: "int", nullable: true),
                    PerformanceIndicator = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeofIndicator = table.Column<int>(type: "int", nullable: true),
                    IndicatorFormulae = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IndicatorDefinition = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OriginalBaseline = table.Column<long>(type: "bigint", nullable: true),
                    Indicatorclassification = table.Column<int>(type: "int", nullable: true),
                    DataType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Unitofmeasure = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FrequencyofReporting = table.Column<int>(type: "int", nullable: true),
                    FY1 = table.Column<double>(type: "float", nullable: true),
                    FY2 = table.Column<double>(type: "float", nullable: true),
                    FY3 = table.Column<double>(type: "float", nullable: true),
                    FY4 = table.Column<double>(type: "float", nullable: true),
                    FY5 = table.Column<double>(type: "float", nullable: true),
                    MeansofVerification = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponsibleParty = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KPI", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KPI_StrategicPlan_StrategicObjective",
                        column: x => x.StrategicObjective,
                        principalTable: "StrategicPlan",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_KPI_StrategicObjective",
                table: "KPI",
                column: "StrategicObjective");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KPI");

           
        }
    }
}
