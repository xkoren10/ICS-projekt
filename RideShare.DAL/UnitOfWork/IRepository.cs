using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using RideShare.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace RideShare.DAL.UnitOfWork
{

    public interface IRepository<TEntity> where TEntity : class, IMainEntity
    {
        IQueryable<TEntity> Get();
        void Delete(Guid entityId);

        Task<TEntity> InsertOrUpdateAsync<TModel>(
            TModel model,
            IMapper mapper,
            CancellationToken cancellationToken = default) where TModel : class;
    }
}