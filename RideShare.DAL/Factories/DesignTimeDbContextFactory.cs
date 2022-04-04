using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace RideShare.DAL.Factories
{
    /// <summary>
    /// EF Core CLI migration generation uses this DbContext to create model and migration
    /// </summary>
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<RideShareDbContext>
    {
        public RideShareDbContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<RideShareDbContext> builder = new();
            builder.UseSqlServer(
                @"Data Source=(LocalDB)\MSSQLLocalDB;
                Initial Catalog = RideShare;
                MultipleActiveResultSets = True;
                Integrated Security = True; ");

            return new RideShareDbContext(builder.Options);
        }
    }
}
