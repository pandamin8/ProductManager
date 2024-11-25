using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ManufactureContact_ManufacturePhone",
                table: "Products",
                type: "nvarchar(11)",
                maxLength: 11,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ManufactureContact_ManufactureEmail",
                table: "Products",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ManufactureContact_ManufactureEmail",
                table: "Products",
                column: "ManufactureContact_ManufactureEmail",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_ManufactureContact_ManufacturePhone",
                table: "Products",
                column: "ManufactureContact_ManufacturePhone",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProduceDate",
                table: "Products",
                column: "ProduceDate",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Products_ManufactureContact_ManufactureEmail",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_ManufactureContact_ManufacturePhone",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_ProduceDate",
                table: "Products");

            migrationBuilder.AlterColumn<string>(
                name: "ManufactureContact_ManufacturePhone",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(11)",
                oldMaxLength: 11);

            migrationBuilder.AlterColumn<string>(
                name: "ManufactureContact_ManufactureEmail",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256);
        }
    }
}
