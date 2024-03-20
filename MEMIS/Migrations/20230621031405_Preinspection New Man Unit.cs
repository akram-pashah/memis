using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MEMIS.Migrations
{
    public partial class PreinspectionNewManUnit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PreinspectionManufacturingUnit",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InspectionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Applicant = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Business = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Road = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Zone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Village = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telephone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GPS = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductClassification = table.Column<int>(type: "int", nullable: false),
                    CategoryOfpremises = table.Column<int>(type: "int", nullable: false),
                    comments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DistrictId = table.Column<int>(type: "int", nullable: false),
                    InspectorId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreinspectionManufacturingUnit", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PreinspectionManufacturingUnit_District_DistrictId",
                        column: x => x.DistrictId,
                        principalTable: "District",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PreinspectionManufacturingUnit_DistrictId",
                table: "PreinspectionManufacturingUnit",
                column: "DistrictId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PreinspectionManufacturingUnit");
        }
    }
}
