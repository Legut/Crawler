using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crawler
{
    class HeadingOne
    {
        private string headingOneText; // Lista wszystkich nagłówków H1 z danej podstrony, znajdujących się w tagu <h1></h1>
        private int headingOneLength; // Lista długości w ilości znaków, wszystkich nagłówków H1
        public string HeadingOneText
        {
            get { return headingOneText; }
            set { headingOneText = value; }
        }
        public int HeadingOneLength
        {
            get { return headingOneLength; }
            set { headingOneLength = value; }
        }
    }
}
