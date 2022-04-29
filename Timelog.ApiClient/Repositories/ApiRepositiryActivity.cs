using System.Text;
using System.Text.Json;
using Timelog.Core.Entities;
using Timelog.Core.Repositories;

namespace Timelog.ApiClient.Repositories
{
    public class ApiRepositiryActivity : ApiRepositoryGeneric<UserActivity>, IRepositoryActivity
    {
        public Guid UserGuid => throw new NotImplementedException();

        public ApiRepositiryActivity(HttpClient client, string actionPrefix) : base(client, actionPrefix)
        {

        }
        public async Task<UserActivity?> getCurrentActivityAsync()
        {
            var response = await _apiClient.GetAsync(_actionPrefix + "/Current");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<UserActivity>(content, jsonOptions);
            }
            return null;
        }
        public async Task StartActivityAsync(Guid projectId, Guid activityTypeId)
        {
            var response = await _apiClient.PostAsync(
                _actionPrefix + "/Start"
                ,
                new StringContent(
                    JsonSerializer.Serialize(new { projectId = projectId, activityTypeId = activityTypeId }),
                    Encoding.UTF8, "application/json"
                    )
                );
        }

        public async Task StopActivityAsync(string comment)
        {
            var response = await _apiClient.PostAsync(
                _actionPrefix + "/Stop"
                ,
                new StringContent(
                    JsonSerializer.Serialize(new {comment = comment }),
                    Encoding.UTF8, "application/json"
                    )
                );
        }
    }
}
