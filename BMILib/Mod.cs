using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Octokit;
using SharpCompress.Archives;
using SharpCompress.Archives.Zip;
using SharpCompress.Archives.Rar;
using SharpCompress.Common;
using SharpCompress.Readers;
using SharpCompress.Writers;

namespace BMILib
{
    public static class IArchiveEntryExtensionsGP
    {
        public static void WriteToDirectoryGP(this IArchiveEntry entry, string destinationDirectory, string modName,
                                      ExtractionOptions options = null)
        {
            string destinationFileName;
            string file = Path.GetFileName(entry.Key);
            string fullDestinationDirectoryPath = Path.GetFullPath(destinationDirectory);

            options = options ?? new ExtractionOptions()
            {
                Overwrite = true
            };


            if (options.ExtractFullPath)
            {
                string folder = Path.GetDirectoryName(entry.Key.Replace(modName + "/","").Replace(modName + "\\",""));
                string destdir = Path.GetFullPath(
                                    Path.Combine(fullDestinationDirectoryPath, folder)
                                 );

                if (!Directory.Exists(destdir))
                {
                    if (!destdir.StartsWith(fullDestinationDirectoryPath))
                    {
                        throw new ExtractionException("Entry is trying to create a directory outside of the destination directory.");
                    }

                    Directory.CreateDirectory(destdir);
                }
                destinationFileName = Path.Combine(destdir, file);
            }
            else
            {
                destinationFileName = Path.Combine(fullDestinationDirectoryPath, file);
            }

            if (!entry.IsDirectory)
            {
                destinationFileName = Path.GetFullPath(destinationFileName);

                if (!destinationFileName.StartsWith(fullDestinationDirectoryPath))
                {
                    throw new ExtractionException("Entry is trying to write a file outside of the destination directory.");
                }

                entry.WriteToFile(destinationFileName, options);
            }
        }
    }
    public class Mod
    {
        public string Name;
        public string ModDirectory;
        public string ModFullDirectory;
        public string Version;
        public Semver.SemVersion SemVer;
        public string Author;
        public string Category;
        public string Website;
        public bool Initialized;
        public List<Octokit.Release> Releases = new List<Release>();
        private static Uri ghe = Utils.ghe;
        private static GitHubClient ghClient = Utils.ghClient;

        public Dictionary<string, List<Tuple<string, string>>> JSONDirectories = new Dictionary<string, List<Tuple<string, string>>>();


        public Byte[] LatestReleaseFile;

        public void fetchLatestReleaseBytes()
        {
            using (var client = new WebClient())
            {
                LatestReleaseFile = client.DownloadData(this.LatestRelease.Assets[0].BrowserDownloadUrl);
            }
        }

        public void LoadVersion()
        {
            DirectoryInfo di = new DirectoryInfo(Path.Combine(new DirectoryInfo(".").FullName, this.Name));
            if (di.Exists)
            {
                var modFile = Path.Combine(di.FullName, "mod.json");
                if (File.Exists(modFile))
                {
                    using (StreamReader file = File.OpenText(modFile))
                    {
                        var modjson = file.ReadToEnd();
                        Mod tempMod = JsonConvert.DeserializeObject<Mod>(modjson);
                        if (tempMod.Version != null && tempMod.Version != "")
                        {
                            if (Semver.SemVersion.TryParse(tempMod.Version, out tempMod.SemVer) == false)
                                if (Semver.SemVersion.TryParse(tempMod.Version.Substring(1), out tempMod.SemVer) == false)
                                    tempMod.SemVer = new Semver.SemVersion(0);


                            this.SemVer = tempMod.SemVer;

                                    
                        }
                    }
                }
            }
        }
         
