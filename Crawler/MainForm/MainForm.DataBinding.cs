using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Crawler.Utilities;

namespace Crawler.MainForm
{
    [System.ComponentModel.DesignerCategory("Code")]
    public class Dummy2 { }
    partial class MainForm
    {
        public void BindDataTableToAll(DataTable dt)
        {
            allDataGridView.DataSource = dt;
            allDataGridView.SelectionChanged += this.DataGridView_SelectionChanged;
            PrepareSingleDataGridView(allDataGridView);
        }
        public void BindDataTableToInternal(DataTable dt)
        {
            BindingSource src = new BindingSource
            {
                DataSource = new DataView(dt),
                Filter = Base.Crawler.ISINTERNAL_COL + " = 'True'"
            };
            internalDataGridView.DataSource = src;
            internalDataGridView.SelectionChanged += this.DataGridView_SelectionChanged;
        }
        public void BindDataTableToExternal(DataTable dt)
        {
            BindingSource src = new BindingSource
            {
                DataSource = new DataView(dt),
                Filter = Base.Crawler.ISINTERNAL_COL + " = 'False'"
            };
            externalDataGridView.DataSource = src;
            foreach (DataGridViewColumn column in externalDataGridView.Columns)
            {
                column.Visible = false;
            }

            externalDataGridView.Columns[Base.Crawler.ADDRESS_COL].Visible = true;
            externalDataGridView.Columns[Base.Crawler.CONTET_TYPE_COL].Visible = true;
            externalDataGridView.Columns[Base.Crawler.STATUS_CODE_COL].Visible = true;
            externalDataGridView.Columns[Base.Crawler.STATUS_COL].Visible = true;
            externalDataGridView.Columns[Base.Crawler.URL_DEPTH_COL].Visible = true;
            externalDataGridView.Columns[Base.Crawler.INLINKS_COL].Visible = true;
            externalDataGridView.Columns[Base.Crawler.UNIQUE_INLINKS_COL].Visible = true;
            externalDataGridView.Columns[Base.Crawler.UNIQUE_INLINKS_OF_TOTAL_COL].Visible = true;
            externalDataGridView.SelectionChanged += this.DataGridView_SelectionChanged;
        }
        public void BindDataTableToPageTitles(DataTable dt)
        {
            EnumerableRowCollection<DataRow> query = from row in dt.AsEnumerable()
                where row.Field<string>(Base.Crawler.TITLE_COL + 1).Length > 0
                select row;

            pageTitlesDataGridView.DataSource = query.AsDataView();
            pageTitlesDataGridView.SelectionChanged += this.DataGridView_SelectionChanged;
            foreach (DataGridViewColumn column in pageTitlesDataGridView.Columns) { column.Visible = false; }

            pageTitlesDataGridView.Columns[Base.Crawler.ADDRESS_COL].Visible = true;
            pageTitlesDataGridView.Columns[Base.Crawler.TITLE_COL + 1].Visible = true;
            pageTitlesDataGridView.Columns[Base.Crawler.TITLE_LENGTH_COL + 1].Visible = true;
            pageTitlesDataGridView.Columns[Base.Crawler.TITLE_PIXEL_WIDTH_COL + 1].Visible = true;
            if (Utils.ExtractIndexability)
            {
                pageTitlesDataGridView.Columns[Base.Crawler.INDEXABILITY_COL].Visible = true;
                pageTitlesDataGridView.Columns[Base.Crawler.INDEXABILITY_STATUS_COL].Visible = true;
            }
        }
        public void BindDataTableToMetaDescs(DataTable dt)
        {
            EnumerableRowCollection<DataRow> query = from row in dt.AsEnumerable()
                where row.Field<string>(Base.Crawler.META_DESC_COL + 1).Length > 0
                select row;

            metaDescDataGridView.DataSource = query.AsDataView();
            metaDescDataGridView.SelectionChanged += this.DataGridView_SelectionChanged;
            foreach (DataGridViewColumn column in metaDescDataGridView.Columns) { column.Visible = false; }

            metaDescDataGridView.Columns[Base.Crawler.ADDRESS_COL].Visible = true;
            metaDescDataGridView.Columns[Base.Crawler.META_DESC_COL + 1].Visible = true;
            metaDescDataGridView.Columns[Base.Crawler.META_DESC_LENGTH_COL + 1].Visible = true;
            metaDescDataGridView.Columns[Base.Crawler.META_DESC_PIXEL_WIDTH_COL + 1].Visible = true;
            if (Utils.ExtractIndexability)
            {
                metaDescDataGridView.Columns[Base.Crawler.INDEXABILITY_COL].Visible = true;
                metaDescDataGridView.Columns[Base.Crawler.INDEXABILITY_STATUS_COL].Visible = true;
            }
        }
        public void BindDataTableToKeywords(DataTable dt)
        {
            EnumerableRowCollection<DataRow> query = from row in dt.AsEnumerable()
                where row.Field<string>(Base.Crawler.META_KEYWORDS_COL + 1).Length > 0
                select row;

            keywordsDataGridView.DataSource = query.AsDataView();
            keywordsDataGridView.SelectionChanged += this.DataGridView_SelectionChanged;
            foreach (DataGridViewColumn column in keywordsDataGridView.Columns) { column.Visible = false; }

            keywordsDataGridView.Columns[Base.Crawler.ADDRESS_COL].Visible = true;
            keywordsDataGridView.Columns[Base.Crawler.META_KEYWORDS_COL + 1].Visible = true;
            keywordsDataGridView.Columns[Base.Crawler.META_KEYWORDS_LENGTH_COL + 1].Visible = true;
            if (Utils.ExtractIndexability)
            {
                keywordsDataGridView.Columns[Base.Crawler.INDEXABILITY_COL].Visible = true;
                keywordsDataGridView.Columns[Base.Crawler.INDEXABILITY_STATUS_COL].Visible = true;
            }
        }
        public void BindDataTableToHeadingsOne(DataTable dt)
        {
            EnumerableRowCollection<DataRow> query = from row in dt.AsEnumerable()
                where row.Field<string>(Base.Crawler.H_ONE_COL + 1).Length > 0
                select row;

            headingOneDataGridView.DataSource = query.AsDataView();
            headingOneDataGridView.SelectionChanged += this.DataGridView_SelectionChanged;
            foreach (DataGridViewColumn column in headingOneDataGridView.Columns) { column.Visible = false; }

            headingOneDataGridView.Columns[Base.Crawler.ADDRESS_COL].Visible = true;
            headingOneDataGridView.Columns[Base.Crawler.H_ONE_COL + 1].Visible = true;
            headingOneDataGridView.Columns[Base.Crawler.H_ONE_LENGTH_COL + 1].Visible = true;
            if (Utils.ExtractIndexability)
            {
                headingOneDataGridView.Columns[Base.Crawler.INDEXABILITY_COL].Visible = true;
                headingOneDataGridView.Columns[Base.Crawler.INDEXABILITY_STATUS_COL].Visible = true;
            }
        }
        public void BindDataTableToHeadingsTwo(DataTable dt)
        {
            EnumerableRowCollection<DataRow> query = from row in dt.AsEnumerable()
                where row.Field<string>(Base.Crawler.H_TWO_COL + 1).Length > 0
                select row;

            headingTwoDataGridView.DataSource = query.AsDataView();
            headingTwoDataGridView.SelectionChanged += this.DataGridView_SelectionChanged;
            foreach (DataGridViewColumn column in headingTwoDataGridView.Columns) { column.Visible = false; }

            headingTwoDataGridView.Columns[Base.Crawler.ADDRESS_COL].Visible = true;
            headingTwoDataGridView.Columns[Base.Crawler.H_TWO_COL + 1].Visible = true;
            headingTwoDataGridView.Columns[Base.Crawler.H_TWO_LENGTH_COL + 1].Visible = true;
            if (Utils.ExtractIndexability)
            {
                headingTwoDataGridView.Columns[Base.Crawler.INDEXABILITY_COL].Visible = true;
                headingTwoDataGridView.Columns[Base.Crawler.INDEXABILITY_STATUS_COL].Visible = true;
            }
        }
        public void BindDataTableToImages(DataTable dt)
        {
            EnumerableRowCollection<DataRow> query = from row in dt.AsEnumerable()
                where row.Field<string>(Base.Crawler.CONTET_TYPE_COL).Contains("image")
                select row;

            imagesDataGridView.DataSource = query.AsDataView();
            imagesDataGridView.SelectionChanged += this.DataGridView_SelectionChanged;
            foreach (DataGridViewColumn column in imagesDataGridView.Columns) { column.Visible = false; }

            imagesDataGridView.Columns[Base.Crawler.ADDRESS_COL].Visible = true;
            imagesDataGridView.Columns[Base.Crawler.CONTET_TYPE_COL].Visible = true;
            if (Utils.ExtractPageSize)
                imagesDataGridView.Columns[Base.Crawler.SIZE_COL].Visible = true;
            if (Utils.ExtractIndexability)
            {
                imagesDataGridView.Columns[Base.Crawler.INDEXABILITY_COL].Visible = true;
                imagesDataGridView.Columns[Base.Crawler.INDEXABILITY_STATUS_COL].Visible = true;
            }

            imagesDataGridView.Columns[Base.Crawler.INLINKS_COL].Visible = true;
            imagesDataGridView.Columns[Base.Crawler.UNIQUE_INLINKS_COL].Visible = true;
            imagesDataGridView.Columns[Base.Crawler.UNIQUE_INLINKS_OF_TOTAL_COL].Visible = true;
        }
        public void HandleNewTitleCol(int index)
        {
            externalDataGridView.Columns[Base.Crawler.TITLE_COL + index].Visible = false;
            externalDataGridView.Columns[Base.Crawler.TITLE_LENGTH_COL + index].Visible = false;
            externalDataGridView.Columns[Base.Crawler.TITLE_PIXEL_WIDTH_COL + index].Visible = false;

            metaDescDataGridView.Columns[Base.Crawler.TITLE_COL + index].Visible = false;
            metaDescDataGridView.Columns[Base.Crawler.TITLE_LENGTH_COL + index].Visible = false;
            metaDescDataGridView.Columns[Base.Crawler.TITLE_PIXEL_WIDTH_COL + index].Visible = false;

            headingOneDataGridView.Columns[Base.Crawler.TITLE_COL + index].Visible = false;
            headingOneDataGridView.Columns[Base.Crawler.TITLE_LENGTH_COL + index].Visible = false;
            headingOneDataGridView.Columns[Base.Crawler.TITLE_PIXEL_WIDTH_COL + index].Visible = false;

            keywordsDataGridView.Columns[Base.Crawler.TITLE_COL + index].Visible = false;
            keywordsDataGridView.Columns[Base.Crawler.TITLE_LENGTH_COL + index].Visible = false;
            keywordsDataGridView.Columns[Base.Crawler.TITLE_PIXEL_WIDTH_COL + index].Visible = false;

            headingTwoDataGridView.Columns[Base.Crawler.TITLE_COL + index].Visible = false;
            headingTwoDataGridView.Columns[Base.Crawler.TITLE_LENGTH_COL + index].Visible = false;
            headingTwoDataGridView.Columns[Base.Crawler.TITLE_PIXEL_WIDTH_COL + index].Visible = false;

            imagesDataGridView.Columns[Base.Crawler.TITLE_COL + index].Visible = false;
            imagesDataGridView.Columns[Base.Crawler.TITLE_LENGTH_COL + index].Visible = false;
            imagesDataGridView.Columns[Base.Crawler.TITLE_PIXEL_WIDTH_COL + index].Visible = false;
        }
        public void HandleNewDescCol(int index)
        {
            externalDataGridView.Columns[Base.Crawler.META_DESC_COL + index].Visible = false;
            externalDataGridView.Columns[Base.Crawler.META_DESC_LENGTH_COL + index].Visible = false;
            externalDataGridView.Columns[Base.Crawler.META_DESC_PIXEL_WIDTH_COL + index].Visible = false;

            pageTitlesDataGridView.Columns[Base.Crawler.META_DESC_COL + index].Visible = false;
            pageTitlesDataGridView.Columns[Base.Crawler.META_DESC_LENGTH_COL + index].Visible = false;
            pageTitlesDataGridView.Columns[Base.Crawler.META_DESC_PIXEL_WIDTH_COL + index].Visible = false;

            headingOneDataGridView.Columns[Base.Crawler.META_DESC_COL + index].Visible = false;
            headingOneDataGridView.Columns[Base.Crawler.META_DESC_LENGTH_COL + index].Visible = false;
            headingOneDataGridView.Columns[Base.Crawler.META_DESC_PIXEL_WIDTH_COL + index].Visible = false;

            keywordsDataGridView.Columns[Base.Crawler.META_DESC_COL + index].Visible = false;
            keywordsDataGridView.Columns[Base.Crawler.META_DESC_LENGTH_COL + index].Visible = false;
            keywordsDataGridView.Columns[Base.Crawler.META_DESC_PIXEL_WIDTH_COL + index].Visible = false;

            headingTwoDataGridView.Columns[Base.Crawler.META_DESC_COL + index].Visible = false;
            headingTwoDataGridView.Columns[Base.Crawler.META_DESC_LENGTH_COL + index].Visible = false;
            headingTwoDataGridView.Columns[Base.Crawler.META_DESC_PIXEL_WIDTH_COL + index].Visible = false;

            imagesDataGridView.Columns[Base.Crawler.META_DESC_COL + index].Visible = false;
            imagesDataGridView.Columns[Base.Crawler.META_DESC_LENGTH_COL + index].Visible = false;
            imagesDataGridView.Columns[Base.Crawler.META_DESC_PIXEL_WIDTH_COL + index].Visible = false;
        }
        public void HandleNewKeywordsCol(int index)
        {
            externalDataGridView.Columns[Base.Crawler.META_KEYWORDS_COL + index].Visible = false;
            externalDataGridView.Columns[Base.Crawler.META_KEYWORDS_LENGTH_COL + index].Visible = false;

            pageTitlesDataGridView.Columns[Base.Crawler.META_KEYWORDS_COL + index].Visible = false;
            pageTitlesDataGridView.Columns[Base.Crawler.META_KEYWORDS_LENGTH_COL + index].Visible = false;

            metaDescDataGridView.Columns[Base.Crawler.META_KEYWORDS_COL + index].Visible = false;
            metaDescDataGridView.Columns[Base.Crawler.META_KEYWORDS_LENGTH_COL + index].Visible = false;

            headingOneDataGridView.Columns[Base.Crawler.META_KEYWORDS_COL + index].Visible = false;
            headingOneDataGridView.Columns[Base.Crawler.META_KEYWORDS_LENGTH_COL + index].Visible = false;

            headingTwoDataGridView.Columns[Base.Crawler.META_KEYWORDS_COL + index].Visible = false;
            headingTwoDataGridView.Columns[Base.Crawler.META_KEYWORDS_LENGTH_COL + index].Visible = false;

            imagesDataGridView.Columns[Base.Crawler.META_KEYWORDS_COL + index].Visible = false;
            imagesDataGridView.Columns[Base.Crawler.META_KEYWORDS_LENGTH_COL + index].Visible = false;
        }
        public void HandleNewHeadsOneCol(int index)
        {
            externalDataGridView.Columns[Base.Crawler.H_ONE_COL + index].Visible = false;
            externalDataGridView.Columns[Base.Crawler.H_ONE_LENGTH_COL + index].Visible = false;

            pageTitlesDataGridView.Columns[Base.Crawler.H_ONE_COL + index].Visible = false;
            pageTitlesDataGridView.Columns[Base.Crawler.H_ONE_LENGTH_COL + index].Visible = false;

            metaDescDataGridView.Columns[Base.Crawler.H_ONE_COL + index].Visible = false;
            metaDescDataGridView.Columns[Base.Crawler.H_ONE_LENGTH_COL + index].Visible = false;

            keywordsDataGridView.Columns[Base.Crawler.H_ONE_COL + index].Visible = false;
            keywordsDataGridView.Columns[Base.Crawler.H_ONE_LENGTH_COL + index].Visible = false;

            headingTwoDataGridView.Columns[Base.Crawler.H_ONE_COL + index].Visible = false;
            headingTwoDataGridView.Columns[Base.Crawler.H_ONE_LENGTH_COL + index].Visible = false;

            imagesDataGridView.Columns[Base.Crawler.H_ONE_COL + index].Visible = false;
            imagesDataGridView.Columns[Base.Crawler.H_ONE_LENGTH_COL + index].Visible = false;
        }
        public void HandleNewHeadsTwoCol(int index)
        {
            externalDataGridView.Columns[Base.Crawler.H_TWO_COL + index].Visible = false;
            externalDataGridView.Columns[Base.Crawler.H_TWO_LENGTH_COL + index].Visible = false;

            pageTitlesDataGridView.Columns[Base.Crawler.H_TWO_COL + index].Visible = false;
            pageTitlesDataGridView.Columns[Base.Crawler.H_TWO_LENGTH_COL + index].Visible = false;

            metaDescDataGridView.Columns[Base.Crawler.H_TWO_COL + index].Visible = false;
            metaDescDataGridView.Columns[Base.Crawler.H_TWO_LENGTH_COL + index].Visible = false;

            headingOneDataGridView.Columns[Base.Crawler.H_TWO_COL + index].Visible = false;
            headingOneDataGridView.Columns[Base.Crawler.H_TWO_LENGTH_COL + index].Visible = false;

            keywordsDataGridView.Columns[Base.Crawler.H_TWO_COL + index].Visible = false;
            keywordsDataGridView.Columns[Base.Crawler.H_TWO_LENGTH_COL + index].Visible = false;

            imagesDataGridView.Columns[Base.Crawler.H_TWO_COL + index].Visible = false;
            imagesDataGridView.Columns[Base.Crawler.H_TWO_LENGTH_COL + index].Visible = false;
        }
    }
}
