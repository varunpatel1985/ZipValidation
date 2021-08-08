using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Data.Migrations
{
    public partial class changetablename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ZipValidations");

            migrationBuilder.CreateTable(
                name: "FileSetups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    source = table.Column<string>(type: "TEXT", nullable: true),
                    target = table.Column<string>(type: "TEXT", nullable: true),
                    adminEmail = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileSetups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AllowedFileTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FileTypeExtension = table.Column<string>(type: "TEXT", nullable: true),
                    FileSetupId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AllowedFileTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AllowedFileTypes_FileSetups_FileSetupId",
                        column: x => x.FileSetupId,
                        principalTable: "FileSetups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AllowedFileTypes_FileSetupId",
                table: "AllowedFileTypes",
                column: "FileSetupId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AllowedFileTypes");

            migrationBuilder.DropTable(
                name: "FileSetups");

            migrationBuilder.CreateTable(
                name: "ZipValidations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    adminEmail = table.Column<string>(type: "TEXT", nullable: true),
                    source = table.Column<string>(type: "TEXT", nullable: true),
                    target = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZipValidations", x => x.Id);
                });
        }
    }
}
