using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BMILib;
using Octokit;

namespace WindowsFormsApp1
{
    public partial class BMI : Form
    {
        public List<BMIObj> data = new List<BMIObj>();
        public class BMIObj
        {
            public bool Installed { get; set; }
            public bool NeedsUpdate { get; set; }
            public string Name { get; set; }
            public string Author { get; set; }
            public string InstalledVersion { get; set; }
            public string LatestVersion{ get; set; }
            public string MaxBTVersion{ get; set; }
            public string MinBTVersion{ get; set; }
            public string Download{ get; set; }
            public string Description{ get; set; }
            public string InstallOrUpdate{ get; set; }

            public static BMIObj FromMod(BMILib.Mod m)
            {
                BMIObj ret = new BMIObj();

                ret.Installed = m.ModFullDirectory != null ? true : m.IsInstalled();
                ret.NeedsUpdate = ret.Installed ? m.NeedsUpdate() : false;
                ret.Name = m.Name;
                ret.Author = m.Author;
                ret.InstalledVersion = m.Version ?? "-";
                ret.LatestVersion = m.LatestRelease?.TagName ?? "-";
                ret.MinBTVersion = "-";
                ret.MaxBTVersion = "-";
                ret.Download = "- Mb";
                ret.Description = m.Website ?? "-";
                ret.InstallOrUpdate = ret.Installed ? m.NeedsUpdate() ? "Update" : "Uninstall" : "Install";

                return ret;
            }
        }

        public BMI()
        {
            InitializeComponent();

            // Double buffering can make DGV slow in remote desktop
            if (!System.Windows.Forms.SystemInformation.TerminalServerSession)
            {
                Type dgvType = dataGridView1.GetType();
                PropertyInfo pi = dgvType.GetProperty("DoubleBuffered",
                  BindingFlags.Instance | BindingFlags.NonPublic);
                pi.SetValue(dataGridView1, true, null);
            }
        }

        public static DataTable CreateDataTable<T>(IEnumerable<T> list)
        {
            Type type = typeof(T);
            var properties = type.GetProperties();

            DataTable dataTable = new DataTable();
            foreach (PropertyInfo info in properties)
            {
                dataTable.Columns.Add(new DataColumn(info.Name, Nullable.GetUnderlyingType(info.PropertyType) ?? info.PropertyType));
            }

            foreach (T entity in list)
            {
                object[] values = new object[properties.Length];
                for (int i = 0; i < properties.Length; i++)
                {
                    values[i] = properties[i].GetValue(entity);
                }

                dataTable.Rows.Add(values);
            }

            return dataTable;
        }

        BindingSource bs = new BindingSource();
        List<Mod> mods = new List<Mod>();

        public static void RefreshModList(IProgress<LoadProgress> progress, WindowsFormsApp1.BMI form)
        {

            form.mods.Clear();
            progress.Report(new LoadProgress { Text = "Loading mods from local directory...", Percentage = 0 });
            System.IO.DirectoryInfo di = new DirectoryInfo(@".");
            var dirs = di.EnumerateDirectories();
            var dirsCount = dirs.Count();
            int curCount = 0;
            foreach (DirectoryInfo d in dirs)
            {
                progress.Report(new LoadProgress { Text = "Loading mods from local directory...", Percentage = 33 * (curCount/dirsCount) });
                try
                {
                    Mod m = Mod.LoadFromDirectory(d.FullName);
                    if (m != null)
                        form.mods.Add(m);
                    //else
                    //    Console.WriteLine($"{d.Name} didn't load right!");
                }
                catch (Exception ext)
                {
                    throw;
                }
                finally { curCount++;  }
            }

            progress.Report(new LoadProgress { Text = "Loading Mods from BMI Index...", Percentage = 33 });

            BMILib.IndexClient.Initialize();
            var modListCount = BMILib.IndexClient.ModList.Count();
            var modListCurrentCount = 0;
            foreach (Mod m in BMILib.IndexClient.ModList.Values)
            {
                progress.Report(new LoadProgress { Text = "Loading Mods from BMI Index...", Percentage = 33 + (33 * (modListCurrentCount / modListCount)) });
                modListCurrentCount++;
                if (form.mods.Where(mm => mm.Name == m.Name).Count() > 0)
                    continue;
                form.mods.Add(m);

            }

            //bs.DataSource = typeof(BMIObj);

            progress.Report(new LoadProgress { Text = "Fetching Most Recent Mod Version for all mods...", Percentage = 66 });

            var totalModCount = form.mods.Count();
            var currentModCount = 0;
            foreach (Mod m in form.mods.OrderBy(m => m.Name))
            {
                progress.Report(new LoadProgress { Text = "Fetching Most Recent Mod Version for all mods...", Percentage = 66 + (33 * (currentModCount / totalModCount)) });
                currentModCount++;
                if (m == null)
                    continue;
                m.fetchReleasesFromWebsite();
                //bs.Add(BMIObj.FromMod(m));
            }

            form.bs.DataSource = WindowsFormsApp1.BMI.CreateDataTable<BMIObj>(form.mods.Select(m => BMIObj.FromMod(m)).ToList());
            form.dataGridView1.DataSource = form.bs;
            form.dataGridView1.AutoGenerateColumns = false;
            (form.bs.DataSource as DataTable).DefaultView.RowFilter = string.Format("Name LIKE '%{0}%' OR Author LIKE '%{0}%'", form.filterTextBox.Text);
        }