        public bool NeedsUpdate()
        {
            if (this.SemVer == null)
                this.LoadVersion();
            if(this.Releases != null && this.Releases.Count > 0)
            {
                try
                {
                    this.fetchLatestReleaseFromWebsite();
                }
                catch
                {
                    try
                    {
                        this.fetchReleasesFromWebsite();
                    }
                    catch
                    {

                    }
                }
            }
            if (this.Releases != null && this.Releases.Count > 0)
            {
                Semver.SemVersion TagNameVersion = null;
                if (Semver.SemVersion.TryParse(this.LatestRelease.TagName.Substring(1), out TagNameVersion) == false)
                    Semver.SemVersion.TryParse(this.LatestRelease.TagName, out TagNameVersion);
                if (TagNameVersion != null && TagNameVersion > this.SemVer)
                {
                    return true;
                }
            }
            return false;
        }

        public Release LatestRelease
        {
            get
            {
                if(Releases != null && Releases.Count > 0)
                {
                    return Releases.OrderByDescending(l => l.CreatedAt).First();
                }
                return null;
            }
        }

        public void fetchLatestReleaseFromWebsite()
        {
            try
            {
                var releases = ghClient.Repository.Release.GetLatest(this.Author, this.Name);
                releases.Wait();
                this.Releases = new List<Release>();
                this.Releases.Add(releases.Result);
            }
            catch (Exception e)
            {
                try
                {
                    string modauthor = "";
                    string modname = "";
                    var regex = new Regex("http.*://github.com/([A-Za-z0-9]*)/([A-Za-z0-9]*)");
                    var match = regex.Match(this.Website);
                    if (match.Success)
                    {
                        modauthor = match.Groups[1].Value;
                        modname = match.Groups[2].Value;
                    }
                    var releases = ghClient.Repository.Release.GetLatest(modauthor, modname);
                    releases.Wait();
                    this.Releases = new List<Release>();
                    this.Releases.Add(releases.Result);
                }
                catch (Exception ex)
                {
                    //Console.WriteLine(e);
                    //Console.WriteLine(ex);
                }
            }
        }

        public void fetchReleasesFromWebsite()
        {

                try
                {
                    var releases = ghClient.Repository.Release.GetAll(this.Author, this.Name);
                    releases.Wait();
                    this.Releases = new List<Release>(releases.Result);
                }
                catch(Exception e)
                {
                try
                {
                        string modauthor = "";
                        string modname = "";
                        var regex = new Regex("http.*://github.com/([A-Za-z0-9]*)/([A-Za-z0-9]*)");
                        var match = regex.Match(this.Website);
                        if(match.Success)
                        {
                            modauthor = match.Groups[1].Value;
                            modname = match.Groups[2].Value;
                        }
                        var releases = ghClient.Repository.Release.GetAll(modauthor, modname);
                        releases.Wait();
                        this.Releases = new List<Release>(releases.Result);
                    }
                    catch (Exception ex)
                    {
                    //Console.WriteLine(e);
                    //Console.WriteLine(ex);
                    }
                }
        }

        public void enumerateDirectoryForJson(DirectoryInfo di)
        {
            foreach (var dir in di.EnumerateDirectories())
            {
                enumerateDirectoryForJson(dir);
            }
            foreach (var f in di.EnumerateFiles("*.json"))
            {
                var data = new Tuple<string, string>(f.Name, f.OpenText().ReadToEnd());
                if (!JSONDirectories.ContainsKey(di.FullName))
                {
                    JSONDirectories.Add(di.FullName, new List<Tuple<string, string>>());
                }
                JSONDirectories[di.FullName].Add(data);

            }
        }

