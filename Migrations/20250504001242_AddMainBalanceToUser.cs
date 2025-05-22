using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonalFinanceApplication.Migrations
{
    /// <inheritdoc />
    public partial class AddMainBalanceToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "MainBalance",
                table: "Users",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MainBalance",
                table: "Users");
        }
    }
}
