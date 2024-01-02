using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ClosedXML.Excel;

namespace ExcelReportGenerate
{
    public class ExcelReportManager
    {
        public string GetCellAddress(IXLWorksheet ws, int row, int culumn)
        {
            return ws.Cell(row, culumn).Address.ToString();
        }
        public void CellFormula(IXLWorksheet ws, int row, int culumn, string Formula)
        {
            ws.Cell(row, culumn).FormulaA1 = Formula;
        }

        public void CellFormula(IXLWorksheet ws, string range, string Formula)
        {
            ws.Range(range).FormulaA1 = Formula;
            ws.Range(range).Merge();
        }
        public void CellBackgroundColor(IXLWorksheet ws, int row, int culumn, XLColor BackgroundColor)
        {
            ws.Cell(row, culumn).Style.Fill.BackgroundColor = BackgroundColor;
        }

        public void CellBackgroundColor(IXLWorksheet ws, string range, XLColor xLColor)
        {
            ws.Range(range).Style.Fill.BackgroundColor = xLColor;
            ws.Range(range).Merge();
        }
        public void PeremeterNormal(IXLWorksheet ws, string value, int fontsize, int row, int culumn, XLAlignmentHorizontalValues Alignment)
        {
            ws.Cell(row, culumn).Value = value;
            ws.Cell(row, culumn).Style.Font.FontSize = fontsize;
            ws.Cell(row, culumn).Style.Alignment.Horizontal = Alignment;
        }
        public void PeremeterHeader(IXLWorksheet ws, string value, int fontsize, int row, int culumn, XLAlignmentHorizontalValues Alignment)
        {
            ws.Cell(row, culumn).Value = value;
            ws.Cell(row, culumn).Style.Font.FontSize = fontsize;
            ws.Cell(row, culumn).Style.Font.Bold = true;
            ws.Cell(row, culumn).Style.Alignment.Horizontal = Alignment;

        }
        public void PeremeterNormal(IXLWorksheet ws, string value, int fontsize, int row, int culumn, XLColor FontColor, XLAlignmentHorizontalValues Alignment)
        {
            ws.Cell(row, culumn).Value = value;
            ws.Cell(row, culumn).Style.Font.FontSize = fontsize;
            ws.Cell(row, culumn).Style.Font.FontColor = FontColor;
            ws.Cell(row, culumn).Style.Alignment.Horizontal = Alignment;

        }
        public void PeremeterHeader(IXLWorksheet ws, string value, int fontsize, int row, int culumn, XLColor FontColor, XLAlignmentHorizontalValues Alignment)
        {
            ws.Cell(row, culumn).Value = value;
            ws.Cell(row, culumn).Style.Font.FontSize = fontsize;
            ws.Cell(row, culumn).Style.Font.Bold = true;
            ws.Cell(row, culumn).Style.Font.FontColor = FontColor;
            ws.Cell(row, culumn).Style.Alignment.Horizontal = Alignment;

        }
        public void PeremeterNormal(IXLWorksheet ws, string value, int fontsize, string range, XLAlignmentHorizontalValues Alignment)
        {
            ws.Range(range).Value = value;
            ws.Range(range).Style.Font.FontSize = fontsize;
            ws.Range(range).Merge();
            ws.Range(range).Style.Alignment.Horizontal = Alignment;
        }
        public void PeremeterHeader(IXLWorksheet ws, string value, int fontsize, string range, XLAlignmentHorizontalValues Alignment)
        {
            ws.Range(range).Value = value;
            ws.Range(range).Style.Font.FontSize = fontsize;
            ws.Range(range).Style.Font.Bold = true;
            ws.Range(range).Merge();
            ws.Range(range).Style.Alignment.Horizontal = Alignment;

        }
        public void PeremeterNormal(IXLWorksheet ws, string value, int fontsize, string range, XLColor FontColor, XLAlignmentHorizontalValues Alignment)
        {
            ws.Range(range).Value = value;
            ws.Range(range).Style.Font.FontSize = fontsize;
            ws.Range(range).Style.Font.FontColor = FontColor;
            ws.Range(range).Merge();
            ws.Range(range).Style.Alignment.Horizontal = Alignment;
        }
        public void PeremeterHeader(IXLWorksheet ws, string value, int fontsize, string range, XLColor FontColor, XLAlignmentHorizontalValues Alignment)
        {
            ws.Range(range).Value = value;
            ws.Range(range).Style.Font.FontSize = fontsize;
            ws.Range(range).Style.Font.Bold = true;
            ws.Range(range).Style.Font.FontColor = FontColor;
            ws.Range(range).Merge();
            ws.Range(range).Style.Alignment.Horizontal = Alignment;

        }



    }
}