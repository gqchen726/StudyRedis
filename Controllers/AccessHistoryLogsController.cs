using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudyRedis.Models;
using StudyRedis.Services;

namespace StudyRedis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccessHistoryLogsController : ControllerBase
    {
        private readonly StudyRedisContext _context;
        private IAccessHistoryLogService _service { get; set; }

        public AccessHistoryLogsController(StudyRedisContext context,IAccessHistoryLogService service)
        {
            _context = context;
            _service = service;
        }

        // GET: api/AccessHistoryLogs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AccessHistoryLog>>> GetAccessHistoryLogs()
        {
            return await _context.AccessHistoryLogs.ToListAsync();
        }


        [HttpGet("getPV")]
        public long GetPageView()
        {
            _service.AddAccessHistoryLogOfRedisWithEntity(HttpContext);
            return _service.CountOfPageView();
        }


        [HttpGet("getUV")]
        public long GetUniqueVisiter()
        {
            _service.AddAccessHistoryLogOfRedisWithEntity(HttpContext);
            return _service.CountOfUniqueVisitor();
        }
        [HttpGet("getPVOfCurrentPage/{path}")]
        public long GetPageViewOfCurrentPage(string Path)
        {
            /*string path =  HttpContext.Request.Path;*/
            _service.AddAccessHistoryLogOfRedisWithEntity(HttpContext);
            string path = "/api/AccessHistoryLogs/" + Path;
            return _service.CountOfPageViewForPagePath(path);
        }


        [HttpGet("getUVOfCurrentPage/{path}")]
        public long GetUniqueVisiterOfCurrentPage(string Path)
        {
            /*string path = HttpContext.Request.Path;*/
            _service.AddAccessHistoryLogOfRedisWithEntity(HttpContext);
            string path = "/api/AccessHistoryLogs/" + Path;
            return _service.CountOfUniqueVisitorForPagePath(path);
        }

        [HttpGet("getAll")]
        public Task<List<AccessHistoryLog>> GetAllAccessHistoryLog()
        {
            _service.AddAccessHistoryLogOfRedisWithEntity(HttpContext);
            return _service.FindAllAccessHistoryLog();
        }

        [HttpGet("access")]
        public string AddAccessHistoryLog()
        {
            return _service.AddAccessHistoryLogOfRedisWithEntity(HttpContext);
        }

        /*[HttpGet("access1")]
        public AccessHistoryLog AddAccessHistoryLog1()
        {
            _service.AddAccessHistoryLogOfRedis(HttpContext);
            return _service.AddAccessHistoryLog1(HttpContext);
        }*/




        // GET: api/AccessHistoryLogs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AccessHistoryLog>> GetAccessHistoryLog(int id)
        {
            var accessHistoryLog = await _context.AccessHistoryLogs.FindAsync(id);

            if (accessHistoryLog == null)
            {
                return NotFound();
            }

            return accessHistoryLog;
        }

        // PUT: api/AccessHistoryLogs/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAccessHistoryLog(int id, AccessHistoryLog accessHistoryLog)
        {
            if (id != accessHistoryLog.Id)
            {
                return BadRequest();
            }

            _context.Entry(accessHistoryLog).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccessHistoryLogExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/AccessHistoryLogs
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<AccessHistoryLog>> PostAccessHistoryLog(AccessHistoryLog accessHistoryLog)
        {
            _context.AccessHistoryLogs.Add(accessHistoryLog);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAccessHistoryLog", new { id = accessHistoryLog.Id }, accessHistoryLog);
        }

        // DELETE: api/AccessHistoryLogs/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<AccessHistoryLog>> DeleteAccessHistoryLog(int id)
        {
            var accessHistoryLog = await _context.AccessHistoryLogs.FindAsync(id);
            if (accessHistoryLog == null)
            {
                return NotFound();
            }

            _context.AccessHistoryLogs.Remove(accessHistoryLog);
            await _context.SaveChangesAsync();

            return accessHistoryLog;
        }

        private bool AccessHistoryLogExists(int id)
        {
            return _context.AccessHistoryLogs.Any(e => e.Id == id);
        }
    }
}
