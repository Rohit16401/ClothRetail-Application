using RetailUI.MVVM.ViewModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Windows;
using Domain.Services.HelperServices;
using Domain.Repositories;
using Domain.Entities;
using Domain.Services.EntityServices;
using System.Diagnostics;

namespace RetailUI
{
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        //protected override void OnStartup(StartupEventArgs e)
        //{
        //    base.OnStartup(e);

        //    // Set up Dependency Injection
        //    var serviceCollection = new ServiceCollection();

        //    // Set up Configuration
        //    var configuration = new ConfigurationBuilder()
        //        .SetBasePath(AppDomain.CurrentDomain.BaseDirectory) // Set the base path for the configuration files
        //        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true) // Load the appsettings.json
        //        .Build();
        //    var sqlConnectionString = configuration["ConnectionStrings:SqlConnectionString"] ?? throw new ArgumentException("sqlConnectionString is missing.");
        //    serviceCollection.AddOptions<ConnectionStringBuilder>()
        //        .Configure<IConfiguration>((settings, configuration) => {
        //            settings.SqlConnectionString = sqlConnectionString;
        //        });

        //    serviceCollection.AddSingleton<IConnectionStringBuilder>(sp => sp.GetRequiredService<IOptions<ConnectionStringBuilder>>().Value);



        //    // Register your services (repositories, ViewModels, etc.)
        //    serviceCollection.AddSingleton<ISQLGenericRepository<Items>, SQLGenericRepository<Items>>();
        //    serviceCollection.AddTransient<AddItemViewModel>(); // Register ViewModel as transient or singleton

        //    // Register the MainWindow, including the ViewModel it depends on
        //    serviceCollection.AddTransient<MainWindow>();

        //    // Build the service provider
        //    ServiceProvider = serviceCollection.BuildServiceProvider();

        //    // Resolve and show the MainWindow
        //    //var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
        //    //mainWindow.Show();
        //}

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var serviceCollection = new ServiceCollection();

            // Configuration setup
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var sqlConnectionString = configuration["ConnectionStrings:SqlConnectionString"]
                ?? throw new ArgumentException("sqlConnectionString is missing.");

            serviceCollection.AddOptions<ConnectionStringBuilder>()
                .Configure<IConfiguration>((settings, configuration) =>
                {
                    settings.SqlConnectionString = sqlConnectionString;
                    Debug.WriteLine(settings.SqlConnectionString);
                });

            serviceCollection.AddSingleton<IConnectionStringBuilder>(sp =>
                sp.GetRequiredService<IOptions<ConnectionStringBuilder>>().Value);

            // Ensure the order of registration is correct
            serviceCollection.AddTransient<ISQLGenericRepository<Items>, SQLGenericRepository<Items>>();
            serviceCollection.AddTransient<AddItemViewModel>();
            serviceCollection.AddTransient<MainWindow>();

            ServiceProvider = serviceCollection.BuildServiceProvider();

           // var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            //mainWindow.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            // Dispose of the ServiceProvider if necessary (good practice)
            if (ServiceProvider is IDisposable)
            {
                ((IDisposable)ServiceProvider).Dispose();
            }
            base.OnExit(e);
        }
    }
}
