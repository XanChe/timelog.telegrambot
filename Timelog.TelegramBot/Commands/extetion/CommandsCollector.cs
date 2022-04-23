using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Timelog.TelegramBot.Interfaces;

namespace Timelog.TelegramBot.Commands
{
    public class CommandsCollector
    {
        public CommandsCollector(IBotCommandsService commandsService, IServiceProvider services)
        {
            Assembly? asm = Assembly.GetEntryAssembly();
           
            // получаем все типы из сборки MyApp.dll
            Type[] types = asm.GetTypes();
            foreach (Type t in types)
            {                
                if (t.Name.EndsWith("Commands"))
                {
                    foreach (MethodInfo method in t.GetMethods())
                    {
                        var bindAttribute = method.GetCustomAttribute<CommandBindAttribute>();
                        if (bindAttribute != null)
                        {
                            //var atr = method.CustomAttributes.Where(atr => atr.AttributeType.Name == "CommandBindAttribute").SingleOrDefault();
                            var commandToBind = bindAttribute?.Command ?? "";
                            var commandService = services.GetService(t);
                            commandsService.RegisterHandler(commandToBind, method.CreateDelegate<CommandHandler>(commandService));
                        }
                    }
                }

            }
        }
    }
}
