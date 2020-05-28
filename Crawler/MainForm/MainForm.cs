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

        private bool singleDataGridViewPrepared = false;
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
            isCrawling = false;

            allDataGridView.CellFormatting += this.AllDataGridViewCellFormating;
            allDataGridView.ReadOnly = true;
        }

        private void AllDataGridViewCellFormating(object sender, DataGridViewCellFormattingEventArgs e)
        {
            HumanReadableSizes(sender, e);
        }

        private void PrepareSingleDataGridView()
        {
            singleDataGridView.ReadOnly = true;
            singleDataGridView.Dock = DockStyle.Fill;
            singleDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            singleDataGridView.ColumnCount = 2;
            singleDataGridView.Columns[0].Name = "Nazwa";
            singleDataGridView.Columns[0].Width = 100;
            singleDataGridView.Columns[1].Name = "Wartosc";

            foreach(DataGridViewColumn column in allDataGridView.Columns)
            {
                string[] row = new string[] { column.Name, ""};
                singleDataGridView.Rows.Add(row);
            }

            singleDataGridViewPrepared = true;

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

        public void MakeButtonReady()
        {
            this.isCrawling = false;
            this.crawlButton.Text = "Start";
            this.crawlButton.Enabled = true;
        }

        private void DataGridView_SelectionChanged(object sender, EventArgs e)
        {
            // TODO: display selected row in singleDataGridView
            if (!singleDataGridViewPrepared)
            {
                PrepareSingleDataGridView();
            }

            DataGridView view = sender as DataGridView;
            //Debug.WriteLine("Selected Cells Count " + view.SelectedCells.Count);
            //Debug.WriteLine("Selected Columns Count " + view.SelectedColumns.Count);
            //Debug.WriteLine("Selected Rows Count " + view.SelectedRows.Count);
            //Debug.WriteLine("Selected Row Cell count " + allDataGridView.Rows[view.SelectedCells[0].RowIndex].Cells.Count);
            //Debug.WriteLine("columns count " + allDataGridView.Columns.Count);
                        
            if (view == null) return;
            if (view.SelectedCells.Count == 1)
            {
                
                DataGridViewRow selectedRow = allDataGridView.Rows[view.SelectedCells[0].RowIndex];

                //Debug.WriteLine("selected row cells count " + selectedRow.Cells.Count);
                //Debug.WriteLine("single data rows count " + singleDataGridView.Rows.Count);

                //allDataGridView.Rows[view.SelectedCells[0].RowIndex].Cells.Count) -> static 34?
                for (int a = 0; a < selectedRow.Cells.Count; a++)
                {
                    try
                    {
                        singleDataGridView.Rows[a].Cells[1].Value = selectedRow.Cells[a].Value;
                    }
                    catch
                    {
                        break;
                        // Debug.WriteLine("INdex: " + a + " error: " + ex.Message);
                    }
                }                         
            }
        }

        private void HumanReadableSizes(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (allDataGridView.Columns[e.ColumnIndex].Name.Equals(Base.Crawler.SIZE_COL))
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

        private void AllDataGridView_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Right)
            {
                if (allDataGridView.SelectedCells.Count > 0)
                {
                    ContextMenuStrip cellClickMenu = new System.Windows.Forms.ContextMenuStrip();
                    int hit_row = allDataGridView.HitTest(e.X, e.Y).RowIndex;
                    //int hit_cell = allDataGridView.HitTest(e.X, e.Y).ColumnIndex;

                    if (hit_row >= 0)
                    {
                        cellClickMenu.Items.Add("Kopiuj komórkę").Tag = "1";
                        cellClickMenu.Items.Add("Kopiuj zaznaczenie").Tag = "2";
                        cellClickMenu.Items.Add("Otwórz w przeglądarce").Tag = "3";
                        if (allDataGridView.SelectedRows.Count > 0)
                        {
                            cellClickMenu.Items.Add("Zapisz rzędy do CSV").Tag = "4";
                        }
                        
                    }

                    cellClickMenu.Show(allDataGridView, new Point(e.X, e.Y));

                    cellClickMenu.ItemClicked += new ToolStripItemClickedEventHandler(AllDataCellClicked);
                    
                }
            }
        }

        private void AllDataCellClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            string temp = "";

            ContextMenuStrip menustrip = (ContextMenuStrip)sender;
            menustrip.Close();

            switch (e.ClickedItem.Tag.ToString())
            {
                case "1":
                    temp = allDataGridView.SelectedCells[0].Value.ToString();                    
                    Clipboard.SetText(temp);
                    break;

                case "2":
                    foreach(DataGridViewCell cell in allDataGridView.SelectedCells)
                    {
                        temp += cell.Value.ToString() + ";";
                        Clipboard.SetText(temp);
                    }
                    break;

                case "3":
                    List<int> openedRows = new List<int>();
                    foreach (DataGridViewCell cell in allDataGridView.SelectedCells)
                    {         
                        if (!openedRows.Contains(cell.RowIndex))
                        {
                            openedRows.Add(cell.RowIndex);
                            temp = allDataGridView.Rows[cell.RowIndex].Cells[0].Value.ToString();
                            System.Diagnostics.Process.Start(temp);
                        }
                    }
                    break;

                case "4":
                    SaveRowsToCsv();
                    break;

                default:
                    temp = allDataGridView.SelectedCells[0].Value.ToString();
                    temp += "\n" + e.ClickedItem.Tag.ToString();
                    MessageBox.Show(temp, "pan bug");
                    break;
            }
        }

        public void SaveRowsToCsv()
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
                    foreach (DataGridViewRow row in allDataGridView.SelectedRows)
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
            allDataGridView.SelectAll();
            SaveRowsToCsv();
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

    }
}
