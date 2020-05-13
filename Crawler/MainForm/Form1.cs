using System;
using System.Diagnostics;
using System.Net;
using System.Windows.Forms;
using System.Windows.Input;
using Crawler.Utilities;

namespace Crawler.MainForm
{
    public partial class Form1 : Form
    {
        private bool isCrawling;
        private Crawler crawler;

        private bool resizing;
        private TableLayoutRowStyleCollection rowStyles;
        private TableLayoutColumnStyleCollection columnStyles;
        private int colindex;
        private int rowindex; 
        private int firstRowHeight;
        private int lastRowMinHeight;
        private int middleRowMinHeight;
        private int firstColumnMinWidth;
        private int lastColumnMinWidth;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            allDataGridView.ColumnCount = 0;
            internalDataGridView.ColumnCount = 0;
            externalDataGridView.ColumnCount = 0;
            isCrawling = false;

            colindex = -1;
            rowindex = -1;
            resizing = false;
            firstRowHeight = 80;
            lastRowMinHeight = 50;
            middleRowMinHeight = 80;
            firstColumnMinWidth = 100;
            lastColumnMinWidth = 100;
            rowStyles = tableLayoutPanel1.RowStyles;
            columnStyles = tableLayoutPanel1.ColumnStyles;
        }
        private void Button1_Click(object sender, EventArgs e)
        {
            crawlButton.Enabled = false;
            if (isCrawling == false)
            {
                isCrawling = true;
                if (siteAddress.Text.Length > 0)
                {
                    EnsureThatProtocolIsProvided(siteAddress.Text);
                    if (PageExists(siteAddress.Text))
                    {
                        siteToCrawlMsg.Text = PageHasCertificate(siteAddress.Text) ? "Istnieje i ma certyfikat" : "Istnieje i nie ma certyfikatu";
                        crawler = new Crawler(this, siteAddress.Text);
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

        private void DataGridView_SelectionChanged(object sender, EventArgs e)
        {
            DataGridView view = sender as DataGridView;
            // TODO: display selected row in singleDataGridView
            if (view == null) return;
            if (view.SelectedCells.Count > 0)
            {
                if (Mouse.LeftButton == MouseButtonState.Pressed)
                {
                    Debug.WriteLine("sadasdasd " + view.SelectedCells[0]);
                }
            }
            /*foreach (DataGridViewRow row in view.SelectedRows)
            {
                string value1 = row.Cells[0].Value.ToString();
                string value2 = row.Cells[1].Value.ToString();
            }*/
        }

        private void EnsureThatProtocolIsProvided(string url)
        {
            // Ensure there is protocol provided at the begining of url
            if (!url.StartsWith("http://") && !url.StartsWith("https://"))
            {
                siteAddress.Text = "http://" + url;
            }
        }

        private bool PageExists(string url)
        {
            try
            {
                // Check if there is response from given url
                if (WebRequest.Create(url) is HttpWebRequest request)
                {
                    request.Method = "HEAD";
                    HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                    response?.Close();
                }

                return true;
            }
            catch
            {
                CustomMessages.DisplayPageDoesntExistMsg();
                isCrawling = false;
                return false;
            }
        }

        private bool PageHasCertificate(string url)
        {
            // if site has certificate than it will load by https://
            url = url.Replace("http://", "https://");
            try
            {
                if (WebRequest.Create(url) is HttpWebRequest request)
                {
                    request.Method = "HEAD";
                    HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                    response?.Close();
                }

                siteAddress.Text = url;
                return true;
            }
            catch (NullReferenceException exception)
            {
                CustomMessages.DisplayCustomErrorMsg("NullReferenceException", exception.Message);
                return false;
            }
            catch (Exception exception)
            {
                CustomMessages.DisplayCustomErrorMsg("UnrecognizedException", exception.Message);
                return false;
            }

            // TODO: Obsługa poszczególnych wyjątków, tak aby poinformować użytkownika nie tylko o tym, że nie udało się nawiązać połączenia, ale również czemu to się nie udało 
        }
        public void UpdateCrawlingStatus(int status, int max)
        {
            crawlingStatusLabel.Text = status + " / " + max;
        }
        public void UpdateCrawledStatus(int left, int all)
        {
            crawledStatusLabel.Text = left + " / " + all;
        }
    }
}
