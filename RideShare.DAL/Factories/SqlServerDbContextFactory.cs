using Microsoft.EntityFrameworkCore;

namespace RideShare.DAL.Factories
{
    public class SqlServerDbContextFactory : IDbContextFactory<RideShareDbContext>
    {
        private readonly string _connectionString;
        private readonly bool _seedDemoData;

        public SqlServerDbContextFactory(string connectionString, bool seedDemoData = false)
        {
            _connectionString = connectionString;
            _seedDemoData = seedDemoData;
        }

        public RideShareDbContext CreateDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<RideShareDbContext>();
            optionsBuilder.UseSqlServer(_connectionString);

            //optionsBuilder.LogTo(System.Console.WriteLine); //Enable in case you want to see tests details, enabled may cause some inconsistencies in tests
            //optionsBuilder.EnableSensitiveDataLogging();

            return new RideShareDbContext(optionsBuilder.Options, _seedDemoData);
        }
    }
}