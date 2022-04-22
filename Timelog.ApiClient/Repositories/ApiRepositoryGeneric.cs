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
        private readonly HttpClient _apiClient;
        private readonly string _actionPrefix;
        private JsonSerializerOptions jsonOptions = new JsonSerializerOptions
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

        public void Create(T item)
        {
            throw new NotImplementedException();
        }

        public Task CreateAsync(T item)
        {
            throw new NotImplementedException();
        }

        public void Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public T? Read(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<T?> ReadAsync(Guid id)
        {
            string result = await _apiClient.GetStringAsync(_actionPrefix + '/' + id);
            return JsonSerializer.Deserialize<T>(result, jsonOptions);
        }

        public Task<long> SaveChangesAsync()
        {
            throw new NotImplementedException();
        }

        public void SetUser(Guid userIdentityGuid)
        {
            userGuid = userIdentityGuid;
        }

        public void Update(T item)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(T item)
        {
            throw new NotImplementedException();
        }

        public void UseFilter(Func<T, bool> filter)
        {
            throw new NotImplementedException();
        }
    }
}
