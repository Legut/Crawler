using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crawler
{
    class Title
    {
        private string titleText; // Tytuł podstrony (jesli to podstrona, a nie inny byt jak np. obrazek). Znajduje się w tagu <title></title>
        private int titleLength; // Długość tytułu w ilości znaków
        private int titlePixelWidth; // długość tutułu w pixelach (czyli szerokość tekstu jaki znajduje się w tytule). Każda literka ma jakąś szerokość w pixelach i trzeba to przeliczyć da każdego tytułu
        public string TitleText
        {
            get { return titleText; }
            set { titleText = value; }
        }
        public int TitleLength
        {
            get { return titleLength; }
            set { titleLength = value; }
        }
        public int TitlePixelWidth
        {
            get { return titlePixelWidth; }
            set { titlePixelWidth = value; }
        }
    }
}
