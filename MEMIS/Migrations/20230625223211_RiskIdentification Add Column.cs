using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MEMIS.Migrations
{
    public partial class RiskIdentificationAddColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ApprStatus",
                table: "RiskIdentification",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "HerbalInspection",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InspectionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InspectorName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GPS = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    intRegion = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DistrictId = table.Column<int>(type: "int", nullable: false),
                    FacilityName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FacilityStatus = table.Column<int>(type: "int", nullable: false),
                    FacilityPersonType = table.Column<int>(type: "int", nullable: false),
                    PersonName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Contact = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Qualifications = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CategoryOfpremises = table.Column<int>(type: "int", nullable: false),
                    LicenseStatus = table.Column<int>(type: "int", nullable: false),
                    Activity = table.Column<int>(type: "int", nullable: false),
                    CertRecommendation = table.Column<int>(type: "int", nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InspectorId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HerbalInspection", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HerbalInspection_District_DistrictId",
                        column: x => x.DistrictId,
                        principalTable: "District",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HerbalInspection_Region_intRegion",
                        column: x => x.intRegion,
                        principalTable: "Region",
                        principalColumn: "intRegion",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HerbalInspection_DistrictId",
                table: "HerbalInspection",
                column: "DistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_HerbalInspection_intRegion",
                table: "HerbalInspection",
                column: "intRegion");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HerbalInspection");

            migrationBuilder.DropColumn(
                name: "ApprStatus",
                table: "RiskIdentification");
        }
    }
}
