using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using BlueBlocksLib.Reports;
using System.Text.RegularExpressions;
using System.IO;
using BlueBlocksLib.SetUtils;
using System.Drawing.Imaging;
using System.Globalization;
using System.Drawing;
using System.Reflection;
using System.Diagnostics;

namespace JitOpener
{
    class SiteGenerator
    {
        public static string SimpleTxt;
        public static string TemplateTxt;
        public static Image PointPng;
        static SiteGenerator()
        {
            var assembly = Assembly.GetExecutingAssembly();



            using (Stream stream = assembly.GetManifestResourceStream("JitOpener.simple.php"))
            using (StreamReader reader = new StreamReader(stream))
            {
                SimpleTxt = reader.ReadToEnd();
            }

            using (Stream stream = assembly.GetManifestResourceStream("JitOpener.template.html"))
            using (StreamReader reader = new StreamReader(stream))
            {
                TemplateTxt = reader.ReadToEnd();
            }

            using (Stream stream = assembly.GetManifestResourceStream("JitOpener.point.png"))
            PointPng = Image.FromStream(stream);
        }

        [StructLayout(LayoutKind.Sequential)]
        struct NPCDisplay
        {
            [HeaderName("Name")]
            public string Name;

            [HeaderName("Position")]
            public string Pos;

        }

        [StructLayout(LayoutKind.Sequential)]
        struct ItemDisplay
        {

            [HeaderName("Icon")]
            public string Icon;

            [HeaderName("Name")]
            public string Name;

            [HeaderName("Level")]
            public string Level;

            [HeaderName("Rank")]
            public string Rank;

            [HeaderName("PhyAtk")]
            public string PhyAtk;

            [HeaderName("MagAtk")]
            public string MagAtk;

            [HeaderName("PhyDef")]
            public string PhyDef;

            [HeaderName("MagDef")]
            public string MagDef;

            [HeaderName("Honor Points")]
            public string Honor;

            [HeaderName("Medals of Hero")]
            public string HeroMedals;

            [HeaderName("NPC Sell Price")]
            public string NPCSellPrice;

            [HeaderName("Effects")]
            public string Effects;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct AccessoryDisplay
        {

            [HeaderName("Icon")]
            public string Icon;

            [HeaderName("Name")]
            public string Name;

            [HeaderName("Level")]
            public string Level;

            [HeaderName("Rank")]
            public string Rank;

            [HeaderName("Honor Points")]
            public string Honor;

            [HeaderName("Medals of Hero")]
            public string HeroMedals;

            [HeaderName("NPC Sell Price")]
            public string NPCSellPrice;

            [HeaderName("Effects")]
            public string Effects;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct SkillOutLine
        {
            [ColumnSize(55)]
            [HeaderName("Icon")]
            public string Icon;

            [HeaderName("Name")]
            public string Name;

            [HeaderName("Minimum Level")]
            public string MinimumLevel;

            [HeaderName("Min Caelium Required")]
            public string MinFacionRequired;

            [HeaderName("Max Caelium Required")]
            public string MaxFacionRequired;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct SkillDisplay
        {
            [ColumnSize(55)]
            [HeaderName("Icon")]
            public string Icon;

            [HeaderName("Name")]
            public string Name;

            [HeaderName("Level")]
            public string Level;

            [HeaderName("Level Required")]
            public string LevelReq;

            [HeaderName("Used By")]
            public string SkilProf;

            [HeaderName("Description")]
            public string Desc;

            [HeaderName("Learning Cost")]
            public string Cost;

            [HeaderName("MP Required")]
            public string MP;

            [HeaderName("Piece of Facion Required")]
            public string Facion;

            [HeaderName("Casting Time")]
            public string CastTime;

            [HeaderName("Cooldown Time")]
            public string CoolDownTime;

            [HeaderName("Spell Duration")]
            public string SpellDuration;

            [HeaderName("Instant Effect")]
            public string InstantEffect;

            [HeaderName("Lingering Effect")]
            public string LingeringEffect;

            [HeaderName("Passive Effect")]
            public string PassiveEffect;
        }

        

        private static string SaveImageOfSkill(string path, FileFormats.Skill skill)
        {
            string iconName = Program.destPath + "/images/" + slugger.Replace(MakeSkillSlug(skill),"/").Trim('/').Replace("con","ccon") + ".png";
            string localPathname = Path.Combine(path, iconName);
            string iconTag = @"<img src=""" + iconName + @""" alt=""" + skill.Name + @""">";

            if (!Directory.Exists(Path.GetDirectoryName(localPathname)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(localPathname));
            }

            // Put the icon in images
            if (!File.Exists(localPathname))
            {
                skill.Image.Save(localPathname, ImageFormat.Png);
            }
            return iconTag;
        }

        static Regex skillSlugRegex = new Regex(@"[^\w]+", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        static Regex 余計なスキル表示 = new Regex(@"Lv\.?[0-9]+", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private static string MakeSkillSlug(FileFormats.Skill skill)
        {
            string noLv = 余計なスキル表示.Replace(skill.Name, "", 100);
            string noInvalidChar = skillSlugRegex.Replace(noLv, "-", 100);

            return RemoveDiacritics("skill-" + MakeSlug(skill
                .skillprofession) + "-" + noInvalidChar.ToLower());
        }

        [StructLayout(LayoutKind.Sequential)]
        struct SetItemDisplayForEquip
        {
            [ColumnSize(55)]
            [HeaderName("Icon")]
            public string Icon;

            [HeaderName("Name")]
            public string Name;
        }


        [StructLayout(LayoutKind.Sequential)]
        class QuestSeriesDisplay
        {
            [HeaderName("Series Name")]
            public string Name;

            [HeaderName("Level Requirement")]
            public string Level;

            [HeaderName("Start Point")]
            public string StartPoint;
        }

        [StructLayout(LayoutKind.Sequential)]
        class QuestDisplay
        {
            [HeaderName("Name")]
            public string Name;

            [HeaderName("Level Requirement")]
            public string Level;

            [HeaderName("Previous Mission")]
            public string Prerequisite;

            [HeaderName("Order In Series")]
            public string SeriesNumber;

            [HeaderName("Start Point")]
            public string StartPoint;

            [HeaderName("EXP Reward")]
            public string EXPReward;

            [HeaderName("Gold Reward")]
            public string GoldReward;

        }

        [StructLayout(LayoutKind.Sequential)]
        struct SetItemDisplay
        {
            [HeaderName("Name")]
            public string Name;

            [HeaderName("Level")]
            public string Level;

            [HeaderName("ItemIcons")]
            public string ItemIcons;

            [HeaderName("Effects")]
            public string Effects;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct MountDisplay
        {
            [HeaderName("Icon")]
            public string Icon;

            [HeaderName("Name")]
            public string Name;

            [HeaderName("Description")]
            public string Desc;

            [HeaderName("Level")]
            public string Level;

            [HeaderName("Effects")]
            public string Effects;
        }


        public static string AddSpacesBeforeCapitals(string str)
        {
            return FileFormats.translation.Translate(str.Replace("_", " "));
        }

        public static NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;


        interface IDijitMenuItem
        {
            string Render(RenderType type);
        }


        class DijitCategoryPane : IDijitMenuItem
        {
            string menutitle;
            string menuslug;
            public DijitCategoryPane(string title, string slug)
            {
                this.menutitle = title;
                this.menuslug = slug;
            }

            List<IDijitMenuItem> items = new List<IDijitMenuItem>();
            public void Add(IDijitMenuItem item)
            {
                items.Add(item);
            }

            public string Render(RenderType type)
            {
                string menu = "";


                if (type == RenderType.Dijit)
                {

                    menu += @"

            <div dojotype=""dijit.layout.AccordionPane"" id=""" + menuslug + @""" title=""" + menutitle + @""" style=""padding: 0px;"">
                <div dojotype=""dijit.layout.AccordionContainer"">
            ";

                    foreach (var item in items)
                    {
                        menu += item.Render(type);
                    }

                    menu += @"
                </div>
            </div>
";
                }
                else
                {
                    menu += @"
<table class=""pretty-table"">
            <thead>
            <th>" + menutitle + @"</th>
            </thead>
";


                    foreach (var item in items)
                    {
                        menu += @"
            <tr>
            <td>" + item.Render(type) + @"</td>
            </tr>
";
                    }


                    menu += @"
            </table>
";

                }

                return menu;
            }
        }

        enum RenderType
        {
            Dijit,
            Table,
        }

        class DijitMenuPane : IDijitMenuItem
        {
            string menutitle;
            string menuslug;
            public DijitMenuPane(string title, string slug)
            {
                this.menutitle = title;
                this.menuslug = slug;
            }

            List<KeyValuePair<string, string>> slugToTitle = new List<KeyValuePair<string, string>>();
            public void AddMenuItem(string slug, string title)
            {
                slugToTitle.Add(new KeyValuePair<string, string>(slug, title));
            }

            public string Render(RenderType type)
            {
                string menu = "";

                if (type == RenderType.Dijit)
                {


                    menu += @"
            <div dojotype=""dijit.layout.AccordionPane"" id=""" + menuslug + @""" title=""" + menutitle + @""" style=""padding: 0px;"">
                <div dojotype=""dijit.Menu"" style=""border:0px;"">";

                    foreach (var kvpair in slugToTitle)
                    {
                        menu += @"
					<div dojotype=""dijit.MenuItem"" onclick=""dojo.hash('" + kvpair.Key + @"')"">		   
					";
                        menu += kvpair.Value;
                        menu += @"
                    </div>
					";
                    }

                    menu += @"
                </div>
            </div>
";
                }
                else if (type == RenderType.Table)
                {
                    menu += @"
<table class=""pretty-table"">
            <thead>
            <th>" + menutitle + @"</th>
            </thead>
";


                    foreach (var kvpair in slugToTitle)
                    {

                        menu += @"
            <tr>
            <td><a href=""simple" + Program.indexsuffix + @".php?" + kvpair.Key + @""">" + kvpair.Value + @"</a></td>
            </tr>
";
                    }


                    menu += @"
            </table>
";
                }

                return menu;
            }
        }


        class DijitMenu
        {
            List<IDijitMenuItem> items = new List<IDijitMenuItem>();
            public void Add(IDijitMenuItem item)
            {
                items.Add(item);
            }

            public string Render(RenderType type)
            {
                string menu = "";

                foreach (var item in items)
                {
                    menu += item.Render(type);
                }

                return menu;
            }
        }

        class BySeries : IEqualityComparer<FileFormats.Quest>
        {
            public bool Equals(FileFormats.Quest x, FileFormats.Quest y)
            {
                return x.SeriesStart.Name == y.SeriesStart.Name;
            }

            public int GetHashCode(FileFormats.Quest obj)
            {
                return obj.SeriesStart.Name.GetHashCode();
            }
        }

        static Regex slugger = new Regex(@"[^\w]+", RegexOptions.IgnoreCase);

