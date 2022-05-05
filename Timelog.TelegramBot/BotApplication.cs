using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Timelog.Core;
using Timelog.TelegramBot.Commands;
using Timelog.TelegramBot.Interfaces;
using Timelog.TelegramBot.Models;
using Timelog.TelegramBot.Requests;
using Timelog.TelegramBot.Settings;

namespace Timelog.TelegramBot
{
    /// <summary>
    ///     Основной класс бота. Тут всё начинается.
    /// </summary>
    public class BotApplication
    {
        private readonly ITelegramBotClient _telegramBot;
        private readonly IBotCommandsService _botCommands;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserStorage _userStorage;
        private readonly IChatStateStorage _chatStateStorage;

        public BotApplication(TelegramBotSettings? botSettings,
            IBotCommandsService botCommands,
            IUnitOfWork unitOfWork,
            IUserStorage userStorage,
            IChatStateStorage chatStateStorage,
            CommandsCollector commandsCollector)
        {
            _botCommands = botCommands;
            _unitOfWork = unitOfWork;
            _userStorage = userStorage;
            _chatStateStorage = chatStateStorage;
            if (botSettings != null)
            {
                _telegramBot = new TelegramBotClient(botSettings.Token ?? "");
            }
            else
            {
                throw new ArgumentNullException(nameof(botSettings));
            }

        }
        /// <summary>
        ///     Точка входа в Timelog телаграмм бот.
        /// </summary>
        public void Run()
        {
            var telegramBotUser = _telegramBot.GetMeAsync().Result;
            Console.WriteLine("Запущен бот " + telegramBotUser.FirstName);

            using var cts = new CancellationTokenSource();
            var cancellationToken = cts.Token;
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { }, // receive all update types
            };
            _telegramBot.StartReceiving(
                HandleUpdateAsync, // оброботчик изменений чата
                HandleErrorAsync, // оброботчик ошибок
                receiverOptions,
                cancellationToken
            );
            Console.ReadLine();
            cts.Cancel();
        }

        private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(update));

            if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message ||
                update.Type == Telegram.Bot.Types.Enums.UpdateType.CallbackQuery)
            {
                var updateRequest = new UpdateRequest(update);

                ConfigureUnitOfWorkForUser(updateRequest);

                var command = ExtractCommandFromRequest(updateRequest);

                if (command != null)
                {
                    if (await command.Validation(updateRequest))
                    {
                        await command.Execute(botClient, updateRequest);
                    }
                    else
                    {
                        await botClient.SendTextMessageAsync(updateRequest.TelegramChatId, updateRequest.ErrorMessage);
                    }
                }
            }
        }

        private Command? ExtractCommandFromRequest(UpdateRequest updateRequest)
        {
            var lastChatState = _chatStateStorage.GetChatStateByChatId(updateRequest.TelegramChatId);

            Command? command = null;

            if (updateRequest.IsCommand())
            {
                var newChatState = lastChatState ?? new ChatStateModel()
                {
                    ChatId = updateRequest.TelegramChatId
                };
                newChatState.CurrentCommand = updateRequest.Command;
                _chatStateStorage.SetChatStateToChatId(updateRequest.TelegramChatId, newChatState);

                command = _botCommands.GetCommand(updateRequest.Command);
            }
            else if (lastChatState != null)
            {
                command = _botCommands.GetCommand(lastChatState.CurrentCommand);
            }

            return command;
        }


        private async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            // Некоторые действия
            await Task.Delay(0);
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
        }

        private void ConfigureUnitOfWorkForUser(UpdateRequest updateRequest)
        {
            var userAuthString = _userStorage.GetTokenByUserId(updateRequest.TelegramUserId);

            if (userAuthString != null)
            {
                _unitOfWork.UseUserFilter(userAuthString ?? "");
                updateRequest.IsUserSingIn = true;

            }
            else
            {
                updateRequest.IsUserSingIn = false;
            }
        }
    }
}
