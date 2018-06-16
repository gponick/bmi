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
        public static string Version = "v0.0.1";
        public static List<Mod> mods = new List<Mod>();
        static void Initialize(bool fakecli = true)
        {
            mods.Clear();
            if (fakecli)
            {
                Console.WriteLine(@"C:\BattleTech\Mods\> bmi init");
            }
            System.IO.DirectoryInfo di = new DirectoryInfo(@".");
            Console.Write($"bmi> Initializing Mods from [{di.FullName}]...");

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
            System.IO.DirectoryInfo di = new DirectoryInfo(@".");
            Console.Write($"bmi> Initializing {modName} from [{di.FullName}]...");

            Mod toRemove = null;
            foreach(Mod m in mods)
            {
                if(m.Name.Equals(modName,StringComparison.OrdinalIgnoreCase))
                {
                    toRemove = m;
                    break;
                }
            }
            if (toRemove != null)
                mods.Remove(toRemove);
            //toRemove = Mod.LoadFromDirectory(toRemove.ModFullDirectory);
            toRemove = Mod.LoadFromDirectory(Path.Combine(di.FullName, modName == "*" ? "" : modName));
            mods.Add(toRemove);

            Console.WriteLine(@" SUCCESS!");
        }

        public static void List(bool fakecli = false)
        {
            if(fakecli)
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

        private static void Install(string[] args)
        {
            if (args.Length == 2)
                Install(args[1]);
            if (args.Length == 3)
                Install(args[1], args[2]);

        }

        private static void Install(string name, string version)
        {
            Initialize(false);
            if (mods.Where(md => md.Name.ToLower() == name.ToLower()).Count() > 0)
            {
                Console.WriteLine($"bmi> Installing {name}... FAIL!");
                Console.WriteLine($"     MOD ALREADY INSTALLED!");
                return;
            }
            Mod m = BMILib.IndexClient.GetModByName(name);
            Console.Write($"bmi> Installing {m.Name} [Version: {version}]...");
            m.Install(version);
            Console.WriteLine(" SUCCESS!");
            Initialize(m.Name, false);
            List(false);

        }

        private static void Install(string name)
        {
            Initialize();
            if(mods.Where(md=>md.Name.ToLower() == name.ToLower()).Count() > 0)
            {
                Console.WriteLine($"bmi> Installing {name}... FAIL!");
                Console.WriteLine($"     MOD ALREADY INSTALLED!");
                return;
            }
            Mod m = BMILib.IndexClient.GetModByName(name);
            Console.Write($"bmi> Installing {m.Name}...");
            m.Install();
            Console.WriteLine(" SUCCESS!");
            Initialize(name,false);
            List(false);
        }

        public static void Update(string updateString = "*", bool fakecli = false)
        {
            if(fakecli)
                Console.WriteLine($"C:\\BattleTech\\Mods\\> bmi update  {updateString}");
            foreach (Mod m in mods.Where(mm => mm.NeedsUpdate()))
            {
                if(updateString == "*" || m.Name.Equals(updateString,StringComparison.OrdinalIgnoreCase))
                    Console.WriteLine($"bmi> Updating {m.Name} from {m.Version} to {m.LatestRelease.TagName}... {(m.Update() ? "Success" : "FAIL!")}");
            }
            Initialize(updateString, false);

        }
        
        static void Help()
        {
            Console.WriteLine(
@"
Battletech Mod Installerizer

Usage:
  bmi <command> [options]

Commands:
  init                        Initialize Mod directory (Installs and/or updates modtek.dll) (NOT SUPPORTED)
  install       [modname]     Install latest release of [modname] Will not install if [modname] is already installed.
  uninstall     [modname]     Uninstall [modname] (NOT IMPLEMENTED)
  list                        List installed mods.
  show          [modname]     Show information about [modname] (NOT SUPPORTED)
  update        [modname]     Update [modname] to latest version (NOT SUPPORTED)
  search        (modname)     Search bmi-index for mods. Optional argument (modname) is a case insensitive wildcard search (eg: *(modname) )
  help                        Show help for commands.
"
);
            Console.WriteLine($"Version: {Program.Version}");
        }

        static void Search(string[] args)
        {
            BMILib.IndexClient.Initialize();
            Console.WriteLine($"{"Name",-30}|{"Version",-10}|{"Catgeory",-15}|{"Website"}");
            Console.WriteLine($"------------------------------+----------+---------------+-----------------------------------");
            foreach (Mod m in BMILib.IndexClient.ModList.Values.OrderBy(ml=>ml.Name))
            {
                if (args.Count() > 1)
                {
                    if (m.Name.ToLower().Contains(args[1].ToLower()))
                    {
                        m.PrintSearchString();
                    }
                }
                else
                {

                    m.PrintSearchString();
                }
            }
            Console.WriteLine();
            Console.WriteLine("Legend: [* Installed - Up-to-date] [+ Installed - Needs Update]");
        }

        static void Show(string[] args)
        {
            Mod m = BMILib.IndexClient.GetModByName(args[1]);
            m.fetchReleasesFromWebsite();
            Console.WriteLine($"Mod info for: {m.Name}");
            Console.WriteLine($"Author:       {m.Author}");
            Console.WriteLine($"Website:      {m.Website}");
            Console.WriteLine($"Category:     {m.Category}");
            Console.WriteLine($"----------------------- INSTALLABLE VERSIONS -----------------------");
            Console.WriteLine($"{"Version",-20}|Description");
            Console.WriteLine($"--------------------+-----------------------------------------------");
            foreach(var release in m.Releases)
            {
                string body = "";
                if(release.Body != "")
                {
                    if (release.Body.Length <= 255)
                        body = release.Body;
                    else
                        body = release.Body.Substring(0, 255);
                }
                if(body.Contains('\n'))
                {
                    body = body.Split('\n')[0];
                }
                if (body == "")
                    body = release.Name;
                Console.WriteLine($"{release.TagName,-20}|{body}");
                Console.WriteLine($"--------------------+-----------------------------------------------");
            }
        }

        static void Create()
        {
            Console.WriteLine($"bmi> Initializing Mod directory.");
            Console.WriteLine($"bmi> Checking for existence of Modtek");
            // check here
            if(File.Exists(Path.Combine(".","modtek.dll")))
            {
                Console.WriteLine($"bmi> modtek.dll exists. Backing it up to Modtek.dll.old!");
                if(File.Exists(Path.Combine(".", "modtek.dll.old")))
                    File.Delete(Path.Combine(".", "modtek.dll.old"));
                File.Move(Path.Combine(".", "modtek.dll"), Path.Combine(".", "modtek.dll.old"));
            }
            Console.WriteLine($"bmi> Fetching Modtek from internet...");
            Mod m = new Mod();
            m.Website = "https://github.com/Mpstark/ModTek";
            m.fetchLatestReleaseFromWebsite();
            m.fetchLatestReleaseBytes();
            if(m.LatestReleaseFile != null && m.LatestReleaseFile.Count() > 0)
            {
                Console.Write($"bmi> Installing Modtek version {m.LatestRelease.TagName}...");
                System.IO.File.WriteAllBytes("modtek.dll", m.LatestReleaseFile);
                Console.WriteLine(" SUCCESS!");
            }
        }

        static void Main(string[] args)
        {
            if(args.Count() == 0)
            {
                Help();
            }
            else if (args[0] == "init")
            {
                Create();
            }
            else if (args[0] == "list")
            {
                Initialize(false);
                List(false);
            }
            else if (args[0] == "update")
            {
                if(args.Count() > 1)
                {
                    Initialize(false);
                    List(false);
                    Update(args[1], false);
                    List(false);
                }
                else
                {
                    Initialize(false);
                    List(false);
                    Update("*", false);
                    List(false);
                }
            }
            else if(args[0] == "help")
            {
                Help();
            }
            else if (args[0] == "install")
            {
                Install(args);
            }
            else if (args[0] == "search")
            {
                Search(args);
            }
            else if (args[0] == "show")
            {
                Show(args);
            }
        }
    }
}
