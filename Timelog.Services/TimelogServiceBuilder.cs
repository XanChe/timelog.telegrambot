using Timelog.Core;
using Timelog.Core.Entities;
using Timelog.Core.Services;

namespace Timelog.Services
{
    public class TimelogServiceBuilder: ITimelogServiceBuilder
    {
        private readonly IUnitOfWork _unitOfWork;

        public TimelogServiceBuilder(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void UseUserFilter(Guid userIdentityGuid)
        {
            _unitOfWork.UseUserFilter(userIdentityGuid);
        }

        public IStatisticsService CreateStatisticsService()
        {
            return new StatisticsService(_unitOfWork);
        }

        public IUserActivityService CreateUserActivityService()
        {            
            return new UserActivityService(_unitOfWork);
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
