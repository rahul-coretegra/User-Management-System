using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace User_Management_System.Migrations
{
    public partial class UpdateCompositekeytoPKofuserandrols : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserAndUserRoles",
                table: "UserAndUserRoles");

            migrationBuilder.AlterColumn<string>(
                name: "userAndRoleUniqueId",
                table: "UserAndUserRoles",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserAndUserRoles",
                table: "UserAndUserRoles",
                column: "userAndRoleUniqueId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAndUserRoles_userUniqueCode",
                table: "UserAndUserRoles",
                column: "userUniqueCode");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserAndUserRoles",
                table: "UserAndUserRoles");

            migrationBuilder.DropIndex(
                name: "IX_UserAndUserRoles_userUniqueCode",
                table: "UserAndUserRoles");

            migrationBuilder.AlterColumn<string>(
                name: "userAndRoleUniqueId",
                table: "UserAndUserRoles",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserAndUserRoles",
                table: "UserAndUserRoles",
                columns: new[] { "userUniqueCode", "roleUniqueCode" });
        }
    }
}
