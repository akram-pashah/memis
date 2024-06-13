﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MEMIS.Migrations
{
    /// <inheritdoc />
    public partial class projectOtherTabFile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProjectOthersTab",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Resourses = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Attachment = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    ProjectInitiationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectOthersTab", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectOthersTab_ProjectInitiation_ProjectInitiationId",
                        column: x => x.ProjectInitiationId,
                        principalTable: "ProjectInitiation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectOthersTab_ProjectInitiationId",
                table: "ProjectOthersTab",
                column: "ProjectInitiationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectOthersTab");
        }
    }
}