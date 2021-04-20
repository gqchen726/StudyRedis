using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
namespace StudyRedis.Models
{
    public class StudyRedisContext : DbContext
    {
        public DbSet<AccessHistoryLog> AccessHistoryLogs { get; set; }

        public StudyRedisContext(DbContextOptions<StudyRedisContext> options)
            : base (options)
            { }
    }
}
