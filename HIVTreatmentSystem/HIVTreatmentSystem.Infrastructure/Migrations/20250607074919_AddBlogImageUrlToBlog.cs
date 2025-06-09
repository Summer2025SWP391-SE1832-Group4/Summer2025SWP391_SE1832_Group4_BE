using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HIVTreatmentSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBlogImageUrlToBlog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BlogImageUrl",
                table: "Blogs",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BlogImageUrl",
                table: "Blogs");
        }
    }
}
