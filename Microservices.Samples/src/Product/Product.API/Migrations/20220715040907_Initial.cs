using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MicroServices.Samples.Services.Product.API.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tbl_Product",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    ProductName = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: false),
                    ProductPrice = table.Column<decimal>(type: "DECIMAL(18,2)", nullable: false),
                    AvailableQuantity = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Tbl_Product_PK", x => x.ProductId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tbl_Product");
        }
    }
}
