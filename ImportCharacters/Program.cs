// See https://aka.ms/new-console-template for more information
using HtmlParser;
using HtmlParser.Domain;
using HtmlParser.Html;
using ImportCharacters;
using Newtonsoft.Json;
using Parser.ShadowRun;
using Parser.ShadowRun.Model;

await RunAsync();

Console.WriteLine("in the end");

static async Task RunAsync()
{
    var fileNameJson = @"appsettings.json";
    var jsonText = await File.ReadAllTextAsync(fileNameJson);
    var mikeOptions = JsonConvert.DeserializeObject<MikeOptions>(jsonText) ?? throw new Exception("options null");
    var htmlText = await File.ReadAllTextAsync(mikeOptions.InputHtmlFile);
    try
    {
        var parser = new HtmlAdapter<HtmlTable>(new SluchajHtmlClient(htmlText));

        var test = await parser.ImportExampleAsync<UsersModel, MikeManager>(new MikeManager(mikeOptions));
        var diskWriter = new DiskWriter();
        await diskWriter.Write(mikeOptions, test);
    }
    catch (Exception e)
    {

        throw;
    }
}