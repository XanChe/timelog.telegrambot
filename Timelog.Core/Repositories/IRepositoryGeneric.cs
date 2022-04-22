using Timelog.Core.Entities;

namespace Timelog.Core.Repositories
{
    public interface IRepositoryGeneric<T> where T : Entity
    {
        public void UseFilter(Func<T, bool> filter);

        public void SetUser(Guid userIdentityGuid);
        public Guid UserGuid { get; }
        IEnumerable<T> GetAll();
        Task<IEnumerable<T>> GetAllAsync();
        T? Read(Guid id);
        Task<T?> ReadAsync(Guid id);
        void Create(T item);
        Task CreateAsync(T item);
        void Update(T item);
        Task UpdateAsync(T item);
        void Delete(Guid id);
        Task<long> SaveChangesAsync();

    }

}
