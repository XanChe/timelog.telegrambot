using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timelog.TelegramBot.Models
{
    public class ChatStateModel
    {
        public long ChatId { get; set; }
        public string? CurrentCommand { get; set; }
    }
}
