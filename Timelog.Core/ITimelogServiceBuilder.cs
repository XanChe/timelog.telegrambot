using Timelog.Core.Entities;
using Timelog.Core.Services;

namespace Timelog.Core
{
    /// <summary>
    ///     Интерфейс "Строителя" сервисов Timelog уровня бизнес логики.
    /// </summary>
    public interface ITimelogServiceBuilder
    {
        public void UseUserFilter(string userIdentityId);
        public IStatisticsService CreateStatisticsService();
        public IUserActivityService CreateUserActivityService();       
        public IEntityService<Project> CreateProjectService();     
        public IEntityService<ActivityType> CreateActivityTypeService();
       
    }
}
