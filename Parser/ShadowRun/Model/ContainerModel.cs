using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser.ShadowRun.Model
{
    public class ContainerModel
    {
        [JsonProperty("type")]
        public int TypeProp { get; set; }
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }
        [JsonProperty("controllerID")]
        public long ControlleId { get; set; }
        [JsonProperty("ownerID")]
        public long OwnerID { get; set; }
        [JsonProperty("resources")]
        public ResourcesModel Resources { get; set; } = new();
        [JsonProperty("fields")]
        public FieldsModel Fieelds { get; set; } = new();
        [JsonIgnore]
        public bool IsInitialized { get; set; } = false;
    }
}
