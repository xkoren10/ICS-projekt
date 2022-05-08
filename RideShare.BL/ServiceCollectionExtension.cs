using AutoMapper;
using AutoMapper.EquivalencyExpression;
using RideShare.BL.Facades;
using RideShare.DAL;
using RideShare.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace RideShare.BL
{

    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddBLServices(this IServiceCollection services)
        {
            services.AddSingleton<IUnitOfWorkFactory, UnitOfWorkFactory>();
            services.AddSingleton<UserFacade>();
            services.AddSingleton<CarFacade>();
            services.AddSingleton<RideFacade>();
            services.AddSingleton<RideUserFacade>();

            services.AddAutoMapper((serviceProvider, cfg) =>
            {
                cfg.AddCollectionMappers();

                var dbContextFactory = serviceProvider.GetRequiredService<IDbContextFactory<RideShareDbContext>>();
                using var dbContext = dbContextFactory.CreateDbContext();
                cfg.UseEntityFrameworkCoreModel<RideShareDbContext>(dbContext.Model);
            }, typeof(BusinessLogic).Assembly);
            return services;
        }
    }
}