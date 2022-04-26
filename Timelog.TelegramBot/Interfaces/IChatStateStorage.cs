using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timelog.TelegramBot.Models;

namespace Timelog.TelegramBot.Interfaces
{
    public interface IChatStateStorage
    {
        public ChatStateModel? GetChatStateByChatId(long? chatId);
        public void SetChatStateByChatId(long chatId, ChatStateModel chatState);
    }
}
