using DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DashboardWebApp.Models
{
    [NotMapped]
    public class TrapLog
    {
        public int ManagerId { get; set; }
        public int SourceRsuId { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Type { get; set; }
        public string Message { get; set; }

        public static TrapLog Parse(TrapLogDto mld, Manager manager)
        {
            return new TrapLog
            {
                ManagerId = manager.Id,
                SourceRsuId = mld.SourceRsu,
                TimeStamp = mld.TimeStamp,
                Type = mld.Type,
                Message = mld.Message
            };
        }
    }
}
