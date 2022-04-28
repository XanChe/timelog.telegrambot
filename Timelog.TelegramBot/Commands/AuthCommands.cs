using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Telegram.Bot;
using Telegram.Bot.Types;
using Timelog.ApiClient.Settings;
using Timelog.TelegramBot.Interfaces;
using Timelog.TelegramBot.Requests;

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
        public async Task SingInAsync(ITelegramBotClient botClient, UpdateRequest updateRequest)
        {
            var parametrsRow = (updateRequest?.ParametrString ?? "").Split();
            var response = await _httpClient.PostAsync("Auth/SignIn", new StringContent(
                    JsonSerializer.Serialize(new { email = parametrsRow[0], password = parametrsRow[1] }),
                    Encoding.UTF8, "application/json"));
            var content = await response.Content.ReadAsStringAsync();
            var token = JsonSerializer.Deserialize<string>(content);
#nullable disable
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                _userStorage.SetTokenByUserId(updateRequest.TelegramUserId, token);
            }
         
            await botClient.SendTextMessageAsync(updateRequest.TelegramChatId, "Вы вошли!");
#nullable enable
        }
        [CommandBind("/singout")]
        public async Task SingOutAsync(ITelegramBotClient botClient, UpdateRequest updateRequest)
        {
#nullable disable
            _userStorage.RemoveTokenByUserId(updateRequest.TelegramUserId);
            await botClient.SendTextMessageAsync(updateRequest.TelegramChatId, "Вы вышли!");
#nullable enable
        }

    }
}
