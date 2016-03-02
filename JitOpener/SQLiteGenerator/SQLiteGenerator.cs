using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BlueBlocksLib.Database;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using BlueBlocksLib.SetUtils;

namespace JitOpener.SQLiteGenerator
{
    struct Profession
    {
        public int num;
        public string name;
    }

    struct ItemType
    {
        public int num;
        public string name;
    }

    struct ItemSuperType
    {
        public int num;
        public string name;
    }

    struct ItemSubType
    {
        public int num;
        public string name;
    }

    struct QuestCommandType
    {
        public int num;
        public string name;
    }

    struct ObjectItem
    {
        public int npcId;
        public int itemId;
    }

    struct Item
    {
        public int id;
        public int iconNum;
        public int iconx0;
        public int iconx1;
        public int icony0;
        public int icony1;
        public string name;
        public string description;
        public int type;
        public int subtype;
        public int supertype;
        public int profession;
        public int buyprice;
        public int sellprice;
        public string rank;
        public int honorpoints;
        public int level;
        public int iconId;
        public int quality;

        public int phyatk;
        public int phydef;
        public int magatk;
        public int magdef;

        public int canReinforce;
        public int canTrade;

        public string effect1;
        public string effect2;
        public string effect3;

        public int reinforceChanceId;
        public int reinforceBonusPrimary;
        public int reinforceBonusHPDmg;

        public int reinforcePrice;

        public int setId;
    }

    struct GearCore
    {
        public int gearCoreItemId;
        public int gearCoreId;
        public int fromItemId;
        public int toItemId;
    }

    struct ReinforcementChance
    {
        public int reinforceChanceId;
        public int level;
        public int chance;
    }

    struct ReinforcementBonus
    {
        public int reinforceBonusId;
        public int level;
        public int bonus1;
        public int bonus2;
    }

    struct Crafting
    {
        public int recipeItemId;
        public int craftingId;
        public int productItemId;
        public int productItemIdSuperior;
        public int productItemQuantity;
        public int superiorChance;
        public int doubleChance;
        public int cost;

        public int recipeDependantItemId;
        public int recipeSuccessRateId;
    }

    struct RecipeSuccessRate
    {
        public int recipeSuccessRateId;
        public int level;
        public int chance;
    }

    struct CraftingIngredients
    {
        public int craftingId;
        public int ingredientItemId;
        public int ingredientItemQuantity;
    }

    struct Skill
    {
        public int skillId;
        public string name;
        public int tier;
        public int classtype;

        public int iconFileNum;
        public int iconx0;
        public int iconx1;
        public int icony0;
        public int icony1;
    }

    struct SkillLevel
    {
        public string name;
        public string desc;
        public int profession;
        public int skillId;
        public int skillLevelId;
        public int skillLevel;
        public int learncost;
        public int manacost;
        public int levelreq;
        public int facionreq;
        public string casteffect;
        public string durationeffect;
        public string passiveeffect;
        public int casttime;
        public int cooldowntime;
        public int duration;
    }

    struct SkillIdentity
    {
        public string name;
        public int profession;
        public int learncost;
        public int manacost;
        public int levelreq;
        public int facionreq;
        public string casteffect;
        public string durationeffect;
        public string passiveeffect;

        public int iconFileNum;
        public int iconx0;
        public int iconx1;
        public int icony0;
        public int icony1;
    }

    struct EquipSet
    {
        public int setId;
        public string name;
        public int profession;
        public int quality;
    }

    struct EquipSetEffect
    {
        public int setId;
        public int number;
        public string effects;
    }

    struct NPC
    {
        public int npcId;
        public int mapId;
        public string name;
        public int type;
    }

    struct NPCPos
    {
        public int npcId;
        public int x;
        public int y;
    }

    struct Map
    {
        public int mapId;
        public string name;
        public string desc;
        public int x0;
        public int x1;
        public int y0;
        public int y1;
    }

    struct Quest
    {
        public int questId;
        public string name;
        public string desc;
        public int level;
        public int serialNo;
        public int seriesId;

        public int exp;
        public int gold;

        public int startDialogId;
        public int remindDialogId;
        public int endDialogId;

        public int startNpcId;
        public int endNpcId;

        public int preconditionCommandId;
        public int requiresCommandId;
        public int rewardsCommandId;
        public int removesCommandId;
        public int choicesCommandId;
        public int miscCommandId;
    }

    struct QuestCommand
    {
        public int questId;
        public int commandId;
        public int type;

        public int exp;
        public int gold;

        public int itemId;

