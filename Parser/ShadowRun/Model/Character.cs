using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser.ShadowRun.Model
{
    public class Character
    {
        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;
        [JsonProperty("id")]
        public long Id { get; set; }
        [JsonProperty("mainContainer")]
        public long? MainContainer { get; set; } 
    }
}
