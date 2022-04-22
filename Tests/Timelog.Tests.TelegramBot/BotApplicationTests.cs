using Moq;
using Timelog.TelegramBot;
using Timelog.TelegramBot.Interfaces;
using Timelog.TelegramBot.Settings;
using Xunit;

namespace Timelog.Tests.TelegramBot
{
    public class BotApplicationTests
    {
        [Fact]
        public void TestBotApplicationExist()
        {
            var commands = new Mock<IBotCommandService>();

            var settings = new TelegramBotSettings() { Token = "TokenForTest" };
            var app = new BotApplication(settings, commands.Object);
            Assert.NotNull(app);
        }
    }
}
