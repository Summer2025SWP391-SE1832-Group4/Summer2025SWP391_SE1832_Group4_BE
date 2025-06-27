using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HIVTreatmentSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAppointmentIdToTestResult : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "DoctorComments",
                table: "TestResults",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AppointmentId",
                table: "TestResults",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AttachedFileUrl",
                table: "TestResults",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TestResults_AppointmentId",
                table: "TestResults",
                column: "AppointmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_TestResults_Appointments_AppointmentId",
                table: "TestResults",
                column: "AppointmentId",
                principalTable: "Appointments",
                principalColumn: "AppointmentId",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TestResults_Appointments_AppointmentId",
                table: "TestResults");

            migrationBuilder.DropIndex(
                name: "IX_TestResults_AppointmentId",
                table: "TestResults");

            migrationBuilder.DropColumn(
                name: "AppointmentId",
                table: "TestResults");

            migrationBuilder.DropColumn(
                name: "AttachedFileUrl",
                table: "TestResults");

            migrationBuilder.AlterColumn<string>(
                name: "DoctorComments",
                table: "TestResults",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
