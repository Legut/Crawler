using System;
using System.Windows.Forms;

namespace Crawler.MainForm
{
    partial class MainForm
    {
        /// <summary>
        /// Wymagana zmienna projektanta.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Wyczyść wszystkie używane zasoby.
        /// </summary>
        /// <param name="disposing">prawda, jeżeli zarządzane zasoby powinny zostać zlikwidowane; Fałsz w przeciwnym wypadku.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Kod generowany przez Projektanta formularzy systemu Windows

        /// <summary>
        /// Metoda wymagana do obsługi projektanta — nie należy modyfikować
        /// jej zawartości w edytorze kodu.
        /// </summary>
        private void InitializeComponent()
        {
            this.siteAddress = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.crawlButton = new System.Windows.Forms.Button();
            this.siteToCrawlMsg = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.allTabPage = new System.Windows.Forms.TabPage();
            this.allDataGridView = new System.Windows.Forms.DataGridView();
            this.Address = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.internalTabPage = new System.Windows.Forms.TabPage();
            this.internalDataGridView = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.externalTabPage = new System.Windows.Forms.TabPage();
            this.externalDataGridView = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.titlesTabPage = new System.Windows.Forms.TabPage();
            this.pageTitlesDataGridView = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.metaDescTabPage = new System.Windows.Forms.TabPage();
            this.metaDescDataGridView = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.metaKeywordsTabPage = new System.Windows.Forms.TabPage();
            this.keywordsDataGridView = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.h1TabPage = new System.Windows.Forms.TabPage();
            this.headingOneDataGridView = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.h2TabPage = new System.Windows.Forms.TabPage();
            this.headingTwoDataGridView = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.imagesTabPage = new System.Windows.Forms.TabPage();
            this.imagesDataGridView = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label4 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer4 = new System.Windows.Forms.SplitContainer();
            this.singleDataGridView = new System.Windows.Forms.DataGridView();
            this.splitContainer5 = new System.Windows.Forms.SplitContainer();
            this.countersList = new System.Windows.Forms.ListBox();
            this.richTextBox2 = new System.Windows.Forms.RichTextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.plikToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nowyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zapiszToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.opcjeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ustawieniaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.widokToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pomocToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.oProgramieToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.label3 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.allTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.allDataGridView)).BeginInit();
            this.internalTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.internalDataGridView)).BeginInit();
            this.externalTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.externalDataGridView)).BeginInit();
            this.titlesTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pageTitlesDataGridView)).BeginInit();
            this.metaDescTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.metaDescDataGridView)).BeginInit();
            this.metaKeywordsTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.keywordsDataGridView)).BeginInit();
            this.h1TabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.headingOneDataGridView)).BeginInit();
            this.h2TabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.headingTwoDataGridView)).BeginInit();
            this.imagesTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imagesDataGridView)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).BeginInit();
            this.splitContainer4.Panel1.SuspendLayout();
            this.splitContainer4.Panel2.SuspendLayout();
            this.splitContainer4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.singleDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer5)).BeginInit();
            this.splitContainer5.Panel1.SuspendLayout();
            this.splitContainer5.Panel2.SuspendLayout();
            this.splitContainer5.SuspendLayout();
            this.panel1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // siteAddress
            // 
            this.siteAddress.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.siteAddress.Location = new System.Drawing.Point(121, 24);
            this.siteAddress.Name = "siteAddress";
            this.siteAddress.Size = new System.Drawing.Size(259, 29);
            this.siteAddress.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label1.Location = new System.Drawing.Point(9, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 24);
            this.label1.TabIndex = 1;
            this.label1.Text = "Adres URL:";
            // 
            // crawlButton
            // 
            this.crawlButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.crawlButton.Location = new System.Drawing.Point(386, 24);
            this.crawlButton.Name = "crawlButton";
            this.crawlButton.Size = new System.Drawing.Size(121, 29);
            this.crawlButton.TabIndex = 2;
            this.crawlButton.Text = "Start";
            this.crawlButton.UseVisualStyleBackColor = true;
            this.crawlButton.Click += new System.EventHandler(this.Button1_ClickAsync);
            // 
            // siteToCrawlMsg
            // 
            this.siteToCrawlMsg.AutoSize = true;
            this.siteToCrawlMsg.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.siteToCrawlMsg.Location = new System.Drawing.Point(516, 8);
            this.siteToCrawlMsg.Name = "siteToCrawlMsg";
            this.siteToCrawlMsg.Size = new System.Drawing.Size(0, 24);
            this.siteToCrawlMsg.TabIndex = 3;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.allTabPage);
            this.tabControl1.Controls.Add(this.internalTabPage);
            this.tabControl1.Controls.Add(this.externalTabPage);
            this.tabControl1.Controls.Add(this.titlesTabPage);
            this.tabControl1.Controls.Add(this.metaDescTabPage);
            this.tabControl1.Controls.Add(this.metaKeywordsTabPage);
            this.tabControl1.Controls.Add(this.h1TabPage);
            this.tabControl1.Controls.Add(this.h2TabPage);
            this.tabControl1.Controls.Add(this.imagesTabPage);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(3, 3, 0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.Padding = new System.Drawing.Point(0, 0);
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1030, 516);
            this.tabControl1.TabIndex = 4;
            this.tabControl1.Tag = "all";
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // allTabPage
            // 
            this.allTabPage.AutoScroll = true;
            this.allTabPage.Controls.Add(this.allDataGridView);
            this.allTabPage.Location = new System.Drawing.Point(4, 22);
            this.allTabPage.Name = "allTabPage";
            this.allTabPage.Size = new System.Drawing.Size(1022, 490);
            this.allTabPage.TabIndex = 0;
            this.allTabPage.Tag = "allTabPage";
            this.allTabPage.Text = "Wszystkie";
            this.allTabPage.UseVisualStyleBackColor = true;
            // 
            // allDataGridView
            // 
            this.allDataGridView.BackgroundColor = System.Drawing.SystemColors.Window;
            this.allDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.allDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Address});
            this.allDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.allDataGridView.Location = new System.Drawing.Point(0, 0);
            this.allDataGridView.Name = "allDataGridView";
            this.allDataGridView.Size = new System.Drawing.Size(1022, 490);
            this.allDataGridView.TabIndex = 3;
            this.allDataGridView.MouseClick += new System.Windows.Forms.MouseEventHandler(this.DataGridView_MouseClick);
            // 
            // Address
            // 
            this.Address.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Address.HeaderText = "Address";
            this.Address.Name = "Address";
            // 
            // internalTabPage
            // 
            this.internalTabPage.Controls.Add(this.internalDataGridView);
            this.internalTabPage.Location = new System.Drawing.Point(4, 22);
            this.internalTabPage.Name = "internalTabPage";
            this.internalTabPage.Size = new System.Drawing.Size(1022, 490);
            this.internalTabPage.TabIndex = 2;
            this.internalTabPage.Tag = "internalTabPage";
            this.internalTabPage.Text = "Wewnętrzne";
            this.internalTabPage.UseVisualStyleBackColor = true;
            // 
            // internalDataGridView
            // 
            this.internalDataGridView.BackgroundColor = System.Drawing.SystemColors.Window;
            this.internalDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.internalDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1});
            this.internalDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.internalDataGridView.Location = new System.Drawing.Point(0, 0);
            this.internalDataGridView.Name = "internalDataGridView";
            this.internalDataGridView.Size = new System.Drawing.Size(1022, 490);
            this.internalDataGridView.TabIndex = 4;
            this.internalDataGridView.Tag = "internal";
            this.internalDataGridView.MouseClick += new System.Windows.Forms.MouseEventHandler(this.DataGridView_MouseClick);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn1.HeaderText = "Address";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            // 
            // externalTabPage
            // 
            this.externalTabPage.Controls.Add(this.externalDataGridView);
            this.externalTabPage.Location = new System.Drawing.Point(4, 22);
            this.externalTabPage.Name = "externalTabPage";
            this.externalTabPage.Size = new System.Drawing.Size(1022, 490);
            this.externalTabPage.TabIndex = 1;
            this.externalTabPage.Tag = "externalTabPage";
            this.externalTabPage.Text = "Zewnętrzne";
            this.externalTabPage.UseVisualStyleBackColor = true;
            // 
            // externalDataGridView
            // 
            this.externalDataGridView.BackgroundColor = System.Drawing.SystemColors.Window;
            this.externalDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.externalDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn2});
            this.externalDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.externalDataGridView.Location = new System.Drawing.Point(0, 0);
            this.externalDataGridView.Name = "externalDataGridView";
            this.externalDataGridView.Size = new System.Drawing.Size(1022, 490);
            this.externalDataGridView.TabIndex = 4;
            this.externalDataGridView.Tag = "external";
            this.externalDataGridView.MouseClick += new System.Windows.Forms.MouseEventHandler(this.DataGridView_MouseClick);
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn2.HeaderText = "Address";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            // 
            // titlesTabPage
            // 
            this.titlesTabPage.Controls.Add(this.pageTitlesDataGridView);
            this.titlesTabPage.Location = new System.Drawing.Point(4, 22);
            this.titlesTabPage.Name = "titlesTabPage";
            this.titlesTabPage.Size = new System.Drawing.Size(1022, 490);
            this.titlesTabPage.TabIndex = 3;
            this.titlesTabPage.Tag = "titlesTabPage";
            this.titlesTabPage.Text = "Tytuły podstron";
            this.titlesTabPage.UseVisualStyleBackColor = true;
            // 
            // pageTitlesDataGridView
            // 
            this.pageTitlesDataGridView.BackgroundColor = System.Drawing.SystemColors.Window;
            this.pageTitlesDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.pageTitlesDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn3});
            this.pageTitlesDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pageTitlesDataGridView.Location = new System.Drawing.Point(0, 0);
            this.pageTitlesDataGridView.Name = "pageTitlesDataGridView";
            this.pageTitlesDataGridView.Size = new System.Drawing.Size(1022, 490);
            this.pageTitlesDataGridView.TabIndex = 5;
            this.pageTitlesDataGridView.Tag = "pageTitles";
            this.pageTitlesDataGridView.MouseClick += new System.Windows.Forms.MouseEventHandler(this.DataGridView_MouseClick);
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn3.HeaderText = "Address";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            // 
            // metaDescTabPage
            // 
            this.metaDescTabPage.Controls.Add(this.metaDescDataGridView);
            this.metaDescTabPage.Location = new System.Drawing.Point(4, 22);
            this.metaDescTabPage.Name = "metaDescTabPage";
            this.metaDescTabPage.Size = new System.Drawing.Size(1022, 490);
            this.metaDescTabPage.TabIndex = 4;
            this.metaDescTabPage.Tag = "metaDescTabPage";
            this.metaDescTabPage.Text = "Opisy meta";
            this.metaDescTabPage.UseVisualStyleBackColor = true;
            // 
            // metaDescDataGridView
            // 
            this.metaDescDataGridView.BackgroundColor = System.Drawing.SystemColors.Window;
            this.metaDescDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.metaDescDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn4});
            this.metaDescDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.metaDescDataGridView.Location = new System.Drawing.Point(0, 0);
            this.metaDescDataGridView.Name = "metaDescDataGridView";
            this.metaDescDataGridView.Size = new System.Drawing.Size(1022, 490);
            this.metaDescDataGridView.TabIndex = 5;
            this.metaDescDataGridView.Tag = "metaDesc";
            this.metaDescDataGridView.MouseClick += new System.Windows.Forms.MouseEventHandler(this.DataGridView_MouseClick);
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn4.HeaderText = "Address";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            // 
            // metaKeywordsTabPage
            // 
            this.metaKeywordsTabPage.Controls.Add(this.keywordsDataGridView);
            this.metaKeywordsTabPage.Location = new System.Drawing.Point(4, 22);
            this.metaKeywordsTabPage.Name = "metaKeywordsTabPage";
            this.metaKeywordsTabPage.Size = new System.Drawing.Size(1022, 490);
            this.metaKeywordsTabPage.TabIndex = 5;
            this.metaKeywordsTabPage.Tag = "metaKeywordsTabPage";
            this.metaKeywordsTabPage.Text = "Słowa kluczowe";
            this.metaKeywordsTabPage.UseVisualStyleBackColor = true;
            // 
            // keywordsDataGridView
            // 
            this.keywordsDataGridView.BackgroundColor = System.Drawing.SystemColors.Window;
            this.keywordsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.keywordsDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn5});
            this.keywordsDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.keywordsDataGridView.Location = new System.Drawing.Point(0, 0);
            this.keywordsDataGridView.Name = "keywordsDataGridView";
            this.keywordsDataGridView.Size = new System.Drawing.Size(1022, 490);
            this.keywordsDataGridView.TabIndex = 5;
            this.keywordsDataGridView.Tag = "keywords";
            this.keywordsDataGridView.MouseClick += new System.Windows.Forms.MouseEventHandler(this.DataGridView_MouseClick);
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn5.HeaderText = "Address";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            // 
            // h1TabPage
            // 
            this.h1TabPage.Controls.Add(this.headingOneDataGridView);
            this.h1TabPage.Location = new System.Drawing.Point(4, 22);
            this.h1TabPage.Name = "h1TabPage";
            this.h1TabPage.Size = new System.Drawing.Size(1022, 490);
            this.h1TabPage.TabIndex = 6;
            this.h1TabPage.Tag = "h1TabPage";
            this.h1TabPage.Text = "H1";
            this.h1TabPage.UseVisualStyleBackColor = true;
            // 
            // headingOneDataGridView
            // 
            this.headingOneDataGridView.BackgroundColor = System.Drawing.SystemColors.Window;
            this.headingOneDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.headingOneDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn6});
            this.headingOneDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.headingOneDataGridView.Location = new System.Drawing.Point(0, 0);
            this.headingOneDataGridView.Name = "headingOneDataGridView";
            this.headingOneDataGridView.Size = new System.Drawing.Size(1022, 490);
            this.headingOneDataGridView.TabIndex = 5;
            this.headingOneDataGridView.Tag = "headingOne";
            this.headingOneDataGridView.MouseClick += new System.Windows.Forms.MouseEventHandler(this.DataGridView_MouseClick);
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn6.HeaderText = "Address";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            // 
            // h2TabPage
            // 
            this.h2TabPage.Controls.Add(this.headingTwoDataGridView);
            this.h2TabPage.Location = new System.Drawing.Point(4, 22);
            this.h2TabPage.Name = "h2TabPage";
            this.h2TabPage.Size = new System.Drawing.Size(1022, 490);
            this.h2TabPage.TabIndex = 7;
            this.h2TabPage.Tag = "h2TabPage";
            this.h2TabPage.Text = "H2";
            this.h2TabPage.UseVisualStyleBackColor = true;
            // 
            // headingTwoDataGridView
            // 
            this.headingTwoDataGridView.BackgroundColor = System.Drawing.SystemColors.Window;
            this.headingTwoDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.headingTwoDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn7});
            this.headingTwoDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.headingTwoDataGridView.Location = new System.Drawing.Point(0, 0);
            this.headingTwoDataGridView.Name = "headingTwoDataGridView";
            this.headingTwoDataGridView.Size = new System.Drawing.Size(1022, 490);
            this.headingTwoDataGridView.TabIndex = 5;
            this.headingTwoDataGridView.Tag = "headingTwo";
            this.headingTwoDataGridView.MouseClick += new System.Windows.Forms.MouseEventHandler(this.DataGridView_MouseClick);
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn7.HeaderText = "Address";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            // 
            // imagesTabPage
            // 
            this.imagesTabPage.Controls.Add(this.imagesDataGridView);
            this.imagesTabPage.Location = new System.Drawing.Point(4, 22);
            this.imagesTabPage.Name = "imagesTabPage";
            this.imagesTabPage.Size = new System.Drawing.Size(1022, 490);
            this.imagesTabPage.TabIndex = 8;
            this.imagesTabPage.Tag = "imagesTabPage";
            this.imagesTabPage.Text = "Obrazki";
            this.imagesTabPage.UseVisualStyleBackColor = true;
            // 
            // imagesDataGridView
            // 
            this.imagesDataGridView.BackgroundColor = System.Drawing.SystemColors.Window;
            this.imagesDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.imagesDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn8});
            this.imagesDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imagesDataGridView.Location = new System.Drawing.Point(0, 0);
            this.imagesDataGridView.Name = "imagesDataGridView";
            this.imagesDataGridView.Size = new System.Drawing.Size(1022, 490);
            this.imagesDataGridView.TabIndex = 5;
            this.imagesDataGridView.Tag = "images";
            this.imagesDataGridView.MouseClick += new System.Windows.Forms.MouseEventHandler(this.DataGridView_MouseClick);
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn8.HeaderText = "Address";
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 73);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(120, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Strony do przeszukania:";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 1290F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 164F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.splitContainer1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.AddColumns;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 65F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 465F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1290, 805);
            this.tableLayoutPanel1.TabIndex = 10;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 68);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer4);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer5);
            this.splitContainer1.Size = new System.Drawing.Size(1284, 734);
            this.splitContainer1.SplitterDistance = 1030;
            this.splitContainer1.TabIndex = 8;
            // 
            // splitContainer4
            // 
            this.splitContainer4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer4.Location = new System.Drawing.Point(0, 0);
            this.splitContainer4.Name = "splitContainer4";
            this.splitContainer4.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer4.Panel1
            // 
            this.splitContainer4.Panel1.Controls.Add(this.tabControl1);
            // 
            // splitContainer4.Panel2
            // 
            this.splitContainer4.Panel2.BackColor = System.Drawing.SystemColors.Window;
            this.splitContainer4.Panel2.Controls.Add(this.singleDataGridView);
            this.splitContainer4.Panel2.Padding = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.splitContainer4.Size = new System.Drawing.Size(1030, 734);
            this.splitContainer4.SplitterDistance = 516;
            this.splitContainer4.TabIndex = 9;
            // 
            // singleDataGridView
            // 
            this.singleDataGridView.BackgroundColor = System.Drawing.SystemColors.Window;
            this.singleDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.singleDataGridView.Dock = System.Windows.Forms.DockStyle.Top;
            this.singleDataGridView.Location = new System.Drawing.Point(3, 0);
            this.singleDataGridView.Margin = new System.Windows.Forms.Padding(6, 6, 3, 6);
            this.singleDataGridView.Name = "singleDataGridView";
            this.singleDataGridView.Size = new System.Drawing.Size(1024, 268);
            this.singleDataGridView.TabIndex = 6;
            // 
            // splitContainer5
            // 
            this.splitContainer5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer5.Location = new System.Drawing.Point(0, 0);
            this.splitContainer5.Name = "splitContainer5";
            this.splitContainer5.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer5.Panel1
            // 
            this.splitContainer5.Panel1.Controls.Add(this.countersList);
            this.splitContainer5.Panel1.Padding = new System.Windows.Forms.Padding(0, 3, 3, 3);
            // 
            // splitContainer5.Panel2
            // 
            this.splitContainer5.Panel2.Controls.Add(this.richTextBox2);
            this.splitContainer5.Panel2.Padding = new System.Windows.Forms.Padding(0, 0, 3, 3);
            this.splitContainer5.Size = new System.Drawing.Size(250, 734);
            this.splitContainer5.SplitterDistance = 468;
            this.splitContainer5.TabIndex = 10;
            // 
            // countersList
            // 
            this.countersList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.countersList.FormattingEnabled = true;
            this.countersList.Location = new System.Drawing.Point(0, 3);
            this.countersList.Name = "countersList";
            this.countersList.Size = new System.Drawing.Size(247, 462);
            this.countersList.TabIndex = 13;
            this.countersList.Tag = "countersList";
            // 
            // richTextBox2
            // 
            this.richTextBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox2.Location = new System.Drawing.Point(0, 0);
            this.richTextBox2.Name = "richTextBox2";
            this.richTextBox2.Size = new System.Drawing.Size(247, 259);
            this.richTextBox2.TabIndex = 8;
            this.richTextBox2.Text = "";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.siteAddress);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.crawlButton);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.menuStrip1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1287, 59);
            this.panel1.TabIndex = 5;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.plikToolStripMenuItem,
            this.opcjeToolStripMenuItem,
            this.widokToolStripMenuItem,
            this.pomocToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1287, 24);
            this.menuStrip1.TabIndex = 10;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // plikToolStripMenuItem
            // 
            this.plikToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.nowyToolStripMenuItem,
            this.zapiszToolStripMenuItem});
            this.plikToolStripMenuItem.Name = "plikToolStripMenuItem";
            this.plikToolStripMenuItem.Size = new System.Drawing.Size(38, 20);
            this.plikToolStripMenuItem.Text = "Plik";
            // 
            // nowyToolStripMenuItem
            // 
            this.nowyToolStripMenuItem.Name = "nowyToolStripMenuItem";
            this.nowyToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.nowyToolStripMenuItem.Text = "Nowy";
            // 
            // zapiszToolStripMenuItem
            // 
            this.zapiszToolStripMenuItem.Name = "zapiszToolStripMenuItem";
            this.zapiszToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.zapiszToolStripMenuItem.Text = "Zapisz";
            this.zapiszToolStripMenuItem.Click += new System.EventHandler(this.Save_StripMenuItem_Click);
            // 
            // opcjeToolStripMenuItem
            // 
            this.opcjeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ustawieniaToolStripMenuItem});
            this.opcjeToolStripMenuItem.Name = "opcjeToolStripMenuItem";
            this.opcjeToolStripMenuItem.Size = new System.Drawing.Size(50, 20);
            this.opcjeToolStripMenuItem.Text = "Opcje";
            // 
            // ustawieniaToolStripMenuItem
            // 
            this.ustawieniaToolStripMenuItem.Name = "ustawieniaToolStripMenuItem";
            this.ustawieniaToolStripMenuItem.Size = new System.Drawing.Size(131, 22);
            this.ustawieniaToolStripMenuItem.Text = "Ustawienia";
            this.ustawieniaToolStripMenuItem.Click += new System.EventHandler(this.Settings_StripMenuItem_Click);
            // 
            // widokToolStripMenuItem
            // 
            this.widokToolStripMenuItem.Name = "widokToolStripMenuItem";
            this.widokToolStripMenuItem.Size = new System.Drawing.Size(53, 20);
            this.widokToolStripMenuItem.Text = "Widok";
            // 
            // pomocToolStripMenuItem
            // 
            this.pomocToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.oProgramieToolStripMenuItem});
            this.pomocToolStripMenuItem.Name = "pomocToolStripMenuItem";
            this.pomocToolStripMenuItem.Size = new System.Drawing.Size(57, 20);
            this.pomocToolStripMenuItem.Text = "Pomoc";
            // 
            // oProgramieToolStripMenuItem
            // 
            this.oProgramieToolStripMenuItem.Name = "oProgramieToolStripMenuItem";
            this.oProgramieToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
            this.oProgramieToolStripMenuItem.Text = "O programie";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(535, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Preferencje";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1290, 805);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.siteToCrawlMsg);
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "MainForm";
            this.Text = "Crawler";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabControl1.ResumeLayout(false);
            this.allTabPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.allDataGridView)).EndInit();
            this.internalTabPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.internalDataGridView)).EndInit();
            this.externalTabPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.externalDataGridView)).EndInit();
            this.titlesTabPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pageTitlesDataGridView)).EndInit();
            this.metaDescTabPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.metaDescDataGridView)).EndInit();
            this.metaKeywordsTabPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.keywordsDataGridView)).EndInit();
            this.h1TabPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.headingOneDataGridView)).EndInit();
            this.h2TabPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.headingTwoDataGridView)).EndInit();
            this.imagesTabPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.imagesDataGridView)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer4.Panel1.ResumeLayout(false);
            this.splitContainer4.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).EndInit();
            this.splitContainer4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.singleDataGridView)).EndInit();
            this.splitContainer5.Panel1.ResumeLayout(false);
            this.splitContainer5.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer5)).EndInit();
            this.splitContainer5.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        #endregion

        private System.Windows.Forms.TextBox siteAddress;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button crawlButton;
        private System.Windows.Forms.Label siteToCrawlMsg;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage allTabPage;
        private System.Windows.Forms.TabPage externalTabPage;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DataGridView allDataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn Address;
        private System.Windows.Forms.TabPage internalTabPage;
        private System.Windows.Forms.DataGridView internalDataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridView externalDataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView singleDataGridView;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private FolderBrowserDialog folderBrowserDialog1;
        private SaveFileDialog saveFileDialog1;
        private SplitContainer splitContainer1;
        private SplitContainer splitContainer4;
        private SplitContainer splitContainer5;
        private RichTextBox richTextBox2;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem plikToolStripMenuItem;
        private ToolStripMenuItem nowyToolStripMenuItem;
        private ToolStripMenuItem zapiszToolStripMenuItem;
        private ToolStripMenuItem opcjeToolStripMenuItem;
        private ToolStripMenuItem widokToolStripMenuItem;
        private ToolStripMenuItem pomocToolStripMenuItem;
        private ToolStripMenuItem oProgramieToolStripMenuItem;
        private ToolStripMenuItem ustawieniaToolStripMenuItem;
        private Label label3;
        private TabPage titlesTabPage;
        private TabPage metaDescTabPage;
        private TabPage metaKeywordsTabPage;
        private TabPage h1TabPage;
        private TabPage h2TabPage;
        private TabPage imagesTabPage;
        private DataGridView pageTitlesDataGridView;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private DataGridView metaDescDataGridView;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private DataGridView keywordsDataGridView;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private DataGridView headingOneDataGridView;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private DataGridView headingTwoDataGridView;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private DataGridView imagesDataGridView;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private ListBox countersList;
    }
}

