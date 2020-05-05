using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebArchivProject.Migrations
{
    public partial class InitMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppUsers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Mail = table.Column<string>(nullable: true),
                    Role = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    AvatarUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Authors",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NameRu = table.Column<string>(nullable: true),
                    NameUa = table.Column<string>(nullable: true),
                    NameEn = table.Column<string>(nullable: true),
                    IsFirst = table.Column<bool>(nullable: false),
                    ExternalId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    OwnerId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Year = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    Issuer = table.Column<string>(nullable: true),
                    MaxPageCount = table.Column<int>(nullable: false),
                    AuthorExternalId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    OwnerId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Year = table.Column<string>(nullable: true),
                    TomName = table.Column<string>(nullable: true),
                    Magazine = table.Column<string>(nullable: true),
                    MagazineNumber = table.Column<string>(nullable: true),
                    PagesInterval = table.Column<string>(nullable: true),
                    AuthorExternalId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Theses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    OwnerId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Year = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    ConferenceName = table.Column<string>(nullable: true),
                    DatesInterval = table.Column<string>(nullable: true),
                    PagesInterval = table.Column<string>(nullable: true),
                    AuthorExternalId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Theses", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppUsers");

            migrationBuilder.DropTable(
                name: "Authors");

            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.DropTable(
                name: "Theses");
        }
    }
}
