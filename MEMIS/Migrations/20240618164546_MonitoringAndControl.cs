using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MEMIS.Migrations
{
    /// <inheritdoc />
    public partial class MonitoringAndControl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MonitoringAndControl",
                columns: table => new
                {
                    TaskName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Duration = table.Column<long>(type: "bigint", nullable: false),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: false),
                    ImplementationStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompletedDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OverDue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LateBy = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonitoringAndControl", x => x.TaskName);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MonitoringAndControl");
        }
    }
}
