using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Timelog.TelegramBot.Commands
{
    public static class CommandsAddSeriveceExtetensions
    {        
        /// <summary>
        /// Добавляет все классы именованые *Commands в коллекцию сервисов.
        /// </summary>
        public static IServiceCollection AddBotCommands(this IServiceCollection services)
        {
            Assembly? asm = Assembly.GetEntryAssembly();

            // получаем все типы из сборки MyApp.dll
            Type[] types = asm?.GetTypes() ?? new Type[0];

            var commandsTypes = types.Where(t => t.Name.EndsWith("Commands")).ToList();

            foreach (var type in commandsTypes)
            {
                var servicesTpe = typeof(ServiceCollectionServiceExtensions);
                var altMethod = servicesTpe.GetMethods().Where(t => t.Name == "AddTransient").FirstOrDefault(x => x.IsGenericMethod && x.GetGenericArguments().Count() == 1);
                var generic = altMethod?.MakeGenericMethod(type);
                generic?.Invoke(services, new object[] { services });
            }

            services.AddTransient<CommandsCollector>();

            return services;
        }
    }
}
