using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Timelog.Core;
using Timelog.Core.Services;
using Timelog.TelegramBot.Interfaces;
using Timelog.TelegramBot.Requests;

namespace Timelog.TelegramBot.Commands
{
    public class ActivityCommands
    {
        private readonly IUserActivityService _activityService;
        private readonly IChatStateStorage _chatStateStorage;
        public ActivityCommands(ITimelogServiceBuilder serviceBuilder, IChatStateStorage chatStateStorage)
        {
            _activityService = serviceBuilder.CreateUserActivityService();
            _chatStateStorage = chatStateStorage;

        }
        [CommandBind("/activities")]
        public async Task ReplayActivitiesAsync(ITelegramBotClient botClient, UpdateRequest updateRequests)
        {
            var currentChatState = _chatStateStorage.GetChatStateByChatId(updateRequests.TelegramChatId);

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
            await botClient.SendTextMessageAsync(updateRequests.TelegramChatId, result, parseMode: ParseMode.MarkdownV2);
#nullable enable
        }
        
    }
}
