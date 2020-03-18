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

                    // Pobieram wszystkie <a></a> z podstrony i rekurencyjnie zaczynam ich crawlowanie
                    var anchors = htmlDocument.DocumentNode.Descendants("a").ToList();
                    foreach (var anchor in anchors)
                    {
                        PageFragment pf = new PageFragment();
                        pf.address = anchor.GetAttributeValue("href", "null").ToString();
                        // Popraw adres tak aby był pełnym linkiem
                        pf.NormalizeAddress(baseUrl);

                        // Sprawdzam czy adres podstrony został już przecrawlowany zanim zacznę go crawlować
                        if (pf.address != null) { 
                            if (!crawledPages.Contains(pf.address))
                            {
                                // To jest tu tymczasowo, ogólnie dodawanie czegokolwiek do widoku, będzie odbywać się na samym końcu
                                if (pf.address.Contains("oferta")) 
                                {
                                    Debug.Write(" oferta: " + pf.address + "\n");
                                }
                                okienkoGui.AddUriToDataGridView(pf.address);
                                // Zaczynam rekurencyjne crawlowanie kolejnej podstrony
                                stronyDoPrzejrzenia++;
                                Task task1 = startCrawlingPage(pf.address);
                            }
                        }
                        /* foreach (var img in imgs)
                        {
                            var htmlImage = new HtmlImage();
                            htmlImage.src = img.GetAttributeValue("src", "null").ToString();
                            htmlImage.alt = img.GetAttributeValue("alt", "null").ToString();
                            htmlImage.title = img.GetAttributeValue("title", "null").ToString();
                            htmlImages.Add(htmlImage);

                        }*/
                    }

                }
                else
                {
                    Debug.Write(" strona: " + page + " wychodzi poza strone bazowa \n");
                }
            }
            catch (UriFormatException e) 
            {
                Debug.Write(" strona: " + page + " Wywala UriFormatException \n");
            }

            // Zwalniam semafor
            this.semaphore.Release();

            // Update GUI
            przejrzaneStrony++;
            okienkoGui.UpdateCrawlingStatus(semaphore.CurrentCount, maxSemaphores);
            okienkoGui.UpdateCrawledStatus(przejrzaneStrony,stronyDoPrzejrzenia);

        }
    }
}
