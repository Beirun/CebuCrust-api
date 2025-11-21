using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CebuCrust_api.Migrations
{
    /// <inheritdoc />
    public partial class updatePizza : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsAvailable",
                table: "Pizzas",
                newName: "Stock");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Stock",
                table: "Pizzas",
                newName: "IsAvailable");
        }
    }
}
