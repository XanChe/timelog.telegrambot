using Timelog.Core;
using Timelog.Core.Entities;
using Timelog.Core.Services;
using Timelog.Core.ViewModels;
using Timelog.Services;

namespace Timelog.ApiClient.Services
{
    public class ApiUserActivityService : EntityService<UserActivity>, IUserActivityService
    {
        protected readonly IUnitOfWork _unitOfWork;


        public ApiUserActivityService(IUnitOfWork unitOfWork) : base(unitOfWork.Activities)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<ActivityViewModel>> GetActivitiesAsync()
        {
            var activities =  await _unitOfWork.Activities.GetAllAsync();

            return activities.Select(activity => ActivityViewModel.MapFromUserActivity(activity));
           
        }

        public async Task<ActivityViewModel?> GetCurrentActivityIfExistAsync()
        {
            var currentActivity = await _unitOfWork.Activities.getCurrentActivityAsync();
            if (currentActivity != null)
            {
                return (ActivityViewModel)currentActivity;
            }
            return null;
        }

        public async Task<ActivityViewModel?> StartNewActivityAsync(Guid projectId, Guid activityTypeId)
        {


            await _unitOfWork.Activities.StartActivityAsync(projectId, activityTypeId);


            return null;
        }

        public async Task StopCurrentActivityIfExistAsync(string comment)
        {
            await _unitOfWork.Activities.StopActivityAsync(comment);
        }
    }
}
