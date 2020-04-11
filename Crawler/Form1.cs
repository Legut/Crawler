using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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
        private bool isCrawling;
        private Crawler crawler;
        public Form1()
        {
            // Ustawienia ogólne forma
            InitializeComponent();
            dataGridView1.ColumnCount = 0;
            dataGridView2.ColumnCount = 0;
            dataGridView3.ColumnCount = 0;
            isCrawling = false;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Button senderButton = (Button) sender;
            if (senderButton.Text == "Start")
            {
                // Reakcja na wciśnięcie guzika "Crawluj"
                if (isCrawling == false)
                {
                    isCrawling = true;
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
                                crawler = new Crawler(this, siteToCrawl.Text);
                                crawler.StartCrawl();
                            }
                        }
                        else
                        {
                            siteToCrawlMsg.Text = "Nie istnieje";
                        }
                    }
                    button1.Text = "Stop";
                    button1.Enabled = true;
                }
                else
                {
                    button1.Enabled = false;
                    isCrawling = false;
                    button1.Text = "Start";
                    button1.Enabled = true;
                }
            }
            else
            {
                if (crawler != null)
                {
                    crawler.AbortCrawl();
                    button1.Text = "Aborcjowanie";
                    Debug.WriteLine("Aborcja");
                    siteToCrawlMsg.Text = "Zatrzymywanie crawlowania";
                    button1.Enabled = false;
                }
            }
        }

        private bool PageExists(string url)
        {
            // Jeśli nie dodano prefixu protokołu to dodaj go do adresu url
            if (!url.StartsWith("http://") && !url.StartsWith("https://"))
            {
                url = "http://" + url;
                siteToCrawl.Text = url;
            }

            try
            {
                // Inicjalizacja odwołania do podanego adresu Url w celu uzyskania odpowiedzi
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                request.Method = "HEAD";
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                response.Close();

                //Jeśli Status code == 200 to zwrócę true. Innymi słowy jeśli uda się nawiązać połączenie to zwrócę true
                return (response.StatusCode == HttpStatusCode.OK);
            }
            catch
            {
                // Jakiekolwiek wyjątki zwrócą false.
                // TODO: Obsługa poszczególnych wyjątków, tak aby poinformować użytkownika nie tylko o tym, że nie udało się nawiązać połączenia, ale również czemu to się nie udało 
                return false;
            }
        }
        private bool PageHasCertificate(string url)
        {
            // Jeśli strona ma certyfikat to należy zastąpić http:// na https://
            url = url.Replace("http://", "https://");
            try
            {
                // Inicjalizacja odwołania do podanego adresu Url w celu uzyskania odpowiedzi
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                request.Method = "HEAD";
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                response.Close();

                //Jeśli Status code == 200 to zwrócę true. Innymi słowy jeśli uda się nawiązać połączenie to zwrócę true
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    siteToCrawl.Text = url;
                }
                return (response.StatusCode == HttpStatusCode.OK);
            }
            catch
            {
                // Jakiekolwiek wyjątki zwrócą false.
                // TODO: Obsługa poszczególnych wyjątków, tak aby poinformować użytkownika nie tylko o tym, że nie udało się nawiązać połączenia, ale również czemu to się nie udało 
                return false;
            }
        }
        public void UpdateIdleCounter(int count)
        {
            label6.Text = "IdleCounter: " + count;
        }
        public void UpdateCrawlingStatus(int status, int max)
        {
            label3.Text = status + " / " + max;
        }
        public void UpdateCrawledStatus(int left, int all)
        {
            label5.Text = left + " / " + all;

            //hehe
            if (left == all && button1.Text == "Aborcjowanie")
            {
                button1.Text = "Start";
                // TODO: Umożliwnie wznowienia
                //button1.Enabled = true;
                siteToCrawlMsg.Text = "Skonczylem anulowac zadania";
            }
        }
        public void BindDataTableToWszystkie(DataTable dt)
        {
            dataGridView1.DataSource = dt;
        }
        public void BindDataTableToWewnetrzne(DataTable dt)
        {
            BindingSource src = new BindingSource();
            src.DataSource = new DataView(dt);
            src.Filter = "IsInternal = 'True'";
            dataGridView2.DataSource = src;
        }
        public void BindDataTableToZewnetrzne(DataTable dt)
        {
            BindingSource src = new BindingSource();
            src.DataSource = new DataView(dt);
            src.Filter = "IsInternal = 'False'";
            dataGridView3.DataSource = src;
            dataGridView3.Columns["Indexability"].Visible = false;
        }
    }
}
