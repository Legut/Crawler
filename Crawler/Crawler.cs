using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
        private static int maxSemaphores = 5;
        private readonly SemaphoreSlim semaphore = new SemaphoreSlim(maxSemaphores);
        private CancellationToken cancellationToken = default(CancellationToken);

        private List<PageFragment> pageFragments;

        //GUI
        Form1 okienkoGui;
        private int przejrzaneStrony;
        private int stronyDoPrzejrzenia;

        public Crawler(Form1 form1, string siteToCrawl)
        {
            pageFragments = new List<PageFragment>();
            okienkoGui = form1;
            baseUrl = siteToCrawl;

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
            // Update GUI
            okienkoGui.UpdateCrawlingStatus(semaphore.CurrentCount, maxSemaphores);

            // Dodaje do podstron już przecrawlowanych (bo nawet jesli to nie jest jeszcze przecrawlowane, to będzie crawlowane zaraz jak tylko semafor się zwolni)
            crawledPages.Add(page);

            // Czekam aż semafor będzie wolny
            await this.semaphore.WaitAsync(cancellationToken);

            try
            {
                // Sprawdzam czy powinienem Crawlować tą podstronę
                if (Uri.Compare(new Uri(baseUrl), new Uri(page), UriComponents.Host, UriFormat.SafeUnescaped, StringComparison.CurrentCulture) == 0)
                {
                    // Pobieram HTML podstrony
                    var httpClient = new HttpClient();
                    var html = await httpClient.GetStringAsync(page);
                    var htmlDocument = new HtmlAgilityPack.HtmlDocument();
                    htmlDocument.LoadHtml(html);

                    PageFragment pf = new PageFragment();
                    CrawlFurther(htmlDocument, ref pf);
                }
                else
                {
                    //Debug.Write(" strona: " + page + " wychodzi poza strone bazowa \n");
                }
            }
            catch (UriFormatException e)
            {
                Debug.Write(" strona: " + page + " Wywala UriFormatException \n");
            } catch (Exception e)
            {
                Debug.Write("Mamy any exception ziom");
            }

            // Zwalniam semafor
            this.semaphore.Release();

            // Update GUI
            przejrzaneStrony++;
            okienkoGui.UpdateCrawlingStatus(semaphore.CurrentCount, maxSemaphores);
            okienkoGui.UpdateCrawledStatus(przejrzaneStrony,stronyDoPrzejrzenia);

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
                pf.address = image.GetAttributeValue("src", "null").ToString();

                TryCrawlingNextPage(ref pf);
            }
        }
        private void CrawlThroughIframes(HtmlDocument htmlDocument, ref PageFragment pf)
        {
            // Pobieram wszystkie <iframe></iframe> z podstrony i rekurencyjnie zaczynam ich crawlowanie
            var iframes = htmlDocument.DocumentNode.Descendants("iframe").ToList();
            foreach (var iframe in iframes)
            {
                pf.address = iframe.GetAttributeValue("src", "null").ToString();

                TryCrawlingNextPage(ref pf);
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
                    pf.address = script.GetAttributeValue("src", "null").ToString();

                    TryCrawlingNextPage(ref pf);
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
                    pf.address = link.GetAttributeValue("href", "null").ToString();

                    TryCrawlingNextPage(ref pf);
                }
            }
        }
        private void CrawlThroughAnchors(HtmlDocument htmlDocument, ref PageFragment pf)
        {
            // Pobieram wszystkie <a></a> z podstrony i rekurencyjnie zaczynam ich crawlowanie
            var anchors = htmlDocument.DocumentNode.Descendants("a").ToList();
            foreach (var anchor in anchors)
            {
                pf.address = anchor.GetAttributeValue("href", "null").ToString();
                
                TryCrawlingNextPage(ref pf);
            }
        }
        private void TryCrawlingNextPage(ref PageFragment pf)
        {
            // Popraw adres tak aby był pełnym linkiem
            pf.NormalizeAddress(baseUrl);
            // Sprawdzam czy adres jest poprawny
            if (pf.address != null)
            {
                // Sprawdzam czy adres podstrony został już przecrawlowany zanim zacznę go crawlować
                if (!crawledPages.Contains(pf.address))
                {
                    // To jest tu tymczasowo, ogólnie dodawanie czegokolwiek do widoku, będzie odbywać się na samym końcu
                    okienkoGui.AddUriToDataGridView(pf.address);
                    // Zaczynam rekurencyjne crawlowanie kolejnej podstrony
                    stronyDoPrzejrzenia++;
                    Task task1 = startCrawlingPage(pf.address);
                }
            }
        }
    }
}
