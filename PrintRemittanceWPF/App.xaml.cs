using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PrintRemittance.Core;
using PrintRemittance.Core.Interfaces.Repositories;
using PrintRemittance.Core.Repositories;
using PrintRemittanceWPF.Helper;
using System.Windows;

namespace PrintRemittanceWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ServiceProvider serviceProvider;

        public App()
        {
            ServiceCollection services = new ServiceCollection();
            ConfigureServices(services);
            serviceProvider = services.BuildServiceProvider();
            //ReadSettingsFromJsonFile();
        }

        private void ConfigureServices(ServiceCollection services)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
          .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
          .AddJsonFile("appsettings.json")
          .Build();

            string connectionString = configuration.GetConnectionString("SqlConnectionString") ?? string.Empty;


            services.AddScoped<IDocumentsRepository, DocumentRepository>();

            services.AddDbContext<PrintRemittanceDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("SqlConnectionString")));

            MigrateDatabase(services);

            services.AddSingleton<MainWindow>();

        }

        private void OnStartup(object sender, StartupEventArgs e)
        {
            var mainWindow = serviceProvider.GetService<MainWindow>();
            mainWindow!.Show();
        }

        private void MigrateDatabase(ServiceCollection services)
        {
            try
            {
                using (var serviceProvider = services.BuildServiceProvider())
                {
                    var dbContext = serviceProvider.GetRequiredService<PrintRemittanceDbContext>();
                    dbContext.Database.Migrate();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطا در ارتباط با پایگاه داده");
                Logger.LogException(ex);
            }
        }

        /// <summary>
        /// تنظیمات نرم افزار از یک فایل جیسون از مسیر روت نرم افزار خوانده میشود
        /// و در نهایت بر روی برنامه اعمال میشود
        /// </summary>
        //private void ReadSettingsFromJsonFile()
        //{
        //    try
        //    {
        //        var path = $@"{Environment.CurrentDirectory}\Setting.json";
        //        if (!File.Exists(path))
        //            return;
        //        var JsonString = File.ReadAllText(path);
        //        var setting = JsonSerializer.Deserialize<AppSettings>(JsonString);
        //        if (setting != null)
        //            AppSession.AppSettings = setting;
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.LogException(ex);
        //    }
        //}
    }

}
