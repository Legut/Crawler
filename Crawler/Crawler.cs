using HtmlAgilityPack;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Crawler
{
    class Crawler
    {
        private string baseUrl { get; set; }
        private HashSet<string> crawledPages = new HashSet<string>();
        private static int maxSemaphores = 50;
        private readonly SemaphoreSlim semaphore = new SemaphoreSlim(maxSemaphores);
        private CancellationToken cancellationToken = default(CancellationToken);

        //GUI
        Form1 okienkoGui;
        private int przejrzaneStrony;
        private int stronyDoPrzejrzenia;

        //Data
        private DataTable dt;
        private int titlesCounter;
        private int metaDescriptionsCounter;


        public Crawler(Form1 form1, string siteToCrawl)
        {
            okienkoGui = form1;
            baseUrl = siteToCrawl;
            titlesCounter = 0;
            metaDescriptionsCounter = 0;

            //GUI
            okienkoGui.UpdateCrawlingStatus(maxSemaphores, maxSemaphores);
            przejrzaneStrony = 0;
            stronyDoPrzejrzenia = 1;
        }
        private async Task startCrawlingPage(string page)
        {
            okienkoGui.UpdateCrawlingStatus(semaphore.CurrentCount, maxSemaphores); // Update GUI
            crawledPages.Add(page); // Dodaje do podstron już przecrawlowanych (bo nawet jesli to nie jest jeszcze przecrawlowane, to będzie crawlowane zaraz jak tylko semafor się zwolni)
            await this.semaphore.WaitAsync(cancellationToken); // Czekam aż semafor będzie wolny

            PageFragment pf = new PageFragment();

            // Pobieram podstronę
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.GetAsync(page);

            try
            {
                // Sprawdzam czy podstrona jest wewnętrzna czy zewnętrzna
                if (Uri.Compare(new Uri(baseUrl), new Uri(page), UriComponents.Host, UriFormat.SafeUnescaped, StringComparison.CurrentCulture) == 0)
                {
                    // Wyjmuję Html jako string
                    string html = await response.Content.ReadAsStringAsync();
                    HtmlDocument htmlDocument = new HtmlDocument();
                    htmlDocument.LoadHtml(html);

                    // Głębsze crawlowanie znalezionych na podstronie linków, w osobnych wątkach
                    CrawlFurther(htmlDocument);

                    // Uzupełniam PageFragment danymi
                    pf.Address = page;
                    pf.IsInternal = true;

                    if (pf.Address != null) { 
                        pf.ContentType = response.Content.Headers.ContentType.MediaType;
                        pf.StatusCode = ((int)response.StatusCode).ToString();
                        pf.Status = response.StatusCode.ToString();
                        

                        // Update Indexability and IndexabilityStatus values
                        pf.Indexability = "Indexable";

                        if (pf.StatusCode != "200") 
                        {
                            pf.Indexability = "Non-indexable";
                            if (pf.StatusCode.StartsWith("3"))
                            {
                                pf.IndexabilityStatus = "Redirect";
                            } 
                            else if (pf.StatusCode.StartsWith("4"))
                            {
                                pf.IndexabilityStatus = "Client Error";
                            } 
                            else if (pf.StatusCode.StartsWith("5"))
                            {
                                pf.IndexabilityStatus = "Server Error";
                            }
                        }


                        List<HtmlNode> metas = htmlDocument.DocumentNode.Descendants("meta").ToList();
                        foreach (HtmlNode meta in metas) 
                        {
                            if (meta.GetAttributeValue("name", "null") == "robots") 
                            {
                                if (meta.GetAttributeValue("content", "null") == "noindex")
                                {
                                    pf.Indexability = "Non-Indexable";
                                    pf.IndexabilityStatus = "Noindex";
                                }
                            }
                        }

                        List<HtmlNode> htmlTitles = htmlDocument.DocumentNode.Descendants("title").ToList();
                        pf.Titles = new List<Title>();
                        foreach (HtmlNode htmlTitle in htmlTitles) 
                        {
                            Title title = new Title();
                            title.TitleText = htmlTitle.InnerText;
                            title.TitleLength = title.TitleText.Length;

                            Font arialBold = new Font("Arial", 16.0F);
                            title.TitlePixelWidth = System.Windows.Forms.TextRenderer.MeasureText(title.TitleText, arialBold).Width;

                            pf.Titles.Add(title);
                        }

                        pf.MetaDescriptions = new List<MetaDescription>();
                        foreach (HtmlNode meta in metas)
                        {
                            if (meta.GetAttributeValue("name", "null") == "description")
                            {
                                MetaDescription metaDesc = new MetaDescription();
                                metaDesc.MetaDescriptionText = meta.GetAttributeValue("content", "");
                                metaDesc.MetaDescriptionLength = metaDesc.MetaDescriptionText.Length;

                                Font arialBold = new Font("Arial", 13.0F);
                                metaDesc.MetaDescriptionPixelWidth = System.Windows.Forms.TextRenderer.MeasureText(metaDesc.MetaDescriptionText, arialBold).Width;

                                pf.MetaDescriptions.Add(metaDesc);
                            }
                        }


                        // Aktualizuję źródło danych
                        updateDataTable(pf);
                    }
                }
                else
                {
                    pf.Address = page;
                    pf.IsInternal = false;

                    pf.ContentType = response.Content.Headers.ContentType.MediaType;
                    pf.StatusCode = ((int)response.StatusCode).ToString();
                    pf.Status = response.StatusCode.ToString();
                    pf.Indexability = "";

                    pf.Titles = new List<Title>();
                    pf.MetaDescriptions = new List<MetaDescription>();

                    // Aktualizuję źródło danych
                    updateDataTable(pf);
                    // Debug.Write(" strona: " + page + " wychodzi poza strone bazowa \n");
                }
            }
            catch (UriFormatException e)
            {
                Debug.WriteLine(" Podstrona: " + page + " ma niepoprawnie sformatowany url ");
            }
            catch (WebException ex) when ((ex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.NotFound)
            {
                pf.StatusCode = "404";
                Debug.WriteLine(" strona " + page + " jest niedostepna -> 404 NotFound");
            }
            catch(WebException ex)
            {
                string status = (ex.Response as HttpWebResponse).StatusCode.ToString();
                pf.StatusCode = status;
                Debug.WriteLine(" strona " + page + " WebEx: " + status);
            }
            catch (Exception e)
            {
                Debug.WriteLine(" strona " + page + " spotkala niezdefiniowany (nieobsłużony indywidualnie) wyjątek: " + e.Message);
            }

            // Zwalniam semafor
            this.semaphore.Release();

            // Aktualizuję GUI
            przejrzaneStrony++;
            okienkoGui.UpdateCrawlingStatus(semaphore.CurrentCount, maxSemaphores);
            okienkoGui.UpdateCrawledStatus(przejrzaneStrony,stronyDoPrzejrzenia);
            
        }

        public async void StartCrawl()
        {
            createDataTable();
            await startCrawlingPage(baseUrl);
        }
        private void CrawlFurther(HtmlDocument htmlDocument)
        {
            CrawlThroughAnchors(htmlDocument);
            CrawlThroughLinks(htmlDocument);
            CrawlThroughScripts(htmlDocument);
            CrawlThroughIframes(htmlDocument);
            CrawlThroughImages(htmlDocument);
        }
        private void CrawlThroughImages(HtmlDocument htmlDocument)
        {
            // Pobieram wszystkie <img> z podstrony i rekurencyjnie zaczynam ich crawlowanie
            var images = htmlDocument.DocumentNode.Descendants("img").ToList();
            foreach (var image in images)
            {
                string address = image.GetAttributeValue("src", "null").ToString();

                TryCrawlingNextPage(address);
            }
        }
        private void CrawlThroughIframes(HtmlDocument htmlDocument)
        {
            // Pobieram wszystkie <iframe></iframe> z podstrony i rekurencyjnie zaczynam ich crawlowanie
            var iframes = htmlDocument.DocumentNode.Descendants("iframe").ToList();
            foreach (var iframe in iframes)
            {
                string address = iframe.GetAttributeValue("src", "null").ToString();

                TryCrawlingNextPage(address);
            }
        }
        private void CrawlThroughScripts(HtmlDocument htmlDocument)
        {
            // Pobieram wszystkie <script></script> z podstrony i rekurencyjnie zaczynam ich crawlowanie
            var scripts = htmlDocument.DocumentNode.Descendants("script").ToList();
            foreach (var script in scripts)
            {
                if (script.GetAttributeValue("type", "null") == "text/javascript")
                {
                    string address = script.GetAttributeValue("src", "null").ToString();

                    TryCrawlingNextPage(address);
                }
            }
        }
        private void CrawlThroughLinks(HtmlDocument htmlDocument)
        {
            // Pobieram wszystkie <link></link> z podstrony i rekurencyjnie zaczynam ich crawlowanie
            var links = htmlDocument.DocumentNode.Descendants("link").ToList();
            foreach (var link in links)
            {
                if (link.GetAttributeValue("rel", "null") == "stylesheet")
                {
                    string address = link.GetAttributeValue("href", "null").ToString();

                    TryCrawlingNextPage(address);
                }
            }
        }
        private void CrawlThroughAnchors(HtmlDocument htmlDocument)
        {
            // Pobieram wszystkie <a></a> z podstrony i rekurencyjnie zaczynam ich crawlowanie
            var anchors = htmlDocument.DocumentNode.Descendants("a").ToList();
            foreach (var anchor in anchors)
            {
                string address = anchor.GetAttributeValue("href", "null").ToString();
                
                TryCrawlingNextPage(address);
            }
        }
        private void TryCrawlingNextPage(string address)
        {
            // Popraw adres tak aby był pełnym linkiem
            NormalizeAddress(baseUrl, ref address);
            // Sprawdzam czy adres jest poprawny
            if (address != null)
            {
                // Sprawdzam czy adres podstrony został już przecrawlowany zanim zacznę go crawlować
                if (!crawledPages.Contains(address))
                {
                    // Zaczynam rekurencyjne crawlowanie kolejnej podstrony
                    stronyDoPrzejrzenia++;
                    startCrawlingPage(address);
                }
            }
        }
        private void NormalizeAddress(string baseUrl, ref string address)
        {
            // Normalizacja adresu url. Wywalamy argumenty po znaku zapytania. Sprawdzamy, czy na początku jest prefix protokołu.
            // Wykluczamy adresy, które nas nieinteresują, przez ustawienie wartościu null.
            if (address.Contains("?"))
            {
                address = address.Remove(address.IndexOf("?"));
            }
            if (address.StartsWith("http://") || address.StartsWith("https://"))
                return;
            else if (address.StartsWith("/"))
                address = baseUrl + address;
            else if (address.StartsWith("mailto") || address.StartsWith("tel") || address.StartsWith("#") || address.StartsWith("null"))
                address = null;

            Uri outUri;
            if (!(Uri.TryCreate(address, UriKind.Absolute, out outUri) && (outUri.Scheme == Uri.UriSchemeHttp || outUri.Scheme == Uri.UriSchemeHttps)))
            {
                address = null;
            }
        }
        private void createDataTable() 
        {
            // Bazowa konstrukcja źródła danych. Definicja kolumn.
            dt = new DataTable();
            dt.Clear();
            dt.Columns.Add("Address").DefaultValue = "";
            dt.Columns.Add("Content Type").DefaultValue = "";
            dt.Columns.Add("Status Code").DefaultValue = "";
            dt.Columns.Add("Status").DefaultValue = "";
            dt.Columns.Add("Indexability").DefaultValue = "";
            dt.Columns.Add("Indexability Status").DefaultValue = "";
            dt.Columns.Add("IsInternal").DefaultValue = "";

            okienkoGui.bindDataTableToWszystkie(dt);
            okienkoGui.bindDataTableToZewnetrzne(dt);
            okienkoGui.bindDataTableToWewnetrzne(dt);
        }
        private void updateDataTable(PageFragment pf) {
            // Aktualizacja źródła danych przez umieszczenie danych w odpowiednich kolumnach
            DataRow row = dt.NewRow();
            row["Address"] = pf.Address;
            row["Content Type"] = pf.ContentType;
            row["Status Code"] = pf.StatusCode;
            row["Status"] = pf.Status;
            row["Indexability"] = pf.Indexability;
            row["Indexability Status"] = pf.IndexabilityStatus;

            // Ogarnij wszystkie title (może być ich na stronie 0 lub więcej, ilośc nieokreślona)
            int i = 1;
            foreach (Title title in pf.Titles) {
                if (titlesCounter < i) 
                {
                    dt.Columns.Add("Title " + i).DefaultValue = "";
                    dt.Columns.Add("Title Length " + i).DefaultValue = "";
                    dt.Columns.Add("Title Pixel Width " + i).DefaultValue = "";
                    titlesCounter++;
                }
                row["Title " + i] = title.TitleText;
                row["Title Length " + i] = title.TitleLength;
                row["Title Pixel Width " + i] = title.TitlePixelWidth;
                i++;
            }

            // Ogarnij wszystkie meta description (może być ich na stronie 0 lub więcej, ilośc nieokreślona)
            i = 1;
            foreach (MetaDescription desc in pf.MetaDescriptions)
            {
                if (metaDescriptionsCounter < i)
                {
                    dt.Columns.Add("Meta Description " + i).DefaultValue = "";
                    dt.Columns.Add("Meta Description Length " + i).DefaultValue = "";
                    dt.Columns.Add("Meta Description Pixel Width " + i).DefaultValue = "";
                    metaDescriptionsCounter++;
                }
                row["Meta Description " + i] = desc.MetaDescriptionText;
                row["Meta Description Length " + i] = desc.MetaDescriptionLength;
                row["Meta Description Pixel Width " + i] = desc.MetaDescriptionPixelWidth;
                i++;
            }
            row["IsInternal"] = pf.IsInternal;

            dt.Rows.Add(row);
        }
    }
}