        public static void Import()
        {
            Dictionary<JitOpener.FileFormats.Profession, Dictionary<JitOpener.FileFormats.ItemType, List<JitOpener.FileFormats.Item>>> equipmentIndex =
                FileFormats.equipmentIndex;
            nfi.NumberDecimalDigits = 0;

            string path = Program.webpath;

            string template = TemplateTxt;

            string simple = SimpleTxt;

            DijitMenu menu = new DijitMenu();
            DijitCategoryPane equippane = new DijitCategoryPane("Equipment", "equipment");
            menu.Add(equippane);

            EnumUtil.GetValues<JitOpener.FileFormats.Profession>().ToList().ForEach(prof =>
            {
                if (!equipmentIndex.ContainsKey(prof))
                {
                    return;
                }
                DijitMenuPane profpane = new DijitMenuPane(AddSpacesBeforeCapitals(prof.ToString()), prof.ToString());

                EnumUtil.GetValues<JitOpener.FileFormats.ItemType>().ToList().ForEach(type =>
                {

                    if (!equipmentIndex[prof].ContainsKey(type) || type == FileFormats.ItemType.Recipe)
                    {
                        return;
                    }
                    profpane.AddMenuItem(MakeSlug(prof, type), AddSpacesBeforeCapitals(type.ToString()));

                });

                profpane.AddMenuItem(MakeSetItemSlug(prof), FileFormats.translation.Translate("Equipment Sets"));

                equippane.Add(profpane);
            });

            DijitMenuPane accpane = new DijitMenuPane( FileFormats.translation.Translate("Accessories"), "accessories");
            equippane.Add(accpane);
            EnumUtil.GetValues<JitOpener.FileFormats.ItemType>().
                Where(x => FileFormats.GetSuperType(x) == FileFormats.SuperType.Accessory).
                ToList().ForEach(type =>
                {
                    accpane.AddMenuItem(MakeSlug(type), AddSpacesBeforeCapitals(type.ToString()));

                    AccessoryDisplay[] accesories = FileFormats.itemlistbin.items.
                        Where(item => item.ItemType == type).
                        Where(item => item.Name[0] < 256 || Program.allowKorean).	// Grab only the ones with non-korean names
                        Where(item => item.Name[1] < 256 || Program.allowKorean).	// Grab only the ones with non-korean names
                        Where(item => item.things[32] != 0).
                        Select(item =>
                        {
                            string link = MakePageForItem(path, item);

                            return new AccessoryDisplay()
                                {
                                    Icon = SaveImageOfItem(path, item),
                                    Name = link,
                                    HeroMedals = item.MedalsOfHero.ToString(),
                                    Honor = item.HonorPoints.ToString(),
                                    Level = item.Level.ToString(),
                                    NPCSellPrice = item.SellPrice.ToString(),
                                    Effects = makeEffectString(item),
                                    Rank = item.Rank.ToString()
                                };
                        }).OrderBy(x => ushort.Parse(x.Level)).ToArray();

                    HTMLTable<AccessoryDisplay> accdisp = new HTMLTable<AccessoryDisplay>(accesories);
                    accdisp.ClassName = "clothed";

                    string page = MakeSlug(type);
                    string table = accdisp.Render(FileFormats.translation.Translate);
                    MakePage(path, page, table);
                });

            accpane.AddMenuItem( FileFormats.translation.Translate("accessory-sets"),  FileFormats.translation.Translate("Accessory Sets"));

            {
                SetItemDisplay[] display = FileFormats.setitembin.sets.
                    Where(x => x.numitems > 0 && x.SetItems[0].SuperType == FileFormats.SuperType.Accessory).
                    Select(x => new SetItemDisplay()
                    {
                        Name = x.Name,
                        Level = x.SetItems[0].Level.ToString(),
                        ItemIcons = string.Join(" ", x.SetItems.
                            Where(item => item.ID != 0).
                            Select(item => GetEquipLink(item, SaveImageOfItem(path, item)))),
                        Effects = MakeEffectsList(x)

                    }).OrderBy(x => short.Parse(x.Level)).ToArray();
                HTMLTable<SetItemDisplay> setitemdisp = new HTMLTable<SetItemDisplay>(display);
                setitemdisp.ClassName = "clothed";

                string page =  FileFormats.translation.Translate("accessory-sets");
                string table = setitemdisp.Render(FileFormats.translation.Translate);
                MakePage(path, page, table);
            }

            DijitCategoryPane itemspane = new DijitCategoryPane( FileFormats.translation.Translate("Items"), "items");
            menu.Add(itemspane);

            DijitMenuPane enchantpane = new DijitMenuPane( FileFormats.translation.Translate("Magic Crystals"), "magiccrystals");
            itemspane.Add(enchantpane);

            EnumUtil.GetValues<JitOpener.FileFormats.ItemType>().
                Where(x => FileFormats.GetSuperType(x) == FileFormats.SuperType.Magic_Crystals).
                ToList().ForEach(x =>
                {

                    enchantpane.AddMenuItem(MakeSlug(x), AddSpacesBeforeCapitals(x.ToString()));
                });


            DijitMenuPane materialpane = new DijitMenuPane( FileFormats.translation.Translate("Materials"), "materials");
            itemspane.Add(materialpane);

            EnumUtil.GetValues<JitOpener.FileFormats.SubType>().
                ToList().ForEach(x =>
                {
                    materialpane.AddMenuItem(MakeSlug(x), AddSpacesBeforeCapitals(x.ToString()));
                });


            DijitMenuPane consumableitempane = new DijitMenuPane( FileFormats.translation.Translate("Consumable Items"), "consumableitems");
            itemspane.Add(consumableitempane);

            EnumUtil.GetValues<JitOpener.FileFormats.ItemType>().
                Where(x => FileFormats.GetSuperType(x) == FileFormats.SuperType.Consumables).
                ToList().ForEach(x =>
                {

                    consumableitempane.AddMenuItem(MakeSlug(x), AddSpacesBeforeCapitals(x.ToString()));
                });

            DijitMenuPane equipenhancementpane = new DijitMenuPane( FileFormats.translation.Translate("Equip Enhancement"), "equipenhancement");
            itemspane.Add(equipenhancementpane);

            EnumUtil.GetValues<JitOpener.FileFormats.ItemType>().
                Where(x => FileFormats.GetSuperType(x) == FileFormats.SuperType.Reinforce).
                ToList().ForEach(x =>
                {

                    equipenhancementpane.AddMenuItem(MakeSlug(x), AddSpacesBeforeCapitals(x.ToString()));
                });


            DijitMenuPane pranitempane = new DijitMenuPane( FileFormats.translation.Translate("Pran Items"), "pranitems");
            itemspane.Add(pranitempane);

            EnumUtil.GetValues<JitOpener.FileFormats.ItemType>().
                Where(x => FileFormats.GetSuperType(x) == FileFormats.SuperType.Pran).
                ToList().ForEach(x =>
                {
                    string pranpagename = MakeSlug(x);

                    Dictionary<string, FileFormats.Item> distinct = new Dictionary<string, FileFormats.Item>();
                    FileFormats.itemlistbin.items.
                        Where(item => item.things[32] != 0).
                        Where(item => item.ItemType == x).
                        ToList().ForEach(item =>
                    {

                        if (!distinct.ContainsKey(item.Name))
                        {
                            distinct[item.Name] = item;
                        }
                    });


                    var pranitems = distinct.Values.
                        Select(item =>
                        new MountDisplay()
                        {
                            Icon = SaveImageOfItem(path, item),
                            Name = MakePageForItem(path, item),
                            Desc = item.Description,
                            Level = item.Level.ToString(),
                            Effects = string.Join("<br />", new string[] { 
								item.Effect, 
								item.Effect2, 
								item.Effect3 }.Where(effect => effect != FileFormats.strdefbin.None)),
                        }).OrderBy(q => ushort.Parse(q.Level)).ToArray();

                    HTMLTable<MountDisplay> contents = new HTMLTable<MountDisplay>(pranitems);
                    contents.ClassName = "clothed";
                    MakePage(path, pranpagename, contents.Render(FileFormats.translation.Translate));

                    pranitempane.AddMenuItem(pranpagename, AddSpacesBeforeCapitals(x.ToString()));
                });


            DijitMenuPane otheritempane = new DijitMenuPane( FileFormats.translation.Translate("Other Items"), "otheritems");
            itemspane.Add(otheritempane);

            EnumUtil.GetValues<JitOpener.FileFormats.ItemType>().
                Where(x => FileFormats.GetSuperType(x) == FileFormats.SuperType.Misc_Item).
                ToList().ForEach(x =>
                {

                    otheritempane.AddMenuItem(MakeSlug(x), AddSpacesBeforeCapitals(x.ToString()));
                });





            DijitMenuPane craftpane = new DijitMenuPane( FileFormats.translation.Translate("Crafting"), "crafting");
            menu.Add(craftpane);

            EnumUtil.GetValues<FileFormats.Profession>().
                    Where(prof => (((int)prof) % 2 == 1)).
                    ToList().ForEach(prof =>
            {
                string pagename = AddSpacesBeforeCapitals(FileFormats.translation.Translate(prof.ToString() + "Recipes"));
                craftpane.AddMenuItem(FileFormats.translation.Translate(MakeSlug(prof) + "-recipes"), pagename);
            });
            craftpane.AddMenuItem( FileFormats.translation.Translate("other-recipes"),  FileFormats.translation.Translate("Other Recipes"));



            DijitMenuPane titlepane = new DijitMenuPane(FileFormats.translation.Translate("Titles"), "titles");
            menu.Add(titlepane);

            var titles = Enumerable.Range(0, FileFormats.titlebin.infos.Length).
                Where(x => FileFormats.titlebin.infos[x].titles[0].name[0] != 0 && FileFormats.titlebin.infos[x].titles[0].Name != "0").
                Select(x => new { idx = x, title = FileFormats.titlebin.infos[x] });

            ArrayUtils.ForEach(titles, x => titlepane.AddMenuItem( MakeTitlePage(path, x.idx), x.title.titles[0].Name));


            KeyValuePair<int, int>[] questRanges = { 
                                                       new KeyValuePair<int,int>(0,0),
                                                       new KeyValuePair<int,int>(1,10),
                                                       new KeyValuePair<int,int>(11,20),
                                                       new KeyValuePair<int,int>(21,30),
                                                       new KeyValuePair<int,int>(31,40),
                                                       new KeyValuePair<int,int>(41,45),
                                                       new KeyValuePair<int,int>(46,50),
                                                       new KeyValuePair<int,int>(51,55),
                                                       new KeyValuePair<int,int>(56,60),
                                                       new KeyValuePair<int,int>(61,65),
                                                       new KeyValuePair<int,int>(66,70),
                                                       new KeyValuePair<int,int>(71,75),
                                                       new KeyValuePair<int,int>(76,80),
                                                       new KeyValuePair<int,int>(81,85),
                                                       new KeyValuePair<int,int>(86,90),
                                                       new KeyValuePair<int,int>(91,95),
                                                   };

            DijitMenuPane questpane = new DijitMenuPane(FileFormats.translation.Translate("Quests"), "quests");
            menu.Add(questpane);

            questpane.AddMenuItem(FileFormats.translation.Translate("quests-by-series"), FileFormats.translation.Translate("Quests by Series"));

            List<QuestSeriesDisplay> serieses = new List<QuestSeriesDisplay>();

            FileFormats.questbin.quests.
                    Where(quest => quest.Name.Length != 0).
                    Distinct(new BySeries()).ToList().ForEach(quest =>
                    {

                        string slug = MakeQuestSeriesSlug(quest);


                        QuestDisplay[] quests = FileFormats.questbin.quests.
                            Where(q => q.Name.Length != 0).
                            Where(q => q.SeriesStart.Name == quest.SeriesStart.Name).
                            Select(q => new QuestDisplay()
                            {
                                Name = GetQuestLink(q),
                                Level = q.StartLevel.ToString(),
                                Prerequisite = q.PrerequisiteMission.Name,
                                SeriesNumber = q.NumberInSeries.ToString(),
                                StartPoint = q.StartNPCName,
                                EXPReward = q.EXP.ToString(),
                                GoldReward = q.Gold.ToString(),
                                //ItemReward = string.Join(" ", quest.Items.
                                //    Where(item => item.ID != 0).
                                //    Select(item => SaveImageOfItem(path, item)))
                            }).
                            OrderBy(q => ushort.Parse(q.SeriesNumber)).ToArray();

                        if (quests.Length == 1)
                        {
                            return;
                        }

                        serieses.Add(new QuestSeriesDisplay()
                        {
                            Name = @"<a href=""#" + slug + @""">" + MakeQuestSeriesName(quest),
                            Level = quest.SeriesStart.StartLevel.ToString(),
                            StartPoint = quest.SeriesStart.StartNPCName
                        });

                        //questseriespane.AddMenuItem(slug, MakeQuestSeriesName(quest));

                        HTMLTable<QuestDisplay> questDisp = new HTMLTable<QuestDisplay>(quests);
                        questDisp.ClassName = "clothed";

                        string page = slug;
                        string table = questDisp.Render(FileFormats.translation.Translate);
                        MakePage(path, page, table);
                    });


            serieses.Sort((x, y) => uint.Parse(x.Level).CompareTo(uint.Parse(y.Level)));
            HTMLTable<QuestSeriesDisplay> seriesDisp = new HTMLTable<QuestSeriesDisplay>(serieses.ToArray());
            seriesDisp.ClassName = "clothed";

            MakePage(path, FileFormats.translation.Translate("quests-by-series"), seriesDisp.Render(FileFormats.translation.Translate));

            questRanges.ToList().ForEach(x =>
            {
                string range = x.Key + "-" + x.Value;
                string name = "quests-" + range;
                string propername = "Levels " + name;
                if (x.Key == 0)
                {
                    name = FileFormats.translation.Translate("other");
                    propername = FileFormats.translation.Translate("Other");
                }

                questpane.AddMenuItem(FileFormats.translation.Translate(name), FileFormats.translation.Translate(propername));

                QuestDisplay[] quests = FileFormats.questbin.quests.
                    Where(quest => quest.Name.Length != 0).
                    Where(quest => quest.level >= x.Key && quest.level <= x.Value).
                    Select(quest => new QuestDisplay()
                    {
                        Name = MakeQuestPage(path, quest),
                        Level = quest.StartLevel.ToString(),
                        Prerequisite = quest.PrerequisiteMission.Name,
                        SeriesNumber = quest.NumberInSeries.ToString(),
                        StartPoint = quest.StartNPCName,
                        EXPReward = quest.EXP.ToString(),
                        GoldReward = quest.Gold.ToString(),
                        //ItemReward = string.Join(" ", quest.Items.
                        //    Where(item => item.ID != 0).
                        //    Select(item => SaveImageOfItem(path, item)))
                    }).
                    OrderBy(quest => ushort.Parse(quest.Level)).ToArray();

                HTMLTable<QuestDisplay> questDisp = new HTMLTable<QuestDisplay>(quests);
                questDisp.ClassName = "clothed";

                string page = name;
                string table = questDisp.Render(FileFormats.translation.Translate);
                MakePage(path, page, table);
            });


            DijitMenuPane npcpane = new DijitMenuPane(FileFormats.translation.Translate("NPCs"), "npcs");
            menu.Add(npcpane);

            if (FileFormats.npcpos2bin != null && FileFormats.npcpos2bin.npcs != null)
            {
                AddNPCsFrom(path, npcpane, FileFormats.npcposbin, FileFormats.npcpos2bin);
            }
            else
            {
                AddNPCsFrom(path, npcpane, FileFormats.npcposbin);
            }


            DijitMenuPane mobpane = new DijitMenuPane(FileFormats.translation.Translate("Monsters"), "monsters");
            menu.Add(mobpane);

            FileFormats.mapbin.Maps.ToList().ForEach(x =>
            {
                string name = FileFormats.translation.Translate(slugger.Replace(x.name.Name + " " + x.id, "-").ToLower() + "-mobs");

                if (npcNames.Contains(name))
                {
                    return;
                }

                NPCDisplay[] npcs = FileFormats.mobposbin.entries.
                    Where(npc => npc.positions[0].x > x.range.xBottomLeft &&
                        npc.positions[0].x < x.range.xTopRight &&
                        npc.positions[0].y > x.range.yBottomLeft &&
                        npc.positions[0].y < x.range.yTopRight).
                    Select(npc => new NPCDisplay()
                {
                    Name = MakeNPCLink(path, x, npc.positions, npc.Name),
                    Pos = npc.Pos.Replace(",0 0", "")
                }).OrderBy(npc => npc.Name).ToArray();

                if (npcs.Length == 0)
                {
                    return;
                }

                mobpane.AddMenuItem(name, x.name.Name);

                HTMLTable<NPCDisplay> npcDisp = new HTMLTable<NPCDisplay>(npcs);
                npcDisp.ClassName = "clothed";

                string page = name;
                string table = npcDisp.Render(FileFormats.translation.Translate);
                MakePage(path, page, table);
            });


            DijitMenuPane objpane = new DijitMenuPane(FileFormats.translation.Translate("Objects"), "objects");
            menu.Add(objpane);

            FileFormats.mapbin.Maps.ToList().ForEach(x =>
            {
                string name = slugger.Replace(x.name.Name + " " + x.id, "-").ToLower() + "-objects";

                NPCDisplay[] npcs = FileFormats.objposbin.objects.
                    Where(npc => npc.positions[0].x > x.range.xBottomLeft &&
                        npc.positions[0].x < x.range.xTopRight &&
                        npc.positions[0].y > x.range.yBottomLeft &&
                        npc.positions[0].y < x.range.yTopRight).
                    Select(npc => new NPCDisplay()
                    {
                        Name = MakeNPCLink(path, x, npc.positions, npc.Name),
                        Pos = npc.Pos.Replace(",0 0", "")
                    }).OrderBy(npc => npc.Name).ToArray();

                if (npcs.Length == 0)
                {
                    return;
                }

                objpane.AddMenuItem(FileFormats.translation.Translate(name), FileFormats.translation.Translate(x.name.Name));

                HTMLTable<NPCDisplay> npcDisp = new HTMLTable<NPCDisplay>(npcs);
                npcDisp.ClassName = "clothed";

                string page = name;
                string table = npcDisp.Render(FileFormats.translation.Translate);
                MakePage(path, page, table);
            });


            DijitMenuPane skillpane = new DijitMenuPane(FileFormats.translation.Translate("Skills"), "skills");
            menu.Add(skillpane);

            // = testing =========================================================
            //skillpane.AddMenuItem("skill-page", "skill listing");
            //HTMLTable<SkillDisplay> skilldisp = new HTMLTable<SkillDisplay>(
            //    FileFormats.skilldatabin.skills
            //    .Where(x => x.Name.Length != 0)
            //    .Select(x => new SkillDisplay() {
            //        Icon = SaveImageOfSkill(path, x),
            //        Name = x.Name,
            //        Desc = x.Type,
            //        Effects = x.Effect,
            //        Level = x.skillLevel.ToString(),
            //        LevelReq = x.levelRequired.ToString(),
            //        Mystery2 = x.mystery2.ToString(),
            //        Mystery3 = x.mystery3.ToString(),
            //        Mystery4 = x.mystery4.ToString(),
            //        SkilProf = x.skillprofession.ToString(),
            //        Mystery6 = x.mystery6.ToString()

            //    }).ToArray()
            //    );
            //skilldisp.ClassName = "clothed";
            //MakePage(path, "skill-page", skilldisp.Render(FileFormats.translation.Translate));
            // = testing =========================================================

            //FileFormats.skilldatabin.skills

            EnumUtil.GetValues<FileFormats.SkillProfession>()
                .ToList().ForEach(prof => {
                    string slug = FileFormats.translation.Translate(MakeSlug(prof) + "-skills");
                    string page = FileFormats.translation.Translate(AddSpacesBeforeCapitals(prof.ToString()) + " Skills");
                    skillpane.AddMenuItem(slug, page);

                    Dictionary<string, List<FileFormats.Skill>> dict = new Dictionary<string, List<FileFormats.Skill>>();

                    FileFormats.skilldatabin.skills
                       .Where(x => x.Name.Length != 0)
                       .Where(x => x.skillprofession == prof)
                       .ToList()
                       .ForEach(x => {
                           if (!dict.ContainsKey(MakeSkillSlug(x)))
                           {
                               dict[MakeSkillSlug(x)] = new List<FileFormats.Skill>();
                           }
                           dict[MakeSkillSlug(x)].Add(x);
                       });

                    HTMLTable<SkillOutLine> skillOutLine = new HTMLTable<SkillOutLine>(
                        dict
                        .Select(x => new SkillOutLine()
                        {
                            Icon = SaveImageOfSkill(path, x.Value[0]),
                            Name = MakeSkillPage(path, x),
                            MinimumLevel = x.Value.Min(skill => skill.levelRequired).ToString(),
                            MinFacionRequired = x.Value.Min(skill=>skill.facionRequired).ToString(),
                            MaxFacionRequired = x.Value.Max(skill=>skill.facionRequired).ToString()
                        })
                        .ToArray());
                    skillOutLine.ClassName = "clothed";

                    MakePage(path, slug, skillOutLine.Render(FileFormats.translation.Translate));

                });
                

            EnumUtil.GetValues<FileFormats.Profession>().
                    Where(prof => (((int)prof) % 2 == 1)).
                    ToList().ForEach(prof =>
                    {
                        craftpane.AddMenuItem(FileFormats.translation.Translate(MakeSlug(prof) + "-anvil"), FileFormats.translation.Translate(AddSpacesBeforeCapitals(prof.ToString()) + " Anvil"));
                    });
            craftpane.AddMenuItem(FileFormats.translation.Translate("other-anvil"), FileFormats.translation.Translate("Other Anvil"));

            DijitMenuPane otherPane = new DijitMenuPane(FileFormats.translation.Translate("Misc Info"), "miscinfo");
            menu.Add(otherPane);

            otherPane.AddMenuItem("explist", "EXP Table");
            otherPane.AddMenuItem("pranexplist", "Pran EXP Table");

            template = template
                .Replace("'data/'", "'" + Program.destPath + "/'")
                .Replace("INDEX_HTML", "index" + Program.indexsuffix + ".html")
                .Replace("SIMPLE_PHP", "simple" + Program.indexsuffix + ".php")
                .Replace("COOL_PAGE_TITLE", Program.COOL_PAGE_TITLE)
                .Replace("THE TITLE", Program.title)
                .Replace("<!-- MENU -->", menu.Render(RenderType.Dijit));

            simple = simple
                .Replace("'data/'", "'" + Program.destPath + "/'")
                .Replace("INDEX_HTML", "index" + Program.indexsuffix + ".html")
                .Replace("SIMPLE_PHP", "simple" + Program.indexsuffix + ".php")
                .Replace("SIMPLE_PAGE_TITLE", Program.SIMPLE_PAGE_TITLE)
                .Replace("THE TITLE", Program.title)
                .Replace("<!-- MENU -->", menu.Render(RenderType.Table));

            MakeIndex(path, template, "");

            MakeSimpleIndex(path, simple);

            {
                EnumUtil.GetValues<FileFormats.Profession>().
                    Where(prof => (((int)prof) % 2 == 1)).
                    ToList().ForEach(prof =>
                {
                    Recipe[] recipes = FileFormats.recipebin.recs.
                        Where(item => item.CreatedItem.Profession == prof).
                        Where(item => item.itemid != 0).
                        Where(item => item.RecipeItem.Name.Length > 0 && (item.RecipeItem.Name[0] < 256 || Program.allowKorean)).	// Grab only the ones with non-korean names
                        Select(item => new Recipe()
                        {
                            Image = SaveImageOfItem(path, item.RecipeItem),
                            RecipeName = MakePageForItem(path, item.RecipeItem),
                            Product = SaveImageOfItem(path, item.CreatedItem),

                            ProductName = @"<a href=""#" + MakeEquipSlug(item.CreatedItem) + @""">" + item.CreatedItem.Name + "</a>",
                            Level = item.CreatedItem.Level.ToString(),
                            Ingredients = item.Ingredients.
                            Select(x =>
                                x.Value == 0 ?
                                " "
                                :
                                SaveImageOfItem(path, x.Key) + "<br />" + x.Key.Name + " x" + x.Value

                                ).ToArray()

                        }).OrderBy(x => ushort.Parse(x.Level)).ToArray();


                    HTMLTable<Recipe> mtdisp = new HTMLTable<Recipe>(recipes);
                    mtdisp.ClassName = "clothed";

                    string page = FileFormats.translation.Translate(MakeSlug(prof) + "-recipes");
                    string table = mtdisp.Render(FileFormats.translation.Translate);
                    MakePage(path, page, table);



                    MakeItem[] crafts = FileFormats.makeitemsbin.recipes.
                        Where(item => item.CreatedItem.Profession == prof).
                        Where(item => item.CreatedItem.ID != 0).
                        Select(item => new MakeItem()
                        {
                            Product = SaveImageOfItem(path, item.CreatedItem),
                            Cost = item.cost.ToString("N", nfi),
                            Level = item.CreatedItem.Level.ToString(),
                            ProductName = @"<a href=""#" + MakeEquipSlug(item.CreatedItem) + @""">" + item.CreatedItem.Name + "</a>",
                            Ingredients = item.Ingredients.
                            Select(x =>
                                x.Value == 0 ?
                                " "
                                :
                                SaveImageOfItem(path, x.Key) + "<br />" + x.Key.Name + " x" + x.Value

                                ).ToArray()
                        }).OrderBy(x => ushort.Parse(x.Level)).ToArray();

                    HTMLTable<MakeItem> craftdisp = new HTMLTable<MakeItem>(crafts);
                    craftdisp.ClassName = "clothed";

                    page = FileFormats.translation.Translate(MakeSlug(prof) + "-anvil");
                    table = craftdisp.Render(FileFormats.translation.Translate);
                    MakePage(path, page, table);
                });

                {
                    Recipe[] other = FileFormats.recipebin.recs.
                        Where(item => !EnumUtil.GetValues<FileFormats.Profession>().
                            Contains(item.CreatedItem.Profession)).
                        Where(item => item.itemid != 0).
                        Where(item => item.RecipeItem.Name.Length > 0 && (item.RecipeItem.Name[0] < 256 || Program.allowKorean)).	// Grab only the ones with non-korean names
                        Select(item => new Recipe()
                        {
                            Image = SaveImageOfItem(path, item.RecipeItem),
                            RecipeName = MakePageForItem(path, item.RecipeItem),
                            Product = SaveImageOfItem(path, item.CreatedItem),
                            ProductName = MakePageForItem(path, item.CreatedItem),
                            Ingredients = item.Ingredients.
                            Select(x =>
                                x.Value == 0 ?
                                " "
                                :
                                SaveImageOfItem(path, x.Key) + "<br />" + x.Key.Name + " x" + x.Value

                                ).ToArray()

                        }).ToArray();


                    HTMLTable<Recipe> recipedisp = new HTMLTable<Recipe>(other);
                    recipedisp.ClassName = "clothed";

                    string page = FileFormats.translation.Translate("other-recipes");
                    string table = recipedisp.Render(FileFormats.translation.Translate);
                    MakePage(path, page, table);
                }
            }

            {
                MakeItem[] recipes = FileFormats.makeitemsbin.recipes.
                    Where(item => !EnumUtil.GetValues<FileFormats.Profession>().
                        Contains(item.CreatedItem.Profession)).
                    Where(item => item.CreatedItem.ID != 0).
                    Select(item => new MakeItem()
                    {
                        Product = SaveImageOfItem(path, item.CreatedItem),
                        Cost = item.cost.ToString("N", nfi),
                        ProductName = MakePageForItem(path, item.CreatedItem),
                        Ingredients = item.Ingredients.
                        Select(x =>
                            x.Value == 0 ?
                            " "
                            :
                            SaveImageOfItem(path, x.Key) + "<br />" + x.Key.Name + " x" + x.Value

                            ).ToArray()
                    }).ToArray();

                HTMLTable<MakeItem> mtdisp = new HTMLTable<MakeItem>(recipes);
                mtdisp.ClassName = "clothed";

                string page = FileFormats.translation.Translate("other-anvil");
                string table = mtdisp.Render(FileFormats.translation.Translate);
                MakePage(path, page, table);
            }


            EnumUtil.GetValues<JitOpener.FileFormats.SubType>().
                ToList().ForEach(x =>
                {
                    Materials[] crystals = FileFormats.itemlistbin.items.
                        Where(item => item.ItemType == FileFormats.ItemType.Materials
                            && item.SubType == x).
                        Where(item => item.Name[0] < 256|| Program.allowKorean).	// Grab only the ones with non-korean names
                        Select(item => new Materials()
                        {
                            Image = SaveImageOfItem(path, item),
                            Name = MakePageForItem(path, item),
                            Desc = item.Description
                        }).ToArray();

                    HTMLTable<Materials> mtdisp = new HTMLTable<Materials>(crystals);
                    mtdisp.ClassName = "clothed";

                    string page = MakeSlug(x);
                    string table = mtdisp.Render(FileFormats.translation.Translate);
                    MakePage(path, page, table);
                });


            EnumUtil.GetValues<JitOpener.FileFormats.ItemType>().
                Where(x => FileFormats.GetSuperType(x) == FileFormats.SuperType.Reinforce).
                ToList().ForEach(x =>
                {
                    Materials[] crystals = FileFormats.itemlistbin.items.
                        Where(item => item.ItemType == x).
                        Where(item => item.Name[0] < 256|| Program.allowKorean).	// Grab only the ones with non-korean names
                        Select(item => new Materials()
                        {
                            Image = SaveImageOfItem(path, item),
                            Name = MakePageForItem(path, item),
                            Desc = item.Description,
                        }).ToArray();

                    HTMLTable<Materials> mtdisp = new HTMLTable<Materials>(crystals);
                    mtdisp.ClassName = "clothed";

                    string page = MakeSlug(x);
                    string table = mtdisp.Render(FileFormats.translation.Translate);
                    MakePage(path, page, table);
                });

            EnumUtil.GetValues<JitOpener.FileFormats.ItemType>().
                Where(x => FileFormats.GetSuperType(x) == FileFormats.SuperType.Consumables).
                ToList().ForEach(x =>
                {
                    Materials[] crystals = FileFormats.itemlistbin.items.
                        Where(item => item.ItemType == x).
                        Where(item => item.Name[0] < 256|| Program.allowKorean).	// Grab only the ones with non-korean names
                        Select(item => new Materials()
                        {
                            Image = SaveImageOfItem(path, item),
                            Name = MakePageForItem(path, item),
                            Desc = item.Description,
                        }).ToArray();

                    HTMLTable<Materials> mtdisp = new HTMLTable<Materials>(crystals);
                    mtdisp.ClassName = "clothed";

                    string page = MakeSlug(x);
                    string table = mtdisp.Render(FileFormats.translation.Translate);
                    MakePage(path, page, table);
                });


            EnumUtil.GetValues<JitOpener.FileFormats.ItemType>().
                Where(x => FileFormats.GetSuperType(x) == FileFormats.SuperType.Magic_Crystals).
                ToList().ForEach(x =>
                {
                    MagicCrystal[] crystals = FileFormats.itemlistbin.items.
                        Where(item => item.ItemType == x).
                        Where(item => item.Name[0] < 256|| Program.allowKorean).	// Grab only the ones with non-korean names
                        Select(item => new MagicCrystal()
                        {
                            Image = SaveImageOfItem(path, item),
                            Name = MakePageForItem(path, item),
                            Desc = item.Description,
                            Effects = item.Effect
                        }).ToArray();

                    HTMLTable<MagicCrystal> mtdisp = new HTMLTable<MagicCrystal>(crystals);
                    mtdisp.ClassName = "clothed";

                    string page = MakeSlug(x);
                    string table = mtdisp.Render(FileFormats.translation.Translate);
                    MakePage(path, page, table);
                });

            EnumUtil.GetValues<JitOpener.FileFormats.ItemType>().
                Where(x => FileFormats.GetSuperType(x) == FileFormats.SuperType.Misc_Item).
                ToList().ForEach(x =>
                {
                    MagicCrystal[] crystals = FileFormats.itemlistbin.items.
                        Where(item => item.ItemType == x).
                        Where(item => item.Name[0] < 256|| Program.allowKorean).	// Grab only the ones with non-korean names
                        Select(item => new MagicCrystal()
                        {
                            Image = SaveImageOfItem(path, item),
                            Name = MakePageForItem(path, item),
                            Desc = item.Description,
                            Effects = item.Effect
                        }).ToArray();

                    HTMLTable<MagicCrystal> mtdisp = new HTMLTable<MagicCrystal>(crystals);
                    mtdisp.ClassName = "clothed";

                    string page = MakeSlug(x);
                    string table = mtdisp.Render(FileFormats.translation.Translate);
                    MakePage(path, page, table);
                });

            EnumUtil.GetValues<JitOpener.FileFormats.Profession>().ToList().ForEach(prof =>
            {


                if (!equipmentIndex.ContainsKey(prof))
                {
                    return;
                }

                {
                    Dictionary<string, FileFormats.Item> distinct = new Dictionary<string, FileFormats.Item>();
                    equipmentIndex[prof][FileFormats.ItemType.Mount].ToList().ForEach(x =>
                    {

                        if (!distinct.ContainsKey(x.Name))
                        {
                            distinct[x.Name] = x;
                        }
                    });

                    MountDisplay[] mounts = distinct.Values.
                        Where(item => item.Name[0] < 256|| Program.allowKorean).	// Grab only the ones with non-korean names
                        Where(item => item.things[32] != 0).	// Grab only the ones with pics
                        Select(item =>
                        {
                            ushort iconNum = item.things[32];
                            string iconName = Program.destPath + "/images/" + iconNum / 100 + "/" + iconNum + ".png";
                            string localPathname = Path.Combine(path, iconName);
                            string iconTag = @"<img src=""" + iconName + @""">";

                            // Put the icon in images
                            if (!File.Exists(localPathname))
                            {
                                item.Image.Save(localPathname, ImageFormat.Png);
                            }

                            return new MountDisplay()
                            {

                                Icon = iconTag,
                                Name = MakePageForItem(path, item),
                                Desc = item.Description,
                                Level = item.Level.ToString(),
                                Effects = makeEffectString(item)
                            };
                        }).OrderBy(x => short.Parse(x.Level)).ToArray();

                    HTMLTable<MountDisplay> mtdisp = new HTMLTable<MountDisplay>(mounts);
                    mtdisp.ClassName = "clothed";

                    string page = MakeSlug(prof, FileFormats.ItemType.Mount);
                    string table = mtdisp.Render(FileFormats.translation.Translate);
                    MakePage(path, page, table);
                }

                {
                    SetItemDisplay[] display = FileFormats.setitembin.sets.
                        Where(x => x.numitems > 0 && x.SetItems[0].Profession == prof).
                        Where(x => x.SetItems[0].Level != 0).
                        Select(x => new SetItemDisplay()
                        {
                            Name = x.Name,
                            Level = x.SetItems[0].Level.ToString(),
                            ItemIcons = string.Join(" ", x.SetItems.
                                Where(item => item.ID != 0).
                                Where(item => item.things[32] != 0).
                                Select(item => GetEquipLink(item, SaveImageOfItem(path, item)))),
                            Effects = MakeEffectsList(x)

                        }).OrderBy(x => short.Parse(x.Level)).ToArray();
                    HTMLTable<SetItemDisplay> setitemdisp = new HTMLTable<SetItemDisplay>(display);
                    setitemdisp.ClassName = "clothed";

                    string page = MakeSetItemSlug(prof);
                    string table = setitemdisp.Render(FileFormats.translation.Translate);
                    MakePage(path, page, table);
                }

                EnumUtil.GetValues<JitOpener.FileFormats.ItemType>().ToList().ForEach(type =>
                {

                    if (!equipmentIndex[prof].ContainsKey(type))
                    {
                        return;
                    }

                    if (FileFormats.GetSuperType(type) != FileFormats.SuperType.Equipment)
                    {
                        return;
                    }


                    Dictionary<string, FileFormats.Item> distinct = new Dictionary<string, FileFormats.Item>();
                    equipmentIndex[prof][type].ToList().ForEach(x =>
                    {
                        if (!distinct.ContainsKey(x.Name))
                        {
                            distinct[x.Name] = x;
                        }
                    });
                    ItemDisplay[] displays = distinct.Values.
                        Where(item => item.Name[0] < 256|| Program.allowKorean).	// Grab only the ones with non-korean names
                        Where(item => item.things[32] != 0).	// Grab only the ones with pics
                        Select(item =>
                        {
                            string iconTag = SaveImageOfItem(path, item);

                            // Make a page for the equip itself and get a link for it
                            string link = MakePageForItem(path, item);

                            return new ItemDisplay()
                            {
                                Icon = iconTag,
                                Name = link,
                                Level = item.Level.ToString(),
                                Rank = item.Rank.ToString(),
                                PhyAtk = item.PATK.ToString(),
                                MagAtk = item.MATK.ToString(),
                                PhyDef = item.PDEF.ToString(),
                                MagDef = item.MDEF.ToString(),
                                NPCSellPrice = item.SellPrice.ToString("N", nfi),
                                HeroMedals = item.MedalsOfHero.ToString(),
                                Honor = item.HonorPoints.ToString(),
                                Effects = makeEffectString(item)
                            };
                        }).OrderBy(x => short.Parse(x.Level)).ToArray();

                    HTMLTable<ItemDisplay> disp = new HTMLTable<ItemDisplay>(displays);
                    disp.ClassName = "clothed";

                    string page = MakeSlug(prof, type);
                    string table = disp.Render(FileFormats.translation.Translate);
                    MakePage(path, page, table);

                });
            });




            ExpDisplay[] expDisp = Enumerable.Range(1, FileFormats.explistbin.exp.Length)
                .Select(x => 
                    new ExpDisplay()
                    {
                        Level = x.ToString(),
                        Experience = FileFormats.explistbin.exp[x - 1].ToString(),
                        TrabSapp = (FileFormats.explistbin.exp[x - 1] / 172000).ToString()
                    }
                ).ToArray();

            HTMLTable<ExpDisplay> expTable = new HTMLTable<ExpDisplay>(expDisp);
            expTable.ClassName = "clothed";

            MakePage(path, "explist", expTable.Render(FileFormats.translation.Translate));


            ExpDisplay[] pranExpDisp = Enumerable.Range(1, FileFormats.pranexplistbin.exp.Length)
                .Select(x =>
                    new ExpDisplay()
                    {
                        Level = x.ToString(),
                        Experience = FileFormats.explistbin.exp[x - 1].ToString()
                    }
                ).ToArray();

            HTMLTable<ExpDisplay> pranexpTable = new HTMLTable<ExpDisplay>(pranExpDisp);
            pranexpTable.ClassName = "clothed";

            MakePage(path, "pranexplist", pranexpTable.Render(FileFormats.translation.Translate));
        }


        [StructLayout(LayoutKind.Sequential)]
        struct ExpDisplay
        {
            [HeaderName("Level")]
            public string Level;

            [HeaderName("Experience")]
            public string Experience;

            [HeaderName("Traband Sapphires")]
            public string TrabSapp;
        }

        static HashSet<string> npcNames = new HashSet<string>();

        private static void AddNPCsFrom(string path, DijitMenuPane npcpane, params FileFormats.NPCPosBin[] npcSource)
        {

            FileFormats.mapbin.Maps.ToList().ForEach(x =>
            {
                string name = slugger.Replace(x.name.Name + " " + x.id, "-").ToLower() + "-npcs";
                npcNames.Add(name);

                NPCDisplay[] npcs = npcSource.SelectMany(n=>n.npcs).
                    Where(npc => npc.x > x.range.xBottomLeft &&
                        npc.x < x.range.xTopRight &&
                        npc.y > x.range.yBottomLeft &&
                        npc.y < x.range.yTopRight).
                    Select(npc => new NPCDisplay()
                    {
                        Name = MakeNPCLink(path, x, new FileFormats.Position[] {
                            new FileFormats.Position() {
                            x = npc.x, 
                            y = npc.y
                            } 
                        },
                            npc.Name),
                        Pos = npc.x + "," + npc.y
                    }).OrderBy(npc => npc.Name).ToArray();


                if (npcs.Length == 0)
                {
                    return;
                }

                npcpane.AddMenuItem(name, x.name.Name);

                HTMLTable<NPCDisplay> npcDisp = new HTMLTable<NPCDisplay>(npcs);
                npcDisp.ClassName = "clothed";

                string page = name;
                string table = npcDisp.Render(FileFormats.translation.Translate);
                MakePage(path, page, table);
            });
        }

        private static string MakeSkillPage(string path, KeyValuePair<string, List<FileFormats.Skill>> x)
        {
            string page = MakeSkillSlug(x.Value[0]);

            HTMLTable<SkillDisplay> disp = new HTMLTable<SkillDisplay>(
                x.Value
                .Select(skill => new SkillDisplay()
                               {
                                   Icon = SaveImageOfSkill(path, skill),
                                   Name = skill.Name,
                                   Desc = skill.Type,
                                   Facion = skill.facionRequired.ToString(),
                                   CastTime = FileFormats.translation.Translate("{0} seconds", ((double)skill.castTime / (double)1000)),
                                   CoolDownTime = FileFormats.translation.Translate("{0} seconds", ((double)skill.cooldownTime / (double)1000)),
                                   SpellDuration = FileFormats.translation.Translate("{0} seconds", skill.effectExpiry),
                                   InstantEffect = skill.CastInstantEffect,
                                   LingeringEffect = skill.SpellDurationEffect,
                                   PassiveEffect = skill.PassiveEffect,
                                   Level = skill.skillLevel.ToString(),
                                   LevelReq = skill.levelRequired.ToString(),
                                   SkilProf = AddSpacesBeforeCapitals(skill.skillprofession.ToString()),
                                   Cost = skill.learningCost.ToString(),
                                   MP = skill.mpRequired.ToString(),
                               })
                .ToArray());
            disp.ClassName = "clothed";

            string table = disp.Render(FileFormats.translation.Translate);
            MakePage(path, page, table);

            return GetSkillPageLink(x.Value[0]);
        }

        public static string GetSkillPageLink(FileFormats.Skill x)
        {
            string noLv = 余計なスキル表示.Replace(x.Name, "", 100);
            return @"<a href=""#" + MakeSkillSlug(x) + @""">" + noLv + "</a>";
        }

        private static int IndexOf<T>(T[] array, Func<T, bool> predicate)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (predicate(array[i]))
                {
                    return i;
                }
            }
            return -1;
        }

        private static string MakeNPCLink(string path, FileFormats.MapInfo x, FileFormats.Position[] npc, string npcname)
        {
            int mapid = IndexOf(FileFormats.mapbin.Maps, map =>

                npc[0].x > map.range.xBottomLeft &&
                npc[0].x < map.range.xTopRight &&
                npc[0].y > map.range.yBottomLeft &&
                npc[0].y < map.range.yTopRight
                
                ) + 1;
            string pngname = @"Map" + mapid.ToString("00") + ".jit.png";
            string srcdir = Program.sourcePath + @"\UI\";
            string mappic = Path.Combine(path, Program.destPath + "/images", pngname);
            string slug = MakeNPCSlug(npc, npcname);
            string npcblink = Path.Combine(path, Program.destPath + "/images", slug + ".gif");

            string content = "<h1>" + npcname + "</h1>";

            content += "<br />";
            content += "<br />";
            content += "<h2>" + FileFormats.translation.Translate("Location") + ":</h2>";

            int width = 0, height = 0;
            if (mapid >= FileFormats.texturesizes.mapsizes.Length || FileFormats.texturesizes.mapsizes[mapid - 1].width == 0 || FileFormats.texturesizes.mapsizes[mapid - 1].height == 0)
            {
                //content += FileFormats.translation.Translate("Somewhere in ") + x.name.Name;
                using (Image img = Image.FromFile(Path.Combine(srcdir, pngname)))
                {
                    width = img.Width;
                    height = img.Height;
                }
            }
            else
            {
                width = FileFormats.texturesizes.mapsizes[mapid - 1].width;
                height = FileFormats.texturesizes.mapsizes[mapid - 1].height;
            }


            if (!File.Exists(mappic))
            {
                if (File.Exists(Path.Combine(srcdir, pngname)))
                {
                    File.Copy(Path.Combine(srcdir, pngname), mappic);
                }
            }


            using (Bitmap b = new Bitmap(width, height))
            {
                using (Graphics g = Graphics.FromImage(b))
                {
                    g.Clear(Color.Transparent);

                    npc.ToList().ForEach(pt =>
                    {
                        int xpoint = (int)(((double)(pt.x - x.range.xBottomLeft) / (double)(x.range.xTopRight - x.range.xBottomLeft)) * (double)width);
                        int ypoint = height - (int)((double)(pt.y - x.range.yBottomLeft) / (double)(x.range.yTopRight - x.range.yBottomLeft) * (double)height);

                        g.DrawImage(PointPng, xpoint - PointPng.Width / 2, ypoint - PointPng.Height / 2);
                    });

                }
                JitConverter.MakeBlinker(npcblink, b);

                content += @"<table cellspacing=""0px"" cellpadding=""0px""  style=""height:" + height + @"px; width:" + width + @"px; background-image: url(" + Program.destPath + "/images/" + pngname + @"); background-size:cover"">
<tr><td>
<img src=""" + Program.destPath + "/images/" + slug + @".gif"" />
</td></tr>
</table>
";
            }


            QuestDisplay[] disp = FileFormats.questbin.quests.Where(quest =>
                MakeNPCSlug(quest.StartNPC) == slug).
                Select(q => new QuestDisplay()
                {
                    Name = GetQuestLink(q),
                    Level = q.StartLevel.ToString(),
                    Prerequisite = q.PrerequisiteMission.Name,
                    SeriesNumber = q.NumberInSeries.ToString(),
                    StartPoint = q.StartNPCName,
                    EXPReward = q.EXP.ToString(),
                    GoldReward = q.Gold.ToString(),
                }).
                ToArray();

            if (disp.Length != 0)
            {
                content += "<h1>"+FileFormats.translation.Translate("Start NPC of")+":</h1>";
                HTMLTable<QuestDisplay> table = new HTMLTable<QuestDisplay>(disp);
                table.ClassName = "clothed";
                content += table.Render(FileFormats.translation.Translate);
            }


            disp = FileFormats.questbin.quests.Where(quest =>
                MakeNPCSlug(quest.EndNPC) == slug).
                Select(q => new QuestDisplay() {
                    Name = GetQuestLink(q),
                    Level = q.StartLevel.ToString(),
                    Prerequisite = q.PrerequisiteMission.Name,
                    SeriesNumber = q.NumberInSeries.ToString(),
                    StartPoint = q.StartNPCName,
                    EXPReward = q.EXP.ToString(),
                    GoldReward = q.Gold.ToString(),
                }).
                ToArray();

            if (disp.Length != 0)
            {
                content += "<h1>"+FileFormats.translation.Translate("End NPC of")+":</h1>";
                HTMLTable<QuestDisplay> table = new HTMLTable<QuestDisplay>(disp);
                table.ClassName = "clothed";
                content += table.Render(FileFormats.translation.Translate);
            }

            disp = FileFormats.questbin.quests.Where(quest =>
                quest.KillMob.ToList().Exists(m =>
                    m.Key.FakeName.Length !=0 &&
                    MakeNPCSlug(m.Key.positions, m.Key.Name) == slug)).
                Select(q => new QuestDisplay()
                {
                    Name = GetQuestLink(q),
                    Level = q.StartLevel.ToString(),
                    Prerequisite = q.PrerequisiteMission.Name,
                    SeriesNumber = q.NumberInSeries.ToString(),
                    StartPoint = q.StartNPCName,
                    EXPReward = q.EXP.ToString(),
                    GoldReward = q.Gold.ToString(),
                }).
                ToArray();

            if (disp.Length != 0)
            {
                content += "<h1>"+FileFormats.translation.Translate("Killed for Quests")+":</h1>";
                HTMLTable<QuestDisplay> table = new HTMLTable<QuestDisplay>(disp);
                table.ClassName = "clothed";
                content += table.Render(FileFormats.translation.Translate);
            }

            disp = FileFormats.questbin.quests.Where(quest =>
                quest.UseObject.ToList().Exists(m =>
                    m.Key.FakeName.Length != 0 &&
                    MakeNPCSlug(m.Key.positions, m.Key.Name) == slug)).
                Select(q => new QuestDisplay()
                {
                    Name = GetQuestLink(q),
                    Level = q.StartLevel.ToString(),
                    Prerequisite = q.PrerequisiteMission.Name,
                    SeriesNumber = q.NumberInSeries.ToString(),
                    StartPoint = q.StartNPCName,
                    EXPReward = q.EXP.ToString(),
                    GoldReward = q.Gold.ToString(),
                }).
                ToArray();

            if (disp.Length != 0)
            {
                content += "<h1>"+FileFormats.translation.Translate("Used for Quests")+":</h1>";
                HTMLTable<QuestDisplay> table = new HTMLTable<QuestDisplay>(disp);
                table.ClassName = "clothed";
                content += table.Render(FileFormats.translation.Translate);
            }

            MakePage(path, slug, content);

            return MakeNPCLink(npcname, slug);
        }

        private static string MakeNPCLink(FileFormats.NPCPos npc)
        {
            return RemoveDiacritics(MakeNPCLink(npc.Name, MakeNPCSlug(npc)));
        }

        private static string MakeNPCSlug(FileFormats.NPCPos npc)
        {
            return MakeNPCSlug(new FileFormats.Position[]{
                new FileFormats.Position(){ x=npc.x,y=npc.y}
            }, npc.Name);
        }

        private static string MakeNPCLink(string npcname, string slug)
        {
            return @"<a href=""#" + slug + @""">" + npcname + "</a>";
        }

        private static string MakeNPCSlug(FileFormats.Position[] npc, string npcname)
        {
            return RemoveDiacritics("npc-" + slugger.Replace(npcname.ToLower(), "-") + "-" + npc[0].x + "-" + npc[0].y);
        }

        private static string MakeQuestSeriesSlug(FileFormats.Quest quest)
        {
            return RemoveDiacritics(slugger.Replace(MakeQuestSeriesName(quest), "-").ToLower().Trim('-'));
        }

        private static string MakeQuestSeriesName(FileFormats.Quest quest)
        {
            return FileFormats.translation.Translate("{0} Series", quest.SeriesStart.Name);
        }

        private static string MakePageForItem(string path, FileFormats.Item item)
        {
            string equipslug = MakeEquipSlug(item);

            string equipPageContent = MakePageForEquip(path, item);
            string link = GetEquipLink(item, item.Name);
            MakePage(path, equipslug, equipPageContent);
            return link;
        }

        private static string makeEffectString(FileFormats.Item item)
        {
            return string.Join("<br />", new string[] { 
								item.Effect, 
								item.Effect2, 
								item.Effect3 }.Where(x => x != FileFormats.strdefbin.None)) + " ";
        }

        private static string MakeEffectsList(FileFormats.SetEffect x)
        {
            return string.Join("<br />", x.SetEffects(GetSkillPageLink).Select(effect =>

                                            FileFormats.translation.Translate("Set Effect")+" (" + effect.Key + ")" +
                                            string.Join("<br />", effect.Value.Split(',')) + "<br />"));
        }

        private static string GetEquipLink(FileFormats.Item item, string text)
        {
            return @"<a href=""#" + MakeEquipSlug(item) + @""">" + text + "</a>";
        }


        private static string MakeSetItemSlug(FileFormats.Profession prof)
        {
            return RemoveDiacritics(MakeSlug(prof) + FileFormats.translation.Translate("-equipment-sets"));
        }


        public static string MakeEquipSlug(FileFormats.Item item)
        {
            string equipslug = slugger.Replace(item.Name.ToLower(), "-").Trim('-');

            if (EnumUtil.GetValues<FileFormats.Profession>().Contains(item.Profession))
            {
                string profslug = slugger.Replace(FileFormats.translation.Translate(item.Profession.ToString()).ToLower(), "-");

                if (!equipslug.StartsWith(profslug))
                {
                    equipslug = profslug + "-" + equipslug;
                }
            }

            if (item.ItemType == FileFormats.ItemType.Mount_Stones ||
                item.SuperType == FileFormats.SuperType.Magic_Crystals
                )
            {
                equipslug += "-" + slugger.Replace(item.Effect.ToLower(), "-");
            }

            return RemoveDiacritics(equipslug.Trim('-'));
        }

        private static string SaveImageOfItem(string path, FileFormats.Item item)
        {

            ushort iconNum = item.things[32];
            string iconName = Program.destPath + "/images/" + iconNum/100 + "/" + iconNum + ".png";
            string localPathname = Path.Combine(path, iconName);
            string iconTag = @"<img src=""" + iconName + @""" alt=""" + item.Name + @""">";

            if (!Directory.Exists(Path.GetDirectoryName(localPathname)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(localPathname));
            }

            // Put the icon in images
            if (!File.Exists(localPathname))
            {
                item.Image.Save(localPathname, ImageFormat.Png);
            }
            return iconTag;
        }

        private static string MakePageName(JitOpener.FileFormats.Profession prof, JitOpener.FileFormats.ItemType type)
        {
            return MakeSlug(prof, type) + ".html";
        }

        private static string MakeSlug(JitOpener.FileFormats.Profession prof, JitOpener.FileFormats.ItemType type)
        {
            return RemoveDiacritics(slugger.Replace((FileFormats.translation.Translate(prof.ToString()).ToLower() + "-" + type.ToString().ToLower()).Trim('-'), "-"));
        }

        private static string MakeSlug(Enum type)
        {
            return RemoveDiacritics(slugger.Replace(AddSpacesBeforeCapitals(FileFormats.translation.Translate(type.ToString())).ToLower(), "-")).Trim('-');
        }

        private static string MakeSlug(string name)
        {
            return RemoveDiacritics(slugger.Replace(AddSpacesBeforeCapitals(name).ToLower(), "-")).Trim('-');
        }

        struct MakeItem
        {

            [HeaderName("Product")]
            public string Product;

            [HeaderName("Product Name")]
            public string ProductName;

            [HeaderName("Product Level")]
            public string Level;

            [HeaderName("Cost")]
            public string Cost;

            [HeaderName("Ingredient")]
            [MultiColumn(12)]
            public string[] Ingredients;
        }

        struct Recipe
        {
            [HeaderName("Image")]
            [ColumnSize(55)]
            public string Image;

            [HeaderName("Recipe Name")]
            public string RecipeName;

            [HeaderName("Product")]
            public string Product;

            [HeaderName("Product Name")]
            public string ProductName;

            [HeaderName("Product Level")]
            public string Level;

            [HeaderName("Ingredient")]
            [MultiColumn(12)]
            public string[] Ingredients;
        }


        struct Materials
        {
            [HeaderName("Image")]
            [ColumnSize(55)]
            public string Image;

            [HeaderName("Name")]
            public string Name;

            [HeaderName("Description")]
            public string Desc;

        }

        struct MagicCrystal
        {
            [HeaderName("Image")]
            [ColumnSize(55)]
            public string Image;

            [HeaderName("Name")]
            public string Name;

            [HeaderName("Description")]
            public string Desc;

            [HeaderName("Effects")]
            public string Effects;
        }

        struct ReinforcePossibilitiesWeap
        {

            [HeaderName("Reinforce Level")]
            public string Level;

            [HeaderName("Success Rate")]
            public string Possibility;

            [HeaderName("PATK")]
            public string PATK;

            [HeaderName("MATK")]
            public string MATK;
        }

        struct ReinforcePossibilitiesArmor
        {

            [HeaderName("Reinforce Level")]
            public string Level;

            [HeaderName("Success Rate")]
            public string Possibility;

            [HeaderName("PDEF")]
            public string PDEF;

            [HeaderName("MDEF")]
            public string MDEF;

            [HeaderName("Additional Damage Reduction")]
            public string DmgRed;

            [HeaderName("Additional HP/MP")]
            public string AddHPMP;
        }

        struct Ingredients
        {
            [HeaderName("Image")]
            [ColumnSize(55)]
            public string Image;

            [HeaderName("Ingredient Name")]
            public string Name;

            [HeaderName("Quantity")]
            public string Quantity;
        }


        public struct GearCoreUpgrade
        {
            [HeaderName("Upgradable")]
            [ColumnSize(55)]
            public string UpgradableImage;

            [HeaderName("Upgradable Name")]
            public string UpgradableName;

            [HeaderName("Upgraded")]
            [ColumnSize(55)]
            public string UpgradedImage;

            [HeaderName("Upgraded Name")]
            public string UpgradedName;
        }

        private static string MakePageForEquip(string path, JitOpener.FileFormats.Item item)
        {

            string equipPageContent = SaveImageOfItem(path, item) + "<h1>" + item.Name + "</h1>";
            equipPageContent += "<h2>" + item.Description + "</h2><br />";

            if (item.Name.Contains("Augmented"))
            {
                int a = 1;
            }

            if (item.SuperType == FileFormats.SuperType.Equipment)
            {
                equipPageContent += string.Join("<br />", new string[] { 
				"<b>" + item.Attrs + "</b>",
				FileFormats.translation.Translate("Buy Price: ") + item.BuyPrice.ToString( "N",nfi),
				FileFormats.translation.Translate("Sell Price: ") + item.SellPrice.ToString( "N",nfi),
                FileFormats.translation.Translate("Honor Points:") + item.HonorPoints.ToString( "N",nfi),
				FileFormats.translation.Translate("Cost of Upgrade: ") + item.UpgradeAmount.ToString( "N",nfi),
				FileFormats.translation.Translate("Level Required: ") + item.Level,
				FileFormats.translation.Translate("Rank: ") + item.Rank,

				FileFormats.translation.Translate("Physical Attack: ") + item.PATK,
				FileFormats.translation.Translate("Physical Defense: ") + item.PDEF,
				FileFormats.translation.Translate("Magical Attack: ") + item.MATK,
				FileFormats.translation.Translate("Magical Defense: ") + item.MDEF,

			    }) + "<br />";
            }
            else if (item.SuperType == FileFormats.SuperType.Equipment)
            {
                equipPageContent += string.Join("<br />", new string[] { 
				"<b>" + item.Attrs + "</b>",
				FileFormats.translation.Translate("Buy Price: ") + item.BuyPrice.ToString( "N",nfi),
				FileFormats.translation.Translate("Sell Price: ") + item.SellPrice.ToString( "N",nfi),
				FileFormats.translation.Translate("Level Required: ") + item.Level,
				FileFormats.translation.Translate("Rank: ") + item.Rank,
			    }) + "<br />";
            }
            else if (item.ItemType == FileFormats.ItemType.Gear_Core)
            {
                equipPageContent += string.Join("<br />", new string[] { 
				"<b>" + item.Attrs + "</b>"
			    }) + "<br />";

                var entries = FileFormats.gearcorebin.entries.Where(x => x.gcid == item.GearCoreLevel);

                GearCoreUpgrade[] gcu = entries.Select(x => new GearCoreUpgrade()
                {   
                    UpgradableImage = SaveImageOfItem(path, FileFormats.itemlistbin.items[x.sourceItem]),
                    UpgradableName = GetEquipLink(FileFormats.itemlistbin.items[x.sourceItem], FileFormats.itemlistbin.items[x.sourceItem].Name),
                    UpgradedImage = SaveImageOfItem(path, FileFormats.itemlistbin.items[x.destItem]),
                    UpgradedName = GetEquipLink(FileFormats.itemlistbin.items[x.destItem], FileFormats.itemlistbin.items[x.destItem].Name),
                }).ToArray();

                HTMLTable<GearCoreUpgrade> gearcoretable = new HTMLTable<GearCoreUpgrade>(gcu);
                gearcoretable.ClassName = "clothed";
                equipPageContent += gearcoretable.Render(FileFormats.translation.Translate);

            }
            else if (FileFormats.recipes.ContainsKey(item.ID))
            {
                var recipe = FileFormats.recipes[item.ID];

                equipPageContent += string.Join("<br />", new string[] { 
				"<b>" + item.Attrs + "</b>",
				FileFormats.translation.Translate("Creates: ") + GetEquipLink(recipe.CreatedItem, recipe.CreatedItem.Name)
			    }) + "<br />";


                equipPageContent += GetEquipLink(recipe.CreatedItem, SaveImageOfItem(path, recipe.CreatedItem));

                equipPageContent += "<br />";
                equipPageContent += "<br />";

                Ingredients[] ing = recipe.Ingredients.
                    Where(x => x.Key.ID != 0).
                    Select(x => new Ingredients()
                    {
                        Image = SaveImageOfItem(path, x.Key),
                        Name = GetEquipLink(x.Key, x.Key.Name + " " + (x.Key.Attrs.Length == 0 ? "" : "[" + x.Key.Attrs + "]")),
                        Quantity = x.Value.ToString()
                    }).ToArray();

                HTMLTable<Ingredients> disp = new HTMLTable<Ingredients>(ing);
                disp.ClassName = "clothed";
                equipPageContent += disp.Render(FileFormats.translation.Translate);
            }
            else
            {
                equipPageContent += string.Join("<br />", new string[] { 
				"<b>" + item.Attrs + "</b>",
				FileFormats.translation.Translate("Buy Price: ") + item.BuyPrice.ToString( "N",nfi),
				FileFormats.translation.Translate("Sell Price: ") + item.SellPrice.ToString( "N",nfi),
			    }) + "<br />";
            }


            equipPageContent += string.Join("<br />", new string[] { 
								item.Effect, 
								item.Effect2, 
								item.Effect3 }.Where(x => x != FileFormats.strdefbin.None));

            // Set Effect
            if (FileFormats.itemIdToSetEffect.ContainsKey(FileFormats.ItemSetKey(item)))
            {
                FileFormats.SetEffect set = FileFormats.itemIdToSetEffect[FileFormats.ItemSetKey(item)];
                equipPageContent += "<br />";
                equipPageContent += "<br />";

                equipPageContent += FileFormats.translation.Translate("This item is part of a set: {0}", set.Name);
                SetItemDisplayForEquip[] setitems = set.SetItems.Where(x => x.ID != 0).Select(x =>
                    new SetItemDisplayForEquip()
                    {
                        Icon = GetEquipLink(x, SaveImageOfItem(path, x)),
                        Name = GetEquipLink(x, x.Name)
                    }
                    ).ToArray();
                HTMLTable<SetItemDisplayForEquip> table = new HTMLTable<SetItemDisplayForEquip>(setitems);
                table.ClassName = "clothed";
                equipPageContent += table.Render(FileFormats.translation.Translate);

                equipPageContent += "<br />";

                equipPageContent += MakeEffectsList(set);

                equipPageContent += "<br />";
                equipPageContent += "<br />";
            }

            if (item.Reinforceable && item.Bonuses.firstbonus != null)
            {
                if ((int)item.ItemType > 1000)
                {
                    ReinforcePossibilitiesWeap[] poss = Enumerable.Range(1, 15).
                                Select(x => new ReinforcePossibilitiesWeap()
                                {
                                    Level = "+" + x,
                                    Possibility = (item.RL.chance[x - 1] / 10) + "%",
                                    PATK = item.Bonuses.firstbonus[x - 1].ToString(),
                                    MATK = item.Bonuses.secondbonus[x - 1].ToString()
                                }).ToArray();

                    HTMLTable<ReinforcePossibilitiesWeap> disp = new HTMLTable<ReinforcePossibilitiesWeap>(poss);
                    disp.ClassName = "clothed";
                    equipPageContent += disp.Render(FileFormats.translation.Translate);
                }
                else
                {
                    ReinforcePossibilitiesArmor[] poss = Enumerable.Range(1, 15).
                                Select(x => new ReinforcePossibilitiesArmor()
                                {
                                    Level = "+" + x,
                                    Possibility = (item.RL.chance[x - 1] / 10) + "%",
                                    PDEF = item.Bonuses.firstbonus[x - 1].ToString(),
                                    MDEF = item.Bonuses.secondbonus[x - 1].ToString(),
                                    DmgRed = ((double)item.HPDmgBonuses.firstbonus[x - 1] / (double)10).ToString() + "%",
                                    AddHPMP = item.HPDmgBonuses.secondbonus[x - 1].ToString(),
                                }).ToArray();

                    HTMLTable<ReinforcePossibilitiesArmor> disp = new HTMLTable<ReinforcePossibilitiesArmor>(poss);
                    disp.ClassName = "clothed";
                    equipPageContent += disp.Render(FileFormats.translation.Translate);
                }
            }
            else
            {
                if (item.SuperType == FileFormats.SuperType.Equipment)
                {
                    equipPageContent += "<br /><b>"+FileFormats.translation.Translate("Item cannot be reinforced")+"</b>";
                }
            }


            string itemslug = MakeEquipSlug(item);
            if (FileFormats.itemRewardToQuest.ContainsKey(itemslug))
            {
                equipPageContent += "<br />";
                equipPageContent += "<br /> "+FileFormats.translation.Translate("Obtainable from quests:");
                equipPageContent += "<br />";


                QuestDisplay[] quests = FileFormats.itemRewardToQuest[itemslug].
                    Select(q => new QuestDisplay()
                    {
                        Name = GetQuestLink(q),
                        Level = q.StartLevel.ToString(),
                        Prerequisite = q.PrerequisiteMission.Name,
                        SeriesNumber = q.NumberInSeries.ToString(),
                        StartPoint = q.StartNPCName,
                        EXPReward = q.EXP.ToString(),
                        GoldReward = q.Gold.ToString(),
                        //ItemReward = string.Join(" ", quest.Items.
                        //    Where(item => item.ID != 0).
                        //    Select(item => SaveImageOfItem(path, item)))
                    }).
                    OrderBy(q => ushort.Parse(q.SeriesNumber)).ToArray();

                equipPageContent += "<br />";

                HTMLTable<QuestDisplay> questdisp = new HTMLTable<QuestDisplay>(quests);
                questdisp.ClassName = "clothed";
                equipPageContent += questdisp.Render(FileFormats.translation.Translate);
            }


            string recipekey = item.Name.Replace(FileFormats.translation.Translate("Superior "), "");

            if (recipekey != "" && FileFormats.itemNameToRecipes.ContainsKey(recipekey))
            {
                foreach (var recipe in FileFormats.itemNameToRecipes[recipekey])
                {
                    equipPageContent += "<hr />";
                    equipPageContent += "<br /> "+FileFormats.translation.Translate("Recipe:");
                    equipPageContent += "<br />";
                    equipPageContent += SaveImageOfItem(path, recipe.RecipeItem);
                    equipPageContent += "<br />";
                    equipPageContent += recipe.RecipeItem.Name;
                    equipPageContent += "<br />";

                    // Grab the success rate of the recipe
                    var recipeIDItemIDKeys = recipe.Ingredients.Where(x => x.Value != 0).Select(x => recipe.itemid + "_" + x.Key.ID);
                    var successRates = recipeIDItemIDKeys.Where(x => FileFormats.recipeIDItemIDToRecipeRate.ContainsKey(x)).Select(x => FileFormats.recipeIDItemIDToRecipeRate[x]).ToArray();

                    if (successRates.Length != 0)
                    {
                        foreach (var rates in successRates)
                        {
                            var dependentequip = recipe.Ingredients.First(x => x.Key.ID == rates.ingredientid).Key;

                            equipPageContent += FileFormats.translation.Translate("Success depends on reinforcement level of ") + GetEquipLink(dependentequip, dependentequip.Name);
                            equipPageContent += "<br />";
                            equipPageContent += "<table class=\"clothed\">";

                            equipPageContent += "<tr>";
                            equipPageContent += "<th>" + FileFormats.translation.Translate("Reinforcement Level") + "</th>";
                            for (int i = 0; i < 12; i++)
                            {
                                equipPageContent += "<td>+" + i + "</td>";
                            }
                            equipPageContent += "</tr>";

                            equipPageContent += "<tr>";
                            equipPageContent += "<th>" + FileFormats.translation.Translate("Success Chance") + "</th>";
                            for (int i = 0; i < 12; i++)
                            {
                                equipPageContent += "<td>" + ((double)rates.chances[i]/10.0 + rates.Adjustment).ToString() + "%</td>";
                            }
                            equipPageContent += "</tr>";

                            equipPageContent += "</table>";
                        }
                    }
                    

                    Ingredients[] ing = recipe.Ingredients.
                        Where(x => x.Key.ID != 0).
                        Select(x => new Ingredients()
                        {
                            Image = SaveImageOfItem(path, x.Key),
                            Name = GetEquipLink(x.Key, x.Key.Name + " " + (x.Key.Attrs.Length == 0 ? "" : "[" + x.Key.Attrs + "]")),
                            Quantity = x.Value.ToString()
                        }).ToArray();

                    HTMLTable<Ingredients> disp = new HTMLTable<Ingredients>(ing);
                    disp.ClassName = "clothed";
                    equipPageContent += disp.Render(FileFormats.translation.Translate);

                    equipPageContent += "<br />";
                }
            }


            if (FileFormats.itemNameToMakeItem.ContainsKey(recipekey))
            {
                FileFormats.MakeItem recipe = FileFormats.itemNameToMakeItem[recipekey];
                equipPageContent += "<br />";
                equipPageContent += "<br /> "+FileFormats.translation.Translate("Can be made at the anvil with these ingredients:");
                equipPageContent += "<br /> "+FileFormats.translation.Translate("Cost of making: ") + recipe.cost.ToString("N", nfi);
                equipPageContent += "<br /> "+FileFormats.translation.Translate("Chance of getting a Superior: ") + recipe.superiorChance / 10 + "%";
                equipPageContent += "<br />";
                equipPageContent += "<br />";

                Ingredients[] ing = recipe.Ingredients.
                    Where(x => x.Key.ID != 0).
                    Select(x => new Ingredients()
                    {
                        Image = SaveImageOfItem(path, x.Key),
                        Name = GetEquipLink(x.Key, x.Key.Name + " " + (x.Key.Attrs.Length == 0 ? "" : "[" + x.Key.Attrs + "]")),
                        Quantity = x.Value.ToString()
                    }).ToArray();

                HTMLTable<Ingredients> disp = new HTMLTable<Ingredients>(ing);
                disp.ClassName = "clothed";
                equipPageContent += disp.Render(FileFormats.translation.Translate);

                equipPageContent += "<br />";

            }

            if (FileFormats.itemRequiredToQuest.ContainsKey(itemslug))
            {
                equipPageContent += "<br />";
                equipPageContent += "<br />"+FileFormats.translation.Translate("Required for quests:");
                equipPageContent += "<br />";

                QuestDisplay[] quests = FileFormats.itemRequiredToQuest[itemslug].
                    Select(q => new QuestDisplay()
                    {
                        Name = GetQuestLink(q),
                        Level = q.StartLevel.ToString(),
                        Prerequisite = q.PrerequisiteMission.Name,
                        SeriesNumber = q.NumberInSeries.ToString(),
                        StartPoint = q.StartNPCName,
                        EXPReward = q.EXP.ToString(),
                        GoldReward = q.Gold.ToString(),
                        //ItemReward = string.Join(" ", quest.Items.
                        //    Where(item => item.ID != 0).
                        //    Select(item => SaveImageOfItem(path, item)))
                    }).
                    OrderBy(q => ushort.Parse(q.SeriesNumber)).ToArray();

                equipPageContent += "<br />";

                HTMLTable<QuestDisplay> questdisp = new HTMLTable<QuestDisplay>(quests);
                questdisp.ClassName = "clothed";
                equipPageContent += questdisp.Render(FileFormats.translation.Translate);
            }


            if (FileFormats.itemNeededToCraft.ContainsKey(itemslug))
            {
                equipPageContent += "<br />";
                equipPageContent += "<br />"+FileFormats.translation.Translate("Required to craft:");
                equipPageContent += "<br />";

                Dictionary<string, FileFormats.Item> distinctItems = new Dictionary<string, FileFormats.Item>();
                FileFormats.itemNeededToCraft[itemslug].ForEach(x =>
                {
                    distinctItems[MakeEquipSlug(x)] = x;
                });

                Materials[] items = distinctItems.Values.
                    Select(q => new Materials()
                    {
                        Image = SaveImageOfItem(path, q),
                        Name = GetEquipLink(q, q.Name),
                        Desc = q.Description
                    })
                    .ToArray();

                equipPageContent += "<br />";

                HTMLTable<Materials> itemdisp = new HTMLTable<Materials>(items);
                itemdisp.ClassName = "clothed";
                equipPageContent += itemdisp.Render(FileFormats.translation.Translate);
            }

            if (item.ID != 0 && FileFormats.upgradableToGearCore.ContainsKey(item.ID))
            {
                var gcentries = FileFormats.upgradableToGearCore[item.ID];

                foreach (var gcentry in gcentries)
                {
                    equipPageContent += "<br />";
                    equipPageContent += "<br /> " + SaveImageOfItem(path, FileFormats.gcIdToGC[gcentry.gcid]);
                    equipPageContent += "<br /> " + FileFormats.translation.Translate("Can be upgraded with: {0}", GetEquipLink(FileFormats.gcIdToGC[gcentry.gcid], FileFormats.gcIdToGC[gcentry.gcid].Name));
                    equipPageContent += "<br />";
                    equipPageContent += "<br /> " + FileFormats.translation.Translate("Into a: ");
                    equipPageContent += "<br /> " + SaveImageOfItem(path, FileFormats.itemlistbin.items[gcentry.destItem]);
                    equipPageContent += "<br /> " + GetEquipLink(FileFormats.itemlistbin.items[gcentry.destItem], FileFormats.itemlistbin.items[gcentry.destItem].Name);
                    equipPageContent += "<br />";
                }
            }

            if (item.ID != 0 && FileFormats.upgradedToGearCore.ContainsKey(item.ID))
            {
                var gcentries = FileFormats.upgradedToGearCore[item.ID];
                foreach (var gcentry in gcentries)
                {
                    equipPageContent += "<br />";
                    equipPageContent += "<br /> " + SaveImageOfItem(path, FileFormats.gcIdToGC[gcentry.gcid]);
                    equipPageContent += "<br /> " + FileFormats.translation.Translate("Can be obtained with: {0}", GetEquipLink(FileFormats.gcIdToGC[gcentry.gcid], FileFormats.gcIdToGC[gcentry.gcid].Name));
                    equipPageContent += "<br />";
                    equipPageContent += "<br /> " + FileFormats.translation.Translate("By upgrading: ");
                    equipPageContent += "<br /> " + SaveImageOfItem(path, FileFormats.itemlistbin.items[gcentry.sourceItem]);
                    equipPageContent += "<br /> " + GetEquipLink(FileFormats.itemlistbin.items[gcentry.sourceItem], FileFormats.itemlistbin.items[gcentry.sourceItem].Name);
                    equipPageContent += "<br />";
                }
            }

            return equipPageContent;
        }

        private static string BrowserString(string str)
        {
            if (Program.allowKorean)
            {
                return str;
            }

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] > 128)
                {
                    sb.Append("&#" + (int)str[i] + ";");
                }
                else
                {
                    sb.Append(str[i]);
                }
            }
            return sb.ToString();
        }

        public struct TitleInformation
        {
            [HeaderName("Level")]
            public string Level;

            [HeaderName("Description")]
            public string Description;

            [HeaderName("Effect")]
            public string Effect;
        }

        private static string MakeTitlePage(string path, int titleidx)
        {
            var title = FileFormats.titlebin.infos[titleidx];
            var desc = FileFormats.titlebin.descs[titleidx];
            var story = FileFormats.titlebin.stories[titleidx];

            var name = MakeSlug(title.titles[0].Name);

            string content = "";

            content += "<h1>" + title.titles[0].Name + "</h1>";
            content += "<br />";
            content += story.Content;
            content += "<br />";

            TitleInformation[] ti = Enumerable.Range(0, title.titles.Length)
                .Where(x => title.titles[x].Name != "0")
                .Select(x => new { lv = x+1, tt = title.titles[x], dd = desc.descs[x] })
                .Select(x => new TitleInformation() { 
                    Level = x.lv.ToString(),
                    Description = x.dd.Content,
                    Effect = x.tt.Effects
                })
                .ToArray();

            HTMLTable<TitleInformation> table = new HTMLTable<TitleInformation>(ti);
            table.ClassName = "clothed";
            content += table.Render(FileFormats.translation.Translate);

            MakePage(path, name, content);
            return name;
        }

        private static string MakeQuestPage(string path, FileFormats.Quest quest)
        {
            string page = MakeQuestSlug(quest);
            //string contents = quest.Name;
            //contents += "<br />";


            string contents = "";

            contents += @"<table cellpadding=""3"" cellspacing=""3"" style=""width: 800px; border:1px solid #fff; border-collapse:separate;"">
<tr>
<td bgcolor=""#AECCEF"" colspan=""2"" style=""text-align: center; width: 200px;""> Lv." + quest.Level + " " + quest.Name + @"
</td><td bgcolor=""#DAE4F2"" colspan=""2"" style=""text-align: center; width: 600px;"">
" + string.Join(" ", quest.SummaryDialog.dialog.Where(x => x.Content.Length != 0).Select(x => x.Content)) + @"
</td></tr>
<tr>
<td bgcolor=""#AECCEF"" style=""text-align: center; width: 100px;""><b>"+FileFormats.translation.Translate("Start Point")+@"</b>
</td><td bgcolor=""#DAE4F2"" style=""text-align: center; width: 300px;"">
" + (quest.StartNPCName.StartsWith("From") ? quest.StartNPCName : MakeNPCLink(quest.StartNPC)) + @"
</td><td bgcolor=""#AECCEF"" style=""text-align: center; width: 100px;""><b>"+FileFormats.translation.Translate("Prerequisites")+@"</b>
</td><td bgcolor=""#DAE4F2"" style=""text-align: center; width: 300px;"">
" + (quest.PrerequisiteMission.Name.Length == 0 ? "" : GetQuestLink(quest.PrerequisiteMission) + "<br />") + string.Join("<br />", quest.Prequels.Select(x => GetQuestLink(x)).ToArray()) + @"

</td></tr>
<tr>
<td bgcolor=""#AECCEF"" style=""text-align: center; width: 100px;""><b>"+FileFormats.translation.Translate("Finish NPC")+@"</b>
</td><td bgcolor=""#DAE4F2"" style=""text-align: center; width: 300px;"">
" + (quest.EndNPCName.StartsWith("From") ? quest.EndNPCName : MakeNPCLink(quest.EndNPC)) + @"

</td><td bgcolor=""#AECCEF"" style=""text-align: center; width: 100px;""><b>"+FileFormats.translation.Translate("Follow-up")+@"</b>
</td><td bgcolor=""#DAE4F2"" style=""text-align: center; width: 300px;"">
" + string.Join("<br />", FileFormats.questbin.quests.Where(x =>
      x.PrerequisiteMission.Name == quest.Name
      || x.Prequels.ToList().Exists(prequel => prequel.Name == quest.Name)
      ).Select(x => GetQuestLink(x)).ToArray()) + @"

</td></tr>
<tr>
<td bgcolor=""#AECCEF"" style=""text-align: center; width: 100px;""><b>Reward</b>
</td><td bgcolor=""#DAE4F2"" colspan=""3"" style=""text-align: left; width: 700px;"">
" + quest.EXP + FileFormats.translation.Translate(" EXP, ") + quest.Gold + FileFormats.translation.Translate(" Gold, ") + string.Join(", ", quest.Items.Select(x => GetEquipLink(x.Key, x.Key.Name + " x" + x.Value))) + @"

</td></tr>
<tr>
<td bgcolor=""#AECCEF"" style=""text-align: center; width: 100px;""><b>Notes</b>
</td><td bgcolor=""#DAE4F2"" colspan=""3"" style=""text-align: left; width: 700px;"">" + FileFormats.strdefbin.None + @"
</td></tr></table>";

            if (quest.KillMob.Length != 0)
            {
                NPCDisplay[] disp = quest.KillMob
                    .Where(x => x.Key.FakeName.Length != 0)
                    .Select(x => new NPCDisplay()
                    { 
                    Name = "Kill " + MakeNPCLink(x.Key.Name, MakeNPCSlug(x.Key.positions, x.Key.Name)) 
                    + " x" + x.Value
                }).ToArray();

                if (disp.Length != 0)
                {
                    contents += "<br />"+FileFormats.translation.Translate("Kill Monsters:");
                    HTMLTable<NPCDisplay> table = new HTMLTable<NPCDisplay>(disp);
                    table.ClassName = "clothed";
                    contents += table.Render(FileFormats.translation.Translate);
                }

            }

            if (quest.UseObject.Length != 0)
            {
                NPCDisplay[] disp = quest.UseObject
                    .Where(x=>x.Key.FakeName.Length!=0)
                    .Select(x => new NPCDisplay()
                {
                    Name = FileFormats.translation.Translate("Use ") + MakeNPCLink(x.Key.Name, MakeNPCSlug(x.Key.positions, x.Key.Name))
                    + " " + x.Value + FileFormats.translation.Translate(" times")
                }).ToArray();

                if (disp.Length != 0)
                {
                    contents += "<br />"+FileFormats.translation.Translate("Use Objects:");
                    HTMLTable<NPCDisplay> table = new HTMLTable<NPCDisplay>(disp);
                    table.ClassName = "clothed";
                    contents += table.Render(FileFormats.translation.Translate);

                }
            }

            if (quest.ItemsRequired.Length != 0)
            {
                Ingredients[] disp = quest.ItemsRequired.Select(x => new Ingredients()
                {
                    Image = SaveImageOfItem(path, x.Key),
                    Name = GetEquipLink(x.Key, x.Key.Name),
                    Quantity = x.Value.ToString()

                }).ToArray();

                contents += "<br />"+FileFormats.translation.Translate("Collect items:");
                HTMLTable<Ingredients> table = new HTMLTable<Ingredients>(disp);
                table.ClassName = "clothed";
                contents += table.Render(FileFormats.translation.Translate);
            }



            string[] names = { 
                                 "<span style=\"color:#3923D6\">" + (string.IsNullOrEmpty(quest.StartNPC.Name) ? FileFormats.translation.Translate("Incognito") : quest.StartNPC.Name) + "</span>",
                                 "<span style=\"color:#FF4848\">NakanoAzusa</span>",
                                 "<span style=\"color:#4BFE78\">HiiragiKagami</span>"
                             };


            if (quest.startDialog != 0)
            {
                contents += "<br />";
                contents += "<h1>"+FileFormats.translation.Translate("Starting Dialog")+@"</h1>";
                contents += string.Join("<br />", quest.StartDialog.dialog.Where(x => x.Content.Length != 0).Select(x => names[x.a & 3] + ": " + FormatForSubjects(x)).ToArray());

            }

            if (quest.unfinishedDialog != 0)
            {
                contents += "<br />";
                contents += "<h1>"+FileFormats.translation.Translate("Reminder Dialog")+"</h1>";
                contents += string.Join("<br />", quest.MidDialog.dialog.Where(x => x.Content.Length != 0).Select(x => names[x.a & 3] + ": " + FormatForSubjects(x)).ToArray());
            }

            names = new string[] { 
                                 "<span style=\"color:#3923D6\">" + (string.IsNullOrEmpty(quest.EndNPC.Name) ? FileFormats.translation.Translate("Incognito") : quest.EndNPC.Name) + "</span>",
                                 "<span style=\"color:#FF4848\">NakanoAzusa</span>",
                                 "<span style=\"color:#4BFE78\">HiiragiKagami</span>"
                             };

            if (quest.endDialog != 0)
            {
                contents += "<br />";
                contents += "<h1>"+FileFormats.translation.Translate("Completion Dialog")+"</h1>";
                contents += string.Join("<br />", quest.EndDialog.dialog.Where(x => x.Content.Length != 0).Select(x => names[x.a & 3] + ": " + FormatForSubjects(x)).ToArray());
            }

            MakePage(path, page, contents);
            return GetQuestLink(quest);
        }

        static Regex percentS = new Regex("%s");
        private static string FormatForSubjects(FileFormats.Paragraph x)
        {
            string r = x.Content;

            Dictionary<int, string> secondNames = new Dictionary<int, string>();
            secondNames[0] = "MISSINGNO.";
            secondNames[1] = "<span style=\"color:#FF4848\">NakanoAzusa</span>";
            secondNames[2] = "<span style=\"color:#3923D6\">Priest</span>";
            secondNames[3] = "<span style=\"color:#3923D6\">ExplicitDawn</span>";
            secondNames[4] = "<span style=\"color:#FF4848\">Feonir</span>";
            secondNames[5] = "<span style=\"color:#4BFE78\">HiiragiKagami</span>";

            for (int i = 0; i < 4 && r.Contains("%s"); i++)
            {
                int nameToken = (x.b >> (8 * i)) & 0xff;
                r = percentS.Replace(r, secondNames[nameToken], 1);
            }

            return r;
        }

        private static string GetQuestLink(FileFormats.Quest quest)
        {
            string extra = "";
            if (quest.Name.StartsWith(FileFormats.translation.Translate("[Relic]")))
            {
                var id = quest.RemovesItemsIDs[0];
                string name = GetRelicNameForSealedRelicID(id);
                extra = " (" + name + ")";
            }
            if (quest.ClassRequired.Length != 0)
            {
                extra = "(" + string.Join(", ", quest.ClassRequired) + ")";
            }

            return @"<a href=""#" + MakeQuestSlug(quest) + @""">Lv." + quest.level + " " + quest.Name + extra + "</a>";
        }

        static Regex NameOfRelic = new Regex(@": (?<name>[\w]+) -", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static string MakeQuestSlug(FileFormats.Quest quest)
        {
            string extra = "";
            if (quest.Name.StartsWith(FileFormats.translation.Translate("[Relic]")))
            {
                var id = quest.RemovesItemsIDs[0];
                string name = GetRelicNameForSealedRelicID(id);
                extra = "-" + name.ToLower();
            }
            if (quest.ClassRequired.Length != 0)
            {
                extra = "-" + string.Join("-", quest.ClassRequired.Select(x => x.ToString().ToLower())) + "";
            }

            return RemoveDiacritics("quest-" + quest.level + "-" + slugger.Replace(quest.Name.ToLower(), "-") + extra).Trim('-');
        }

        static string RemoveDiacritics(string text)
        {
            if (Program.allowKorean)
            {
                return text;
            }

            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        private static string GetRelicNameForSealedRelicID(short id)
        {
            var idx = FileFormats.SealedRelics.FindIndex(x => x.Key == id);
            FileFormats.Item relic = FileFormats.Relics[idx * 3].Value;
            string name = NameOfRelic.Match(relic.Name).Groups["name"].ToString();
            return name;
        }


        private static void MakeSimpleIndex(string path, string simple)
        {
            using (StreamWriter sw = new StreamWriter(Path.Combine(path, "simple" + Program.indexsuffix + ".php")))
            {
                sw.Write(BrowserString(simple));
            }
        }

        private static void MakeIndex(string path, string template, string content)
        {
            using (StreamWriter sw = new StreamWriter(Path.Combine(path, "index"+Program.indexsuffix+".html")))
            {
                sw.Write(BrowserString(template.Replace("HAHAHAHAHAHAHAHAHAHAHHA", content)));
            }
        }

        private static void MakePage(string path, string page, string content, bool redirect = true)
        {

            if (page == "")
            {
                return;
            }
            string actualpath = Path.Combine(path, Program.destPath + "/" + slugger.Replace(page, "/").Replace("con","ccon") + ".html");

            if (!Directory.Exists(Path.GetDirectoryName(actualpath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(actualpath));
            }

            Console.WriteLine("Writing to: {0}", actualpath);

            using (StreamWriter sw = new StreamWriter(actualpath))
            {
                sw.Write(BrowserString(content));
                sw.Write("\r\n");

                if (redirect)
                {
                    sw.Write(@"

    <script type=""text/javascript"">
        tablecloth();
    </script>

   <div id=""disqus_thread""></div>
    <script type=""text/javascript"">
        /* * * CONFIGURATION VARIABLES: EDIT BEFORE PASTING INTO YOUR WEBPAGE * * */
        var disqus_shortname = 'aikadb'; // required: replace example with your forum shortname
        var disqus_identifier = '" + page + @"';
        var disqus_url = 'http://aikadb.astrobunny.net/simple" + Program.indexsuffix + ".php?" + page + @"';

        /* * * DON'T EDIT BELOW THIS LINE * * */
        (function() {
            var dsq = document.createElement('script'); dsq.type = 'text/javascript'; dsq.async = true;
            dsq.src = '//' + disqus_shortname + '.disqus.com/embed.js';
            (document.getElementsByTagName('head')[0] || document.getElementsByTagName('body')[0]).appendChild(dsq);
        })();
    </script>
    <noscript>Please enable JavaScript to view the <a href=""http://disqus.com/?ref_noscript"">comments powered by Disqus.</a></noscript>
    <a href=""http://disqus.com"" class=""dsq-brlink"">comments powered by <span class=""logo-disqus"">Disqus</span></a>
    

");
                }
            }
        }




        [StructLayout(LayoutKind.Sequential)]
        struct ItemPriceInfo
        {
            [HeaderName("Icon")]
            public string Icon;

            [HeaderName("Item Id")]
            public string ID;

            [HeaderName("Name")]
            public string Name;

            [HeaderName("20% tax price")]
            public string Price;

        }

        public static void MakePriceList()
        {
            var info = FileFormats.itemlistbin.items
                .Where(x => x.things[32] != 0)
                .Where(x => x.Name != string.Empty)
                .OrderBy(x=>x.Name)
                .Select(x => new ItemPriceInfo()
                {
                    Icon = SaveImageOfItem(Program.webpath, x),
                    ID = x.ID.ToString(),
                    Name = x.Name,
                    Price = ((int)((double)x.SellPrice - ((double)x.SellPrice * 0.20))).ToString()
                });

            HTMLTable<ItemPriceInfo> ipo = new HTMLTable<ItemPriceInfo>(info.ToArray());
            ipo.ClassName = "clothed";
            MakePage(Program.webpath, "pricelist.html", ipo.Render(FileFormats.translation.Translate), false);

        }

    }
}
