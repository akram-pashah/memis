using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MEMIS.Migrations
{
    /// <inheritdoc />
    public partial class QuarterlyPlan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QuaterlyPlans",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Quarter = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QTarget = table.Column<long>(type: "bigint", nullable: true),
                    QBudget = table.Column<long>(type: "bigint", nullable: true),
                    ActivityAccessId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuaterlyPlans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuaterlyPlans_ActivityAssess_ActivityAccessId",
                        column: x => x.ActivityAccessId,
                        principalTable: "ActivityAssess",
                        principalColumn: "intAssess",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuaterlyPlans_ActivityAccessId",
                table: "QuaterlyPlans",
                column: "ActivityAccessId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuaterlyPlans");
        }
    }
}
