using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using static Crawler.Utilities.Utils;
using CheckBox = System.Windows.Forms.CheckBox;

namespace Crawler.OptionsForm
{
    public partial class OptionsForm : Form
    {
        private List<NumericUpDown> numericUpDowns;
        private List<CheckBox> checkBoxes;
        private bool madeChanges;

        public OptionsForm()
        {
            InitializeComponent();
        }
        protected override void OnLoad(EventArgs e)
        {
            this.madeChanges = false;
            this.numericUpDowns = new List<NumericUpDown>();
            this.checkBoxes = new List<CheckBox>();

            LoadControlsFromForm();
            LoadSettings();
            ApplyListeners();
        }

        private void ApplyListeners()
        {
            foreach (NumericUpDown nud in numericUpDowns)
            {
                nud.ValueChanged += ChangesListener;
            }

            foreach (CheckBox cb in checkBoxes)
            {
                cb.CheckedChanged += ChangesListener;
            }
        }

        private void LoadControlsFromForm()
        {
            foreach (Control control in this.Preferences.Controls)
            {
                LoadSingleControlFromForm(control);
            }
        }

        private void LoadSingleControlFromForm(Control control)
        {
            foreach (Control element in control.Controls)
            {
                if (element.GetType() == typeof(GroupBox))
                {
                    LoadSingleControlFromForm(element);
                }
                if (element.GetType() == typeof(NumericUpDown))
                {
                    numericUpDowns.Add((NumericUpDown)element);
                }
                if (element.GetType() == typeof(CheckBox))
                {
                    checkBoxes.Add((CheckBox)element);
                }
            }
        }       

        private void ChangesListener(object sender, EventArgs e)
        {
            this.madeChanges = true;
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (madeChanges)
            {
                // Save changes
                string message, caption;
                if (SaveData())
                {
                    message = "Zmiany zostały zapisane!";
                    caption = "OK";
                }
                else
                {
                    message = "Nie udało się zapisać wprowadzonych zmian.";
                    caption = "ERROR";
                }

                // Displays the MessageBox.
                DialogResult result = MessageBox.Show(message, caption, MessageBoxButtons.OK);
                if (result == DialogResult.OK)
                {
                    // Reload options
                    LoadSettingsFromFile();
                    this.Close();
                }
            }
            else
            {
                this.Close();
            }
        }

        private void LoadSettings()
        {
            if (File.Exists(ConfigPath))
            {
                LoadSettingsFile();
            }
            else
            {
                // MessageBox.Show("Plik konfiguracyjny nie istnieje, pamiętaj aby zapisać opcje po ich ustawieniu!", "Brak opcji", MessageBoxButtons.OK);
                SetDefaultSettings();
                LoadSettingsFile();
            }
        }

        private void LoadSettingsFile()
        {
            using (StreamReader sr = new StreamReader(ConfigPath))
            {
                string line = "";
                while ((line = sr.ReadLine()) != null)
                {
                    string[] temp = line.Split('=');

                    foreach (NumericUpDown nud in numericUpDowns)
                    {
                        if (!nud.Name.Equals(temp[0])) continue;
                        try
                        {
                            nud.Value = int.Parse(temp[1]);
                        }
                        catch
                        {
                            nud.Value = 0;
                        }

                        break;
                    }

                    foreach (System.Windows.Forms.CheckBox cb in checkBoxes)
                    {
                        if (!cb.Name.Equals(temp[0])) continue;
                        try
                        {
                            cb.Checked = temp[1] == "True";
                        }
                        catch
                        {
                            cb.Checked = false;
                        }

                        break;
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
                    nodes.Add(nud.Name + "=" + nud.Value);
                }

                foreach (CheckBox cb in checkBoxes)
                {
                    nodes.Add(cb.Name + "=" + cb.Checked);
                }

                using (StreamWriter sw = new StreamWriter(ConfigPath))
                {
                    Debug.WriteLine("Saved data:");
                    foreach (string line in nodes)
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

        private void CloseButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
