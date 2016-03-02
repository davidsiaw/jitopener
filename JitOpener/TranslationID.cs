﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JitOpener
{
    class TranslationID
    {
        public static Dictionary<string, string> table = new Dictionary<string, string>();
        static TranslationID()
        {
            table["Sniper"] = "Sniper";
            table["Priest"] = "Priest";
            table["Warrior"] = "Warrior";
            table["Crusader"] = "Crusader";
            table["DualGunner"] = "DualGunner";
            table["NightMagician"] = "NightMagician";
            table["Superior "] = "Superior ";
            table["Head"] = "Kepala";
            table["Body"] = "Badan";
            table["Glove"] = "Tangan";
            table["Boots"] = "Kasut";
            table["Mount"] = "Tunggangan";
            table["TwoHandSword"] = "PedangDuaTangan";
            table["-equipment-sets"] = "-set-peralatan";
            table["Equipment Sets"] = "Set Peralatan";
            table["Shield"] = "Perisai";
            table["OneHandSword"] = "Pedang";
            table["Ammunition"] = "Amunisi";
            table["Rifle"] = "Senapan";
            table["Gun"] = "Pistol";
            table["Dual Gunner"] = "Dual Gunner";
            table["Staff"] = "Tongkat";
            table["Night Magician"] = "Night Magicion";
            table["Wand"] = "Tongkat Sihir";
            table["Accessories"] = "Alatan";
            table["Ring"] = "Cincin";
            table["Buy Price: "] = "Harga Beli: ";
            table["Sell Price: "] = "Harga Jual: ";
            table["Obtainable from quests:"] = "Boleh didapati dari quest:";
            table["[Relic]"] = "[Relic]";
            table["This item is part of a set: "] = "Item ini adalah bagian dari set: ";
            table["Other"] = "Lain";
            table["Set Effect"] = "Efek Set";
            table["Recipe:"] = "Resep:";
            table["Required for quests:"] = "Diperlukan untuk quest:";
            table["Earring"] = "Anting";
            table["Bracelet"] = "Gelang";
            table["Necklace"] = "Kalung";
            table["accessory-sets"] = "accessorios-completos";
            table["Accessory Sets"] = "Set Peralatan";
            table["Items"] = "Item";
            table["Magic Crystals"] = "Kristal";
            table["AllEnchant"] = "EnchantSemua";
            table["WeaponEnchant"] = "EnchantSenjata";
            table["ArmorEnchant"] = "EnchantArmor";
            table["MountEnchant"] = "EnchantTunggangan";
            table["AccessoryEnchant"] = "EnchantPeralatan";
            table["Materials"] = "Bahan";
            table["MonsterDrops"] = "BarangMob";
            table["OresAndMetals"] = "Logam";
            table["Leather"] = "Kulit";
            table["Cloth"] = "Kain";
            table["Minerals"] = "Mineral";
            table["Essence"] = "Essence";
            table["Fuels"] = "Bahan Bakar";
            table["Miscellaneous"] = "Bahan Lain";
            table["Consumable Items"] = "Makanan";
            table["HpConsumables"] = "MakananHp";
            table["MpConsumables"] = "MakananMp";
            table["Reinforce Items"] = "Reinforce";
            table["WeaponReinforce"] = "ReinforceSenjata";
            table["ArmorReinforce"] = "ReinforceArmor";
            table["WRExtract"] = "WRExtract";
            table["WRConcentrate"] = "WRConcentrate";
            table["ARExtract"] = "ARExtract";
            table["ARConcentrate"] = "ARConcentrate";
            table["Pran Items"] = "Item Untuk Pran";
            table["PranWing"] = "SayapPran";
            table["PranDoll"] = "BonekaPran";
            table["PranFood"] = "MakananPran";
            table["PranDress"] = "PakaianPran";
            table["PranAccessory"] = "PeralatanPran";
            table["PranHeadgear"] = "AlatKepalaPran";
            table["Other Items"] = "Item Lain";
            table["Relics"] = "Relic";
            table["MountStones"] = "MountStone";
            table["LegionMissions"] = "PerintahUntukLegion";
            table["Facions"] = "Facion";
            table["FacionOres"] = "FacionOre";
            table["QuestItems"] = "ItemQuest";
            table["Storage"] = "Simpanan";
            table["QuestStarterItems"] = "ItemPermulaanQuest";
            table["SealedRelics"] = "Sealed Relic";
            table["MagicBoxes"] = "MagicBox";
            table["Evidences"] = "Evidence";
            table["GiftBoxes"] = "GiftBox";
            table["HolyWater"] = "HolyWater";
            table["Changing"] = "Transferan";
            table["Crafting"] = "Pembuatan";
            table["WarriorRecipes"] = "ResepWarrior";
            table["warrior-recipes"] = "resep-warrior";
            table["CrusaderRecipes"] = "ResepCrusader";
            table["crusader-recipes"] = "resep-crusader";
            table["SniperRecipes"] = "ResepSniper";
            table["sniper-recipes"] = "resep-sniper";
            table["DualGunnerRecipes"] = "ResepDualGunner";
            table["dual-gunner-recipes"] = "resep-dual-gunner";
            table["NightMagicianRecipes"] = "ResepNightMagician";
            table["night-magician-recipes"] = "resep-night-magician";
            table["PriestRecipes"] = "ResepPriest";
            table["priest-recipes"] = "resep-priest";
            table["other-recipes"] = "resep-lain";
            table["Other Recipes"] = "Resep Lain";
            table["Quests"] = "Quest";
            table["quests-by-series"] = "rangkaian-quest";
            table["Quests by Series"] = "Rangkaian Quest";
            table["Pre-series "] = "Rangkaian ";
            table[" Series"] = "";
            table["other"] = "lain";
            table["Start Point"] = "Titik Permulaan";
            table["Prerequisites"] = "Quest Yang Diperlukan";
            table["Finish NPC"] = "NPC Terakhir";
            table["Follow-up"] = "Quest Berikut";
            table[" EXP, "] = " EXP, ";
            table[" Gold, "] = " Gold, ";
            table["Collect items:"] = "Dapatkan Item:";
            table["Starting Dialog"] = "Dialog Permulaan";
            table["Reminder Dialog"] = "Dialog Peringatan";
            table["Completion Dialog"] = "Dialog Penyelesaian";
            table["quests-1-10"] = "quests-1-10";
            table["Levels quests-1-10"] = "Levels quests-1-10";
            table["Kill Monsters:"] = "Bunuh Mob:";
            table["quests-11-20"] = "quests-11-20";
            table["Levels quests-11-20"] = "Levels quests-11-20";
            table["Use "] = "Guna ";
            table[" times"] = " kali";
            table["Use Objects:"] = "Gunakan Objek:";
            table["quests-21-30"] = "quests-21-30";
            table["Levels quests-21-30"] = "Levels quests-21-30";
            table["quests-31-40"] = "quests-31-40";
            table["Levels quests-31-40"] = "Levels quests-31-40";
            table["quests-41-45"] = "quests-41-45";
            table["Levels quests-41-45"] = "Levels quests-41-45";
            table["quests-46-50"] = "quests-46-50";
            table["Levels quests-46-50"] = "Levels quests-46-50";
            table["quests-51-55"] = "quests-51-55";
            table["Levels quests-51-55"] = "Levels quests-51-55";
            table["quests-56-60"] = "quests-56-60";
            table["Levels quests-56-60"] = "Levels quests-56-60";
            table["quests-61-65"] = "quests-61-65";
            table["Levels quests-61-65"] = "Levels quests-61-65";
            table["quests-66-70"] = "quests-66-70";
            table["Levels quests-66-70"] = "Levels quests-66-70";
            table["quests-71-75"] = "quests-71-75";
            table["Levels quests-71-75"] = "Levels quests-71-75";
            table["quests-76-80"] = "quests-76-80";
            table["Levels quests-76-80"] = "Levels quests-76-80";
            table["quests-81-85"] = "quests-81-85";
            table["Levels quests-81-85"] = "Levels quests-81-85";
            table["NPCs"] = "NPCs";
            table["Location"] = "Lokasi";
            table["Start NPC of"] = "NPC Permulaan Untuk";
            table["End NPC of"] = "NPC Terakhir Untuk";
            table["Monsters"] = "Mob";
            table["regenshein-mobs"] = "regenshein-mobs";
            table["verband-mobs"] = "verband-mobs";
            table["Killed for Quests"] = "Dibunuh untuk Quest";
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
            table["Objects"] = "Objek-objek";
            table["Used for Quests"] = "Diguna Untuk Quest";
            table["Name"] = "Nama";
            table["Level Requirement"] = "Level Yang Diperlukan";
            table["Previous Mission"] = "Quest Sebelumnya";
            table["Order In Series"] = "Urutan dalam Series";
            table["EXP Reward"] = "Ganjaran EXP";
            table["Gold Reward"] = "Ganjaran Gold";
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
            table["Skills"] = "Kebolehan";
            table["warrior-skills"] = "warrior-skills";
            table["Warrior Skills"] = "Kebolehan Warrior";
            table[" seconds"] = " seconds";
            table["Gladiator"] = "Gladiator";
            table["gladiator-skills"] = "gladiator-skills";
            table["Gladiator Skills"] = "Kebolehan Gladiator";
            table["crusader-skills"] = "crusader-skills";
            table["Crusader Skills"] = "Kebolehan Crusader";
            table["Paladin"] = "Paladin";
            table["paladin-skills"] = "paladin-skills";
            table["Paladin Skills"] = "Kebolehan Paladin";
            table["sniper-skills"] = "sniper-skills";
            table["Sniper Skills"] = "Kebolehan Sniper";
            table["Buster"] = "Buster";
            table["buster-skills"] = "buster-skills";
            table["Buster Skills"] = "Kebolehan Buster";
            table["dual-gunner-skills"] = "dual-gunner-skills";
            table["Dual   Gunner Skills"] = "Kebolehan Dual Gunner";
            table["Blaster"] = "Blaster";
            table["blaster-skills"] = "blaster-skills";
            table["Blaster Skills"] = "Kebolehan Blaster";
            table["night-magician-skills"] = "night-magician-skills";
            table["Night   Magician Skills"] = "Kebolehan Night Magician";
            table["ChaosSorcerer"] = "ChaosSorcerer";
            table["Chaos Sorcerer"] = "Chaos Sorcerer";
            table["chaos-sorcerer-skills"] = "chaos-sorcerer-skills";
            table["Chaos   Sorcerer Skills"] = "Kebolehan Chaos Sorcerer";
            table["priest-skills"] = "priest-skills";
            table["Priest Skills"] = "Kebolehan Priest";
            table["Saint"] = "Saint";
            table["saint-skills"] = "saint-skills";
            table["Saint Skills"] = "Kebolehan Saint";
            table["BabyFirePran"] = "BabyFirePran";
            table["baby-fire-pran-skills"] = "baby-fire-pran-skills";
            table["Baby Fire Pran Skills"] = "Kebolehan Baby Fire Pran";
            table["ChildFirePran"] = "ChildFirePran";
            table["child-fire-pran-skills"] = "child-fire-pran-skills";
            table["Child Fire Pran Skills"] = "Kebolehan Child Fire Pran";
            table["TeenFirePran"] = "TeenFirePran";
            table["teen-fire-pran-skills"] = "teen-fire-pran-skills";
            table["Teen Fire Pran Skills"] = "Kebolehan Teen Fire Pran";
            table["AdultFirePran"] = "AdultFirePran";
            table["adult-fire-pran-skills"] = "adult-fire-pran-skills";
            table["Adult Fire Pran Skills"] = "Kebolehan Adult Fire Pran";
            table["BabyWaterPran"] = "BabyWaterPran";
            table["baby-water-pran-skills"] = "baby-water-pran-skills";
            table["Baby Water Pran Skills"] = "Kebolehan Baby Water Pran";
            table["ChildWaterPran"] = "ChildWaterPran";
            table["child-water-pran-skills"] = "child-water-pran-skills";
            table["Child Water Pran Skills"] = "Kebolehan Child Water Pran";
            table["TeenWaterPran"] = "TeenWaterPran";
            table["teen-water-pran-skills"] = "teen-water-pran-skills";
            table["Teen Water Pran Skills"] = "Kebolehan Teen Water Pran";
            table["AdultWaterPran"] = "AdultWaterPran";
            table["adult-water-pran-skills"] = "adult-water-pran-skills";
            table["Adult Water Pran Skills"] = "Kebolehan Adult Water Pran";
            table["BabyAirPran"] = "BabyAirPran";
            table["baby-air-pran-skills"] = "baby-air-pran-skills";
            table["Baby Air Pran Skills"] = "Kebolehan Baby Air Pran";
            table["ChildAirPran"] = "ChildAirPran";
            table["child-air-pran-skills"] = "child-air-pran-skills";
            table["Child Air Pran Skills"] = "Kebolehan Child Air Pran";
            table["TeenAirPran"] = "TeenAirPran";
            table["teen-air-pran-skills"] = "teen-air-pran-skills";
            table["Teen Air Pran Skills"] = "Kebolehan Teen Air Pran";
            table["AdultAirPran"] = "AdultAirPran";
            table["adult-air-pran-skills"] = "adult-air-pran-skills";
            table["Adult Air Pran Skills"] = "Kebolehan Adult Air Pran Skills";
            table["Legion"] = "Legion";
            table["legion-skills"] = "legion-skills";
            table["Legion Skills"] = "Kebolehan Legion";
            table["other-skills"] = "other-skills";
            table["Other Skills"] = "Kebolehan Lain";
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
            table["Other Anvil"] = "Anvil Lain";
            table["Can be made at the anvil with these ingredients:"] = "Boleh dibuat di anvil dengan:";
            table["Cost of making: "] = "Harga pembuatan: ";
            table["Chance of getting a Superior: "] = "Kemungkinan mendapat Superior: ";
            table["Required to craft:"] = "Diperlukan untuk membuat:";
            table["Cost of Upgrade: "] = "Harga Upgrade: ";
            table["Level Required: "] = "Level yang Diperlukan: ";
            table["Rank: "] = "Ranking: ";
            table["Physical Attack: "] = "Serangan Fizik: ";
            table["Physical Defense: "] = "Pertahanan Fizik: ";
            table["Magical Attack: "] = "Serangan Magic: ";
            table["Magical Defense: "] = "Pertahanan Magic: ";
            table["Item cannot be reinforced"] = "Item tidak boleh di-reinforce";
            table["Somewhere in "] = "Lokasi Yang Tidak Diketahui Dalam ";
            table["Incognito"] = "Hantu";
            table["Success depends on reinforcement level of "] = "Sukses tergantung pada tahap reinforce";
            table["Reinforcement Level"] = "Tahap Reinforce";
            table["Success Chance"] = "Kemungkinan Sukses";
            table["Cannot be traded"] = "Tidak boleh dijual";

            table["Success Rate"] = "Kemungkinan Sukses";
            table["PDEF"] = "PDEF";
            table["MDEF"] = "MDEF";
            table["Additional Damage Reduction"] = "Pengurangan Damage";
            table["Additional HP/MP"] = "Tambahan HP/MP";
            table["Can be upgraded with: "] = "Boleh diupgradekan dengan: ";
            table["Into a: "] = "Into a: ";
            table["Can be obtained with: "] = "Boleh didapati dengan: ";
            table["By upgrading: "] = "Dengan menguatkan: ";
            table["PhyAtk"] = "PhyAtk";
            table["MagAtk"] = "MagAtk";
            table["PhyDef"] = "PhyDef";
            table["MagDef"] = "MagDef";
            table["PATK"] = "PATK";
            table["MATK"] = "MATK";
            table["Product"] = "Produk";
            table["Recipe Name"] = "Nama Resep";

            table["Cost"] = "Harga";
            table["Upgradable"] = "Boleh Diupgradekan";
            table["Upgradable Name"] = "Nama Upgrade";
            table["Upgraded"] = "Diupgradekan";
            table["Upgraded Name"] = "Nama Setelah Diupgradekan";
            table["Honor Points:"] = "Honor Points:";
            table["Level Required"] = "Level Required";

            table["Used By"] = "Diguna Oleh";
            table["Learning Cost"] = "Harga Belajar";
            table["MP Required"] = "MP Yang Diperlukan";
            table["Piece of Facion Required"] = "Piece of Facion yang Diperlukan";
            table["Casting Time"] = "Masa Casting";
            table["Cooldown Time"] = "Masa Cooldown";
            table["Spell Duration"] = "Durasi Spell";
            table["Instant Effect"] = "Efek Segera";
            table["Lingering Effect"] = "Efek Yang Tinggal";
            table["Passive Effect"] = "Efek Pasif"; 
            
            table["Icon"] = "Icon";
            table["Image"] = "Imej";
            table["Ingredient Name"] = "Nama Bahan";
            table["Quantity"] = "Kuantiti";
            table["Description"] = "Deskripsi";
            table["Level"] = "Level";
            table["Rank"] = "Rank";
            table["Honor Points"] = "Honor Points";
            table["Medals of Hero"] = "Medal of Hero";
        }
    }
}
