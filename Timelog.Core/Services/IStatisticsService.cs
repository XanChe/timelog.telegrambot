using Timelog.Core.ViewModels;

namespace Timelog.Core.Services
{
    public interface IStatisticsService
    {
        public Task<TotalStatisticsVewModel> GetTotalStatisticsAsync(DateTime from, DateTime to);
        public Task<IEnumerable<ProjectStatViewModel>> GetProjectStatisticsAsync(DateTime from, DateTime to);
        public Task<IEnumerable<ActivityTypeStatViewModel>> GetActivityTypeStatisticsAsync(DateTime from, DateTime to);
    }
}
