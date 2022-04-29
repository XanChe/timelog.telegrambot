using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Timelog.Core.Entities;
using Timelog.Core.Repositories;

namespace Timelog.ApiClient.Repositories
{
    public class ApiRepositoryGeneric<T> : IRepositoryGeneric<T> where T : Entity
    {
        protected readonly HttpClient _apiClient;
        protected readonly string _actionPrefix;
        protected JsonSerializerOptions jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };

        private Guid userGuid;
        public ApiRepositoryGeneric(HttpClient client, string actionPrefix)
        {
            _apiClient = client;
            _actionPrefix = actionPrefix;
        }
        public Guid UserGuid => userGuid;
        public async Task CreateAsync(T item)
        {
            var response = await _apiClient.PostAsync(
                _actionPrefix, 
                new StringContent(
                    JsonSerializer.Serialize(item),
                    Encoding.UTF8, "application/json"
                    )
                );
        }

        public async Task DeleteAsync(Guid id)
        {
            var response = await _apiClient.DeleteAsync(_actionPrefix + '/' + id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var response = await _apiClient.GetAsync(_actionPrefix);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<IEnumerable<T>>(content, jsonOptions);
            }
            return new List<T>();
        }
        public async Task<T?> ReadAsync(Guid id)
        {
            var response = await _apiClient.GetAsync(_actionPrefix + '/' + id);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<T>(content, jsonOptions);
            }
            return null;
        }
        public async Task<long> SaveChangesAsync()
        {
            return 0;
        }
        public void SetUser(Guid userIdentityGuid)
        {
            userGuid = userIdentityGuid;
        }
        public async Task UpdateAsync(T item)
        {
            var response = await _apiClient.PostAsync(
                 _actionPrefix,
                 new StringContent(
                     JsonSerializer.Serialize(item),
                     Encoding.UTF8, "application/json"
                     )
                 );
        }
        public void UseFilter(Func<T, bool> filter)
        {
            throw new NotImplementedException();
        }
    }
}
