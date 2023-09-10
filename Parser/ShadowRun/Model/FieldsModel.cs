using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser.ShadowRun.Model
{
    public class FieldsModel
    {
        [JsonProperty("1")]
        public string FieldOne { get; set; } = string.Empty;
        [JsonProperty("2")]
        public string FieldTwo { get; set; } = string.Empty;
        [JsonProperty("3")]
        public string FieldThree { get; set; } = string.Empty;
        [JsonProperty("4")]
        public int FieldFour { get; set; }
        [JsonProperty("5")]
        public int FieldFive { get; set; }
        [JsonProperty("6")]
        public bool FieldSix { get; set; }
        [JsonProperty("9")]
        public bool FieldNine { get; set; }
    }
}
