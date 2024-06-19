using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MEMIS.Migrations
{
    /// <inheritdoc />
    public partial class monitoringAndControlPk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MonitoringAndControl",
                table: "MonitoringAndControl");

            migrationBuilder.AlterColumn<string>(
                name: "TaskName",
                table: "MonitoringAndControl",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "MonitoringAndControl",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MonitoringAndControl",
                table: "MonitoringAndControl",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MonitoringAndControl",
                table: "MonitoringAndControl");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "MonitoringAndControl");

            migrationBuilder.AlterColumn<string>(
                name: "TaskName",
                table: "MonitoringAndControl",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MonitoringAndControl",
                table: "MonitoringAndControl",
                column: "TaskName");
        }
    }
}
