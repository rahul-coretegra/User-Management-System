using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace User_Management_System.Migrations
{
    public partial class AddInitial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    roleUniqueCode = table.Column<string>(type: "text", nullable: false),
                    roleId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    roleName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    roleLevel = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => x.roleUniqueCode);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    userUniqueCode = table.Column<string>(type: "text", nullable: false),
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    username = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    phoneNumber = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    address = table.Column<string>(type: "text", nullable: false),
                    password = table.Column<string>(type: "text", nullable: false),
                    isActiveUser = table.Column<int>(type: "integer", nullable: false),
                    isVerifiedEmail = table.Column<int>(type: "integer", nullable: false),
                    isVerifiedPhoneNumber = table.Column<int>(type: "integer", nullable: false),
                    token = table.Column<string>(type: "text", nullable: true),
                    createdAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.userUniqueCode);
                });

            migrationBuilder.CreateTable(
                name: "UserVerifications",
                columns: table => new
                {
                    identity = table.Column<string>(type: "text", nullable: false),
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    otp = table.Column<string>(type: "text", nullable: true),
                    otpTimeStamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserVerifications", x => x.identity);
                });

            migrationBuilder.CreateTable(
                name: "RoleAndAccess",
                columns: table => new
                {
                    roleAndAccessId = table.Column<string>(type: "text", nullable: false),
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    routePath = table.Column<string>(type: "text", nullable: false),
                    routeName = table.Column<string>(type: "text", nullable: false),
                    roleUniqueCode = table.Column<string>(type: "text", nullable: false),
                    isAccess = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleAndAccess", x => x.roleAndAccessId);
                    table.ForeignKey(
                        name: "FK_RoleAndAccess_UserRoles_roleUniqueCode",
                        column: x => x.roleUniqueCode,
                        principalTable: "UserRoles",
                        principalColumn: "roleUniqueCode",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoleAssignments",
                columns: table => new
                {
                    UserUniqueCode = table.Column<string>(type: "text", nullable: false),
                    RoleUniqueCode = table.Column<string>(type: "text", nullable: false),
                    AssignmentId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoleAssignments", x => new { x.UserUniqueCode, x.RoleUniqueCode });
                    table.ForeignKey(
                        name: "FK_UserRoleAssignments_UserRoles_RoleUniqueCode",
                        column: x => x.RoleUniqueCode,
                        principalTable: "UserRoles",
                        principalColumn: "roleUniqueCode",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoleAssignments_Users_UserUniqueCode",
                        column: x => x.UserUniqueCode,
                        principalTable: "Users",
                        principalColumn: "userUniqueCode",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RoleAndAccess_roleUniqueCode",
                table: "RoleAndAccess",
                column: "roleUniqueCode");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoleAssignments_RoleUniqueCode",
                table: "UserRoleAssignments",
                column: "RoleUniqueCode");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoleAndAccess");

            migrationBuilder.DropTable(
                name: "UserRoleAssignments");

            migrationBuilder.DropTable(
                name: "UserVerifications");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
