using Microsoft.EntityFrameworkCore;
using NNNDataContext.Context;
using NNNDataModel.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NNNDataContext.Repositories
{
    public interface ITraceRepository : IBaseRepository<UserTraceLog>
    {
        Task<List<UserTraceLog>> GetByTraceId(string traceId);
    }

    public class TraceRepository : BaseRepository<UserTraceLog>, ITraceRepository
    {
        public TraceRepository(NewsContext dbContext)
           : base(dbContext)
        {
        }

        public async Task<List<UserTraceLog>> GetByTraceId(string traceId)
        {
            return await DbContext.Set<UserTraceLog>()
                        .AsNoTracking()
                        .Where(t => t.TraceId == traceId)
                        .ToListAsync();
        }
    }
}
