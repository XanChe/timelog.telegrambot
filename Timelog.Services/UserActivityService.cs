
using Timelog.Core;
using Timelog.Core.Entities;
using Timelog.Core.Repositories;
using Timelog.Core.Services;
using Timelog.Core.ViewModels;

namespace Timelog.Services
{
    public class UserActivityService: EntityService<UserActivity>, IUserActivityService
    {
        protected readonly IUnitOfWork _unitOfWork;

        public UserActivityService(IUnitOfWork unitOfWork) :base(unitOfWork.Activities)
        {
            _unitOfWork = unitOfWork;            
        }              
        public bool IsDraft(UserActivity activity)
        {
            return activity.Status == ActivityStatus.Draft;
        }
        public bool IsStarted(UserActivity activity)
        {
            return activity.Status == ActivityStatus.Started;
        }
        public bool IsComplite(UserActivity activity)
        {
            return activity.Status == ActivityStatus.Complite;
        }
        public void Start(UserActivity activity)
        {
            Start(activity, DateTime.Now);
        }
        public void Start(UserActivity activity, DateTime customStart)
        {
            if (activity.Status == ActivityStatus.Draft)
            {
                activity.Status = ActivityStatus.Started;
                activity.StartTime = customStart;
            }
        }
        public void Stop(UserActivity activity)
        {
            Stop(activity, DateTime.Now);
        }
        public void Stop(UserActivity activity, DateTime customEnd)
        {
            if (activity.Status == ActivityStatus.Started)
            {
                activity.Status = ActivityStatus.Complite;
                activity.EndTime = customEnd;
            }
        }
        public async Task StopPreviousActivityIfExistAsync()
        {
            UserActivity? currentActivity = await _unitOfWork.Activities.getCurrentActivityAsync();

            if (currentActivity != null && IsStarted(currentActivity))
            {
                Stop(currentActivity);
                await _unitOfWork.Activities.UpdateAsync(currentActivity);
                await _unitOfWork.Activities.SaveChangesAsync();
            }
        }
        public virtual async Task<ActivityViewModel?> StartNewActivityAsync(Guid projectId, Guid activityTypeId)
        {
            await StopPreviousActivityIfExistAsync();
            var newUserActivity = new UserActivity()
            {
                ProjectId = projectId,
                ActivityTypeId = activityTypeId
            };
            
            Start(newUserActivity);

            await _unitOfWork.Activities.CreateAsync(newUserActivity);
            await _unitOfWork.Activities.SaveChangesAsync();

            return newUserActivity;
        }
        public async Task<ActivityViewModel?> GetCurrentActivityIfExistAsync()
        {
            return await _unitOfWork.Activities.getCurrentActivityAsync();
        }

        public async Task StopCurrentActivityIfExistAsync(string comment)
        {
            UserActivity? currentActivity = await _unitOfWork.Activities.getCurrentActivityAsync();
            if (currentActivity != null)
            {
                currentActivity.Comment = comment;
                Stop(currentActivity);

                await _unitOfWork.Activities.UpdateAsync(currentActivity);
                await _unitOfWork.Activities.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<ActivityViewModel?>> GetActivitiesAsync()
        {
            
            return (await _unitOfWork.Activities.GetAllAsync()).Select(item => (ActivityViewModel?)item);
        }
    }
}
