using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crawler.Elements
{
    class InLinksCounter
    {
        private int inLinksCount; // Ilość linków linkujących do tej (aktualnie crawlowanej) podstrony na wszystkich podstronach w serwisie (czyli trzeba przecrawlować wszystko inne żeby mieć ostateczną wartośc tutaj)
        private HashSet<string> uniqueInLinks; // Ilość unikalnych linków linkujących do tej (aktualnie crawlowanej) podstrony. Czyli po prostu ilość podstron, które linkują chociaż raz do tej podstrony
        // private float uniqueInLinksOfTotal; // NIE KORZYSTAMY ZTEGO ALE ZOSTAWIAMY BO OPIS JEST COOL - Procentowa ilość unikalnych linków do tej podstrony względem ilości podstron w serwisie. Czyli inaczej, ilość podstron linkujących chociaż raz do tej podstrony podzielona przez ilość podstron w serwisie * 100%
        
        public InLinksCounter()
        {
            this.inLinksCount = 0;
            this.uniqueInLinks = new HashSet<string>();
        }

        public int InLinksCount
        {
            get => inLinksCount;
            set => inLinksCount = value;
        }

        public HashSet<string> UniqueInLinks
        {
            get => uniqueInLinks;
            set => uniqueInLinks = value;
        }
    }
}
