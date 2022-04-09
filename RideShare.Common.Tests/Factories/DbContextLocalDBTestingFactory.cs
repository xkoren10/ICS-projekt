using RideShare.DAL;
using Microsoft.EntityFrameworkCore;

namespace RideShare.Common.Tests.Factories
{
    public class DbContextLocalDBTestingFactory : IDbContextFactory<RideShareDbContext>
    {
        private readonly string _databaseName;
        private readonly bool _seedTestingData;

        public DbContextLocalDBTestingFactory(string databaseName, bool seedTestingData = false)
        {
            _databaseName = databaseName;
            _seedTestingData = seedTestingData;
        }
        public RideShareDbContext CreateDbContext()
        {
            DbContextOptionsBuilder<RideShareDbContext> builder = new();
            builder.UseSqlServer($"Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog = {_databaseName};MultipleActiveResultSets = True;Integrated Security = True; ");

            // contextOptionsBuilder.LogTo(System.Console.WriteLine); //Enable in case you want to see tests details, enabled may cause some inconsistencies in tests
            // builder.EnableSensitiveDataLogging();

            return new RideShareTestingDbContext(builder.Options, _seedTestingData);
        }
    }
}