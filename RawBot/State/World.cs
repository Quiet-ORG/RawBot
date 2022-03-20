using RawBot.State.Model.Combat;
using RawBot.State.Model.Combat.Skills;
using RawBot.State.Model.Entities;
using RawBot.State.Model.Entities.Stats;
using RawBot.State.Model.Factions;
using RawBot.State.Model.Items;
using RawBot.State.Model.Map;
using RawBot.State.Model.Quests;
using RawBot.State.Model.Shops;
using RawBot.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace RawBot.State
{
    public class World
    {
        private readonly Game _game;
        private readonly Stopwatch _stopwatch = new();

        public const int MaxActionId = 30;

        public int RoomId { get; set; }
        public int ActionId { get; set; }
        public string Frame => Player?.Frame;
        public string Pad => Player?.Pad;

        public string SpawnFrame { get; set; }
        public string SpawnPad { get; set; }

        public IMapData CurrentMap { get; set; }

        public MonsterManager Monsters { get; } = new();
        public PlayerManager Players { get; } = new();
        public IPlayer Player => Players.Get(_game.Username);

        public Dictionary<string, Dictionary<string, float>> SlotStats { get; set; } = new();
        public PlayerStats Stats { get; set; } = new();

        public Inventory<InventoryItem> Inventory { get; } = new();
        public Inventory<InventoryItem> HouseInventory { get; } = new();
        public Inventory<InventoryItem> Bank { get; } = new();
        public SkillManager Skills { get; } = new();
        public FactionManager Factions { get; } = new();
        public Target Target { get; } = new();

        public Inventory<ItemBase> Drops { get; } = new();
        public Dictionary<int, Quest> Quests { get; } = new();
        public Dictionary<string, int> UserIds { get; } = new();

        public IShop Shop { get; } = new Shop().DecorateState<IShop, Shop>();

        public Events Events { get; } = new();
        public bool InventoryLoaded { get; set; }
        public bool BankLoaded { get; set; }

        public TimeSpan Elapsed => _stopwatch.Elapsed;
        public double Time => _stopwatch.Elapsed.TotalSeconds;

        public World(Game game)
        {
            _game = game;
        }

        public IEntityBase ParseSource(string source)
        {
            var parts = source.Split(':');
            if (parts.Length != 2 || !int.TryParse(parts[1], out var id))
            {
                return null;
            }

            return parts[0] switch
            {
                "p" => Players.Get(id),
                "m" => Monsters.Get(id),
                _ => null
            };
        }

        public Task RetrieveUserData(params int[] userIds)
        {
            return SendXtAsync("retrieveUserDatas", userIds);
        }

        public Task GetDropAsync(string name)
        {
            var drop = Drops.Find(d => d.Name.EqualsIgnoreCase(name));
            if (drop is not null)
            {
                return GetDropAsync(drop.Id);
            }

            return Task.CompletedTask;
        }

        public Task GetDropAsync(int id)
        {
            return SendXtAsync("getDrop", id);
        }

        public Task AggroAsync(IEnumerable<Monster> monsters)
        {
            return SendXtAsync("aggroMon", monsters.Select(m => m.MapId).ToArray());
        }

        public Task AttackAsync(string skill, IEnumerable<IEntityBase> entities)
        {
            var entList = entities.ToList();
            var targets = string.Join(',', entList.Select(m => $"{skill}>{m.TargetString}"));
            var task = SendXtAsync("gar", ActionId, targets, "wvz");
            Target.Entity = entList.FirstOrDefault();
            ActionId++;
            if (ActionId > MaxActionId)
            {
                ActionId = 0;
            }

            return task;
        }

        public Task UseSkillAsync(int index)
        {
            var skill = Skills.Get(index);
            return skill is not null ? UseSkillAsync(skill) : Task.CompletedTask;
        }

        public Task UseSkillAsync(Skill skill)
        {
            skill.LastUse = Elapsed;
            return AttackAsync(skill.Ref, Monsters.InFrame(Frame).GetTargets(Target.Entity).Take(skill.MaxTargets));
        }

        public Task MoveToAsync(int x, int y)
        {
            return MoveToAsync(x, y, _game.Options.MoveSpeed);
        }

        public Task MoveToAsync(int x, int y, int speed)
        {
            return SendXtAsync("mv", x, y, speed);
        }

        public async Task MoveToCellAsync(string frame, string pad, bool sendPacket = false)
        {
            if (pad.ToLower() == "none")
            {
                Player.X = Player.Y = 0;
            }

            Player.Frame = frame;
            Player.Pad = pad;

            if (sendPacket)
            {
                await SendXtAsync("moveToCell", frame, pad);
            }

            await ExitCombatAsync();
        }

        public Task ExitCombatAsync()
        {
            return Task.CompletedTask;
        }

        public Task JoinAsync(string mapName)
        {
            return SendXtAsync("cmd", "tfer", _game.Username, mapName);
        }

        public Task LoadBankAsync()
        {
            return SendXtAsync("loadBank", "All");
        }

        public Task LoadQuestsAsync(params int[] questIds)
        {
            Events.QuestsLoad.Reset();
            var unloaded = questIds.Where(id => !Quests.ContainsKey(id)).ToArray();
            if (unloaded.Length > 0)
            {
                return SendXtAsync("getQuests", unloaded);
            }

            return Task.CompletedTask;
        }

        public Task LoadShopAsync(int shopId)
        {
            return SendXtAsync("loadShop", shopId);
        }

        public Task SendXtAsync(string command, params object[] args)
        {
            var combined = new object[3 + args.Length];
            combined[0] = "zm";
            combined[1] = command;
            combined[2] = RoomId;
            Array.Copy(args, 0, combined, 3, args.Length);
            return _game.SendXtAsync(combined);
        }
    }
}
