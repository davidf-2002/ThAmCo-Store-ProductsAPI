using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ProductsAPI.Data.Migrations
{
    /// <inheritdoc />
    public partial class UnfuckUpMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CategoryId", "LastUpdated", "StockLevel" },
                values: new object[] { 1, new DateTime(2025, 1, 2, 19, 7, 48, 153, DateTimeKind.Local).AddTicks(585), 2 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CategoryId", "LastUpdated", "StockLevel" },
                values: new object[] { 2, new DateTime(2025, 1, 2, 19, 7, 48, 153, DateTimeKind.Local).AddTicks(626), 7 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CategoryId", "LastUpdated", "StockLevel" },
                values: new object[] { 3, new DateTime(2025, 1, 2, 19, 7, 48, 153, DateTimeKind.Local).AddTicks(628), 0 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "StockStatus",
                value: "In Stock");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                column: "StockStatus",
                value: "In Stock");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                column: "StockStatus",
                value: "Out of Stock");
        }
    }
}
