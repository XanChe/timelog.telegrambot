using Timelog.Core.Entities;
using Timelog.Core.Repositories;

namespace Timelog.Core
{
    /// <summary>
    ///     Интерфейс сервиса работы с основными репозидотиями Timelog.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        IRepositoryGeneric<ActivityType> ActivityTypes { get; }
        IRepositoryGeneric<Project> Projects { get; }
        IRepositoryActivity Activities { get; }
        IRepositirySatistics Satistics { get; }
        void UseUserFilter(string userIdentityId);
        Task SaveChangesAsync();
    }
}
