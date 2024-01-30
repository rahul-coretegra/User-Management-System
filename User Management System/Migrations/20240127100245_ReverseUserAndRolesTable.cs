using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace User_Management_System.Migrations
{
    public partial class ReverseUserAndRolesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isActiveUser",
                table: "UserAndUserRoles");

            migrationBuilder.AddColumn<int>(
                name: "isActiveUser",
                table: "Users",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isActiveUser",
                table: "Users");

            migrationBuilder.AddColumn<int>(
                name: "isActiveUser",
                table: "UserAndUserRoles",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
