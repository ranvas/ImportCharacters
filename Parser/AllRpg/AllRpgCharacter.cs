using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser.AllRpg
{
    public class AllRpgCharacter
    {
        [Display(Name="Имя пользователя")]
        public string? RolePlayerName { get; set; }
        [Display(Name = "Имя персонажа")]
        public string? CharacterName { get; set; }
        [Display(Name = "Описание")]
        public string? Description { get; set; }
        [Display(Name = "Имя группы")]
        public string? Group { get; set; }
        [Display(Name = "characterUrl")]
        public string? UrlChar { get; set; }
        [Display(Name = "playerUrl")]
        public string? UrlPlayer { get; set; }
    }
}
