using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Crawler
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            // Ustawienia ogólne forma
            dataGridView1.ColumnCount = 5;
            dataGridView1.Columns[0].Name = "Address";
            dataGridView1.Columns[1].Name = "Content Type";
            dataGridView1.Columns[2].Name = "Status Code";
            dataGridView1.Columns[3].Name = "Status";
            dataGridView1.Columns[4].Name = "Indexability";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            if (siteToCrawl.Text.Length > 0) 
            {
                if (PageExists(siteToCrawl.Text))
                {
                    siteToCrawlMsg.Text = "Istnieje";
                    if (!siteToCrawl.Text.StartsWith("https://"))
                    {
                        if (PageHasCertificate(siteToCrawl.Text))
                        {
                            siteToCrawlMsg.Text = "Istnieje i ma certyfikat";

                        }
                        else
                        {
                            siteToCrawlMsg.Text = "Istnieje i nie ma certyfikatu";
                        }
                        Crawler crawl = new Crawler(this,siteToCrawl.Text);
                        crawl.StartCrawl();
                    }
                }
                else 
                {
                    siteToCrawlMsg.Text = "Nie istnieje";
                }
            }
        }

        internal void pageFragments_ListChanged(object sender, ListChangedEventArgs e)
        {
            dataGridView1.Update();
        }

        internal void SetDataSource(BindingList<PageFragment> pageFragments)
        {
            dataGridView1.DataSource = pageFragments;

        }

        private bool PageExists(string url)
        {
            // If no http:// or https:// was given add it to url
            if (!url.StartsWith("http://") && !url.StartsWith("https://"))
            {
                url = "http://" + url;
                siteToCrawl.Text = url;
            }

            try
            {
                //Creating the HttpWebRequest
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                //Setting the Request method HEAD, you can also use GET too.
                request.Method = "HEAD";
                //Getting the Web Response.
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                //Returns TRUE if the Status code == 200
                response.Close();
                return (response.StatusCode == HttpStatusCode.OK);
            }
            catch
            {
                //Any exception will returns false.
                return false;
            }
        }
        private bool PageHasCertificate(string url)
        {
            
            url = url.Replace("http://", "https://");

            try
            {
                //Creating the HttpWebRequest
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                //Setting the Request method HEAD, you can also use GET too.
                request.Method = "HEAD";
                //Getting the Web Response.
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                //Returns TRUE if the Status code == 200
                response.Close();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    siteToCrawl.Text = url;
                }
                return (response.StatusCode == HttpStatusCode.OK);
            }
            catch
            {
                //Any exception will returns false.
                return false;
            }
        }

        internal void AddUriToDataGridView(ref PageFragment pf)
        {
            string[] row = { pf.Address, pf.ContentType, pf.StatusCode, pf.Status, pf.Indexability.ToString() };
            dataGridView1.Rows.Add(row);
            dataGridView1.Update();
        }

        public void UpdateIdleCounter(int count)
        {
            label6.Text = "IdleCounter: " + count;
        }

        public void UpdateCrawlingStatus(int status, int max)
        {
            label3.Text = status + " / " + max;
            this.Update();
        }

        public void UpdateCrawledStatus(int left, int all)
        {
            label5.Text = left + " / " + all;
        }

        public void UnblockSearchButton()
        {
            button1.Enabled = true;
        }

    }
}
