using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MEMIS.Migrations
{
    public partial class RelocationPharma : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RelocationPharma",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InspectionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Applicant = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Business = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CRoad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CZone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CVillage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CCountry = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CTelephone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CGPS = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PRoad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PZone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PVillage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PCountry = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PTelephone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PGPS = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductClassification = table.Column<int>(type: "int", nullable: false),
                    CategoryOfpremises = table.Column<int>(type: "int", nullable: false),
                    comments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApprovalStatusInspector = table.Column<int>(type: "int", nullable: false),
                    ApprovalStatusHead = table.Column<int>(type: "int", nullable: false),
                    ApprovalStatusDir = table.Column<int>(type: "int", nullable: false),
                    DistrictId = table.Column<int>(type: "int", nullable: false),
                    InspectorId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InspectorName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Inspectorcomments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HeadInspectorId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HeadInspectorName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HeadInspectorcomments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DirectorInspectorId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DirectorInspectorName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DirectorInspectorcomments = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RelocationPharma", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RelocationPharma_District_DistrictId",
                        column: x => x.DistrictId,
                        principalTable: "District",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RelocationPharma_DistrictId",
                table: "RelocationPharma",
                column: "DistrictId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RelocationPharma");
        }
    }
}
