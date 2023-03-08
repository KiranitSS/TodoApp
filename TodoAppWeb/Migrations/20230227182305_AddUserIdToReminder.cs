using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TodoAppWeb.Migrations
{
    public partial class AddUserIdToReminder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Reminders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Reminders");
        }
    }
}
