using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace ZIMS.Migrations
{
    public partial class initialcreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FirstName = table.Column<string>(type: "text", nullable: true),
                    LastName = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    Role = table.Column<string>(type: "text", nullable: true),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    ResetToken = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "FirstName", "LastName", "PasswordHash", "ResetToken", "Role" },
                values: new object[,]
                {
                    { 1, "Mitchellscott@me.com", "Mitchell", "Scott", "$2a$11$6GOC938jotYVtUamR6ekKOqL6UTKPQ3NTKtrCSZ1SqppQNt3d.B9K", "", "Manager" },
                    { 2, "Sarahscott@me.com", "Sarah", "Scott", "$2a$11$HWrdjdtWVYer3SrtNj4N1OBTu78uJor9TATO/TUmNjLnPGgqxOOlK", "", "Guide" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
