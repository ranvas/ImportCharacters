using AngleSharp;
using AngleSharp.Dom;
using CommonExcel;
using HtmlAgilityPack;
using Newtonsoft.Json;
using Parser.AllRpg;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Parser.Html
{
    public class AllrpgParser
    {
        private int _projectId;
        public AllrpgParser(int projectId)
        {
            _projectId = projectId;
        }
        private async Task<string> GetHtml()
        {
            var url = "https://www.allrpg.info/dynrequest.php";
            var client = new HttpClient();
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("action", "get_roles_list"),
                new KeyValuePair<string, string>("command", "create"),
                new KeyValuePair<string, string>("project_id", _projectId.ToString()),
                new KeyValuePair<string, string>("obj_type", "group"),
                new KeyValuePair<string, string>("obj_id","all")
            });
            var response = await client.PostAsync(url, content);
            if (!response.IsSuccessStatusCode)
            {
                return "fail";
            }
            var result = await response.Content.ReadAsStringAsync();
            return result;
        }

        private class AllRpgResponse
        {
            public string? Response { get; set; }
            public string? Response_data { get; set; }
        }

        public async Task SaveHtmlToFile(string outputFileName)
        {
            var html = await GetHtml();
            var json = JsonConvert.DeserializeObject<AllRpgResponse>(html);
            var input = json?.Response_data ?? string.Empty;
            var list = GetAllGroups(input);
            var excelDto = new List<AllRpgCharacter>();
            foreach (var item in list)
            {
                foreach (var subItem in item.Characters)
                {
                    subItem.Group = item.Name;
                    excelDto.Add(subItem);
                }
            }

            ExcelWriter.SaveReportTo(excelDto, outputFileName);
        }

        public void SeeHtmls(string inputFileName)
        {
            var list = GetAllGroups(inputFileName);
            foreach (var item in list)
            {
                Console.WriteLine(item.Name);
            }
            Console.WriteLine($"total groups {list.Count}");
        }


        private List<AllRpgGroup> GetAllGroups(string input)
        {
            Console.OutputEncoding = Encoding.UTF8;

            var list = new List<AllRpgGroup>();
            var convertedHtml = ConvertUnicode(input);
            HtmlDocument hap = new HtmlDocument();
            hap.LoadHtml(convertedHtml);
            var parentNodes = hap.DocumentNode.SelectNodes(EAllRpgNames.GroupSelector);
            if (parentNodes != null)
            {
                foreach (var parentNode in parentNodes)
                {
                    var item = new AllRpgGroup { InnerHtml = parentNode.InnerHtml };
                    var name = GetSingleInnerText(parentNode, EAllRpgNames.GroupName);
                    var description = GetSingleInnerText(parentNode, EAllRpgNames.GroupDescription);
                    var id = GetId(parentNode);
                    var parent = GetParentPathIfExists(parentNode);
                    if (!string.IsNullOrEmpty(parent))
                    {
                        name = parent;
                    }
                    item.Id = id;
                    item.Name = name;
                    item.Description = description;
                    item.Characters = GetCharacters(parentNode);
                    list.Add(item);
                }
            }
            return list;
        }

        private List<AllRpgCharacter> GetCharacters(HtmlNode node)
        {
            var characters = new List<AllRpgCharacter>();
            foreach (var child in node.ChildNodes)
            {
                if (child.Attributes["class"]?.Value?.Contains(EAllRpgNames.RoleMain) ?? false)
                {
                    var name = GetSingleInnerText(child, EAllRpgNames.CharacterName);
                    var description = GetSingleInnerText(child, EAllRpgNames.CharacterDescription);
                    var charUrl = string.IsNullOrEmpty(name) ?
                        string.Empty :
                        child.Descendants().FirstOrDefault(c => c.Name == "a")?.Attributes["href"]?.Value ?? string.Empty;

                    var rolePlayer = child.Descendants().Where(n => n.Attributes["class"]?.Value == EAllRpgNames.RolePlayer);
                    var rolePlayerName = string.Empty;
                    var rolePlayerUrl = string.Empty;
                    if (rolePlayer.Count() == 1)
                    {
                        rolePlayerName = rolePlayer.First().InnerText;
                        var rolePlayerUrlNode = rolePlayer.First().Descendants().LastOrDefault(n => n.Name == "a" && !string.IsNullOrEmpty(n.Attributes["href"]?.Value));
                        rolePlayerUrl = rolePlayerUrlNode?.Attributes["href"]?.Value ?? string.Empty;
                    }
                    if (rolePlayer.Count() > 1)
                    {
                        rolePlayerName = "ошибка";
                    }
                    var player = new AllRpgCharacter()
                    {
                        RolePlayerName = rolePlayerName,
                        Description = description,
                        CharacterName = name,
                        UrlChar = charUrl,
                        UrlPlayer = rolePlayerUrl
                    };
                    characters.Add(player);
                }
            }
            return characters;
        }

        private string GetParentPathIfExists(HtmlNode node)
        {
            var searchNode = node
                .Descendants()
                .FirstOrDefault(x => x.Attributes["class"]?.Value == EAllRpgNames.GroupNamePath);
            var name = string.Empty;
            if (searchNode != null)
            {
                var pathElements = searchNode
                    .Descendants()
                    .Where(x => x.Attributes["class"]?.Value == EAllRpgNames.GroupNamePathElement);
                foreach (var item in pathElements)
                {
                    if (string.IsNullOrEmpty(name))
                    {
                        name = item.InnerText;
                    }
                    else
                    {
                        name = $"{name}->{item.InnerText}";
                    }
                }
                return name;
            }
            return string.Empty;
        }

        private string GetSingleInnerText(HtmlNode node, string className)
        {
            var searchNode = node
                .Descendants()
                .FirstOrDefault(x => x.Attributes["class"]?.Value == className);

            return searchNode?.InnerText ?? string.Empty;
        }

        private string GetId(HtmlNode node)
        {
            var id = node.Attributes["data-obj-id"]?.Value;
            return id ?? string.Empty;
        }

        private string ConvertUnicode(string input)
        {
            var rx = new Regex(@"\\[uU]([0-9A-Fa-f]{4})");
            var result = rx.Replace(input, delegate (Match match) { return ((char)Int32.Parse(match.Value.Substring(2), NumberStyles.HexNumber)).ToString(); });
            result = result.Replace(@"\", "");
            return result;
        }

    }
}
