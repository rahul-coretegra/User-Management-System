using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace User_Management_System.Migrations.MicrosoftSqlServerApplicationDb
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

            migrationBuilder.AlterColumn<string>(
                name: "ItemUniqueId",
                table: "ConfigureServices",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "ItemName",
                table: "ConfigureServices",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ItemValue",
                table: "ConfigureServices",
                type: "nvarchar(max)",
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

            migrationBuilder.AlterColumn<string>(
                name: "ItemUniqueId",
                table: "ConfigureServices",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    ItemId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ServiceId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ItemUniqueId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ItemValue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ServiceUniqueId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
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
