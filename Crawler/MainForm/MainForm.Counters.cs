using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crawler.Utilities;

namespace Crawler.MainForm
{
    partial class MainForm
    {
        private int titleCharProblemsCounter;
        private int titlePixelProblemsCounter;
        private int descCharProblemsCounter;
        private int descPixelProblemsCounter;
        private int urlProblemsCounter;
        private int headOneProblemsCounter;
        private int headTwoProblemsCounter;
        private int imgProblemsCounter;

        public void UpdateSemaphoresStatus(int status)
        {
            if (countersList.SelectedIndex == Utils.SEMAPHORES_COUNTER_INDEX)
            {
                countersList.Items.RemoveAt(Utils.SEMAPHORES_COUNTER_INDEX);
                countersList.Items.Insert(Utils.SEMAPHORES_COUNTER_INDEX, "Wolne wątki: " + status + " / " + Utils.MaxSemaphores);
                countersList.SetSelected(Utils.SEMAPHORES_COUNTER_INDEX, true);
            }
            else
            {
                countersList.Items.RemoveAt(Utils.SEMAPHORES_COUNTER_INDEX);
                countersList.Items.Insert(Utils.SEMAPHORES_COUNTER_INDEX, "Wolne wątki: " + status + " / " + Utils.MaxSemaphores);
            }
        }
        public void UpdateVisitedPagesStatus(int crawled, int all)
        {
            if (countersList.SelectedIndex == Utils.VISITED_PAGES_COUNTER_INDEX)
            {
                countersList.Items.RemoveAt(Utils.VISITED_PAGES_COUNTER_INDEX);
                countersList.Items.Insert(Utils.VISITED_PAGES_COUNTER_INDEX, "Przejrzane strony: " + crawled + " / " + all);
                countersList.SetSelected(Utils.VISITED_PAGES_COUNTER_INDEX, true);
            }
            else
            {
                countersList.Items.RemoveAt(Utils.VISITED_PAGES_COUNTER_INDEX);
                countersList.Items.Insert(Utils.VISITED_PAGES_COUNTER_INDEX, "Przejrzane strony: " + crawled + " / " + all);
            }
        }

        public void IncreaseTitleCharProblemsCounter()
        {
            titleCharProblemsCounter++;
            if (countersList.SelectedIndex == Utils.TITLE_CHAR_SIZE_COUNTER_INDEX)
            {
                countersList.Items.RemoveAt(Utils.TITLE_CHAR_SIZE_COUNTER_INDEX);
                countersList.Items.Insert(Utils.TITLE_CHAR_SIZE_COUNTER_INDEX, "Błedne tytuły (literki): " + titleCharProblemsCounter);
                countersList.SetSelected(Utils.TITLE_CHAR_SIZE_COUNTER_INDEX, true);
            }
            else
            {
                countersList.Items.RemoveAt(Utils.TITLE_CHAR_SIZE_COUNTER_INDEX);
                countersList.Items.Insert(Utils.TITLE_CHAR_SIZE_COUNTER_INDEX, "Błedne tytuły (literki): " + titleCharProblemsCounter);
            }
        }

        public void IncreaseTitlePixelProblemsCounter()
        {
            titlePixelProblemsCounter++;
            if (countersList.SelectedIndex == Utils.TITLE_PIXEL_SIZE_PROBLEM_COUNTER_INDEX)
            {
                countersList.Items.RemoveAt(Utils.TITLE_PIXEL_SIZE_PROBLEM_COUNTER_INDEX);
                countersList.Items.Insert(Utils.TITLE_PIXEL_SIZE_PROBLEM_COUNTER_INDEX, "Błedne tytuły (piksele): " + titlePixelProblemsCounter);
                countersList.SetSelected(Utils.TITLE_PIXEL_SIZE_PROBLEM_COUNTER_INDEX, true);
            }
            else
            {
                countersList.Items.RemoveAt(Utils.TITLE_PIXEL_SIZE_PROBLEM_COUNTER_INDEX);
                countersList.Items.Insert(Utils.TITLE_PIXEL_SIZE_PROBLEM_COUNTER_INDEX, "Błedne tytuły (piksele): " + titlePixelProblemsCounter);
            }
        }

        public void IncreaseDescCharProblemsCounter()
        {
            descCharProblemsCounter++;
            if (countersList.SelectedIndex == Utils.DESC_CHAR_SIZE_PROBLEM_COUNTER_INDEX)
            {
                countersList.Items.RemoveAt(Utils.DESC_CHAR_SIZE_PROBLEM_COUNTER_INDEX);
                countersList.Items.Insert(Utils.DESC_CHAR_SIZE_PROBLEM_COUNTER_INDEX, "Błędne opisy (literki): " + descCharProblemsCounter);
                countersList.SetSelected(Utils.DESC_CHAR_SIZE_PROBLEM_COUNTER_INDEX, true);
            }
            else
            {
                countersList.Items.RemoveAt(Utils.DESC_CHAR_SIZE_PROBLEM_COUNTER_INDEX);
                countersList.Items.Insert(Utils.DESC_CHAR_SIZE_PROBLEM_COUNTER_INDEX, "Błędne opisy (literki): " + descCharProblemsCounter);
            }
        }

