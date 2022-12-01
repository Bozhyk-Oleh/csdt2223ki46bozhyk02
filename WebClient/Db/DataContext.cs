using Microsoft.EntityFrameworkCore;

namespace WebClient.Db
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options) 
        {
        }

        public DbSet<Model> Models { get; set; }
    }
}
