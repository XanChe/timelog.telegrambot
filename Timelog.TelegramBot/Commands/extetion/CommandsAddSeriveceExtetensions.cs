using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Timelog.TelegramBot.Commands
{
    public static class CommandsAddSeriveceExtetensions
    {
        public static IServiceCollection AddBotCommands(this IServiceCollection services)
        {
            Assembly? asm = Assembly.GetEntryAssembly();

            // получаем все типы из сборки MyApp.dll
            Type[] types = asm.GetTypes();

            var commandsTypes = types.Where(t => t.Name.EndsWith("Commands")).ToList();

            foreach (var type in commandsTypes)
            {
                var servicesTpe = typeof(ServiceCollectionServiceExtensions);
                var altMethod = servicesTpe.GetMethods().Where(t => t.Name == "AddTransient").FirstOrDefault(x => x.IsGenericMethod && x.GetGenericArguments().Count() == 1);
                var generic = altMethod.MakeGenericMethod(type);
                generic.Invoke(services, new object[] { services });
            }           
            
            services.AddTransient<CommandsCollector>();

            return services;
        }
    }
}
