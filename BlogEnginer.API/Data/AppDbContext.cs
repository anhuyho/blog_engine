using BlogEnginer.API.Entites;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
namespace BlogEnginer.API.Data
{
    //public interface IDbContext
    //{
    //}
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
           : base(options)
        {
        }
        public DbSet<Post> Posts { get; set; }
    }
    public class SQLiteDbContext : IdentityDbContext
    {
        public SQLiteDbContext(DbContextOptions<SQLiteDbContext> options)
           : base(options)
        {
        }
        public DbSet<Post> Posts { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=MyDatabase.db");
        }
    }
}
