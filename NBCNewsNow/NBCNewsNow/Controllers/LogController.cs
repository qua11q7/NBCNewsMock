using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NBCNewsNow.Filters;
using NNNDataContext.Repositories;
using NNNDataModel;
using NNNDataModel.Helpers;
using NNNDataModel.Log;

namespace NBCNewsNow.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [ServiceFilter(typeof(ResponseFilter))]
    public class LogController : ControllerBase
    {
        private readonly ILogRepository _logRepo;
        private readonly ITraceRepository _traceRepo;

        public LogController(ILogRepository logRepo, ITraceRepository traceRepo)
        {
            _logRepo = logRepo;
            _traceRepo = traceRepo;
        }

        #region TraceLogs
        [HttpGet("trace")]
        [Produces("text/plain", "application/json")]
        public async Task<IActionResult> GetTraceLogs([FromQuery] int skip = 0, [FromQuery] int take = 100, [FromQuery] bool descending = true)
        {
            var result = HttpContext.Items["result"] as ResponseModel;
            if (take > 1000)
                take = 1000;

            var traceLogs = await _traceRepo.GetAll().OrderByDescending(t => t.CreatedTime).Skip(skip).Take(take).ToListAsync();
            if (traceLogs.IsNullOrEmpty())
            {
                result.SuccessMessage = "No trace log found.";
            }
            else
            {
                result.SuccessMessage = $"{traceLogs.Count} trace log{(traceLogs.Count > 1 ? "s" : "")} found.";
                result.Result = CreateMessage(traceLogs, descending);
            }

            return Ok(result);
            //return StatusCode(406); // Not Acceptable
        }

        [HttpGet("trace/{traceId}")]
        public async Task<IActionResult> GetTrace(string traceId, [FromQuery] bool descending = true)
        {
            var result = HttpContext.Items["result"] as ResponseModel;

            if (traceId.IsNullOrEmpty()) 
            {
                result.ErrorMessage = $"traceId cannot be null or empty.";
                return BadRequest(result);
            }

            var traceLogs = await _traceRepo.GetByTraceId(traceId);
            if (traceId.IsNullOrEmpty())
            {
                result.SuccessMessage = "No trace log found.";
            }
            else
            {
                result.SuccessMessage = $"{traceLogs.Count} log{(traceLogs.Count > 1 ? "s" : "")} found.";
                result.Result = CreateMessage(traceLogs, descending);
            }

            return Ok(result);
        }
        #endregion

        #region InfoLogs
        [HttpGet("info")]
        public async Task<IActionResult> GetInfoLogs([FromQuery] int skip = 0, [FromQuery] int take = 0, [FromQuery] bool descending = true)
        {
            var result = HttpContext.Items["result"] as ResponseModel;
            if (take > 1000)
                take = 1000;

            var logs = await _logRepo.GetAll().OrderByDescending(t => t.CreatedTime).Skip(skip).Take(take).ToListAsync();
            if (logs.IsNullOrEmpty())
            {
                result.SuccessMessage = "No log found.";
            }
            else
            {
                result.SuccessMessage = $"{logs.Count} log{(logs.Count > 1 ? "s" : "")} found.";
                result.Result = CreateMessage(logs, descending);
            }

            return Ok(result);
        }

        [HttpGet("info/{traceId}")]
        public async Task<IActionResult> GetTraceInfo(string traceId, [FromQuery] bool descending = true)
        {
            var result = HttpContext.Items["result"] as ResponseModel;

            if (traceId.IsNullOrEmpty())
            {
                result.ErrorMessage = $"traceId cannot be null or empty.";
                return BadRequest(result);
            }

            var logs = await _logRepo.GetByTraceId(traceId);
            if (traceId.IsNullOrEmpty())
            {
                result.SuccessMessage = "No trace log found.";
            }
            else
            {
                result.SuccessMessage = $"{logs.Count} log{(logs.Count > 1 ? "s" : "")} found.";
                result.Result = CreateMessage(logs, descending);
            }

            return Ok(result);
        }
        #endregion

        private string CreateMessage<TLog>(List<TLog> logs, bool descending = true) where TLog : BaseLog 
        {
            if (!descending)
                logs = logs.OrderBy(log => log.CreatedTime).ToList();

            return string.Join("\n", logs.Select(log => log.ToString()));
        }
    }
}