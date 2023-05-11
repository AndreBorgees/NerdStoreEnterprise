using Microsoft.EntityFrameworkCore.Migrations;

namespace NSE.Carrinho.API.Migrations
{
    public partial class Voucher : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Discount",
                table: "CartCustomer",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "DiscountType",
                table: "CartCustomer",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "DiscountValue",
                table: "CartCustomer",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Percentage",
                table: "CartCustomer",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "UsedVoucher",
                table: "CartCustomer",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "VoucherCode",
                table: "CartCustomer",
                type: "varchar(50)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discount",
                table: "CartCustomer");

            migrationBuilder.DropColumn(
                name: "DiscountType",
                table: "CartCustomer");

            migrationBuilder.DropColumn(
                name: "DiscountValue",
                table: "CartCustomer");

            migrationBuilder.DropColumn(
                name: "Percentage",
                table: "CartCustomer");

            migrationBuilder.DropColumn(
                name: "UsedVoucher",
                table: "CartCustomer");

            migrationBuilder.DropColumn(
                name: "VoucherCode",
                table: "CartCustomer");
        }
    }
}
