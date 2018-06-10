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
            toRemove = Mod.LoadFromDirectory(toRemove.ModFullDirectory);
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

        static void Main(string[] args)
        {
            if(args.Count() == 0)
            {
                Initialize(false);
                List(false);
                Update("*",false);
                List(false);
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
        }
    }
}
