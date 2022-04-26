using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Timelog.TelegramBot.Interfaces;
using Timelog.TelegramBot.Models;

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
                        var validateAttribute = method.GetCustomAttribute<CommandValidateAttribute>();
                        if (bindAttribute != null || validateAttribute != null)
                        {
                            //var atr = method.CustomAttributes.Where(atr => atr.AttributeType.Name == "CommandBindAttribute").SingleOrDefault();
                            var commandToBind = bindAttribute?.Command ?? validateAttribute?.Command ?? "";
                            var commandService = services.GetService(t);
                            var command = commandsService.GetCommand(commandToBind);
                            if (command == null)
                            {
                                command = new Command(commandToBind);
                            }
                            if (bindAttribute != null)
                            {
                                command.SetCommandHandler(method.CreateDelegate<CommandHandler>(commandService));
                            }
                            else if (validateAttribute != null)
                            {
                                command.SetValidateHandler(method.CreateDelegate<ValidateHandler>(commandService));
                            }
                            commandsService.RegisterCommand(commandToBind, command);
                        }                       
                       
                    }
                }

            }
        }
    }
}
