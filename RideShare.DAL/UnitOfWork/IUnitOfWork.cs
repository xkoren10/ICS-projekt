using System;
using System.Threading.Tasks;
using RideShare.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace RideShare.DAL.UnitOfWork
{

    public interface IUnitOfWork : IAsyncDisposable
    {
        IRepository<TEntity> GetRepository<TEntity>() where TEntity : class, IMainEntity;
        Task CommitAsync();
    }
}