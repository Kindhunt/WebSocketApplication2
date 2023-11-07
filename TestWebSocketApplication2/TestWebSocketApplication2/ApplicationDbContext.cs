using Microsoft.EntityFrameworkCore;
using TestWebSocketApplication2.Models;

namespace TestWebSocketApplication2
{
    public class ApplicationDbContext : DbContext {
        public DbSet<User> Users { get; set; }

        public ApplicationDbContext() { }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);
        }
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
			optionsBuilder.UseSqlServer(System.Configuration.ConfigurationManager.AppSettings["DatabaseConnection"]);
		}
	}
}
