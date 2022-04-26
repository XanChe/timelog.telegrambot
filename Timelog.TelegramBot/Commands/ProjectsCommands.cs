using Telegram.Bot;
using Telegram.Bot.Types;
using Timelog.Core;
using Timelog.Core.Entities;
using Timelog.Core.Services;
using Timelog.TelegramBot.Requests;

namespace Timelog.TelegramBot.Commands
{
    public class ProjectsCommands
    {
        private readonly IEntityService<Project> _projectService;
        public ProjectsCommands(ITimelogServiceBuilder serviceBuilder)
        {
            _projectService = serviceBuilder.CreateProjectService();
            
        }
        [CommandBind("/project")]
        public async Task ProjectCommandAsync(ITelegramBotClient botClient, Update update, string parameters)
        {
            if (parameters != "")
            {
                var project = await _projectService.GetByIdAsync(new Guid(parameters));

                var message = update.Message;
#nullable disable
                await botClient.SendTextMessageAsync(message.Chat, $"Возвращаю: {Newtonsoft.Json.JsonConvert.SerializeObject(project)}");
#nullable enable
            }
        }
        [CommandValidate("/project")]
        public async Task<bool> ValidateProjectCommandAsync(CommandRequest commandRequest)
        {
            if (!commandRequest.IsUserSingIn)
            {
                commandRequest.ErrorMessage = "Пользователь не авторизован!";
                return false;
            }
            if (!Guid.TryParse(commandRequest.ParametrString, out _))
            {
                commandRequest.ErrorMessage = "Неверный фомат параметра";
                return false;
            }
            return true;
           
        }

    }
}
