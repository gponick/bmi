using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Octokit;

namespace BMILib
{
    public class IndexClient
    {
        public static bool Initialized = false;
        public static Dictionary<string, Mod> ModList = null;

        public static Mod GetModByName(string name)
        {
            if(!Initialized)
            {
                Initialize();
            }
            Mod m = null;
            if (ModList.ContainsKey(name))
            {
                m = ModList[name];
            }
            return m;
        }

        public static void LoadFromServer()
        {
            var json = new WebClient().DownloadString(Utils.ghe + "modlist.json");
            Dictionary<string, Mod> temp = JsonConvert.DeserializeObject<Dictionary<string, Mod>>(json);
            foreach(string modkey in temp.Keys)
            {
                Mod modd = temp[modkey];
                modd.Name = modkey;
                modd.fetchLatestReleaseFromWebsite();
            }
            ModList = temp;
        }

        public static void Initialize()
        {
            LoadFromServer();
            foreach (string modName in ModList.Keys)
            {
                Mod mod = ModList[modName];
                if(mod.LatestRelease == null)
                    mod.fetchLatestReleaseFromWebsite();
            }
            Initialized = true;
        }
    }
}
