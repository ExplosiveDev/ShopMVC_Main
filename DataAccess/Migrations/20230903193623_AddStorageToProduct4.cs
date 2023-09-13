using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddStorageToProduct4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Storage_Products_ProductId1",
                table: "Storage");

            migrationBuilder.RenameColumn(
                name: "ProductId1",
                table: "Storage",
                newName: "ProductsId");

            migrationBuilder.RenameIndex(
                name: "IX_Storage_ProductId1",
                table: "Storage",
                newName: "IX_Storage_ProductsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Storage_Products_ProductsId",
                table: "Storage",
                column: "ProductsId",
                principalTable: "Products",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Storage_Products_ProductsId",
                table: "Storage");

            migrationBuilder.RenameColumn(
                name: "ProductsId",
                table: "Storage",
                newName: "ProductId1");

            migrationBuilder.RenameIndex(
                name: "IX_Storage_ProductsId",
                table: "Storage",
                newName: "IX_Storage_ProductId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Storage_Products_ProductId1",
                table: "Storage",
                column: "ProductId1",
                principalTable: "Products",
                principalColumn: "Id");
        }
    }
}
