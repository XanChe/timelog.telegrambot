using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Timelog.Core;
using Timelog.Core.Services;

namespace Timelog.TelegramBot.Commands
{
    public class ActivityCommands
    {
        private readonly IUserActivityService _activityService;
        public ActivityCommands(ITimelogServiceBuilder serviceBuilder)
        {
            _activityService = serviceBuilder.CreateUserActivityService();

        }
        [CommandBind("/activities")]
        public async Task ReplayActivitiesAsync(ITelegramBotClient botClient, Update update, string parameters)
        {
            var activities = await _activityService.GetActivitiesAsync();
            
            var message = update.Message;
            var replayText = new StringBuilder();
           
            activities.Aggregate(replayText, (accum, activity) => accum.Append($"* {activity.Comment} \\- {activity.StartTime:t} Длительность: {activity.Duration:hh}:{activity.Duration:mm}\n"));
            var result = replayText.ToString();
#nullable disable
            await botClient.SendTextMessageAsync(message.Chat, result, parseMode: ParseMode.MarkdownV2);
#nullable enable
        }
        
    }
}
