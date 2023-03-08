using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TodoAppWeb.Migrations
{
    /// <inheritdoc />
    public partial class TodoListId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ListId",
                table: "Todo",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ListId",
                table: "Todo");
        }
    }
}
