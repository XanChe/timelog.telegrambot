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
        private readonly IBotCommandsService _botCommands;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserStorage _userStorage;

        public BotApplication(TelegramBotSettings? botSettings,
            IBotCommandsService botCommands, 
            IUnitOfWork unitOfWork,
            IUserStorage userStorage,
            CommandsCollector commandsCollector)
        {
            _botCommands = botCommands;
            _unitOfWork = unitOfWork;
            _userStorage = userStorage;
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
                   
                    var commandRow = message.Text.Split(' ', 2);
                    var commandRequest = new { Command = commandRow[0].ToLower(), Prameter = commandRow.Length > 1 ? commandRow[1] : "" };
                    var userAuthString = _userStorage.GetTokenById(message.From.Id);

                    if (userAuthString != null || commandRow[0].ToLower() == "/singin")
                    {
                        _unitOfWork.UseUserFilter(userAuthString ?? "");

                        await _botCommands.ExecuteCommandAsync(commandRequest.Command, botClient, update, commandRequest.Prameter);
                    }
                    else
                    {
#nullable disable
                        await botClient.SendTextMessageAsync(message.Chat, "Пользователь не аторизирован!");
                    }



                }

                if (message?.Text?.ToLower() == "/start")
                {
                    await botClient.SendTextMessageAsync(message.Chat, "Добро пожаловать на борт, добрый путник!");
                    return;
                }
                
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
