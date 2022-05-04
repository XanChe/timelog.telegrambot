using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using Timelog.Core;
using Timelog.Core.Entities;
using Timelog.Core.Services;
using Timelog.TelegramBot.Requests;

namespace Timelog.TelegramBot.Helpers
{
    /// <summary>
    ///     Общие методы для ответов на комманды бота
    /// </summary>
    public class BotDialogHelper
    {
        private readonly IEntityService<Project> _projectService;
        public BotDialogHelper(ITimelogServiceBuilder serviceBuilder)
        {
            _projectService = serviceBuilder.CreateProjectService();
        }
        /// <summary>
        ///     Отптравка боту запроса на выбор проекта, как параметра к команде бота.
        /// </summary>
        /// <param name="botClient">Сам бот, кторому отсылается ответ.</param>
        /// <param name="updateRequest"> Текущий запрос на обновление</param>
        /// <param name="dialogMessage">Собщение к которому, прикрепляются кнопки.</param>
        /// <returns></returns>
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
