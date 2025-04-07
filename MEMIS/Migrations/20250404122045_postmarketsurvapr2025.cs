using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MEMIS.Migrations
{
    /// <inheritdoc />
    public partial class postmarketsurvapr2025 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PostMarketSurveillance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InspectionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InspectorName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InspectorId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Latitude = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Longitude = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    intRegion = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DistrictId = table.Column<int>(type: "int", nullable: false),
                    FacilityName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FacilityStatus = table.Column<int>(type: "int", nullable: true),
                    FacilityPersonType = table.Column<int>(type: "int", nullable: true),
                    PersonName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Contact = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Qualifications = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CategoryOfpremises = table.Column<int>(type: "int", nullable: true),
                    Other_CategoryPremise = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LicenseStatus = table.Column<int>(type: "int", nullable: true),
                    LicenseNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Unlicensed = table.Column<int>(type: "int", nullable: true),
                    PMSActivity = table.Column<int>(type: "int", nullable: true),
                    Sample_ProductName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Sample_No = table.Column<int>(type: "int", nullable: true),
                    Sample_Batch = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Followup_Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Complaint_Product = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Other_Activity = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostMarketSurveillance", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PostMarketSurveillance_District_DistrictId",
                        column: x => x.DistrictId,
                        principalTable: "District",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostMarketSurveillance_Region_intRegion",
                        column: x => x.intRegion,
                        principalTable: "Region",
                        principalColumn: "intRegion",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PostMarketSurveillance_DistrictId",
                table: "PostMarketSurveillance",
                column: "DistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_PostMarketSurveillance_intRegion",
                table: "PostMarketSurveillance",
                column: "intRegion");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PostMarketSurveillance");
        }
    }
}