        public void Install()
        {
            try
            {
                
                this.fetchLatestReleaseBytes();
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                return;
            }
            if (this.LatestReleaseFile == null || this.LatestReleaseFile.Length == 0)
                return;
            
            System.IO.DirectoryInfo di = new DirectoryInfo(@".");
            this.ModFullDirectory = Path.Combine(di.FullName, this.Name);
            var latestReleaseFileName = System.IO.Path.Combine(di.FullName, this.LatestRelease.Assets[0].Name);
            System.IO.File.WriteAllBytes(latestReleaseFileName, this.LatestReleaseFile);
            if (latestReleaseFileName.ToLower().EndsWith(".zip"))
            {
                using (var archive = ZipArchive.Open(latestReleaseFileName))
                {
                    foreach (var entry in archive.Entries.Where(x => !x.IsDirectory))
                    {
                        entry.WriteToDirectoryGP(this.ModFullDirectory, this.Name, new ExtractionOptions()
                        {
                            ExtractFullPath = true,
                            Overwrite = true
                        });
                    }
                }
            }
            else if (latestReleaseFileName.ToLower().EndsWith(".rar"))
            {
                using (var archive = RarArchive.Open(latestReleaseFileName))
                {
                    foreach (var entry in archive.Entries.Where(x => !x.IsDirectory))
                    {
                        entry.WriteToDirectoryGP(this.ModFullDirectory, this.Name, new ExtractionOptions()
                        {
                            ExtractFullPath = true,
                            Overwrite = true
                        });
                    }
                }
            }
            System.IO.File.Delete(latestReleaseFileName);
        }

