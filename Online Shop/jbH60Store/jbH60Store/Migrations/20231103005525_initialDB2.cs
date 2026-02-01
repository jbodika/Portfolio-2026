using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace jbH60Store.Migrations
{
    public partial class initialDB2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    CustomerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Province = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreditCard = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.CustomerId);
                });

            migrationBuilder.CreateTable(
                name: "ProductCategory",
                columns: table => new
                {
                    CategoryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProdCat = table.Column<string>(type: "varchar(60)", unicode: false, maxLength: 60, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCategory", x => x.CategoryID);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    OrderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateFufilled = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Taxes = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_Orders_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShoppingCarts",
                columns: table => new
                {
                    CartId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingCarts", x => x.CartId);
                    table.ForeignKey(
                        name: "FK_ShoppingCarts_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    ProductID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProdCatId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "varchar(80)", unicode: false, maxLength: 80, nullable: false),
                    Manufacturer = table.Column<string>(type: "varchar(80)", unicode: false, maxLength: 80, nullable: false),
                    Stock = table.Column<int>(type: "int", nullable: false),
                    BuyPrice = table.Column<decimal>(type: "numeric(8,2)", nullable: false),
                    SellPrice = table.Column<decimal>(type: "numeric(8,2)", nullable: true),
                    EmployeeNotes = table.Column<string>(type: "varchar(80)", unicode: false, maxLength: 80, nullable: true),
                    Image = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.ProductID);
                    table.ForeignKey(
                        name: "FK_Product_ProductCategory",
                        column: x => x.ProdCatId,
                        principalTable: "ProductCategory",
                        principalColumn: "CategoryID");
                });

            migrationBuilder.CreateTable(
                name: "CartItems",
                columns: table => new
                {
                    CartItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CartId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItems", x => x.CartItemId);
                    table.ForeignKey(
                        name: "FK_CartItem_ShoppingCart_CartId",
                        column: x => x.CartId,
                        principalTable: "ShoppingCarts",
                        principalColumn: "CartId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartItems_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    OrderItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.OrderItemId);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderItems_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "CustomerId", "CreditCard", "Email", "FirstName", "LastName", "PhoneNumber", "Province" },
                values: new object[,]
                {
                    { 1, "4519 4383 3829 4738", "joelle.cunningham@gmail.com", "Joelle", "Cunningham", "613-478-4783", "ON" },
                    { 2, "4378 4367 2143 0329", "brandie.lucas@yahoo.com", "Brandie", "Lucas", "819-478-3923", "QC" },
                    { 3, "3267 2173 4398 4388", "t.pena@outlook.com", "Troy", "Pena", "825-372-1273", "AB" }
                });

            migrationBuilder.InsertData(
                table: "ProductCategory",
                columns: new[] { "CategoryID", "ProdCat" },
                values: new object[,]
                {
                    { 1, "Skin Care" },
                    { 2, "Hair Care" },
                    { 3, "Fragrances" },
                    { 4, "Makeup" },
                    { 5, "Tools and Brushes" }
                });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "OrderId", "CustomerId", "DateCreated", "DateFufilled", "Taxes", "Total" },
                values: new object[] { 1, 1, new DateTime(2023, 10, 23, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2023, 10, 28, 0, 0, 0, 0, DateTimeKind.Local), 12.88m, 86m });

            migrationBuilder.InsertData(
                table: "Product",
                columns: new[] { "ProductID", "BuyPrice", "Description", "EmployeeNotes", "Image", "Manufacturer", "ProdCatId", "SellPrice", "Stock" },
                values: new object[,]
                {
                    { 1, 10m, "Radiant Glow Serum", null, null, "Lux Beauty", 1, 25m, 100 },
                    { 2, 12m, "Hyaluronic Acid Moisturizer", null, null, "Pure Essentials", 1, 28m, 200 },
                    { 3, 9m, "Charcoal Detoxifying Mask", "Newly added product", null, "Zen Skin Solutions", 1, 22m, 130 },
                    { 4, 8m, "Vitamin C Brightening Toner", null, null, "GlowBunny Cosmetics", 1, 25m, 180 },
                    { 5, 15m, "Midnight Orchid Eau de Parfum", null, null, "Lux Beauty", 3, 40m, 75 },
                    { 6, 5m, "Velvet Matte Lipstick", "Best Selling Product!", null, "Lux Beauty", 4, 15m, 120 },
                    { 7, 25m, "Jade Roller and Gua Sha Set", null, null, "Zen Skin Solutions", 5, 56m, 160 },
                    { 8, 7m, "HD Foundation Primer", null, null, "Pure Essentials", 4, 20m, 150 },
                    { 9, 13m, "Metallic Eyeshadow Palette", null, null, "GlowBunny Cosmetics", 4, 32m, 110 },
                    { 10, 16m, "Vanilla Spice Perfume Oil", null, null, "GlowBunny Cosmetics", 3, 38m, 0 },
                    { 11, 14m, "Ocean Breeze Body Mist", null, null, "Zen Skin Solutions", 3, 32m, 80 },
                    { 12, 6m, "Waterproof Mascara", null, null, "Zen Skin Solutions", 4, 18m, 140 },
                    { 13, 18m, "Citrus Sunshine Cologne", null, null, "Pure Essentials", 3, 45m, 92 },
                    { 14, 125m, "Ionic Hair Straightener", null, null, "Pure Essentials", 5, 300m, 90 },
                    { 15, 24m, "Professional Makeup Brush Set", null, null, "Lux Beauty", 5, 49m, 50 },
                    { 16, 7m, "Precision Tweezers", null, null, "Lux Beauty", 5, 15m, 75 },
                    { 17, 9m, "Keratin Hair Mask", null, null, "Zen Skin Solutions", 2, 22m, 160 },
                    { 18, 10m, "Coconut Milk Conditioner", null, null, "Pure Essentials", 2, 24m, 180 },
                    { 19, 8m, "Argan Oil Shampoo", null, null, "Lux Beauty", 2, 20m, 150 },
                    { 20, 8m, "Glow Bomb", null, null, "GlowBunny Cosmetics", 4, 20m, 0 },
                    { 21, 10m, "Curl Defining Cream", null, null, "GlowBunny Cosmetics", 2, 26m, 130 }
                });

            migrationBuilder.InsertData(
                table: "ShoppingCarts",
                columns: new[] { "CartId", "CustomerId", "DateCreated" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2023, 10, 23, 0, 0, 0, 0, DateTimeKind.Local) },
                    { 2, 2, new DateTime(2023, 10, 28, 0, 0, 0, 0, DateTimeKind.Local) }
                });

            migrationBuilder.InsertData(
                table: "CartItems",
                columns: new[] { "CartItemId", "CartId", "Price", "ProductId", "Quantity" },
                values: new object[,]
                {
                    { 1, 1, 40m, 5, 1 },
                    { 2, 1, 20m, 8, 2 },
                    { 3, 1, 25m, 4, 1 },
                    { 4, 2, 49m, 15, 2 },
                    { 5, 2, 22m, 17, 1 }
                });

            migrationBuilder.InsertData(
                table: "OrderItems",
                columns: new[] { "OrderItemId", "OrderId", "Price", "ProductId", "Quantity" },
                values: new object[,]
                {
                    { 1, 1, 40m, 5, 1 },
                    { 2, 1, 20m, 8, 2 },
                    { 3, 1, 25m, 4, 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_CartId",
                table: "CartItems",
                column: "CartId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_ProductId",
                table: "CartItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_ProductId",
                table: "OrderItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerId",
                table: "Orders",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_ProdCatId",
                table: "Product",
                column: "ProdCatId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCarts_CustomerId",
                table: "ShoppingCarts",
                column: "CustomerId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartItems");

            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "ShoppingCarts");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "ProductCategory");
        }
    }
}
