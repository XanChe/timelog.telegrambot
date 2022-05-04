using Moq;
using Timelog.Core;
using Timelog.TelegramBot;
using Timelog.TelegramBot.Commands;
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
            var commands = new Mock<IBotCommandsService>();
            var unitOfWork = new Mock<IUnitOfWork>();
            var userSorage = new Mock<IUserStorage>();
            var chatStorage = new Mock<IChatStateStorage>();
            var cmdCollector = new Mock<CommandsCollector>();

            var settings = new TelegramBotSettings() { Token = "TokenForTest" };
            var app = new BotApplication(settings, commands.Object, unitOfWork.Object, userSorage.Object, chatStorage.Object, cmdCollector.Object);
            Assert.NotNull(app);
        }
    }
}
