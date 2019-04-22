using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DashboardWebApp.ViewModels
{
    public class MapNode
    {
        public MapNode(string name, string description, double lat, double lon)
        {
            Name = name;
            Description = description;
            Lat = lat;
            Lon = lon;
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public double Lat { get; set; }
        public double Lon { get; set; }
    }
}
