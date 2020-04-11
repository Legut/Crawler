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
        private string BaseUrl { get; set; }
        private HashSet<string> crawledPages;
        private const int MaxSemaphores = 50;
        private readonly SemaphoreSlim semaphore;
        private CancellationToken cancellationToken;
        private readonly CancellationTokenSource cts;

        //GUI
        public readonly Form1 MainForm;
        private int przejrzaneStrony;
        private int stronyDoPrzejrzenia;

        // Data
        private DataTable dt;

        // column counters
        private int titlesCounter;
        private int metaDescriptionsCounter;
        private int metaKeywordsCounter;
        private int headingOneCounter;
        private int headingTwoCounter;

        // TODO: sortowanie liczb w widoku jest alfabetyczne zamiast wielkościowego
        private static readonly string[] SizeSuffixes = { "B", "KB", "MB", "GB", "TB" };

        public Crawler(Form1 form1, string siteToCrawl)
        {
            MainForm = form1;
            BaseUrl = siteToCrawl;

            cts = new CancellationTokenSource();
            crawledPages = new HashSet<string>();
            semaphore = new SemaphoreSlim(MaxSemaphores);
            cancellationToken = default;

            // columns counter initiation
            titlesCounter = 1;
            metaDescriptionsCounter = 1;
            metaKeywordsCounter = 1;
            headingOneCounter = 1;
            headingTwoCounter = 1;

            // GUI
            MainForm.UpdateCrawlingStatus(MaxSemaphores, MaxSemaphores);
            przejrzaneStrony = 0;
            stronyDoPrzejrzenia = 1;
        }
        private async Task StartCrawlingPage(string page, CancellationToken ctsToken)
        {
            MainForm.UpdateCrawlingStatus(semaphore.CurrentCount, MaxSemaphores); // Update GUI
            crawledPages.Add(page); // Dodaje do podstron już przecrawlowanych (bo nawet jesli to nie jest jeszcze przecrawlowane, to będzie crawlowane zaraz jak tylko semafor się zwolni)
            await this.semaphore.WaitAsync(cancellationToken); // Czekam aż semafor będzie wolny
            try {
                // Sprawdzam cancelation token - czyli sprawdzam czy nie wciśniętu guzika stop
                if (!cts.IsCancellationRequested)
                {
                    PageFragment pf = new PageFragment();

                    // Pobieram podstronę
                    HttpClient httpClient = new HttpClient();
                    HttpResponseMessage response = await httpClient.GetAsync(page);

                    try
                    {
                        if (Uri.Compare(new Uri(BaseUrl), new Uri(page), UriComponents.Host, UriFormat.SafeUnescaped, StringComparison.CurrentCulture) == 0)
                        {
                            // Podstrona jest wewnętrzna
                            // Wyjmuję Html jako string
                            string html = await response.Content.ReadAsStringAsync();
                            HtmlDocument htmlDocument = new HtmlDocument();
                            htmlDocument.LoadHtml(html);

                            // Głębsze crawlowanie znalezionych na podstronie linków, w osobnych wątkach
                            CrawlFurther(htmlDocument, ref pf);

                            // Uzupełnij PageFragment danymi
                            ManagePageFragment(ref pf, ref response, ref htmlDocument, page);

                            // Aktualizuję źródło danych
                            UpdateDataTable(pf);
                        }
                        else
                        {
                            // Podstrona jest zewnętrzna
                            // Uzupełnij PageFragment danymi
                            ManagePageFragmentIfExternal(ref pf, ref response, page);

                            // Aktualizuję źródło danych
                            UpdateDataTable(pf);
                            // Debug.Write(" strona: " + page + " wychodzi poza strone bazowa \n");
                        }
                    }
                    catch (UriFormatException ex)
                    {
                        Debug.WriteLine(" Podstrona: " + page + " ma niepoprawnie sformatowany url. Message: " + ex.Message);
                    }
                    catch (WebException ex) when ((ex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.NotFound)
                    {
                        pf.StatusCode = "404";
                        Debug.WriteLine(" strona " + page + " jest niedostepna -> 404 NotFound");
                    }
                    catch (WebException ex)
                    {
                        try
                        {
                            string status = (ex.Response as HttpWebResponse).StatusCode.ToString();
                            pf.StatusCode = status;
                            Debug.WriteLine(" strona " + page + " WebEx: " + status);
                        }
                        catch (NullReferenceException e)
                        {
                            pf.StatusCode = "Undefined";
                            Debug.WriteLine(" strona " + page + " WebEx: Undefined. Message: " + e );
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(" strona " + page + " spotkala niezdefiniowany (nieobsłużony indywidualnie) wyjątek: " + ex.Message);
                    }

                    UpdateDebugGui();
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
        }

        private static void ManagePageFragmentIfExternal(ref PageFragment pf, ref HttpResponseMessage response, string page)
        {
            pf.Address = page;
            pf.IsInternal = false;

            pf.ContentType = response.Content.Headers.ContentType.MediaType;
            pf.StatusCode = ((int)response.StatusCode).ToString();
            pf.Status = response.StatusCode.ToString();
        }
        private static void ManagePageFragment(ref PageFragment pf, ref HttpResponseMessage response, ref HtmlDocument htmlDocument, string page)
        {
            pf.Address = page;
            pf.IsInternal = true;
            pf.ContentType = response.Content.Headers.ContentType.MediaType;
            pf.StatusCode = ((int)response.StatusCode).ToString();
            pf.Status = response.StatusCode.ToString();
            pf.Size = response.Content.Headers.ContentLength.GetValueOrDefault();

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
            AnalyzeMetas(ref pf, ref htmlDocument);
            AnalyzeTitles(ref pf, ref htmlDocument);
            AnalyzeHeadingsOne(ref pf, ref htmlDocument);
            AnalyzeHeadingsTwo(ref pf, ref htmlDocument);
        }
        private void UpdateDebugGui()
        {
            // Aktualizuję GUI
            przejrzaneStrony++;
            MainForm.UpdateCrawlingStatus(semaphore.CurrentCount, MaxSemaphores);
            MainForm.UpdateCrawledStatus(przejrzaneStrony, stronyDoPrzejrzenia);
        }
        public async void StartCrawl()
        {
            CreateDataTable();
            await StartCrawlingPage(BaseUrl, cts.Token);
        }
        private static void NormalizeAddress(string baseUrl, ref string address)
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
        private void CreateDataTable()
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

            dt.Columns.Add("OutLinks").DefaultValue = "";
            dt.Columns.Add("UniqueOutLinks").DefaultValue = "";
            dt.Columns.Add("UniqueOutLinksOfTotal").DefaultValue = "";

            dt.Columns.Add("ExternalOutLinks").DefaultValue = "";
            dt.Columns.Add("UniqueExternalOutLinks").DefaultValue = "";
            dt.Columns.Add("UniqueExternalOutLinksOfTotal").DefaultValue = "";

            MainForm.BindDataTableToWszystkie(dt);
            MainForm.BindDataTableToZewnetrzne(dt);
            MainForm.BindDataTableToWewnetrzne(dt);
        }
        private void UpdateDataTable(PageFragment pf)
        {
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
            foreach (Title title in pf.Titles)
            {
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
                if (i == 3) break;
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
                if (i == 3) break;
                i++;
            }

            if (pf.OutLinks > 0)
            {
                row["OutLinks"] = pf.OutLinks;
                row["UniqueOutLinks"] = pf.UniqueOutLinks;
                row["UniqueOutLinksOfTotal"] = ((double)pf.UniqueOutLinks / (double)pf.OutLinks) * 100;
            }

            if (pf.ExternalOutLinks > 0)
            {
                row["ExternalOutLinks"] = pf.ExternalOutLinks;
                row["UniqueExternalOutLinks"] = pf.UniqueExternalOutLinks;
                row["UniqueExternalOutLinksOfTotal"] = ((double)pf.UniqueExternalOutLinks / (double)pf.ExternalOutLinks) * 100;
            }

            dt.Rows.Add(row);
        }
        private static string SizeSuffix(long value, int decimalPlaces = 1)
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

        private void CrawlFurther(HtmlDocument htmlDocument, ref PageFragment pf)
        {
            CrawlThroughAnchors(htmlDocument, ref pf);
            CrawlThroughLinks(htmlDocument, ref pf);
            CrawlThroughScripts(htmlDocument, ref pf);
            CrawlThroughIframes(htmlDocument, ref pf);
            CrawlThroughImages(htmlDocument, ref pf);
        }
        private void CrawlThroughImages(HtmlDocument htmlDocument, ref PageFragment pf)
        {
            // Pobieram wszystkie <img> z podstrony i rekurencyjnie zaczynam ich crawlowanie
            var images = htmlDocument.DocumentNode.Descendants("img").ToList();
            foreach (var image in images)
            {
                string address = image.GetAttributeValue("src", "null").ToString();

                TryCrawlingNextPage(address, ref pf);
            }
        }
        private void CrawlThroughIframes(HtmlDocument htmlDocument, ref PageFragment pf)
        {
            // Pobieram wszystkie <iframe></iframe> z podstrony i rekurencyjnie zaczynam ich crawlowanie
            var iframes = htmlDocument.DocumentNode.Descendants("iframe").ToList();
            foreach (var iframe in iframes)
            {
                string address = iframe.GetAttributeValue("src", "null").ToString();

                TryCrawlingNextPage(address, ref pf);
            }
        }
        private void CrawlThroughScripts(HtmlDocument htmlDocument, ref PageFragment pf)
        {
            // Pobieram wszystkie <script></script> z podstrony i rekurencyjnie zaczynam ich crawlowanie
            var scripts = htmlDocument.DocumentNode.Descendants("script").ToList();
            foreach (var script in scripts)
            {
                if (script.GetAttributeValue("type", "null") == "text/javascript")
                {
                    string address = script.GetAttributeValue("src", "null").ToString();

                    TryCrawlingNextPage(address, ref pf);
                }
            }
        }
        private void CrawlThroughLinks(HtmlDocument htmlDocument, ref PageFragment pf)
        {
            // Pobieram wszystkie <link></link> z podstrony i rekurencyjnie zaczynam ich crawlowanie
            var links = htmlDocument.DocumentNode.Descendants("link").ToList();
            foreach (var link in links)
            {
                if (link.GetAttributeValue("rel", "null") == "stylesheet")
                {
                    string address = link.GetAttributeValue("href", "null").ToString();

                    TryCrawlingNextPage(address, ref pf);
                }
            }
        }
        private void CrawlThroughAnchors(HtmlDocument htmlDocument, ref PageFragment pf)
        {
            // Pobieram wszystkie <a></a> z podstrony i rekurencyjnie zaczynam ich crawlowanie
            var anchors = htmlDocument.DocumentNode.Descendants("a").ToList();
            foreach (var anchor in anchors)
            {
                string address = anchor.GetAttributeValue("href", "null").ToString();
                
                TryCrawlingNextPage(address, ref pf);
            }
        }
        private void TryCrawlingNextPage(string address, ref PageFragment pf)
        {
            
            // Popraw adres tak aby był pełnym linkiem
            NormalizeAddress(BaseUrl, ref address);
            // Sprawdzam czy adres jest poprawny
            if (address != null)
            {
                // Sprawdzam czy adres podstrony został już przecrawlowany zanim zacznę go crawlować
                if (!crawledPages.Contains(address))
                {
                    // Zaczynam rekurencyjne crawlowanie kolejnej podstrony
                    stronyDoPrzejrzenia++;

                    StartCrawlingPage(address, cts.Token);
                    //StartCrawlingPage(address);
                } 
                else
                {
                    if (Uri.Compare(new Uri(BaseUrl), new Uri(address), UriComponents.Host, UriFormat.SafeUnescaped, StringComparison.CurrentCulture) == 0)
                    {
                        pf.OutLinks++;
                        if (!pf.OutLinksAdresses.Contains(address))
                        {
                            pf.OutLinksAdresses.Add(address);
                            pf.UniqueOutLinks++;
                        }
                    }
                    else
                    {
                        pf.ExternalOutLinks++;
                        if (!pf.ExternalOutLinksAdresses.Contains(address))
                        {
                            pf.ExternalOutLinksAdresses.Add(address);
                            pf.UniqueExternalOutLinks++;
                        }
                    }
                }
            }
        }

        private static void AnalyzeHeadingsTwo(ref PageFragment pf, ref HtmlDocument htmlDocument)
        {
            List<HtmlNode> htmlHeadingsTwo = htmlDocument.DocumentNode.Descendants("h2").ToList();
            foreach (HtmlNode htmlHeadingTwo in htmlHeadingsTwo)
            {
                HeadingTwo headingTwo = new HeadingTwo();
                headingTwo.HeadingTwoText = htmlHeadingTwo.InnerText;
                headingTwo.HeadingTwoLength = headingTwo.HeadingTwoText.Length;

                pf.HeadingsTwo.Add(headingTwo);
            }
        }
        private static void AnalyzeHeadingsOne(ref PageFragment pf, ref HtmlDocument htmlDocument)
        {
            List<HtmlNode> htmlHeadingsOne = htmlDocument.DocumentNode.Descendants("h1").ToList();
            foreach (HtmlNode htmlHeadingOne in htmlHeadingsOne)
            {
                HeadingOne headingOne = new HeadingOne();
                headingOne.HeadingOneText = htmlHeadingOne.InnerText;
                headingOne.HeadingOneLength = headingOne.HeadingOneText.Length;

                pf.HeadingsOne.Add(headingOne);
            }
        }
        private static void AnalyzeTitles(ref PageFragment pf, ref HtmlDocument htmlDocument)
        {
            List<HtmlNode> htmlTitles = htmlDocument.DocumentNode.Descendants("title").ToList();
            foreach (HtmlNode htmlTitle in htmlTitles)
            {
                Title title = new Title();
                title.TitleText = htmlTitle.InnerText;
                title.TitleLength = title.TitleText.Length;

                Font arialBold = new Font("Arial", 16.0F);
                title.TitlePixelWidth = System.Windows.Forms.TextRenderer
                    .MeasureText(title.TitleText, arialBold).Width;

                pf.Titles.Add(title);
            }
        }
        private static void AnalyzeMetas(ref PageFragment pf, ref HtmlDocument htmlDocument)
        {
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
                    metaDesc.MetaDescriptionPixelWidth = System.Windows.Forms.TextRenderer
                        .MeasureText(metaDesc.MetaDescriptionText, arialBold).Width;

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
