using Telegram.Bot;
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
        public async Task ProjectCommandAsync(ITelegramBotClient botClient, UpdateRequest updateRequest)
        {
            if (updateRequest.ParametrString != null && updateRequest.ParametrString != "")
            {
                var project = await _projectService.GetByIdAsync(new Guid(updateRequest.ParametrString));
#nullable disable
                await botClient.SendTextMessageAsync(updateRequest.TelegramChatId, $"Возвращаю: {Newtonsoft.Json.JsonConvert.SerializeObject(project)}");
#nullable enable
            }
        }
        [CommandValidate("/project")]
        public async Task<bool> ValidateProjectCommandAsync(UpdateRequest commandRequest)
        {
            return await Task.FromResult<bool>(ValidateProjectCommand(commandRequest));
        }
        private bool ValidateProjectCommand(UpdateRequest commandRequest)
        {
            if (!commandRequest.IsUserSingIn)
            {
                commandRequest.ErrorMessage = "Пользователь не авторизован!";
                return false;
            }
            if (!Guid.TryParse(commandRequest.ParametrString, out _))
            {
                commandRequest.ErrorMessage = "Неверный фомат параметра. Введите id проекта.";
                return false;
            }
            return true;

        }

    }
}
