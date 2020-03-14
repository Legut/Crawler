using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Crawler
{
    class Crawler
    {
        private string siteToCrawl { get; set; }
        private HashSet<string> crawledPages = new HashSet<string>();

        private int maxCrawling = 10;

        public Crawler(string siteToCrawl)
        {
            this.siteToCrawl = siteToCrawl;
        }
        public void StartCrawl() 
        {
            Task task1 = startCrawlingPage(siteToCrawl, siteToCrawl);
        }
        private async Task startCrawlingPage(string baseUrl, string siteToCrawl)
        {
            var httpClient = new HttpClient();
            var html = await httpClient.GetStringAsync(siteToCrawl);
            var htmlDocument = new HtmlAgilityPack.HtmlDocument();
            htmlDocument.LoadHtml(html);

            if (Uri.Compare(new Uri(baseUrl), new Uri(siteToCrawl), UriComponents.Host, UriFormat.SafeUnescaped, StringComparison.CurrentCulture) == 0) {
                if (!crawledPages.Contains(siteToCrawl))
                {
                    crawledPages.Add(siteToCrawl);
                    /*var imgs = htmlDocument.DocumentNode.Descendants("img").ToList();
                    foreach (var img in imgs)
                    {
                        var htmlImage = new HtmlImage();
                        htmlImage.src = img.GetAttributeValue("src", "null").ToString();
                        htmlImage.alt = img.GetAttributeValue("alt", "null").ToString();
                        htmlImage.title = img.GetAttributeValue("title", "null").ToString();
                        htmlImages.Add(htmlImage);
                        
                    }*/
                }
            }
        }
    }
}