        public class LoadProgress
        {
            public int Percentage { get; set; }
            public string Text { get; set;  }
        }
        private async void BMI_Load(object sender, EventArgs e)
        {
            var progress = new Progress<LoadProgress>(p  => { toolStripStatusLabel1.Text = p.Text ; toolStripProgressBar1.Value = p.Percentage; } );
            await Task.Factory.StartNew(() => RefreshModList(progress,this), TaskCreationOptions.LongRunning);
            toolStripStatusLabel1.Text = "Done!";
            toolStripProgressBar1.Value = 100;
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            //foreach (DataGridViewRow r in dataGridView1.Rows)
            //{
            //    if (System.Uri.IsWellFormedUriString(r.Cells["Description"].Value.ToString(), UriKind.Absolute))
            //    {
            //        r.Cells["Description"] = new DataGridViewLinkCell();
            //        // Note that if I want a different link colour for example it must go here
            //        DataGridViewLinkCell c = r.Cells["Description"] as DataGridViewLinkCell;
            //        c.LinkColor = Color.Blue;
            //    }
            //}
        }

        private void dataGridView1_DataSourceChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow r in dataGridView1.Rows)
            {
                //if (System.Uri.IsWellFormedUriString(r.Cells["Description"].Value.ToString(), UriKind.Absolute))
                //{
                //    r.Cells["Description"] = new DataGridViewLinkCell();
                //    // Note that if I want a different link colour for example it must go here
                //    DataGridViewLinkCell c = r.Cells["Description"] as DataGridViewLinkCell;
                //    c.LinkColor = Color.Blue;
                //}
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
                return;
            try
            {
                var ioru = (dataGridView1[e.ColumnIndex, e.RowIndex] as DataGridViewButtonCell).Value.ToString();
                try
                {
                    switch (ioru)
                    {
                        case "Install":
                            Mod mi = mods.Where(mm => mm.Name == dataGridView1[2, e.RowIndex].Value.ToString()).First();
                            toolStripStatusLabel1.Text = $"Installing {mi.Name}";
                            this.Refresh();
                            mi.Install();
                            toolStripStatusLabel1.Text = "Done!";
                            this.Refresh();
                            BMI_Load(sender, null);
                            break;
                        case "Uninstall":
                            Mod mui = mods.Where(mm => mm.Name == dataGridView1[2, e.RowIndex].Value.ToString()).First();
                            toolStripStatusLabel1.Text = $"Uninstalling {mui.Name}";
                            this.Refresh();
                            DirectoryInfo di = new DirectoryInfo(mui.ModFullDirectory);
                            di.Delete(true);
                            toolStripStatusLabel1.Text = "Done!";
                            this.Refresh();
                            BMI_Load(sender, null);
                            break;
                        case "Update":
                            Mod mu = mods.Where(mm => mm.Name == dataGridView1[2, e.RowIndex].Value.ToString()).First();
                            toolStripStatusLabel1.Text = $"Updating {mu.Name}";
                            this.Refresh();
                            mu.Update();
                            toolStripStatusLabel1.Text = "Done!";
                            this.Refresh();
                            BMI_Load(sender, null);
                            break;
                    }
                }
                catch(Exception exx)
                {
                    MessageBox.Show(exx.ToString());
                }
            }
            catch(Exception ex)
            {
                
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void filterTextBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                (this.bs.DataSource as DataTable).DefaultView.RowFilter = string.Format("Name LIKE '%{0}%' OR Author LIKE '%{0}%'", this.filterTextBox.Text);
            }
            catch
            {

            }
        }
    }
}
