using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MEMIS.Migrations
{
    /// <inheritdoc />
    public partial class unitCostQuarterlyplan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UnitCost",
                table: "ProgramImplementationPlan",
                newName: "unitCost");

            migrationBuilder.RenameColumn(
                name: "UnitCost",
                table: "AnnualImplemtationPlan",
                newName: "unitCost");

            migrationBuilder.RenameColumn(
                name: "UnitCost",
                table: "ActivityAssessRegion",
                newName: "unitCost");

            migrationBuilder.RenameColumn(
                name: "UnitCost",
                table: "ActivityAssessmentRegion",
                newName: "unitCost");

            migrationBuilder.RenameColumn(
                name: "UnitCost",
                table: "ActivityAssess",
                newName: "unitCost");

            migrationBuilder.AddColumn<double>(
                name: "unitCost",
                table: "QuaterlyPlans",
                type: "float",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "unitCost",
                table: "QuaterlyPlans");

            migrationBuilder.RenameColumn(
                name: "unitCost",
                table: "ProgramImplementationPlan",
                newName: "UnitCost");

            migrationBuilder.RenameColumn(
                name: "unitCost",
                table: "AnnualImplemtationPlan",
                newName: "UnitCost");

            migrationBuilder.RenameColumn(
                name: "unitCost",
                table: "ActivityAssessRegion",
                newName: "UnitCost");

            migrationBuilder.RenameColumn(
                name: "unitCost",
                table: "ActivityAssessmentRegion",
                newName: "UnitCost");

            migrationBuilder.RenameColumn(
                name: "unitCost",
                table: "ActivityAssess",
                newName: "UnitCost");
        }
    }
}
