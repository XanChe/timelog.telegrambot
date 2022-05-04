using System.Net.Http.Headers;
using Timelog.ApiClient.Repositories;
using Timelog.ApiClient.Settings;
using Timelog.Core;
using Timelog.Core.Entities;
using Timelog.Core.Repositories;

namespace Timelog.ApiClient
{
    /// <summary>
    ///     Api реализация интерфейса IUnitOfWork
    /// </summary>
    public class ApiUnitOfWork : IUnitOfWork
    {
        private readonly HttpClient _httpClient;
        private readonly ApiRepositoryGeneric<Project> _projectRepository;
        private readonly ApiRepositiryActivity _activityRepository;

        public IRepositoryGeneric<ActivityType> ActivityTypes => throw new NotImplementedException();

        public IRepositoryGeneric<Project> Projects => _projectRepository;

        public IRepositoryActivity Activities => _activityRepository;

        public IRepositirySatistics Satistics => throw new NotImplementedException();

        public ApiUnitOfWork(ApiClientSettings apiSettings)
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

            _httpClient = new HttpClient(handler);
            _httpClient.BaseAddress = new Uri(apiSettings?.Url ?? "");
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));



            _projectRepository = new ApiRepositoryGeneric<Project>(_httpClient, "Projects");
            _activityRepository = new ApiRepositiryActivity(_httpClient, "Activities");
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }

        public Task SaveChangesAsync()
        {
            return Task.Delay(0);
        }

        public void UseUserFilter(string userIdentityId)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    "Bearer",
                    userIdentityId
                    );
        }
    }
}
