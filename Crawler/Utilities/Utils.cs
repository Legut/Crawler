using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Crawler.Utilities
{
    public static class Utils
    {
        // TODO: sortowanie liczb w widoku jest alfabetyczne zamiast wielkościowego
        public static readonly string[] SizeSuffixes = { "B", "KB", "MB", "GB", "TB" };

        #region crawlerOptions

        //--here be options for crawler and stuff--//        
        public static string configPath = Application.StartupPath + "\\opcjeCrawleraConfig.cfg";
        
        public static List<NumericUpDown> numericUpDowns = new List<NumericUpDown>();
        public static List<System.Windows.Forms.CheckBox> checkBoxes = new List<System.Windows.Forms.CheckBox>();
        public static int ImgSizeMax = 100;
        public static int ImgAltCharMax = 100;
        public static int H2CharMax = 70;
        public static int H1CharMax = 70;
        public static int UrlCharMax = 115;
        public static int DescCharMax = 100;
        public static int DescCharMin = 70;
        public static int DescPixMax = 1010;
        public static int DescPixMin = 400;
        public static int PagenameCharMax = 60;
        public static int PagenameCharMin = 30;
        public static int PagenamePixMax = 545;
        public static int PagenamePixMin = 200;
        public static bool ExtractPageSize = false;
        public static bool ExtractHash = false;
        public static bool ExtractTxtCodeRatio = false;
        public static bool ExtractWordCount = false;
        public static bool ExtractIndexability = false;
        public static bool ExtractH2 = false;
        public static bool ExtractH1 = false;
        public static bool ExtractMetaKeywords = false;
        public static bool ExtractMetadataDesc = false;
        public static bool ExtractPageTitle = false;
        public static bool CrawlSWF = false;
        public static bool CrawlJavaScript = false;
        public static bool CrawlCss = false;
        public static bool CrawlImages = false;
        public static int MaxPageSize = 1000000;
        public static int MaxLinksPerUrl = 10000000;
        public static int CrawlDepthLimit = 10000000;
        public static int TotalCrawlLimit = 100000000;
        //-----------------------------------------//
        public static int MaxSemaphores = 50;
        public static int iloscBledow = 0;

        #endregion

        public static Task WhenMouseUp(this Control control)
        {
            var tcs = new TaskCompletionSource<object>();
            MouseEventHandler onMouseUp = null;
            onMouseUp = (sender, e) =>
            {
                control.MouseUp -= onMouseUp;
                tcs.TrySetResult(null);
            };
            control.MouseUp += onMouseUp;
            return tcs.Task;
        }

        public static int CountWords(string text)
        {
            int wordCount = 0, index = 0;

            // skip whitespace until first word
            while (index < text.Length && char.IsWhiteSpace(text[index]))
                index++;

            while (index < text.Length)
            {
                // check if current char is part of a word
                while (index < text.Length && !char.IsWhiteSpace(text[index]))
                    index++;

                wordCount++;

                // skip whitespace until next word
                while (index < text.Length && char.IsWhiteSpace(text[index]))
                    index++;
            }

            return wordCount;
        }

        public static void GenerujDefaultoweOpcje()
        {
            using (StreamWriter sw = new StreamWriter(configPath))
            {
                sw.WriteLine("ImgSizeMax=102");
                sw.WriteLine("ImgAltCharMax=100");
                sw.WriteLine("H2CharMax=70");
                sw.WriteLine("H1CharMax=70");
                sw.WriteLine("UrlCharMax=115");
                sw.WriteLine("DescCharMax=100");
                sw.WriteLine("DescCharMin=70");
                sw.WriteLine("DescPixMax=1010");
                sw.WriteLine("DescPixMin=400");
                sw.WriteLine("PagenameCharMax=60");
                sw.WriteLine("PagenameCharMin=30");
                sw.WriteLine("PagenamePixMax=545");
                sw.WriteLine("PagenamePixMin=200");
                sw.WriteLine("MaxSemaphores=4");
                sw.WriteLine("MaxPageSize=1000000");
                sw.WriteLine("MaxLinksPerUrl=10000000");
                sw.WriteLine("CrawlDepthLimit=10000000");
                sw.WriteLine("TotalCrawlLimit=100000000");
            }
        }

        public static void ZaladujOpcje()
        {
            if (!File.Exists(configPath))
            {
                MessageBox.Show("Plik konfiguracyjny nie istnieje, stwórz konfigurację crawlera w opcjach!\n Załadowano domyślne opcje crawlingu.", "Brak opcji", MessageBoxButtons.OK);
                GenerujDefaultoweOpcje();
            }

            using (StreamReader sr = new StreamReader(Utils.configPath))
            {
                string line = "";
                while ((line = sr.ReadLine()) != null)
                {
                    string[] temp = line.Split('=');

                    switch (temp[0])
                    {
                        case "PagenamePixMin":
                            int.TryParse(temp[1], out PagenamePixMin);
                            break;
                        case "PagenamePixMax":
                            int.TryParse(temp[1], out PagenamePixMax);
                            break;
                        case "PagenameCharMin":
                            int.TryParse(temp[1], out PagenameCharMin);
                            break;
                        case "PagenameCharMax":
                            int.TryParse(temp[1], out PagenameCharMax);
                            break;
                        case "MaxSemaphores":
                            int.TryParse(temp[1], out MaxSemaphores);                            
                            break;
                        case "MaxPageSize":
                            int.TryParse(temp[1], out MaxPageSize);
                            break;
                        case "MaxLinksPerUrl":
                            int.TryParse(temp[1], out MaxLinksPerUrl);
                            break;
                        case "CrawlDepthLimit":
                            int.TryParse(temp[1], out CrawlDepthLimit);
                            break;
                        case "TotalCrawlLimit":
                            int.TryParse(temp[1], out TotalCrawlLimit);
                            break;
                        case "DescPixMin":
                            int.TryParse(temp[1], out DescPixMin);
                            break;
                        case "DescPixMax":
                            int.TryParse(temp[1], out DescPixMax);
                            break;
                        case "DescCharMin":
                            int.TryParse(temp[1], out DescCharMin);
                            break;
                        case "DescCharMax":
                            int.TryParse(temp[1], out DescCharMax);
                            break;
                        case "UrlCharMax":
                            int.TryParse(temp[1], out UrlCharMax);
                            break;
                        case "H1CharMax":
                            int.TryParse(temp[1], out H1CharMax);
                            break;
                        case "H2CharMax":
                            int.TryParse(temp[1], out H2CharMax);
                            break;
                        case "ImgAltCharMax":
                            int.TryParse(temp[1], out ImgAltCharMax);
                            break;
                        case "ImgSizeMax":
                            int.TryParse(temp[1], out ImgSizeMax);
                            break;
                        case "ExtractPageSize":
                            bool.TryParse(temp[1].ToLower(), out ExtractPageSize);
                            break;
                        case "ExtractHash":
                            bool.TryParse(temp[1].ToLower(), out ExtractHash);
                            break;
                        case "ExtractTxtCodeRatio":
                            bool.TryParse(temp[1].ToLower(), out ExtractTxtCodeRatio);
                            break;
                        case "ExtractWordCount":
                            bool.TryParse(temp[1].ToLower(), out ExtractWordCount);
                            break;
                        case "ExtractIndexability":
                            bool.TryParse(temp[1].ToLower(), out ExtractIndexability);
                            break;
                        case "ExtractH2":
                            bool.TryParse(temp[1].ToLower(), out ExtractH2);
                            break;
                        case "ExtractH1":
                            bool.TryParse(temp[1].ToLower(), out ExtractH1);
                            break;
                        case "ExtractMetaKeywords":
                            bool.TryParse(temp[1].ToLower(), out ExtractMetaKeywords);
                            break;
                        case "ExtractMetadataDesc":
                            bool.TryParse(temp[1].ToLower(), out ExtractMetadataDesc);
                            break;
                        case "ExtractPageTitle":
                            bool.TryParse(temp[1].ToLower(), out ExtractPageTitle);
                            break;
                        case "CrawlSWF":
                            bool.TryParse(temp[1].ToLower(), out CrawlSWF);
                            break;
                        case "CrawlJavaScript":
                            bool.TryParse(temp[1].ToLower(), out CrawlJavaScript);
                            break;
                        case "CrawlCss":
                            bool.TryParse(temp[1].ToLower(), out CrawlCss);
                            break;
                        case "CrawlImages":
                            bool.TryParse(temp[1].ToLower(), out CrawlImages);
                            break;
                    }
                }
            }

        }
   
    }
}
