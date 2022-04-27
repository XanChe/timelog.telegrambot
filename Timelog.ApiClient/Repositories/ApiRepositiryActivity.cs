using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timelog.Core.Entities;
using Timelog.Core.Repositories;

namespace Timelog.ApiClient.Repositories
{
    public class ApiRepositiryActivity : ApiRepositoryGeneric<UserActivity>, IRepositoryActivity
    {
        public Guid UserGuid => throw new NotImplementedException();

        public ApiRepositiryActivity(HttpClient client, string actionPrefix): base(client, actionPrefix)
        {
            
        }

        public Task<UserActivity?> getCurrentActivityAsync()
        {
            throw new NotImplementedException();
        }
    }
}
