using System;
using System.Collections.Generic;
using System.Numerics;

namespace Crawler
{
    internal class PageFragment
    {
        // TO jest nasza biblia, tu jest wszystko: https://docs.microsoft.com/en-us/dotnet/api/system.net.httpwebresponse?view=netframework-4.8
        private string address; //url elementu
        private string contentType; // text/css, image/jpeg, text/html, application/pdf... Wszystko załatwia: HttpWebResponse.ContentType

        // https://restfulapi.net/http-status-codes/, HttpStatusCode załatwia robotę
        private string statusCode; // np. 404 kiedy not found, albo 200 jak wszystko git, albo 301 przekierowanie
        private string status; // to samo co status code, tylko słowem, jak 301 to "Przekierowanie", jak 404 "Not Found", jak 200 to "OK" itd.

        // W zależności od status code, bo jak jest 200 / OK to wiadomo, że indexable, jak jest co innego (np. 301, to non-indexable, przyczyna: przekierowanie) to praktycznie zawsze nie jest indexable, ale trzeba sprawdzić jeszcze
        private string indexability; // Czy strona jest możliwa do zaindexowania. Jeśli np. masz podstrone /o-nas, która od razu przekierowuje na /kontakt, to /o-nas nie jest indexowalne, tak samo jak masz atrybut no-index. Dwie możliwe wartości: Indexable, Non-Indexable.
        private string indexabilityStatus; // Jeśli w poprzednim non-indexable, to tutaj przyczyna, np. nie jest indexable, bo było przekierowanie, albo nie jest indexable, bo robots.txt zabrania

        private List<Title> titles;
        private List<MetaDescription> metaDescriptions;
        private List<MetaKeywords> metaKeywords;
        private List<HeadingOne> headingsOne;
        private List<HeadingTwo> headingsTwo;

        private Int64 size; // rozmiar elementu na dysku. W sensie w MB kb itd. Kod html waży, obrazki ważą, załączone arkusze stylów (css) ważą, załączone JS (.js) ważą.
        private int wordCount; // Ilość słów na podstronie. Chodzi o treść samą, we wszystkich nagłówkach i paragrafach itd. Tylko dla kontentu typu html, bo nie liczymy słów w cssie, czy js, czy na zdjęciach.
        private float textRatio; // Number of non-HTML characters found in the HTML body tag on a page (the text), divided by the total number of characters the HTML page is made up of, and displayed as a percentage.

        private int urlDepth; // "Głębokość" przecrawlowanego elementu. Np. obrazek znajdujący się pod adresem https://bcd.pl/obrazki/2020/01/obrazek.jpg ma głębokość 4. Innymi słowy jest to ilość slashy ("/") w linku, po jego baseURL (https://bcd.pl)

        private long outLinks; // Ilość wewnętrznych linków na danej podstronie
        private long uniqueOutLinks; // ilość unikalnych wewnętrznych linków na danej podstronie
        // private int uniqueOutLinksOfTotal; // Wartość w procentach. Jeśli na stronie bcd.pl jest 50 podstron, a na crawlowanej podstronie są linki do 30 unikalnych podstron, to ta wartość wynosi: 30/50 * 100% 
        private HashSet<string> outLinksAdresses; // kolekcja adresów z tej podstrony, które linkują do innej podstrony w obrębie danej domeny. Adresy znajdują się w hashsecie po to, żeby zebrać listę unikalnych podstron.

        private long externalOutLinks; // To samo co wyżej tylko dla linków zewnętrznych
        private long uniqueExternalOutLinks; // To samo co wyżej tylko dla linków zewnętrznych
        // private int uniqueExternalOutLinksOfTotal; // To samo co wyżej tylko dla linków zewnętrznych
        private HashSet<string> externalOutLinksAdresses; // kolekcja adresów z tej podstrony, które linkują do innej podstrony poza obrębem danej domeny. Adresy znajdują się w hashsecie po to, żeby zebrać listę unikalnych podstron.

        private int hash; // Treść HTML przekształcona w hash w celu zweryfikowania duplicate content;
        private int responseTime; // Czas w sekundach od początku nawiązywania połączenia z podstroną do pobrania jej zawartości (jak masz już var htmlDocument to koniec liczenia czasu)
        private string redirectURL; // na co zostałeś przekierowany, jeśli było przekierowanie.
        private string redirectType; // jaki rodzaj przekierowania słownie.
        private bool isInternal;
        private bool isDuplicate;

