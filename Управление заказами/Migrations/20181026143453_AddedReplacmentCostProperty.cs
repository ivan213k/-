using Microsoft.EntityFrameworkCore.Migrations;

namespace Управление_заказами.Migrations
{
    public partial class AddedReplacmentCostProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ReplacmentCost",
                table: "EquipmentsInStock",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ReplacmentCost",
                table: "EquipmentsInRent",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ReplacmentCost",
                table: "EquipmentsFromOrder",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReplacmentCost",
                table: "EquipmentsInStock");

            migrationBuilder.DropColumn(
                name: "ReplacmentCost",
                table: "EquipmentsInRent");

            migrationBuilder.DropColumn(
                name: "ReplacmentCost",
                table: "EquipmentsFromOrder");
        }
    }
}
