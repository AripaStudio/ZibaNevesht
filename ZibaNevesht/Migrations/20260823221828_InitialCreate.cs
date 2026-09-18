using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZibaNevesht.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ZBchamekade",
                columns: table => new
                {
                    ChameID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PoetName = table.Column<string>(type: "TEXT", nullable: false),
                    CategoryName = table.Column<string>(type: "TEXT", nullable: false),
                    ChameText = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZBchamekade", x => x.ChameID);
                });

            migrationBuilder.CreateTable(
                name: "ZBgoshBeZang",
                columns: table => new
                {
                    NameWord = table.Column<string>(type: "TEXT", nullable: false),
                    AlertLevel = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZBgoshBeZang", x => x.NameWord);
                });

            migrationBuilder.CreateTable(
                name: "ZBmishmare",
                columns: table => new
                {
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    NumberOfUsedWord = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZBmishmare", x => x.Name);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ZBchamekade");

            migrationBuilder.DropTable(
                name: "ZBgoshBeZang");

            migrationBuilder.DropTable(
                name: "ZBmishmare");
        }
    }
}
