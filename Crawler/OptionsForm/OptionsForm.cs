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

        public OptionsForm()
        {
            InitializeComponent();
            WczytajKontrolki();
            WczytajOpcje();
            this.Close();
        }

        private void WczytajKontrolki()
        {
            foreach (Control nud in this.Preferencje.Controls)
            {
                RecursiveLoadControls(nud);
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

        private void OptionsForm_Load(object sender, EventArgs e)
        {
            //radioButton1.CheckedChanged += new EventHandler(ChangesWereMade);

            foreach(NumericUpDown nud in numericUpDowns)
            {
                nud.ValueChanged += ChangesWereMade;
            }

            foreach(System.Windows.Forms.CheckBox cb in checkBoxes)
            {
                cb.CheckedChanged += ChangesWereMade;
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
