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

        private void AddRowToPanel(TableLayoutPanel panel, string[] rowElements)
        {
            if (panel.ColumnCount != rowElements.Length)
                throw new Exception("Elements number doesn't match!");
            //get a reference to the previous existent row
            //RowStyle temp = panel.RowStyles[panel.RowCount - 1];
            RowStyle temp = new RowStyle();
            temp.SizeType = SizeType.Absolute;
            temp.Height = 20;

            //increase panel rows count by one
            panel.RowCount++;
            //add a new RowStyle as a copy of the previous one
            
            panel.RowStyles.Add(new RowStyle(temp.SizeType, temp.Height));
            //add the control
            for (int i = 0; i < rowElements.Length; i++)
            {
                panel.Controls.Add(new Label() { Text = rowElements[i] }, i, panel.RowCount - 1);
            }
        }

        public void AddUriToGui2(string url)
        {
            string[] dane = new string[1];
            dane[0] = url;
            AddRowToPanel(tableLayoutPanel2, dane);
        }

        public void AddUriToGui(string url)
        {
            string[] dane = new string[3];
            dane[0] = url;
            dane[1] = "benis";
            dane[2] = ":---DDD";
            AddRowToPanel(tableLayoutPanel1, dane);

/*
            tableLayoutPanel1.RowCount++;
            int lastrow = tableLayoutPanel1.RowCount;

            //this.tabPage1.Text += "url" + '\n';
            Label label = new Label();
            label.Text = url;

            tableLayoutPanel1.SetRow(label, lastrow);            
            *///this.tabPage1.Controls.Add(label);
        }

        public void UpdateIdleCounter(int count)
        {
            label6.Text = "IdleCounter: " + count;
        }

        public void UpdateCrawlingStatus(int status)
        {
            label3.Text = status + " / 10";
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
