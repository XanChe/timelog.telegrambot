using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Timelog.Core;
using Timelog.Core.Services;
using Timelog.TelegramBot.Helpers;
using Timelog.TelegramBot.Interfaces;
using Timelog.TelegramBot.Requests;

namespace Timelog.TelegramBot.Commands
{
    public class ActivityCommands
    {
        private readonly IUserActivityService _activityService;
        private readonly IChatStateStorage _chatStateStorage;
        private readonly DialogHelper _dialgHelper;
        public ActivityCommands(
            ITimelogServiceBuilder serviceBuilder, 
            IChatStateStorage chatStateStorage, 
            DialogHelper dialogHelper
            )
        {
            _activityService = serviceBuilder.CreateUserActivityService();
            _chatStateStorage = chatStateStorage;
            _dialgHelper = dialogHelper;

        }
        
        [CommandBind("/startactivity")]
        public async Task StartActivityCommand(ITelegramBotClient botClient, UpdateRequest updateRequest)
        {
            var currentChatState = _chatStateStorage.GetChatStateByChatId(updateRequest.TelegramChatId);
            if (currentChatState.ProjectId != null)
            {
                await _activityService.StartNewActivityAsync(new Guid(currentChatState.ProjectId), new Guid("0c8d4c9f-4977-4ac3-8075-d88de3e2ae1f"));
            }
            else
            {
                await _dialgHelper.SendSelectProjectDialogAsync(botClient, updateRequest, "Выбирите проект:");
            }
        }
        [CommandBind("/stopactivity")]
        public async Task StopActivityCommand(ITelegramBotClient botClient, UpdateRequest updateRequest)
        {
            var currentChatState = _chatStateStorage.GetChatStateByChatId(updateRequest.TelegramChatId);
            await _activityService.StopCurrentActivityIfExistAsync(updateRequest.ParametrString ?? "");

        }

        [CommandBind("/current")]
        public async Task ReplyCurrentActivityAsync(ITelegramBotClient botClient, UpdateRequest updateRequest)
        {
            var currentChatState = _chatStateStorage.GetChatStateByChatId(updateRequest.TelegramChatId);

            var currentActivity = await _activityService.GetCurrentActivityIfExistAsync();
            
#nullable disable
            await botClient.SendTextMessageAsync(updateRequest.TelegramChatId, $"|{currentActivity.Title} | {currentActivity.ProjectName} | {currentActivity.Duration}|");
#nullable enable
        }
        [CommandBind("/activities")]
        public async Task ReplyActivitiesAsync(ITelegramBotClient botClient, UpdateRequest updateRequest)
        {
            var currentChatState = _chatStateStorage.GetChatStateByChatId(updateRequest.TelegramChatId);

            var activities = await _activityService.GetActivitiesAsync();
            if (currentChatState.ProjectId != null)
            {
                activities = activities.Where(activity => activity.ProjectId.ToString() == currentChatState.ProjectId);
            }
            var replayText = new StringBuilder();
            var result = "";
            if (activities.Count() > 0)
            {
                activities.Aggregate(replayText, (accum, activity) => accum.Append($"* {activity.Comment} \\- {activity.StartTime:t} Длительность: {activity.Duration:hh}:{activity.Duration:mm}\n"));
                result = replayText.ToString();
            }
            else
            {
                result = "Нет активностей у текущего проекта";
            }
#nullable disable
            await botClient.SendTextMessageAsync(updateRequest.TelegramChatId, result, parseMode: ParseMode.MarkdownV2);
#nullable enable
        }
        
    }
}
