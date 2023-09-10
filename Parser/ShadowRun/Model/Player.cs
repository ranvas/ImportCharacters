using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser.ShadowRun.Model
{
    public class Player
    {
        [JsonProperty("id")]
        [Display(Name = "Логин")]
        public long Id { get; set; }
        [Display(Name="Имя персонажа")]
        [JsonProperty("name")]
        public string  Name { get; set; } = string.Empty;
        [Display(Name = "Никнейм")]
        [JsonProperty("nickname")]
        public string NickName { get; set; } = string.Empty;
        [JsonProperty("characters")]
        public long[] Characters { get; set; } = new long[0];
        [Display(Name = "ID Персонажа")]
        [JsonProperty("activeCharacter")]
        public long ActiveCharacter { get; set; } = 0;
        [JsonProperty("password")]
        public string PasswordHash { get; set; } = string.Empty;
        [JsonIgnore]
        [Display(Name = "Пароль")]
        public string Password { get; set; } = string.Empty;
    }
}
