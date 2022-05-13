using RideShare.App.Services;
using RideShare.App.Services.MessageDialog;
using RideShare.App.ViewModels;
using RideShare.App.Views;
using RideShare.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Globalization;
using System.Threading;
using System.Windows;
using RideShare.App.Factories;
using RideShare.BL;
using RideShare.BL.Facades;
using RideShare.DAL.Factories;
using RideShare.DAL.UnitOfWork;
using Microsoft.Extensions.Options;
using RideShare.App.Settings;
using RideShare.App.Extensions;

namespace RideShare.App
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IHost _host;
        public App()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("cs");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("cs");

            _host = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration(ConfigureAppConfiguration)
                .ConfigureServices((context, services) => { ConfigureServices(context.Configuration, services); })
                .Build();
        }

        private static void ConfigureAppConfiguration(HostBuilderContext context, IConfigurationBuilder builder)
        {
            builder.AddJsonFile(@"appsettings.json", false, false);
        }

        private static void ConfigureServices(IConfiguration configuration,
           IServiceCollection services)
        {
            services.AddBLServices();

            services.Configure<DALSettings>(configuration.GetSection("RideShare:DAL"));

            services.AddSingleton<IDbContextFactory<RideShareDbContext>>(provider =>
            {
                var dalSettings = provider.GetRequiredService<IOptions<DALSettings>>().Value;
                return new SqlServerDbContextFactory(dalSettings.ConnectionString!, dalSettings.SkipMigrationAndSeedDemoData);
            });

            services.AddSingleton<MainWindow>();

            services.AddSingleton<IMessageDialogService, MessageDialogService>();
            services.AddSingleton<IMediator, Mediator>();

            services.AddSingleton<MainViewModel>();
            //services.AddSingleton<IProfileViewModel, ProfileViewModel>();
            services.AddFactory<INewRideViewModel, NewRideViewModel>();
            services.AddFactory<ILogScreenViewModel, LogScreenViewModel>();
            services.AddFactory<IProfileViewModel, ProfileViewModel>();
            services.AddFactory<IMainAreaViewModel, MainAreaViewModel>();
            services.AddFactory<IRidesListViewModel, RidesListViewModel>();
            services.AddFactory<INewUserViewModel, NewUserViewModel>();
        }
        protected override async void OnStartup(StartupEventArgs e)
        {
            await _host.StartAsync();

            var dbContextFactory = _host.Services.GetRequiredService<IDbContextFactory<RideShareDbContext>>();

            var dalSettings = _host.Services.GetRequiredService<IOptions<DALSettings>>().Value;

            await using (var dbx = dbContextFactory.CreateDbContext())
            {
                if (dalSettings.SkipMigrationAndSeedDemoData)
                {
                    await dbx.Database.EnsureDeletedAsync();
                    await dbx.Database.EnsureCreatedAsync();
                }
                else
                {
                    await dbx.Database.MigrateAsync();
                }
            }

            var mainWindow = _host.Services.GetRequiredService<MainWindow>();
            mainWindow.Show();

            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            using (_host)
            {
                await _host.StopAsync(TimeSpan.FromSeconds(5));
            }

            base.OnExit(e);
        }

    }
}
