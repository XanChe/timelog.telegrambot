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
            var telegramUser = _telegramBot.GetMeAsync().Result;
            Console.WriteLine("Запущен бот " + telegramUser.FirstName);

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
            if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
            {
                var botRequest = new CommandRequest(update.Message);
                
                var lastChatState = _chatStateStorage.GetChatStateByChatId(botRequest.TelegramChatId);
                
                if (botRequest.IsCommand() || lastChatState != null)
                {
                    botRequest.IsUserSingIn = ConfigureUnitOfWorkForUser(botRequest.TelegramUserId);
                }

                Command? command = null;

                if (botRequest.IsCommand())
                {        
                    _chatStateStorage.SetChatStateByChatId(botRequest.TelegramChatId, new ChatStateModel() { ChatId = botRequest.TelegramChatId, CurrentCommand = botRequest.Command });

                    command = _botCommands.GetCommand(botRequest.Command);
                }
                else if (lastChatState != null)
                {                    
                    command = _botCommands.GetCommand(lastChatState.CurrentCommand);
                }

                if (command != null)
                {
                    if (await command.Validation(botRequest))
                    {
                        await command.Execute(botClient, update, botRequest.ParametrString);
                    }
                    else
                    {
                        await botClient.SendTextMessageAsync(botRequest.TelegramChatId, botRequest.ErrorMessage);
                    }                   
                }
                if (botRequest.MessageText.ToLower() == "/test")
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
                        chatId: botRequest.TelegramChatId,
                        text: "A message with an inline keyboard markup",
                        replyMarkup: inlineKeyboard,
                        cancellationToken: cancellationToken);
                }
                if (botRequest.MessageText.ToLower() == "/untest")
                {
                    Message sentMessage = await botClient.SendTextMessageAsync(
                    chatId: botRequest.TelegramChatId,
                    text: "Removing keyboard",
                    replyMarkup: new ReplyKeyboardRemove(),
                    cancellationToken: cancellationToken);
                                }

                if (botRequest.MessageText.ToLower() == "/start")
                {
                    await botClient.SendTextMessageAsync(botRequest.TelegramChatId, "Добро пожаловать на борт, добрый путник!");
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
