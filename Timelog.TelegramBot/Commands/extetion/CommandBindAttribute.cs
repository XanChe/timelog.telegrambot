namespace Timelog.TelegramBot.Commands
{
    public class CommandBindAttribute: Attribute
    {
        public string Command { get; set; }        
        public CommandBindAttribute(string command)
        {
            Command = command.ToLower();
        }
    }
}
