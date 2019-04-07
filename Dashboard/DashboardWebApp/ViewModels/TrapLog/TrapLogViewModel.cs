using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DashboardWebApp.ViewModels.TrapLog
{
    public class TrapLogViewModel : Models.TrapLog
    {
        public Models.RSU SourceRSU { get; set; }

        public TrapLogViewModel(Models.RSU rsu, Models.TrapLog trapLog)
        {
            SourceRSU = rsu;
            ManagerId = trapLog.ManagerId;
            SourceRsuId = trapLog.SourceRsuId;
            TimeStamp = trapLog.TimeStamp;
            Type = trapLog.Type;
            Message = trapLog.Message;
        }
    }
}
