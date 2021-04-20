using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StudyRedis.Models;
using Microsoft.AspNetCore.Http;
namespace StudyRedis.Services
{
    public interface IAccessHistoryLogService
    {
        string AddAccessHistoryLog(AccessHistoryLog newAccessHistoryLog);
        string AddAccessHistoryLogOfEntity(HttpContext httpContext, string sessionId);
        Task<List<AccessHistoryLog>> FindAllAccessHistoryLog();
        long CountOfPageView();
        long CountOfPageViewForPagePath(string Path);
        long CountOfUniqueVisitor();
        long CountOfUniqueVisitorForPagePath(string Path);
        AccessHistoryLog AddAccessHistoryLog1(HttpContext httpContext);
        ///string AddAccessHistoryLogOfScope(HttpContext httpContext,AccessHistoryLog accessHistoryLog);

        string AddAccessHistoryLogOfRedis(HttpContext httpContext);
        string AddAccessHistoryLogOfRedisWithEntity(HttpContext httpContext);
    }
}
