using NNNDataContext.Context;
using NNNDataModel;

namespace NNNDataContext.Repositories
{
    public interface INewsRepository : IBaseRepository<News> 
    {
    }

    public class NewsRepository : BaseRepository<News>, INewsRepository
    {
        public NewsRepository(NewsContext dbContext)
           : base(dbContext)
        {
        }
    }
}
