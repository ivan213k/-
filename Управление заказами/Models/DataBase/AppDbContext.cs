using Microsoft.EntityFrameworkCore;

namespace Управление_заказами.Models.DataBase
{
    sealed class AppDbContext : DbContext
    {
        public DbSet<Equipment> AvalibleEquipments { get; set; }

        public AppDbContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=OrderManagmentDb;Trusted_Connection=True;");
        }
    }
}