        public bool Update()
        {
            string fromVersion = this.Version;
            try
            {
                this.fetchLatestReleaseBytes();
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
            if (this.LatestReleaseFile == null)
                return false;
            if (this.LatestReleaseFile.Length == 0)
                return false;
            try
            {
                // write temp file
                var latestReleaseFileName = System.IO.Path.Combine(this.ModFullDirectory, this.LatestRelease.Assets[0].Name);
                System.IO.File.WriteAllBytes(latestReleaseFileName,this.LatestReleaseFile);
                // make backup
                var backupFileName = this.Name + "-" + (this.Version != "??" ? this.Version : DateTime.Now.Ticks.ToString()) + ".zip";
                using (var archive = SharpCompress.Archives.Zip.ZipArchive.Create())
                {
                    archive.AddAllFromDirectory(this.ModFullDirectory);
                    try
                    {
                        archive.SaveTo(Path.Combine(this.ModFullDirectory, backupFileName), new WriterOptions(CompressionType.Deflate));
                    }
                    catch
                    {
                        // who cares 
                    }
                }
                //using (var backup = System.IO.Compression.ZipFile.Open(Path.Combine(this.ModFullDirectory, backupFileName), System.IO.Compression.ZipArchiveMode.Create))
                //{
                //    var files = new DirectoryInfo(this.ModFullDirectory).EnumerateFiles();
                //    foreach (var file in files)
                //    {
                //        if (file.Name != backupFileName)
                //        {
                //            var entry = backup.CreateEntry(file.Name);
                //            using (var stream = entry.Open())
                //            {
                //                using (var filestream = file.OpenRead())
                //                {
                //                    while (filestream.Position != filestream.Length)
                //                    {
                //                        var fb = filestream.ReadByte();
                //                        stream.WriteByte((byte)fb);
                //                    }
                //                }
                //            }
                //        }
                //    }
                //}
                if(latestReleaseFileName.ToLower().EndsWith(".zip"))
                { 
                    using (var archive = ZipArchive.Open(latestReleaseFileName))
                    {
                        //archive.WriteToDirectory(this.ModFullDirectory,new ExtractionOptions()
                        //{
                        //    ExtractFullPath = true,
                        //    Overwrite = true
                        //});
                        foreach(var entry in archive.Entries.Where(x=>!x.IsDirectory))
                        {
                            entry.WriteToDirectoryGP(this.ModFullDirectory, this.Name, new ExtractionOptions()
                            {
                                ExtractFullPath = true,
                                Overwrite = true
                            });
                        }
                        //foreach (var entry in archive.Entries.Where(entry => !entry.IsDirectory))
                        //{
                        //    entry.
                        //    System.IO.File.Delete(System.IO.Path.Combine(this.ModFullDirectory, entry.Key));
                        //    entry.WriteToDirectory(this.ModFullDirectory, new ExtractionOptions()
                        //    {
                        //        ExtractFullPath = true,
                        //        Overwrite = true
                        //    });
                        //}
                    }
                }
                else if (latestReleaseFileName.ToLower().EndsWith(".rar"))
                {
                    using (var archive = RarArchive.Open(latestReleaseFileName))
                    {
                        foreach(var entry in archive.Entries.Where(x=>!x.IsDirectory))
                        {
                            entry.WriteToDirectoryGP(this.ModFullDirectory, this.Name, new ExtractionOptions()
                            {
                                ExtractFullPath = true,
                                Overwrite = true
                            });
                        }
                    }
                }
                //using (var outZip = new FileStream(latestReleaseFileName, System.IO.FileMode.Open))
                //{
                //    using (var zipArchive = new System.IO.Compression.ZipArchive(outZip, System.IO.Compression.ZipArchiveMode.Read))
                //    {
                //        foreach (var entry in zipArchive.Entries)
                //        {
                //            if (entry.Name == "" && entry.Name != this.ModDirectory)
                //                continue;
                //            try
                //            {
                //                System.IO.File.Delete(System.IO.Path.Combine(this.ModFullDirectory, entry.Name));
                //                byte[] bytes = new byte[entry.Length];
                //                using (var stream = entry.Open())
                //                {
                //                    stream.Read(bytes, 0, (int)entry.Length);
                //                }
                //                System.IO.File.WriteAllBytes(Path.Combine(this.ModFullDirectory, entry.FullName.Replace(this.ModDirectory + "/", "")), bytes);
                //            }
                //            catch (Exception e)
                //            {
                //                Console.WriteLine(e);
                //            }
                //        }
                //    }
                //}
                return true;
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
            return false;
        }

        public bool IsInstalled()
        {
            DirectoryInfo di = new DirectoryInfo(Path.Combine(".", this.Name));
            return di.Exists;
        }

        public void PrintSearchString()
        {
            Console.WriteLine($"{this.Name.Substring(0, Math.Min(30, this.Name.Length)),-30}|{this.LatestRelease.TagName + (this.IsInstalled() ? (this.NeedsUpdate() ? "+" : "*") : ""),-10}|{this.Category,-15}|{this.Website}");
        }

        public static Mod LoadFromDirectory(string directory)
        {
            //Mod.ghClient.Credentials = Mod.basicAuth;
            DirectoryInfo di = new DirectoryInfo(directory);
            if(di.Exists)
            {
                var modFile = Path.Combine(directory, "mod.json");
                if (File.Exists(modFile))
                {
                    using (StreamReader file = File.OpenText(modFile))
                    {
                        var modjson = file.ReadToEnd();
                        Mod tempMod = JsonConvert.DeserializeObject<Mod>(modjson);
                        if (tempMod.Version != null && tempMod.Version != "")
                        {
                            if (Semver.SemVersion.TryParse(tempMod.Version, out tempMod.SemVer) == false)
                                if(Semver.SemVersion.TryParse(tempMod.Version.Substring(1), out tempMod.SemVer) == false)
                                    tempMod.SemVer = new Semver.SemVersion(0);
                        }
                        else
                        {
                            tempMod.Version = "??";
                        }
                        tempMod.ModDirectory = di.Name;
                        tempMod.ModFullDirectory = di.FullName;
                        foreach (DirectoryInfo did in di.EnumerateDirectories())
                        {
                            tempMod.enumerateDirectoryForJson(did);
                        }
                        try
                        {
                            tempMod.fetchLatestReleaseFromWebsite();
                        }
                        catch
                        {
                            try
                            {
                                tempMod.fetchReleasesFromWebsite();
                            }
                            catch
                            {

                            }
                        }
                        if (tempMod.Name == null)
                            tempMod.Name = "??";
                        return tempMod;
                    }
                }
            }
            return null;
        }
    }
}
