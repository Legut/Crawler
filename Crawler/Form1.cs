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
            // Ustawienia ogólne forma
            InitializeComponent();
            dataGridView1.ColumnCount = 0;
            dataGridView2.ColumnCount = 0;
            dataGridView3.ColumnCount = 0;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            // Reakcja na wciśnięcie guzika "Crawluj"
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
            // Dynamiczna aktualizacja listy w postaci Listenera
            dataGridView1.Update();
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
        public void bindDataTableToWszystkie(DataTable dt)
        {
            dataGridView1.DataSource = dt;
        }
        public void bindDataTableToWewnetrzne(DataTable dt)
        {
            BindingSource src = new BindingSource();
            src.DataSource = new DataView(dt);
            src.Filter = "IsInternal = 'True'";
            dataGridView2.DataSource = src;
        }
        public void bindDataTableToZewnetrzne(DataTable dt)
        {
            BindingSource src = new BindingSource();
            src.DataSource = new DataView(dt);
            src.Filter = "IsInternal = 'False'";
            dataGridView3.DataSource = src;
            dataGridView3.Columns["Indexability"].Visible = false;
        }
    }
}
