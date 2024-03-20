using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MEMIS.Migrations
{
    public partial class focusarea_new : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "T_FOCUSAREA");

            migrationBuilder.CreateTable(
                name: "FocusArea",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FocusAreacode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    FocusAreaName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FocusArea", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FocusArea");

            migrationBuilder.CreateTable(
                name: "T_FOCUSAREA",
                columns: table => new
                {
                    intFocusArea = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FocusAreaCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FocusAreaName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_FOCUSAREA", x => x.intFocusArea);
                });
        }
    }
}
