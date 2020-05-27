using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Crawler.Utilities
{
    public static class Utils
    {
        // TODO: sortowanie liczb w widoku jest alfabetyczne zamiast wielkościowego
        public static readonly string[] SizeSuffixes = { "B", "KB", "MB", "GB", "TB" };

        //--here be options for crawler and stuff--//
        
        private static string configPath = Application.StartupPath + "\\opcjeCrawleraConfig.cfg";
        public static List<NumericUpDown> numericUpDowns = new List<NumericUpDown>();
        public static List<System.Windows.Forms.CheckBox> checkBoxes = new List<System.Windows.Forms.CheckBox>();
        //-----------------------------------------//
        public static int iloscBledow = 0;

        public static Task WhenMouseUp(this Control control)
        {
            var tcs = new TaskCompletionSource<object>();
            MouseEventHandler onMouseUp = null;
            onMouseUp = (sender, e) =>
            {
                control.MouseUp -= onMouseUp;
                tcs.TrySetResult(null);
            };
            control.MouseUp += onMouseUp;
            return tcs.Task;
        }

        public static int CountWords(string text)
        {
            int wordCount = 0, index = 0;

            // skip whitespace until first word
            while (index < text.Length && char.IsWhiteSpace(text[index]))
                index++;

            while (index < text.Length)
            {
                // check if current char is part of a word
                while (index < text.Length && !char.IsWhiteSpace(text[index]))
                    index++;

                wordCount++;

                // skip whitespace until next word
                while (index < text.Length && char.IsWhiteSpace(text[index]))
                    index++;
            }

            return wordCount;
        }


        public static void WczytajOpcje()
        {
            using (StreamReader sr = new StreamReader(configPath))
            {
                string line = "";
                while ((line = sr.ReadLine()) != null)
                {
                    string[] temp = line.Split('=');
                    bool found = false;

                    foreach (NumericUpDown nud in numericUpDowns)
                    {
                        if (!found)
                        {
                            if (nud.Name.Equals(temp[0]))
                            {
                                try
                                {
                                    nud.Value = int.Parse(temp[1]);
                                }
                                catch (Exception e)
                                {
                                    nud.Value = 0;
                                }
                                found = true;
                            }
                        }
                    }

                    foreach (System.Windows.Forms.CheckBox cb in checkBoxes)
                    {
                        if (!found)
                        {
                            if (cb.Name.Equals(temp[0]))
                            {
                                try
                                {
                                    if (temp[1] == "True")
                                    {
                                        cb.Checked = true;
                                    }
                                    else
                                    {
                                        cb.Checked = false;
                                    }
                                }
                                catch (Exception e)
                                {
                                    cb.Checked = false;
                                }
                                found = true;
                            }
                        }
                    }

                }
            }
        }
    }
}
