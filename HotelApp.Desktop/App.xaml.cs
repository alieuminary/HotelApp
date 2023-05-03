using HotelAppLibrary.Data;
using HotelAppLibrary.Databases;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace HotelApp.Desktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        // Set up dependency injection in this file
        // this class gets called first before WPF main window launches.
        // Configure settings here.

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var services = new ServiceCollection(); // obj from microsoft.extensions.dependencyinjection
            services.AddTransient<MainWindow>(); // adding main window to dependency injection; always creates a new instance
            services.AddTransient<IDatabaseData, SqlData>();
            services.AddTransient<ISqlDataAccess, SqlDataAccess>();

            // Set up configuration - tinkering with the settings to read from appsettings.json file.
            var builder = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json");

            // Build an IConfiguration of the appsettings.json
            IConfiguration config = builder.Build();

            // Add IConfiguration to DI - only one instance to work with the same file.
            services.AddSingleton(config);

            var serviceProvider = services.BuildServiceProvider();
            var mainWindow = serviceProvider.GetService<MainWindow>(); // get the service that was added to the serviceCollection

            mainWindow.Show();
        }
    }

}
