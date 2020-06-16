using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public static readonly string[] SizeSuffixes = { "B", "KB", "MB", "GB", "TB" };
        public static readonly string ConfigPath = Application.StartupPath + "\\opcjeCrawleraConfig.cfg";
        #region crawlerOptions

        //--here be options for crawler and stuff--//        
        public static string ConfigFilePath = Application.StartupPath + "\\opcjeCrawleraConfig.cfg";
        
        public static int ImgSizeMax = 100;
        public static int ImgAltCharMax = 100;
        public static int H2CharMax = 70;
        public static int H1CharMax = 70;
        public static int UrlCharMax = 115;
        public static int DescCharMax = 100;
        public static int DescCharMin = 70;
        public static int DescPixMax = 1010;
        public static int DescPixMin = 400;
        public static int TitleCharMax = 60;
        public static int TitleCharMin = 30;
        public static int TitlePixMax = 545;
        public static int TitlePixMin = 200;
        public static bool ExtractPageSize = true;
        public static bool ExtractHash = true;
        public static bool ExtractTxtCodeRatio = true;
        public static bool ExtractWordCount = true;
        public static bool ExtractIndexability = true;
        public static bool ExtractH2 = true;
        public static bool ExtractH1 = true;
        public static bool ExtractMetaKeywords = true;
        public static bool ExtractMetadataDesc = true;
        public static bool ExtractPageTitle = true;
        public static bool CrawlIframes = true;
        public static bool CrawlJavaScript = true;
        public static bool CrawlCss = true;
        public static bool CrawlImages = true;
        public static int CrawlDepthLimit = 10;
        public static int TotalCrawlLimit = 100000;
        public static int MaxTitles = 3;
        public static int MaxDescs = 3;
        public static int MaxKeywords = 3;
        public static int MaxHeadsOne = 3;
        public static int MaxHeadsTwo = 3;
        //-----------------------------------------//
        public static int MaxSemaphores = 50;
        public static int ErrorsCounter = 0;
        #endregion

        public const int SEMAPHORES_COUNTER_INDEX = 0;
        public const int VISITED_PAGES_COUNTER_INDEX = 1;
        public const int TITLE_PIXEL_SIZE_PROBLEM_COUNTER_INDEX = 2;
        public const int TITLE_CHAR_SIZE_COUNTER_INDEX = 3;
        public const int DESC_PIXEL_SIZE_PROBLEM_COUNTER_INDEX = 4;
        public const int DESC_CHAR_SIZE_PROBLEM_COUNTER_INDEX = 5;
        public const int URL_CHAR_SIZE_PROBLEM_COUNTER_INDEX = 6;
        public const int H1_CHAR_SIZE_PROBLEM_COUNTER_INDEX = 7;
        public const int H2_CHAR_SIZE_PROBLEM_COUNTER_INDEX = 8;
        public const int IMAGE_SIZE_PROBLEM_COUNTER_INDEX = 9;

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

        public static void SetDefaultSettings()
        {
            using (StreamWriter sw = new StreamWriter(ConfigFilePath))
            {
                // TODO: declare constant strings for names 
                sw.WriteLine("ImgSizeMax=100");
                sw.WriteLine("ImgAltCharMax=100");
                sw.WriteLine("H2CharMax=70");
                sw.WriteLine("H1CharMax=70");
                sw.WriteLine("UrlCharMax=115");
                sw.WriteLine("DescCharMax=100");
                sw.WriteLine("DescCharMin=70");
                sw.WriteLine("DescPixMax=1010");
                sw.WriteLine("DescPixMin=400");
                sw.WriteLine("TitleCharMax=60");
                sw.WriteLine("TitleCharMin=30");
                sw.WriteLine("TitlePixMax=545");
                sw.WriteLine("TitlePixMin=200");
                sw.WriteLine("MaxSemaphores=50");
                sw.WriteLine("CrawlDepthLimit=10");
                sw.WriteLine("TotalCrawlLimit=1000000");
                sw.WriteLine("MaxTitles=3");
                sw.WriteLine("MaxDescs=3");
                sw.WriteLine("MaxKeywords=3");
                sw.WriteLine("MaxHeadsOne=3");
                sw.WriteLine("MaxHeadsTwo=3");
            }
        }

        public static void LoadSettingsFromFile()
        {
            if (!File.Exists(ConfigFilePath))
            {
                // MessageBox.Show("Plik konfiguracyjny nie istnieje, stwórz konfigurację crawlera w opcjach!\n Załadowano domyślne opcje crawlingu.", "Brak opcji", MessageBoxButtons.OK);
                SetDefaultSettings();
            }

            using (StreamReader sr = new StreamReader(Utils.ConfigFilePath))
            {
                string line = "";
                while ((line = sr.ReadLine()) != null)
                {
                    string[] temp = line.Split('=');

                    switch (temp[0])
                    {
                        case "TitlePixMin":
                            int.TryParse(temp[1], out TitlePixMin);
                            break;
                        case "TitlePixMax":
                            int.TryParse(temp[1], out TitlePixMax);
                            break;
                        case "TitleCharMin":
                            int.TryParse(temp[1], out TitleCharMin);
                            break;
                        case "TitleCharMax":
                            int.TryParse(temp[1], out TitleCharMax);
                            break;
                        case "MaxSemaphores":
                            int.TryParse(temp[1], out MaxSemaphores);                            
                            break;
                        case "MaxTitles":
                            int.TryParse(temp[1], out MaxTitles);
                            break;
                        case "MaxDescs":
                            int.TryParse(temp[1], out MaxDescs);
                            break;
                        case "MaxKeywords":
                            int.TryParse(temp[1], out MaxKeywords);
                            break;
                        case "MaxHeadsOne":
                            int.TryParse(temp[1], out MaxHeadsOne);
                            break;
                        case "MaxHeadsTwo":
                            int.TryParse(temp[1], out MaxHeadsTwo);
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
                            bool.TryParse(temp[1].ToLower(), out CrawlIframes);
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
                        default:
                            Debug.WriteLine("Unrecognised settings element");
                            break;
                    }
                }
            }
        }
    }
}
