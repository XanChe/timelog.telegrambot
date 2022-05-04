using Timelog.Core.Entities;
using Timelog.Core.ViewModels;

namespace Timelog.Core.Services
{
    public interface IUserActivityService : IEntityService<UserActivity>
    {
        //public Task StopPreviousActivityIfExistAsync();
        public Task<ActivityViewModel?> StartNewActivityAsync(Guid projectId, Guid activityTypeId);
        public Task<ActivityViewModel?> GetCurrentActivityIfExistAsync();
        public Task StopCurrentActivityIfExistAsync(string comment);
        public Task<IEnumerable<ActivityViewModel>> GetActivitiesAsync();
    }
}
