using HtmlParser.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser.ShadowRun
{
    public class MikeOptions : DomainOptions
    {
        public string InputUsersFile { get; set; } = string.Empty;
        public string InputHtmlFile { get; set; } = string.Empty;
        public string OutputFolder { get; set; } = string.Empty;
        public string OutputExcelName { get; set; } = string.Empty;
    }
}
