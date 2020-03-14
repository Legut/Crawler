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
                        Crawler crawl = new Crawler(siteToCrawl.Text);
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
    }
}
