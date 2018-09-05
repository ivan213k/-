using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Управление_заказами.Migrations
{
    public partial class AddedDateProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "EquipmentsFromOrder",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "EquipmentsFromOrder",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "EquipmentsFromOrder");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "EquipmentsFromOrder");
        }
    }
}
