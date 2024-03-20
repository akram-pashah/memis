using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MEMIS.Migrations
{
    public partial class ProjectStakeholder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProjectStakeHolder",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StakeholderName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactPersonName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactPersonEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactPersonPhone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactPersonAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactPersonWebsite = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Impact = table.Column<int>(type: "int", nullable: false),
                    Influence = table.Column<int>(type: "int", nullable: false),
                    StakeHolderImportant = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StakeholderContribution = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Stakeholderblock = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StakeholderStrategy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProjectInitiationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectStakeHolder", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectStakeHolder_ProjectInitiation_ProjectInitiationId",
                        column: x => x.ProjectInitiationId,
                        principalTable: "ProjectInitiation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectStakeHolder_ProjectInitiationId",
                table: "ProjectStakeHolder",
                column: "ProjectInitiationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectStakeHolder");
        }
    }
}
