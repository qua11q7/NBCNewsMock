using NNNDataContext.Context;
using NNNDataModel;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using NNNDataModel.Helpers;
using System;

namespace NNNDataContext.Repositories
{
    public interface INewsRepository : IBaseRepository<News> 
    {
        Task<News> GetByTitle(string title);
        Task<List<News>> GetByTitles(IEnumerable<string> titles);
        Task<List<News>> AddMany(IEnumerable<News> news);
    }

    public class NewsRepository : BaseRepository<News>, INewsRepository
    {
        public NewsRepository(NewsContext dbContext)
           : base(dbContext)
        {
        }

        public async Task<News> GetByTitle(string title)
        {
            return await DbContext.Set<News>().AsNoTracking().FirstOrDefaultAsync(n => n.Title == title);
        }


        public async Task<List<News>> GetByTitles(IEnumerable<string> titles)
        {
            var existingNews = await DbContext.Set<News>().AsNoTracking().Where(n => titles.Contains(n.Title)).ToListAsync();
            return existingNews ?? new List<News>();
        }

        public async Task<List<News>> AddMany(IEnumerable<News> news)
        {
            var existingNews = await GetByTitles(news.Select(n => n.Title));
            if (!existingNews.IsNullOrEmpty())
            {
                existingNews.ForEach(en => en.UpdatedTime = DateTime.UtcNow);
                DbContext.Set<News>().UpdateRange(existingNews);
                await DbContext.SaveChangesAsync();
            }

            List<News> newNews = new List<News>();
            foreach (News n in news)
            {
                if (existingNews.Exists(en => en.Title == n.Title))
                    continue;
                newNews.Add(n);
            }

            if (!newNews.IsNullOrEmpty()) 
            {
                await DbContext.AddRangeAsync(newNews);
                await DbContext.SaveChangesAsync();
            }

            return newNews;
        }
    }
}
