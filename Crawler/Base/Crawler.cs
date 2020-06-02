using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Crawler.MainForm;
using Crawler.Utilities;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;
using Crawler.Elements;
using static Crawler.Utilities.Utils;
using System.Windows.Forms;
using System.IO;

namespace Crawler.Base
{
    partial class Crawler
    {
        private Uri BaseUrl { get; set; }
        private readonly HashSet<Uri> crawledPages;
        private static Dictionary<string, InLinksCounter> inLinksData;


        private readonly SemaphoreSlim semaphore;
        private CancellationToken cancellationToken;
        private readonly CancellationTokenSource cts;

        public readonly MainForm.MainForm MainForm;
        private int przejrzaneStrony;
        private int stronyDoPrzejrzenia;

        private DataTable dt;

        public Crawler(MainForm.MainForm mainForm, string siteToCrawl)
        {
            MainForm = mainForm;

            LoadCrawlingOptions();

            BaseUrl = new Uri(siteToCrawl);
            semaphore = new SemaphoreSlim(MaxSemaphores);
            cts = new CancellationTokenSource();
            crawledPages = new HashSet<Uri>();
            inLinksData = new Dictionary<string, InLinksCounter>();            
            cancellationToken = default;
            
            przejrzaneStrony = 0;
            stronyDoPrzejrzenia = 1;
        }

        private void LoadCrawlingOptions()
        {
            //TODO: updateGui
            MainForm.Invalidate();
            MainForm.Update();
        }

        private async Task StartCrawlingPage(Uri page, CancellationToken ctsToken)
        {
            MainForm.UpdateCrawlingStatus(semaphore.CurrentCount, MaxSemaphores);
            crawledPages.Add(page);

            // Wait for semaphore
            await this.semaphore.WaitAsync(cancellationToken);
            try {
                // Checking cancelation token (checking whether stop button has been pressed)
                if (!cts.IsCancellationRequested)
                {
                    PageFragment pf = new PageFragment {Address = page.AbsoluteUri};

                    //TODO: Dobrze zaznaczyc błędy/*
                    if (pf.Address.Length < PagenameCharMin || pf.Address.Length > PagenameCharMax)
                    {
                        MainForm.IncreaseErrorCount();
                    }

                    // Download page
                    HttpClient httpClient = new HttpClient();
                    HttpResponseMessage response = await httpClient.GetAsync(page);
                    try
                    {
                        // Check whether page is internal or external 
                        if (Uri.Compare(BaseUrl, page, UriComponents.Host, 
                            UriFormat.SafeUnescaped, StringComparison.CurrentCulture) == 0)
                        {
                            // Get page source
                            string sourceHtml = await response.Content.ReadAsStringAsync();
                            HtmlDocument htmlDocument = new HtmlDocument();
                            htmlDocument.LoadHtml(sourceHtml);

                            // Crawl deeper through urls found on this page
                            // (it happens in separate threads simultanously)
                            CrawlFurther(htmlDocument, ref pf);

                            // Fulfill PageFragment with data
                            ManagePageFragment(ref pf, ref response, ref htmlDocument, page);

                            // Update data source
                            UpdateDataTable(pf);
                        }
                        else
                        {
                            // Fulfill PageFragment with data
                            ManagePageFragmentIfExternal(ref pf, ref response, page);

                            // Update data source
                            UpdateDataTable(pf);
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
                            string status = (ex.Response as HttpWebResponse)?.StatusCode.ToString();
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

            this.semaphore.Release();
        }

        private void UpdateDebugGui()
        {
            MainForm.UpdateCrawlingStatus(semaphore.CurrentCount, MaxSemaphores);
            MainForm.UpdateCrawledStatus(przejrzaneStrony, stronyDoPrzejrzenia);
            przejrzaneStrony++;
        }
        public async void StartCrawl()
        {
            CreateDataTable();
            await StartCrawlingPage(BaseUrl, cts.Token);
            await OnAbortionComplete();
        }
        private static void NormalizeAddress(Uri baseUrl, ref string address, string pfAddress)
        {
            // Addresses starting with protocol (http / https) are considered complete. 
            if (!address.StartsWith("http://") && !address.StartsWith("https://"))
            {
                // Checking whether url is a proper relative. Here we exclude addreses like "#whatever", or "tel:123123123", or empty ones - "" 
                bool isProperRelative = Uri.IsWellFormedUriString(address, UriKind.Relative);
                if (isProperRelative)
                {
                    // Addresses like "//google.com" should be considered as "http://google.com"
                    // Addresses like "/abc/abc.jpg" should be considered as "http://root.com/abc/abc.jpg"
                    // Addresses like "abc.png" should be considered as "http://root.com/abc/abc.png" when clicked at "http://root.com/abc/"
                    // and as "http://root.com/xyz/aaa/abc.png" when clicked at "http://root.com/xyz/aaa/" and so on...
                    if (address.StartsWith("//"))
                        address = "http:" + address;
                    else if (address.StartsWith("/"))
                        address = baseUrl.AbsoluteUri + address.Substring(1);
                    else
                        address = pfAddress.Substring(0, pfAddress.LastIndexOf("/") + 1) + address;
                }
                else
                {
                    address = null;
                }
            }

            // Address which is null will be discarded in further operations (not in this method).
            // However if address is not null than it has to take last test. It needs to be proper absolute String.
            // TODO: Resarch how to improve it.
            if(address!=null)
            { 
                bool isProperAbsolute = Uri.IsWellFormedUriString(Uri.UnescapeDataString(address), UriKind.Absolute);
                if (!isProperAbsolute)
                    address = null;
            }
        }
        public static string SizeSuffix(long value, int decimalPlaces = 1)
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

            return string.Format("{0:n" + decimalPlaces + "} {1}", adjustedSize, Utils.SizeSuffixes[mag]);
        }
        public void AbortCrawl()
        {
            if (cts != null)
            {
                cts.Cancel();
            }
            else
            {
                Debug.WriteLine("CancelationToken is null, cannot abort!");
            }
        }

        private async Task OnAbortionComplete()
        {
            while (semaphore.CurrentCount != MaxSemaphores)
            {
                await Task.Delay(1000);
            }

            UpdateInLinks();
            UpdateDebugGui();
            MainForm.MakeButtonReady();
        }

        private void UpdateInLinks()
        {
            foreach (var item in inLinksData)
            {
                var singleRow = (from row in dt.Rows.OfType<DataRow>() where row[ADDRESS_COL].ToString() == item.Key select row).FirstOrDefault();
                if (singleRow != null)
                {
                    singleRow[INLINKS_COL] = item.Value.InLinksCount;
                    singleRow[UNIQUE_INLINKS_COL] = item.Value.UniqueInLinks.Count;
                    float temp = ((float)item.Value.UniqueInLinks.Count / (float)item.Value.InLinksCount) * 100;
                    singleRow[UNIQUE_INLINKS_OF_TOTAL_COL] = temp.ToString("F");
                }
            }
        }
    }   
}
