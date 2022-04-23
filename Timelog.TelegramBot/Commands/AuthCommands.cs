using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Timelog.ApiClient.Settings;
using Timelog.TelegramBot.Interfaces;

namespace Timelog.TelegramBot.Commands
{   
    public class AuthCommands
    {
        private readonly IUserStorage _userStorage;
        private readonly HttpClient _httpClient;
        public AuthCommands(IBotCommandsService botCommands, IUserStorage userStorage, ApiClientSettings apiSettings)
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

            _httpClient = new HttpClient(handler);
            _httpClient.BaseAddress = new Uri(apiSettings?.Url ?? "");
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _userStorage = userStorage;
            botCommands.RegisterHandler("/singin", SingInCommandAsync);
        }
        public async Task SingInCommandAsync(ITelegramBotClient botClient, Update update, string parameters)
        {
            var parametrsRow = parameters.Split();
            var response = await _httpClient.PostAsync("Auth/SignIn", new StringContent(
                    JsonSerializer.Serialize(new { email = parametrsRow[0], password = parametrsRow[1] }),
                    Encoding.UTF8, "application/json"));
            var content = await response.Content.ReadAsStringAsync();
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                _userStorage.SetTokenById(update.Message.From.Id, content);
            }
            var message = update.Message;
#nullable disable          
            await botClient.SendTextMessageAsync(message.Chat, $"Возвращаю: {content}");
#nullable enable
        }

    }
}
