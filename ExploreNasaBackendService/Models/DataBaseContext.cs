using Microsoft.EntityFrameworkCore;

namespace ExploreNasaBackendService.Models
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options)
        {
        }

        public DbSet<ApodWithId> TodayApod { get; set; } = null!;
    }
}
