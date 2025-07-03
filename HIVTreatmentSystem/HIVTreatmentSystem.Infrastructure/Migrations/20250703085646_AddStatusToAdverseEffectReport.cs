using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HIVTreatmentSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddStatusToAdverseEffectReport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "AdverseEffectReports",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "AdverseEffectReports");
        }
    }
}
