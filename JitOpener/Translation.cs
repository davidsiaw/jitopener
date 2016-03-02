using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JitOpener
{

    class Translation
    {
        public static HashSet<string> strings = new HashSet<string>();

        Dictionary<string, string> dict = new Dictionary<string, string>();
        Dictionary<string, string> secondary = new Dictionary<string, string>();

        public Translation(FileFormats.StrDefBin strdefbin, FileFormats.ItemListBin itemlist, Dictionary<string,string> secondary)
        {
            dict["Warrior"] = strdefbin.strs[60].GetString(0);
            dict["Gladiator"] = strdefbin.strs[61].GetString(0);
            dict["Crusader"] = strdefbin.strs[63].GetString(0);
            dict["Paladin"] = strdefbin.strs[64].GetString(0);
            dict["Sniper"] = strdefbin.strs[66].GetString(0);
            dict["Buster"] = strdefbin.strs[67].GetString(0);
            dict["Dual_Gunner"] = strdefbin.strs[69].GetString(0);
            dict["Blaster"] = strdefbin.strs[70].GetString(0);
            dict["Night_Magician"] = strdefbin.strs[72].GetString(0);
            dict["Chaos_Sorcerer"] = strdefbin.strs[73].GetString(0);
            dict["Priest"] = strdefbin.strs[75].GetString(0);
            dict["Saint"] = strdefbin.strs[76].GetString(0);

            dict["Head"] = strdefbin.strs[153].GetString(0);
            dict["Body"] = strdefbin.strs[149].GetString(0);
            dict["Glove"] = strdefbin.strs[155].GetString(0);
            dict["Boots"] = strdefbin.strs[156].GetString(0);
            dict["Two_Hand_Sword"] = strdefbin.strs[157].GetString(0);
            dict["Shield"] = strdefbin.strs[151].GetString(0);
            dict["One_Hand_Sword"] = strdefbin.strs[159].GetString(0);
            dict["Rifle"] = strdefbin.strs[162].GetString(0);
            dict["Gun"] = strdefbin.strs[161].GetString(0);
            dict["Staff"] = strdefbin.strs[164].GetString(0);
            dict["Wand"] = strdefbin.strs[163].GetString(0);

            dict["Accessories"] = strdefbin.strs[829].GetString(0);
            dict["Ring"] = strdefbin.strs[834].GetString(0);
            dict["Earring"] = strdefbin.strs[835].GetString(0);
            dict["Bracelet"] = strdefbin.strs[836].GetString(0);
            dict["Necklace"] = strdefbin.strs[837].GetString(0);

            dict["Facion"] = itemlist.items[5101].Name;
            dict["Facion Ore"] = itemlist.items[5837].Name;

            dict["Magic Crystals"] = strdefbin.strs[176].GetString(0);

            dict["Leather"] = strdefbin.strs[378].GetString(0);
            dict["Cloth"] = strdefbin.strs[379].GetString(0);
            dict["Minerals"] = strdefbin.strs[377].GetString(0);
            dict["Essence"] = strdefbin.strs[380].GetString(0);

            dict["Changing"] = strdefbin.strs[177].GetString(0);

            this.secondary = secondary;
        }

        public string Translate(string str, params object[] parms)
        {
            return string.Format(str, parms);
        }

        public string Translate(string str)
        {
            strings.Add(str);
            if (dict.ContainsKey(str))
            {
                return dict[str];
            }
            if (secondary.ContainsKey(str))
            {
                return secondary[str];
            }
            return str;
        }
    }
}
