using System;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Windows.Input;
using Crawler.Utilities;
using Crawler.Base;
using System.Drawing;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Crawler.MainForm
{
    public partial class MainForm : Form
    {
        private bool isCrawling;
        private Base.Crawler crawler;

        private DataTable singleRowDataTable;
        private bool pageExists;
        private bool pageHasCerificate;

        public MainForm()
        {
            InitializeComponent();
            Utils.LoadSettingsFromFile();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            allDataGridView.ColumnCount = 0;
            internalDataGridView.ColumnCount = 0;
            externalDataGridView.ColumnCount = 0;
            pageTitlesDataGridView.ColumnCount = 0;
            metaDescDataGridView.ColumnCount = 0;
            keywordsDataGridView.ColumnCount = 0;
            headingOneDataGridView.ColumnCount = 0;
            headingTwoDataGridView.ColumnCount = 0;
            imagesDataGridView.ColumnCount = 0;

            allDataGridView.CellFormatting += this.DataGridViewCellFormating;
            internalDataGridView.CellFormatting += this.DataGridViewCellFormating;
            externalDataGridView.CellFormatting += this.DataGridViewCellFormating;
            imagesDataGridView.CellFormatting += this.DataGridViewCellFormating;

            allDataGridView.ReadOnly = true;
            internalDataGridView.ReadOnly = true;
            externalDataGridView.ReadOnly = true;
            pageTitlesDataGridView.ReadOnly = true;
            metaDescDataGridView.ReadOnly = true;
            keywordsDataGridView.ReadOnly = true;
            headingOneDataGridView.ReadOnly = true;
            headingTwoDataGridView.ReadOnly = true;
            imagesDataGridView.ReadOnly = true;

            ConfigureSingleDataGridView();
            isCrawling = false;
        }
        private async void Button1_ClickAsync(object sender, EventArgs e)
        {
            crawlButton.Enabled = false;
            if (isCrawling == false)
            {
                isCrawling = true;
                if (siteAddress.Text.Length > 0)
                {
                    EnsureThatProtocolIsProvided(siteAddress.Text);
                    await PageExists(siteAddress.Text);
                    if (pageExists)
                    {
                        await PageHasCertificate(siteAddress.Text);
                        siteToCrawlMsg.Text = pageHasCerificate ? "Istnieje i ma certyfikat" : "Istnieje i nie ma certyfikatu";
                        crawler = new Base.Crawler(this, siteAddress.Text);
                        crawler.StartCrawl();
                        crawlButton.Text = "Stop";
                    }
                }
                else
                {
                    CustomMessages.DisplayWrongUrlMsg();
                    isCrawling = false;
                }
            }
            else
            {
                this.isCrawling = false;
                if (crawler != null)
                {
                    this.crawlButton.Text = "Zatrzymywanie";
                    siteToCrawlMsg.Text = "Zatrzymywanie crawlowania";
                    crawler.AbortCrawl();
                }

                this.crawlButton.Text = "Start";
            }
            crawlButton.Enabled = true;
        }
        private void EnsureThatProtocolIsProvided(string url)
        {
            // Ensure there is protocol provided at the begining of url
            if (!url.StartsWith("http://") && !url.StartsWith("https://"))
            {
                siteAddress.Text = "http://" + url;
            }
        }
        private async Task PageExists(string url)
        {
            try
            {
                // Check if there is response from given url
                if (WebRequest.Create(url) is HttpWebRequest request)
                {
                    request.Method = "HEAD";
                    HttpWebResponse response = await request.GetResponseAsync() as HttpWebResponse;
                    response?.Close();
                }

                pageExists = true;
            }
            catch
            {
                CustomMessages.DisplayPageDoesntExistMsg();
                isCrawling = false;
                pageExists = false;
            }
        }
        private async Task PageHasCertificate(string url)
        {
            // if site has certificate than it will load by https://
            url = url.Replace("http://", "https://");
            try
            {
                if (WebRequest.Create(url) is HttpWebRequest request)
                {
                    request.Method = "HEAD";
                    HttpWebResponse response = await request.GetResponseAsync() as HttpWebResponse;
                    response?.Close();
                }

                siteAddress.Text = url;
                pageHasCerificate = true;
            }
            catch (NullReferenceException exception)
            {
                CustomMessages.DisplayCustomErrorMsg("NullReferenceException", exception.Message);
                pageHasCerificate = false;
            }
            catch (Exception exception)
            {
                CustomMessages.DisplayCustomErrorMsg("UnrecognizedException", exception.Message);
                pageHasCerificate = false;
            }

            // TODO: Obsługa poszczególnych wyjątków, tak aby poinformować użytkownika nie tylko o tym, że nie udało się nawiązać połączenia, ale również czemu to się nie udało 
        }
        public void UpdateCrawlingStatus(int status, int max)
        {
            crawlingStatusLabel.Text = status + " / " + max;
        }
        public void UpdateCrawledStatus(int crawled, int all)
        {
            crawledStatusLabel.Text = crawled + " / " + all;
        }
        public void MakeButtonReady()
        {
            this.isCrawling = false;
            this.crawlButton.Text = "Start";
            this.crawlButton.Enabled = true;
        }
        private void DataGridView_SelectionChanged(object sender, EventArgs e)
        {
            // recognize whether selection changed and make changes to singleDataGridView
            DataGridView view = sender as DataGridView;
            PrepareSingleDataGridView(view);

            if (view == null) return;
            if (view.SelectedCells.Count >= 1)
            {
                DataGridViewRow selectedRow = view.Rows[view.SelectedCells[0].RowIndex];

                int corrector = 0; 
                for (int index = 0; index < selectedRow.Cells.Count; index++)
                {
                    try
                    {
                        if (selectedRow.Cells[index].OwningColumn.Visible)
                        {
                            singleRowDataTable.Rows[index-corrector].SetField(1, selectedRow.Cells[index].Value);
                        }
                        else
                        {
                            corrector++;
                        }
                    }
                    catch
                    {
                        CustomMessages.DisplayCustomErrorMsg("Something went wrong while displaying selected row in single row view -> DataGridView_SelectionChanged()", "Single row view Error");
                        break;
                    }
                }                         
            }
        }
        private void DataGridViewCellFormating(object sender, DataGridViewCellFormattingEventArgs e)
        {
            HumanReadableSizes(sender, e);
        }
        private void PrepareSingleDataGridView(DataGridView view)
        {
            singleRowDataTable.Clear();
            foreach (DataGridViewColumn column in view.Columns)
            {
                if (column.Visible)
                {
                    string[] row = { column.Name, "" };
                    singleRowDataTable.Rows.Add(row);
                }
            }
        }
        private void ConfigureSingleDataGridView()
        {
            Padding margin = new Padding(6, 6, 3, 6);
            singleDataGridView.ReadOnly = true;
            singleDataGridView.Dock = DockStyle.Fill;
            singleDataGridView.Margin = margin;
            singleDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            singleDataGridView.RowHeadersWidth = 41;
            singleDataGridView.ScrollBars = ScrollBars.Both;

            singleRowDataTable = new DataTable();
            singleRowDataTable.Columns.Add("Nazwa").DefaultValue = "";
            singleRowDataTable.Columns.Add("Wartość").DefaultValue = "";
            singleDataGridView.DataSource = singleRowDataTable;
            singleDataGridView.Columns[0].Width = 100;
        }
        private void HumanReadableSizes(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Convert sizes in bytes to readable sizes in kb, mg, gb etc.
            DataGridView view = sender as DataGridView;
            if (view.Columns[e.ColumnIndex].Name.Equals(Base.Crawler.SIZE_COL))
            {
                e.Value = ShownSize(true, e.Value.ToString());
                e.FormattingApplied = true;
            }
        }
        public object ShownSize(bool isInternal, string size)
        {
            long temp = long.Parse(size);
            return isInternal ? Base.Crawler.SizeSuffix(temp, 2) : String.Empty;
        }
        private void DataGridView_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            DataGridView view = sender as DataGridView;
            if (e.Button == MouseButtons.Right)
            {
                if (view.SelectedCells.Count > 0)
                {
                    ContextMenuStrip cellClickMenu = new ContextMenuStrip();
                    int hit_row = view.HitTest(e.X, e.Y).RowIndex;

                    if (hit_row >= 0)
                    {
                        cellClickMenu.Items.Add("Kopiuj komórkę").Tag = "Copy cell";
                        cellClickMenu.Items.Add("Kopiuj zaznaczenie").Tag = "Copy selected";
                        cellClickMenu.Items.Add("Otwórz w przeglądarce").Tag = "Open in browser";
                        if (view.SelectedRows.Count > 0)
                        {
                            cellClickMenu.Items.Add("Zapisz rzędy do CSV").Tag = "Save to csv";
                        }
                        
                    }

                    cellClickMenu.Show(view, new Point(e.X, e.Y));
                    cellClickMenu.ItemClicked += DataViewCellClicked;
                    
                }
            }
        }
        private void DataViewCellClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            string temp = "";
            ContextMenuStrip menuStrip = (ContextMenuStrip) sender;
            DataGridView view = (DataGridView) menuStrip.SourceControl;
            menuStrip.Close();

            switch (e.ClickedItem.Tag.ToString())
            {
                case "Copy cell":
                    temp = view.SelectedCells[0].Value.ToString();                    
                    Clipboard.SetText(temp);
                    break;

                case "Copy selected":
                    foreach(DataGridViewCell cell in view.SelectedCells)
                    {
                        temp += cell.Value + ";";
                        Clipboard.SetText(temp);
                    }
                    break;

                case "Open in browser":
                    List<int> openedRows = new List<int>();
                    foreach (DataGridViewCell cell in view.SelectedCells)
                    {
                        if (openedRows.Contains(cell.RowIndex)) continue;
                        openedRows.Add(cell.RowIndex);
                        temp = view.Rows[cell.RowIndex].Cells[0].Value.ToString();
                        Process.Start(temp);
                    }
                    break;

                case "Save to csv":
                    SaveRowsToCsv(view);
                    break;

                default:
                    temp = view.SelectedCells[0].Value.ToString();
                    temp += "\n" + e.ClickedItem.Tag;
                    MessageBox.Show(temp, "Error");
                    break;
            }
        }
        public void SaveRowsToCsv(DataGridView view)
        {
            SaveFileDialog saveFileDialog2 = new SaveFileDialog
            {
                Filter = "csv files (*.csv)|*.csv|All files (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = true
            };

            if (saveFileDialog2.ShowDialog() == DialogResult.OK)
            {
                string temp = saveFileDialog2.FileName;

                using (StreamWriter sw = new StreamWriter(temp))
                {
                    foreach (DataGridViewRow row in view.SelectedRows)
                    {
                        foreach (DataGridViewCell cell in row.Cells)
                        {
                            string cellval = "";
                            if (cell.Value != null)
                            {
                                cellval = cell.Value.ToString();
                            }

                            sw.Write(cellval + ";");
                        }
                        sw.Write('\n');
                    }
                }
            }
        }
        private void Save_StripMenuItem_Click(object sender, EventArgs e)
        {
            DataGridView view;
            foreach (Control control in tabControl1.SelectedTab.Controls)
            {
                if (control is DataGridView)
                {
                    view = (DataGridView) control;
                    view.SelectAll();
                    SaveRowsToCsv(view);
                }
            }
        }
        private void Settings_StripMenuItem_Click(object sender, EventArgs e)
        {
            OptionsForm.OptionsForm optionsForm = new OptionsForm.OptionsForm();
            optionsForm.Show();
        }
        public void IncreaseErrorCount()
        {
            Utils.ErrorsCounter++;
            label7.Text = Utils.ErrorsCounter.ToString();
            Invalidate();
        }
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (Control control in tabControl1.SelectedTab.Controls)
            {
                if(control is DataGridView)
                {
                    singleRowDataTable.Rows.Clear();
                    DataGridView view = (DataGridView) control;
                    PrepareSingleDataGridView(view);
                }
            }
        }
    }
}
