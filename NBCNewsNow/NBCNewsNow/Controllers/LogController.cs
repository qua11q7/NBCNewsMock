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
using NNNDataModel.Logger;

namespace NBCNewsNow.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        public async Task<IActionResult> GetTraceLogs([FromQuery] int skip = 0, [FromQuery] int take = 100, [FromQuery] bool descending = true)
        {
            if (take > 1000)
                take = 1000;

            var traceLogs = await _traceRepo.GetAll().OrderByDescending(t => t.CreatedTime).Skip(skip).Take(take).ToListAsync();

            if (traceLogs.IsNullOrEmpty())
            {
                return NoContent();
            }
            else
            {
                return Ok(CreateMessage(traceLogs, descending));
            }
        }

        [HttpGet("trace/{traceId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetTrace(string traceId, [FromQuery] bool descending = true)
        {
            if (traceId.IsNullOrEmpty())
            {
                return BadRequest("traceId cannot be null or empty.");
            }

            var traceLogs = await _traceRepo.GetByTraceId(traceId);
            if (traceId.IsNullOrEmpty())
            {
                return NoContent();
            }
            else
            {
                return Ok(CreateMessage(traceLogs, descending));
            }
        }
        #endregion

        #region InfoLogs
        [HttpGet("info")]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        public async Task<IActionResult> GetInfoLogs([FromQuery] int skip = 0, [FromQuery] int take = 100, [FromQuery] bool descending = true)
        {
            if (take > 1000)
                take = 1000;

            var logs = await _logRepo.GetAll().OrderByDescending(t => t.CreatedTime).Skip(skip).Take(take).ToListAsync();
            if (logs.IsNullOrEmpty())
            {
                return NoContent();
            }
            else
            {
                return Ok(CreateMessage(logs, descending));
            }
        }

        [HttpGet("info/{traceId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetTraceInfo(string traceId, [FromQuery] bool descending = true)
        {
            if (traceId.IsNullOrEmpty())
            {
                return BadRequest("traceId cannot be null or empty.");
            }

            var logs = await _logRepo.GetByTraceId(traceId);
            if (traceId.IsNullOrEmpty())
            {
                return NoContent();
            }
            else
            {
                return Ok(CreateMessage(logs, descending));
            }
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