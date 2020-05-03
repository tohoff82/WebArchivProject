using Microsoft.EntityFrameworkCore.Migrations;

namespace WebArchivProject.Migrations
{
    public partial class AppUser_Add_Field_Role : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "AppUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Role",
                table: "AppUsers");
        }
    }
}
