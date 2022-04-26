namespace Timelog.TelegramBot.Commands
{
    public class CommandValidateAttribute : Attribute
    {
        public string Command { get; set; }
        public CommandValidateAttribute(string command)
        {
            Command = command.ToLower();
        }
    }
}
