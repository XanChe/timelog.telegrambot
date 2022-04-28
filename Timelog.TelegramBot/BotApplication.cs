using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Timelog.Core;
using Timelog.TelegramBot.Commands;
using Timelog.TelegramBot.Interfaces;
using Timelog.TelegramBot.Models;
using Timelog.TelegramBot.Requests;
using Timelog.TelegramBot.Settings;

namespace Timelog.TelegramBot
{
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
#nullable disable          
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(update));
            if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message || update.Type == Telegram.Bot.Types.Enums.UpdateType.CallbackQuery)
            {
                var updateRequest = new UpdateRequest(update);
                
                var lastChatState = _chatStateStorage.GetChatStateByChatId(updateRequest.TelegramChatId);
                
                if (updateRequest.IsCommand() || lastChatState != null)
                {
                    updateRequest.IsUserSingIn = ConfigureUnitOfWorkForUser(updateRequest.TelegramUserId);
                }

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
                if (updateRequest.MessageText.ToLower() == "/test")
                {
                    InlineKeyboardMarkup inlineKeyboard = new(new[]
                            {
                                // first row
                                new []
                                {
                                    InlineKeyboardButton.WithCallbackData(text: "1.1", callbackData: "11"),
                                    InlineKeyboardButton.WithCallbackData(text: "1.2", callbackData: "12"),
                                },
                                // second row
                                new []
                                {
                                    InlineKeyboardButton.WithCallbackData(text: "2.1", callbackData: "21"),
                                    InlineKeyboardButton.WithCallbackData(text: "2.2", callbackData: "22"),
                                },
                            });

                    Message sentMessage = await botClient.SendTextMessageAsync(
                        chatId: updateRequest.TelegramChatId,
                        text: "A message with an inline keyboard markup",
                        replyMarkup: inlineKeyboard,
                        cancellationToken: cancellationToken);
                }
                if (updateRequest.MessageText.ToLower() == "/untest")
                {
                    Message sentMessage = await botClient.SendTextMessageAsync(
                    chatId: updateRequest.TelegramChatId,
                    text: "Removing keyboard",
                    replyMarkup: new ReplyKeyboardRemove(),
                    cancellationToken: cancellationToken);
                                }

                if (updateRequest.MessageText.ToLower() == "/start")
                {
                    await botClient.SendTextMessageAsync(updateRequest.TelegramChatId, "Добро пожаловать на борт, добрый путник!");
                    return;
                }
            }
#nullable enable
        }

       

        private async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            // Некоторые действия
            await Task.Delay(0);
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
        }

        private bool ConfigureUnitOfWorkForUser(long userId)
        {
            var userAuthString = _userStorage.GetTokenByUserId(userId);

            if (userAuthString != null)
            {
                _unitOfWork.UseUserFilter(userAuthString ?? "");
                return true;
            }
            return false;
        }

    }
}
