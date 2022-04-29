using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using Timelog.Core;
using Timelog.Core.Entities;
using Timelog.Core.Services;
using Timelog.TelegramBot.Requests;

namespace Timelog.TelegramBot.Helpers
{
    public class DialogHelper
    {
        private readonly IEntityService<Project> _projectService;
        public DialogHelper(ITimelogServiceBuilder serviceBuilder)
        {
            _projectService = serviceBuilder.CreateProjectService();
        }
        public async Task SendSelectProjectDialogAsync(ITelegramBotClient botClient, UpdateRequest updateRequest, string dialogMessage)
        {
            var availableProjects = await _projectService.GetAllAsync();
            var buttons = availableProjects
                .Select(project => InlineKeyboardButton.WithCallbackData(text: project.Name, callbackData: $"{updateRequest.Command} {project.Id}"))
                .ToArray();

            InlineKeyboardMarkup inlineKeyboard = new(buttons);

            var sentMessage = await botClient.SendTextMessageAsync(
                chatId: updateRequest.TelegramChatId,
                text: dialogMessage,
                replyMarkup: inlineKeyboard);
        }
    }
}
