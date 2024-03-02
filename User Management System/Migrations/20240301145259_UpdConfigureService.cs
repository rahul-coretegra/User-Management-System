using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace User_Management_System.Migrations
{
    public partial class UpdConfigureService : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConfigureServices_Items_ItemUniqueId",
                table: "ConfigureServices");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropIndex(
                name: "IX_ConfigureServices_ItemUniqueId",
                table: "ConfigureServices");

            migrationBuilder.RenameColumn(
                name: "ItemUniqueId",
                table: "ConfigureServices",
                newName: "ItemName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ItemName",
                table: "ConfigureServices",
                newName: "ItemUniqueId");

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    ItemUniqueId = table.Column<string>(type: "text", nullable: false),
                    ItemId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ItemName = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.ItemUniqueId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConfigureServices_ItemUniqueId",
                table: "ConfigureServices",
                column: "ItemUniqueId");

            migrationBuilder.AddForeignKey(
                name: "FK_ConfigureServices_Items_ItemUniqueId",
                table: "ConfigureServices",
                column: "ItemUniqueId",
                principalTable: "Items",
                principalColumn: "ItemUniqueId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
