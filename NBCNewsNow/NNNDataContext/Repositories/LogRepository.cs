using NNNDataContext.Context;
using NNNDataModel.Log;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace NNNDataContext.Repositories
{
    public interface ILogRepository : IBaseRepository<UserLog>
    {
        Task<List<UserLog>> GetByTraceId(string traceId);
    }

    public class LogRepository : BaseRepository<UserLog>, ILogRepository
    {
        public LogRepository(NewsContext dbContext)
           : base(dbContext)
        {
        }
        public async Task<List<UserLog>> GetByTraceId(string traceId)
        {
            return await DbContext.Set<UserLog>()
                        .AsNoTracking()
                        .Where(t => t.TraceId == traceId)
                        .ToListAsync();
        }
    }
}
