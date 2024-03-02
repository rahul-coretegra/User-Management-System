using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace User_Management_System.Migrations.PostgreSqlApplicationDb
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

            migrationBuilder.AddColumn<string>(
                name: "ItemName",
                table: "ConfigureServices",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ItemValue",
                table: "ConfigureServices",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ItemName",
                table: "ConfigureServices");

            migrationBuilder.DropColumn(
                name: "ItemValue",
                table: "ConfigureServices");

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    ItemId = table.Column<string>(type: "text", nullable: false),
                    ServiceId = table.Column<string>(type: "text", nullable: true),
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ItemName = table.Column<string>(type: "text", nullable: false),
                    ItemUniqueId = table.Column<string>(type: "text", nullable: false),
                    ItemValue = table.Column<string>(type: "text", nullable: false),
                    ServiceUniqueId = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.ItemId);
                    table.ForeignKey(
                        name: "FK_Items_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "ServiceId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConfigureServices_ItemUniqueId",
                table: "ConfigureServices",
                column: "ItemUniqueId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_ServiceId",
                table: "Items",
                column: "ServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_ConfigureServices_Items_ItemUniqueId",
                table: "ConfigureServices",
                column: "ItemUniqueId",
                principalTable: "Items",
                principalColumn: "ItemId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
