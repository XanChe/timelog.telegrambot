using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using Telegram.Bot;
using Telegram.Bot.Types;
using Timelog.Core;
using Timelog.Core.Entities;
using Timelog.Core.Services;
using Timelog.TelegramBot.Interfaces;

namespace Timelog.TelegramBot.Commands
{
    public class ProjectsCommands
    {
        private readonly IEntityService<Project> _projectService;
        public ProjectsCommands(IBotCommandsService botCommands, ITimelogServiceBuilder serviceBuilder)
        {
            _projectService = serviceBuilder.CreateProjectService();
            
        }
        [CommandBind("/project")]
        public async Task ProjectCommandAsync(ITelegramBotClient botClient, Update update, string parameters)
        {

            var project = await _projectService.GetByIdAsync(new Guid(parameters));
           
            var message = update.Message;
#nullable disable          
            await botClient.SendTextMessageAsync(message.Chat, $"Возвращаю: {Newtonsoft.Json.JsonConvert.SerializeObject(project)}");
#nullable enable
        }

    }
}
