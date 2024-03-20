using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MEMIS.Migrations
{
    public partial class regionhead : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.DropIndex(
                name: "IX_Region_intDir",
                table: "Region");

            migrationBuilder.DropColumn(
                name: "intDir",
                table: "Region");

            migrationBuilder.AddColumn<string>(
                name: "IntHeadId",
                table: "Region",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RegCoordinatorId",
                table: "Region",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TypeofIndicator",
                table: "KPI",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Indicatorclassification",
                table: "KPI",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "FrequencyofReporting",
                table: "KPI",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Region_IntHeadId",
                table: "Region",
                column: "IntHeadId");

            migrationBuilder.CreateIndex(
                name: "IX_Region_RegCoordinatorId",
                table: "Region",
                column: "RegCoordinatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Region_AspNetUsers_IntHeadId",
                table: "Region",
                column: "IntHeadId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Region_AspNetUsers_RegCoordinatorId",
                table: "Region",
                column: "RegCoordinatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Region_AspNetUsers_IntHeadId",
                table: "Region");

            migrationBuilder.DropForeignKey(
                name: "FK_Region_AspNetUsers_RegCoordinatorId",
                table: "Region");

            migrationBuilder.DropIndex(
                name: "IX_Region_IntHeadId",
                table: "Region");

            migrationBuilder.DropIndex(
                name: "IX_Region_RegCoordinatorId",
                table: "Region");

            migrationBuilder.DropColumn(
                name: "IntHeadId",
                table: "Region");

            migrationBuilder.DropColumn(
                name: "RegCoordinatorId",
                table: "Region");

            migrationBuilder.AddColumn<Guid>(
                name: "intDir",
                table: "Region",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<int>(
                name: "TypeofIndicator",
                table: "KPI",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "Indicatorclassification",
                table: "KPI",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "FrequencyofReporting",
                table: "KPI",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Region_intDir",
                table: "Region",
                column: "intDir");

          
        }
    }
}
