using System;
using Microsoft.EntityFrameworkCore;
using RideShare.DAL;

namespace RideShare.DAL.UnitOfWork
{

    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        private readonly IDbContextFactory<RideShareDbContext> _dbContextFactory;

        public UnitOfWorkFactory(IDbContextFactory<RideShareDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public IUnitOfWork Create() => new UnitOfWork(_dbContextFactory.CreateDbContext());
    }
}