﻿using HtmlAgilityPack;
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

        CancellationTokenSource cts;
        List<Task> taskList = new List<Task>();

        //GUI
        Form1 okienkoGui;
        private int przejrzaneStrony;
        private int stronyDoPrzejrzenia;

        // Data
        private DataTable dt;

        // Countery kolumn danego typu
        private int titlesCounter;
        private int metaDescriptionsCounter;
        private int metaKeywordsCounter;
        private int headingOneCounter;
        private int headingTwoCounter;

        // TODO: sortowanie liczb w widoku jest alfabetyczne zamiast wielkościowego
        static readonly string[] SizeSuffixes = { "B", "KB", "MB", "GB", "TB" };

        public Crawler(Form1 form1, string siteToCrawl)
        {
            okienkoGui = form1;
            baseUrl = siteToCrawl;

            cts = new CancellationTokenSource();

            // inicjacja counterów kolumn;
            titlesCounter = 1;
            metaDescriptionsCounter = 1;
            metaKeywordsCounter = 1;
            headingOneCounter = 1;
            headingTwoCounter = 1;

            // GUI
            okienkoGui.UpdateCrawlingStatus(maxSemaphores, maxSemaphores);
            przejrzaneStrony = 0;
            stronyDoPrzejrzenia = 1;
        }
        private async Task startCrawlingPage(string page, CancellationToken ctsToken)
        {
            try
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
                        if (!cts.IsCancellationRequested)
                        {
                            CrawlFurther(htmlDocument);
                        }

                        // Uzupełniam PageFragment danymi
                        pf.Address = page;
                        pf.IsInternal = true;

                        if (pf.Address != null)
                        {
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

                            // Przegląd wszystkich meta - description, keywords i sprawdzenie robots w poszukiwaniu noindex
                            pf.MetaDescriptions = new List<MetaDescription>();
                            pf.MetaKeywords = new List<MetaKeywords>();
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
                                else if (meta.GetAttributeValue("name", "null") == "description")
                                {
                                    MetaDescription metaDesc = new MetaDescription();
                                    metaDesc.MetaDescriptionText = meta.GetAttributeValue("content", "");
                                    metaDesc.MetaDescriptionLength = metaDesc.MetaDescriptionText.Length;

                                    Font arialBold = new Font("Arial", 13.0F);
                                    metaDesc.MetaDescriptionPixelWidth = System.Windows.Forms.TextRenderer.MeasureText(metaDesc.MetaDescriptionText, arialBold).Width;

                                    pf.MetaDescriptions.Add(metaDesc);
                                }
                                else if (meta.GetAttributeValue("name", "null") == "keywords")
                                {
                                    MetaKeywords metaKey = new MetaKeywords();
                                    metaKey.MetaKeywordsText = meta.GetAttributeValue("content", "");
                                    metaKey.MetaKeywordsLength = metaKey.MetaKeywordsText.Length;

                                    pf.MetaKeywords.Add(metaKey);
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

                            List<HtmlNode> htmlHeadingsOne = htmlDocument.DocumentNode.Descendants("h1").ToList();
                            pf.HeadingsOne = new List<HeadingOne>();
                            foreach (HtmlNode htmlHeadingOne in htmlHeadingsOne)
                            {
                                HeadingOne headingOne = new HeadingOne();
                                headingOne.HeadingOneText = htmlHeadingOne.InnerText;
                                headingOne.HeadingOneLength = headingOne.HeadingOneText.Length;

                                pf.HeadingsOne.Add(headingOne);
                            }

                            List<HtmlNode> htmlHeadingsTwo = htmlDocument.DocumentNode.Descendants("h2").ToList();
                            pf.HeadingsTwo = new List<HeadingTwo>();
                            foreach (HtmlNode htmlHeadingTwo in htmlHeadingsTwo)
                            {
                                HeadingTwo headingTwo = new HeadingTwo();
                                headingTwo.HeadingTwoText = htmlHeadingTwo.InnerText;
                                headingTwo.HeadingTwoLength = headingTwo.HeadingTwoText.Length;

                                pf.HeadingsTwo.Add(headingTwo);
                            }


                            pf.Size = response.Content.Headers.ContentLength.GetValueOrDefault();
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
                        pf.MetaKeywords = new List<MetaKeywords>();
                        pf.HeadingsOne = new List<HeadingOne>();
                        pf.HeadingsTwo = new List<HeadingTwo>();

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
                catch (WebException ex)
                {
                    string status = (ex.Response as HttpWebResponse).StatusCode.ToString();
                    pf.StatusCode = status;
                    Debug.WriteLine(" strona " + page + " WebEx: " + status);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(" strona " + page + " spotkala niezdefiniowany (nieobsłużony indywidualnie) wyjątek: " + e.Message);
                }
            }
            catch (OperationCanceledException)
            {
                Debug.WriteLine("Task anulowany");
            }
            catch (Exception)
            {
                Debug.WriteLine("Task sie nie powiodl");
            }

            // Zwalniam semafor
            this.semaphore.Release();
            UpdateDebugGui();
        }

        private void UpdateDebugGui()
        {
            // Aktualizuję GUI
            przejrzaneStrony++;
            okienkoGui.UpdateCrawlingStatus(semaphore.CurrentCount, maxSemaphores);
            okienkoGui.UpdateCrawledStatus(przejrzaneStrony, stronyDoPrzejrzenia);
        }

        public async void StartCrawl()
        {
            createDataTable();
            await startCrawlingPage(baseUrl, cts.Token);
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

                    taskList.Add(startCrawlingPage(address, cts.Token));
                    //startCrawlingPage(address);
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

            dt.Columns.Add("Title 1").DefaultValue = "";
            dt.Columns.Add("Title Length 1").DefaultValue = "";
            dt.Columns.Add("Title Pixel Width 1").DefaultValue = "";

            dt.Columns.Add("Meta Description 1").DefaultValue = "";
            dt.Columns.Add("Meta Description Length 1").DefaultValue = "";
            dt.Columns.Add("Meta Description Pixel Width 1").DefaultValue = "";

            dt.Columns.Add("Meta Keywords 1").DefaultValue = "";
            dt.Columns.Add("Meta Keywords Length 1").DefaultValue = "";

            dt.Columns.Add("H1 1").DefaultValue = "";
            dt.Columns.Add("H1 Length 1").DefaultValue = "";

            dt.Columns.Add("H2 1").DefaultValue = "";
            dt.Columns.Add("H2 Length 1").DefaultValue = "";

            dt.Columns.Add("Size").DefaultValue = "";

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
            row["IsInternal"] = pf.IsInternal;
            if (pf.IsInternal == true)
            {
                row["Size"] = SizeSuffix(pf.Size, 2);
            }
            else 
            {
                row["Size"] = "";
            }

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

            // Ogarnij wszystkie meta keywords (może być ich na stronie 0 lub więcej, ilośc nieokreślona)
            i = 1;
            foreach (MetaKeywords desc in pf.MetaKeywords)
            {
                if (metaKeywordsCounter < i)
                {
                    dt.Columns.Add("Meta Keywords " + i).DefaultValue = "";
                    dt.Columns.Add("Meta Keywords Length " + i).DefaultValue = "";
                    metaKeywordsCounter++;
                }
                row["Meta Keywords " + i] = desc.MetaKeywordsText;
                row["Meta Keywords Length " + i] = desc.MetaKeywordsLength;
                i++;
            }

            // Ogarnij wszystkie h1 (może być ich na stronie 0 lub więcej, ilośc nieokreślona)
            i = 1;
            foreach (HeadingOne headingOne in pf.HeadingsOne)
            {
                if (headingOneCounter < i)
                {
                    dt.Columns.Add("H1 " + i).DefaultValue = "";
                    dt.Columns.Add("H1 Length " + i).DefaultValue = "";
                    headingOneCounter++;
                }
                row["H1 " + i] = headingOne.HeadingOneText;
                row["H1 Length " + i] = headingOne.HeadingOneLength;
                i++;
            }

            // Ogarnij wszystkie h2 (może być ich na stronie 0 lub więcej, ilośc nieokreślona)
            i = 1;
            foreach (HeadingTwo headingTwo in pf.HeadingsTwo)
            {
                if (headingTwoCounter < i)
                {
                    dt.Columns.Add("H2 " + i).DefaultValue = "";
                    dt.Columns.Add("H2 Length " + i).DefaultValue = "";
                    headingTwoCounter++;
                }
                row["H2 " + i] = headingTwo.HeadingTwoText;
                row["H2 Length " + i] = headingTwo.HeadingTwoLength;
                i++;
            }

            dt.Rows.Add(row);
        }
        private static string SizeSuffix(Int64 value, int decimalPlaces = 1)
        {
            if (decimalPlaces < 0) { throw new ArgumentOutOfRangeException("decimalPlaces"); }
            if (value < 0) { return "-" + SizeSuffix(-value); }
            if (value == 0) { return string.Format("{0:n" + decimalPlaces + "} bytes", 0); }

            // mag is 0 for bytes, 1 for KB, 2, for MB, etc.
            int mag = (int)Math.Log(value, 1024);

            // 1L << (mag * 10) == 2 ^ (10 * mag) 
            // [i.e. the number of bytes in the unit corresponding to mag]
            decimal adjustedSize = (decimal)value / (1L << (mag * 10));

            // make adjustment when the value is large enough that
            // it would round up to 1000 or more
            if (Math.Round(adjustedSize, decimalPlaces) >= 1000)
            {
                mag += 1;
                adjustedSize /= 1024;
            }

            return string.Format("{0:n" + decimalPlaces + "} {1}", adjustedSize, SizeSuffixes[mag]);
        }
        public void AbortCrawl()
        {
            if (cts != null)
            {
                cts.Cancel();
            }
            else
            {
                //to nie powinno nigdy wyskoczyc ale kto wie xd
                Debug.WriteLine("CancelationToken jest null, nie mozna anulowac");
            }
        }
    }
}
