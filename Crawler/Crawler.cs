using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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

        /*private BindingList<PageFragment> pageFragments;*/

        //GUI
        Form1 okienkoGui;
        private int przejrzaneStrony;
        private int stronyDoPrzejrzenia;

        public Crawler(Form1 form1, string siteToCrawl)
        {
            okienkoGui = form1;
            baseUrl = siteToCrawl;

            // Przypięcie listy do gridView
            /*
            pageFragments = new BindingList<PageFragment>();
            pageFragments.RaiseListChangedEvents = true;
            pageFragments.ListChanged += new ListChangedEventHandler(okienkoGui.pageFragments_ListChanged);
            okienkoGui.SetDataSource(pageFragments);
            */

            //GUI
            okienkoGui.UpdateCrawlingStatus(maxSemaphores, maxSemaphores);
            przejrzaneStrony = 0;
            stronyDoPrzejrzenia = 1;
        }
        public void StartCrawl() 
        {

            Task task1 = startCrawlingPage(baseUrl);
        }
        private async Task startCrawlingPage(string page)
        {
            okienkoGui.UpdateCrawlingStatus(semaphore.CurrentCount, maxSemaphores); // Update GUI
            crawledPages.Add(page); // Dodaje do podstron już przecrawlowanych (bo nawet jesli to nie jest jeszcze przecrawlowane, to będzie crawlowane zaraz jak tylko semafor się zwolni)
            await this.semaphore.WaitAsync(cancellationToken); // Czekam aż semafor będzie wolny

            PageFragment pf = new PageFragment();

            // Pobieram HTML podstrony
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.GetAsync(page);

            try
            {
                // Sprawdzam czy powinienem Crawlować tą podstronę
                if (Uri.Compare(new Uri(baseUrl), new Uri(page), UriComponents.Host, UriFormat.SafeUnescaped, StringComparison.CurrentCulture) == 0)
                {
                    string html = await response.Content.ReadAsStringAsync();
                    HtmlDocument htmlDocument = new HtmlDocument();
                    htmlDocument.LoadHtml(html);

                    CrawlFurther(htmlDocument);

                    pf.Address = page;

                    if (pf.Address != null) { 
                        pf.ContentType = response.Content.Headers.ContentType.MediaType;
                        pf.StatusCode = ((int)response.StatusCode).ToString();
                        pf.Status = response.StatusCode.ToString();
                        pf.Indexability = "Indexable";

                        if (pf.StatusCode != "200") 
                        {
                            pf.Indexability = "Non-indexable";
                        }

                        var metas = htmlDocument.DocumentNode.Descendants("meta").ToList();
                        foreach (var meta in metas) 
                        {
                            if (meta.GetAttributeValue("name", "null") == "robots") 
                            {
                                if (meta.GetAttributeValue("content", "null") == "noindex")
                                {
                                    pf.Indexability = "Non-Indexable";
                                }
                            }
                        }

                        // pageFragments.Add(pf); // Dodaje element do listy dowiązanej do gridView
                        okienkoGui.AddUriToDataGridView(ref pf); // Dodaje rekord na sztywno
                    }
                }
                else
                {
                    pf.Address = page;
                    
                    pf.ContentType = response.Content.Headers.ContentType.MediaType;
                    pf.StatusCode = ((int)response.StatusCode).ToString();
                    pf.Status = response.StatusCode.ToString();
                    pf.Indexability = "";

                    // pageFragments.Add(pf); // Dodaje element do listy dowiązanej do gridView
                    okienkoGui.AddUriToDataGridView(ref pf); // Dodaje rekord na sztywno

                    //Debug.Write(" strona: " + page + " wychodzi poza strone bazowa \n");
                }
            }
            //zly format strony
            catch (UriFormatException e)
            {
                Debug.WriteLine(" strona: " + page + " Wywala UriFormatException.");
            }
            //404
            catch (WebException ex) when ((ex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.NotFound)
            {
                pf.StatusCode = "404";
                Debug.WriteLine(" strona " + page + " jest niedostepna.");
            }
            //inne bledy Web
            catch(WebException ex)
            {
                
                string status = (ex.Response as HttpWebResponse).StatusCode.ToString();
                pf.StatusCode = status;
                Debug.WriteLine(" strona " + page + " WebEx: " + status);
            }
            //inne wyjatki
            catch (Exception e)
            {
                Debug.WriteLine(" strona " + page + " spotkala wyjatek: " + e.Message);
            }

            // Zwalniam semafor
            this.semaphore.Release();

            // Update GUI
            przejrzaneStrony++;
            okienkoGui.UpdateCrawlingStatus(semaphore.CurrentCount, maxSemaphores);
            okienkoGui.UpdateCrawledStatus(przejrzaneStrony,stronyDoPrzejrzenia);

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
                    Task task1 = startCrawlingPage(address);
                }
            }
        }
        private void NormalizeAddress(string baseUrl, ref string address)
        {
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

            if (!(Uri.TryCreate(address, UriKind.Absolute, out outUri)
               && (outUri.Scheme == Uri.UriSchemeHttp || outUri.Scheme == Uri.UriSchemeHttps)))
            {
                address = null;
            }
        }
    }
}
