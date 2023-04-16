using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RealEstateAPI.Model;

namespace RealEstateAPI.Data
{
    public class ApiDbContext :  IdentityDbContext<UsersOrRealtors>
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
        {
            
        }

        public DbSet<Listings> Listings { get; set; }
        //public DbSet<UsersOrRealtors> UsersOrRealtors { get; set; }
        public DbSet<Offers> Offers { get; set; }
        public DbSet<Images> Images { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // this is to prevent circular cascade delete
            modelBuilder.Entity<UsersOrRealtors>()
                .HasMany(e => e.Offers)
                .WithOne(e => e.Owner)
                .OnDelete(DeleteBehavior.Restrict);
            base.OnModelCreating(modelBuilder);
        }
    }
    
}
