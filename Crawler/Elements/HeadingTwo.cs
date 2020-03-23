using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crawler
{
    class HeadingTwo
    {
        private string headingTwoText; // Lista wszystkich nagłówków H2 z danej podstrony, znajdujących się w tagu <h2></h2>
        private int headingTwoLength; // Lista długości w ilości znaków, wszystkich nagłówków H2
        public string HeadingTwoText
        {
            get { return headingTwoText; }
            set { headingTwoText = value; }
        }
        public int HeadingTwoLength
        {
            get { return headingTwoLength; }
            set { headingTwoLength = value; }
        }
    }
}
