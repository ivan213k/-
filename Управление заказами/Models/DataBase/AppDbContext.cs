using Microsoft.EntityFrameworkCore;

namespace Управление_заказами.Models.DataBase
{
    sealed class AppDbContext : DbContext
    {
        public DbSet<EquipmentInStock> EquipmentsInStock { get; set; }
        
        public DbSet<EquipmentInRent> EquipmentsInRent { get; set; }

        public DbSet<Order> OrdersHistory { get; set; }
        
        public DbSet<EquipmentFromOrder> EquipmentsFromOrder { get; set; }

        public AppDbContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EquipmentFromOrder>()
                .HasOne(e => e.Order)
                .WithMany(t => t.Equipments)
                .HasForeignKey(p => p.EquipmentFromOrderKey);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=OrderManagmentDb;Trusted_Connection=True;");
        }
    }
}
