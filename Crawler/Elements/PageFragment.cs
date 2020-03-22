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
        private List<HeadingOne> headingOnes;
        private List<HeadingTwo> headingTwos;

        private BigInteger size; // rozmiar elementu na dysku. W sensie w MB kb itd. Kod html waży, obrazki ważą, załączone arkusze stylów (css) ważą, załączone JS (.js) ważą.
        private int wordCount; // Ilość słów na podstronie. Chodzi o treść samą, we wszystkich nagłówkach i paragrafach itd. Tylko dla kontentu typu html, bo nie liczymy słów w cssie, czy js, czy na zdjęciach.
        private int textRatio; // Number of non-HTML characters found in the HTML body tag on a page (the text), divided by the total number of characters the HTML page is made up of, and displayed as a percentage.

        private int crawlDepth; // "Głębokość" przecrawlowanego elementu. Np. obrazek znajdujący się pod adresem https://bcd.pl/obrazki/2020/01/obrazek.jpg ma głębokość 4. Innymi słowy jest to ilość slashy ("/") w linku, po jego baseURL (https://bcd.pl)

        private long inLinks; // Ilość linków linkujących do tej (aktualnie crawlowanej) podstrony na wszystkich podstronach w serwisie (czyli trzeba przecrawlować wszystko inne żeby mieć ostateczną wartośc tutaj)
        private long uniqueInLinks; // Ilość unikalnych linków linkujących do tej (aktualnie crawlowanej) podstrony. Czyli po prostu ilość podstron, które linkują chociaż raz do tej podstrony
        private int uniqueInLinksOfTotal; // Procentowa ilość unikalnych linków do tej podstrony względem ilości podstron w serwisie. Czyli inaczej, ilość podstron linkujących chociaż raz do tej podstrony podzielona przez ilość podstron w serwisie * 100%

        private long outLinks; // Ilość wewnętrznych linków na danej podstronie
        private long uniqueOutLinks; // ilość unikalnych wewnętrznych linków na danej podstronie
        private int uniqueOutLinksOfTotal; // Wartość w procentach. Jeśli na stronie bcd.pl jest 50 podstron, a na crawlowanej podstronie są linki do 30 unikalnych podstron, to ta wartość wynosi: 30/50 * 100% 

        private long externalOutLinks; // To samo co wyżej tylko dla linków zewnętrznych
        private long uniqueExternalOutLinks; // To samo co wyżej tylko dla linków zewnętrznych
        private int uniqueExternalOutLinksOfTotal; // To samo co wyżej tylko dla linków zewnętrznych

        private string hash; // Treść HTML przekształcona w hash w celu zweryfikowania duplicate content;
        private int responseTime; // Czas w sekundach od początku nawiązywania połączenia z podstroną do pobrania jej zawartości (jak masz już var htmlDocument to koniec liczenia czasu)
        private string redirectURL; // na co zostałeś przekierowany, jeśli było przekierowanie.
        private string redirectType; // jaki rodzaj przekierowania słownie.
        private bool isInternal;

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
        public BigInteger Size
        {
            get { return size; }
            set { size = value; }
        }
        public int WordCount
        {
            get { return wordCount; }
            set { wordCount = value; }
        }
        public int TextRatio
        {
            get { return textRatio; }
            set { textRatio = value; }
        }
        public int CrawlDepth
        {
            get { return crawlDepth; }
            set { crawlDepth = value; }
        }
        public long InLinks
        {
            get { return inLinks; }
            set { inLinks = value; }
        }
        public long UniqueInLinks
        {
            get { return uniqueInLinks; }
            set { uniqueInLinks = value; }
        }
        public int UniqueInLinksOfTotal
        {
            get { return uniqueInLinksOfTotal; }
            set { uniqueInLinksOfTotal = value; }
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
        public int UniqueOutLinksOfTotal
        {
            get { return uniqueOutLinksOfTotal; }
            set { uniqueOutLinksOfTotal = value; }
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
        public int UniqueExternalOutLinksOfTotal
        {
            get { return uniqueExternalOutLinksOfTotal; }
            set { uniqueExternalOutLinksOfTotal = value; }
        }
        public string Hash
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
    }
}