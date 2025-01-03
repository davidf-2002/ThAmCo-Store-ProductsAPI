using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ProductsAPI.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixProductSupplierRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductSuppliers",
                table: "ProductSuppliers");

            migrationBuilder.DropIndex(
                name: "IX_ProductSuppliers_ProductId",
                table: "ProductSuppliers");

            migrationBuilder.DeleteData(
                table: "ProductSuppliers",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ProductSuppliers",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ProductSuppliers",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ProductSuppliers",
                keyColumn: "Id",
                keyValue: 4);


            migrationBuilder.DropColumn(
                name: "Id",
                table: "ProductSuppliers");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "ProductSuppliers",
                type: "int",
                nullable: false);


            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductSuppliers",
                table: "ProductSuppliers",
                columns: new[] { "ProductId", "SupplierId" });

            migrationBuilder.InsertData(
                table: "ProductSuppliers",
                columns: new[] { "ProductId", "SupplierId", "Id" },
                values: new object[,]
                {
                    { 31, 1, 1 },
                    { 30, 3, 4 },
                    { 32, 2, 2 },
                    { 31, 3, 3 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductSuppliers",
                table: "ProductSuppliers");

            migrationBuilder.DeleteData(
                table: "ProductSuppliers",
                keyColumns: new[] { "ProductId", "SupplierId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "ProductSuppliers",
                keyColumns: new[] { "ProductId", "SupplierId" },
                keyValues: new object[] { 1, 3 });

            migrationBuilder.DeleteData(
                table: "ProductSuppliers",
                keyColumns: new[] { "ProductId", "SupplierId" },
                keyValues: new object[] { 2, 2 });

            migrationBuilder.DeleteData(
                table: "ProductSuppliers",
                keyColumns: new[] { "ProductId", "SupplierId" },
                keyValues: new object[] { 3, 3 });


            // Drop the modified 'Id' column
            migrationBuilder.DropColumn(
                name: "Id",
                table: "ProductSuppliers");

            // Recreate the 'Id' column with the IDENTITY property
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "ProductSuppliers",
                nullable: false,
                type: "int")
                .Annotation("SqlServer:Identity", "1, 1");


            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductSuppliers",
                table: "ProductSuppliers",
                column: "Id");

            migrationBuilder.InsertData(
                table: "ProductSuppliers",
                columns: new[] { "Id", "ProductId", "SupplierId" },
                values: new object[,]
                {
                    { 31, 1, 1 },
                    { 30, 3, 4 },
                    { 32, 2, 2 },
                    { 31, 3, 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductSuppliers_ProductId",
                table: "ProductSuppliers",
                column: "ProductId");
        }
    }
}
