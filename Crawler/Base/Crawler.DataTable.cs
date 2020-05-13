using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crawler.Base
{
    partial class Crawler
    {
        // Dynamic colums counters (how much columns of specific type there already are)
        private int titleColumnsCount;
        private int descColumnsCount;
        private int keywordColumnsCount;
        private int headOneColumnsCount;
        private int headTwoColumnsCount;
        private void CreateDataTable()
        {
            // Initalize columns counters
            titleColumnsCount = 1;
            descColumnsCount = 1;
            keywordColumnsCount = 1;
            headOneColumnsCount = 1;
            headTwoColumnsCount = 1;

            // Column definitzion. Basa DataTable declaration.
            dt = new DataTable();
            dt.Columns.Add(ADDRESS_COL).DefaultValue = "";
            dt.Columns.Add(CONTET_TYPE_COL).DefaultValue = "";
            dt.Columns.Add(STATUS_CODE_COL).DefaultValue = "";
            dt.Columns.Add(STATUS_COL).DefaultValue = "";
            dt.Columns.Add(INDEXABILITY_COL).DefaultValue = "";
            dt.Columns.Add(INDEXABILITY_STATUS_COL).DefaultValue = "";
            dt.Columns.Add(ISINTERNAL_COL).DefaultValue = "";

            dt.Columns.Add(TITLE_COL + titleColumnsCount).DefaultValue = "";
            dt.Columns.Add(TITLE_LENGTH_COL + titleColumnsCount).DefaultValue = "";
            dt.Columns.Add(TITLE_PIXEL_WIDTH_COL + titleColumnsCount).DefaultValue = "";

            dt.Columns.Add(META_DESC_COL + descColumnsCount).DefaultValue = "";
            dt.Columns.Add(META_DESC_LENGTH_COL + descColumnsCount).DefaultValue = "";
            dt.Columns.Add(META_DESC_PIXEL_WIDTH_COL + descColumnsCount).DefaultValue = "";

            dt.Columns.Add(META_KEYWORDS_COL + keywordColumnsCount).DefaultValue = "";
            dt.Columns.Add(META_KEYWORDS_LENGTH_COL + keywordColumnsCount).DefaultValue = "";

            dt.Columns.Add(H_ONE_COL + headOneColumnsCount).DefaultValue = "";
            dt.Columns.Add(H_ONE_LENGTH_COL + headOneColumnsCount).DefaultValue = "";

            dt.Columns.Add(H_TWO_COL + headTwoColumnsCount).DefaultValue = "";
            dt.Columns.Add(H_TWO_LENGTH_COL + headTwoColumnsCount).DefaultValue = "";

            dt.Columns.Add(SIZE_COL).DefaultValue = "";

            dt.Columns.Add(OUTLINS_COL).DefaultValue = "";
            dt.Columns.Add(UNIQUE_OUTLINKS_COL).DefaultValue = "";
            dt.Columns.Add(UNIQUE_OUTLINKS_OF_TOTAL_COL).DefaultValue = "";

            dt.Columns.Add(EXTERNAL_OUTLIKNS_COL).DefaultValue = "";
            dt.Columns.Add(UNIQUE_EXTERNAL_OUTLIKNS_COL).DefaultValue = "";
            dt.Columns.Add(UNIQUE_EXTERNAL_OUTLINKS_OF_TOTAL_COL).DefaultValue = "";

            // Bind data to dataGridViews
            MainForm.BindDataTableToWszystkie(dt);
            MainForm.BindDataTableToZewnetrzne(dt);
            MainForm.BindDataTableToWewnetrzne(dt);
        }
        private void UpdateDataTable(PageFragment pf)
        {
            DataRow row = dt.NewRow();

            row[ADDRESS_COL] = pf.Address;
            row[CONTET_TYPE_COL] = pf.ContentType;
            row[STATUS_CODE_COL] = pf.StatusCode;
            row[STATUS_COL] = pf.Status;
            row[INDEXABILITY_COL] = pf.Indexability;
            row[INDEXABILITY_STATUS_COL] = pf.IndexabilityStatus;
            row[ISINTERNAL_COL] = pf.IsInternal;
            row[SIZE_COL] = GetSize(pf.IsInternal, pf.Size);
            HandleTitles(ref row, pf.Titles);
            HandleDesc(ref row, pf.MetaDescriptions);
            HandleKeywords(ref row, pf.MetaKeywords);
            HandleHeadsOne(ref row, pf.HeadingsOne);
            HandleHeadsTwo(ref row, pf.HeadingsTwo);

            if (pf.OutLinks > 0)
            {
                row[OUTLINS_COL] = pf.OutLinks;
                row[UNIQUE_OUTLINKS_COL] = pf.UniqueOutLinks;
                row[UNIQUE_OUTLINKS_OF_TOTAL_COL] = (((double)pf.UniqueOutLinks / (double)pf.OutLinks) * 100).ToString("F");
            }

            if (pf.ExternalOutLinks > 0)
            {
                row[EXTERNAL_OUTLIKNS_COL] = pf.ExternalOutLinks;
                row[UNIQUE_EXTERNAL_OUTLIKNS_COL] = pf.UniqueExternalOutLinks;
                row[UNIQUE_EXTERNAL_OUTLINKS_OF_TOTAL_COL] = (((double)pf.UniqueExternalOutLinks / (double)pf.ExternalOutLinks) * 100).ToString("F");
            }

            dt.Rows.Add(row);
        }
        private void HandleTitles(ref DataRow row, List<Title> titles)
        {
            // Handle all titles no matter how much of them there are
            var i = 1;
            foreach (Title title in titles)
            {
                if (titleColumnsCount < i)
                {
                    dt.Columns.Add(TITLE_COL + i).DefaultValue = "";
                    dt.Columns.Add(TITLE_LENGTH_COL + i).DefaultValue = "";
                    dt.Columns.Add(TITLE_PIXEL_WIDTH_COL + i).DefaultValue = "";
                    titleColumnsCount++;
                }
                row[TITLE_COL + i] = title.TitleText;
                row[TITLE_LENGTH_COL + i] = title.TitleLength;
                row[TITLE_PIXEL_WIDTH_COL + i] = title.TitlePixelWidth;
                i++;
            }
        }
        private void HandleDesc(ref DataRow row, List<MetaDescription> descs)
        {
            // Handle all meta descriptions no matter how much of them there are
            int i = 1;
            foreach (MetaDescription desc in descs)
            {
                if (descColumnsCount < i)
                {
                    dt.Columns.Add(META_DESC_COL + i).DefaultValue = "";
                    dt.Columns.Add(META_DESC_LENGTH_COL + i).DefaultValue = "";
                    dt.Columns.Add(META_DESC_PIXEL_WIDTH_COL + i).DefaultValue = "";
                    descColumnsCount++;
                }
                row[META_DESC_COL + i] = desc.MetaDescriptionText;
                row[META_DESC_LENGTH_COL + i] = desc.MetaDescriptionLength;
                row[META_DESC_PIXEL_WIDTH_COL + i] = desc.MetaDescriptionPixelWidth;
                i++;
            }
        }
        private void HandleKeywords(ref DataRow row, List<MetaKeywords> keywords)
        {
            // Handle all meta keywords no matter how much of them there are
            int i = 1;
            foreach (MetaKeywords keyword in keywords)
            {
                if (keywordColumnsCount < i)
                {
                    dt.Columns.Add(META_KEYWORDS_COL + i).DefaultValue = "";
                    dt.Columns.Add(META_KEYWORDS_LENGTH_COL + i).DefaultValue = "";
                    keywordColumnsCount++;
                }
                row[META_KEYWORDS_COL + i] = keyword.MetaKeywordsText;
                row[META_KEYWORDS_LENGTH_COL + i] = keyword.MetaKeywordsLength;
                i++;
            }
        }
        private void HandleHeadsOne(ref DataRow row, List<HeadingOne> headsOne)
        {
            // Handle all headings one no matter how much of them there are
            int i = 1;
            foreach (HeadingOne headOne in headsOne)
            {
                if (headOneColumnsCount < i)
                {
                    dt.Columns.Add(H_ONE_COL + i).DefaultValue = "";
                    dt.Columns.Add(H_ONE_LENGTH_COL + i).DefaultValue = "";
                    headOneColumnsCount++;
                }
                row[H_ONE_COL + i] = headOne.HeadingOneText;
                row[H_ONE_LENGTH_COL + i] = headOne.HeadingOneLength;
                i++;
            }
        }
        private void HandleHeadsTwo(ref DataRow row, List<HeadingTwo> headsTwo)
        {
            // Handle all headings two no matter how much of them there are
            int i = 1;
            foreach (HeadingTwo headTwo in headsTwo)
            {
                if (headTwoColumnsCount < i)
                {
                    dt.Columns.Add(H_TWO_COL + i).DefaultValue = "";
                    dt.Columns.Add(H_TWO_LENGTH_COL + i).DefaultValue = "";
                    headTwoColumnsCount++;
                }
                row[H_TWO_COL + i] = headTwo.HeadingTwoText;
                row[H_TWO_LENGTH_COL + i] = headTwo.HeadingTwoLength;
                i++;
            }
        }
        private object GetSize(bool isInternal, long size)
        {
            return isInternal ? SizeSuffix(size, 2) : String.Empty;
        }
    }
}
