using Microsoft.EntityFrameworkCore.Migrations;

namespace NSE.Carrinho.API.Migrations
{
    public partial class CarrinhoCascade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_CartCustomer_CartId",
                table: "CartItems");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_CartCustomer_CartId",
                table: "CartItems",
                column: "CartId",
                principalTable: "CartCustomer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_CartCustomer_CartId",
                table: "CartItems");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_CartCustomer_CartId",
                table: "CartItems",
                column: "CartId",
                principalTable: "CartCustomer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
