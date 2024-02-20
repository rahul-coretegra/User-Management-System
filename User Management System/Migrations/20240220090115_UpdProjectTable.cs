using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace User_Management_System.Migrations
{
    public partial class UpdProjectTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IsDatabaseExists",
                table: "Projects",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDatabaseExists",
                table: "Projects");
        }
    }
}
