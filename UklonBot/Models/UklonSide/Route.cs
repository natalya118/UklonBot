using System.Collections.Generic;
using Newtonsoft.Json;

namespace UklonBot.Models.UklonSide
{
    public class Route
    {
        [JsonProperty("comment")]
        public string Comment { get; set; }
        [JsonProperty("entrance")]
        public int Entrance { get; set; }
        [JsonProperty("is_office_building")]
        public bool IsOfficeBuilding { get; set; }
        [JsonProperty("route_points")]
        public List<RoutePoint> RoutePoints { get; set; }
    }
}