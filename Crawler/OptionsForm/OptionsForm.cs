using Crawler.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using static Crawler.Utilities.Utils;

namespace Crawler.OptionsForm
{
    public partial class OptionsForm : Form
    {
        private static string configPath = Application.StartupPath + "\\opcjeCrawleraConfig.cfg";

        MainForm.Form1 mainform;
        public bool closePls = false;

        private bool madeChanges = false;

        public OptionsForm(MainForm.Form1 form1)
        {
            InitializeComponent();
            mainform = form1;
            WczytajKontrolki();
            WczytajOpcje();
        }

        private void WczytajKontrolki()
        {
            foreach (Control nud in this.Preferencje.Controls)
            {
                RecursiveLoadControls(nud);
            }

            foreach (NumericUpDown nud in numericUpDowns)
            {
                nud.ValueChanged += ChangesWereMade;
            }

            foreach (System.Windows.Forms.CheckBox cb in checkBoxes)
            {
                cb.CheckedChanged += ChangesWereMade;
            }
        }

        private void RecursiveLoadControls(Control contrl)
        {
            foreach (Control nud in contrl.Controls)
            {
                if (nud.GetType() == typeof(GroupBox))
                {
                    RecursiveLoadControls(nud);
                }
                if (nud.GetType() == typeof(NumericUpDown))
                {
                    numericUpDowns.Add((NumericUpDown)nud);
                }
                if (nud.GetType() == typeof(System.Windows.Forms.CheckBox))
                {
                    checkBoxes.Add((System.Windows.Forms.CheckBox)nud);
                }
            }
        }       

        private void ChangesWereMade(object sender, EventArgs e)
        {
            madeChanges = true;
            Debug.WriteLine("CHANGES WERE MADE");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (madeChanges)
            {
                //save changes
                string message;
                string caption;
                if (SaveData())
                {
                    message = "Zmiany zostały zapisane";
                    caption = "OK";
                }
                else
                {
                    message = "nie dało rady zapisać";
                    caption = "ERROR";
                }

                //reload options
                ReloadOptions();
                                               
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                DialogResult result;

                // Displays the MessageBox.
                result = MessageBox.Show(message, caption, buttons);
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    //reload options
                    ZaladujOpcje();
                    // Closes the parent form.
                    this.Close();
                }
            }
        }

        private void ReloadOptions()
        {
            numericUpDowns.Clear();
            checkBoxes.Clear();
            WczytajKontrolki();
            WczytajOpcje();
        }

        private void WczytajOpcje()
        {
            if (File.Exists(configPath))
            {
                WczytajPlikOpcji();
            }
            else
            {
                MessageBox.Show("Plik konfiguracyjny nie istnieje, paniętaj aby zapisać opcje po ich ustawieniu!", "Brak opcji", MessageBoxButtons.OK);
                GenerujDefaultoweOpcje();
                WczytajPlikOpcji();
            }
        }

        private static void WczytajPlikOpcji()
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

        private bool SaveData()
        {
            try
            {
                List<string> nodes = new List<string>();

                foreach (NumericUpDown nud in numericUpDowns)
                {
                    nodes.Add(nud.Name + "=" + nud.Value.ToString());
                }

                foreach (System.Windows.Forms.CheckBox cb in checkBoxes)
                {
                    nodes.Add(cb.Name + "=" + cb.Checked.ToString());
                }

                using (StreamWriter sw = new StreamWriter(configPath))
                {
                    foreach(string line in nodes)
                    {
                        sw.WriteLine(line);
                        Debug.WriteLine(line);
                    }
                }

                return true;
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK);
                return false;
            }            
        }
    }
}
