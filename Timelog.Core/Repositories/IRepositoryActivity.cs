using Timelog.Core.Entities;

namespace Timelog.Core.Repositories
{
    public interface IRepositoryActivity : IRepositoryGeneric<UserActivity>  
    {
        //public void SetFilterByUser(string userUniqId);
        public Task<UserActivity?> getCurrentActivityAsync();
    }
}
