using Timelog.Core.Entities;
using Timelog.Core.Repositories;
using Timelog.Core.Services;

namespace Timelog.Services
{
    public class EntityService<T>: IEntityService<T> where T : Entity
    {
        private IRepositoryGeneric<T> _repository;

        public EntityService(IRepositoryGeneric<T> repository)
        {
            _repository = repository;
        }
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<T?> GetByIdAsync(Guid Id)
        {
            return await _repository.ReadAsync(Id);
        }

        public async Task CreateAsync(T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("added item is empty");
            }
            await _repository.CreateAsync(item);   
            await _repository.SaveChangesAsync();
        }

        public async Task UpdateAsync(T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("updated item is empty");
            }
            await _repository.UpdateAsync(item);
            await _repository.SaveChangesAsync();
        }
        public async Task DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
            await _repository.SaveChangesAsync();
        }
    }
}
