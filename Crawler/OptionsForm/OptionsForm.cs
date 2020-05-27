using Crawler.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace Crawler.OptionsForm
{
    public partial class OptionsForm : Form
    {
        MainForm.Form1 mainform;
        public bool closePls = false;

        private bool madeChanges = false;

        public OptionsForm(MainForm.Form1 form1)
        {
            InitializeComponent();
            mainform = form1;
            WczytajOpcje();
        }

        private void WczytajOpcje()
        {
            if (mainform.WczytajPreferencje())
            {
                radioButton1.Checked = true;
            }
            else
            {
                radioButton2.Checked = true;
            }
        }

        private void OptionsForm_Load(object sender, EventArgs e)
        {
            radioButton1.CheckedChanged += new EventHandler(ChangesWereMade);
            radioButton2.CheckedChanged += new EventHandler(ChangesWereMade);


        }

        private void ChangesWereMade(object sender, EventArgs e)
        {
            madeChanges = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (madeChanges)
            {
                //save changes
                bool pref = false;
                if (radioButton1.Checked)
                {
                    pref = true;
                }
                mainform.ZmienPreferencje(pref);


                //msg
                string message = "Zmiany zostały zapisane";
                string caption = "!";
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
    }
}
