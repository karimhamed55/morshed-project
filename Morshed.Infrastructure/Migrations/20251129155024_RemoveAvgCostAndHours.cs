using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Morshed.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveAvgCostAndHours : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvgCost",
                table: "Places");

            migrationBuilder.DropColumn(
                name: "Hours",
                table: "Places");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "AvgCost",
                table: "Places",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Hours",
                table: "Places",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
