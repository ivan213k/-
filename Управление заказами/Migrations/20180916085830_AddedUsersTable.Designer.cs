﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Управление_заказами.Models.DataBase;

namespace Управление_заказами.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20180916085830_AddedUsersTable")]
    partial class AddedUsersTable
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.2-rtm-30932")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Управление_заказами.Models.DataBase.EquipmentFromOrder", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Category");

                    b.Property<int>("Count");

                    b.Property<DateTime>("EndDate");

                    b.Property<int>("EquipmentFromOrderKey");

                    b.Property<string>("Name");

                    b.Property<DateTime>("StartDate");

                    b.HasKey("Id");

                    b.HasIndex("EquipmentFromOrderKey");

                    b.ToTable("EquipmentsFromOrder");
                });

            modelBuilder.Entity("Управление_заказами.Models.DataBase.EquipmentInRent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Category");

                    b.Property<int>("Count");

                    b.Property<DateTime>("EndDate");

                    b.Property<string>("Name");

                    b.Property<DateTime>("StartDate");

                    b.HasKey("Id");

                    b.ToTable("EquipmentsInRent");
                });

            modelBuilder.Entity("Управление_заказами.Models.DataBase.EquipmentInStock", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Category");

                    b.Property<int>("Count");

                    b.Property<string>("ImageUrl");

                    b.Property<string>("Name");

                    b.Property<string>("Note");

                    b.Property<int>("TotalCount");

                    b.HasKey("Id");

                    b.ToTable("EquipmentsInStock");
                });

            modelBuilder.Entity("Управление_заказами.Models.DataBase.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Adress");

                    b.Property<DateTime>("CreateDate");

                    b.Property<string>("CustomerName");

                    b.Property<string>("EventId");

                    b.Property<string>("Manager");

                    b.Property<string>("MobilePhone");

                    b.Property<string>("Note");

                    b.Property<DateTime>("ReturnDate");

                    b.Property<string>("ReturnEventId");

                    b.Property<int>("Status");

                    b.HasKey("Id");

                    b.ToTable("OrdersHistory");
                });

            modelBuilder.Entity("Управление_заказами.Models.DataBase.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AccountType");

                    b.Property<string>("GoogleCalendarColorId");

                    b.Property<string>("Name");

                    b.Property<string>("Password");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Управление_заказами.Models.DataBase.EquipmentFromOrder", b =>
                {
                    b.HasOne("Управление_заказами.Models.DataBase.Order", "Order")
                        .WithMany("Equipments")
                        .HasForeignKey("EquipmentFromOrderKey")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
