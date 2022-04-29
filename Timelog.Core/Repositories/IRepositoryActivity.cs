using Timelog.Core.Entities;

namespace Timelog.Core.Repositories
{
    public interface IRepositoryActivity : IRepositoryGeneric<UserActivity>  
    {        
        public Task<UserActivity?> getCurrentActivityAsync();
        public Task StartActivityAsync(Guid projectId, Guid activityTypeId);
        public Task StopActivityAsync(string comment);
    }
}
