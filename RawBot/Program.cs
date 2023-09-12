using RawBot.Authentication;
using RawBot.Configuration;
using RawBot.Logging;
using RawBot.Runtime;
using RawBot.State;
using RawBot.State.Model.Entities;
using Sharprompt;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RawBot
{
    public class Program
    {
        public const string PluginsFile = "plugins.txt";
        public const string ExitString = ".exit";

        public static async Task Main(string[] args)
        {

            var config = ConfigurationLoader.Load();
            var auth = new Authenticator(config.LoginUrl, config.UserAgent);
            var login = await auth.LoginAsync(config.Username, config.Password);
            if (!login.User.Success)
            {
                Console.WriteLine($"Login failed: {login.User.StatusMessage}.");
                return;
            }

            var server = Prompt.Select("Select a server", login.Servers, textSelector: s => $"{s.Name} ({s.Players}/{s.MaxPlayers})");
            Console.WriteLine($"Connecting to {server.Name}...");
            var context = new Context();
            context.Game.Username = login.User.Username;
            context.Game.Token = login.User.Token;
            context.Game.Log = new TextWriterLogger { Writer = new StreamWriter("game.log") };
            await context.Game.ConnectAsync(server.Ip, server.Port);
            Console.WriteLine($"Connected to server as: {login.User.Username}");

            if (File.Exists(PluginsFile))
            {
                var lines = await File.ReadAllLinesAsync(PluginsFile);
                foreach (var pluginFile in lines.Where(l => !string.IsNullOrWhiteSpace(l) && File.Exists(l)))
                {
                    await context.Plugins.LoadAsync(pluginFile);
                }
            }

            var input = string.Empty;
            while (input is not ExitString)
            {
                input = Prompt.Input<string>("Bot >");
                if (input == "clear")
                {
                    Console.Clear();
                }
                else if (input == "server")
                {
                    Console.WriteLine($"{server.Name}, {server.Ip}, {server.Port}, {context.World.CurrentMap}");
                }
                else if (input == "players")
                {
                    foreach (var player in context.World.Players.Items)
                    {
                        Console.WriteLine($"{player.Username} ({player.Id}, {player.Frame})");
                    }
                }
                else if (input == "monsters")
                {
                    foreach (var monster in context.World.Monsters.Items)
                    {
                        Console.WriteLine($"{monster.Name} ({monster.MapId}, {monster.Frame})");
                    }
                }
                else if (input == "cellmon")
                {
                    foreach (var monster in context.World.Monsters.Items)
                    {
                        if (monster.Frame == context.World.Player.Frame)
                        {
                            Console.WriteLine($"{monster.Name} ({monster.MapId}, {monster.Frame})");
                        }
                        else
                        {
                            Console.WriteLine("No monsters in current cell.");
                        }
                    }
                }
                else if (input == "join")
                {
                    var mapName = Prompt.Input<string>("Map name");
                    await context.World.JoinAsync(mapName);
                }
                else if (input == "currentmap")
                {
                    Console.WriteLine($"{context.World.CurrentMap.AreaName} ({context.World.Player.Frame}, {context.World.Player.Pad})");
                }
                else if (input == "jump")
                {
                    var cellName = Prompt.Input<string>("Cell");
                    var padName = Prompt.Input<string>("Pad");
                    await context.World.MoveToCellAsync(cellName, padName, true);
                }
                else if (input == "items")
                {
                    foreach (var item in context.World.Inventory.Items)
                    {
                        Console.WriteLine($"{item.Name}x{item.Quantity} ({item.Id})");
                    }
                }
                else if (input == "attack")
                {
                    var monster = context.World.Monsters.InFrame(context.World.Frame).FirstOrDefault(m => m.State != EntityState.Dead);
                    if (monster is null)
                    {
                        Console.WriteLine("No monster to attack.");
                    }
                    else
                    {
                        Console.WriteLine($"Attacking {monster.Name} ({monster.MapId})...");
                        await context.World.AttackAsync("aa", new[] { monster });
                    }
                }
                else if (input == "quest")
                {
                    var questId = Prompt.Input<int>("Quest id");
                    await context.World.LoadQuestsAsync(questId);
                    if (context.World.Quests.TryGetValue(questId, out var quest))
                    {
                        Console.WriteLine($"{quest.Name} ({quest.Id}): {quest.Description}");
                    }
                }
                else if (input == "script")
                {
                    var fileName = Prompt.Input<string>("Script file");
                    if (File.Exists(fileName))
                    {
                        var source = await File.ReadAllTextAsync(fileName);
                        context.Scripts.Load(Path.GetFileName(fileName), source);
                        context.Scripts.Start(context);
                    }
                    else
                    {
                        Console.WriteLine("File not found.");
                    }
                }
                else if (input == "script stop")
                {
                    context.Scripts.Stop();
                }
                else if (input == "script state")
                {
                    Console.WriteLine($"Script running: {context.Scripts.Running}");
                }
                else if (input == "plugin")
                {
                    var fileName = Prompt.Input<string>("Plugin file");
                    if (File.Exists(fileName))
                    {
                        await context.Plugins.LoadAsync(fileName);
                    }
                }
            }
        }
    }
}