        public int skillId;

        public int level1;
        public int level2;

        public int requiredQuestId;

        public int mobId;

        public int npcId;

        public int quantity;

        public string description;

        public int obtainitem;
    }

    struct QuestDialog
    {
        public int dialogId;
        public int ordinal;
        public string line;
    }

    class SQLiteGenerator
    {
        public static void Import()
        {
            string name = "aika" + Program.indexsuffix + ".db";

            if (File.Exists(name))
            {
                File.Delete(name);
            }

            SQLiteConnection sqlite = new SQLiteConnection(name);
            sqlite.CreateTable<Item>("items");
            sqlite.CreateTable<ReinforcementChance>("reinforcementchance");
            sqlite.CreateTable<ReinforcementBonus>("reinforcementbonus");
            sqlite.CreateTable<Crafting>("crafting");
            sqlite.CreateTable<CraftingIngredients>("craftingIngredients");
            sqlite.CreateTable<Skill>("skills");
            sqlite.CreateTable<SkillLevel>("skillLevels");
            sqlite.CreateTable<EquipSet>("sets");
            sqlite.CreateTable<EquipSetEffect>("seteffects");

            sqlite.CreateTable<NPC>("npcs");
            sqlite.CreateTable<NPCPos>("npcpos");

            sqlite.CreateTable<Map>("maps");

            sqlite.CreateTable<Quest>("quests");
            sqlite.CreateTable<QuestCommand>("questcommands");
            sqlite.CreateTable<QuestDialog>("questdialogs");

            sqlite.CreateTable<Quest>("seriesList");

            sqlite.CreateTable<GearCore>("gearCore");

            sqlite.CreateTable<ObjectItem>("objectItems");

            sqlite.CreateTable<RecipeSuccessRate>("recipeSuccess");

            using (SQLiteTransaction transaction = new SQLiteTransaction(sqlite))
            {
                var npcList = FileFormats.npcposbin.npcs;

                Dictionary<int, Map> mapDict = new Dictionary<int, Map>();

                var maps = FileFormats.mapbin.Maps;
                for (int i = 0; i < maps.Length; i++)
                {
                    var map = maps[i];

                    Map m;
                    m.mapId = i;
                    m.name = map.name.Name;
                    m.desc = map.desc.Desc;
                    m.x0 = map.range.xBottomLeft;
                    m.y0 = map.range.yBottomLeft;
                    m.x1 = map.range.xTopRight;
                    m.y1 = map.range.yTopRight;
                    mapDict[i] = m;

                    sqlite.Insert("maps", m);
                }

                for (int i = 0; i < FileFormats.gearcorebin.entries.Length; i++)
                {
                    var ent = FileFormats.gearcorebin.entries[i];

                    if (!FileFormats.gcIdToGC.ContainsKey(ent.gcid))
                    {
                        continue;
                    }
                    
                    GearCore g;

                    g.gearCoreId = ent.gcid;
                    g.fromItemId = ent.sourceItem;
                    g.toItemId = ent.destItem;
                    g.gearCoreItemId = FileFormats.gcIdToGC[ent.gcid].ID;

                    sqlite.Insert("gearCore", g);
                }

                foreach (var npc in npcList)
                {

                    if (npc.namePos == 0)
                    {
                        continue;
                    }

                    var mapsMatching = mapDict.Where(x =>
                            npc.x > x.Value.x0 &&
                            npc.x < x.Value.x1 &&
                            npc.y > x.Value.y0 &&
                            npc.y < x.Value.y1).ToArray();

                    if (mapsMatching.Length == 0)
                    {
                        continue;
                    }

                    var map = mapsMatching[0];

                    if (npc.x == 0 && npc.y == 0)
                    {
                        continue;
                    }

                    NPC n;
                    n.name = npc.Name;
                    n.npcId = npc.NPCID;
                    n.mapId = map.Key;
                    n.type = 0; // npc
                    sqlite.Insert("npcs", n);


                    NPCPos pos;
                    pos.npcId = npc.NPCID;
                    pos.x = npc.x;
                    pos.y = npc.y;
                    sqlite.Insert("npcpos", pos);
                }

                foreach (var npc in FileFormats.objposbin.objects)
                {
                    if (npc.positions.Length == 0)
                    {
                        continue;
                    }

                    var mapsMatching = mapDict.Where(x =>
                            npc.positions[0].x > x.Value.x0 &&
                            npc.positions[0].x < x.Value.x1 &&
                            npc.positions[0].y > x.Value.y0 &&
                            npc.positions[0].y < x.Value.y1).ToArray();

                    if (mapsMatching.Length == 0)
                    {
                        continue;
                    }

                    var map = mapsMatching[0];

                    NPC n;
                    n.name = npc.Name;
                    n.npcId = npc.NPCID;
                    n.mapId = map.Key;
                    n.type = 1; // object
                    sqlite.Insert("npcs", n);

                    foreach (var position in npc.positions)
                    {
                        if (position.x == 0 && position.y == 0)
                        {
                            continue;
                        }

                        NPCPos pos;
                        pos.npcId = npc.NPCID;
                        pos.x = position.x;
                        pos.y = position.y;
                        sqlite.Insert("npcpos", pos);
                    }

                    foreach (var item in npc.items)
                    {
                        if (item > 0)
                        {
                            ObjectItem oi;
                            oi.npcId = npc.NPCID;
                            oi.itemId = item;
                            sqlite.Insert("objectItems", oi);
                        }
                    }
                }

                using(StreamWriter sw = new StreamWriter("positionlessMobs.txt"))
                for (int i = 0; i < FileFormats.mobposbin.entries.Length; i++)
                {
                    var npc = FileFormats.mobposbin.entries[i];
                    var npcname = FileFormats.mobnamebin.names[i];


                    if (npcname.Name.ToLower().Contains("asmodius"))
                    {
                        int a = 1;
                    }

                    if (npc.Name.Length == 0)
                    {
                        sw.WriteLine("{0} {1}", i, npcname.Name);
                        continue;
                    }

                    var mapsMatching = mapDict.Where(x =>
                            npc.positions[0].x > x.Value.x0 &&
                            npc.positions[0].x < x.Value.x1 &&
                            npc.positions[0].y > x.Value.y0 &&
                            npc.positions[0].y < x.Value.y1).ToArray();

                    if (mapsMatching.Length == 0)
                    {
                        continue;
                    }

                    var map = mapsMatching[0];

                    NPC n;
                    n.name = npcname.Name;
                    n.npcId = npc.NPCID;
                    n.mapId = map.Key;
                    n.type = 2; // mob
                    sqlite.Insert("npcs", n);

                    foreach (var position in npc.positions)
                    {
                        if (position.x == 0 && position.y == 0)
                        {
                            continue;
                        }

                        NPCPos pos;
                        pos.npcId = npc.NPCID;
                        pos.x = position.x;
                        pos.y = position.y;
                        sqlite.Insert("npcpos", pos);
                    }

                }

                Dictionary<int, int> itemIdToSetId = new Dictionary<int, int>();

                for (int i = 0; i < FileFormats.setitembin.sets.Length; i++)
                {
                    var set = FileFormats.setitembin.sets[i];

                    if (string.IsNullOrEmpty(set.Name) || set.SetItems.Length == 0)
                    {
                        continue;
                    }

                    EquipSet es;
                    es.setId = i;
                    es.name = set.Name;
                    es.profession = (int)set.SetItems[0].Profession;
                    es.quality = (int)set.SetItems[0].Quality;

                    var effects = set.SetEffects(skill => skill.Name);

                    foreach (var effect in effects)
                    {
                        EquipSetEffect ese;
                        ese.effects = effect.Value;
                        ese.number = effect.Key;
                        ese.setId = i;
                        sqlite.Insert("seteffects", ese);
                    }

                    foreach (var item in set.SetItems)
                    {
                        itemIdToSetId[item.ID] = i;
                    }

                    sqlite.Insert("sets", es);
                }

                int skillId = 0;
                string skillName = "";
                string skillKey = "";

                for (int i = 0; i < FileFormats.skilldatabin.skills.Length; i++)
                {
                    var skill = FileFormats.skilldatabin.skills[i];

                    if (skill.iconid == 0 || string.IsNullOrEmpty(skill.Name) )
                    {
                        continue;
                    }

                    if (skill.skillLevel > skill.skillLevelCap)
                    {
                        continue;
                    }

                    if (skill.skillprofession > 0 
                        && (int)skill.skillprofession < 90 
                        && i > 6111)
                    {
                        continue;
                    }

                    SkillIdentity si;

                    si.levelreq = 0;
                    si.learncost = 0;
                    si.profession = (int)skill.skillprofession;
                    si.manacost = skill.mpRequired;
                    si.facionreq = skill.facionRequired;
                    si.name = skill.Name;
                    si.casteffect = skill.CastInstantEffect;
                    si.durationeffect = skill.SpellDurationEffect;
                    si.passiveeffect = skill.PassiveEffect;

                    si.iconFileNum = skill.ImagePos.Item1;
                    si.iconx0 = skill.ImagePos.Item2.X;
                    si.iconx1 = skill.ImagePos.Item2.X + skill.ImagePos.Item2.Width;
                    si.icony0 = skill.ImagePos.Item2.Y;
                    si.icony1 = skill.ImagePos.Item2.Y + skill.ImagePos.Item2.Height;

                    var obj = JObject.FromObject(si);

                    si.levelreq = skill.levelRequired;
                    si.learncost = skill.learningCost;

                    if (skillKey == "" || skillKey != obj.ToString())
                    {
                        string cmp = skill.Name;
                        if (si.profession < 60 && si.profession != 0)
                        {
                            cmp = ((i - 1) / 16).ToString();
                        }
                        if (skillName == "" || skillName != cmp)
                        {
                            skillId++;
                            Skill s;

                            s.iconFileNum = si.iconFileNum;
                            s.iconx0 = si.iconx0;
                            s.iconx1 = si.iconx1;
                            s.icony0 = si.icony0;
                            s.icony1 = si.icony1;

                            s.skillId = skillId;
                            s.name = si.name;

                            // manufacture the class type and
                            // use it instead of profession.
                            // i-1 is the index minus null
                            s.classtype = (int)skill.ProfType ;
                            

                            // each tier block has 6 skills of 16 entries each
                            // each profession has 10 tier blocks
                            s.tier = skill.Tier; 

                            sqlite.Insert("skills", s);

                            skillName = skill.Name;
                            if (si.profession < 60)
                            {
                                skillName = ((i - 1) / 16).ToString();
                            }
                        }

                        SkillLevel sl;
                        sl.name = skill.Name;
                        sl.skillLevelId = i;
                        sl.skillLevel = skill.skillLevel;
                        sl.skillId = skillId;
                        sl.profession = (int)skill.skillprofession;

                        sl.learncost = si.learncost;
                        sl.levelreq = si.levelreq;
                        sl.manacost = si.manacost;
                        sl.facionreq = si.facionreq;
                        sl.passiveeffect = si.passiveeffect;
                        sl.casteffect = si.casteffect;
                        sl.durationeffect = si.durationeffect;

                        sl.casttime = skill.castTime;
                        sl.cooldowntime = skill.cooldownTime;
                        sl.duration = skill.effectExpiry;

                        sl.desc = skill.Type;

                        sqlite.Insert("skillLevels", sl);
                        skillKey = obj.ToString();

                    }
                }


                int iCraft = 0;
                for (; iCraft < FileFormats.makeitemsbin.recipes.Length; iCraft++)
                {
                    var m = FileFormats.makeitemsbin.recipes[iCraft];

                    if (m.createditemid == 0)
                    {
                        continue;
                    }

                    Crafting c;
                    c.cost = m.cost;
                    c.craftingId = iCraft;
                    c.productItemId = m.createditemid;
                    c.productItemIdSuperior = m.createditemid;

                    string nonsup = FileFormats.itemlistbin.items[m.createditemid]
                        .Name.Replace("Superior ", "");
                    if (FileFormats.itemNameToIDs.ContainsKey(nonsup))
                    {
                        foreach (var itemId in
                            FileFormats.itemNameToIDs[nonsup])
                        {
                            c.productItemId = itemId;
                            break;
                        }
                    }

                    c.productItemQuantity = m.craftQuantity;
                    c.recipeItemId = 0;
                    c.superiorChance = m.superiorChance / 10;
                    c.doubleChance = m.doubleChance / 10;
                    c.recipeDependantItemId = 0;
                    c.recipeSuccessRateId = 0;

                    for (int j = 0; j < m.ingredientitem.Length; j++)
                    {
                        if (m.ingredientnum[j] == 0)
                        {
                            continue;
                        }

                        CraftingIngredients ing;
                        ing.craftingId = iCraft;
                        ing.ingredientItemId = m.ingredientitem[j];
                        ing.ingredientItemQuantity = m.ingredientnum[j];

                        sqlite.Insert("craftingIngredients", ing);
                    }

                    sqlite.Insert("crafting", c);
                }

                int rsrCount = 1;
                var recipelist = FileFormats.recipes.ToArray();
                foreach (var m in recipelist)
                {

                    Crafting c;
                    c.cost = m.Value.cost;
                    c.craftingId = iCraft;
                    c.productItemId = m.Value.createditemid;
                    c.productItemIdSuperior = m.Value.createditemidSuperior;
                    c.productItemQuantity = m.Value.quantity;
                    c.recipeItemId = m.Value.itemid;
                    c.superiorChance = m.Value.superiorRate / 10;
                    c.doubleChance = m.Value.doubleRate / 10;

                    var recipeIDItemIDKeys = m.Value.Ingredients.Where(x => x.Value != 0).Select(x => m.Value.itemid + "_" + x.Key.ID);
                    var successRates = recipeIDItemIDKeys.Where(x => FileFormats.recipeIDItemIDToRecipeRate.ContainsKey(x)).Select(x => FileFormats.recipeIDItemIDToRecipeRate[x]).ToArray();

                    if (successRates.Length != 0)
                    {
                        var rates = successRates[0];

                        int rsrId = rsrCount++;
                        for (int i = 0; i < 12; i++)
                        {
                            RecipeSuccessRate rsr;
                            rsr.recipeSuccessRateId = rsrId;
                            rsr.level = i + 1;
                            rsr.chance = (int)((double)rates.chances[i] / 10.0 + rates.Adjustment);

                            sqlite.Insert("recipeSuccess", rsr);
                        }

                        c.recipeDependantItemId = rates.ingredientid;
                        c.recipeSuccessRateId = rsrId;
                    }
                    else
                    {
                        c.recipeSuccessRateId = 0;
                        c.recipeDependantItemId = 0;
                    }

                    for (int j = 0; j < m.Value.ingredientitem.Length; j++)
                    {
                        if (m.Value.ingredientnum[j] == 0)
                        {
                            continue;
                        }

                        CraftingIngredients ing;
                        ing.craftingId = iCraft;
                        ing.ingredientItemId = m.Value.ingredientitem[j];
                        ing.ingredientItemQuantity = m.Value.ingredientnum[j];

                        sqlite.Insert("craftingIngredients", ing);
                    }

                    sqlite.Insert("crafting", c);
                    iCraft++;
                }

                HashSet<int> reinforceChanceIds = new HashSet<int>();
                foreach (var chanceSet in FileFormats.reinforceabin.sets.Concat(FileFormats.reinforcewbin.sets))
                {
                    for (int i = 0; i < 12; i++)
                    {
                        ReinforcementChance rc;
                        rc.reinforceChanceId = chanceSet.ID;
                        rc.chance = chanceSet.chance[i] / 10;
                        rc.level = i + 1;
                        sqlite.Insert("reinforcementchance", rc);
                    }

                    reinforceChanceIds.Add(chanceSet.ID);
                }

                IEnumerable<FileFormats.ReinforceBonus> bonuses = new FileFormats.ReinforceBonus[0]
                    .Concat(FileFormats.reinforce2bin.section.SelectMany(x => x.weapons.SelectMany(y => y.bonuses)))
                    .Concat(FileFormats.reinforce2bin.section.SelectMany(x => x.shields.bonuses))
                    .Concat(FileFormats.reinforce2bin.section.SelectMany(x => x.other.SelectMany(y => y.armors.SelectMany(z => z.bonuses))))

                    .Concat(FileFormats.reinforce3bin.sections.SelectMany(x => x.armors.SelectMany(y => y.bonuses)))
                    .Concat(FileFormats.reinforce3bin.sections.SelectMany(x => x.shields.bonuses))

                    ;

                HashSet<int> reinforceBonusIds = new HashSet<int>();
                foreach (var bonus in bonuses)
                {
                    for (int i = 0; i < 12; i++)
                    {
                        ReinforcementBonus rb;
                        rb.reinforceBonusId = bonus.ID;
                        rb.bonus1 = bonus.firstbonus[i];
                        rb.bonus2 = bonus.secondbonus[i];
                        rb.level = i + 1;
                        sqlite.Insert("reinforcementbonus", rb);
                    }

                    reinforceBonusIds.Add(bonus.ID);
                }

                for (int i = 0; i < FileFormats.itemlistbin.items.Length; i++)
                {
                    FileFormats.Item item = FileFormats.itemlistbin.items[i];
                    Item itemData;
                    itemData.id = FileFormats.itemlistbin.items[i].ID;
                    itemData.iconId = item.things[32];
                    itemData.name = item.Name;
                    itemData.level = item.Level;
                    itemData.description = item.Description;
                    itemData.type = (int)item.ItemType;
                    itemData.subtype = (int)item.SubType;
                    itemData.supertype = (int)item.SuperType;
                    itemData.iconNum = item.ImageData.Item1;
                    itemData.iconx0 = item.ImageData.Item2.X;
                    itemData.iconx1 = item.ImageData.Item2.X + item.ImageData.Item2.Width;
                    itemData.icony0 = item.ImageData.Item2.Y;
                    itemData.icony1 = item.ImageData.Item2.Y + item.ImageData.Item2.Height;
                    itemData.profession = (int)item.Profession;
                    itemData.buyprice = item.BuyPrice;
                    itemData.sellprice = item.SellPrice;
                    itemData.rank = item.Rank.ToString();
                    itemData.honorpoints = item.HonorPoints;
                    itemData.quality = (int)item.Quality;

                    itemData.phyatk = item.PATK;
                    itemData.phydef = item.PDEF;
                    itemData.magatk = item.MATK;
                    itemData.magdef = item.MDEF;

                    itemData.canReinforce = item.Reinforceable ? 1 : 0;
                    itemData.canTrade = item.Tradeable ? 1 : 0;

                    itemData.effect1 = item.Effect == "None" ? "" : item.Effect;
                    itemData.effect2 = item.Effect2 == "None" ? "" : item.Effect2;
                    itemData.effect3 = item.Effect3 == "None" ? "" : item.Effect3;

                    if (item.ItemType == FileFormats.ItemType.Body)
                    {
                        int a = 1;
                    }

                    itemData.reinforceChanceId = reinforceChanceIds.Contains(item.RL.ID) ? item.RL.ID : -1;
                    itemData.reinforceBonusPrimary = reinforceBonusIds.Contains(item.Bonuses.ID) ? item.Bonuses.ID : -1;
                    itemData.reinforceBonusHPDmg = reinforceBonusIds.Contains(item.HPDmgBonuses.ID) ? item.HPDmgBonuses.ID : -1;

                    itemData.reinforcePrice = item.RL.price;

                    itemData.setId = -1;
                    if (itemIdToSetId.ContainsKey(itemData.id))
                    {
                        itemData.setId = itemIdToSetId[itemData.id];
                    }


                    // filter
                    if (itemData.iconId != 0
                        && (item.Name.Length > 1)
                        && (item.Name[0] < 256 || Program.allowKorean)
                        && (item.Name[1] < 256 || Program.allowKorean)

                        )
                    {
                        sqlite.Insert("items", itemData);
                    }
                }

                HashSet<int> series = new HashSet<int>();

                for (int i = 0; i < FileFormats.questbin.quests.Length; i++)
                {
                    var quest = FileFormats.questbin.quests[i];

                    if (quest.Name.Length == 0)
                    {
                        continue;
                    }

                    Quest q;
                    q.name = quest.Name;
                    q.questId = quest.id;
                    q.level = quest.Level;
                    q.serialNo = quest.NumberInSeries;
                    q.seriesId = quest.SeriesStart.id;
                    q.desc = string.Join(" ", quest.SummaryDialog.dialog.Select(x => x.Content));

                    q.exp = quest.EXP;
                    q.gold = quest.Gold;
                    
                    q.startNpcId = quest.StartNPC.NPCID;
                    q.endNpcId = quest.EndNPC.NPCID;

                    q.startDialogId = AddDialogs(sqlite, quest.StartDialog, quest.StartNPCName);
                    q.remindDialogId = AddDialogs(sqlite, quest.MidDialog, quest.StartNPCName);
                    q.endDialogId = AddDialogs(sqlite, quest.EndDialog, quest.EndNPCName);

                    q.preconditionCommandId = AddCommands(sqlite, quest.id, quest.preconditions, 0);
                    q.requiresCommandId = AddCommands(sqlite, quest.id, quest.requires, 0);
                    q.rewardsCommandId = AddCommands(sqlite, quest.id, quest.rewards, 1);
                    q.removesCommandId = AddCommands(sqlite, quest.id, quest.removes, 0);

                    var followups = FileFormats.questbin.quests.Where(x =>
                        x.PrerequisiteMission.Name == quest.Name
                        || x.Prequels.ToList().Exists(prequel => prequel.Name == quest.Name));

                    var questFollowups = followups.Select(x => new FileFormats.QuestCommand()
                    {
                        type = FileFormats.CommandType.MissionChoice,
                        amountOrType = x.ID
                    });

                    q.choicesCommandId = AddCommands(sqlite, quest.id, questFollowups.ToArray(), 0);

                    q.miscCommandId = AddCommands(sqlite, quest.id, quest.misc, 0);

                    if (q.seriesId != 0 && q.serialNo == 1 && !series.Contains(q.seriesId))
                    {
                        sqlite.Insert("seriesList", q);
                    }

                    sqlite.Insert("quests", q);
                }
            }

            WriteEnum<FileFormats.ProfessionType, Profession>(sqlite);
            WriteEnum<FileFormats.SkillProfession, Profession>(sqlite);
            WriteEnum<FileFormats.ItemType, ItemType>(sqlite);
            WriteEnum<FileFormats.SuperType, ItemSuperType>(sqlite);
            WriteEnum<FileFormats.SubType, ItemSubType>(sqlite);
            WriteEnum<FileFormats.CommandType, QuestCommandType>(sqlite);


            sqlite.Index("items", "name");
            sqlite.Index("items", "type");
            sqlite.Index("items", "supertype");
            sqlite.Index("items", "id");
            sqlite.Index("items", "setId");
            sqlite.Index("reinforcementchance", "reinforceChanceId");
            sqlite.Index("reinforcementbonus", "reinforceBonusId");

            sqlite.Index("craftingIngredients", "craftingId");
            sqlite.Index("craftingIngredients", "ingredientItemId");

            sqlite.Index("crafting", "craftingId");
            sqlite.Index("crafting", "productItemId");
            sqlite.Index("crafting", "recipeItemId");

            sqlite.Index("skills", "classType");
            sqlite.Index("skills", "skillId");
            sqlite.Index("skills", "name");

            sqlite.Index("skillLevels", "skillId");
            sqlite.Index("skillLevels", "skillLevelId");
            sqlite.Index("skillLevels", "levelreq");

            sqlite.Index("sets", "setId");
            sqlite.Index("sets", "name");
            sqlite.Index("seteffects", "setId");

            sqlite.Index("gearCore", "gearCoreId");
            sqlite.Index("gearCore", "gearCoreItemId");
            sqlite.Index("gearCore", "fromItemId");
            sqlite.Index("gearCore", "toItemId");

            sqlite.Index("objectItems", "npcId");
            sqlite.Index("objectItems", "itemId");

            sqlite.Index("maps", "mapId");
            sqlite.Index("maps", "name");

            sqlite.Index("npcs", "mapId");
            sqlite.Index("npcs", "name");
            sqlite.Index("npcs", "npcId");

            sqlite.Index("npcpos", "npcId");
            sqlite.Index("npcpos", "x");
            sqlite.Index("npcpos", "y");

            sqlite.Index("seriesList", "name");

            sqlite.Index("quests", "questId");
            sqlite.Index("quests", "startNpcId");
            sqlite.Index("quests", "endNpcId");
            sqlite.Index("quests", "name");
            sqlite.Index("quests", "seriesId");

            sqlite.Index("questcommands", "commandId");
            sqlite.Index("questcommands", "itemId");
            sqlite.Index("questcommands", "skillId");
            sqlite.Index("questcommands", "requiredQuestId");
            sqlite.Index("questcommands", "mobId");
            sqlite.Index("questcommands", "npcId");

            sqlite.Index("questdialogs", "dialogId");

            sqlite.Index("recipeSuccess", "recipeSuccessRateId");
        }

