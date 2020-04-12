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
            // Ustawienia ogólne forma
            InitializeComponent();
            dataGridView1.ColumnCount = 0;
            dataGridView2.ColumnCount = 0;
            dataGridView3.ColumnCount = 0;
            isCrawling = false;

            colindex = -1;
            rowindex = -1;
            resizing = false;
            firstRowHeight = 80;
            lastRowMinHeight = 50;
            middleRowMinHeight = 80;
            firstColumnMinWidth = 100;
            lastColumnMinWidth = 100;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            rowStyles = tableLayoutPanel1.RowStyles;
            columnStyles = tableLayoutPanel1.ColumnStyles;
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

        private void tableLayoutPanel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                rowStyles = tableLayoutPanel1.RowStyles;
                columnStyles = tableLayoutPanel1.ColumnStyles;
                resizing = true;
            }
        }

        private void tableLayoutPanel1_MouseMove(object sender, MouseEventArgs e)
        {
            // Rozpoznawanie pozycji kursora. Czy kursor znajduje się nad krawędzią pola. Jesli tak to nad którą?
            if (!resizing)
            {
                float width = 0;
                float height = 0;

                // Dla wierszy
                for (int i = 0; i < rowStyles.Count; i++)
                {
                    height += rowStyles[i].Height;
                    if (e.Y > height - 3 && e.Y < height + 3)
                    {
                        rowindex = i;
                        // Pomijamy pierwszy wiersz, ponieważ nei chcemy zmieniać jego wymiarów
                        if (rowindex == 1) 
                        { 
                            tableLayoutPanel1.Cursor = Cursors.HSplit;
                        }
                        break;
                    }
                    else
                    {
                        rowindex = -1;
                        tableLayoutPanel1.Cursor = Cursors.Default;
                    }
                }

                // Dla kolumn
                for (int i = 0; i < columnStyles.Count; i++)
                {
                    width += columnStyles[i].Width;
                    if (e.X > width - 3 && e.X < width + 3 && e.Y>firstRowHeight)
                    {
                        colindex = i;
                        if (rowindex == 1 && colindex == 0)
                            tableLayoutPanel1.Cursor = Cursors.Cross;
                        else if (colindex == 0)
                            tableLayoutPanel1.Cursor = Cursors.VSplit;
                        break;
                    }
                    else
                    {
                        colindex = -1;
                        if (rowindex == 0) 
                            tableLayoutPanel1.Cursor = Cursors.Default;
                    }
                }
                if (rowindex == 0)
                    tableLayoutPanel1.Cursor = Cursors.Default;
            } 
            // Zmiana wymiarów względem pozycji myszki
            else if (resizing && (colindex == 0 || rowindex == 1))
            {
                // położenie myszy
                float width = e.X;
                float height = e.Y;

                float nextRowHeight = tableLayoutPanel1.Height;
                float nextColumnWidth = tableLayoutPanel1.Width;

                // szerokośc kolumn
                if (colindex == 0)
                {
                    for (int i = 0; i < colindex; i++)
                    {
                        width -= columnStyles[i].Width;
                    }

                    for (int i = 0; i < colindex + 1; i++)
                    {
                        nextColumnWidth -= columnStyles[i].Width;
                    }

                    if (width > firstColumnMinWidth && width < tableLayoutPanel1.Width - lastColumnMinWidth)
                    {
                        columnStyles[colindex].Width = width;
                        columnStyles[colindex + 1].Width = nextColumnWidth;
                    }
                }

                // wyskość wierszy
                if (rowindex == 1)
                {
                    for (int i = 0; i < rowindex; i++)
                    {
                        height -= rowStyles[i].Height;
                    }

                    for (int i = 0; i < rowindex+1; i++)
                    {
                        nextRowHeight -= rowStyles[i].Height;
                    }

                    // Tu następuje zmiana wysokości
                    if (height > middleRowMinHeight && height < tableLayoutPanel1.Height - firstRowHeight - lastRowMinHeight)
                    {
                        rowStyles[rowindex].Height = height;
                        rowStyles[rowindex+1].Height = nextRowHeight;
                    }
                }
            }
        }

        private void tableLayoutPanel1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                resizing = false;
                if (rowindex == 1)
                {
                    // Weryfikacja wysokości. W razie problemów następuje korekta.
                    float height = 0;
                    for (int i = 0; i < rowStyles.Count; i++)
                    {
                        height += rowStyles[i].Height;
                    }

                    if (e.Y < 0 || e.Y > tableLayoutPanel1.Height)
                    {
                        // Debug.WriteLine(e.Y + " oraz " + tableLayoutPanel1.Height);
                        // Debug.WriteLine("Korekta wysokości - Wyjście kursorem poza obszar resize'u");
                        if (height > tableLayoutPanel1.Height && rowStyles[rowindex + 1].Height < rowStyles[rowindex].Height)
                        {
                            rowStyles[rowindex].Height = tableLayoutPanel1.Height - firstRowHeight - lastRowMinHeight;
                            rowStyles[rowindex + 1].Height = lastRowMinHeight;
                        }
                        else if (height < tableLayoutPanel1.Height && rowStyles[rowindex + 1].Height > rowStyles[rowindex].Height)
                        {
                            rowStyles[rowindex].Height = middleRowMinHeight; ;
                            rowStyles[rowindex + 1].Height = tableLayoutPanel1.Height - firstRowHeight - middleRowMinHeight;
                        }
                    }
                    else if (height > tableLayoutPanel1.Height)
                    {
                        // Debug.WriteLine("Korekta wysokości - Szybkie chwycenie i przesunięcie kursorem, które wywołało nieścisłość wartości");

                        if ((height - tableLayoutPanel1.Height) % 2 != 0)
                        {
                            // Debug.WriteLine("Modulo");
                            if (rowStyles[rowindex].Height > middleRowMinHeight)
                            {
                                rowStyles[rowindex].Height -= 1;
                                height--;
                            }
                        }

                        float difference = (height - tableLayoutPanel1.Height) / 2;
                        if (rowStyles[rowindex].Height - difference > middleRowMinHeight && rowStyles[rowindex + 1].Height - difference > lastRowMinHeight)
                        {
                            // Debug.WriteLine("Metoda szybkiej korekcji");
                            rowStyles[rowindex].Height -= difference;
                            rowStyles[rowindex + 1].Height -= difference;
                        }
                        else
                        {
                            // Debug.WriteLine("Metoda wolnej korekcji");
                            while (height > tableLayoutPanel1.Height)
                            {
                                if (rowStyles[rowindex].Height > middleRowMinHeight)
                                {
                                    rowStyles[rowindex].Height -= 1;
                                    height--;
                                }

                                if (height == tableLayoutPanel1.Height) break;

                                if (rowStyles[rowindex + 1].Height > lastRowMinHeight)
                                {
                                    rowStyles[rowindex + 1].Height -= 1;
                                    height--;
                                }
                            }
                        }
                    }
                    // Przypadek height < tableLayoutPanel1.Height nie musi zostać obsłużony ponieważ nie wystepuje tu błąd 
                }

                if (colindex == 0)
                {
                    // Weryfikacja szerokości. W razie problemów następuje korekta.
                    float width = 0;
                    for (int i = 0; i < columnStyles.Count; i++)
                    {
                        width += columnStyles[i].Width;
                    }

                    if (e.X < 0 || e.X > tableLayoutPanel1.Width)
                    {
                        // Debug.WriteLine(e.X + " oraz " + tableLayoutPanel1.Width);
                        // Debug.WriteLine("Korekta wysokości - Wyjście kursorem poza obszar resize'u");
                        if (width > tableLayoutPanel1.Width && columnStyles[colindex + 1].Width < columnStyles[colindex].Width)
                        {
                            columnStyles[colindex].Width = tableLayoutPanel1.Width - lastColumnMinWidth;
                            columnStyles[colindex + 1].Width = lastColumnMinWidth;
                        }
                        else if (width < tableLayoutPanel1.Width && columnStyles[colindex + 1].Width > columnStyles[colindex].Width)
                        {
                            columnStyles[colindex].Width = firstColumnMinWidth; ;
                            columnStyles[colindex + 1].Width = tableLayoutPanel1.Width - firstColumnMinWidth;
                        }
                    }
                    else if (width > tableLayoutPanel1.Width)
                    {
                        // Debug.WriteLine("Korekta szerokości - Szybkie chwycenie i przesunięcie kursorem, które wywołało nieścisłość wartości");

                        if ((width - tableLayoutPanel1.Width) % 2 != 0)
                        {
                            //Debug.WriteLine("Modulo");
                            if (columnStyles[colindex].Width > firstColumnMinWidth)
                            {
                                columnStyles[colindex].Width -= 1;
                                width--;
                            }
                        }

                        float difference = (width - tableLayoutPanel1.Width) / 2;
                        // Debug.WriteLine("difference = " + difference);
                        if (columnStyles[colindex].Width - difference > firstColumnMinWidth && columnStyles[colindex + 1].Width - difference > lastColumnMinWidth)
                        {
                            // Debug.WriteLine("Metoda szybkiej korekcji");
                            columnStyles[colindex].Width -= difference;
                            columnStyles[colindex + 1].Width -= difference;
                        }
                        else
                        {
                            // Debug.WriteLine("Metoda wolnej korekcji");
                            while (width > tableLayoutPanel1.Width)
                            {
                                if (columnStyles[colindex].Width > firstColumnMinWidth)
                                {
                                    columnStyles[colindex].Width -= 1;
                                    width--;
                                }

                                if (width == tableLayoutPanel1.Width) break;

                                if (columnStyles[colindex + 1].Width > lastColumnMinWidth)
                                {
                                    columnStyles[colindex + 1].Width -= 1;
                                    width--;
                                }
                            }
                        }
                    }
                    // Przypadek width < tableLayoutPanel1.Width nie musi zostać obsłużony ponieważ nie wystepuje tu błąd 
                }

                tableLayoutPanel1.Cursor = Cursors.Default;
            }
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            // Weryfikacja wysokości. W razie problemów następuje korekta.
            rowindex = 1;
            float height = 0;
            for (int i = 0; i < rowStyles.Count; i++)
            {
                height += rowStyles[i].Height;
                // Debug.WriteLine(height);
            }

            if (height > tableLayoutPanel1.Height)
            {
                // Debug.WriteLine("Korekta wysokości - Szybkie chwycenie i przesunięcie kursorem, które wywołało nieścisłość wartości");

                if ((height - tableLayoutPanel1.Height) % 2 != 0)
                {
                    // Debug.WriteLine("Modulo");
                    if (rowStyles[rowindex].Height > middleRowMinHeight)
                    {
                        rowStyles[rowindex].Height -= 1;
                        height--;
                    }
                }

                float difference = (height - tableLayoutPanel1.Height) / 2;
                if (rowStyles[rowindex].Height - difference > middleRowMinHeight && rowStyles[rowindex + 1].Height - difference > lastRowMinHeight)
                {
                    // Debug.WriteLine("Metoda szybkiej korekcji");
                    rowStyles[rowindex].Height -= difference;
                    rowStyles[rowindex + 1].Height -= difference;
                }
                else
                {
                    // Debug.WriteLine("Metoda wolnej korekcji");
                    while (height > tableLayoutPanel1.Height)
                    {
                        if (rowStyles[rowindex].Height > middleRowMinHeight)
                        {
                            rowStyles[rowindex].Height -= 1;
                            height--;
                        }

                        if (height == tableLayoutPanel1.Height) break;

                        if (rowStyles[rowindex + 1].Height > lastRowMinHeight)
                        {
                            rowStyles[rowindex + 1].Height -= 1;
                            height--;
                        }
                    }
                }
            }
            // Przypadek height < tableLayoutPanel1.Height nie musi zostać obsłużony ponieważ nie wystepuje tu błąd 


            // Weryfikacja szerokości. W razie problemów następuje korekta.
            colindex = 0;
            float width = 0;
            for (int i = 0; i < columnStyles.Count; i++)
            {
                width += columnStyles[i].Width;
            }

            if (width > tableLayoutPanel1.Width)
            {
                // Debug.WriteLine("Korekta szerokości - Szybkie chwycenie i przesunięcie kursorem, które wywołało nieścisłość wartości");

                if ((width - tableLayoutPanel1.Width) % 2 != 0)
                {
                    // Debug.WriteLine("Modulo");
                    if (columnStyles[colindex].Width > firstColumnMinWidth)
                    {
                        columnStyles[colindex].Width -= 1;
                        width--;
                    }
                }

                float difference = (width - tableLayoutPanel1.Width) / 2;
                // Debug.WriteLine("difference = " + difference);
                if (columnStyles[colindex].Width - difference > firstColumnMinWidth && columnStyles[colindex + 1].Width - difference > lastColumnMinWidth)
                {
                    // Debug.WriteLine("Metoda szybkiej korekcji");
                    columnStyles[colindex].Width -= difference;
                    columnStyles[colindex + 1].Width -= difference;
                }
                else
                {
                    // Debug.WriteLine("Metoda wolnej korekcji");
                    while (width > tableLayoutPanel1.Width)
                    {
                        if (columnStyles[colindex].Width > firstColumnMinWidth)
                        {
                            columnStyles[colindex].Width -= 1;
                            width--;
                        }

                        if (width == tableLayoutPanel1.Width) break;

                        if (columnStyles[colindex + 1].Width > lastColumnMinWidth)
                        {
                            columnStyles[colindex + 1].Width -= 1;
                            width--;
                        }
                    }
                }
            }
            // Przypadek width < tableLayoutPanel1.Width nie musi zostać obsłużony ponieważ nie wystepuje tu błąd 
        }
    }
}
