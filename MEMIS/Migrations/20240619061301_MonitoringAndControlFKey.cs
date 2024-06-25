﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MEMIS.Migrations
{
    /// <inheritdoc />
    public partial class MonitoringAndControlFKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProjectInitiationId",
                table: "MonitoringAndControl",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MonitoringAndControl_ProjectInitiationId",
                table: "MonitoringAndControl",
                column: "ProjectInitiationId");

            migrationBuilder.AddForeignKey(
                name: "FK_MonitoringAndControl_ProjectInitiation_ProjectInitiationId",
                table: "MonitoringAndControl",
                column: "ProjectInitiationId",
                principalTable: "ProjectInitiation",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MonitoringAndControl_ProjectInitiation_ProjectInitiationId",
                table: "MonitoringAndControl");

            migrationBuilder.DropIndex(
                name: "IX_MonitoringAndControl_ProjectInitiationId",
                table: "MonitoringAndControl");

            migrationBuilder.DropColumn(
                name: "ProjectInitiationId",
                table: "MonitoringAndControl");
        }
    }
}