        private static void WriteEnum<TEnum, TTable>(SQLiteConnection sqlite) where TTable : struct
        {
            string name = typeof(TEnum).Name.ToLower() + "Enum";
            sqlite.CreateTable<TTable>(name);
            var values = Enum.GetValues(typeof(TEnum));

            Type t = typeof(TTable);
            foreach (var value in values)
            {
                TTable p = new TTable();
                t.GetField("num").SetValueDirect(__makeref(p), (int)value);
                t.GetField("name").SetValueDirect(__makeref(p), FileFormats.translation.Translate(SiteGenerator.AddSpacesBeforeCapitals(value.ToString())));
                sqlite.Insert(name, p);
            }

            sqlite.Index(name, "num");
            sqlite.Index(name, "name");
        }

        static int commandCount = 0;
        private static int AddCommands(SQLiteConnection sqlite, int questId, FileFormats.QuestCommand[] questCommands, int obtainitem)
        {
            commandCount++;
            for (int i = 0; i < questCommands.Length; i++)
            {
                var questCommand = questCommands[i];

                QuestCommand cmd = new QuestCommand();

                cmd.commandId = commandCount;
                cmd.questId = questId;
                cmd.type = (int)questCommand.type;
                cmd.description = questCommand.ToString();
                cmd.obtainitem = obtainitem;

                switch (questCommand.type)
                {
                    case JitOpener.FileFormats.CommandType.AfterMission:
                        cmd.requiredQuestId = FileFormats.questbin.quests[questCommand.amountOrType].id;
                        break;
                    case JitOpener.FileFormats.CommandType.LevelRange:
                        cmd.level1 = questCommand.amountOrType;
                        cmd.level2 = questCommand.amount2;
                        break;
                    case JitOpener.FileFormats.CommandType.Prerequisite:
                        cmd.requiredQuestId = FileFormats.questbin.quests[questCommand.amountOrType].id;
                        break;
                    case JitOpener.FileFormats.CommandType.GuildEXP:
                        break;
                    case JitOpener.FileFormats.CommandType.ChangeClass:
                        break;
                    case JitOpener.FileFormats.CommandType.BattleFieldVictory:
                        break;
                    case JitOpener.FileFormats.CommandType.SkillAcquire:
                        cmd.skillId = questCommand.amountOrType;
                        break;
                    case JitOpener.FileFormats.CommandType.Equip:
                        cmd.itemId = questCommand.amountOrType;
                        break;
                    case JitOpener.FileFormats.CommandType.Class:
                        break;
                    case JitOpener.FileFormats.CommandType.Monster:
                        cmd.quantity = questCommand.amount2;
                        cmd.npcId = FileFormats.mobposbin.entries[questCommand.amountOrType].NPCID;
                        break;
                    case JitOpener.FileFormats.CommandType.Item:
                        cmd.itemId = questCommand.item1;
                        cmd.quantity = questCommand.quantity == 0 ? 1 : questCommand.quantity;
                        break;
                    case JitOpener.FileFormats.CommandType.ItemChoice:
                        cmd.itemId = questCommand.item1;
                        cmd.quantity = 1;
                        break;
                    case JitOpener.FileFormats.CommandType.EXP:
                        cmd.exp = questCommand.amountOrType;
                        break;
                    case JitOpener.FileFormats.CommandType.Gold:
                        cmd.gold = questCommand.amountOrType;
                        break;
                    case JitOpener.FileFormats.CommandType.Pran:
                        break;
                    case JitOpener.FileFormats.CommandType.MissionChoice:
                        cmd.requiredQuestId = FileFormats.questbin.quests[questCommand.amountOrType].id;
                        break;
                    case JitOpener.FileFormats.CommandType.ComeFrom:
                        cmd.requiredQuestId = FileFormats.questbin.quests[questCommand.amountOrType].id;
                        break;
                    case JitOpener.FileFormats.CommandType.Use:
                        cmd.quantity = questCommand.amount2 == 0 ? 1 : questCommand.amount2;
                        cmd.npcId = FileFormats.objposbin.objects[questCommand.amountOrType].NPCID;
                        break;
                    case JitOpener.FileFormats.CommandType.TalkTo:
                        cmd.npcId = FileFormats.npcposbin.npcs[questCommand.amountOrType].NPCID;
                        break;

                    default:
                        continue;
                }

                sqlite.Insert("questCommands", cmd);
            }

            return commandCount;
        }

