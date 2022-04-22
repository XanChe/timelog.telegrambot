using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Timelog.Core;
using Timelog.TelegramBot.Commands;
using Timelog.TelegramBot.Interfaces;
using Timelog.TelegramBot.Settings;

namespace Timelog.TelegramBot
{
    public class BotApplication
    {
        private readonly ITelegramBotClient _telegramBot;
        private readonly IBotCommandService _botCommands;
        private readonly IUnitOfWork _unitOfWork;

        public BotApplication(TelegramBotSettings? botSettings, IBotCommandService botCommands, IUnitOfWork unitOfWork, ProjectsCommands projectsCommands)
        {
            _botCommands = botCommands;
            _unitOfWork = unitOfWork;
            if (botSettings != null)
            {
                _telegramBot = new TelegramBotClient(botSettings.Token ?? "");
            }
            else
            {
                throw new ArgumentNullException(nameof(botSettings));
            }
            
        }

        public void Run()
        {
            Console.WriteLine("Запущен бот " + _telegramBot.GetMeAsync().Result.FirstName);

            using var cts = new CancellationTokenSource();
            var cancellationToken = cts.Token;
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { }, // receive all update types
            };
            _telegramBot.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                receiverOptions,
                cancellationToken
            );
            Console.ReadLine();
            cts.Cancel();
        }

        private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            // Некоторые действия
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(update));
            if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
            {
                var message = update.Message;
                if (message?.Text?[0] == '/')
                {
                    _unitOfWork.UseUserFilter(Guid.NewGuid());
                    var commandRow = message.Text.Split(' ', 2);
                    await _botCommands.ExecuteCommandAsync(commandRow[0].ToLower(), botClient, update, commandRow[1]);

                }
#nullable disable
                if (message?.Text?.ToLower() == "/start")
                {
                    await botClient.SendTextMessageAsync(message.Chat, "Добро пожаловать на борт, добрый путник!");
                    return;
                }
                await botClient.SendTextMessageAsync(message.Chat, "Привет-привет!!");
#nullable enable
            }
        }

        private async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            // Некоторые действия
            await Task.Delay(0);
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
        }

    }
}
