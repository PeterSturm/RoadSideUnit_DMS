using DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DashboardWebApp.Models
{
    [NotMapped]
    public class ManagerLog
    {
        public int ManagerId { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Type { get; set; }
        public string Message { get; set; }

        public static ManagerLog Parse(ManagerLogDto mld, Manager manager)
        {
            return new ManagerLog
            {
                ManagerId = manager.Id,
                TimeStamp = mld.TimeStamp,
                Type = mld.Type,
                Message = mld.Message
            };
        }
    }
}
