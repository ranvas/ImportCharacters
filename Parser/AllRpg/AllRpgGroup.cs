using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser.AllRpg
{
    public class AllRpgGroup
    {
        public string? Name { get; set; }
        public string? InnerHtml { get; set; }
        public string? Description { get; set; }
        public List<AllRpgCharacter> Characters { get; set; } = new();
        public string? Id { get; set; }

    }
}