        static int dialogCount = 0;
        private static int AddDialogs(SQLiteConnection sqlite, FileFormats.Dialog dd, string npc)
        {
            dialogCount++;
            for (int d = 0; d < dd.dialog.Length; d++)
            {
                var dialog = dd.dialog[d];

                if (string.IsNullOrEmpty(dialog.Content))
                {
                    continue;
                }

                QuestDialog qd;
                qd.ordinal = d;
                qd.line = FormatWithSubject(dialog, npc);
                qd.dialogId = dialogCount;

                //sqlite.Insert("questdialogs", qd);
            }
            return dialogCount;
        }

        static Regex percentS = new Regex("%s");
        private static string FormatWithSubject(FileFormats.Paragraph x, string npc)
        {
            string[] names = { 
                                 npc,
                                 "NakanoAzusa",
                                 "HiiragiKagami"
                             };

            Dictionary<int, string> secondNames = new Dictionary<int, string>();
            secondNames[0] = "MISSINGNO.";
            secondNames[1] = "NakanoAzusa";
            secondNames[2] = "Priest";
            secondNames[3] = "ExplicitDawn";
            secondNames[4] = "Feonir";
            secondNames[5] = "HiiragiKagami";

            string r = x.Content;
            for (int i = 0; i < 4 && r.Contains("%s"); i++)
            {
                int nameToken = (x.b >> (8 * i)) & 0xff;
                r = percentS.Replace(r, secondNames[nameToken], 1);
            }

            return names[x.a & 3] + ":" + r;
        }
    }
}
