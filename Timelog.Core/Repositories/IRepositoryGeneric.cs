using Timelog.Core.Entities;

namespace Timelog.Core.Repositories
{
    public interface IRepositoryGeneric<T> where T : Entity
    {
        public void UseFilter(Func<T, bool> filter);

        public void SetUser(Guid userIdentityGuid);
        public Guid UserGuid { get; }
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> ReadAsync(Guid id);
        Task CreateAsync(T item);
        Task UpdateAsync(T item);
        Task DeleteAsync(Guid id);
        Task<long> SaveChangesAsync();

    }

}
