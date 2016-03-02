﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JitOpener
{
    class TranslationBR
    {
        public static Dictionary<string, string> table = new Dictionary<string, string>();
        static TranslationBR()
        {
            table["Sniper"] = "Atirador";
            table["Priest"] = "Cleriga";
            table["Warrior"] = "Guerreiro";
            table["Crusader"] = "Templaria";
            table["DualGunner"] = "Dual";
            table["NightMagician"] = "FeiticeiroNegro";
            table["Superior "] = "Superior ";
            table["Head"] = "Elmos";
            table["Body"] = "Armaduras";
            table["Glove"] = "Luvas";
            table["Boots"] = "Botas";
            table["Mount"] = "Montarias";
            table["TwoHandSword"] = "EspadaDeDuasMãos";
            table["-equipment-sets"] = "-sets-completos";
            table["Equipment Sets"] = "Sets Completos";
            table["Shield"] = "Escudo";
            table["OneHandSword"] = "UmaEspadaMão";
            table["Ammunition"] = "Munição";
            table["Rifle"] = "Rifles";
            table["Gun"] = "Pistolas";
            table["Dual Gunner"] = "Dual";
            table["Staff"] = "Cajados";
            table["Night Magician"] = "Feiticeiro Negro";
            table["Wand"] = "Cetros";
            table["Accessories"] = "Accessorios";
            table["Ring"] = "Aneis";
            table["Buy Price: "] = "Comprar Preço: ";
            table["Sell Price: "] = "Preço de Venda: ";
            table["Obtainable from quests:"] = "Podem ser obtidos a partir de missões:";
            table["[Relic]"] = "[Relíquia]";
            table["This item is part of a set: "] = "Este artigo é parte de um conjunto: ";
            table["Other"] = "Outro";
            table["Set Effect"] = "Set Effect";
            table["Recipe:"] = "Receita:";
            table["Required for quests:"] = "Necessário para missões:";
            table["Earring"] = "Brincos";
            table["Bracelet"] = "Bracelete";
            table["Necklace"] = "Colar";
            table["accessory-sets"] = "accessorios-completos";
            table["Accessory Sets"] = "Accessorios Completos";
            table["Items"] = "Items";
            table["Magic Crystals"] = "Cristais";
            table["AllEnchant"] = "CristaisGerais";
            table["WeaponEnchant"] = "CristaisDeArma";
            table["ArmorEnchant"] = "CristaisDeArmaduras";
            table["MountEnchant"] = "CristaisDeMontaria";
            table["AccessoryEnchant"] = "CristaisDeAccessorios";
            table["Materials"] = "Materiais";
            table["MonsterDrops"] = "MonsterDrops";
            table["OresAndMetals"] = "Metais";
            table["Leather"] = "Couros";
            table["Cloth"] = "Tecidos";
            table["Minerals"] = "Minerais";
            table["Essence"] = "Essencias";
            table["Fuels"] = "Simples";
            table["Miscellaneous"] = "Outros";
            table["Consumable Items"] = "Items de Consumo";
            table["HpConsumables"] = "ConsumoHp";
            table["MpConsumables"] = "ConsumoMp";
            table["Reinforce Items"] = "Fortificacao";
            table["WeaponReinforce"] = "FortificacaoDeArmas";
            table["ArmorReinforce"] = "FortificacaoDeArmaduras";
            table["WRExtract"] = "ExtratosDeHira";
            table["WRConcentrate"] = "ExtratosEnriquecidoDeHira";
            table["ARExtract"] = "ExtratosDeKaize";
            table["ARConcentrate"] = "ExtratosEnriquecidoDeKaize";
            table["Pran Items"] = "Items da Pran";
            table["PranWing"] = "AsasParaPran";
            table["PranDoll"] = "BonecasParaPran";
            table["PranFood"] = "ComidasParaPran";
            table["PranDress"] = "VestidosParaPran";
            table["PranAccessory"] = "AccessoriosParaPran";
            table["PranHeadgear"] = "ChapeusParaPran";
            table["Other Items"] = "Outros Items";
            table["Relics"] = "Relikias";
            table["MountStones"] = "CristaisDeMontarias";
            table["LegionMissions"] = "MissoesDeGuilda";
            table["Facions"] = "Facions";
            table["FacionOres"] = "JoiasDeFacion";
            table["QuestItems"] = "ItemsDeQuest";
            table["Storage"] = "Bolsas";
            table["QuestStarterItems"] = "ItensDeQuestIniciado";
            table["SealedRelics"] = "Relikias Seladas";
            table["MagicBoxes"] = "BausMagicos";
            table["Evidences"] = "Evidencias";
            table["GiftBoxes"] = "BausDePresentes";
            table["HolyWater"] = "AguaBenta";
            table["Changing"] = "Aparencia";
            table["Crafting"] = "Criando";
            table["WarriorRecipes"] = "ReceitasGuerreiro";
            table["warrior-recipes"] = "guerreiro-recipes";
            table["CrusaderRecipes"] = "ReceitasTemplaria";
            table["crusader-recipes"] = "receitas-templaria";
            table["SniperRecipes"] = "SniperRecipes";
            table["sniper-recipes"] = "sniper-recipes";
            table["DualGunnerRecipes"] = "ReceitasDual";
            table["dual-gunner-recipes"] = "receitas-dual";
            table["NightMagicianRecipes"] = "ReceitasFeiticeiroNegro";
            table["night-magician-recipes"] = "receitas-feiticeiro-negro";
            table["PriestRecipes"] = "ReceitasCleriga";
            table["priest-recipes"] = "receitas-cleriga";
            table["other-recipes"] = "outras-receitas";
            table["Other Recipes"] = "Outras Receitas";
            table["Quests"] = "Quests";
            table["quests-by-series"] = "quests-de-continuacao";
            table["Quests by Series"] = "Quests de Continuacao";
            table["Pre-series "] = "";
            table[" Series"] = " Continuacao";
            table["other"] = "Outras";
            table["Start Point"] = "Ponto De Inicio";
            table["Prerequisites"] = "Pré-requisitos";
            table["Finish NPC"] = "NPC de Conclusão";
            table["Follow-up"] = "Follow-up";
            table[" EXP, "] = " EXP, ";
            table[" Gold, "] = " Ouro, ";
            table["Collect items:"] = "Coletar itens:";
            table["Starting Dialog"] = "Iniciando Dialog";
            table["Reminder Dialog"] = "Lembrete Dialog";
            table["Completion Dialog"] = "Conclusão Dialog";
            table["quests-1-10"] = "quests-1-10";
            table["Levels quests-1-10"] = "Níveis de missões-1-10";
            table["Kill Monsters:"] = "Matar Monstros:";
            table["quests-11-20"] = "quests-11-20";
            table["Levels quests-11-20"] = "Níveis de missões-11-20";
            table["Use "] = "Usar ";
            table[" times"] = " vezes";
            table["Use Objects:"] = "Use Objects:";
            table["quests-21-30"] = "quests-21-30";
            table["Levels quests-21-30"] = "Níveis de missões-21-30";
            table["quests-31-40"] = "quests-31-40";
            table["Levels quests-31-40"] = "Níveis de missões-31-40";
            table["quests-41-45"] = "quests-41-45";
            table["Levels quests-41-45"] = "Níveis de missões-41-45";
            table["quests-46-50"] = "quests-46-50";
            table["Levels quests-46-50"] = "Níveis de missões-46-50";
            table["quests-51-55"] = "quests-51-55";
            table["Levels quests-51-55"] = "Níveis de missões-51-55";
            table["quests-56-60"] = "quests-56-60";
            table["Levels quests-56-60"] = "Níveis de missões-56-60";
            table["quests-61-65"] = "quests-61-65";
            table["Levels quests-61-65"] = "Níveis de missões-61-65";
            table["quests-66-70"] = "quests-66-70";
            table["Levels quests-66-70"] = "Níveis de missões-66-70";
            table["quests-71-75"] = "quests-71-75";
            table["Levels quests-71-75"] = "Níveis de missões-71-75";
            table["quests-76-80"] = "quests-76-80";
            table["Levels quests-76-80"] = "Níveis de missões-76-80";
            table["quests-81-85"] = "quests-81-85";
            table["Levels quests-81-85"] = "Níveis de missões-81-85";
            table["NPCs"] = "NPCs";
            table["Location"] = "Localização";
            table["Start NPC of"] = "Start NPC de";
            table["End NPC of"] = "NPC Fim da";
            table["Monsters"] = "monstros";
            table["regenshein-mobs"] = "regenshein-mobs";
            table["verband-mobs"] = "verband-mobs";
            table["Killed for Quests"] = "Mortos por Quests";
            table["crac-des-chevaliers-mobs"] = "crac-des-chevaliers-mobs";
            table["amarkand-mobs"] = "amarkand-mobs";
            table["ursula-park-mobs"] = "ursula-park-mobs";
            table["hekla-cave-mobs"] = "hekla-cave-mobs";
            table["halperin-three-forked-road-mobs"] = "halperin-three-forked-road-mobs";
            table["desert-of-sigmund-mobs"] = "desert-of-sigmund-mobs";
            table["disused-mine-l-enfer-mobs"] = "disused-mine-l-enfer-mobs";
            table["basilan-mobs"] = "basilan-mobs";
            table["mt-hessen-mobs"] = "mt-hessen-mobs";
            table["kahill-ruins-mobs"] = "kahill-ruins-mobs";
            table["agros-haima-mobs"] = "agros-haima-mobs";
            table["lump-of-ash-wood-mobs"] = "lump-of-ash-wood-mobs";
            table["ungor-s-forest-mobs"] = "ungor-s-forest-mobs";
            table["tower-of-giovanni-mobs"] = "tower-of-giovanni-mobs";
            table["elter-group-battle-ground-mobs"] = "elter-group-battle-ground-mobs";
            table["epheso-mobs"] = "epheso-mobs";
            table["leopold-mobs"] = "leopold-mobs";
            table["mt-aboel-mobs"] = "mt-aboel-mobs";
            table["argent-forest-mobs"] = "argent-forest-mobs";
            table["hama-valley-mobs"] = "hama-valley-mobs";
            table["kakan-port-mobs"] = "kakan-port-mobs";
            table["sar-port-mobs"] = "sar-port-mobs";
            table["gobay-port-mobs"] = "gobay-port-mobs";
            table["holy-ground-lenape-mobs"] = "holy-ground-lenape-mobs";
            table["pandemonium-mobs"] = "pandemonium-mobs";
            table["air-carrack-mobs"] = "air-carrack-mobs";
            table["karena-mobs"] = "karena-mobs";
            table["panoptinium-mobs"] = "panoptinium-mobs";
            table["la-conti-mobs"] = "la-conti-mobs";
            table["Objects"] = "Objetos";
            table["Used for Quests"] = "Usado para Quests";
            table["Name"] = "Nome";
            table["Level Requirement"] = "Exigência de nível";
            table["Previous Mission"] = "Missão anterior";
            table["Order In Series"] = "Ordem Em Series";
            table["EXP Reward"] = "Recompensa EXP";
            table["Gold Reward"] = "Recompensa de Ouro";
            table["verband-objects"] = "verband-objects";
            table["Verband"] = "Verband";
            table["crac-des-chevaliers-objects"] = "crac-des-chevaliers-objects";
            table["Crac des Chevaliers"] = "Crac des Chevaliers";
            table["amarkand-objects"] = "amarkand-objects";
            table["Amarkand"] = "Amarkand";
            table["hekla-cave-objects"] = "hekla-cave-objects";
            table["Hekla Cave"] = "Hekla Cave";
            table["halperin-three-forked-road-objects"] = "halperin-three-forked-road-objects";
            table["Halperin Three-Forked Road"] = "Halperin Three-Forked Road";
            table["desert-of-sigmund-objects"] = "desert-of-sigmund-objects";
            table["Desert of Sigmund"] = "Desert of Sigmund";
            table["disused-mine-l-enfer-objects"] = "disused-mine-l-enfer-objects";
            table["Disused Mine L'enfer"] = "Disused Mine L'enfer";
            table["mt-hessen-objects"] = "mt-hessen-objects";
            table["Mt. Hessen"] = "Mt. Hessen";
            table["kahill-ruins-objects"] = "kahill-ruins-objects";
            table["Kahill Ruins"] = "Kahill Ruins";
            table["agros-haima-objects"] = "agros-haima-objects";
            table["Agros Haima"] = "Agros Haima";
            table["lump-of-ash-wood-objects"] = "lump-of-ash-wood-objects";
            table["Lump of Ash Wood"] = "Lump of Ash Wood";
            table["ungor-s-forest-objects"] = "ungor-s-forest-objects";
            table["Ungor's Forest"] = "Ungor's Forest";
            table["tower-of-giovanni-objects"] = "tower-of-giovanni-objects";
            table["Tower of Giovanni"] = "Tower of Giovanni";
            table["elter-group-battle-ground-objects"] = "elter-group-battle-ground-objects";
            table["Elter Group Battle Ground"] = "Elter Group Battle Ground";
            table["leopold-objects"] = "leopold-objects";
            table["Leopold"] = "Leopold";
            table["mt-aboel-objects"] = "mt-aboel-objects";
            table["Mt. Aboel"] = "Mt. Aboel";
            table["argent-forest-objects"] = "argent-forest-objects";
            table["Argent Forest"] = "Argent Forest";
            table["hama-valley-objects"] = "hama-valley-objects";
            table["Hama Valley"] = "Hama Valley";
            table["kakan-port-objects"] = "kakan-port-objects";
            table["Kakan Port"] = "Kakan Port";
            table["holy-ground-lenape-objects"] = "holy-ground-lenape-objects";
            table["Holy Ground: Lenape"] = "Holy Ground: Lenape";
            table["karena-objects"] = "karena-objects";
            table["Karena"] = "Karena";
            table["panoptinium-objects"] = "panoptinium-objects";
            table["Panoptinium"] = "Panoptinium";
            table["Skills"] = "Skills";
            table["warrior-skills"] = "warrior-skills";
            table["Warrior Skills"] = "Warrior Skills";
            table[" seconds"] = " seconds";
            table["Gladiator"] = "Gladiator";
            table["gladiator-skills"] = "gladiator-skills";
            table["Gladiator Skills"] = "Gladiator Skills";
            table["crusader-skills"] = "crusader-skills";
            table["Crusader Skills"] = "Crusader Skills";
            table["Paladin"] = "Paladin";
            table["paladin-skills"] = "paladin-skills";
            table["Paladin Skills"] = "Paladin Skills";
            table["sniper-skills"] = "sniper-skills";
            table["Sniper Skills"] = "Sniper Skills";
            table["Buster"] = "Buster";
            table["buster-skills"] = "buster-skills";
            table["Buster Skills"] = "Buster Skills";
            table["dual-gunner-skills"] = "dual-gunner-skills";
            table["Dual   Gunner Skills"] = "Dual   Gunner Skills";
            table["Blaster"] = "Blaster";
            table["blaster-skills"] = "blaster-skills";
            table["Blaster Skills"] = "Blaster Skills";
            table["night-magician-skills"] = "night-magician-skills";
            table["Night   Magician Skills"] = "Night   Magician Skills";
            table["ChaosSorcerer"] = "ChaosSorcerer";
            table["Chaos Sorcerer"] = "Chaos Sorcerer";
            table["chaos-sorcerer-skills"] = "chaos-sorcerer-skills";
            table["Chaos   Sorcerer Skills"] = "Chaos   Sorcerer Skills";
            table["priest-skills"] = "priest-skills";
            table["Priest Skills"] = "Priest Skills";
            table["Saint"] = "Saint";
            table["saint-skills"] = "saint-skills";
            table["Saint Skills"] = "Saint Skills";
            table["BabyFirePran"] = "BabyFirePran";
            table["baby-fire-pran-skills"] = "baby-fire-pran-skills";
            table["Baby Fire Pran Skills"] = "Baby Fire Pran Skills";
            table["ChildFirePran"] = "ChildFirePran";
            table["child-fire-pran-skills"] = "child-fire-pran-skills";
            table["Child Fire Pran Skills"] = "Child Fire Pran Skills";
            table["TeenFirePran"] = "TeenFirePran";
            table["teen-fire-pran-skills"] = "teen-fire-pran-skills";
            table["Teen Fire Pran Skills"] = "Teen Fire Pran Skills";
            table["AdultFirePran"] = "AdultFirePran";
            table["adult-fire-pran-skills"] = "adult-fire-pran-skills";
            table["Adult Fire Pran Skills"] = "Adult Fire Pran Skills";
            table["BabyWaterPran"] = "BabyWaterPran";
            table["baby-water-pran-skills"] = "baby-water-pran-skills";
            table["Baby Water Pran Skills"] = "Baby Water Pran Skills";
            table["ChildWaterPran"] = "ChildWaterPran";
            table["child-water-pran-skills"] = "child-water-pran-skills";
            table["Child Water Pran Skills"] = "Child Water Pran Skills";
            table["TeenWaterPran"] = "TeenWaterPran";
            table["teen-water-pran-skills"] = "teen-water-pran-skills";
            table["Teen Water Pran Skills"] = "Teen Water Pran Skills";
            table["AdultWaterPran"] = "AdultWaterPran";
            table["adult-water-pran-skills"] = "adult-water-pran-skills";
            table["Adult Water Pran Skills"] = "Adult Water Pran Skills";
            table["BabyAirPran"] = "BabyAirPran";
            table["baby-air-pran-skills"] = "baby-air-pran-skills";
            table["Baby Air Pran Skills"] = "Baby Air Pran Skills";
            table["ChildAirPran"] = "ChildAirPran";
            table["child-air-pran-skills"] = "child-air-pran-skills";
            table["Child Air Pran Skills"] = "Child Air Pran Skills";
            table["TeenAirPran"] = "TeenAirPran";
            table["teen-air-pran-skills"] = "teen-air-pran-skills";
            table["Teen Air Pran Skills"] = "Teen Air Pran Skills";
            table["AdultAirPran"] = "AdultAirPran";
            table["adult-air-pran-skills"] = "adult-air-pran-skills";
            table["Adult Air Pran Skills"] = "Adult Air Pran Skills";
            table["Legion"] = "Legion";
            table["legion-skills"] = "legion-skills";
            table["Legion Skills"] = "Legion Skills";
            table["other-skills"] = "other-skills";
            table["Other Skills"] = "Other Skills";
            table["warrior-anvil"] = "warrior-anvil";
            table["Warrior Anvil"] = "Warrior Anvil";
            table["crusader-anvil"] = "crusader-anvil";
            table["Crusader Anvil"] = "Crusader Anvil";
            table["sniper-anvil"] = "sniper-anvil";
            table["Sniper Anvil"] = "Sniper Anvil";
            table["dual-gunner-anvil"] = "dual-gunner-anvil";
            table["Dual   Gunner Anvil"] = "Dual   Gunner Anvil";
            table["night-magician-anvil"] = "night-magician-anvil";
            table["Night   Magician Anvil"] = "Night   Magician Anvil";
            table["priest-anvil"] = "priest-anvil";
            table["Priest Anvil"] = "Priest Anvil";
            table["other-anvil"] = "other-anvil";
            table["Other Anvil"] = "Outros Anvil";
            table["Can be made at the anvil with these ingredients:"] = "Pode ser feito na bigorna com estes ingredientes:";
            table["Cost of making: "] = "O custo de fazer: ";
            table["Chance of getting a Superior: "] = "Possibilidade de obtenção de um Superior: ";
            table["Required to craft:"] = "Obrigatório para embarcações:";
            table["Cost of Upgrade: "] = "Custo de Upgrade: ";
            table["Level Required: "] = "Nível necessário: ";
            table["Rank: "] = "Rank: ";
            table["Physical Attack: "] = "Ataque Físico: ";
            table["Physical Defense: "] = "Defesa Física: ";
            table["Magical Attack: "] = "Ataque Mágico: ";
            table["Magical Defense: "] = "Defesa Mágica: ";
            table["Item cannot be reinforced"] = "Item não pode ser reforçado";
            table["Titles"] = "Títulos";
            table["Success depends on reinforcement level of "] = "O sucesso depende do nível de reforço da ";
            table["Reinforcement Level"] = "Nível de Reforço";
            table["Success Chance"] = "Possibilidade Sucesso";
            table["Cannot be traded"] = "Não pode ser negociado";

            table["Cost"] = "Custo";
            table["Upgradable"] = "Atualizável";
            table["Upgradable Name"] = "Nome atualizável";
            table["Upgraded"] = "Atualizado";
            table["Upgraded Name"] = "Nome atualizado ";
            table["Honor Points:"] = "Pontos de Honra:";
            table["Level Required"] = "Nível necessário";

            table["Used By"] = "Usado por";
            table["Learning Cost"] = "Aprender custo";
            table["MP Required"] = "MP Obrigatório";
            table["Piece of Facion Required"] = "Pedaço de Facion Obrigatório";
            table["Casting Time"] = "Tempo de Execução";
            table["Cooldown Time"] = "Tempo de Espera";
            table["Spell Duration"] = "Feitiço Duração";
            table["Instant Effect"] = "Efeito Imediato";
            table["Lingering Effect"] = "Efeito prolongado";
            table["Passive Effect"] = "Efeito passivo";

            table["Icon"] = "Ícone";
            table["Image"] = "Imagem";
            table["Ingredient Name"] = "Nome do ingrediente";
            table["Quantity"] = "Quantidade";
            table["Description"] = "Descrição";
            table["Level"] = "Nível";
            table["Rank"] = "Rank";
            table["Honor Points"] = "Pontos de Honra";
            table["Medals of Hero"] = "Medalhas de Hero";
        }
    }
}