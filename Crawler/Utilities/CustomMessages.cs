using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Crawler.Utilities
{
    internal class CustomMessages
    {
        private const MessageBoxButtons Buttons = MessageBoxButtons.OK;
        internal static void DisplayWrongUrlMsg()
        {
            const string message = "Proszę wprowadzić poprawny URL. Przykład: http://example.pl";
            const string caption = "Niepoprawny URL";
            MessageBox.Show(message, caption, Buttons);
        }

        internal static void DisplayPageDoesntExistMsg()
        {
            const string message = "Strona o podanym adresie nie istnieje. Upewnij się, że podałeś odpowiedni adres URL. Przykład: http://example.pl";
            const string caption = "Niepoprawny URL";
            MessageBox.Show(message, caption, Buttons);
        }

        internal static void DisplayCustomErrorMsg(string message, string caption)
        {
            MessageBox.Show(message, caption, Buttons);
        }
        internal static void DisplayCustomInfoMsg(string message, string caption)
        {
            MessageBox.Show(message, caption, Buttons);
        }
    }
}