        public string Address
        {
            get { return address; }
            set { address = value; }
        }
        public string ContentType
        {
            get { return contentType; }
            set { contentType = value; }
        }
        public string StatusCode
        {
            get { return statusCode; }
            set { statusCode = value; }
        }
        public string Status
        {
            get { return status; }
            set { status = value; }
        }
        public string Indexability
        {
            get { return indexability; }
            set { indexability = value; }
        }
        public string IndexabilityStatus
        {
            get { return indexabilityStatus; }
            set { indexabilityStatus = value; }
        }
        public List<Title> Titles
        {
            get { return titles; }
            set { titles = value; }
        }
        public List<MetaDescription> MetaDescriptions
        {
            get { return metaDescriptions; }
            set { metaDescriptions = value; }
        }
        public List<MetaKeywords> MetaKeywords
        {
            get { return metaKeywords; }
            set { metaKeywords = value; }
        }
        public List<HeadingOne> HeadingsOne
        {
            get { return headingsOne; }
            set { headingsOne = value; }
        }
        public List<HeadingTwo> HeadingsTwo
        {
            get { return headingsTwo; }
            set { headingsTwo = value; }
        }
        public Int64 Size
        {
            get { return size; }
            set { size = value; }
        }
        public int WordCount
        {
            get { return wordCount; }
            set { wordCount = value; }
        }
        public float TextRatio
        {
            get { return textRatio; }
            set { textRatio = value; }
        }
        public int UrlDepth
        {
            get { return urlDepth; }
            set { urlDepth = value; }
        }
        public long OutLinks
        {
            get { return outLinks; }
            set { outLinks = value; }
        }
        public long UniqueOutLinks
        {
            get { return uniqueOutLinks; }
            set { uniqueOutLinks = value; }
        }
        public HashSet<string> OutLinksAdresses
        {
            get { return outLinksAdresses; }
            set { outLinksAdresses = value; }
        }
        public long ExternalOutLinks
        {
            get { return externalOutLinks; }
            set { externalOutLinks = value; }
        }
        public long UniqueExternalOutLinks
        {
            get { return uniqueExternalOutLinks; }
            set { uniqueExternalOutLinks = value; }
        }
        public HashSet<string> ExternalOutLinksAdresses
        {
            get { return externalOutLinksAdresses; }
            set { externalOutLinksAdresses = value; }
        }
        public int Hash
        {
            get { return hash; }
            set { hash = value; }
        }
        public int ResponseTime
        {
            get { return responseTime; }
            set { responseTime = value; }
        }
        public string RedirectURL
        {
            get { return redirectURL; }
            set { redirectURL = value; }
        }
        public string RedirectType
        {
            get { return redirectType; }
            set { redirectType = value; }
        }
        public bool IsInternal
        {
            get { return isInternal; }
            set { isInternal = value; }
        }
        public bool IsDuplicate
        {
            get { return isDuplicate; }
            set { isDuplicate = value; }
        }

        public PageFragment()
        {
            this.Address = ""; 
            this.ContentType = "";
            this.StatusCode = "";
            this.Status = "";
            this.Indexability = "Indexable";
            this.IndexabilityStatus = "";
            this.Titles = new List<Title>();
            this.MetaDescriptions = new List<MetaDescription>();
            this.MetaKeywords = new List<MetaKeywords>();
            this.HeadingsOne = new List<HeadingOne>();
            this.HeadingsTwo = new List<HeadingTwo>();
            this.Size = 0;
            this.WordCount = 0;
            this.TextRatio = 0;
            this.UrlDepth = 0;
            this.OutLinks = 0;
            this.UniqueOutLinks = 0;
            this.OutLinksAdresses = new HashSet<string>();
            this.ExternalOutLinks = 0;
            this.UniqueExternalOutLinks = 0;
            this.ExternalOutLinksAdresses = new HashSet<string>();
            this.Hash = 0;
            this.ResponseTime = 0;
            this.RedirectURL = "";
            this.RedirectType = "";
            this.IsInternal = false;
            this.isDuplicate = false;
        }
    }
}