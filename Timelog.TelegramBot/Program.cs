using Microsoft.Extensions.Configuration;
using Timelog.TelegramBot.Settings;
using Microsoft.Extensions.DependencyInjection;
using Timelog.TelegramBot.Interfaces;
using Timelog.TelegramBot.Services;
using Timelog.TelegramBot;
using Timelog.Core;
using Timelog.ApiClient;
using Timelog.ApiClient.Settings;
using Timelog.Services;
using Timelog.TelegramBot.Commands;
using System.Reflection;
using Timelog.TelegramBot.Helpers;

namespace TelegramBotExperiments
{
    class Program
    {
        static void Main(string[] args)
        {
            var devEnvironmentVariable = Environment.GetEnvironmentVariable("NETCORE_ENVIRONMENT");
            var isDevelopment = string.IsNullOrEmpty(devEnvironmentVariable) || devEnvironmentVariable.ToLower() == "development";
            //Determines the working environment as IHostingEnvironment is unavailable in a console app

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            if (isDevelopment) //only add secrets in development
            {
                builder.AddUserSecrets<TelegramBotSettings>();
            }
            var configuration = builder.Build();

            var serviceCollection = new ServiceCollection();

            serviceCollection
                .AddOptions()
                .AddTransient<IConfiguration>(provider => configuration)
                .AddTransient<TelegramBotSettings>(provider => configuration.GetSection(nameof(TelegramBotSettings)).Get<TelegramBotSettings>())
                .AddTransient<ApiClientSettings>(provider => configuration.GetSection(nameof(ApiClientSettings)).Get<ApiClientSettings>())
                .AddTransient<DialogHelper>()
                .AddScoped<IUnitOfWork, ApiUnitOfWork>()
                .AddScoped<ITimelogServiceBuilder, ApiTimelogServiceBuilder>()
                .AddSingleton<IBotCommandsService, BotCommandsService>()
                .AddSingleton<IChatStateStorage, SimpleChatStateStorage>()
                .AddSingleton<IUserStorage, SimpleUserStorage>()
                .AddBotCommands()                                
                .AddTransient<BotApplication>();
            var services = serviceCollection.BuildServiceProvider();          


            var app = services.GetService<BotApplication>();
            
            app?.Run();

            

        }
    }
}