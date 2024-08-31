using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MEMIS.Migrations
{
    /// <inheritdoc />
    public partial class AddStatusActivityPlan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "ActivityPlan",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "ActivityPlan");
        }
    }
}
