using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ProductsAPI.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StockStatus = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Description", "LastUpdated", "Name", "Price", "StockStatus" },
                values: new object[,]
                {
                    { 1, "Cotton white", new DateTime(2024, 12, 13, 4, 41, 38, 933, DateTimeKind.Utc).AddTicks(3343), "T-shirt", 15.99m, "In Stock" },
                    { 2, "Best for marathons", new DateTime(2024, 12, 13, 4, 41, 38, 933, DateTimeKind.Utc).AddTicks(3345), "Running Shoes", 50.00m, "In Stock" },
                    { 3, "Adjustable size", new DateTime(2024, 12, 13, 4, 41, 38, 933, DateTimeKind.Utc).AddTicks(3346), "Baseball Cap", 12.50m, "Out of Stock" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
