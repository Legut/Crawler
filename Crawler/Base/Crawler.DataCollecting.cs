using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Crawler.Utilities;
using HtmlAgilityPack;

namespace Crawler.Base
{
    partial class Crawler
    {
        private static void ManagePageFragmentIfExternal(ref PageFragment pf, ref HttpResponseMessage response, Uri page)
        {
            pf.Address = page.AbsoluteUri;
            pf.IsInternal = false;

            pf.ContentType = response.Content.Headers.ContentType.MediaType;
            pf.StatusCode = ((int)response.StatusCode).ToString();
            pf.Status = response.StatusCode.ToString();
            pf.UrlDepth = -1;
            foreach (string segment in page.Segments)
            {
                pf.UrlDepth++;
            }
        }
        private void ManagePageFragment(ref PageFragment pf, ref HttpResponseMessage response, ref HtmlDocument htmlDocument, Uri page)
        {
            pf.Address = page.AbsoluteUri;
            pf.IsInternal = true;
            pf.ContentType = response.Content.Headers.ContentType.MediaType;
            pf.StatusCode = ((int)response.StatusCode).ToString();
            pf.Status = response.StatusCode.ToString();
            if(Utils.ExtractPageSize)
                pf.Size = response.Content.Headers.ContentLength.GetValueOrDefault();

            if (Utils.ExtractHash)
            {
                pf.Hash = htmlDocument.Text.GetHashCode();
                string hash = pf.Hash.GetHashCode().ToString();
                var k = (from row in dt.Rows.OfType<DataRow>() where row[HASH_VALUE_COL].ToString() == hash select row)
                    .FirstOrDefault();
                if (k != null)
                {
                    k[ISDUPLICATE_COL] = true;
                    pf.IsDuplicate = true;
                }
            }

            if (Utils.ExtractWordCount || Utils.ExtractTxtCodeRatio)
            {
                List<HtmlNode> bodies = htmlDocument.DocumentNode.Descendants("body").ToList();
                int nonHtmlCharsCount = 0;
                foreach (HtmlNode body in bodies)
                {
                    pf.WordCount += Utils.CountWords(body.InnerText);
                    nonHtmlCharsCount += body.InnerText.Length;
                }

                if (Utils.ExtractTxtCodeRatio)
                {
                    if (htmlDocument.Text.Length != 0)
                        pf.TextRatio = (float) nonHtmlCharsCount / (float) htmlDocument.Text.Length;
                    else
                        pf.TextRatio = 0;
                }
            }

            pf.UrlDepth = page.Segments.Length-1;
          
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

            // Analyze all tags - description, keywords and checking rel robots to realize whether size is inexable
            AnalyzeMetas(ref pf, ref htmlDocument);
            AnalyzeTitles(ref pf, ref htmlDocument);
            AnalyzeHeadingsOne(ref pf, ref htmlDocument);
            AnalyzeHeadingsTwo(ref pf, ref htmlDocument);
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
                if (pf.Titles.Count == Utils.MaxTitles)
                    break;
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
                    if (pf.MetaDescriptions.Count == Utils.MaxDescs)
                        continue;

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
                    if (pf.MetaKeywords.Count == Utils.MaxKeywords)
                        continue;

                    MetaKeywords metaKey = new MetaKeywords();
                    metaKey.MetaKeywordsText = meta.GetAttributeValue("content", "");
                    metaKey.MetaKeywordsLength = metaKey.MetaKeywordsText.Length;

                    pf.MetaKeywords.Add(metaKey);
                }
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
                if (pf.HeadingsOne.Count == Utils.MaxHeadsOne)
                    break;
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
                if (pf.HeadingsTwo.Count == Utils.MaxHeadsTwo)
                    break;
            }
        }
    }
}
