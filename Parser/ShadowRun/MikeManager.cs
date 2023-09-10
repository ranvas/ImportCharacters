using HtmlParser.Domain;
using Newtonsoft.Json;
using Parser.ShadowRun.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Resources;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Parser.ShadowRun
{
    public class MikeManager : DomainManager
    {
        private MikeOptions _options;
        public MikeManager(MikeOptions options)
        {
            _options = options;
        }

        public override async Task<TResult> ImportAsync<TResult, TModel>(TModel model) where TResult : class
        {
            return (await ImportCharactersAsync((model as HtmlTable) ?? throw new Exception("ImportAsync"))) as TResult ?? throw new Exception("ImportAsync return");
        }

        public async Task<UsersModel> ImportCharactersAsync(HtmlTable model)
        {
            var json = await File.ReadAllTextAsync(_options.InputUsersFile);
            var modelDeserialized = JsonConvert.DeserializeObject<UsersJsonModel>(json) ?? throw new Exception("ModelDeserialized");
            var containers = Directory.EnumerateFiles(_options.OutputFolder, "*.json");

            var work = new UsersModel(modelDeserialized, model, containers.ToList());

            var dictionaryCharacters = new Dictionary<long, Character>();
            var dictionaryPlayers = new Dictionary<long, Player>();
            var dictionaryContainers = work.Containers;

            foreach (var item in work.ModelDeserialized.Characters)
            {
                if (dictionaryCharacters.ContainsKey(item.Id))
                    throw new Exception("dublicate key Characters");
                dictionaryCharacters.Add(item.Id, item);
            }
            foreach (var item in work.ModelDeserialized.Players)
            {
                if (dictionaryPlayers.ContainsKey(item.Id))
                    throw new Exception("dublicate key Players");
                dictionaryPlayers.Add(item.Id, item);
            }

            foreach (var item in work.Table.Items)
            {
                var id = item.GetInt("#", model.Headers);
                //containers
                var container = CreateUserContainer(item, model.Headers, id);
                if (container == null)
                    continue;
                var fileName = $"{_options.OutputFolder}\\{id}.json";
                dictionaryContainers[fileName] = container;
                //usersmodel
                //player
                var player = CreatePlayer(item, model.Headers, id);
                dictionaryPlayers[id] = player;
                //character
                var character = CreateCharacter(item, model.Headers, id);
                dictionaryCharacters[id] = character;
            }
            work.ModelDeserialized.Characters = dictionaryCharacters.Values.ToArray();
            work.ModelDeserialized.Players = dictionaryPlayers.Values.ToArray();
            work.ModelDeserialized.Timestamp = MikeHelper.GetCurrentTime();
            return work;
        }

        private ContainerModel? CreateUserContainer(HtmlRow item, HtmlRow headers, int id)
        {
            var timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            var status = item.GetString("Статус", headers);
            if (status.ToLower() != "принята")
            {
                return null;
            }
            var container = new ContainerModel
            {
                Id = id,
                TypeProp = 3,
                Timestamp = timestamp,
                ControlleId = -1,
                OwnerID = id,
                Resources = new ResourcesModel { Field1 = 1 },
                Fieelds = new FieldsModel
                {
                    FieldOne = item.GetString(@"Метатип персонажа (&quot;Чьих будешь?&quot;)", headers),
                    FieldTwo = item.GetString(@"Архетип (&quot;Что выигрывал?&quot;)", headers),
                    FieldThree = item.GetString("Прописка", headers),
                    FieldFive = item.GetInt("Эссенс", headers),
                    FieldSix = item.GetString("Фиксер", headers) == "+"
                },
                IsInitialized = true
            };
            return container;
        }

        private Character CreateCharacter(HtmlRow item, HtmlRow headers, int id)
        {
            var character = new Character
            {
                MainContainer = id,
                Name = item.GetString("Имя персонажа", headers),
                Id = id
            };
            return character;
        }

        private Player CreatePlayer(HtmlRow item, HtmlRow headers, int id)
        {
            var password = MikeHelper.RandomString(4);
            var playerSummary = item.GetHtml("Игрок", headers).Split("<br>"); 
            var player = new Player
            {
                Id = id,
                ActiveCharacter = id,
                Characters = new long[1] { id },
                Name = playerSummary?[3] ?? "Ошибка имени",
                NickName = item.GetString("Телеграмм-аккаунт", headers),
                Password = password,
                PasswordHash = MikeHelper.ComputeSHA256(password)
            };
            return player;
        }

    }
}
