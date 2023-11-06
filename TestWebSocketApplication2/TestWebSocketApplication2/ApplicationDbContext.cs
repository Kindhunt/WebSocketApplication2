using Microsoft.EntityFrameworkCore;
using TestWebSocketApplication2.Models;

namespace TestWebSocketApplication2
{
    public class ApplicationDbContext : DbContext {
        public class UserDB : User
        {
            public int Id { get; set; }
            public bool IsAuth { get; set; }

            public UserDB() { }
        }
        public DbSet<UserDB> Users { get; set; }

        public ApplicationDbContext() { }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);
        }
    }
}
