using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EntittFramework_Project.Migrations
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
                    ProductId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Size = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "money", nullable: false),
                    Picture = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    OnSale = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductId);
                });

            migrationBuilder.CreateTable(
                name: "Sales",
                columns: table => new
                {
                    SaleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "date", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: true),
                    ProductId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sales", x => x.SaleId);
                    table.ForeignKey(
                        name: "FK_Sales_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ProductId", "OnSale", "Picture", "Price", "ProductName", "Size" },
                values: new object[,]
                {
                    { 1, true, "1.jpg", 1030.00m, "Product 1", 2 },
                    { 2, true, "2.jpg", 1462.00m, "Product 2", 4 },
                    { 3, true, "3.jpg", 1718.00m, "Product 3", 2 },
                    { 4, true, "4.jpg", 1289.00m, "Product 4", 1 },
                    { 5, true, "5.jpg", 1288.00m, "Product 5", 4 }
                });

            migrationBuilder.InsertData(
                table: "Sales",
                columns: new[] { "SaleId", "Date", "ProductId", "Quantity" },
                values: new object[,]
                {
                    { 1, new DateTime(2022, 5, 25, 15, 27, 12, 899, DateTimeKind.Local).AddTicks(4915), 1, 110 },
                    { 2, new DateTime(2023, 2, 8, 15, 27, 12, 899, DateTimeKind.Local).AddTicks(4948), 2, 152 },
                    { 3, new DateTime(2022, 9, 6, 15, 27, 12, 899, DateTimeKind.Local).AddTicks(4956), 3, 128 },
                    { 4, new DateTime(2023, 1, 9, 15, 27, 12, 899, DateTimeKind.Local).AddTicks(4964), 4, 250 },
                    { 5, new DateTime(2022, 11, 18, 15, 27, 12, 899, DateTimeKind.Local).AddTicks(4971), 5, 248 },
                    { 6, new DateTime(2022, 9, 14, 15, 27, 12, 899, DateTimeKind.Local).AddTicks(4979), 1, 232 },
                    { 7, new DateTime(2022, 7, 18, 15, 27, 12, 899, DateTimeKind.Local).AddTicks(4986), 2, 231 },
                    { 8, new DateTime(2022, 6, 30, 15, 27, 12, 899, DateTimeKind.Local).AddTicks(4993), 3, 210 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sales_ProductId",
                table: "Sales",
                column: "ProductId");
            string procInsert = @"CREATE PROC InsertProduct
                          @name NVARCHAR(50), @price MONEY, @size INT, @picture NVARCHAR(50), @onsale BIT
                     AS
                     INSERT INTO Products (ProductName, Price, Size, Picture, OnSale)
                     VALUES (@name, @price, @size, @picture, @onsale)
                     RETURN @@IDENTITY
                     GO";
            migrationBuilder.Sql(procInsert);
            string procUpdate = @"CREATE PROC UpdateProduct
                         @id INT, @name NVARCHAR(50), @price MONEY, @size INT, @picture NVARCHAR(50), @onsale BIT
                     AS
                     UPDATE Products SET ProductName=@name, Price=@price, Size=@size, Picture=@picture, OnSale=@onsale
                     WHERE ProductId = @id
                     RETURN @@ROWCOUNT
                     GO";
            migrationBuilder.Sql(procUpdate);
            string procDel = @"CREATE PROC DeleteProduct
                 @id INT
                 AS
                 DELETE Products 
                 WHERE ProductId = @id
                 RETURN @@ROWCOUNT
                 GO";
            migrationBuilder.Sql(procDel);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Sales");

            migrationBuilder.DropTable(
                name: "Products");
            migrationBuilder.Sql("DROP PROC InsertProduct");
            migrationBuilder.Sql("DROP PROC UpdateProduct");
            migrationBuilder.Sql("DROP PROC DeleteProduct");
        }
    }
}
