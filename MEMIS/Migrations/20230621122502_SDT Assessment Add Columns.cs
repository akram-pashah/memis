using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MEMIS.Migrations
{
    public partial class SDTAssessmentAddColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ApprovalStatusDDCS",
                table: "SDTAssessment",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ApprovalStatusHBPD",
                table: "SDTAssessment",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ApprovalStatusMEOFfficer",
                table: "SDTAssessment",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "DDCSAction",
                table: "SDTAssessment",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DDCSActionDate",
                table: "SDTAssessment",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DDCSComment",
                table: "SDTAssessment",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HBPDAction",
                table: "SDTAssessment",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "HBPDActionDate",
                table: "SDTAssessment",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HBPDComment",
                table: "SDTAssessment",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MEOfficerAction",
                table: "SDTAssessment",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "MEOfficerActionDate",
                table: "SDTAssessment",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MEOfficerComment",
                table: "SDTAssessment",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApprovalStatusDDCS",
                table: "SDTAssessment");

            migrationBuilder.DropColumn(
                name: "ApprovalStatusHBPD",
                table: "SDTAssessment");

            migrationBuilder.DropColumn(
                name: "ApprovalStatusMEOFfficer",
                table: "SDTAssessment");

            migrationBuilder.DropColumn(
                name: "DDCSAction",
                table: "SDTAssessment");

            migrationBuilder.DropColumn(
                name: "DDCSActionDate",
                table: "SDTAssessment");

            migrationBuilder.DropColumn(
                name: "DDCSComment",
                table: "SDTAssessment");

            migrationBuilder.DropColumn(
                name: "HBPDAction",
                table: "SDTAssessment");

            migrationBuilder.DropColumn(
                name: "HBPDActionDate",
                table: "SDTAssessment");

            migrationBuilder.DropColumn(
                name: "HBPDComment",
                table: "SDTAssessment");

            migrationBuilder.DropColumn(
                name: "MEOfficerAction",
                table: "SDTAssessment");

            migrationBuilder.DropColumn(
                name: "MEOfficerActionDate",
                table: "SDTAssessment");

            migrationBuilder.DropColumn(
                name: "MEOfficerComment",
                table: "SDTAssessment");
        }
    }
}
