using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crawler.Elements;
using Crawler.Utilities;
using HtmlAgilityPack;

namespace Crawler.Base
{
    partial class Crawler
    {
        private void CrawlFurther(HtmlDocument htmlDocument, ref PageFragment pf)
        {
            CrawlThroughAnchors(htmlDocument, ref pf);
            if (Utils.CrawlCss)
                CrawlThroughLinks(htmlDocument, ref pf);
            if (Utils.CrawlJavaScript)
                CrawlThroughScripts(htmlDocument, ref pf);
            if(Utils.CrawlIframes)
                CrawlThroughIframes(htmlDocument, ref pf);
            if (Utils.CrawlImages)
                CrawlThroughImages(htmlDocument, ref pf);
        }
        private void CrawlThroughAnchors(HtmlDocument htmlDocument, ref PageFragment pf)
        {
            // Collect all anchor tags (<a/>) and start crawling urls inside href attribute
            var anchors = htmlDocument.DocumentNode.Descendants("a").ToList();
            foreach (var anchor in anchors)
            {
                string address = anchor.GetAttributeValue("href", String.Empty);
                TryCrawlingNextPage(address, ref pf);
            }
        }
        private void CrawlThroughLinks(HtmlDocument htmlDocument, ref PageFragment pf)
        {
            // Collect all link tags (<link/>) and start crawling urls inside href attribute
            // Only interested in .CSS files
            var links = htmlDocument.DocumentNode.Descendants("link").ToList();
            foreach (var link in links)
            {
                if (link.GetAttributeValue("rel", "null") == "stylesheet")
                {
                    string address = link.GetAttributeValue("href", String.Empty);
                    if (address.Contains("?"))
                        address = address.Remove(address.IndexOf("?"));
                    TryCrawlingNextPage(address, ref pf);
                }
            }
        }
        private void CrawlThroughScripts(HtmlDocument htmlDocument, ref PageFragment pf)
        {
            // Collect all script tags (<script/>) and start crawling urls inside src attribute
            // Only interested in .js files
            var scripts = htmlDocument.DocumentNode.Descendants("script").ToList();
            foreach (var script in scripts)
            {
                if (script.GetAttributeValue("type", "null") == "text/javascript")
                {
                    string address = script.GetAttributeValue("src", String.Empty);
                    TryCrawlingNextPage(address, ref pf);
                }
            }
        }
        private void CrawlThroughIframes(HtmlDocument htmlDocument, ref PageFragment pf)
        {
            // Collect all iframe tags (<iframe/>) and start crawling urls inside src attribute
            var iframes = htmlDocument.DocumentNode.Descendants("iframe").ToList();
            foreach (var iframe in iframes)
            {
                string address = iframe.GetAttributeValue("src", String.Empty);
                TryCrawlingNextPage(address, ref pf);
            }
        }
        private void CrawlThroughImages(HtmlDocument htmlDocument, ref PageFragment pf)
        {
            // Collect all image tags (<img/>) and start crawling urls inside src attribute
            var images = htmlDocument.DocumentNode.Descendants("img").ToList();
            foreach (var image in images)
            {
                string address = image.GetAttributeValue("src", String.Empty);
                if (address.Contains("?"))
                    address = address.Remove(address.IndexOf("?"));
                TryCrawlingNextPage(address, ref pf);
            }
        }
        private void TryCrawlingNextPage(string address, ref PageFragment pf)
        {
            // Refactor addres to make it a full absolute URL
            NormalizeAddress(BaseUrl, ref address, pf.Address);
            // Check whether address is correct or not
            if (address == null) return;

            if (Uri.Compare(BaseUrl, new Uri(address), UriComponents.Host, UriFormat.SafeUnescaped, StringComparison.CurrentCulture) == 0)
            {
                pf.OutLinks++;
                if (!pf.OutLinksAdresses.Contains(address))
                {
                    pf.OutLinksAdresses.Add(address);
                    pf.UniqueOutLinks++;
                }
                if (!inLinksData.ContainsKey(address))
                    inLinksData.Add(address, new InLinksCounter());

                inLinksData[address].InLinksCount++;
                inLinksData[address].UniqueInLinks.Add(pf.Address);
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

            // Check whether address had been crawled before or not
            if (crawledPages.Contains(new Uri(address))) { return; }
            // Check whether total crawl limit has been exceeded
            if (crawledPages.Count >= Utils.TotalCrawlLimit) { return; }

            pagesTovisit++;
            _ = StartCrawlingPage(new Uri(address), cts.Token);
        }
    }
}
