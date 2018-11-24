using Microsoft.EntityFrameworkCore.Migrations;

namespace Управление_заказами.Migrations
{
    public partial class AddedAllDayEventsProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AllDayEventId",
                table: "OrdersHistory",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsAllDayEvent",
                table: "OrdersHistory",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AllDayEventId",
                table: "OrdersHistory");

            migrationBuilder.DropColumn(
                name: "IsAllDayEvent",
                table: "OrdersHistory");
        }
    }
}
