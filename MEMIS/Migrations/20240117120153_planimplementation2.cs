using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MEMIS.Migrations
{
    public partial class planimplementation2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProgramImplementationPlan",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Activity = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Output = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    ResponsibleParty = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    DateofAction = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Baseline = table.Column<long>(type: "bigint", nullable: false),
                    FY1 = table.Column<long>(type: "bigint", nullable: false),
                    FY2 = table.Column<long>(type: "bigint", nullable: false),
                    FY3 = table.Column<long>(type: "bigint", nullable: false),
                    FY4 = table.Column<long>(type: "bigint", nullable: false),
                    FY5 = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProgramImplementationPlan", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProgramImplementationPlan");
        }
    }
}
