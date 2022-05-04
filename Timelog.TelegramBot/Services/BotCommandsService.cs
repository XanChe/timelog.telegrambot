using Timelog.TelegramBot.Interfaces;
using Timelog.TelegramBot.Models;

namespace Timelog.TelegramBot.Services
{
    public class BotCommandsService : IBotCommandsService
    {
        private Dictionary<string, Command> _commands = new Dictionary<string, Command>();

        public Command? GetCommand(string botCommant)
        {
            if (_commands.ContainsKey(botCommant))
            {
                return _commands[botCommant];
            }
            else
            {
                return null;
            }
        }
        public void RegisterCommand(string botCommant, Command command)
        {
            if (_commands.ContainsKey(botCommant))
            {
                _commands[botCommant] = command;
            }
            else
            {
                _commands.Add(botCommant, command);
            }

        }
        public void UnregisterCommand(string botCommant)
        {
            if (_commands.ContainsKey(botCommant))
            {
                _commands.Remove(botCommant);
            }
        }
    }
}