        public void IncreaseDescPixelProblemsCounter()
        {
            descPixelProblemsCounter++;
            if (countersList.SelectedIndex == Utils.DESC_PIXEL_SIZE_PROBLEM_COUNTER_INDEX)
            {
                countersList.Items.RemoveAt(Utils.DESC_PIXEL_SIZE_PROBLEM_COUNTER_INDEX);
                countersList.Items.Insert(Utils.DESC_PIXEL_SIZE_PROBLEM_COUNTER_INDEX, "Błędne opisy (piksele): " + descPixelProblemsCounter);
                countersList.SetSelected(Utils.DESC_PIXEL_SIZE_PROBLEM_COUNTER_INDEX, true);
            }
            else
            {
                countersList.Items.RemoveAt(Utils.DESC_PIXEL_SIZE_PROBLEM_COUNTER_INDEX);
                countersList.Items.Insert(Utils.DESC_PIXEL_SIZE_PROBLEM_COUNTER_INDEX, "Błędne opisy (piksele): " + descPixelProblemsCounter);
            }
        }
        public void IncreaseUrlProblemsCounter()
        {
            urlProblemsCounter++;
            if (countersList.SelectedIndex == Utils.URL_CHAR_SIZE_PROBLEM_COUNTER_INDEX)
            {
                countersList.Items.RemoveAt(Utils.URL_CHAR_SIZE_PROBLEM_COUNTER_INDEX);
                countersList.Items.Insert(Utils.URL_CHAR_SIZE_PROBLEM_COUNTER_INDEX, "Błędne URL: " + urlProblemsCounter);
                countersList.SetSelected(Utils.URL_CHAR_SIZE_PROBLEM_COUNTER_INDEX, true);
            }
            else
            {
                countersList.Items.RemoveAt(Utils.URL_CHAR_SIZE_PROBLEM_COUNTER_INDEX);
                countersList.Items.Insert(Utils.URL_CHAR_SIZE_PROBLEM_COUNTER_INDEX, "Błędne URL: " + urlProblemsCounter);
            }
        }

        public void IncreaseHeadOneProblemsCounter()
        {
            headOneProblemsCounter++;
            if (countersList.SelectedIndex == Utils.H1_CHAR_SIZE_PROBLEM_COUNTER_INDEX)
            {
                countersList.Items.RemoveAt(Utils.H1_CHAR_SIZE_PROBLEM_COUNTER_INDEX);
                countersList.Items.Insert(Utils.H1_CHAR_SIZE_PROBLEM_COUNTER_INDEX, "Błędne H1: " + headOneProblemsCounter);
                countersList.SetSelected(Utils.H1_CHAR_SIZE_PROBLEM_COUNTER_INDEX, true);
            }
            else
            {
                countersList.Items.RemoveAt(Utils.H1_CHAR_SIZE_PROBLEM_COUNTER_INDEX);
                countersList.Items.Insert(Utils.H1_CHAR_SIZE_PROBLEM_COUNTER_INDEX, "Błędne H1: " + headOneProblemsCounter);
            }
        }

        public void IncreaseHeadTwoProblemsCounter()
        {
            headTwoProblemsCounter++;
            if (countersList.SelectedIndex == Utils.H2_CHAR_SIZE_PROBLEM_COUNTER_INDEX)
            {
                countersList.Items.RemoveAt(Utils.H2_CHAR_SIZE_PROBLEM_COUNTER_INDEX);
                countersList.Items.Insert(Utils.H2_CHAR_SIZE_PROBLEM_COUNTER_INDEX, "Błędne H2: " + headTwoProblemsCounter);
                countersList.SetSelected(Utils.H2_CHAR_SIZE_PROBLEM_COUNTER_INDEX, true);
            }
            else
            {
                countersList.Items.RemoveAt(Utils.H2_CHAR_SIZE_PROBLEM_COUNTER_INDEX);
                countersList.Items.Insert(Utils.H2_CHAR_SIZE_PROBLEM_COUNTER_INDEX, "Błędne H2: " + headTwoProblemsCounter);
            }
        }

        public void IncreaseImgProblemsCounter()
        {
            imgProblemsCounter++;
            if (countersList.SelectedIndex == Utils.IMAGE_SIZE_PROBLEM_COUNTER_INDEX)
            {
                countersList.Items.RemoveAt(Utils.IMAGE_SIZE_PROBLEM_COUNTER_INDEX);
                countersList.Items.Insert(Utils.IMAGE_SIZE_PROBLEM_COUNTER_INDEX, "Błędne obrazki: " + imgProblemsCounter);
                countersList.SetSelected(Utils.IMAGE_SIZE_PROBLEM_COUNTER_INDEX, true);
            }
            else
            {
                countersList.Items.RemoveAt(Utils.IMAGE_SIZE_PROBLEM_COUNTER_INDEX);
                countersList.Items.Insert(Utils.IMAGE_SIZE_PROBLEM_COUNTER_INDEX, "Błędne obrazki: " + imgProblemsCounter);
            }
        }
    }
}
