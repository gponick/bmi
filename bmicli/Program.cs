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
                if(m.Name == modName)
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
                if(updateString == "*" || m.Name == updateString)
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
  install       [modname]     Install latest release of [modname] Will not install if [modname] is already installed.
  uninstall     [modname]     Uninstall [modname] (NOT IMPLEMENTED)
  list                        List installed mods.
  show          [modname]     Show information about installed [modname] (NOT IMPLEMENTED)
  update        [modname]     Update [modname] to latest version (NOT SUPPORTED)
  search        (modname)     Search bmi-index for mods. Optional argument (modname) is a case insensitive wildcard search (eg: *(modname) )
  help                        Show help for commands.
"
);
        }

        static void Search(string[] args)
        {
            BMILib.IndexClient.Initialize();
            Console.WriteLine($"{"Name",-30}|{"Version",-10}|{"Catgeory",-15}|{"Website"}");
            Console.WriteLine($"------------------------------+----------+---------------+-----------------------------------");
            foreach (Mod m in BMILib.IndexClient.ModList.Values)
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

        static void Main(string[] args)
        {
            if(args.Count() == 0)
            {
                Help();
                //Search(args);
            }
            else if (args[0] == "init")
            {
                Initialize();
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
                Install(args[1]);
            }
            else if (args[0] == "search")
            {
                Search(args);
            }
        }
    }
}
