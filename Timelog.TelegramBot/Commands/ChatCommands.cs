using System.Linq;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using Timelog.Core;
using Timelog.Core.Entities;
using Timelog.Core.Services;
using Timelog.TelegramBot.Helpers;
using Timelog.TelegramBot.Interfaces;
using Timelog.TelegramBot.Models;
using Timelog.TelegramBot.Requests;

namespace Timelog.TelegramBot.Commands
{
    public class ChatCommands       
    {
        private readonly IEntityService<Project> _projectService;
        private readonly IChatStateStorage _chatStateStorage;
        private readonly BotDialogHelper _dialgHelper;
        public ChatCommands(
            ITimelogServiceBuilder serviceBuilder, 
            IChatStateStorage chatStateStorage, 
            BotDialogHelper dialogHelper
            )
        {
            _projectService = serviceBuilder.CreateProjectService();
            _chatStateStorage = chatStateStorage;
            _dialgHelper = dialogHelper;
        }

        [CommandBind("/bind_project")]
        public async Task BindProjectToCurrentChatCommand(ITelegramBotClient botClient, UpdateRequest updateRequest)
        {
            if (updateRequest.Args.Count > 0)
            {
                var currentChatState = _chatStateStorage
                    .GetChatStateByChatId(updateRequest.TelegramChatId) ?? new ChatStateModel() { ChatId = updateRequest.TelegramChatId};
                currentChatState.ProjectId = updateRequest.Args[0];
                var project = await _projectService.GetByIdAsync(new Guid(currentChatState.ProjectId));
                if (project != null)
                {
                    _chatStateStorage.SetChatStateToChatId(updateRequest.TelegramChatId, currentChatState);                    
                    await botClient.SendTextMessageAsync(updateRequest.TelegramChatId, $"Проект <{project.Name}> привязан к текущему чату");
                }
                else
                {
                    await botClient.SendTextMessageAsync(updateRequest.TelegramChatId, "Проект не существует");
                }
            }
            else
            {
                await _dialgHelper.SendSelectProjectDialogAsync(botClient, updateRequest, "Вибирите проект для привязки:");

                //var availableProjects = await _projectService.GetAllAsync();
                //var buttons = availableProjects
                //    .Select(project => InlineKeyboardButton.WithCallbackData(text: project.Name, callbackData: $"/bind_project {project.Id}"))
                //    .ToArray();
                
                //InlineKeyboardMarkup inlineKeyboard = new(buttons);

                //var sentMessage = await botClient.SendTextMessageAsync(
                //    chatId: updateRequest.TelegramChatId,
                //    text: "Вибирите проект для привязки:",
                //    replyMarkup: inlineKeyboard);
            }

        }
    }
}
