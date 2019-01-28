using BMILib;
using Octokit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace BMIwFormsApp
{
    partial class BMI
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.bMIObjBindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.bMIObjBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.installedDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.needsUpdateDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.authorDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.installedVersionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.latestVersionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.maxBTVersionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.minBTVersionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.downloadDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.descriptionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewLinkColumn();
            this.installOrUpdateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewButtonColumn();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.filterTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bMIObjBindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bMIObjBindingSource)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1144, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(92, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.aboutToolStripMenuItem.Text = "&About";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripProgressBar1});
            this.statusStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.statusStrip1.Location = new System.Drawing.Point(0, 428);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1144, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(16, 17);
            this.toolStripStatusLabel1.Text = "...";
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(100, 16);
            this.toolStripProgressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            // 
            // bMIObjBindingSource1
            // 
            this.bMIObjBindingSource1.DataSource = typeof(BMIwFormsApp.BMI.BMIObj);
            // 
            // bMIObjBindingSource
            // 
            this.bMIObjBindingSource.DataSource = typeof(BMIwFormsApp.BMI.BMIObj);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 24);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1144, 404);
            this.tabControl1.TabIndex = 3;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.dataGridView1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1136, 378);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Mods";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.installedDataGridViewCheckBoxColumn,
            this.needsUpdateDataGridViewCheckBoxColumn,
            this.nameDataGridViewTextBoxColumn,
            this.authorDataGridViewTextBoxColumn,
            this.installedVersionDataGridViewTextBoxColumn,
            this.latestVersionDataGridViewTextBoxColumn,
            this.maxBTVersionDataGridViewTextBoxColumn,
            this.minBTVersionDataGridViewTextBoxColumn,
            this.downloadDataGridViewTextBoxColumn,
            this.descriptionDataGridViewTextBoxColumn,
            this.installOrUpdateDataGridViewTextBoxColumn});
            this.dataGridView1.DataSource = this.bMIObjBindingSource1;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(3, 3);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.Size = new System.Drawing.Size(1130, 372);
            this.dataGridView1.TabIndex = 1;
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            // 
            // installedDataGridViewCheckBoxColumn
            // 
            this.installedDataGridViewCheckBoxColumn.DataPropertyName = "Installed";
            this.installedDataGridViewCheckBoxColumn.HeaderText = "Installed";
            this.installedDataGridViewCheckBoxColumn.Name = "installedDataGridViewCheckBoxColumn";
            this.installedDataGridViewCheckBoxColumn.ReadOnly = true;
            this.installedDataGridViewCheckBoxColumn.Width = 52;
            // 
            // needsUpdateDataGridViewCheckBoxColumn
            // 
            this.needsUpdateDataGridViewCheckBoxColumn.DataPropertyName = "NeedsUpdate";
            this.needsUpdateDataGridViewCheckBoxColumn.HeaderText = "NeedsUpdate";
            this.needsUpdateDataGridViewCheckBoxColumn.Name = "needsUpdateDataGridViewCheckBoxColumn";
            this.needsUpdateDataGridViewCheckBoxColumn.ReadOnly = true;
            this.needsUpdateDataGridViewCheckBoxColumn.Width = 79;
            // 
            // nameDataGridViewTextBoxColumn
            // 
            this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
            this.nameDataGridViewTextBoxColumn.HeaderText = "Name";
            this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            this.nameDataGridViewTextBoxColumn.ReadOnly = true;
            this.nameDataGridViewTextBoxColumn.Width = 60;
            // 
            // authorDataGridViewTextBoxColumn
            // 
            this.authorDataGridViewTextBoxColumn.DataPropertyName = "Author";
            this.authorDataGridViewTextBoxColumn.HeaderText = "Author";
            this.authorDataGridViewTextBoxColumn.Name = "authorDataGridViewTextBoxColumn";
            this.authorDataGridViewTextBoxColumn.ReadOnly = true;
            this.authorDataGridViewTextBoxColumn.Width = 63;
            // 
            // installedVersionDataGridViewTextBoxColumn
            // 
            this.installedVersionDataGridViewTextBoxColumn.DataPropertyName = "InstalledVersion";
            this.installedVersionDataGridViewTextBoxColumn.HeaderText = "InstalledVersion";
            this.installedVersionDataGridViewTextBoxColumn.Name = "installedVersionDataGridViewTextBoxColumn";
            this.installedVersionDataGridViewTextBoxColumn.ReadOnly = true;
            this.installedVersionDataGridViewTextBoxColumn.Width = 106;
            // 
            // latestVersionDataGridViewTextBoxColumn
            // 
            this.latestVersionDataGridViewTextBoxColumn.DataPropertyName = "LatestVersion";
            this.latestVersionDataGridViewTextBoxColumn.HeaderText = "LatestVersion";
            this.latestVersionDataGridViewTextBoxColumn.Name = "latestVersionDataGridViewTextBoxColumn";
            this.latestVersionDataGridViewTextBoxColumn.ReadOnly = true;
            this.latestVersionDataGridViewTextBoxColumn.Width = 96;
            // 
            // maxBTVersionDataGridViewTextBoxColumn
            // 
            this.maxBTVersionDataGridViewTextBoxColumn.DataPropertyName = "MaxBTVersion";
            this.maxBTVersionDataGridViewTextBoxColumn.HeaderText = "MaxBTVersion";
            this.maxBTVersionDataGridViewTextBoxColumn.Name = "maxBTVersionDataGridViewTextBoxColumn";
            this.maxBTVersionDataGridViewTextBoxColumn.ReadOnly = true;
            this.maxBTVersionDataGridViewTextBoxColumn.Width = 101;
            // 
            // minBTVersionDataGridViewTextBoxColumn
            // 
            this.minBTVersionDataGridViewTextBoxColumn.DataPropertyName = "MinBTVersion";
            this.minBTVersionDataGridViewTextBoxColumn.HeaderText = "MinBTVersion";
            this.minBTVersionDataGridViewTextBoxColumn.Name = "minBTVersionDataGridViewTextBoxColumn";
            this.minBTVersionDataGridViewTextBoxColumn.ReadOnly = true;
            this.minBTVersionDataGridViewTextBoxColumn.Width = 98;
            // 
            // downloadDataGridViewTextBoxColumn
            // 
            this.downloadDataGridViewTextBoxColumn.DataPropertyName = "Download";
            this.downloadDataGridViewTextBoxColumn.HeaderText = "Download";
            this.downloadDataGridViewTextBoxColumn.Name = "downloadDataGridViewTextBoxColumn";
            this.downloadDataGridViewTextBoxColumn.ReadOnly = true;
            this.downloadDataGridViewTextBoxColumn.Width = 80;
            // 
            // descriptionDataGridViewTextBoxColumn
            // 
            this.descriptionDataGridViewTextBoxColumn.DataPropertyName = "Description";
            this.descriptionDataGridViewTextBoxColumn.HeaderText = "Description";
            this.descriptionDataGridViewTextBoxColumn.Name = "descriptionDataGridViewTextBoxColumn";
            this.descriptionDataGridViewTextBoxColumn.ReadOnly = true;
            this.descriptionDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.descriptionDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.descriptionDataGridViewTextBoxColumn.Width = 85;
            // 
            // installOrUpdateDataGridViewTextBoxColumn
            // 
            this.installOrUpdateDataGridViewTextBoxColumn.DataPropertyName = "InstallOrUpdate";
            this.installOrUpdateDataGridViewTextBoxColumn.HeaderText = "InstallOrUpdate";
            this.installOrUpdateDataGridViewTextBoxColumn.Name = "installOrUpdateDataGridViewTextBoxColumn";
            this.installOrUpdateDataGridViewTextBoxColumn.ReadOnly = true;
            this.installOrUpdateDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.installOrUpdateDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.installOrUpdateDataGridViewTextBoxColumn.Width = 105;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1136, 378);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Pending Actions";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // filterTextBox
            // 
            this.filterTextBox.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.filterTextBox.Location = new System.Drawing.Point(0, 408);
            this.filterTextBox.Name = "filterTextBox";
            this.filterTextBox.Size = new System.Drawing.Size(1144, 20);
            this.filterTextBox.TabIndex = 2;
            this.filterTextBox.TextChanged += new System.EventHandler(this.filterTextBox_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label1.Location = new System.Drawing.Point(0, 395);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Filter:";
            // 
            // BMI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1144, 450);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.filterTextBox);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "BMI";
            this.Text = "BMI - Battletech Mod Installer";
            this.Load += new System.EventHandler(this.BMI_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bMIObjBindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bMIObjBindingSource)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.BindingSource bMIObjBindingSource;
        private BindingSource bMIObjBindingSource1;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem helpToolStripMenuItem;
        private ToolStripMenuItem aboutToolStripMenuItem;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private DataGridView dataGridView1;
        private DataGridViewCheckBoxColumn installedDataGridViewCheckBoxColumn;
        private DataGridViewCheckBoxColumn needsUpdateDataGridViewCheckBoxColumn;
        private DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn authorDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn installedVersionDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn latestVersionDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn maxBTVersionDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn minBTVersionDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn downloadDataGridViewTextBoxColumn;
        private DataGridViewLinkColumn descriptionDataGridViewTextBoxColumn;
        private DataGridViewButtonColumn installOrUpdateDataGridViewTextBoxColumn;
        private TabPage tabPage2;
        private ToolStripProgressBar toolStripProgressBar1;
        private TextBox filterTextBox;
        private System.Windows.Forms.Label label1;
    }
}

