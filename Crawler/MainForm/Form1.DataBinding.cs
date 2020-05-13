using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Crawler.MainForm
{
    [System.ComponentModel.DesignerCategory("Code")]
    public class Dummy2 { }
    partial class Form1
    {
        public void BindDataTableToWszystkie(DataTable dt)
        {
            allDataGridView.DataSource = dt;
            allDataGridView.SelectionChanged += new System.EventHandler(this.DataGridView_SelectionChanged);
            

        }
        public void BindDataTableToWewnetrzne(DataTable dt)
        {
            BindingSource src = new BindingSource
            {
                DataSource = new DataView(dt),
                Filter = "IsInternal = 'True'"
            };
            internalDataGridView.DataSource = src;
            internalDataGridView.SelectionChanged += new System.EventHandler(this.DataGridView_SelectionChanged);
        }
        public void BindDataTableToZewnetrzne(DataTable dt)
        {
            BindingSource src = new BindingSource
            {
                DataSource = new DataView(dt),
                Filter = "IsInternal = 'False'"
            };
            externalDataGridView.DataSource = src;
            externalDataGridView.Columns["Indexability"].Visible = false;
            externalDataGridView.SelectionChanged += new System.EventHandler(this.DataGridView_SelectionChanged);
            
        }
    }
}
