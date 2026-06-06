using Microsoft.EntityFrameworkCore;
using inventory_of_equipment_in_classrooms.Models;
using inventory_of_equipment_in_classrooms.Data;

namespace inventory_of_equipment_in_classrooms.Data
{
    public class DatabaseContent : DbContext
    {
        public DbSet<JobTitle> JobTitles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<ItemCategory> ItemCategories { get; set; }
        public DbSet<InventoryItem> InventoryItems { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<DocumentItem> DocumentItems { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql("Host=245e1-rw.db.pub.dbaas.postgrespro.ru;Database=dbdiploma;Username=savchenko_dm;Password=rW#3bsb7i0t");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("savchenko_dm");

            modelBuilder.Entity<JobTitle>().HasKey(j => j.Id);
            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<Room>().HasKey(r => r.Id);
            modelBuilder.Entity<InventoryItem>().HasKey(i => i.Id);

            modelBuilder.Entity<JobTitle>().ToTable("job_title");
            modelBuilder.Entity<User>().ToTable("user");
            modelBuilder.Entity<Room>().ToTable("room");
            modelBuilder.Entity<InventoryItem>().ToTable("inventory_item");

            modelBuilder.Entity<InventoryItem>()
                .Property(i => i.DateOnAccounting)
                .HasColumnName("date_on_accounting");


            modelBuilder.Entity<InventoryItem>()
                .HasOne(i => i.Custodian)
                .WithMany()
                .HasForeignKey(i => i.CustodianId);

            modelBuilder.Entity<InventoryItem>()
                .HasOne(i => i.Room)
                .WithMany()
                .HasForeignKey(i => i.RoomId);
            modelBuilder.Entity<Room>()
                .HasOne(r => r.Teacher)
                .WithMany()
                .HasForeignKey(r => r.TeacherId);
        }

        private static DatabaseContent _context;
        public static DatabaseContent GetContext()
        {
            if (_context == null) _context = new DatabaseContent();
            return _context;
        }
    }
}