using Microsoft.EntityFrameworkCore.Migrations;

namespace Управление_заказами.Migrations
{
    public partial class Added_Property_Google_Calendar_ColorId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GoogleCalendarColorId",
                table: "OrdersHistory",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GoogleCalendarColorId",
                table: "OrdersHistory");
        }
    }
}
