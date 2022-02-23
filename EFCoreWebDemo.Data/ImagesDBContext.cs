
using Microsoft.EntityFrameworkCore;

namespace ImagesEF.Data
{
    public class ImagesDBContext : DbContext
    {
        private readonly string _connectionString;
        public ImagesDBContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }

        public DbSet<Image> Images { get; set; }
    }
}
