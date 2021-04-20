using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace StudyRedis.Models
{
    public class AccessHistoryLog
    {
        [Key]
        public int Id { get; set; }
        public string IpAddress { get; set; }
        public DateTime DateTime { get; set; }
        public string AccessPath { get; set; }

        public string SessionId { get; set; }
    }
}
