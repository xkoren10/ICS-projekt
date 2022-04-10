
using RideShare.DAL;
using Microsoft.EntityFrameworkCore;


namespace RideShare.Common.Tests.Factories
{
    public class DbContextTestingInMemoryFactory: IDbContextFactory<RideShareDbContext>
    {
        private readonly string _databaseName;
        private readonly bool _seedTestingData;

        public DbContextTestingInMemoryFactory(string databaseName, bool seedTestingData = false)
        {
            _databaseName = databaseName;
            _seedTestingData = seedTestingData;
        }

        public RideShareDbContext CreateDbContext()
        {
            DbContextOptionsBuilder<RideShareDbContext> contextOptionsBuilder = new();
            contextOptionsBuilder.UseInMemoryDatabase(_databaseName);
            
            // contextOptionsBuilder.LogTo(System.Console.WriteLine); //Enable in case you want to see tests details, enabled may cause some inconsistencies in tests
            // builder.EnableSensitiveDataLogging();
            
            return new RideShareTestingDbContext(contextOptionsBuilder.Options, _seedTestingData);
        }
    }
}