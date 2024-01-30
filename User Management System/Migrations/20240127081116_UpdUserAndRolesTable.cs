using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace User_Management_System.Migrations
{
    public partial class UpdUserAndRolesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserRoleAssignments");

            migrationBuilder.DropColumn(
                name: "isActiveUser",
                table: "Users");

            migrationBuilder.CreateTable(
                name: "UserAndUserRoles",
                columns: table => new
                {
                    userUniqueCode = table.Column<string>(type: "text", nullable: false),
                    roleUniqueCode = table.Column<string>(type: "text", nullable: false),
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    userAndRoleUniqueId = table.Column<string>(type: "text", nullable: true),
                    accessToRole = table.Column<int>(type: "integer", nullable: false),
                    isActiveUser = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAndUserRoles", x => new { x.userUniqueCode, x.roleUniqueCode });
                    table.ForeignKey(
                        name: "FK_UserAndUserRoles_UserRoles_roleUniqueCode",
                        column: x => x.roleUniqueCode,
                        principalTable: "UserRoles",
                        principalColumn: "roleUniqueCode",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserAndUserRoles_Users_userUniqueCode",
                        column: x => x.userUniqueCode,
                        principalTable: "Users",
                        principalColumn: "userUniqueCode",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserAndUserRoles_roleUniqueCode",
                table: "UserAndUserRoles",
                column: "roleUniqueCode");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserAndUserRoles");

            migrationBuilder.AddColumn<int>(
                name: "isActiveUser",
                table: "Users",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "UserRoleAssignments",
                columns: table => new
                {
                    userUniqueCode = table.Column<string>(type: "text", nullable: false),
                    roleUniqueCode = table.Column<string>(type: "text", nullable: false),
                    accessToRole = table.Column<int>(type: "integer", nullable: false),
                    assignmentUniqueId = table.Column<string>(type: "text", nullable: true),
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoleAssignments", x => new { x.userUniqueCode, x.roleUniqueCode });
                    table.ForeignKey(
                        name: "FK_UserRoleAssignments_UserRoles_roleUniqueCode",
                        column: x => x.roleUniqueCode,
                        principalTable: "UserRoles",
                        principalColumn: "roleUniqueCode",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoleAssignments_Users_userUniqueCode",
                        column: x => x.userUniqueCode,
                        principalTable: "Users",
                        principalColumn: "userUniqueCode",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserRoleAssignments_roleUniqueCode",
                table: "UserRoleAssignments",
                column: "roleUniqueCode");
        }
    }
}
