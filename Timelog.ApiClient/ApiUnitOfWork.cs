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
            throw new NotImplementedException();
        }

        public void UseUserFilter(Guid userIdentityGuid)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    "Bearer",
                    "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiI0MzU0NzgzZS1lY2Y4LTRjNzgtYWQ3Zi04OTRjMTU2MGJjNDUiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoieGFuQHhhbi5ydSIsImp0aSI6IjJlYzY1MDdlLTcxNzctNDczNC04NzBjLTIyMDg3NDI3MWZhYyIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWVpZGVudGlmaWVyIjoiNDM1NDc4M2UtZWNmOC00Yzc4LWFkN2YtODk0YzE1NjBiYzQ1IiwiZXhwIjoxNjUzMTAzMTA3LCJpc3MiOiJodHRwczovL2xvY2FsaG9zdDo1MDAwIiwiYXVkIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NTAwMCJ9.AwxdOkTZabWLPj-bNyPO4BMoQd-bCgYfA9fRe4b2_vU"
                    );
        }
    }
}
