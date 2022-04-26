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
                    //if (command.IsAuthorizationRequired())
                    //{
                    //    if (isUserSingIn)
                    //    {
                    //        await command.Execute(botClient, update, botRequest.ParametrString);
                    //    }
                    //    else
                    //    {
                    //        await botClient.SendTextMessageAsync(message.Chat, "Пользователь не аторизирован!");
                    //    }
                    //}
                    //else
                    //{
                    //    await command.Execute(botClient, update, commandRequest.ParametrString);
                    //}
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
