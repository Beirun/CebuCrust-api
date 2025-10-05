using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CebuCrust_api.Migrations
{
    /// <inheritdoc />
    public partial class PizzaModelUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateDeleted",
                table: "Pizzas",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsAvailable",
                table: "Pizzas",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateDeleted",
                table: "Pizzas");

            migrationBuilder.DropColumn(
                name: "IsAvailable",
                table: "Pizzas");
        }
    }
}
