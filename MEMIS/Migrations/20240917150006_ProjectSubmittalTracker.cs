using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MEMIS.Migrations
{
    /// <inheritdoc />
    public partial class ProjectSubmittalTracker : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           

            migrationBuilder.CreateTable(
                name: "ProjectSubmittalTracker",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubmittalDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TaskOwner = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TaskDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ActionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaidDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExpectedDays = table.Column<int>(type: "int", nullable: true),
                    VarianceDays = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpectedAmount = table.Column<long>(type: "bigint", nullable: true),
                    AmountPaid = table.Column<long>(type: "bigint", nullable: true),
                    ProjectInitiationId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectSubmittalTracker", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectSubmittalTracker_ProjectInitiation_ProjectInitiationId",
                        column: x => x.ProjectInitiationId,
                        principalTable: "ProjectInitiation",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectSubmittalTracker_ProjectInitiationId",
                table: "ProjectSubmittalTracker",
                column: "ProjectInitiationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectSubmittalTracker");

            
        }
    }
}
