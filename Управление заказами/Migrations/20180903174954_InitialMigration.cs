using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Управление_заказами.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EquipmentsInRent",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Category = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Count = table.Column<int>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentsInRent", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EquipmentsInStock",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Category = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Count = table.Column<int>(nullable: false),
                    ImageUrl = table.Column<string>(nullable: true),
                    TotalCount = table.Column<int>(nullable: false),
                    Note = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentsInStock", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrdersHistory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    ReturnDate = table.Column<DateTime>(nullable: false),
                    CustomerName = table.Column<string>(nullable: true),
                    Manager = table.Column<string>(nullable: true),
                    MobilePhone = table.Column<string>(nullable: true),
                    Adress = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Note = table.Column<string>(nullable: true),
                    EventId = table.Column<string>(nullable: true),
                    ReturnEventId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrdersHistory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EquipmentsFromOrder",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Category = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Count = table.Column<int>(nullable: false),
                    EquipmentFromOrderKey = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentsFromOrder", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipmentsFromOrder_OrdersHistory_EquipmentFromOrderKey",
                        column: x => x.EquipmentFromOrderKey,
                        principalTable: "OrdersHistory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentsFromOrder_EquipmentFromOrderKey",
                table: "EquipmentsFromOrder",
                column: "EquipmentFromOrderKey");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EquipmentsFromOrder");

            migrationBuilder.DropTable(
                name: "EquipmentsInRent");

            migrationBuilder.DropTable(
                name: "EquipmentsInStock");

            migrationBuilder.DropTable(
                name: "OrdersHistory");
        }
    }
}
