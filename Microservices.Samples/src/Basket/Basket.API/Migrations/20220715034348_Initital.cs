using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Basket.API.Migrations
{
    public partial class Initital : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tbl_CustomerBasket",
                columns: table => new
                {
                    CustomerId = table.Column<string>(type: "NVARCHAR2(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Tbl_CustomerBasket_Id", x => x.CustomerId);
                });

            migrationBuilder.CreateTable(
                name: "Tbl_BasketItem",
                columns: table => new
                {
                    BasketItemId = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    ProductId = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    ProductName = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    Quantity = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    Status = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    CustomerBasketId = table.Column<string>(type: "NVARCHAR2(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Tbl_BasketItem_Pk", x => x.BasketItemId);
                    table.ForeignKey(
                        name: "FK_Tbl_BasketItem_Tbl_CustomerBasket_CustomerBasketId",
                        column: x => x.CustomerBasketId,
                        principalTable: "Tbl_CustomerBasket",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tbl_BasketItem_CustomerBasketId",
                table: "Tbl_BasketItem",
                column: "CustomerBasketId");

            migrationBuilder.CreateIndex(
                name: "IX_Tbl_BasketItem_ProductId",
                table: "Tbl_BasketItem",
                column: "ProductId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tbl_BasketItem");

            migrationBuilder.DropTable(
                name: "Tbl_CustomerBasket");
        }
    }
}
