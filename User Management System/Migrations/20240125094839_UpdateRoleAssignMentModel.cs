using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace User_Management_System.Migrations
{
    public partial class UpdateRoleAssignMentModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserRoleAssignments_UserRoles_RoleUniqueCode",
                table: "UserRoleAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoleAssignments_Users_UserUniqueCode",
                table: "UserRoleAssignments");

            migrationBuilder.RenameColumn(
                name: "RoleUniqueCode",
                table: "UserRoleAssignments",
                newName: "roleUniqueCode");

            migrationBuilder.RenameColumn(
                name: "UserUniqueCode",
                table: "UserRoleAssignments",
                newName: "userUniqueCode");

            migrationBuilder.RenameColumn(
                name: "AssignmentId",
                table: "UserRoleAssignments",
                newName: "id");

            migrationBuilder.RenameIndex(
                name: "IX_UserRoleAssignments_RoleUniqueCode",
                table: "UserRoleAssignments",
                newName: "IX_UserRoleAssignments_roleUniqueCode");

            migrationBuilder.AddColumn<int>(
                name: "accessToRole",
                table: "UserRoleAssignments",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "assignmentUniqueId",
                table: "UserRoleAssignments",
                type: "text",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoleAssignments_UserRoles_roleUniqueCode",
                table: "UserRoleAssignments",
                column: "roleUniqueCode",
                principalTable: "UserRoles",
                principalColumn: "roleUniqueCode",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoleAssignments_Users_userUniqueCode",
                table: "UserRoleAssignments",
                column: "userUniqueCode",
                principalTable: "Users",
                principalColumn: "userUniqueCode",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserRoleAssignments_UserRoles_roleUniqueCode",
                table: "UserRoleAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoleAssignments_Users_userUniqueCode",
                table: "UserRoleAssignments");

            migrationBuilder.DropColumn(
                name: "accessToRole",
                table: "UserRoleAssignments");

            migrationBuilder.DropColumn(
                name: "assignmentUniqueId",
                table: "UserRoleAssignments");

            migrationBuilder.RenameColumn(
                name: "roleUniqueCode",
                table: "UserRoleAssignments",
                newName: "RoleUniqueCode");

            migrationBuilder.RenameColumn(
                name: "userUniqueCode",
                table: "UserRoleAssignments",
                newName: "UserUniqueCode");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "UserRoleAssignments",
                newName: "AssignmentId");

            migrationBuilder.RenameIndex(
                name: "IX_UserRoleAssignments_roleUniqueCode",
                table: "UserRoleAssignments",
                newName: "IX_UserRoleAssignments_RoleUniqueCode");

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoleAssignments_UserRoles_RoleUniqueCode",
                table: "UserRoleAssignments",
                column: "RoleUniqueCode",
                principalTable: "UserRoles",
                principalColumn: "roleUniqueCode",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoleAssignments_Users_UserUniqueCode",
                table: "UserRoleAssignments",
                column: "UserUniqueCode",
                principalTable: "Users",
                principalColumn: "userUniqueCode",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
