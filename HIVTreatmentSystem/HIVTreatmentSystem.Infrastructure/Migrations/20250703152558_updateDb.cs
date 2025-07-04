using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HIVTreatmentSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BaselineCD4",
                table: "PatientTreatments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BaselineHivViralLoad",
                table: "PatientTreatments",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BaselineCD4",
                table: "PatientTreatments");

            migrationBuilder.DropColumn(
                name: "BaselineHivViralLoad",
                table: "PatientTreatments");
        }
    }
}
