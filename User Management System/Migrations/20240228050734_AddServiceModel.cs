using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace User_Management_System.Migrations
{
    public partial class AddServiceModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ServiceUniqueId",
                table: "Items",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    ServiceUniqueId = table.Column<string>(type: "text", nullable: false),
                    ServiceId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ServiceName = table.Column<string>(type: "text", nullable: false),
                    ServiceType = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.ServiceUniqueId);
                });

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Services_ServiceUniqueId",
                table: "Items");

            migrationBuilder.DropTable(
                name: "Services");

            migrationBuilder.DropIndex(
                name: "IX_Items_ServiceUniqueId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "ServiceUniqueId",
                table: "Items");
        }
    }
}
