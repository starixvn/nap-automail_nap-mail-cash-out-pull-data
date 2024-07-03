using Fanex.COPullData.Business.Models;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;

namespace Fanex.COPullData.Business.Services
{
    public interface ExcelService
    {
        string Export(List<COModel> data, DateTime beginDate, DateTime endDate);
    }

    public class ExcelServiceImp : ExcelService
    {
        public string Export(List<COModel> data, DateTime beginDate, DateTime endDate)
        {
            using var excelPackage = new ExcelPackage(new FileInfo("Templates/Template.xlsx"));
            using var sheet = excelPackage.Workbook.Worksheets["JackTime"];

            sheet.View.ShowGridLines = false;

            AssignTitle(sheet, beginDate, endDate);
            RenderData(sheet, data);

            return Save(excelPackage, beginDate, endDate);
        }

        private static void AssignTitle(ExcelWorksheet sheet, DateTime beginDate, DateTime endDate)
        {
            sheet.Cells["B2"].Value = $"Daily JackTime Report Data From {beginDate:dd MMM, yyyy} To {endDate:dd MMM, yyyy}";
            sheet.Cells["B3"].Value = $"Generated on {DateTimeOffset.Now.DateTime:dd MMM yyy HH:mm:ss tt}";
        }

        private static void RenderData(ExcelWorksheet sheet, List<COModel> data)
        {
            sheet.Cells["B6"].LoadFromCollection(data, false);
            const int starCol = 2;
            var endCol = sheet.Dimension.End.Column;
            const int startRow = 6;
            var endRow = sheet.Dimension.End.Row;
            sheet.Columns.AutoFit(10, 50);
            for (var col = starCol; col <= endCol; col++)
            {
                sheet.Cells[startRow, col, endRow, col].StyleID = sheet.Cells[6, col].StyleID;
                sheet.Column(col).Width = sheet.Column(col).Width + 1;
            }

            sheet.Column(2).Width = 22;
            sheet.Row(5).Height = 20;

            sheet.View.FreezePanes(6, 1);
        }

        private static string Save(ExcelPackage excelPackage, DateTime beginDate, DateTime endDate)
        {
            var path = $"{AppConfigService.Config.Path}/{DateTimeOffset.Now.DateTime.Year}/{DateTimeOffset.Now.DateTime:MM}";
            Directory.CreateDirectory(path);
            var fileName = $"Daily_JackTime_Report_Data_From_{beginDate:yyyy_MM_dd}_To_{endDate:yyyy_MM_dd}.xlsx";
            path += $"/{fileName}";
            excelPackage.SaveAs(new FileInfo(path), AppConfigService.Config.Password);

            return path;
        }
    }
}