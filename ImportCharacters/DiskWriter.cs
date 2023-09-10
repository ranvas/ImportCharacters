using CommonExcel;
using Newtonsoft.Json;
using Parser.ShadowRun;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportCharacters
{
    public class DiskWriter
    {
        public async Task Write(MikeOptions options, UsersModel model)
        {
            var usersJson = JsonConvert.SerializeObject(model.ModelDeserialized, Formatting.Indented);
            await File.WriteAllTextAsync($"{options.InputUsersFile}.scripted", usersJson);
            ExcelWriter.SaveReportTo(model.ModelDeserialized.Players.ToList(), options.OutputExcelName);
            foreach (var item in model.Containers)
            {
                if (!item.Value.IsInitialized)
                { continue; }
                var containerJson = JsonConvert.SerializeObject(item.Value, Formatting.Indented);
                await File.WriteAllTextAsync($"{item.Key}", containerJson);
            }
        }
    }
}
