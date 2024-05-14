using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MEMIS.Migrations
{
    /// <inheritdoc />
    public partial class acttypedept : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "actType",
                table: "ActivityAssess",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "intDept",
                table: "ActivityAssess",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ActivityAssess_intDept",
                table: "ActivityAssess",
                column: "intDept");

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityAssess_Departments_intDept",
                table: "ActivityAssess",
                column: "intDept",
                principalTable: "Departments",
                principalColumn: "intDept");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActivityAssess_Departments_intDept",
                table: "ActivityAssess");

            migrationBuilder.DropIndex(
                name: "IX_ActivityAssess_intDept",
                table: "ActivityAssess");

            migrationBuilder.DropColumn(
                name: "actType",
                table: "ActivityAssess");

            migrationBuilder.DropColumn(
                name: "intDept",
                table: "ActivityAssess");
        }
    }
}
