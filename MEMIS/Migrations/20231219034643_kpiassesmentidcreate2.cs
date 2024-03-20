using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MEMIS.Migrations
{
    public partial class kpiassesmentidcreate2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "KPI_Assessment",
                type: "int",
                nullable: false )
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_KPI_Assessment",
                table: "KPI_Assessment",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
           
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "KPI_Assessment",
                type: "int",
                nullable: false ) ;

            
        }
    }
}
