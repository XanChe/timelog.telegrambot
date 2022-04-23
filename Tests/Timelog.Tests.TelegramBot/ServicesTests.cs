using Timelog.TelegramBot.Services;
using Xunit;

namespace Timelog.Tests.TelegramBot
{
    public class ServicesTests
    {
        [Fact]
        public void TestBotCommandServiceForExist()
        {
            var commsndService = new BotCommandsService();
            Assert.NotNull(commsndService);
        }
    }
}