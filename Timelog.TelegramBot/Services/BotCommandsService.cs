using Telegram.Bot;
using Telegram.Bot.Types;
using Timelog.TelegramBot.Interfaces;

namespace Timelog.TelegramBot.Services
{
    public class BotCommandsService : IBotCommandsService
    {
        private Dictionary<string, CommandHandler> _commands = new Dictionary<string, CommandHandler>();        
        public async Task ExecuteCommandAsync(string botCommant, ITelegramBotClient botClient, Update update, string parameters)
        {
            if (_commands.ContainsKey(botCommant))
            {
                await _commands[botCommant].Invoke(botClient, update, parameters);
            }
        }
        public void RegisterHandler(string botCommant, CommandHandler command)
        {
            if (_commands.ContainsKey(botCommant))
            {
                _commands[botCommant] += command;
            }
            else
            {
                _commands.Add(botCommant, command);
            }

        }
        public void UnregisterHandler(string botCommant)
        {
            if (_commands.ContainsKey(botCommant))
            {
                _commands.Remove(botCommant);
            }
        }
    }
}
