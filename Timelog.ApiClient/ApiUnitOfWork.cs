using System.Net.Http.Headers;
using Timelog.ApiClient.Repositories;
using Timelog.ApiClient.Settings;
using Timelog.Core;
using Timelog.Core.Entities;
using Timelog.Core.Repositories;

namespace Timelog.ApiClient
{
    public class ApiUnitOfWork : IUnitOfWork
    {
        private readonly HttpClient _httpClient;
        private readonly ApiRepositoryGeneric<Project> _projectRepository;

        public IRepositoryGeneric<ActivityType> ActivityTypes => throw new NotImplementedException();

        public IRepositoryGeneric<Project> Projects => _projectRepository;

        public IRepositoryActivity Activities => throw new NotImplementedException();

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
