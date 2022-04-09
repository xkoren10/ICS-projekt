using RideShare.DAL.Seeds;
using RideShare.DAL;
using Microsoft.EntityFrameworkCore;

namespace RideShare.Common.Tests
{
    public class RideShareTestingDbContext : RideShareDbContext
    {
        private readonly bool _seedTestingData;

        public RideShareTestingDbContext(DbContextOptions contextOptions, bool seedTestingData = false)
            : base(contextOptions, seedDemoData:false)
        {
            _seedTestingData = seedTestingData;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            if (_seedTestingData)
            {
                CarSeeds.Seed(modelBuilder);
                UserSeeds.Seed(modelBuilder);
                RideSeeds.Seed(modelBuilder);
            }
        }
    }
}
