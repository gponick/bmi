using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMILib;

namespace bmicli
{
    class Program
    {
        public static List<Mod> mods = new List<Mod>();
        public void DoStuff()
        {
            BMILib.Mod mod = BMILib.Mod.LoadFromDirectory(@"C:\Users\George\source\repos\BTechMods\temp\Mods-Non-Rogue-2018-05-31\BTMLColorLOSMod");
            //BMILib.Mod mod = BMILib.Mod.LoadFromDirectory(@"C:\Program Files (x86)\Steam\steamapps\common\BATTLETECH\Mods\DirtyBallisticFix");
            Console.WriteLine($"Mod: {mod.Name} from [{mod.ModDirectory}] is version: {mod.Version}");
            Console.WriteLine($"{mod.Author} wrote it and it lives at {mod.Website}");
            if (mod.JSONDirectories.Count > 0)
            {
                foreach (var key in mod.JSONDirectories.Keys)
                {
                    Console.WriteLine($"[{key}]");
                    foreach (var thing in mod.JSONDirectories[key])
                    {
                        Console.WriteLine($"\t\tcontained: {thing.Item1} file");
                    }
                }
            }
            if (mod.Releases.Count > 0)
            {
                foreach (var release in mod.Releases.OrderByDescending(l => l.CreatedAt))
                {
                    Console.WriteLine($"Version {release.TagName} released at {release.CreatedAt}. Download at: {release.Assets[0].BrowserDownloadUrl}");
                }
                Console.WriteLine($"Needs Update????? {mod.NeedsUpdate()}");
                if (mod.NeedsUpdate())
                {
                    mod.fetchLatestReleaseBytes();
                    System.IO.File.WriteAllBytes(@"C:\Users\George\source\repos\BTechMods\temp\Mods-Non-Rogue-2018-05-31\BTMLColorLOSMod\" + mod.LatestRelease.Assets[0].Name, mod.LatestReleaseFile);
                    Console.WriteLine("Update file written");
                }
            }
        }
        static void Initialize(bool fakecli = true)
        {
            mods.Clear();
            if (fakecli)
            {
                Console.WriteLine(@"C:\BattleTech\Mods\> bmi init");
            }
            Console.Write(@"bmi> Initializing Mods from [C:\Battletech\Mods\]...");

            //System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(@"C:\Users\George\source\repos\BTechMods\temp\RogueTech-79-0-9856HF1\");
            System.IO.DirectoryInfo di = new DirectoryInfo(@"C:\Users\George\source\repos\BTechMods\temp\BMITest\");
            //System.IO.DirectoryInfo di = new DirectoryInfo(".");
            mods = new List<Mod>();
            foreach (DirectoryInfo d in di.EnumerateDirectories())
            {
                try
                {
                    Mod m = Mod.LoadFromDirectory(d.FullName);
                    if (m != null)
                        mods.Add(m);
                    //else
                    //    Console.WriteLine($"{d.Name} didn't load right!");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            Console.WriteLine(@" SUCCESS!");
        }
        static void Initialize(string modName, bool fakecli = true)
        {
            if(modName == "")
            {
                return;
            }
            if (modName == "*")
            {
                Initialize(false);
                return;
            }
            if (fakecli)
            {
                Console.WriteLine($"C:\\BattleTech\\Mods\\> bmi init {modName}");
            }
            Console.Write($"bmi> Initializing {modName} from [C:\\Battletech\\Mods\\]...");

            Mod toRemove = null;
            foreach(Mod m in mods)
            {
                if(m.Name == modName)
                {
                    toRemove = m;
                    break;
                }
            }
            if (toRemove != null)
                mods.Remove(toRemove);
            toRemove = Mod.LoadFromDirectory(toRemove.ModFullDirectory);
            mods.Add(toRemove);

            Console.WriteLine(@" SUCCESS!");
        }

        public static void List()
        {
            Console.WriteLine(@"C:\BattleTech\Mods\> bmi list");
            Console.WriteLine($"{"Name",-30}|{"Version",-10}|{"Directory",-30}|{"Needs Update"}");
            Console.WriteLine($"------------------------------+----------+------------------------------+--------------------");
            foreach (Mod m in mods.Where(mmm=>mmm != null).OrderBy(mm=>mm.Name))
            {
                try
                {
                    Console.WriteLine($"{m.Name.Substring(0,Math.Min(30,m.Name.Length)),-30}|{m.Version.ToString(),-10}|{m.ModDirectory.Substring(0, Math.Min(30, m.ModDirectory.Length)),-30}|{m.NeedsUpdate()}{(m.NeedsUpdate() ? ":" + m.LatestRelease.TagName.Replace("v", "") : "")}");
                }
                catch
                {
                    Console.WriteLine($"{m.Name,-30}|{"",-10}|{m.ModDirectory.Substring(0, Math.Min(30, m.ModDirectory.Length)),-30}|");
                }
            }
        }

        public static void Update(string updateString = "*")
        {
            Console.WriteLine($"C:\\BattleTech\\Mods\\> bmi update  {updateString}");
            foreach (Mod m in mods.Where(mm => mm.NeedsUpdate()))
            {
                if(updateString == "*" || m.Name == updateString)
                    Console.WriteLine($"bmi> Updating {m.Name} from {m.Version} to {m.LatestRelease.TagName}... {(m.Update() ? "Success" : "FAIL!")}");
            }
            Initialize(updateString, false);

        }

        static void Main(string[] args)
        {
            if(args.Count() == 0)
            {
                Initialize();
                List();
                Update();
                List();
            }
            if (args[0] == "init")
            {
                Initialize();
                
            }
            List();
            //Update("BTMLAddBindableEscapeKey");
            //Update("CommanderPortraitLoader");
            //Update("RandomCampaignStart");
            Update();
            //Update("DynModLib");

            List();
        }
    }
}
