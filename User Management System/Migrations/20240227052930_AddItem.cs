using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace User_Management_System.Migrations
{
    public partial class AddItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "SupremeUsers",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "UniqueId",
                table: "SupremeUsers",
                newName: "UserUniqueId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Items",
                newName: "itemId");

            migrationBuilder.RenameColumn(
                name: "UniqueId",
                table: "Items",
                newName: "ItemUniqueId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "SupremeUsers",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "UserUniqueId",
                table: "SupremeUsers",
                newName: "UniqueId");

            migrationBuilder.RenameColumn(
                name: "itemId",
                table: "Items",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ItemUniqueId",
                table: "Items",
                newName: "UniqueId");
        }
    }
}
