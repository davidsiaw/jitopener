using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using BlueBlocksLib.FileAccess;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using BlueBlocksLib.SetUtils;
using System.Drawing.Imaging;

namespace JitOpener
{
    class FileFormats
    {

        [StructLayout(LayoutKind.Sequential)]
        public struct GearCoreEntry
        {
            public byte gcid;
            public byte mystery1;
            public short sourceItem;
            public short destItem;

            [ArraySize(10)]
            public byte[] mysteries;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct GearCoreBin
        {
            [ArraySize(512)]
            public GearCoreEntry[] entries;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ExpListBin
        {
            [ArraySize(150)]
            public ulong[] exp;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct PranExpListBin
        {
            [ArraySize(100)]
            public uint[] exp;

            [ArraySize(100)]
            public ushort[] levels;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct TitleInfo
        {
            public short mystery;
            public short numOfTheMobKilled;

            [ArraySize(3)]
            public short[] effect;

            [ArraySize(3)]
            public short[] value;

            [ArraySize(32)]
            public byte[] name;

            public byte mystery2;
            public byte id;

            public short empty;

            // color (dont know for what)
            public byte r;
            public byte g;
            public byte b;
            public byte a;

            public string Name
            {
                get
                {
                    return Encoding.GetEncoding(Program.encoding).GetString(name).Trim('\0');
                }
            }

            public Color Color
            {
                get
                {
                    return Color.FromArgb(a, r, g, b);
                }
            }

            public string Effects
            {
                get
                {
                    return string.Join("<br />",new string[] { 
								GetEffectDescription(effect[0], value[0]), 
								GetEffectDescription(effect[1], value[1]), 
								GetEffectDescription(effect[2], value[2]) }.Where(x => x != FileFormats.strdefbin.None));
                }
            }

            private static string GetEffectDescription(int effect, int amount)
            {
                if (efdescbin.descs.Length == 0)
                {
                    return strdefbin.strs[400 + effect].GetString(amount);
                }
                string res = efdescbin.descs[effect].GetString(amount);
                if (res.Length == 0)
                {
                    return strdefbin.strs[400 + effect].GetString(amount);
                }
                return res;
            }
        }


        [StructLayout(LayoutKind.Sequential)]
        public struct TitleInfoGroup
        {
            [ArraySize(4)]  // each title can have 4 levels presumably
            public TitleInfo[] titles;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct TitleDesc
        {
            [ArraySize(64)]
            public byte[] desc;

            public string Content
            {
                get
                {
                    return Encoding.GetEncoding(Program.encoding).GetString(desc).Trim('\0');
                }
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct TitleDescGroup
        {
            [ArraySize(4)]  // each title can have 4 levels presumably
            public TitleDesc[] descs;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct TitleStory
        {
            [ArraySize(128)]
            public byte[] story;

            public string Content
            {
                get
                {
                    return Encoding.GetEncoding(Program.encoding).GetString(story).Trim('\0');
                }
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct TitleBin
        {
            [ArraySize(256)]
            public TitleInfoGroup[] infos;

            [ArraySize(256)]
            public TitleDescGroup[] descs;

            [ArraySize(256)]
            public TitleStory[] stories;
        }


        [StructLayout(LayoutKind.Sequential)]
        public struct RecipeRateEntry
        {
            public int recipeid;
            public int ingredientid; // ingredient whose reinforce level is looked at
            public int addflag; // 0 means add 10% to all chances, 1 means add 50% to all chances

            [ArraySize(16)] // chance for +0 to +15
            public short[] chances;

            public int Adjustment
            {
                get
                {
                    if (addflag == 0)
                    {
                        return 10;
                    }
                    else
                    {
                        return 50;
                    }
                }
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RecipeRateBin
        {
            [ArraySize(350)]
            public RecipeRateEntry[] entries;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct TextureSet
        {
            public int id;
            public int num;

            [ArraySize("num")]
            public TextureEntry[] entries;

            public override string ToString()
            {
                return "[" + entries.Length + "]";
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct TextureEntry
        {
            public int id;
            public int mystery;
            public int mystery2;
            public int width;
            public int height;
            public int mystery5;
            public int mystery6;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Texturesizes
        {
            [ArraySize(100)]
            public TextureSet[] sets;

            public TextureEntry[] mapsizes
            {
                get
                {
                    return sets.First(x => x.id == 0x5b).entries;
                }
            }

            public override string ToString()
            {
                return "[" + mapsizes.Length + "]";
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MobPosBin
        {
            [ArraySize(4096)]
            public MobPos[] entries;
        }


        [StructLayout(LayoutKind.Sequential)]
        public class MobPos
        {
            public int NPCID { get; private set; }

            public MobPos()
            {
                NPCID = NPCPosBin.npcid++;
            }

            static Regex idExtractor = new Regex("MOB(?<id>[0-9]+)", RegexOptions.Compiled);

            public int id;

            [ArraySize(12)]
            public byte[] name;

            [ArraySize(50)]
            public Position[] positions;

            public string FakeName
            {
                get
                {
                    return Encoding.GetEncoding(Program.encoding).GetString(name).Trim('\0');
                }
            }

            public string Name
            {
                get
                {
                    int id;
                    if (int.TryParse(idExtractor.Match(FakeName).Groups["id"].ToString(), out id))
                    {
                        return mobnamebin.names[id].Name;
                    }
                    return "";
                }
            }

            public string Pos
            {
                get
                {
                    return string.Join(",", positions.Select(x => x.x + " " + x.y).ToArray());
                }
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MapRange
        {
            public int xBottomLeft;
            public int yBottomLeft;
            public int xTopRight;
            public int yTopRight;
        }


        [StructLayout(LayoutKind.Sequential)]
        public struct MapName
        {
            [ArraySize(28)]
            public byte[] name;
            public string Name
            {
                get
                {
                    return Encoding.GetEncoding(Program.encoding).GetString(name).Trim('\0');
                }
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MapDesc
        {
            [ArraySize(1024)]
            public byte[] desc;
            public string Desc
            {
                get
                {
                    return Encoding.GetEncoding(Program.encoding).GetString(desc).Trim((char)0xcd).Trim('\0');
                }
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MapBin
        {
            [ArraySize(64)]
            public MapRange[] ranges;


            [ArraySize(64)]
            public MapName[] names;


            [ArraySize(64)]
            public MapDesc[] descs;

            [ArraySize(64)]
            public int[] mystery;

            [ArraySize(64)]
            public int[] mystery2;

            public MapInfo[] Maps
            {
                get
                {
                    MapName[] n = names;
                    MapRange[] r = ranges;
                    MapDesc[] d = descs;
                    return Enumerable.Range(0, 64).
                        Where(x => n[x].Name.Length != 0).
                        Select(x => new MapInfo() { 
                            id = x,
                            name = n[x],
                            range = r[x],
                            desc = d[x]
                        }).ToArray();
                }
            }
        }

        public class MapInfo
        {
            public int id;
            public MapRange range;
            public MapName name;
            public MapDesc desc;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct DialogBin
        {
            [ArraySize(10240)]
            public Dialog[] dialogs;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Dialog
        {
            [ArraySize(68)]
            public byte[] empty;

            [ArraySize(40)]
            public Paragraph[] dialog;

        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Paragraph
        {
            public int a;

            [ArraySize(128)]
            public byte[] content;

            public string Content
            {
                get
                {
                    return Encoding.GetEncoding(Program.encoding).GetString(content).Trim('\0');
                }
            }

            public int b;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ObjPosBin
        {
            [ArraySize(512)]
            public ObjPos[] objects;
        }

        [StructLayout(LayoutKind.Sequential)]
        public class ObjPos
        {
            public int NPCID { get; private set; }

            public ObjPos()
            {
                NPCID = NPCPosBin.npcid++;
            }

            static Regex idExtractor = new Regex("MOB(?<id>[0-9]+)", RegexOptions.Compiled);

            public int id;

            [ArraySize(12)]
            public byte[] name;

            [ArraySize(10)]
            public Position[] positions;

            [ArraySize(10)]
            public int[] items;

            public string Pos
            {
                get
                {
                    return string.Join(",", positions.Select(x => x.x + " " + x.y).ToArray());
                }
            }

            public string FakeName
            {
                get
                {
                    return Encoding.GetEncoding(Program.encoding).GetString(name).Trim('\0');
                }
            }

            public string Name
            {
                get
                {
                    if (string.IsNullOrEmpty(FakeName))
                    {
                        return "";
                    }
                    return mobnamebin.names[
                        int.Parse(idExtractor.Match(FakeName).Groups["id"].ToString())].Name;
                }
            }

            public string Items
            {
                get
                {
                    return string.Join(",", items.Select(x => itemlistbin.items[x].Name).ToArray());
                }
            }
        }

        public struct Position
        {
            public int x, y;
        }


        [StructLayout(LayoutKind.Sequential)]
        public struct NNBin
        {
            [ArraySize(5)]
            public NationName[] names;
        }


        [StructLayout(LayoutKind.Sequential)]
        public struct NationName
        {
            [ArraySize(32)]
            public byte[] name;

            public string Name
            {
                get
                {
                    return Encoding.GetEncoding(Program.encoding).GetString(name).Trim('\0');
                }
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct EFDescBin
        {
            [ArraySize(512)]
            public EFDesc[] descs;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct EFDesc
        {
            [ArraySize(96)]
            public byte[] name;

            public string GetString(int value)
            {
                return Text.Replace("%d", value.ToString()).Replace("%%", "%");
            }

            public string Text
            {
                get
                {
                    return Encoding.GetEncoding(Program.encoding).GetString(name).Trim('\0');
                }
            }
        }

        public enum CommandType
        {
            Monster = 0x1,
            Item = 0x2,
            ItemChoice = 0xD,
            EXP = 0xE,
            Gold = 0xF,
            Pran = 0x15,
            Use = 0x1F,
            AfterMission = 0x30,
            MissionChoice = 0x25,
            ComeFrom = 0x26,
            SkillAcquire = 62,
            Equip = 34,
            Class = 5,
            ChangeClass = 16,
            BattleFieldVictory = 51,
            GuildEXP = 7,
            LevelRange = 4,
            Prerequisite = 9,
            TalkTo = 19,
        }

        public enum PranType
        {
            Fire,
            Water,
            Air
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct QuestCommand
        {
            public CommandType type;
            public int amountOrType;
            public int amount2;
            public int mystery2;
            public short item1;
            public short item2;
            public int mystery3;
            public int mystery4;
            public int mystery5;
            public int quantity;

            public override string ToString()
            {
                switch (type)
                {
                    case CommandType.AfterMission:
                        return "Prerequisite Quest: " + questbin.quests[amountOrType].Name;
                    case CommandType.LevelRange:
                        return "Level Range: " + amountOrType + " to " + amount2;
                    case CommandType.Prerequisite:
                        return "Prerequisite Quest: " + questbin.quests[amountOrType].Name;
                    case CommandType.GuildEXP:
                        return "Guild Gains " + amountOrType + " EXP";
                    case CommandType.ChangeClass:
                        return "Change class to: " + amountOrType;
                    case CommandType.BattleFieldVictory:
                        return "Win " + amountOrType + " battlefield game";
                    case CommandType.SkillAcquire:
                        return "Acquire Skill: " + skilldatabin.skills[amountOrType].Name;
                    case CommandType.Equip:
                        return "Equipped: " + itemlistbin.items[amountOrType].Name;
                    case CommandType.Class:
                        return "Class: " + (ParamJob)amountOrType;

                    case CommandType.Monster:
                        return "Kill " + amount2 + "x " + mobnamebin.names[amountOrType].Name;
                    case CommandType.Item:
                        return (quantity == 0 ? 1 : quantity) + "x " + itemlistbin.items[item1].Name;
                    case CommandType.ItemChoice:
                        return "Choice of: " + itemlistbin.items[item1].Name;
                    case CommandType.EXP:
                        return "EXP: " + amountOrType;
                    case CommandType.Gold:
                        return "Gold: " + amountOrType;
                    case CommandType.Pran:
                        return "Recieve " + (PranType)amountOrType + " Pran";
                    case CommandType.MissionChoice:
                        return "Next Mission: " + questbin.quests[amountOrType].Name;
                    case CommandType.ComeFrom:
                        return "Comes from: " + questbin.quests[amountOrType].Name;
                    case CommandType.Use:
                        return "Use: " + objposbin.objects[amountOrType].Name + " " + amount2 + " times";
                    case CommandType.TalkTo:
                        return "Talk to: " + npcposbin.npcs[amountOrType].Name;

                }
                return type.ToString();
            }
        }

        [Flags]
        public enum ParamJob
        {
            Crusader = 0x4,
            Warrior = 0x2,
            Sniper = 0x10,
            DualGunner = 0x8,
            NightMagician = 0x20,
            Priest = 0x40,
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Quest
        {
            public short id;
            public short mystery;
            public int startNPC;
            public int endNPC;
            public int myst2;

            [ArraySize(64)]
            public byte[] name;

            public short startDialog;
            public short unfinishedDialog;
            public short endDialog;
            public short summary;

            [ArraySize(1155)]
            public byte[] empty;

            public byte level;

            public short mystery0;
            public short mystery1;
            public int mystery2;


            [ArraySize(5)]
            public QuestCommand[] preconditions;

            [ArraySize(5)]
            public QuestCommand[] requires;

            [ArraySize(8)]
            public QuestCommand[] rewards;

            [ArraySize(5)]
            public QuestCommand[] removes;

            [ArraySize(3)]
            public QuestCommand[] choices;

            [ArraySize(4)]
            public QuestCommand[] misc;

            public int ID
            {
                get
                {
                    return id;
                }
            }

            public int Level
            {
                get
                {
                    return level;
                }
            }

            public string Name
            {
                get
                {
                    return Encoding.GetEncoding(Program.encoding).GetString(name).Trim('\0');
                }
            }

            public string Cond
            {
                get
                {
                    return string.Join(", ", preconditions.Select(x => x.ToString()).ToArray());
                }
            }

            public int NumberInSeries
            {
                get
                {
                    if (SeriesStart.id == id)
                    {
                        return 1;
                    }

                    if (PrerequisiteMission.Name.Length != 0)
                    {
                        return PrerequisiteMission.NumberInSeries + 1;
                    }

                    if (Prequels.Length != 0)
                    {
                        return Prequels[0].NumberInSeries + 1;
                    }

                    return 0;
                }

            }

            public Quest SeriesStart
            {
                get
                {
                    if (PrerequisiteMission.Name.Length != 0)
                    {
                        return PrerequisiteMission.SeriesStart;
                    }

                    if (Prequels.Length != 0)
                    {
                        return Prequels[0].SeriesStart;
                    }

                    return this;
                }
            }

            public NPCPos StartNPC
            {
                get
                {
                    if (startNPC > 0 && !string.IsNullOrEmpty(npcposbin.npcs[startNPC].Name))
                    {
                        return npcposbin.npcs[startNPC];
                    }
                    return new NPCPos();
                }
            }

            public NPCPos EndNPC
            {
                get
                {
                    if (endNPC > 0 && !string.IsNullOrEmpty(npcposbin.npcs[startNPC].Name))
                    {
                        return npcposbin.npcs[endNPC];
                    }
                    return new NPCPos();
                }
            }

            public KeyValuePair<ObjPos, int>[] UseObject
            {
                get
                {
                    return requires.
                        Where(x => x.type == CommandType.Use).
                        Select(x => new KeyValuePair<ObjPos, int>(objposbin.objects[x.amountOrType], x.amount2)).
                        ToArray();
                }
            }

            public KeyValuePair<MobPos, int>[] KillMob
            {
                get
                {
                    return requires.
                        Where(x => x.type == CommandType.Monster).
                        Select(x => new KeyValuePair<MobPos, int>(mobposbin.entries[x.amountOrType], x.amount2)).
                        ToArray();
                }
            }

            public string StartNPCName
            {
                get
                {
                    // Figure out where the quest came from
                    if (startNPC == 0)
                    {
                        if (endNPC == 0)
                        {
                            return "Not implemented";
                        }

                        if (Name.StartsWith("[Legion]"))
                        {
                            return "From Item: Legion Mission Lv. " + (((level - 1) / 10) + 1);
                        }

                        if (choices.ToList().Exists(x => x.type == CommandType.ComeFrom))
                        {
                            return "From Quests: " + string.Join(", ", choices.
                                Where(x => x.type == CommandType.ComeFrom).
                                Select(x => x.ToString()));
                        }

                        QuestCommand[] reqs = requires;
                        QuestCommand[] rems = removes;
                        if (rems.ToList().Exists(x => x.type == CommandType.Item))
                        {
                            int itemid = rems.Where(x => x.type == CommandType.Item).
                                ToArray()[0].item1;
                            return "From Item: " + itemlistbin.items[itemid].Name;
                        }


                        return "Your Pran";
                    }

                    if (startNPC == 0xffff)
                    {
                        return "Automatic Reward";
                    }
                    if (string.IsNullOrEmpty(npcposbin.npcs[startNPC].Name))
                    {
                        return "Not implemented - " + startNPC.ToString();
                    }
                    return npcposbin.npcs[startNPC].Name;
                }
            }

            public string EndNPCName
            {
                get
                {
                    if (endNPC == 0)
                    {
                        return "Not implemented";
                    }
                    if (endNPC == 0xffff)
                    {
                        return "Automatic Reward";
                    }
                    if (string.IsNullOrEmpty(npcposbin.npcs[startNPC].Name))
                    {
                        return "Not implemented - " + endNPC.ToString();
                    }
                    return npcposbin.npcs[endNPC].Name;
                }
            }

            public Dialog StartDialog
            {
                get
                {
                    return dialogbin.dialogs[startDialog];
                }
            }

            public Dialog MidDialog
            {
                get
                {
                    return dialogbin.dialogs[unfinishedDialog];
                }
            }

            public Dialog EndDialog
            {
                get
                {
                    if (Name.StartsWith("[Relic]") && (id % 2 != 0))
                    {
                        return dialogbin.dialogs[summary];
                    }
                    return dialogbin.dialogs[endDialog];
                }
            }

            public Dialog SummaryDialog
            {
                get
                {
                    if (Name.StartsWith("[Relic]") && (id % 2 != 0))
                    {
                        return dialogbin.dialogs[endDialog];
                    }
                    return dialogbin.dialogs[summary];
                }
            }

            public KeyValuePair<Item, int>[] ItemsRequired
            {
                get
                {
                    return requires.
                        Where(x => x.type == CommandType.Item).
                        Select(x => new KeyValuePair<Item, int>(itemlistbin.items[x.item1], x.quantity)).
                        ToArray();
                }
            }

            public ParamJob[] ClassRequired
            {
                get
                {
                    var classesconds = preconditions.
                       Where(x => x.type == CommandType.Class).
                       Select(x => (ParamJob)x.amountOrType).ToArray();

                    if (classesconds.Length == 0)
                    {
                        return new ParamJob[0];
                    }

                    var classes = classesconds[0];

                    return EnumUtil.GetValues<ParamJob>().Where(x => classes.HasFlag(x)).ToArray();
                }
            }

            public Quest[] Prequels
            {
                get
                {
                    return choices.
                        Where(x => x.type == CommandType.ComeFrom).
                        Select(x => questbin.quests[x.amountOrType]).ToArray();
                }
            }

            public Quest PrerequisiteMission
            {
                get
                {
                    var a = preconditions.Where(x => x.type == CommandType.Prerequisite || x.type == CommandType.AfterMission);
                    if (a.Count() > 0)
                    {
                        return questbin.quests[a.First().amountOrType];
                    }
                    return new Quest() { name = Encoding.UTF8.GetBytes("") };
                }
            }

            public int StartLevel
            {
                get
                {
                    var a = preconditions.Where(x => x.type == CommandType.LevelRange);
                    if (a.Count() > 0)
                    {
                        return a.First().amountOrType;
                    }
                    return 0;
                }
            }

            public int CutoffLevel
            {
                get
                {
                    var a = preconditions.Where(x => x.type == CommandType.LevelRange);
                    if (a.Count() > 0)
                    {
                        return a.First().amount2;
                    }
                    return 0;
                }
            }

            public string Require
            {
                get
                {
                    return string.Join(", ", requires.Select(x => x.ToString()).ToArray());
                }
            }

            public string Rewards
            {
                get
                {
                    return string.Join(", ", rewards.Select(x => x.ToString()).ToArray());
                }
            }

            public short[] RemovesItemsIDs
            {
                get
                {
                    return removes.Where(x => x.type == CommandType.Item).
                        Select(x => x.item1).
                        ToArray();
                }
            }
            public Item[] RemovesItems
            {
                get
                {
                    return removes.Where(x => x.type == CommandType.Item).
                        Select(x => itemlistbin.items[x.item1]).
                        ToArray();
                }
            }

            public string Removes
            {
                get
                {
                    return string.Join(", ", removes.Select(x => x.ToString()).ToArray());
                }
            }

            public string Choices
            {
                get
                {
                    return string.Join(", ", choices.Select(x => x.ToString()).ToArray());
                }
            }

            public string Misc
            {
                get
                {
                    return string.Join(", ", misc.Select(x => x.ToString()).ToArray());
                }
            }

            public int EXP
            {
                get
                {
                    var a = rewards.Where(x => x.type == CommandType.EXP);
                    if (a.Count() > 0)
                    {
                        return a.First().amountOrType;
                    }
                    return 0;
                }
            }

            public int Gold
            {
                get
                {
                    var a = rewards.Where(x => x.type == CommandType.Gold);
                    if (a.Count() > 0)
                    {
                        return a.First().amountOrType;
                    }
                    return 0;
                }
            }

            public KeyValuePair<Item, int>[] Items
            {
                get
                {
                    List<KeyValuePair<Item, int>> items = new List<KeyValuePair<Item, int>>();
                    var a = rewards.Where(x => x.type == CommandType.Item);
                    var b = rewards.Where(x => x.type == CommandType.ItemChoice);
                    items.AddRange(a.Select(x => new KeyValuePair<Item, int>(itemlistbin.items[x.item1], x.quantity == 0 ? 1 : x.quantity)));
                    items.AddRange(b.Select(x => new KeyValuePair<Item, int>(itemlistbin.items[x.item1], 1)));
                    return items.ToArray();
                }
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public class NPCPos
        {
            public int NPCID { get; private set; }

            public NPCPos()
            {
                NPCID = NPCPosBin.npcid++;
            }

            public int namePos;
            public int x;
            public int y;

            public string Name
            {
                get
                {
                    return mobnamebin.names[namePos].Name;
                }
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public class NPCPosBin
        {
            public static int npcid { get; set; }

            public int num;

            [ArraySize(2048)]
            public NPCPos[] npcs;

            public int Num
            {
                get
                {
                    return num;
                }
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MNBin
        {
            [ArraySize(4096)]
            public MobName[] names;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MobName
        {
            [ArraySize(128)]
            public byte[] bytes;

            public string Name
            {
                get
                {
                    return Encoding.GetEncoding(Program.encoding).GetString(bytes).Trim('\0');
                }
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct QuestBin
        {
            [ArraySize(7168)]
            public Quest[] quests;
        }


        [StructLayout(LayoutKind.Sequential)]
        public struct SkillDataBin
        {
            [ArraySize(12000)]
            public Skill[] skills;
        }

        [StructLayout(LayoutKind.Sequential)]
        public class Skill
        {
            public int iconid; // 4
            public int levelRequired; // 8
            public int skillLevelCap; // 12
            public int skillLevel; // 16
            public int mystery3; // 20

            [ArraySize(64)]
            public byte[] name; // 84

            [ArraySize(64)]
            public byte[] name2; // 148

            public int skillPointCost; // 152
            public int learningCost; // 156
            public SkillProfession skillprofession; // 160
            public int guildSkill; // 164
            public int mystery7; // 168

            public int Tier
            {
                get;
                internal set;
            }

            public ProfessionType ProfType
            {
                get;
                internal set;
            }

            [ArraySize(65)]
            public int[] mysteries; // 428

            public int mpRequired
            {
                get
                {
                    return mysteries[1];
                }
            }

            public int facionRequired
            {
                get
                {
                    return mysteries[2];
                }
            }

            public int cooldownTime
            {
                get
                {
                    return mysteries[4];
                }
            }

            public int effectExpiry
            {
                get
                {
                    return mysteries[31];
                }
            }

            public int castTime
            {
                get
                {
                    return mysteries[38];
                }
            }

            [ArraySize(288)]
            public byte[] description; // 704


            public string Name
            {
                get
                {
                    if (Program.cultureNum == 2 || Program.cultureNum == 0)
                    {
                        return Name2;
                    }
                    return Encoding.GetEncoding(Program.encoding).GetString(name).Trim('\0');
                }
            }
            public string Name2
            {
                get
                {
                    return Encoding.GetEncoding(Program.encoding).GetString(name2).Trim('\0');
                }
            }
            public string Type
            {
                get
                {
                    return Encoding.GetEncoding(Program.encoding).GetString(description).Trim('\0');
                }
            }

            public string Effect
            {
                get
                {
                    KeyValuePair<int, int>[] effects = new KeyValuePair<int, int>[] {
                        new KeyValuePair<int,int>(17,20),
                        new KeyValuePair<int,int>(18,21),
                        new KeyValuePair<int,int>(19,22),
                        new KeyValuePair<int,int>(23,27),
                        new KeyValuePair<int,int>(24,28),
                        new KeyValuePair<int,int>(25,29),
                        new KeyValuePair<int,int>(26,30),
                        new KeyValuePair<int,int>(32,35),
                        new KeyValuePair<int,int>(33,36),
                        new KeyValuePair<int,int>(34,37),

                        };
                    return ExtractEffectStringsFor(effects);
                }
            }

            public string CastInstantEffect
            {
                get
                {
                    KeyValuePair<int, int>[] effects = new KeyValuePair<int, int>[] {
                        new KeyValuePair<int,int>(17,20),
                        new KeyValuePair<int,int>(18,21),
                        new KeyValuePair<int,int>(19,22),

                        };
                    return ExtractEffectStringsFor(effects);
                }
            }

            public string SpellDurationEffect
            {
                get
                {
                    KeyValuePair<int, int>[] effects = new KeyValuePair<int, int>[] {
                        new KeyValuePair<int,int>(23,27),
                        new KeyValuePair<int,int>(24,28),
                        new KeyValuePair<int,int>(25,29),
                        new KeyValuePair<int,int>(26,30),

                        };
                    return ExtractEffectStringsFor(effects);
                }
            }

            public string PassiveEffect
            {
                get
                {
                    KeyValuePair<int, int>[] effects = new KeyValuePair<int, int>[] {
                        new KeyValuePair<int,int>(32,35),
                        new KeyValuePair<int,int>(33,36),
                        new KeyValuePair<int,int>(34,37),

                        };
                    return ExtractEffectStringsFor(effects);
                }
            }

            private string ExtractEffectStringsFor(KeyValuePair<int, int>[] effects)
            {
                int[] myst = mysteries;

                return string.Join(", ", effects
                    .Select(x =>

                    myst[x.Key] > 257 && myst[x.Key] < bin.effects.Length
                    ? GetNameOfEffect(myst, ref x) :

                    GetStringForEffect(myst, ref x).Length == 0 ?
                    GetNameOfEffect(myst, ref x) :
                    GetStringForEffect(myst, ref x))

                    .Where(x => x != strdefbin.None).

                ToArray());
            }

            private static string GetNameOfEffect(int[] myst, ref KeyValuePair<int, int> x)
            {
                return bin.effects[myst[x.Key]].Name + " = " + myst[x.Value];
            }

            private static string GetStringForEffect(int[] myst, ref KeyValuePair<int, int> x)
            {
                return strdefbin.strs[
                (myst[x.Key] > 0 && myst[x.Key] < 655
                ? myst[x.Key] : 0) + 400].
                GetString(myst[x.Value]);
            }

            const int ImageSize = 42;

            public Tuple<int, Rectangle> ImagePos
            {
                get
                {
                    const int NumOfImagesInEachFile = 24 * 11;
                    int id = iconid;
                    int filenum = ((id / NumOfImagesInEachFile) % 2) + 1;
                    int idx = (id % NumOfImagesInEachFile);
                    int x = idx % 24;
                    int y = idx / 24;

                    return new Tuple<int,Rectangle>(filenum, new Rectangle(x * ImageSize, y * ImageSize, ImageSize, ImageSize));
                }
            }

            public Image Image
            {
                get
                {
                    using (Image img = Image.FromFile(Program.sourcePath + @"\UI\skillicons" + ImagePos.Item1 + ".jit.png"))
                    {
                        Bitmap bmap = new Bitmap(ImageSize, ImageSize);
                        using (Graphics g = Graphics.FromImage(bmap))
                        {
                            g.DrawImage(img, new Rectangle(0, 0, ImageSize, ImageSize), ImagePos.Item2, GraphicsUnit.Pixel);
                        }
                        return bmap;
                    }
                }
            }

        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SetItemBin
        {
            [ArraySize(1024)]
            public SetEffect[] sets;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SetEffect
        {
            [ArraySize(64)]
            public byte[] name1;

            [ArraySize(64)]
            public byte[] name2;

            public int numitems;

            [ArraySize(12)]
            public short[] setitems;

            [ArraySize(6)]
            public short[] effectNumbers;

            [ArraySize(6)]
            public short[] mysteries;

            [ArraySize(6)]
            public short[] castchance;

            [ArraySize(6)]
            public short[] effectIDs;

            [ArraySize(6)]
            public short[] effectValues;


            public string Name
            {
                get
                {
                    string name = Encoding.GetEncoding(Program.encoding).GetString(name1).Trim('\0');

                    if (undesirablename.Match(name).Success)
                    {
                        name = Name2;
                    }

                    return name;
                }
            }
            public string Name2
            {
                get
                {
                    return Encoding.GetEncoding(Program.encoding).GetString(name2).Trim('\0');
                }
            }
            public Item[] SetItems
            {
                get
                {
                    return setitems.Where(x => x != 0).Select(x => itemlistbin.items[x]).ToArray();
                }
            }

            public string[] Effects(Func<Skill, string> skillHandler)
            {
                short[] ei = effectIDs;
                short[] ev = effectValues;
                short[] cc = castchance;
                return Enumerable.Range(0, 6).Select(x =>
                    ei[x] == 180 ?
                    translation.Translate("{0}% chance to cast {1}", cc[x], skillHandler(skilldatabin.skills[ev[x]]))
                :
                strdefbin.strs[ei[x] + 400].GetString(ev[x])

                ).ToArray();
            }

            public Dictionary<int, string> SetEffects(Func<Skill, string> skillhandler)
            {
                short[] en = effectNumbers;
                string[] Ef = Effects(skillhandler);
                Dictionary<int, string> str = new Dictionary<int, string>();

                Enumerable.Range(0, 6).ToList().ForEach(x =>
                {
                    int num = en[x];
                    if (num == 0)
                    {
                        return;
                    }
                    if (!str.ContainsKey(num))
                    {
                        str[num] = "";
                    }
                    str[num] = str[num] + "," + Ef[x];
                });

                return str;
            }

        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MakeItem
        {
            public int createditemid;
            public int cost;

            public byte craftQuantity;
            public byte levelReq;
            public short successChance;

            public short superiorChance;
            public short doubleChance;

            [ArraySize(12)]
            public short[] ingredientitem;

            [ArraySize(12)]
            public byte[] ingredientnum;

            public Item CreatedItem
            {
                get
                {
                    return itemlistbin.items[createditemid];
                }
            }

            public KeyValuePair<Item, int>[] Ingredients
            {
                get
                {
                    KeyValuePair<Item, int>[] ingredients = new KeyValuePair<Item, int>[12];
                    for (int i = 0; i < ingredientitem.Length; i++)
                    {
                        ingredients[i] = new KeyValuePair<Item, int>(
                            itemlistbin.items[ingredientitem[i]],
                            ingredientnum[i]);
                    }
                    return ingredients;
                }
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MakeItemsBin
        {
            [ArraySize(12000)]
            public MakeItem[] recipes;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Reinforce3Bin
        {
            // normal, unique, sup
            [ArraySize(3)]
            public Reinforce3Section[] sections;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Reinforce3Section
        {
            public ReinforceArmor shields;

            [ArraySize(4)]
            public ReinforceArmor[] armors;
        }



        [StructLayout(LayoutKind.Sequential)]
        public struct Reinforce2Bin
        {
            [ArraySize(3)]
            public ReinforceSection[] section;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ReinforceSection
        {
            [ArraySize(6)]
            public ReinforceWeapon[] weapons;

            public ReinforceArmor shields;

            [ArraySize(4)]
            public ReinforceArmorProf[] other;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ReinforceWeapon
        {
            [ArraySize(35)]
            public ReinforceBonus[] bonuses;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ReinforceArmorProf
        {
            [ArraySize(6)]
            public ReinforceArmor[] armors;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ReinforceArmor
        {
            [ArraySize(30)]
            public ReinforceBonus[] bonuses;
        }

        [StructLayout(LayoutKind.Sequential)]
        public class ReinforceBonus
        {
            static int reinforceid = 1;

            public int ID { get; private set; }

            public ReinforceBonus()
            {
                ID = reinforceid++;
            }

            [ArraySize(16)]
            public int[] firstbonus;

            [ArraySize(16)]
            public int[] secondbonus;
        }


        [StructLayout(LayoutKind.Sequential)]
        public struct RecipeBin
        {
            [ArraySize(3000)]
            public Recipe[] recs;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Recipe
        {
            public int itemid;

            public short createditemid;
            public short createditemidSuperior;

            public int cost;

            public byte quantity;
            public byte levelReq;
            public short successRate;

            public short superiorRate;
            public short doubleRate;

            [ArraySize(12)]
            public short[] ingredientitem;

            [ArraySize(12)]
            public byte[] ingredientnum;

            public Item CreatedItem
            {
                get
                {
                    return itemlistbin.items[createditemid];
                }
            }

            public Item RecipeItem
            {
                get
                {
                    return itemlistbin.items[itemid];
                }
            }

            public KeyValuePair<Item, int>[] Ingredients
            {
                get
                {
                    KeyValuePair<Item, int>[] ingredients = new KeyValuePair<Item, int>[12];
                    for (int i = 0; i < ingredientitem.Length; i++)
                    {
                        ingredients[i] = new KeyValuePair<Item, int>(
                            itemlistbin.items[ingredientitem[i]],
                            ingredientnum[i]);
                    }
                    return ingredients;
                }
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ReinforceBin
        {
            [ArraySize(105)]
            public ReinforceLayout[] sets;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ReinforceLayout
        {
            public int mystery;
            public int price;

            [ArraySize(16)]
            public short[] chance;

        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ReinforceBinV2
        {
            [ArraySize(105)]
            public ReinforceLayoutV2[] sets;
        }

        [StructLayout(LayoutKind.Sequential)]
        public class ReinforceLayoutV2
        {
            static int reinforceid = 1;

            public int ID { get; private set; }

            public ReinforceLayoutV2()
            {
                ID = reinforceid++;
            }

            public int mystery;
            public int price;

            [ArraySize(16)]
            public short[] chance;

            public int mystery2;
            public int mystery3;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct StringDef
        {
            [ArraySize(128)]
            public byte[] name1;

            public string GetString(int value)
            {
                return Name.Replace("%d", value.ToString()).Replace("%%", "%");
            }

            string Name
            {
                get
                {
                    return Encoding.GetEncoding(Program.encoding).GetString(name1).Trim('\0');
                }
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct StrDefBin
        {
            [ArraySize(1563)]
            public StringDef[] strs;

            public string None
            {
                get
                {
                    return strs[400].GetString(0);
                }
            }
        }



        [StructLayout(LayoutKind.Sequential)]
        public struct ItemEffect
        {
            [ArraySize(64)]
            public byte[] name1;

            public int index;

            public string Name
            {
                get
                {
                    return Encoding.GetEncoding(Program.encoding).GetString(name1).Trim('\0');
                }
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ItemEffectBin
        {

            [ArraySize(328)]
            public ItemEffect[] effects;
        }
        

        [StructLayout(LayoutKind.Sequential)]
        public struct ItemListBinJP
        {

            [ArraySize(0)]
            public byte[] stuff;

            [ArraySize(15360)]
            public Item[] items;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ItemListBin
        {

            [ArraySize(0)]
            public byte[] stuff;

            [ArraySize(20480)]
            public Item[] items;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Item
        {
            [ArraySize(64)]
            public byte[] name1;

            [ArraySize(64)]
            public byte[] name2;

            [ArraySize(128)]
            public byte[] type;

            [ArraySize(104)]
            public ushort[] things;

            public ushort ID
            {
                get;
                set;
            }

            const int ImageSize = 42;

            public Tuple<int, Rectangle> ImageData
            {
                get
                {
                    const int NumOfImagesInEachFile = 24 * 24;
                    int id = things[32];
                    int filenum = (id / NumOfImagesInEachFile) + 1;
                    int idx = (id % NumOfImagesInEachFile);
                    int x = idx % 24;
                    int y = idx / 24;

                    Rectangle rect = new Rectangle(x * ImageSize, y * ImageSize, ImageSize, ImageSize);

                    return new Tuple<int, Rectangle>(filenum, rect);
                }
            }

            public Image Image
            {
                get
                {
                    using (Image img = Image.FromFile(Program.sourcePath + @"\UI\" + @"ItemIcons0" + ImageData.Item1 + ".jit.png"))
                    {
                        Bitmap bmap = new Bitmap(ImageSize, ImageSize);
                        using (Graphics g = Graphics.FromImage(bmap))
                        {
                            g.DrawImage(img, new Rectangle(0, 0, ImageSize, ImageSize), ImageData.Item2, GraphicsUnit.Pixel);
                        }
                        return bmap;
                    }
                }
            }

            public int GearCoreLevel
            {
                get
                {
                    return things[6];
                }
            }

            public string Name
            {
                get
                {
                    string name = Encoding.GetEncoding(Program.encoding).GetString(name1).Trim('\0');


                    if (undesirablename.Match(name).Success)
                    {
                        name = Name2;
                    }

                    if (Program.cultureNum == 8)
                    {
                        if (name.Length > 0 && name[0] > 256)
                        {
                            name = Name2;
                        }

                        if (name.Length > 1 && name[1] > 256)
                        {
                            name = Name2;
                        }
                    }

                    if (things[67] >= 256)
                    {
                        string n = nnbin.names[(things[67] >> 8) - 1].Name;
                        name = n + " " + name;
                    }

                    return name;
                }
            }
            public string Name2
            {
                get
                {
                    return Encoding.GetEncoding(Program.encoding).GetString(name2).Trim('\0');
                }
            }
            public string Description
            {
                get
                {
                    return Encoding.GetEncoding(Program.encoding).GetString(type).Trim('\0');
                }
            }

            public bool Tradeable
            {
                get
                {
                    return (things[68] & 256) == 0;
                }
            }

            public bool Reinforceable
            {
                get
                {
                    return (things[85] & 1) == 0;
                }
            }

            public string Attrs
            {
                get
                {
                    if ((things[68] & 256) != 0)
                    {
                        return FileFormats.translation.Translate("Cannot be traded");
                    }
                    return "";
                }
            }

            public ReinforceLayoutV2 RL
            {
                get
                {
                    if (things[6] >= 105 || things[6] == 0)
                    {
                        return new ReinforceLayoutV2();
                    }

                    if (things[1] >= 2 && things[1] <= 7)
                    {
                        return reinforceabin.sets[things[6] - 1];
                    }
                    if (things[1] >= 1001 && things[1] <= 1008)
                    {
                        return reinforcewbin.sets[things[6] - 1];
                    }
                    return new ReinforceLayoutV2();
                }
            }

            public string ReinforcePoss
            {
                get
                {
                    ReinforceLayoutV2 rl = RL;
                    if (rl.chance == null)
                    {
                        return "";
                    }

                    return string.Join(", ",
                        Enumerable.Range(1, 12).Select(x => "+" + x + ": " + (rl.chance[x - 1] / 10) + "%"))
                        ;
                }
            }

            public string ReinforceBonus
            {
                get
                {
                    ReinforceBonus rb = Bonuses;
                    if (rb.firstbonus == null)
                    {
                        return "";
                    }

                    return string.Join(", ",
                        Enumerable.Range(1, 12).Select(x => "+" + x + ": " + (rb.firstbonus[x - 1])))
                        ;
                }
            }

            public string ReinforceBonus2
            {
                get
                {
                    ReinforceBonus rb = Bonuses;
                    if (rb.firstbonus == null)
                    {
                        return "";
                    }

                    return string.Join(", ",
                        Enumerable.Range(1, 12).Select(x => "+" + x + ": " + (rb.secondbonus[x - 1])))
                        ;
                }
            }

            public string ReinforceBonus3
            {
                get
                {
                    ReinforceBonus rb = HPDmgBonuses;
                    if (rb.firstbonus == null)
                    {
                        return "";
                    }

                    return string.Join(", ",
                        Enumerable.Range(1, 12).Select(x => "+" + x + ": " + ((double)rb.firstbonus[x - 1] / (double)10) + "% /" + (rb.secondbonus[x - 1])))
                        ;
                }
            }

            public ReinforceBonus Bonuses
            {
                get
                {
                    ReinforceSection section;
                    int sectionNum;
                    bool hasSection = GetSectionNum(out sectionNum);

                    if (!hasSection)
                    {
                        return new ReinforceBonus();
                    }

                    section = reinforce2bin.section[sectionNum];

                    int prof = (int)Profession / 10;

                    switch (prof)
                    {
                        case 0:
                            prof = 1;
                            break;
                        case 1:
                            prof = 0;
                            break;
                        case 2:
                            prof = 3;
                            break;
                        case 3:
                            prof = 2;
                            break;
                        case 4:
                            prof = 5;
                            break;
                        case 5:
                            prof = 4;
                            break;
                    }

                    int armprof = prof;
                    switch (prof)
                    {
                        case 3:
                            armprof = 1;
                            break;
                        case 5:
                            armprof = 3;
                            break;
                        case 1:
                            armprof = 5;
                            break;

                    }

                    int offset = GetNormalizedReinforceFileIndex();

                    if ((int)ItemType >= 1000 && (int)ItemType != 1003 && (int)ItemType != 1004)
                    {
                        if (offset >= 35)
                        {
                            return new ReinforceBonus();
                        }
                        return section.weapons[prof].bonuses[offset];
                    }

                    if (ItemType == FileFormats.ItemType.Shield)
                    {
                        if (offset >= 30)
                        {
                            return new ReinforceBonus();
                        }
                        return section.shields.bonuses[offset];
                    }

                    if ((int)ItemType >= 2 && (int)ItemType <= 5)
                    {
                        int item = (int)ItemType;
                        if (prof == 1)
                        {

                            item = GetItemIndexInFile(item);
                        }

                        if (offset >= 30)
                        {
                            return new ReinforceBonus();
                        }
                        return section.other[item - 2].armors[armprof].bonuses[offset];
                    }

                    return new ReinforceBonus();

                }
            }

            private static int GetItemIndexInFile(int item)
            {
                switch (item)
                {
                    case 2:
                        item = 5;
                        break;
                    case 3:
                        item = 2;
                        break;
                    case 4:
                        item = 3;
                        break;
                    case 5:
                        item = 4;
                        break;

                }
                return item;
            }

            

            private bool GetSectionNum(out int sectionNum)
            {
                bool hasSection = true;
                sectionNum = 0;

                sectionNum = things[6] / 35;
                if (sectionNum > 2)
                {
                    switch (things[67])
                    {
                        case 0:
                            sectionNum = 0;
                            break;
                        case 3:
                            sectionNum = 1;
                            break;
                        case 5:
                            sectionNum = 2;
                            break;
                        default:
                            hasSection = false;
                            break;
                    }
                }


                return hasSection;
            }

            public ReinforceBonus HPDmgBonuses
            {
                get
                {
                    int sectionNum;
                    if (!GetSectionNum(out sectionNum))
                    {
                        return new ReinforceBonus();
                    }

                    Reinforce3Section section = reinforce3bin.sections[sectionNum];

                    int index = GetNormalizedReinforceFileIndex();

                    if (index >= 30)
                    {
                        return new ReinforceBonus();
                    }

                    if (ItemType == FileFormats.ItemType.Shield)
                    {
                        return section.shields.bonuses[index];
                    }

                    if ((int)ItemType >= 2 && (int)ItemType <= 5)
                    {
                        int item = (int)ItemType;
                        item = GetItemIndexInFile(item);
                        return section.armors[item - 2].bonuses[index];
                    }

                    return new ReinforceBonus();
                }
            }

            private int GetNormalizedReinforceFileIndex()
            {
                int offset = things[6] % 35;
               
                return offset;
            }


            public int UpgradeAmount
            {
                get
                {
                    return RL.price;
                }
            }

            public int HonorPoints
            {
                get
                {
                    return things[14] + (things[15] << 16);
                }
            }
            public int MedalsOfHero
            {
                get
                {
                    return things[16] + (things[17] << 16);
                }
            }

            public Rank Rank
            {
                get
                {
                    return (Rank)(things[85] >> 8);
                }
            }

            public Profession Profession
            {
                get
                {
                    if (things[22] % 2 == 0)
                    {
                        return (Profession)things[22] - 1;
                    }
                    return (Profession)things[22];
                }
            }

            public int BuyPrice
            {
                get
                {
                    return things[18] + (things[19] << 16);
                }
            }

            public int SellPrice
            {
                get
                {
                    return things[20] + (things[21] << 16);
                }
            }


            public int Level
            {
                get
                {
                    return things[37];
                }
            }

            public int PATK
            {
                get
                {
                    return things[51];
                }
            }

            public int PDEF
            {
                get
                {
                    return things[52];
                }
            }

            public int MATK
            {
                get
                {
                    return things[53];
                }
            }

            public int MDEF
            {
                get
                {
                    return things[54];
                }
            }

            public string Effect
            {
                get
                {
                    int effect = things[78];
                    int amount = things[81];
                    return GetEffectDescription(effect, amount);
                }
            }

            public string Effect2
            {
                get
                {
                    int effect = things[79];
                    int amount = things[82];
                    return GetEffectDescription(effect, amount);
                }
            }
            public string Effect3
            {
                get
                {
                    int effect = things[80];
                    int amount = things[83];
                    return GetEffectDescription(effect, amount);
                }
            }

            private static string GetEffectDescription(int effect, int amount)
            {
                if (efdescbin.descs.Length == 0)
                {
                    return strdefbin.strs[400 + effect].GetString(amount);
                }
                string res = efdescbin.descs[effect].GetString(amount);
                if (res.Length == 0)
                {
                    return strdefbin.strs[400 + effect].GetString(amount);
                }
                return res;
            }

            public ItemType ItemType
            {
                get
                {
                    return (ItemType)things[1];
                }
            }

            public SuperType SuperType
            {
                get
                {
                    return GetSuperType(ItemType);
                }
            }

            public SubType SubType
            {
                get
                {
                    return (SubType)things[4];
                }
            }

            public ItemQuality Quality
            {
                get
                {
                    return (ItemQuality)(things[67] & 0xf);
                }
            }

            public Color ItemColor
            {
                get
                {
                    switch (Quality)
                    {
                        case ItemQuality.Unique:
                            return Color.FromArgb(0xff, 0x99, 0x99);
                        case ItemQuality.Superior_Reinforced:
                            return Color.FromArgb(0xd1, 0xb9, 0xff);
                        case ItemQuality.Superior:
                            return Color.FromArgb(0x72, 0x66, 0xff);
                        case ItemQuality.Normal:
                            return Color.White;
                        case ItemQuality.Normal_Enchanted:
                            return Color.FromArgb(0x00, 0xee, 0x00);
                        case ItemQuality.Legendary:
                            return Color.FromArgb(0xff, 0x99, 0x00);
                        case ItemQuality.Premium:
                            return Color.FromArgb(0xff, 0x00, 0x99);
                    }
                    return Color.White;
                }
            }
        }

        public static SuperType GetSuperType(FileFormats.ItemType type)
        {
            if (type == FileFormats.ItemType.Head
                || type == FileFormats.ItemType.Body
                || type == FileFormats.ItemType.Glove
                || type == FileFormats.ItemType.Boots
                || type == FileFormats.ItemType.Shield
                || type == FileFormats.ItemType.One_Hand_Sword
                || type == FileFormats.ItemType.Two_Hand_Sword
                || type == FileFormats.ItemType.Gun
                || type == FileFormats.ItemType.Rifle
                || type == FileFormats.ItemType.Wand
                || type == FileFormats.ItemType.Staff
                || type == FileFormats.ItemType.Ammunition
                || type == FileFormats.ItemType.Premium_Ammo
                )
            {
                return FileFormats.SuperType.Equipment;
            }

            if (type == FileFormats.ItemType.Mount)
            {
                return FileFormats.SuperType.Mount;
            }

            if (type == FileFormats.ItemType.Ring
                || type == FileFormats.ItemType.Earring
                || type == FileFormats.ItemType.Bracelet
                || type == FileFormats.ItemType.Necklace)
            {
                return FileFormats.SuperType.Accessory;
            }


            if (type == FileFormats.ItemType.All_Enchant
                || type == FileFormats.ItemType.Weapon_Enchant
                || type == FileFormats.ItemType.Armor_Enchant
                || type == FileFormats.ItemType.Mount_Enchant
                || type == FileFormats.ItemType.Mount_Enchant_Random
                || type == FileFormats.ItemType.Accessory_Enchant
                || type == FileFormats.ItemType.Pran_Enchant)
            {
                return SuperType.Magic_Crystals;
            }
            

            if (type == FileFormats.ItemType.Relics
                || type == FileFormats.ItemType.Mount_Stones
                || type == FileFormats.ItemType.Fishing_Rods
                || type == FileFormats.ItemType.Fishing_Bait
                || type == FileFormats.ItemType.Facions
                || type == FileFormats.ItemType.Facion_Ores
                || type == FileFormats.ItemType.Quest_Items
                || type == FileFormats.ItemType.Storage
                || type == FileFormats.ItemType.Quest_Starter_Items
                || type == FileFormats.ItemType.Sealed_Relics
                || type == FileFormats.ItemType.Holy_Water
                || type == FileFormats.ItemType.Legion_Missions
                || type == FileFormats.ItemType.Changing
                || type == FileFormats.ItemType.Magic_Boxes
                || type == FileFormats.ItemType.Evidences
                || type == FileFormats.ItemType.Gift_Boxes

                || type == FileFormats.ItemType.Portable_Repair
                || type == FileFormats.ItemType.Greater_Portable_Repair
                || type == FileFormats.ItemType.Repair_Weapon
                || type == FileFormats.ItemType.Repair_Armor
                || type == FileFormats.ItemType.Event_Goodies
                || type == FileFormats.ItemType.Money_Items
                || type == FileFormats.ItemType.Durability_Increaser
                || type == FileFormats.ItemType.Association_Mission
                || type == FileFormats.ItemType.Gold_Bar
                || type == FileFormats.ItemType.Aika_Coupon
                || type == FileFormats.ItemType.Voice_Actor_Changer
                || type == FileFormats.ItemType.Mercenary_Market
                || type == FileFormats.ItemType.Mark_Of_Battlefield
                || type == FileFormats.ItemType.Event_Items
                || type == FileFormats.ItemType.Marionette_Kit
                || type == FileFormats.ItemType.Tickets
                || type == FileFormats.ItemType.Spare_Bag
                || type == FileFormats.ItemType.Spare_Bank
                || type == FileFormats.ItemType.Spare_Pran_Bag
                || type == FileFormats.ItemType.Mount_License
                || type == FileFormats.ItemType.Mount_Change_Ticket
                || type == FileFormats.ItemType.Mark_Of_Honor
                || type == FileFormats.ItemType.Auction_Storage
                || type == FileFormats.ItemType.Dungeon_Adventurer
                || type == FileFormats.ItemType.Dungeon_Specialist
                || type == FileFormats.ItemType.Dungeon_Hero
                || type == FileFormats.ItemType.Remote_Vendor
                || type == FileFormats.ItemType.Remote_Auction
                || type == FileFormats.ItemType.Remote_Bank
                || type == FileFormats.ItemType.Mark_Of_The_Smuggler
                || type == FileFormats.ItemType.Random_Stuff 
            
            )
            {
                return SuperType.Misc_Item;
            }



            if (type == FileFormats.ItemType.Pran_Wing
                || type == FileFormats.ItemType.Pran_Doll
                || type == FileFormats.ItemType.Pran_Food
                || type == FileFormats.ItemType.Pran_Dress
                || type == FileFormats.ItemType.Pran_Accessory
                || type == FileFormats.ItemType.Pran_Headgear
                || type == FileFormats.ItemType.Pran_Digestive)
            {
                return SuperType.Pran;
            }


            if (type == FileFormats.ItemType.Weapon_Reinforce
                || type == FileFormats.ItemType.Armor_Reinforce
                || type == FileFormats.ItemType.Pel_Extract
                || type == FileFormats.ItemType.Pel_Enriched
                || type == FileFormats.ItemType.Rub_Extract
                || type == FileFormats.ItemType.Rub_Enriched
                || type == FileFormats.ItemType.Gear_Core)
            {
                return SuperType.Reinforce;
            }

            if (type == FileFormats.ItemType.HP_Consumables
                || type == FileFormats.ItemType.MP_Consumables
                || type == FileFormats.ItemType.Premium_Potions
                || type == FileFormats.ItemType.Premium_Potions_2
                || type == FileFormats.ItemType.Return_Scroll
                || type == FileFormats.ItemType.Exile_Warrant
                )
            {
                return SuperType.Consumables;
            }

            return FileFormats.SuperType.Untyped;
        }


        public enum Rank
        {
            F,
            E,
            D,
            C,
            B,
            A,
            AA,
            S,
            SS,
            SSS,
            SSSS,
        }

        public enum SuperType
        {
            Untyped,
            Equipment,
            Accessory,
            Mount,
            Magic_Crystals,
            Misc_Item,
            Pran,
            Reinforce,
            Consumables
        }

        public enum ItemQuality
        {
            Normal = 0,
            Superior_Reinforced = 1,
            Normal_Enchanted = 2,
            Unique = 3,
            Quest = 4,
            Superior = 5,
            Legendary = 6,
            Premium = 7
        }

        public enum ItemType
        {
            Head = 2,
            Body = 3,
            Glove = 4,
            Boots = 5,
            Shield = 7,
            Mount = 9,
            Ring = 11,
            Earring = 12,
            Bracelet = 13,
            Necklace = 14,
            Ammunition = 50,

            One_Hand_Sword = 1001,
            Two_Hand_Sword = 1002,
            Gun = 1005,
            Rifle = 1006,
            Wand = 1007,
            Staff = 1008,

            All_Enchant = 508,
            Weapon_Enchant = 509,
            Armor_Enchant = 510,
            Mount_Enchant = 511,
            Accessory_Enchant = 512,

            Fishing_Rods = 1019,
            Fishing_Bait = 102,

            Materials = 505,

            Relics = 40,
            Mount_Stones = 518,
            Legion_Missions = 712,
            Facions = 501,
            Facion_Ores = 506,
            Quest_Items = 507,
            Storage = 100,
            Quest_Starter_Items = 711,
            Sealed_Relics = 713,
            Magic_Boxes = 714,
            Evidences = 716,
            Gift_Boxes = 705,
            Holy_Water = 800,

            Premium_Potions = 702,
            Premium_Potions_2 = 715,
            Return_Scroll = 99,
            Exile_Warrant = 207,

            Portable_Repair = 706,
            Greater_Portable_Repair = 707,
            Repair_Weapon = 708,
            Repair_Armor = 709,
            Event_Goodies = 710,
            Money_Items = 717,
            Durability_Increaser = 718,
            Association_Mission = 719,
            Gold_Bar = 239,
            Aika_Coupon = 234,
            Voice_Actor_Changer = 521,
            Mercenary_Market = 71,
            Mark_Of_Battlefield = 404,
            Event_Items = 724,
            Marionette_Kit = 90,
            Tickets = 92,
            Spare_Bag = 217,
            Spare_Bank = 218,
            Spare_Pran_Bag = 219,
            Mount_License = 520,
            Mount_Change_Ticket = 519,
            Mark_Of_Honor = 216,
            Auction_Storage = 220,
            Dungeon_Adventurer = 221,
            Dungeon_Specialist = 222,
            Dungeon_Hero = 223,
            Remote_Vendor = 224,
            Remote_Auction = 225,
            Remote_Bank = 226,
            Mark_Of_The_Smuggler = 227,
            Random_Stuff = 59,

            Premium_Ammo = 52,

            Pran_Digestive = 230,

            Pran_Enchant = 524,
            Mount_Enchant_Random = 525,

            
            Recipe = 205,

            Gear_Core = 243,

            Weapon_Reinforce = 60,
            Armor_Reinforce = 61,
            Changing = 62,
            Pel_Extract = 63,
            Pel_Enriched = 64,
            Rub_Extract = 65,
            Rub_Enriched = 66,

            HP_Consumables = 700,
            MP_Consumables = 701,

            Pran_Wing = 22,
            Pran_Doll = 23,
            Pran_Food = 26,
            Pran_Dress = 21,
            Pran_Accessory = 20,
            Pran_Headgear = 19,

        }

        public enum SubType
        {
            Monster_Drops = 0,
            Ores_And_Metals,
            Leather,
            Cloth,
            Minerals,
            Essence,
            Fuels,
            Miscellaneous,
        }

        public enum Profession
        {
            Warrior = 1,
            Gladiator = 2,
            Crusader = 11,
            Paladin = 12,
            Sniper = 21,
            Buster = 22,
            Dual_Gunner = 31,
            Blaster = 32,
            Night_Magician = 41,
            Chaos_Sorcerer = 42,
            Priest = 51,
            Saint = 52,
        }

        public enum ProfessionType
        {
            Warrior = 1,
            Crusader = 11,
            Sniper = 21,
            Dual_Gunner = 31,
            Night_Magician = 41,
            Priest = 51,

            Fire_Pran = 61,
            Water_Pran = 71,
            Air_Pran = 81,

            Legion = 91,
            Other = 0,
        }

        public enum SkillProfession
        {

            Warrior = 1,
            Gladiator = 2,
            Crusader = 11,
            Paladin = 12,
            Sniper = 21,
            Buster = 22,
            Dual_Gunner = 31,
            Blaster = 32,
            Night_Magician = 41,
            Chaos_Sorcerer = 42,
            Priest = 51,
            Saint = 52,

            Baby_Fire_Pran = 61,
            Child_Fire_Pran = 62,
            Teen_Fire_Pran = 63,
            Adult_Fire_Pran = 64,

            Baby_Water_Pran = 71,
            Child_Water_Pran = 72,
            Teen_Water_Pran = 73,
            Adult_Water_Pran = 74,

            Baby_Air_Pran = 81,
            Child_Air_Pran = 82,
            Teen_Air_Pran = 83,
            Adult_Air_Pran = 84,

            Legion = 91,

            Other = 0,
        }

        public static GearCoreBin gearcorebin;
        public static TitleBin titlebin;
        public static RecipeRateBin reciperatebin;
        public static StrDefBin strdefbin;
        public static ItemEffectBin bin;
        public static ReinforceBinV2 reinforcewbin;
        public static ReinforceBinV2 reinforceabin;
        public static ItemListBin itemlistbin;
        public static RecipeBin recipebin;
        public static MakeItemsBin makeitemsbin;
        public static Reinforce2Bin reinforce2bin;
        public static Reinforce3Bin reinforce3bin;
        public static SetItemBin setitembin;
        public static SkillDataBin skilldatabin;
        public static MNBin mobnamebin;
        public static NPCPosBin npcposbin;
        public static NPCPosBin npcpos2bin;
        public static QuestBin questbin;
        public static ObjPosBin objposbin;
        public static DialogBin dialogbin;
        public static MobPosBin mobposbin;
        public static MapBin mapbin;
        public static Texturesizes texturesizes;
        public static NNBin nnbin;
        public static EFDescBin efdescbin;
        public static Translation translation;
        public static ExpListBin explistbin;
        public static PranExpListBin pranexplistbin;

        static Regex undesirablename = new Regex("[a-z]+[0-9]+", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public static Dictionary<Profession, Dictionary<ItemType, List<Item>>> equipmentIndex = new Dictionary<Profession, Dictionary<ItemType, List<Item>>>();
        public static Dictionary<string, HashSet<Recipe>> itemNameToRecipes = new Dictionary<string, HashSet<Recipe>>();

        public static Dictionary<string, RecipeRateEntry> recipeIDItemIDToRecipeRate = new Dictionary<string, RecipeRateEntry>();
        public static Dictionary<int, Recipe> recipes = new Dictionary<int, Recipe>();

        public static Dictionary<string, MakeItem> itemNameToMakeItem = new Dictionary<string, MakeItem>();
        public static Dictionary<string, SetEffect> itemIdToSetEffect = new Dictionary<string, SetEffect>();
        public static List<KeyValuePair<int, Item>> Relics = new List<KeyValuePair<int, Item>>();
        public static List<KeyValuePair<int, Item>> SealedRelics = new List<KeyValuePair<int, Item>>();

        public static Dictionary<int, Item> gcIdToGC = new Dictionary<int, Item>();
        public static Dictionary<int, List<GearCoreEntry>> upgradableToGearCore = new Dictionary<int, List<GearCoreEntry>>();
        public static Dictionary<int, List<GearCoreEntry>> upgradedToGearCore = new Dictionary<int, List<GearCoreEntry>>();

        public static Dictionary<string, List<Quest>> itemRewardToQuest = new Dictionary<string, List<Quest>>();
        public static Dictionary<string, List<Quest>> itemRequiredToQuest = new Dictionary<string, List<Quest>>();

        public static Dictionary<string, List<Item>> itemNeededToCraft = new Dictionary<string, List<Item>>();

        public static OneToManyMap<string, int> itemNameToIDs = new OneToManyMap<string, int>();

        static FileFormats()
        {
            string aikaFolder = Program.sourcePath;

            using (StreamWriter sw = new StreamWriter(Path.Combine(aikaFolder, @"UI\strdef" + Program.cultureNum + ".txt")))
            using (FormattedReader br = new FormattedReader(Path.Combine(aikaFolder, @"UI\strdef" + Program.cultureNum + ".bin")))
            {
                Console.WriteLine("Reading {0}", Path.GetFileName(br.Filename));
                strdefbin = br.Read<StrDefBin>();
                for (int i = 0; i < strdefbin.strs.Length; i++)
                {
                    sw.WriteLine("{0}:{1}", i, strdefbin.strs[i].GetString(9999));
                }
            }

            using (FormattedReader br = new FormattedReader(Path.Combine(aikaFolder, @"UI/NN.bin")))
            {
                Console.WriteLine("Reading {0}", Path.GetFileName(br.Filename));
                nnbin = br.Read<NNBin>();
            }

            using (FormattedReader br = new FormattedReader(Path.Combine(aikaFolder, @"UI/Textureset.bin")))
            {
                Console.WriteLine("Reading {0}", Path.GetFileName(br.Filename));
                texturesizes = br.Read<Texturesizes>();
            }

            using (StreamWriter sw = new StreamWriter("mapbin.csv")) 
            using (FormattedReader br = new FormattedReader(Path.Combine(aikaFolder, @"UI/map.bin")))
            {
                Console.WriteLine("Reading {0}", Path.GetFileName(br.Filename));
                mapbin = br.Read<MapBin>();

                sw.WriteLine("name,x0,x1,y0,y1,mystery,mystery2", mapbin.mystery2 );

                for (int i = 0; i < mapbin.names.Length; i++)
                {
                    sw.WriteLine("{0},{1},{2},{3},{4},{5},{6}", 
                        mapbin.names[i].Name,
                        mapbin.ranges[i].xBottomLeft,
                        mapbin.ranges[i].xTopRight,
                        mapbin.ranges[i].yBottomLeft,
                        mapbin.ranges[i].yTopRight,
                        mapbin.mystery[i],
                        mapbin.mystery2[i]);
                }


                Regex r = new Regex("z[0-9]{2}(?<x>[0-9]{2})(?<y>[0-9]{2})", RegexOptions.IgnoreCase);

                // world map
                Console.WriteLine("Converting world map files...");
                string[] worldMapFiles = Directory.GetFiles(Program.sourcePath + @"\Env", "*.jit");
                foreach (var file in worldMapFiles)
                {
                    JitConverter.GetImage(file).Save(file + ".png");
                }

                Console.WriteLine("Generating world map");
                string[] files = Directory.GetFiles(Program.sourcePath + @"\Env", "mmz*.jit.png");
                int size = 512;

                int maxx = 0;
                int maxy = 0;
                foreach (var file in files)
                {
                    Match m = r.Match(file);
                    int coordx = int.Parse(m.Groups["x"].Value);
                    int coordy = int.Parse(m.Groups["y"].Value);

                    if (maxx < coordx)
                    {
                        maxx = coordx;
                    }
                    if (maxy < coordy)
                    {
                        maxy = coordy;
                    }
                }

                int width = (maxx + 1) * size;
                int height = (maxy + 1) * size;
                using (Bitmap b = new Bitmap(width, height))
                {
                    using (Graphics g = Graphics.FromImage(b))
                    {
                        g.FillRectangle(Brushes.Black, new Rectangle(0, 0, width, height));
                        foreach (var file in files)
                        {
                            Match m = r.Match(file);
                            int coordx = int.Parse(m.Groups["x"].Value);
                            int coordy = int.Parse(m.Groups["y"].Value);

                            using (Image img = Image.FromFile(file))
                            {
                                g.DrawImage(img,
                                    new Rectangle(coordx * size, (maxy - coordy - 1) * size, size, size),
                                    new Rectangle(0,0,img.Width,img.Height),
                                    GraphicsUnit.Pixel
                                    );
                            }
                        }
                    }

                    b.Save("worldmap.png", ImageFormat.Png);
                    b.Save("worldmap.jpg", ImageFormat.Jpeg);

                    //JitConverter.MakeMapWithMarkers("worldmap.png", "worldmap", "World Map", new PointF[0]);

                    using (Graphics g = Graphics.FromImage(b))
                    {
                        for (int i = 0; i < FileFormats.mapbin.Maps.Length; i++)
                        {
                            string pngname = @"Map" + (i+1).ToString("00") + ".jit.png";
                            string srcdir = Program.sourcePath + @"\UI\";
                            string path = Path.Combine(srcdir, pngname);

                            int imgwidth;
                            int imgheight;
                            if ((i+1) >= FileFormats.texturesizes.mapsizes.Length || FileFormats.texturesizes.mapsizes[i].width == 0 || FileFormats.texturesizes.mapsizes[i].height == 0)
                            {
                                imgwidth = (FileFormats.mapbin.Maps[i].range.xTopRight - FileFormats.mapbin.Maps[i].range.xBottomLeft) * size / 256;
                                imgheight = (FileFormats.mapbin.Maps[i].range.yTopRight - FileFormats.mapbin.Maps[i].range.yBottomLeft) * size / 256;

                                using (Bitmap newMap = new Bitmap(imgwidth, imgheight))
                                {
                                    using (Graphics gg = Graphics.FromImage(newMap))
                                    {
                                        gg.DrawImage(b,
                                            new Rectangle(0, 0, imgwidth, imgheight),
                                            new Rectangle(
                                                FileFormats.mapbin.Maps[i].range.xBottomLeft * size / 256,
                                                maxy * size - FileFormats.mapbin.Maps[i].range.yTopRight * size / 256,
                                                imgwidth,
                                                imgheight), GraphicsUnit.Pixel);

                                    }
                                    newMap.Save(path);
                                }
                            }
                            else
                            {
                                imgwidth = FileFormats.texturesizes.mapsizes[i].width;
                                imgheight = FileFormats.texturesizes.mapsizes[i].height;
                            } 

                           
                            using (Image img = Image.FromFile(path))
                            {

                                g.DrawImage(img,

                                    new Rectangle(
                                    FileFormats.mapbin.Maps[i].range.xBottomLeft * size / 256,
                                    maxy * size - FileFormats.mapbin.Maps[i].range.yTopRight * size / 256,
                                    (FileFormats.mapbin.Maps[i].range.xTopRight - FileFormats.mapbin.Maps[i].range.xBottomLeft) * size / 256,
                                    (FileFormats.mapbin.Maps[i].range.yTopRight - FileFormats.mapbin.Maps[i].range.yBottomLeft) * size / 256
                                    ),

                                    new Rectangle(0, 0, imgwidth, imgheight),
                                    GraphicsUnit.Pixel

                                    );
                            }

                        }
                    }

                    b.Save("worldmap2.png");
                    b.Save("worldmap2.jpg", ImageFormat.Jpeg);
                }

            }

            using (FormattedReader br = new FormattedReader(Path.Combine(aikaFolder, @"UI\mobpos.bin")))
            {
                Console.WriteLine("Reading {0}", Path.GetFileName(br.Filename));
                mobposbin = br.Read<MobPosBin>();
            }

            using (FormattedReader br = new FormattedReader(Path.Combine(aikaFolder, @"UI/dialog.bin.dec")))
            {
                Console.WriteLine("Reading {0}", Path.GetFileName(br.Filename));
                dialogbin = br.Read<DialogBin>();
            }

            using (FormattedReader br = new FormattedReader(Path.Combine(aikaFolder, @"UI\objpos.bin")))
            {
                Console.WriteLine("Reading {0}", Path.GetFileName(br.Filename));
                objposbin = br.Read<ObjPosBin>();
            }

            using (FormattedReader br = new FormattedReader(Path.Combine(aikaFolder, @"UI/Explist.bin")))
            {
                Console.WriteLine("Reading {0}", Path.GetFileName(br.Filename));
                explistbin = br.Read<ExpListBin>();
            }

            using (FormattedReader br = new FormattedReader(Path.Combine(aikaFolder, @"UI/PranExplist.bin")))
            {
                Console.WriteLine("Reading {0}", Path.GetFileName(br.Filename));
                pranexplistbin = br.Read<PranExpListBin>();
            }

            using (FormattedReader br = new FormattedReader(Path.Combine(aikaFolder, @"UI/MN.bin.dec")))
            {
                Console.WriteLine("Reading {0}", Path.GetFileName(br.Filename));
                mobnamebin = br.Read<MNBin>();
            }

            using (FormattedReader br = new FormattedReader(Path.Combine(aikaFolder, @"UI/npcpos.bin")))
            {
                Console.WriteLine("Reading {0}", Path.GetFileName(br.Filename));
                npcposbin = br.Read<NPCPosBin>();
            }

            if (File.Exists(Path.Combine(aikaFolder, @"UI/npcpos2.bin")))
            {
                using (FormattedReader br = new FormattedReader(Path.Combine(aikaFolder, @"UI/npcpos2.bin")))
                {
                    Console.WriteLine("Reading {0}", Path.GetFileName(br.Filename));
                    npcpos2bin = br.Read<NPCPosBin>();
                }
            }

            using (FormattedReader br = new FormattedReader(Path.Combine(aikaFolder, @"UI\Reinforce3.bin.dec")))
            {
                Console.WriteLine("Reading {0}", Path.GetFileName(br.Filename));
                reinforce3bin = br.Read<Reinforce3Bin>();
            }

            using (FormattedReader br = new FormattedReader(Path.Combine(aikaFolder, @"UI\Reinforce2.bin.dec")))
            {
                Console.WriteLine("Reading {0}", Path.GetFileName(br.Filename));
                reinforce2bin = br.Read<Reinforce2Bin>();
            }

            using (FormattedReader br = new FormattedReader(Path.Combine(aikaFolder, @"UI\ReinforceW01.bin.dec")))
            {
                Console.WriteLine("Reading {0}", Path.GetFileName(br.Filename));

                if (Program.reinforceVersion == 2)
                {
                    reinforcewbin = br.Read<ReinforceBinV2>();
                }
                else
                {
                    var m = br.Read<ReinforceBin>();
                    reinforcewbin.sets = m.sets.Select(x => new ReinforceLayoutV2()
                    {
                        chance = x.chance,
                        mystery = x.mystery,
                        price = x.price

                    }).ToArray();
                }
            }

            using (FormattedReader br = new FormattedReader(Path.Combine(aikaFolder, @"UI\ReinforceA01.bin.dec")))
            {
                Console.WriteLine("Reading {0}", Path.GetFileName(br.Filename));

                if (Program.reinforceVersion == 2)
                {
                    reinforceabin = br.Read<ReinforceBinV2>();
                }
                else
                {
                    var m = br.Read<ReinforceBin>();
                    reinforceabin.sets = m.sets.Select(x => new ReinforceLayoutV2()
                    {
                        chance = x.chance,
                        mystery = x.mystery,
                        price = x.price

                    }).ToArray();
                }
            }


            if (File.Exists(Path.Combine(aikaFolder, @"UI/EFDesc.bin")))
            {
                using (FormattedReader br = new FormattedReader(Path.Combine(aikaFolder, @"UI/EFDesc.bin")))
                {
                    Console.WriteLine("Reading {0}", Path.GetFileName(br.Filename));
                    efdescbin = br.Read<EFDescBin>();
                    efdescbin.descs[0] = new EFDesc() { name = Encoding.UTF8.GetBytes(strdefbin.None) };
                }
            }
            else
            {
                efdescbin = new EFDescBin();
                efdescbin.descs = new EFDesc[0];
            }

            using (FormattedReader br = new FormattedReader(Path.Combine(aikaFolder, @"UI\ItemEffect.bin")))
            {
                Console.WriteLine("Reading {0}", Path.GetFileName(br.Filename));
                bin = br.Read<ItemEffectBin>();
            }

            using (StreamWriter sw = new StreamWriter("skilllist.csv")) 
            using (FormattedReader br = new FormattedReader(Path.Combine(aikaFolder, @"SkillData" + Program.cultureNum + ".bin.dec")))
            {
                Console.WriteLine("Reading {0}", Path.GetFileName(br.Filename));
                skilldatabin = br.Read<SkillDataBin>();

                sw.WriteLine("iconid,name,profession,level,tier,proftype,desc,levelreq,skillLevelCap,skillPointCost,guildSkill,mystery3,mystery7,{0}", 
                    string.Join(",", Enumerable.Range(0, skilldatabin.skills[0].mysteries.Length).Select(x => "unknown" + x.ToString())));

                for (int i = 0; i < skilldatabin.skills.Length; i++)
                {

                    if (i < 5761)
                    {
                        // each tier block has 6 skills of 16 entries each
                        // each profession has 10 tier blocks
                        skilldatabin.skills[i].Tier = ((i - 1) / (6 * 16)) % 10;
                    }

                    if (i < 6111 || skilldatabin.skills[i].skillprofession == SkillProfession.Legion)
                    {
                        // manufacture the class type and
                        // use it instead of profession.
                        // i-1 is the index minus null
                        skilldatabin.skills[i].ProfType = (ProfessionType)(((int)(skilldatabin.skills[i].skillprofession) / 10) * 10) + 1;
                    }


                    sw.WriteLine("{0},{1},{2},{3},{4},{5},\"{6}\",{7},{8},{9},{10},{11},{12},{13}",
                        skilldatabin.skills[i].iconid,
                        skilldatabin.skills[i].Name,
                        skilldatabin.skills[i].skillprofession,
                        skilldatabin.skills[i].skillLevel,
                        skilldatabin.skills[i].Tier,
                        skilldatabin.skills[i].ProfType,
                        skilldatabin.skills[i].Type,
                        skilldatabin.skills[i].levelRequired,
                        skilldatabin.skills[i].skillLevelCap,
                        skilldatabin.skills[i].skillPointCost,
                        skilldatabin.skills[i].guildSkill,
                        skilldatabin.skills[i].mystery3,
                        skilldatabin.skills[i].mystery7,
                        string.Join(",", skilldatabin.skills[i].mysteries)
                        );

                }
            }

            using (StreamWriter sw = new StreamWriter("itemlist.csv")) 
            using (FormattedReader br = new FormattedReader(Path.Combine(aikaFolder, "ItemList" + Program.cultureNum + ".bin.dec")))
            {
                Console.WriteLine("Reading {0}", Path.GetFileName(br.Filename));

                if (Program.cultureNum == 2)
                {
                    var read = br.Read<ItemListBinJP>();
                    itemlistbin.items = read.items;
                    itemlistbin.stuff = read.stuff;
                }
                else
                {
                    itemlistbin = br.Read<ItemListBin>();
                }

                sw.WriteLine("id,name,type,subtype,supertype,profession,{0}", string.Join(",", Enumerable.Range(0, itemlistbin.items[0].things.Length)));

                for (int i = 0; i < itemlistbin.items.Length; i++)
                {
                    (itemlistbin.items[i].ID) = (ushort)i;
                    var item = itemlistbin.items[i];

                    itemNameToIDs.Add(itemlistbin.items[i].Name, i);

                    sw.WriteLine("{0},{1},{2},{3},{4},{5},{6}", 
                        itemlistbin.items[i].ID,
                        itemlistbin.items[i].Name,
                        itemlistbin.items[i].ItemType, 
                        itemlistbin.items[i].SubType,
                        itemlistbin.items[i].SuperType, 
                        itemlistbin.items[i].Profession,
                        string.Join(",", itemlistbin.items[i].things));

                    if (item.ItemType == ItemType.Relics)
                    {
                        Relics.Add(new KeyValuePair<int, Item>(i, item));
                    }

                    if (item.ItemType == ItemType.Sealed_Relics)
                    {
                        if (i != 5855)
                        {
                            SealedRelics.Add(new KeyValuePair<int, Item>(i, item));
                        }
                    }

                    if (item.ItemType == ItemType.Gear_Core)
                    {
                        gcIdToGC[item.GearCoreLevel] = item;
                    }

                    if (item.Name.Length != 0)
                    {
                        //Invoke(new Action(() => {
                        //    int rownum = dataGridView1.Rows.Add(str.ToArray());
                        //    dataGridView1.Rows[rownum].Height = 44;
                        //}));

                        if (!equipmentIndex.ContainsKey(item.Profession))
                        {
                            equipmentIndex[item.Profession] = new Dictionary<ItemType, List<Item>>();
                        }
                        if (!equipmentIndex[item.Profession].ContainsKey(item.ItemType))
                        {
                            equipmentIndex[item.Profession][item.ItemType] = new List<Item>();
                        }

                        equipmentIndex[item.Profession][item.ItemType].Add(item);

                    }
                }
            }

            translation = new Translation(strdefbin, itemlistbin, Program.trans);

            using (FormattedReader br = new FormattedReader(Path.Combine(aikaFolder, @"UI/Quest.bin.dec")))
            {
                Console.WriteLine("Reading {0}", Path.GetFileName(br.Filename));
                questbin = br.Read<QuestBin>();

                questbin.quests.Where(x => x.id != 0).ToList().ForEach(x => {
                    x.Items.ToList().ForEach(item =>
                    {
                        string slug = SiteGenerator.MakeEquipSlug(item.Key);
                        if (!itemRewardToQuest.ContainsKey(slug))
                        {
                            itemRewardToQuest[slug] = new List<Quest>();
                        }
                        itemRewardToQuest[slug].Add(x);
                    });

                    x.ItemsRequired.ToList().ForEach(item =>
                    {
                        string slug = SiteGenerator.MakeEquipSlug(item.Key);
                        if (!itemRequiredToQuest.ContainsKey(slug))
                        {
                            itemRequiredToQuest[slug] = new List<Quest>();
                        }
                        itemRequiredToQuest[slug].Add(x);
                    });
                });
                
            }


            using (FormattedReader br = new FormattedReader(Path.Combine(aikaFolder, @"UI\Recipe.bin.dec")))
            {
                Console.WriteLine("Reading {0}", Path.GetFileName(br.Filename));
                recipebin = br.Read<RecipeBin>();

                foreach (var recipe in recipebin.recs)
                {
                    if (!itemNameToRecipes.ContainsKey(recipe.CreatedItem.Name))
                    {
                        itemNameToRecipes[recipe.CreatedItem.Name] = new HashSet<Recipe>();
                    }
                    itemNameToRecipes[recipe.CreatedItem.Name].Add(recipe);

                    recipe.Ingredients.ToList().ForEach(item =>
                    {
                        string slug = SiteGenerator.MakeEquipSlug(item.Key);
                        if (!itemNeededToCraft.ContainsKey(slug))
                        {
                            itemNeededToCraft[slug] = new List<Item>();
                        }
                        itemNeededToCraft[slug].Add(recipe.CreatedItem);
                    });

                    recipes[recipe.itemid] = recipe;
                }

            }

            using (FormattedReader br = new FormattedReader(Path.Combine(aikaFolder, @"UI\RecipeRate.bin.dec")))
            {
                Console.WriteLine("Reading {0}", Path.GetFileName(br.Filename));
                reciperatebin = br.Read<RecipeRateBin>();

                foreach (var reciperate in reciperatebin.entries)
                {
                    recipeIDItemIDToRecipeRate[reciperate.recipeid + "_" + reciperate.ingredientid] = reciperate;
                }
            }

            using (FormattedReader br = new FormattedReader(Path.Combine(aikaFolder, @"UI\Title.bin")))
            {
                Console.WriteLine("Reading {0}", Path.GetFileName(br.Filename));
                titlebin = br.Read<TitleBin>();
            }


            using (FormattedReader br = new FormattedReader(Path.Combine(aikaFolder, @"UI\MakeItems.bin.dec")))
            {
                Console.WriteLine("Reading {0}", Path.GetFileName(br.Filename));
                makeitemsbin = br.Read<MakeItemsBin>();
                foreach (var recipe in makeitemsbin.recipes)
                {
                    itemNameToMakeItem[recipe.CreatedItem.Name] = recipe;
                    itemNameToMakeItem[recipe.CreatedItem.Name.Replace(translation.Translate("Superior "), "")] = recipe;

                    recipe.Ingredients.ToList().ForEach(item =>
                    {
                        string slug = SiteGenerator.MakeEquipSlug(item.Key);
                        if (!itemNeededToCraft.ContainsKey(slug))
                        {
                            itemNeededToCraft[slug] = new List<Item>();
                        }
                        itemNeededToCraft[slug].Add(recipe.CreatedItem);
                    });
                }
            }


            using (FormattedReader br = new FormattedReader(Path.Combine(aikaFolder, @"UI\SetItem.bin.dec")))
            {
                Console.WriteLine("Reading {0}", Path.GetFileName(br.Filename));
                setitembin = br.Read<SetItemBin>();
                foreach (var set in setitembin.sets)
                {
                    foreach (Item item in set.SetItems)
                    {
                        if (item.ID != 0)
                        {
                            itemIdToSetEffect[ItemSetKey(item)] = set;
                        }
                    }
                }
            }

            using (FormattedReader br = new FormattedReader(Path.Combine(aikaFolder, @"UI\GearCore.bin")))
            {
                Console.WriteLine("Reading {0}", Path.GetFileName(br.Filename));
                gearcorebin = br.Read<GearCoreBin>();

                foreach (var entry in gearcorebin.entries)
                {
                    if (!upgradableToGearCore.ContainsKey(entry.sourceItem))
                    {
                        upgradableToGearCore[entry.sourceItem] = new List<GearCoreEntry>();
                    }
                    if (!upgradedToGearCore.ContainsKey(entry.destItem))
                    {
                        upgradedToGearCore[entry.destItem] = new List<GearCoreEntry>();
                    }
                    upgradableToGearCore[entry.sourceItem].Add(entry);
                    upgradedToGearCore[entry.destItem].Add(entry);
                }
            }

        }

        public static string ItemSetKey(Item item)
        {
            return item.Profession.ToString() + item.Name;
        }
    }
}
