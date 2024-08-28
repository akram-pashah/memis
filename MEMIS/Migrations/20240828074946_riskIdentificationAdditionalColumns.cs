using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MEMIS.Migrations
{
    /// <inheritdoc />
    public partial class riskIdentificationAdditionalColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Additional_Mitigation",
                table: "RiskIdentification",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExistingMitigation",
                table: "RiskIdentification",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Weakness",
                table: "RiskIdentification",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "intCategory",
                table: "RiskIdentification",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "RiskCategorys",
                columns: table => new
                {
                    intCategory = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CategoryName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiskCategorys", x => x.intCategory);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RiskIdentification_intCategory",
                table: "RiskIdentification",
                column: "intCategory");

        
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.DropTable(
                name: "RiskCategorys");

            migrationBuilder.DropIndex(
                name: "IX_RiskIdentification_intCategory",
                table: "RiskIdentification");

            migrationBuilder.DropColumn(
                name: "Additional_Mitigation",
                table: "RiskIdentification");

            migrationBuilder.DropColumn(
                name: "ExistingMitigation",
                table: "RiskIdentification");

            migrationBuilder.DropColumn(
                name: "Weakness",
                table: "RiskIdentification");

            migrationBuilder.DropColumn(
                name: "intCategory",
                table: "RiskIdentification");
        }
    }
}
