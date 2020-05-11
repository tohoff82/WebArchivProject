using Microsoft.EntityFrameworkCore.Migrations;

namespace WebArchivProject.Migrations
{
    public partial class Extension_App_User_Add_Name_Surname : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SecondName",
                table: "AppUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SurName",
                table: "AppUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SecondName",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "SurName",
                table: "AppUsers");
        }
    }
}
