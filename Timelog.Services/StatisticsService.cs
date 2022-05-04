using Timelog.Core.Entities;
using Timelog.Core.Repositories;
using Timelog.Core.ViewModels;
using Timelog.Core.Services;
using Timelog.Core;

namespace Timelog.Services
{
    public class StatisticsService: IStatisticsService
    {
        private readonly IUnitOfWork _unitOfWork;
        public StatisticsService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public async Task<TotalStatisticsVewModel> GetTotalStatisticsAsync(DateTime from, DateTime to)
        {
            return await _unitOfWork.Satistics.GetTotalStatisticsForPeriodAsync(from, to) ?? new TotalStatisticsVewModel();
        }

        public async Task<IEnumerable<ProjectStatViewModel>> GetProjectStatisticsAsync( DateTime from, DateTime to)
        {
            return await _unitOfWork.Satistics.GetProjectStatsForPeriodAsync(from, to);
        }

        public async Task<IEnumerable<ActivityTypeStatViewModel>> GetActivityTypeStatisticsAsync(DateTime from, DateTime to)
        {
            return await _unitOfWork.Satistics.GetActivityTypeStatsForPeriodAsync(from, to);
        }
    }
}
