using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NBCNewsNow.Filters;
using NBCNewsNow.Services;
using NNNDataContext.Repositories;
using NNNDataModel.Helpers;
using NNNDataModel;
using NNNLogger;
using Microsoft.EntityFrameworkCore;

namespace NBCNewsNow.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [ServiceFilter(typeof(ResponseFilter))]
    public class NewsController : ControllerBase
    {
        private readonly INewsRepository _newsRepo;
        private readonly INNNLogger _logger;
        private readonly INewsObtainerService _newsObtainer;

        public NewsController(INewsRepository newsRepo, INNNLogger<NewsController> logger, INewsObtainerService newsObtainer)
        {
            _newsRepo = newsRepo;
            _logger = logger;
            _newsObtainer = newsObtainer;
        }

        [HttpGet()]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetNews([FromQuery] int skip = 0, [FromQuery] int take = 10, [FromQuery] string query = "", [FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null, [FromQuery] bool descending = true)
        {
            var result = HttpContext.Items["result"] as ResponseModel;
            
            var newsQuery = _newsRepo.GetAll();
            if (descending)
                newsQuery = newsQuery.OrderByDescending(n => n.CreatedTime);
            else
                newsQuery = newsQuery.OrderBy(n => n.CreatedTime);

            if (!query.IsNullOrEmpty())
                newsQuery = newsQuery.Where(n => n.Title.Contains(query, StringComparison.InvariantCultureIgnoreCase));

            if (startDate.HasValue)
                newsQuery = newsQuery.Where(n => n.CreatedTime >= startDate);
            if (endDate.HasValue)
                newsQuery = newsQuery.Where(n => n.CreatedTime <= endDate);

            var news = await newsQuery.Skip(skip).Take(take).ToListAsync();

            if (news.IsNullOrEmpty())
            { 
                result.SuccessMessage = "No news have been found.";
                _logger.Log(LogPriority.Warning, "No news have been found.");
            }
            else
            {
                result.SuccessMessage = $"{news.Count} news have been found.";
                result.Result = news;
            }

            return Ok(result);
        }

        [HttpPost()]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> ObtainNews()
        {
            var result = HttpContext.Items["result"] as ResponseModel;

            var obtainedNews = await _newsObtainer.ObtainNews();
            if (obtainedNews.IsNullOrEmpty())
            {
                _logger.Log(LogPriority.Warning, "Failed to obtain news!");
                result.ErrorMessage = "Failed to obtain news!";
                return BadRequest(result);
            }

            result.SuccessMessage = $"Obtained {obtainedNews.Count} news";
            result.Result = obtainedNews;

            return Ok(result);
        }
    }
}