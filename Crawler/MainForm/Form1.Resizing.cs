using System;
using System.Windows.Forms;

namespace Crawler.MainForm
{
    [System.ComponentModel.DesignerCategory("Code")]
    public class Dummy { }

    partial class Form1
    {
        /*private void TableLayoutPanel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                rowStyles = tableLayoutPanel1.RowStyles;
                columnStyles = tableLayoutPanel1.ColumnStyles;
                resizing = true;
            }
        }

        private void TableLayoutPanel1_MouseMove(object sender, MouseEventArgs e)
        {
            // Rozpoznawanie pozycji kursora. Czy kursor znajduje się nad krawędzią pola. Jesli tak to nad którą?
            if (!resizing)
            {
                float width = 0;
                float height = 0;

                // Dla wierszy
                for (var i = 0; i < rowStyles.Count; i++)
                {
                    height += rowStyles[i].Height;
                    if (e.Y > height - 3 && e.Y < height + 3)
                    {
                        rowindex = i;
                        if (rowindex == 1)
                        {
                            tableLayoutPanel1.Cursor = Cursors.HSplit;
                        }
                        break;
                    }
                    else
                    {
                        rowindex = -1;
                        tableLayoutPanel1.Cursor = Cursors.Default;
                    }
                }

                // Dla kolumn
                for (var i = 0; i < columnStyles.Count; i++)
                {
                    width += columnStyles[i].Width;
                    if (e.X > width - 3 && e.X < width + 3 && e.Y > firstRowHeight)
                    {
                        colindex = i;
                        if (rowindex == 1 && colindex == 0)
                            tableLayoutPanel1.Cursor = Cursors.Cross;
                        else if (colindex == 0)
                            tableLayoutPanel1.Cursor = Cursors.VSplit;
                        break;
                    }
                    else
                    {
                        colindex = -1;
                        if (rowindex == 0)
                            tableLayoutPanel1.Cursor = Cursors.Default;
                    }
                }
                if (rowindex == 0)
                    tableLayoutPanel1.Cursor = Cursors.Default;
            }
            else if (resizing && (colindex == 0 || rowindex == 1))
            {
                // Zmiana wymiarów względem pozycji myszki
                // położenie myszy
                float width = e.X;
                float height = e.Y;

                float nextRowHeight = tableLayoutPanel1.Height;
                float nextColumnWidth = tableLayoutPanel1.Width;

                if (colindex == 0)
                {
                    for (var i = 0; i < colindex; i++) { width -= columnStyles[i].Width; }
                    for (var i = 0; i < colindex + 1; i++) { nextColumnWidth -= columnStyles[i].Width; }
                    if (width > firstColumnMinWidth && width < tableLayoutPanel1.Width - lastColumnMinWidth)
                    {
                        columnStyles[colindex].Width = width;
                        columnStyles[colindex + 1].Width = nextColumnWidth;
                    }
                }

                if (rowindex == 1)
                {
                    for (var i = 0; i < rowindex; i++) { height -= rowStyles[i].Height; }
                    for (var i = 0; i < rowindex + 1; i++) { nextRowHeight -= rowStyles[i].Height; }
                    if (height > middleRowMinHeight && height < tableLayoutPanel1.Height - firstRowHeight - lastRowMinHeight)
                    {
                        rowStyles[rowindex].Height = height;
                        rowStyles[rowindex + 1].Height = nextRowHeight;
                    }
                }

                tableLayoutPanel1.Update();
            }
        }

        private void TableLayoutPanel1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            resizing = false;

            // Weryfikacja wysokości. W razie problemów następuje korekta.
            if (rowindex == 1)
            {
                var height = 0;
                for (var i = 0; i < rowStyles.Count; i++) { height += (int)rowStyles[i].Height; }
                if (e.Y < 0 || e.Y > tableLayoutPanel1.Height)
                {
                    if (height > tableLayoutPanel1.Height && rowStyles[rowindex + 1].Height < rowStyles[rowindex].Height)
                    {
                        rowStyles[rowindex].Height = tableLayoutPanel1.Height - firstRowHeight - lastRowMinHeight;
                        rowStyles[rowindex + 1].Height = lastRowMinHeight;
                    }
                    else if (height < tableLayoutPanel1.Height && rowStyles[rowindex + 1].Height > rowStyles[rowindex].Height)
                    {
                        rowStyles[rowindex].Height = middleRowMinHeight;
                        rowStyles[rowindex + 1].Height = tableLayoutPanel1.Height - firstRowHeight - middleRowMinHeight;
                    }
                }
                else if (height > tableLayoutPanel1.Height)
                {
                    if ((height - tableLayoutPanel1.Height) % 2 != 0)
                    {
                        if (rowStyles[rowindex].Height > middleRowMinHeight)
                        {
                            rowStyles[rowindex].Height -= 1;
                            height--;
                        }
                    }

                    var difference = (height - tableLayoutPanel1.Height) / 2;
                    if (rowStyles[rowindex].Height - difference > middleRowMinHeight && rowStyles[rowindex + 1].Height - difference > lastRowMinHeight)
                    {
                        rowStyles[rowindex].Height -= difference;
                        rowStyles[rowindex + 1].Height -= difference;
                    }
                    else
                    {
                        while (height > tableLayoutPanel1.Height)
                        {
                            if (rowStyles[rowindex].Height > middleRowMinHeight)
                            {
                                rowStyles[rowindex].Height -= 1;
                                height--;
                            }

                            if (height == tableLayoutPanel1.Height) break;

                            if (rowStyles[rowindex + 1].Height > lastRowMinHeight)
                            {
                                rowStyles[rowindex + 1].Height -= 1;
                                height--;
                            }
                        }
                    }
                }
            }

            // Weryfikacja szerokości. W razie problemów następuje korekta.
            if (colindex == 0)
            {
                var width = 0;
                for (var i = 0; i < columnStyles.Count; i++) { width += (int)columnStyles[i].Width; }

                if (e.X < 0 || e.X > tableLayoutPanel1.Width)
                {
                    if (width > tableLayoutPanel1.Width && columnStyles[colindex + 1].Width < columnStyles[colindex].Width)
                    {
                        columnStyles[colindex].Width = tableLayoutPanel1.Width - lastColumnMinWidth;
                        columnStyles[colindex + 1].Width = lastColumnMinWidth;
                    }
                    else if (width < tableLayoutPanel1.Width && columnStyles[colindex + 1].Width > columnStyles[colindex].Width)
                    {
                        columnStyles[colindex].Width = firstColumnMinWidth; ;
                        columnStyles[colindex + 1].Width = tableLayoutPanel1.Width - firstColumnMinWidth;
                    }
                }
                else if (width > tableLayoutPanel1.Width)
                {
                    if ((int)(width - tableLayoutPanel1.Width) % 2 != 0)
                    {
                        if (columnStyles[colindex].Width > firstColumnMinWidth)
                        {
                            columnStyles[colindex].Width -= 1;
                            width--;
                        }
                    }

                    var difference = (width - tableLayoutPanel1.Width) / 2;
                    if (columnStyles[colindex].Width - difference > firstColumnMinWidth && columnStyles[colindex + 1].Width - difference > lastColumnMinWidth)
                    {
                        columnStyles[colindex].Width -= difference;
                        columnStyles[colindex + 1].Width -= difference;
                    }
                    else
                    {
                        while (width > tableLayoutPanel1.Width)
                        {
                            if (columnStyles[colindex].Width > firstColumnMinWidth)
                            {
                                columnStyles[colindex].Width -= 1;
                                width--;
                            }

                            if (width == tableLayoutPanel1.Width) break;

                            if (columnStyles[colindex + 1].Width > lastColumnMinWidth)
                            {
                                columnStyles[colindex + 1].Width -= 1;
                                width--;
                            }
                        }
                    }
                }
            }

            tableLayoutPanel1.Cursor = Cursors.Default;
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            // Weryfikacja wysokości. W razie problemów następuje korekta.
            rowindex = 1;
            var height = 0;
            for (var i = 0; i < rowStyles.Count; i++) { height += (int)rowStyles[i].Height; }

            if (height > tableLayoutPanel1.Height)
            {
                if ((int)(height - tableLayoutPanel1.Height) % 2 != 0)
                {
                    if (rowStyles[rowindex].Height > middleRowMinHeight)
                    {
                        rowStyles[rowindex].Height -= 1;
                        height--;
                    }
                }

                var difference = ((float)height - tableLayoutPanel1.Height) / 2;
                if (rowStyles[rowindex].Height - difference > middleRowMinHeight && rowStyles[rowindex + 1].Height - difference > lastRowMinHeight)
                {
                    rowStyles[rowindex].Height -= difference;
                    rowStyles[rowindex + 1].Height -= difference;
                }
                else
                {
                    while (height + 9 > tableLayoutPanel1.Height)
                    {
                        if (rowStyles[rowindex].Height > middleRowMinHeight)
                        {
                            rowStyles[rowindex].Height -= 10;
                            height -= 10;
                        }

                        if (height == tableLayoutPanel1.Height) break;

                        if (rowStyles[rowindex + 1].Height > lastRowMinHeight)
                        {
                            rowStyles[rowindex + 1].Height -= 10;
                            height -= 10;
                        }
                    }
                }
            }

            // Weryfikacja szerokości. W razie problemów następuje korekta.
            colindex = 0;
            var width = 0;
            for (var i = 0; i < columnStyles.Count; i++)
            {
                width += (int)columnStyles[i].Width;
            }

            if (width > tableLayoutPanel1.Width)
            {
                if ((width - tableLayoutPanel1.Width) % 2 != 0)
                {
                    if (columnStyles[colindex].Width > firstColumnMinWidth)
                    {
                        columnStyles[colindex].Width -= 1;
                        width--;
                    }
                }

                var difference = (width - tableLayoutPanel1.Width) / 2;
                if (columnStyles[colindex].Width - difference > firstColumnMinWidth && columnStyles[colindex + 1].Width - difference > lastColumnMinWidth)
                {
                    columnStyles[colindex].Width -= difference;
                    columnStyles[colindex + 1].Width -= difference;
                }
                else
                {
                    while (width + 9 > tableLayoutPanel1.Width)
                    {
                        if (columnStyles[colindex].Width > firstColumnMinWidth)
                        {
                            columnStyles[colindex].Width -= 10;
                            width -= 10;
                        }

                        if (width == tableLayoutPanel1.Width) break;

                        if (columnStyles[colindex + 1].Width > lastColumnMinWidth)
                        {
                            columnStyles[colindex + 1].Width -= 10;
                            width -= 10;
                        }
                    }
                }
            }
        }*/
    }
}
