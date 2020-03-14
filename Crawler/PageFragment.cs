namespace Crawler
{
    internal class PageFragment
    {
        private string address; //url elementu
        private string contentType; // enum, text/html, image/jpeg, text/css...
        
        private string statusCode; // np. 404 kiedy not found, albo 200 jak wszystko git, albo 301 przekierowanie
        private string status; // to samo co status code, tylko słowem, jak 301 to "Przekierowanie", jak 404 "Not Found", jak 200 to "OK" itd.
        
        private string indexability; // Czy strona jest możliwa do zaindexowania. Jeśli np. masz podstrone /o-nas, która od razu przekierowuje na /kontakt, to /o-nas nie jest indexowalne, tak samo jak masz atrybut no-index
        private string indexabilityStatus; // Jeśli w poprzednim non-indexable, to tutaj przyczyna, np. nie jest indexable, bo było przekierowanie, albo nie jest indexable, bo robots.txt zabrania
        
        private string title; // Tytuł podstrony (jesli to podstrona, a nie inny byt jak np. obrazek). Znajduje się w tagu <title></title>
        private string titleLength; // Długość tytułu w ilości znaków
        private string titlePixelWidth; // długość tutułu w pixelach (czyli szerokość tekstu jaki znajduje się w tytule). Każda literka ma jakąś szerokość w pixelach i trzeba to przeliczyć da każdego tytułu
        
        private string metaDescription; // Meta dane - Opis pdostrony; Znajduje się w tagu: <meta name="description" content="treść opisu">. To jets opis jaki się wyświetla w google pod linkiem do każdego z wyników
        private string metaDescriptionLength; // Długość opisu w ilości znaków
        private string metaDescriptionPixelWidth; // długość opisu w szerokości liczonej w pikselach
        
        private string metaKeywords; // Meta dane - Słowa kluczowe; Znajdują się w tagu <meta name="keywords" content="klucz1, klucz2, klucz3">.
        private string metaKeywordsLength; // Długość słów kluczowych (wszystkich, nie każdego z osobna)
        
        private string headingOne; // Lista wszystkich nagłówków H1 z danej podstrony, znajdujących się w tagu <h1></h1>
        private string headingOneLength; // Lista długości w ilości znaków, wszystkich nagłówków H1
        private string headingTwo; // Lista wszystkich nagłówków H2 z danej podstrony, znajdujących się w tagu <h2></h2>
        private string headingTwoLength; // Lista długości w ilości znaków, wszystkich nagłówków H2
        
        private string size; // rozmiar elementu na dysku. W sensie w MB kb itd. Kod html waży, obrazki ważą, załączone arkusze stylów (css) ważą, załączone JS (.js) ważą.
        private string wordCount; // Ilość słów na podstronie. Chodzi o treść samą, we wszystkich nagłówkach i paragrafach itd. Tylko dla kontentu typu html, bo nie liczymy słów w cssie, czy js, czy na zdjęciach.
        private string textRatio; // Number of non-HTML characters found in the HTML body tag on a page (the text), divided by the total number of characters the HTML page is made up of, and displayed as a percentage.
        private string crawlDepth; // "Głębokość" przecrawlowanego elementu. Np. obrazek znajdujący się pod adresem https://bcd.pl/obrazki/2020/01/obrazek.jpg ma głębokość 4. Innymi słowy jest to ilość slashy ("/") w linku, po jego baseURL (https://bcd.pl)

        private string inLinks; // Ilość linków linkujących do tej (aktualnie crawlowanej) podstrony na wszystkich podstronach w serwisie (czyli trzeba przecrawlować wszystko inne żeby mieć ostateczną wartośc tutaj)
        private string uniqueInLinks; // Ilość unikalnych linków linkujących do tej (aktualnie crawlowanej) podstrony. Czyli po prostu ilość podstron, które linkują chociaż raz do tej podstrony
        private string uniqueInLinksOfTotal; // Procentowa ilość unikalnych linków do tej podstrony względem ilości podstron w serwisie. Czyli inaczej, ilość podstron linkujących chociaż raz do tej podstrony podzielona przez ilość podstron w serwisie * 100%

        private string outLinks; // Ilość wewnętrznych linków na danej podstronie
        private string uniqueOutLinks; // ilość unikalnych wewnętrznych linków na danej podstronie
        private string uniqueOutLinksOfTotal; // Wartość w procentach. Jeśli na stronie bcd.pl jest 50 podstron, a na crawlowanej podstronie są linki do 30 unikalnych podstron, to ta wartość wynosi: 30/50 * 100% 
        
        private string externalOutLinks; // To samo co wyżej tylko dla linków zewnętrznych
        private string uniqueExternalOutLinks; // To samo co wyżej tylko dla linków zewnętrznych
        private string uniqueExternalOutLinksOfTotal; // To samo co wyżej tylko dla linków zewnętrznych

        private string hash; // Treść HTML przekształcona w hash w celu zweryfikowania duplicate content;
        private string responseTime; // Czas w sekundach od początku nawiązywania połączenia z podstroną do pobrania jej zawartości (jak masz już var htmlDocument to koniec liczenia czasu)
        private string redirectURL; // na co zostałeś przekierowany, jeśli było przekierowanie.
        private string redirectType; // jaki rodzaj przekierowania słownie.
    }
}