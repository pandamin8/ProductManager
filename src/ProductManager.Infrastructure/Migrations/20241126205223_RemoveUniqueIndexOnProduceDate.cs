using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUniqueIndexOnProduceDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Products_ProduceDate",
                table: "Products");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Products_ProduceDate",
                table: "Products",
                column: "ProduceDate",
                unique: true);
        }
    }
}
