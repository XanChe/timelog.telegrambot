using Timelog.Core.Entities;

namespace Timelog.Core.Services
{
    public interface IEntityService<T> where T : Entity
    {
        public Task<IEnumerable<T>> GetAllAsync();
        public Task<T?> GetByIdAsync(Guid Id);
        public Task CreateAsync(T item);
        public Task UpdateAsync(T item);
        public Task DeleteAsync(Guid id);       
    }
}
