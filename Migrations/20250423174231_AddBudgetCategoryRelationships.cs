using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonalFinanceApplication.Migrations
{
    /// <inheritdoc />
    public partial class AddBudgetCategoryRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BudgetCategoryId1",
                table: "Transactions",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_BudgetCategoryId1",
                table: "Transactions",
                column: "BudgetCategoryId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_BudgetCategories_BudgetCategoryId1",
                table: "Transactions",
                column: "BudgetCategoryId1",
                principalTable: "BudgetCategories",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_BudgetCategories_BudgetCategoryId1",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_BudgetCategoryId1",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "BudgetCategoryId1",
                table: "Transactions");
        }
    }
}
