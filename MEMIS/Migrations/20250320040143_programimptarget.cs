using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MEMIS.Migrations
{
    /// <inheritdoc />
    public partial class programimptarget : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RadioTalkShow_Region_RegionintRegion",
                table: "RadioTalkShow");

            migrationBuilder.DropIndex(
                name: "IX_RadioTalkShow_RegionintRegion",
                table: "RadioTalkShow");

            migrationBuilder.DropColumn(
                name: "RegionintRegion",
                table: "RadioTalkShow");

            migrationBuilder.AlterColumn<long>(
                name: "OutputTarget",
                table: "ProgramImplementationPlan",
                type: "bigint",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RadioTalkShow_intRegion",
                table: "RadioTalkShow",
                column: "intRegion");

            migrationBuilder.AddForeignKey(
                name: "FK_RadioTalkShow_Region_intRegion",
                table: "RadioTalkShow",
                column: "intRegion",
                principalTable: "Region",
                principalColumn: "intRegion",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RadioTalkShow_Region_intRegion",
                table: "RadioTalkShow");

            migrationBuilder.DropIndex(
                name: "IX_RadioTalkShow_intRegion",
                table: "RadioTalkShow");

            migrationBuilder.AddColumn<Guid>(
                name: "RegionintRegion",
                table: "RadioTalkShow",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<string>(
                name: "OutputTarget",
                table: "ProgramImplementationPlan",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RadioTalkShow_RegionintRegion",
                table: "RadioTalkShow",
                column: "RegionintRegion");

            migrationBuilder.AddForeignKey(
                name: "FK_RadioTalkShow_Region_RegionintRegion",
                table: "RadioTalkShow",
                column: "RegionintRegion",
                principalTable: "Region",
                principalColumn: "intRegion",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
