using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CebuCrust_api.Migrations
{
    /// <inheritdoc />
    public partial class softDeletions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateDeleted",
                table: "Orders",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateDeleted",
                table: "Orders");
        }
    }
}
