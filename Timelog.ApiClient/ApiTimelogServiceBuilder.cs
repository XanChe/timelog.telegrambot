using Timelog.ApiClient.Services;
using Timelog.Core;
using Timelog.Core.Entities;
using Timelog.Core.Services;
using Timelog.Services;

namespace Timelog.ApiClient
{
    /// <summary>
    ///      Api реализация интерфейса ITimelogServiceBuilder
    /// </summary>
    public class ApiTimelogServiceBuilder: ITimelogServiceBuilder
    {
        private readonly IUnitOfWork _unitOfWork;

        public ApiTimelogServiceBuilder(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void UseUserFilter(string userIdentityId)
        {
            _unitOfWork.UseUserFilter(userIdentityId);
        }

        public IStatisticsService CreateStatisticsService()
        {
            return new StatisticsService(_unitOfWork);
        }

        public IUserActivityService CreateUserActivityService()
        {            
            return new ApiUserActivityService(_unitOfWork);
        }

        public IEntityService<Project> CreateProjectService()
        {
            return new EntityService<Project>(_unitOfWork.Projects);
        }

        public IEntityService<ActivityType> CreateActivityTypeService()
        {
            return new EntityService<ActivityType>(_unitOfWork.ActivityTypes);
        }
    }
}
