using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
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
        public AuthCommands(IUserStorage userStorage, ApiClientSettings apiSettings)
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

            _httpClient = new HttpClient(handler);
            _httpClient.BaseAddress = new Uri(apiSettings?.Url ?? "");
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _userStorage = userStorage;
        }
        [CommandBind("/singin")]
        public async Task SingInCommandAsync(ITelegramBotClient botClient, Update update, string parameters)
        {
            var parametrsRow = parameters.Split();
            var response = await _httpClient.PostAsync("Auth/SignIn", new StringContent(
                    JsonSerializer.Serialize(new { email = parametrsRow[0], password = parametrsRow[1] }),
                    Encoding.UTF8, "application/json"));
            var content = await response.Content.ReadAsStringAsync();
            var token = JsonSerializer.Deserialize<string>(content);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                _userStorage.SetTokenById(update.Message.From.Id, token);
            }
            var message = update.Message;
#nullable disable          
            await botClient.SendTextMessageAsync(message.Chat, $"Возвращаю: {token}");
#nullable enable
        }

    }
}
