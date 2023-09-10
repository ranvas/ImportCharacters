using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser.ShadowRun.Model
{
    public class UsersJsonModel
    {
        [JsonProperty("masterPassword")]
        public string MasterPassword { get; set; } = string.Empty;
        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }
        [JsonProperty("characters")]
        public Character[] Characters { get; set; } = new Character[0];
        [JsonProperty("players")]
        public Player[] Players { get; set; } = new Player[0];
    }
}
