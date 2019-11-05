using HtmlAgilityPack;
using Microsoft.Extensions.Options;
using NBCNewsNow.DataModel;
using NNNDataContext.Repositories;
using NNNDataModel;
using NNNDataModel.Helpers;
using NNNLogger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;

namespace NBCNewsNow.Services
{
    public interface INewsObtainerService 
    {
        Task<List<News>> ObtainNews();
    }

    public class NewsObtainerService : INewsObtainerService
    {
        private readonly INewsRepository _newsRepo;
        private readonly INNNLogger _logger;
        private readonly Settings _settings;

        public NewsObtainerService(INewsRepository newsRepo, INNNLogger<NewsObtainerService> logger, IOptions<Settings> settings)
        {
            _newsRepo = newsRepo;
            _logger = logger;
            _settings = settings.Value;
        }

        public async Task<List<News>> ObtainNews()
        {
            _logger.Log(LogPriority.Info, "Obtaining news...");
            List<News> obtainedNews = Obtain();
            _logger.Log(LogPriority.Info, $"Obtained {obtainedNews.Count} news.");

            if (!obtainedNews.IsNullOrEmpty())
            { 
                List<News> insertedNews = await _newsRepo.AddMany(obtainedNews);
                int insertedCount = obtainedNews.Count - insertedNews.Count;
                int updatedCount = obtainedNews.Count - insertedCount;

                _logger.Log(LogPriority.Info, $"Inserted {insertedCount} and updated {updatedCount} news.");
            }

            return obtainedNews;
        }

        private List<News> Obtain() 
        {
            HtmlWeb web = new HtmlWeb();
            var htmlDoc = web.Load(_settings.NewsUrl);
            var nodes = htmlDoc.DocumentNode.SelectNodes("//body/div/div/div/div/div/div/div/section/div/div/div/div/div/h3/a")?.Where(a => a.Attributes["class"]?.Value == "font-hed fw4 mb1 mt4 lh-none f5 gray-100 vilynx_disabled");

            List<News> news = new List<News>();
            if (nodes != null)
            {
                foreach (var node in nodes)
                {
                    News n = new News
                    {
                        CreatedTime = DateTime.UtcNow,
                        Title = WebUtility.HtmlDecode(node.InnerText),
                        Link = node.Attributes["href"]?.Value
                    };

                    news.Add(n);
                }
            }

            return news;
        }
    }
}
