﻿using System;
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

        private bool crawlerAlive = true;
        private int maxIdleCount = 100;
        private int idleCount = 0;

        private List<string> pagesToCrawl = new List<string>();

        private int maxCrawling = 10;
        private int actualCrawling = 0;

        private int pagesCrawled = 0;
        private int pagesFound = 0;

        Form1 okienkoGui;

        private List<PageFragment> pageFragments;

        public Crawler(Form1 form1, string siteToCrawl)
        {
            pageFragments = new List<PageFragment>();
            okienkoGui = form1;
            this.baseUrl = siteToCrawl;
            pagesToCrawl.Add(siteToCrawl);
            maxCrawling = 10;
            actualCrawling = 0;

            Debug.Write("\nCrawler started with page: " + siteToCrawl + "\n\n"); //no to git, startuje
            //Console.WriteLine("Crawler started with page: {0}", siteToCrawl);

        }
        public void StartCrawl() 
        {
            pagesFound = 1;
            okienkoGui.UpdateCrawledStatus(pagesCrawled, pagesFound);
            
            //crawler loop
            //czeka czy są nowe linki do przeszukania

            //chce zrobic tutaj loopa aby nienasrało się np 10k tasków na zmiane wpierdalających sie do zasobów aż będzie wolne miejsce
            // ale zlapalem errora przy sprawdzaniu baseUrl, siteToCrawl

            while (crawlerAlive)
            {
                if (pagesToCrawl.Count > 0)
                {
                    if (actualCrawling < maxCrawling)
                    {
                        Debug.Write("\nStrona do przeszukania: " + pagesToCrawl[0] + "\n\n");
                        //Task<CrawlPage> task = CrawlPageMethod(pagesToCrawl[0]);
                        Task task1 = startCrawlingPage(pagesToCrawl[0]);
                        pagesToCrawl.RemoveAt(0);                       
                        okienkoGui.UpdateCrawlingStatus(actualCrawling);
                        okienkoGui.UpdateCrawledStatus(pagesCrawled, pagesFound);
                    }
                    else
                    {
                        Debug.Write("\n oczekuje na wolny watek\n");
                        Thread.Sleep(10);
                    }
                }
                else
                {
                    Debug.WriteLine("\n brak zadan " + idleCount);
                    idleCount++;
                    okienkoGui.UpdateIdleCounter(idleCount);
                    Thread.Sleep(10);
                    if (idleCount >= maxIdleCount)
                    {
                        crawlerAlive = false;
                        okienkoGui.UnblockSearchButton();
                    }
                }
            }
        }
        private async Task startCrawlingPage(string pageToCrawl)
        {            
            actualCrawling++;
            okienkoGui.UpdateCrawlingStatus(actualCrawling);

            var httpClient = new HttpClient();
            var html = await httpClient.GetStringAsync(pageToCrawl);
            var htmlDocument = new HtmlAgilityPack.HtmlDocument();
            htmlDocument.LoadHtml(html);

            if (Uri.Compare(new Uri(baseUrl), new Uri(pageToCrawl), UriComponents.Host, UriFormat.SafeUnescaped, StringComparison.CurrentCulture) == 0)
            {
                if (!crawledPages.Contains(pageToCrawl))
                {
                    // Dodaje stronę do listy stron, których nie chcę więcej przeglądać
                    crawledPages.Add(pageToCrawl);

                    var anchors = htmlDocument.DocumentNode.Descendants("a").ToList();

                    foreach (var anchor in anchors)
                    {
                        PageFragment pf = new PageFragment();
                        pf.address = anchor.GetAttributeValue("href", "null").ToString();

                        //dodawanie podstron do kolejki
                        //jezeli podstrona juz przejrzana, to nie dodawaj jej
                        if (!crawledPages.Contains(pf.address))
                        {
                            if (!pagesToCrawl.Contains(pf.address))
                            {

                                pagesToCrawl.Add(pf.address);
                                okienkoGui.AddUriToDataGridView(pf.address);
                                pagesFound++;

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
                    //strona powinna byc juz przejrzana
                    Debug.Write("strona: " + pageToCrawl + "powinna byc juz przejrzana\n");
                }
            }
            else
            {
                Debug.Write("\n strona: " + pageToCrawl + " wychodzi poza strone bazowa \n");
            }

            //zwolnienie miejsca
            pagesCrawled++;
            actualCrawling--;
            okienkoGui.UpdateCrawlingStatus(actualCrawling);
            okienkoGui.UpdateCrawledStatus(pagesCrawled, pagesFound);
        }
    }
}
