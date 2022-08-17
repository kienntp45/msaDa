using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ordering.API.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tbl_Customer",
                columns: table => new
                {
                    CustomerId = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    IdentityId = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CustomerName = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CustomerPhoneNumber = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Tbl_CustomerId_PK", x => x.CustomerId);
                });

            migrationBuilder.CreateTable(
                name: "Tbl_Order",
                columns: table => new
                {
                    OrderId = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    CustomerId = table.Column<string>(type: "NVARCHAR2(450)", nullable: true),
                    Street = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    City = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    District = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    AdditionalAddress = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Tbl_OrderId_PK", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_Tbl_Order_Tbl_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Tbl_Customer",
                        principalColumn: "CustomerId");
                });

            migrationBuilder.CreateTable(
                name: "Tbl_OrderItem",
                columns: table => new
                {
                    OrderItemId = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    ProductName = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ProductId = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    Quantity = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    OrderId = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    OrderId1 = table.Column<string>(type: "NVARCHAR2(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tbl_OrderItem", x => x.OrderItemId);
                    table.ForeignKey(
                        name: "FK_Tbl_OrderItem_Tbl_Order_OrderId1",
                        column: x => x.OrderId1,
                        principalTable: "Tbl_Order",
                        principalColumn: "OrderId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tbl_Order_CustomerId",
                table: "Tbl_Order",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Tbl_OrderItem_OrderId1",
                table: "Tbl_OrderItem",
                column: "OrderId1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tbl_OrderItem");

            migrationBuilder.DropTable(
                name: "Tbl_Order");

            migrationBuilder.DropTable(
                name: "Tbl_Customer");
        }
    }
}
