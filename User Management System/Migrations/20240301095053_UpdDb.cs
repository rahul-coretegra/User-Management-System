using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace User_Management_System.Migrations
{
    public partial class UpdDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Services_ServiceUniqueId",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_ServiceUniqueId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "ServiceUniqueId",
                table: "Items");

            migrationBuilder.RenameColumn(
                name: "IsDatabaseExists",
                table: "Projects",
                newName: "Status");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "SupremeUsers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Services",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MigrateDatabase",
                table: "Projects",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Items",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ConfigureServices",
                columns: table => new
                {
                    UniqueId = table.Column<string>(type: "text", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ServiceUniqueId = table.Column<string>(type: "text", nullable: false),
                    ItemUniqueId = table.Column<string>(type: "text", nullable: false),
                    IsConfigured = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfigureServices", x => x.UniqueId);
                    table.ForeignKey(
                        name: "FK_ConfigureServices_Items_ItemUniqueId",
                        column: x => x.ItemUniqueId,
                        principalTable: "Items",
                        principalColumn: "ItemUniqueId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ConfigureServices_Services_ServiceUniqueId",
                        column: x => x.ServiceUniqueId,
                        principalTable: "Services",
                        principalColumn: "ServiceUniqueId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConfigureServices_ItemUniqueId",
                table: "ConfigureServices",
                column: "ItemUniqueId");

            migrationBuilder.CreateIndex(
                name: "IX_ConfigureServices_ServiceUniqueId",
                table: "ConfigureServices",
                column: "ServiceUniqueId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConfigureServices");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "SupremeUsers");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "MigrateDatabase",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Items");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Projects",
                newName: "IsDatabaseExists");

            migrationBuilder.AddColumn<string>(
                name: "ServiceUniqueId",
                table: "Items",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Items_ServiceUniqueId",
                table: "Items",
                column: "ServiceUniqueId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Services_ServiceUniqueId",
                table: "Items",
                column: "ServiceUniqueId",
                principalTable: "Services",
                principalColumn: "ServiceUniqueId");
        }
    }
}
