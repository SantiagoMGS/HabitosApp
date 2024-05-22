using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using HabitosApp.Web.Data.Entities;

namespace HabitosApp.Web.Data
{
    public class DataContext : IdentityDbContext<User>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<HabitosAppRole> HabitosAppRoles { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        //public DbSet<Section> Sections { get; set; }
        public DbSet<HabitType> HabitType { get; set; }
        public DbSet<Habit> Habit { get; set; }
        public DbSet<Unit> Unit { get; set; }
        public DbSet<ViaAdmin> ViaAdmin { get; set; }
        public DbSet<Medication> Medication { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureIndexes(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        private void ConfigureIndexes(ModelBuilder modelBuilder)
        {

            // Roles
            modelBuilder.Entity<HabitosAppRole>()
                        .HasIndex(s => s.Name)
                        .IsUnique();
            // Users
            modelBuilder.Entity<User>()
                        .HasIndex(s => s.Document)
                        .IsUnique();

            // Role Permission
            modelBuilder.Entity<RolePermission>()
                        .HasKey(rs => new { rs.RoleId, rs.PermissionId });

            modelBuilder.Entity<RolePermission>()
                        .HasOne(rs => rs.Role)
                        .WithMany(r => r.RolePermissions)
                        .HasForeignKey(rs => rs.RoleId);

            modelBuilder.Entity<RolePermission>()
                        .HasOne(rs => rs.Permission)
                        .WithMany(s => s.RolePermissions)
                        .HasForeignKey(rs => rs.PermissionId);
        }
    }
}