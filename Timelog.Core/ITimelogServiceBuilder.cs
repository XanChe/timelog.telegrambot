using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timelog.Core.Entities;
using Timelog.Core.Services;

namespace Timelog.Core
{
    public interface ITimelogServiceBuilder
    {
        public void UseUserFilter(string userIdentityId);
        public IStatisticsService CreateStatisticsService();
        public IUserActivityService CreateUserActivityService();       
        public IEntityService<Project> CreateProjectService();     
        public IEntityService<ActivityType> CreateActivityTypeService();
       
    }
}
