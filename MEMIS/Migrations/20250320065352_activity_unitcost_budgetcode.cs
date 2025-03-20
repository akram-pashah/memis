using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MEMIS.Migrations
{
    /// <inheritdoc />
    public partial class activity_unitcost_budgetcode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "budgetCode",
                table: "Activity",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "unitCost",
                table: "Activity",
                type: "float",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "budgetCode",
                table: "Activity");

            migrationBuilder.DropColumn(
                name: "unitCost",
                table: "Activity");
        }
    }
}
