using System;
using System.Collections.Generic;
using System.Numerics;

namespace Crawler
{
    internal class PageFragment
    {
        // TO jest nasza biblia, tu jest wszystko: https://docs.microsoft.com/en-us/dotnet/api/system.net.httpwebresponse?view=netframework-4.8

        public string address { get; set; } //url elementu
        private string contentType; // text/css, image/jpeg, text/html, application/pdf... Wszystko załatwia: HttpWebResponse.ContentType

        // https://restfulapi.net/http-status-codes/, HttpStatusCode załatwia robotę
        private string statusCode; // np. 404 kiedy not found, albo 200 jak wszystko git, albo 301 przekierowanie
        private string status; // to samo co status code, tylko słowem, jak 301 to "Przekierowanie", jak 404 "Not Found", jak 200 to "OK" itd.
        
        // W zależności od status code, bo jak jest 200 / OK to wiadomo, że indexable, jak jest co innego (np. 301, to non-indexable, przyczyna: przekierowanie) to praktycznie zawsze nie jest indexable, ale trzeba sprawdzić jeszcze
        private bool indexability; // Czy strona jest możliwa do zaindexowania. Jeśli np. masz podstrone /o-nas, która od razu przekierowuje na /kontakt, to /o-nas nie jest indexowalne, tak samo jak masz atrybut no-index. Dwie możliwe wartości: Indexable, Non-Indexable.
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
        internal void NormalizeAddress(string baseUrl)
        {
            if (this.address.StartsWith("http://") || this.address.StartsWith("https://"))
                return;
            else if (this.address.StartsWith("/"))
                this.address = baseUrl + this.address;
            else if (this.address.StartsWith("mailto") || this.address.StartsWith("tel") || this.address.StartsWith("#") || this.address.StartsWith("null"))
                this.address = null;
            else
                return;
        }
    }
}