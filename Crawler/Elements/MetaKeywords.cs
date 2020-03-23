using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crawler
{
    class MetaKeywords
    {
        private string metaKeywordsText; // Meta dane - Słowa kluczowe; Znajdują się w tagu <meta name="keywords" content="klucz1, klucz2, klucz3">.
        private int metaKeywordsLength; // Długość słów kluczowych (wszystkich, nie każdego z osobna)
        public string MetaKeywordsText
        {
            get { return metaKeywordsText; }
            set { metaKeywordsText = value; }
        }
        public int MetaKeywordsLength
        {
            get { return metaKeywordsLength; }
            set { metaKeywordsLength = value; }
        }
    }
}
