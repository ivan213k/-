using Microsoft.EntityFrameworkCore.Migrations;

namespace Управление_заказами.Migrations
{
    public partial class AddedPartnerNameProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPartnerEquipment",
                table: "EquipmentsFromOrder",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PartnerName",
                table: "EquipmentsFromOrder",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPartnerEquipment",
                table: "EquipmentsFromOrder");

            migrationBuilder.DropColumn(
                name: "PartnerName",
                table: "EquipmentsFromOrder");
        }
    }
}
