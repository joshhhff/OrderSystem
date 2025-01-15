using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderSystemApp.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProductTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TaxRateDecimal",
                table: "Product");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "TaxRateDecimal",
                table: "Product",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
