using Microsoft.EntityFrameworkCore.Migrations;

namespace WebArchivProject.Migrations
{
    public partial class Field_OwnerId_Added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OwnerId",
                table: "Theses",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OwnerId",
                table: "Posts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OwnerId",
                table: "Books",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Theses");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Books");
        }
    }
}
