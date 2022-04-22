using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timelog.Core.ViewModels;
using Timelog.Core.Entities;

namespace Timelog.Core.Repositories
{
    public interface IRepositirySatistics
    {
        public void SetUser(Guid userIdentityGuid);
        public Task<TotalStatisticsVewModel?> GetTotalStatisticsForPeriodAsync(DateTime fromDate, DateTime toDate);
        public Task<IEnumerable<ProjectStatViewModel>> GetProjectStatsForPeriodAsync(DateTime fromDate, DateTime toDate);
        public Task<IEnumerable<ActivityTypeStatViewModel>> GetActivityTypeStatsForPeriodAsync(DateTime fromDate, DateTime toDate);
    }
}
