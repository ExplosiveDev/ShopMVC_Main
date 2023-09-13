using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class fixstirage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Storage_Products_ProductId",
                table: "Storage");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                column: "Name",
                value: "Ноутбук Lenovo IdeaPad 1");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                column: "Name",
                value: "Смартфон Samsung Galaxy A24");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                column: "Name",
                value: "Смартфон Apple iPhone 14");

            migrationBuilder.AddForeignKey(
                name: "FK_Storage_Products_ProductId",
                table: "Storage",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Storage_Products_ProductId",
                table: "Storage");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                column: "Name",
                value: "Ноутбук Lenovo IdeaPad 1 15AMN7 (82VG00HHRA)");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                column: "Name",
                value: "Мобільний телефон Samsung Galaxy A24 6/128GB Black (SM-A245FZKVSEK)");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                column: "Name",
                value: "Мобільний телефон Apple iPhone 14 128GB Midnight (MPUF3RX/A)");

            migrationBuilder.AddForeignKey(
                name: "FK_Storage_Products_ProductId",
                table: "Storage",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");
        }
    }
}
