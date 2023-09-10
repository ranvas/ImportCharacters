using HtmlParser.Domain;
using Newtonsoft.Json;
using Parser.ShadowRun.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser.ShadowRun
{
    public class UsersModel 
    {
        public UsersModel(UsersJsonModel jsonModel, HtmlTable table, List<string> containers)
        {
            ModelDeserialized = jsonModel;
            Table = table;
            Containers = new Dictionary<string, ContainerModel>();
            containers.ForEach(t => Containers.Add(t, new ContainerModel()));
        }
        public HtmlTable Table { get; set; }
        public UsersJsonModel ModelDeserialized { get; set; }
        public Dictionary<string, ContainerModel> Containers { get; set; }
    }
}
