using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace User_Management_System.Migrations.PostgreSqlApplicationDb
{
    public partial class UpdateconfigureServicemodel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConfigureServices_Services_ServiceUniqueId",
                table: "ConfigureServices");

            migrationBuilder.RenameColumn(
                name: "ServiceUniqueId",
                table: "ConfigureServices",
                newName: "ServiceId");

            migrationBuilder.RenameIndex(
                name: "IX_ConfigureServices_ServiceUniqueId",
                table: "ConfigureServices",
                newName: "IX_ConfigureServices_ServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_ConfigureServices_Services_ServiceId",
                table: "ConfigureServices",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "ServiceId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConfigureServices_Services_ServiceId",
                table: "ConfigureServices");

            migrationBuilder.RenameColumn(
                name: "ServiceId",
                table: "ConfigureServices",
                newName: "ServiceUniqueId");

            migrationBuilder.RenameIndex(
                name: "IX_ConfigureServices_ServiceId",
                table: "ConfigureServices",
                newName: "IX_ConfigureServices_ServiceUniqueId");

            migrationBuilder.AddForeignKey(
                name: "FK_ConfigureServices_Services_ServiceUniqueId",
                table: "ConfigureServices",
                column: "ServiceUniqueId",
                principalTable: "Services",
                principalColumn: "ServiceId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
