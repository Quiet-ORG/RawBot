using Newtonsoft.Json;

namespace RawBot.State.Model.Map
{
    public class MapData : IMapData
    {
        [JsonProperty("strMapName")]
        public string Name { get; set; }

        [JsonProperty("strMapFileName")]
        public string FileName { get; set; }

        [JsonProperty("intType")]
        public int Type { get; set; }

        [JsonProperty("areaName")]
        public string AreaName { get; set; }

        [JsonProperty("areaId")]
        public int AreaId { get; set; }

        [JsonProperty("sExtra")]
        public string Extra { get; set; }
    }

    public interface IMapData : IState
    {
        string Name { get; set; }
        string FileName { get; set; }
        int Type { get; set; }
        string AreaName { get; set; }
        int AreaId { get; set; }
        string Extra { get; set; }
    }
}