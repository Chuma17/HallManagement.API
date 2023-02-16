using HallManagementTest2.Models;
using Microsoft.EntityFrameworkCore;

namespace HallManagementTest2.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //one hall to one hall admin
            //builder.Entity<Hall>()
            //    .HasOne(h => h.HallAdmin)
            //    .WithOne(ha => ha.Hall)
            //    .HasForeignKey<HallAdmin>(ha => ha.HallId);

            ////many students to one hall
            //builder.Entity<Student>()
            //    .HasOne(s => s.Hall)
            //    .WithMany(ha => ha.Students)
            //    .HasForeignKey(ha => ha.HallId);

            ////many porters to one hall
            //builder.Entity<Porter>()
            //    .HasOne(p => p.Hall)
            //    .WithMany(ha => ha.Porters)
            //    .HasForeignKey(ha => ha.HallId);

            ////many students to one room
            //builder.Entity<Student>()
            //    .HasOne(s => s.Room)
            //    .WithMany(r => r.Students)
            //    .HasForeignKey(s => s.RoomId);

            //////many rooms to one hall
            ////builder.Entity<Room>()
            ////    .HasOne(r => r.Hall)
            ////    .WithMany(h => h.Rooms)
            ////    .HasForeignKey(r => r.HallId);                               

            ////many student devices to one student
            //builder.Entity<StudentDevice>()
            //    .HasOne(s => s.Student)
            //    .WithMany(sd => sd.StudentDevices)
            //    .HasForeignKey(sd => sd.StudentId);

            ////many student devices to one hall
            //builder.Entity<StudentDevice>()
            //    .HasOne(s => s.Hall)
            //    .WithMany(sd => sd.StudentDevices)
            //    .HasForeignKey(sd => sd.HallId);
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<HallAdmin> HallAdmins { get; set; }
        public DbSet<ChiefHallAdmin> ChiefHallAdmins { get; set; }
        public DbSet<Porter> Porters { get; set; }
        public DbSet<Hall> Halls { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Block> Blocks { get; set; }
        public DbSet<StudentDevice> StudentDevices { get; set; }
        public DbSet<HallType> HallTypes { get; set; }
        public DbSet<ComplaintForm> ComplaintForms { get; set; }

    }
}
